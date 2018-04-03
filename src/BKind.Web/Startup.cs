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
using BKind.Web.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BKind.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IContainer Container;

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
                {
                    opt.Filters.Add(typeof(DbContextTransactionFilter));
                })
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddFeatureFolders()
                .AddAreaFeatureFolders()
                .AddControllersAsServices();

            services.AddDbContext<StoriesDbContext>(o => o.UseNpgsql(Configuration.GetConnectionString("BkindPsql")));

            services.AddMemoryCache();
            services.AddSession();

            services.AddLogging();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/account/login";
                    options.LoginPath = "/account/login";
                    options.LogoutPath = "/account/signout";
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("isAdmin", policy => policy.RequireClaim("isAdmin", "True"));
            });

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

                c.For<IUnitOfWork>().Use<EfUnitOfWork>();
                c.For<DbContext>().Use<StoriesDbContext>();

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

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "page",
                    template: "page/{*slug}",
                    defaults: new {controller = "Pages", action = "Index"});

                routes.MapRoute(
                    name: "news",
                    template: "news/{*slug}",
                    defaults: new { controller = "News", action = "Index" });

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=home}/{action=index}/{id?}");
                    
                routes.MapRoute(
                        name: "default",
                        template: "{controller=home}/{action=index}/{id?}");
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
