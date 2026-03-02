using Autofac;

namespace Business
{
    public class BusinessModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(UserBusiness).Assembly)
                   .Where(t => t.Name.EndsWith("Business"))
                   .AsImplementedInterfaces();
        }
    }
}
