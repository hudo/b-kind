using System;
using BKind.Web.Core;
using BKind.Web.Infrastructure.Persistance;
using BKind.Web.Infrastructure.Persistance.StandardHandlers;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using StructureMap;
using StructureMap.Pipeline;
using System.IO;
using System.Linq;
using System.Reflection;
using BKind.Web.Core.StandardQueries;
using BKind.Web.Model;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace BKind.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddDbContext<StoriesDbContext>();

            var container = new Container(c =>
            {
                c.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<Startup>();
                    scanner.AssemblyContainingType<IMediator>();
                    scanner.WithDefaultConventions();
                    //scanner.With(new AddRequestHandlersWithGenericParametersToRegistry());
                    scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICancellableAsyncRequestHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                    scanner.ConnectImplementationsToTypesClosing(typeof(ICancellableAsyncNotificationHandler<>));
                });
                c.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                c.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                c.For<TextWriter>().Use(Console.Out);
                c.For<IMediator>().Use<Mediator>();

                c.For<IDatabase>().Use<InMemoryFakeDatabase>();
                c.For<IUnitOfWork>().Use<InMemoryUnitOfWork>();
                c.For<DbContext>().Use<StoriesDbContext>().ContainerScoped();
                c.For<IHttpContextAccessor>().Use<HttpContextAccessor>().Singleton();
            });

            Console.Write(container.WhatDoIHave());

            container.Populate(services);

            return container.GetInstance<IServiceProvider>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = Application.AuthScheme,
                LoginPath = "/account/login",
                AccessDeniedPath = "/account/login",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
