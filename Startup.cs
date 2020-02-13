using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(opt =>
               opt.UseInMemoryDatabase("TodoList"));
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var builder = new ODataConventionModelBuilder(app.ApplicationServices);

            builder.EntitySet<TodoItem>("TodoItems");

            app.UseMvc(routeBuilder =>
            {
                // and this line to enable OData query option, for example $filter
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();

                routeBuilder.MapODataServiceRoute("ODataRoute", "api", builder.GetEdmModel());

                // uncomment the following line to Work-around for #1175 in beta1
                routeBuilder.EnableDependencyInjection();
            });
            // app.UseMvc(routeBuilder =>
            // {
            //     routeBuilder.EnableDependencyInjection();
            //     routeBuilder.Expand().Select().Count().OrderBy().Filter();
            // });

        }
    }
}
