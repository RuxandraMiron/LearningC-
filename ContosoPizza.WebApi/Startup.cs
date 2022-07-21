using ContosoPizza.WebApi.Interfaces;
using ContosoPizza.WebApi.Services;
using Microsoft.Extensions.Logging;

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
            
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapGet("/", () => "Hello World!");
            });
            
        }
    }
