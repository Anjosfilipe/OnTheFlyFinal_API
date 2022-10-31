using Flights.Services;
using Flights.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flights
{
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight", Version = "v1" });
            });
            services.Configure<DataBaseSettings>(Configuration.GetSection(nameof(DataBaseSettings)));
            services.AddSingleton<IDataBaseSettings>(sp => sp.GetRequiredService<IOptions<DataBaseSettings>>().Value);
            services.AddSingleton<FlightServices>();
            services.Configure<AirportServicesSettings>(Configuration.GetSection(nameof(AirportServicesSettings)));
            services.AddSingleton<IAirportServicesSettings>(sp => sp.GetRequiredService<IOptions<AirportServicesSettings>>().Value);
            services.AddSingleton<AirportServices>();
            services.Configure<CompanyServicesSettings>(Configuration.GetSection(nameof(CompanyServicesSettings)));
            services.AddSingleton<ICompanyServicesSettings>(sp => sp.GetRequiredService<IOptions<CompanyServicesSettings>>().Value);
            services.AddSingleton<CompanyServices>();
            services.Configure<AircraftServicesSettings>(Configuration.GetSection(nameof(AircraftServicesSettings)));
            services.AddSingleton<IAircraftServicesSettings>(sp => sp.GetRequiredService<IOptions<AircraftServicesSettings>>().Value);
            services.AddSingleton<AircraftServices>();
            services.Configure<CompanyServicesSettings>(Configuration.GetSection(nameof(CompanyServicesSettings)));
            services.AddSingleton<ICompanyServicesSettings>(sp => sp.GetRequiredService<IOptions<CompanyServicesSettings>>().Value);
            services.AddSingleton<CompanyServices>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
