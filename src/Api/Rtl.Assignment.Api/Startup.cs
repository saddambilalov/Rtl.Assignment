namespace Rtl.Assignment.Api
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using AutoMapper;
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Rtl.Assignment.Api.Abstractions.Response;
    using Rtl.Assignment.Api.Profiles;
    using Rtl.Assignment.Api.Query;
    using Rtl.Assignment.Api.Query.Handlers;
    using Rtl.Assignment.Domain.Repositories;
    using Rtl.Assignment.Infrastructure.DataPersistence.Configuration;
    using Rtl.Assignment.Infrastructure.Repositories;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(mapperConfig =>
            {
                mapperConfig.AddProfile<ShowProfile>();
            });

            this.RegisterMongoDb(services);

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddSingleton<IRequestHandler<ShowWithCastQuery, IEnumerable<ShowWithCastResource>>, ShowWithCastQueryHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RTL Assignment Api",
                    Description = "Documentation for RTL Assignment Api",
                });
            });
        }

        private void RegisterMongoDb(IServiceCollection services)
        {
            services.Configure<RtlDatabaseSettings>(
                this.Configuration.GetSection(nameof(RtlDatabaseSettings)));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<RtlDatabaseSettings>>()?.Value ??
                throw new ArgumentNullException(nameof(RtlDatabaseSettings)));
            services.AddSingleton<IShowWithCastRepository, ShowWithCastRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RTL Assignment Api");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
