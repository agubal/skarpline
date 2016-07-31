using StructureMap;

namespace Skarpline.Dependencies
{
    /// <summary>
    /// Static class which represents Inversion of Control container
    /// </summary>
    public static class IoC
    {
        private static IContainer _container;

        public static IContainer Container => _container ?? (_container = Initialize());

        /// <summary>
        /// Initializes Inversion of Control container
        /// </summary>
        /// <returns>IoC Container instance</returns>
        public static IContainer Initialize()
        {
            var container = new Container(c => c.AddRegistry<IocRegistry>());
            IocRegistry.RegisterMappings(container);
            return container;
        }
    }
}
