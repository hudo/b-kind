using System;
using BKind.Web.Core;
using BKind.Web.Infrastructure.Persistance;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using System.IO;
using System.Linq;
using BKind.Web.Infrastructure;
using BKind.Web.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;

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
                
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        private IHostingEnvironment HostingEnvironment;
        private IContainer Container;

        public void ConfigureServices2(IServiceCollection services)
        {
            services.AddDbContext<StoriesDbContext>(o => o.UseNpgsql("User ID=user;Password=user;Host=localhost;Port=5432;Database=bkind;Pooling=true;"));
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddControllersAsServices();

            services.AddDbContext<StoriesDbContext>(o => 
                o.UseNpgsql("User ID=user;Password=user;Host=localhost;Port=5432;Database=bkind;Pooling=true;"));

            services.AddDistributedMemoryCache();
            services.AddSession();

            var container = new Container(c =>
            {
                c.Scan(scanner =>
                {
                    scanner.AssemblyContainingType<Startup>();
                    scanner.AssemblyContainingType<IMediator>();
                    scanner.WithDefaultConventions();
                    scanner.With(new AddRequestHandlersWithGenericParameters());
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

                c.For<IUnitOfWork>().Use<EfUnitOfWork>().ContainerScoped();
                c.For<DbContext>().Use<StoriesDbContext>().ContainerScoped();

                c.For<IHttpContextAccessor>().Use<HttpContextAccessor>().Singleton();
            });

            container.Populate(services);

            Container = container;

            return container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddProvider(new ConsoleLoggerProvider());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

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

            ConfigureEFLogger();

            Task.Run(() => 
            { 
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<StoriesDbContext>().EnsureDataSeed();
                }
            });
        }

        private void ConfigureEFLogger()
        {
            using (var db = Container.GetInstance<StoriesDbContext>())
            {
                var serviceProvider = db.GetInfrastructure<IServiceProvider>();
                var dbloggerFactory = serviceProvider.GetService<ILoggerFactory>();
                dbloggerFactory.AddProvider(new ConsoleLoggerProvider());
            }
        }
    }
}
