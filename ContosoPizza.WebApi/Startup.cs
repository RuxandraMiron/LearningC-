using ContosoPizza.WebApi.Interfaces;
using ContosoPizza.WebApi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ContosoPizza.WebApi;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices( IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen( options => {
                options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "ContosoPizza Learning Project",
                        Description = "An ASP.NET Core Web API for managing pizzas",
                    });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            // services.Add(new ServiceDescriptor(typeof(IPizzaService),new PizzaService()));
            services.AddSingleton<IPizzaService, PizzaService>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseReDoc(c =>
                {
                    c.DocumentTitle = "REDOC API Documentation";
                    c.SpecUrl = "/swagger/v1/swagger.json";
                });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapGet("/", () => "Hello World!");
            });
            
        }
    }
