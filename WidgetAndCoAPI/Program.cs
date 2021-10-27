using Infrastructure;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Infrastructure.Repository;
using Infrastructure.Service;

namespace WidgetAndCoAPI
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(Configure)
                .Build();
            host.Run();
        }

        static void Configure(HostBuilderContext Builder, IServiceCollection Services)
        {
            Services.AddDbContext<APIContext>(option =>
            {
                option.UseCosmos(
                    //Builder.Configuration["CosmosDb:Account"],
                    //Builder.Configuration["CosmosDb:Key"],
                    //Builder.Configuration["Cosmos:DatabaseName"]
                    "https://widgetandcostore.documents.azure.com:443/",
                    "f13ROF7tkhpX5MgtRlVHb7PkxfO1ooe5pjj8aJGLedvBtrOP2Dt58m4exMhUsSwyDOLV5jBv2jpx5DJgHkANgg==",
                    "WidgetAndCoDB"
                );
            });

            //repository registration
            Services.AddTransient(typeof(ICosmosReadRepository<>), typeof(CosmosReadRepository<>));
            Services.AddTransient(typeof(ICosmosWriteRepository<>), typeof(CosmosWriteRepository<>));

            //service registration
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IReviewService, ReviewService>();
        }

    }
}