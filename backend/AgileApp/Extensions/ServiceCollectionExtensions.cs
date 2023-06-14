using AgileApp.Services;

namespace AgileApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddSingleton<WebSocketConnectionManager>();
            return services;
        }
    }
}