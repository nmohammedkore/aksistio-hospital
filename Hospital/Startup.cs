using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Hospital.BaseClasses.Intefaces; 
using Hospital.DataAccess.AzureSql;


namespace Hospital
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
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
            services.AddScoped<IHospitalRepo>(sh => new AzureSqlHospital(sqlConfigString));
            services.AddHealthChecks()
                    .AddCheck<ExHealthCheck>("ex_health_check");
            services.AddHostedService<Background>();
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
