using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Parky.api.Data;
using Parky.api.Repository;
using Parky.api.Repository.Interfaces;

namespace Parky.api
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
            //add the sql db context (using Entity framework here, so two nuget packages installed to allow for this to be used, EntityFrameworkCore and then the SqlServer extensions for Entity Framework)
            //the connection string can be found in the appsettings.json file
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("ParkyOpenAPISpec", 
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title="Parky API",
                        Version="1.0",
                        Description="API Tutorial",
                        Contact = new OpenApiContact()
                        {
                            Email= "auticus.daerk@gmail.com",
                            Name="Auticus Daerk",
                            Url = new Uri("https://www.github.com/auticus")
                        }
                        //can get even more detailed here with extensions to add logos, etc... license information
                    });
                //set up the xml documents for better readability in swagger
                var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //this also matches the file name set in the BUILD project setting xml document field
                var fullPath = Path.Combine(AppContext.BaseDirectory, file);
                options.IncludeXmlComments(fullPath);
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //to view swagger documentation go to localhost:{port}/swagger/ParkyOpenAPISpec/swagger.json
            app.UseSwagger(); //use after UseHttpRedirection - prevents anything from messing with the Http, use this to enable Swagger

            //the swagger UI - localhost:{port}/swagger/index.html
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API");
                options.RoutePrefix = "";
            });
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
