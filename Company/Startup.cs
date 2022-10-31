using Companys.Services;
using Companys.Utils;
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

namespace Companys
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company", Version = "v1" });
            });
            services.Configure<DataBaseSettings>(Configuration.GetSection(nameof(DataBaseSettings))); 
            IServiceCollection serviceCollection = services.AddSingleton<IDataBaseSettings>(sp => sp.GetRequiredService<IOptions<DataBaseSettings>>().Value);
            services.AddSingleton<CompanyServices>();
            services.AddSingleton<CompanyGarbageServices>();
            services.AddSingleton<CompanyBlockedServices>();
            services.AddSingleton<CompanyBlockedGarbageServices>();
            services.Configure<AddressServicesSettings>(Configuration.GetSection(nameof(AddressServicesSettings)));
            services.AddSingleton<IAddressServicesSettings>(sp => sp.GetRequiredService<IOptions<AddressServicesSettings>>().Value);
            services.AddSingleton<AddressServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company v1"));
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
