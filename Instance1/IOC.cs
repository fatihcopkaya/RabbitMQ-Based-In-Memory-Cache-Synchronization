using Instance1.Reciver;
using RabbitMQ.Client;

namespace Instance1
{
    public static class IOC
    {
        public static IServiceCollection ServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                    Uri = new Uri(configuration.GetConnectionString("RabbitMQ"))
                };
            });

            services.AddHostedService<Consumer.CacheConsumer>();
            services.AddMemoryCache();
            services.AddScoped<IReciver, Reciver.Reciver>();

            return services;
        }
    }
}
