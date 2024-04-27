namespace SimpleChats.API.Extensions
{
    using System.Reflection;

    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided");
            }

            Type[] servicesTypes = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();

            foreach(Type imType in servicesTypes)
            {
                Type? interfaceType = imType.GetInterface($"I{imType.Name}");

                if (interfaceType == null)
                {
                    throw new InvalidOperationException($"No interface is provided for the service with name: {imType.Name}");
                }

                services.AddScoped(interfaceType, imType);
            }
        }
    }
}
