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

using Microsoft.OpenApi.Models;

namespace Sunaphe.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers()
                    .AddJsonOptions(options => {
                        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    });

            services.AddSwaggerGen(setup => {
                setup.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "Sunaphe API",
                    Description = "Tells you about the stars and stuff",
                    License = new OpenApiLicense {
                        Name = "Use under GPLv2",
                        Url = new Uri("https://opensource.org/licenses/gpl-2.0.php")
                    },
                    Contact = new OpenApiContact {
                        Email = "mcintosh303@gmail.com",
                        Name = "Jo McIntosh",
                        Url = new Uri("https://github.com/user/JoMcIntosh")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sunaphe API v1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
