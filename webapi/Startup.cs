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
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Hospital.BaseClasses.Intefaces;
using Hospital.BaseClasses.Models;
using Hospital.DataAccess.SqlLite;
using Hospital.DataAccess.AzureSql;


namespace Hospital
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                                .AddEnvironmentVariables()
                                .Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddScoped<IHospital>(sh => new SqlLiteHospital("Connectionstring"));
            string sqlConfigString = Configuration.GetValue<string>("SqlConnectionString");            
            //services.AddScoped<IHospital>(sh => new SqlLiteHospital(sqlConfigString));
            services.AddScoped<IHospital>(sh => new AzHospital(sqlConfigString));
            services.AddHealthChecks()
                    .AddCheck<ExHealthCheck>("ex_health_check");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/Health");
            });
        }
    }
}
