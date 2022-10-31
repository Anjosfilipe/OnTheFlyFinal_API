using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Saleses.Services;
using Saleses.Utils;

namespace Saleses
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Saleses", Version = "v1" });
            });
            services.Configure<DataBaseSettings>(Configuration.GetSection(nameof(DataBaseSettings)));
            services.AddSingleton<IDataBaseSettings>(sp => sp.GetRequiredService<IOptions<DataBaseSettings>>().Value);
            services.AddSingleton<SalesServices>();

            services.Configure<PassengerServicesSettings>(Configuration.GetSection(nameof(PassengerServicesSettings)));
            services.AddSingleton<IPassengerServicesSettings>(sp => sp.GetRequiredService<IOptions<PassengerServicesSettings>>().Value);
            services.AddSingleton<PassengerServices>();

            services.Configure<FlightServiceSettings>(Configuration.GetSection(nameof(FlightServiceSettings)));
            services.AddSingleton<IFlightServicesSettings>(sp => sp.GetRequiredService<IOptions<FlightServiceSettings>>().Value);
            services.AddSingleton<FlightServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Saleses v1"));
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
