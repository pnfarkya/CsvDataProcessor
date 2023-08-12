using CsvDataProcessor.DataProcessor;
using Unity;
using Unity.Lifetime;

namespace CsvDataProcessor.Common
{
    public class AppComposition
    {
        public static UnityContainer Container { get; set; }

        public static void Init()
        {
            Container = new UnityContainer();
            Container.RegisterType<IPersonFileReader, PersonFileReader>(new ContainerControlledLifetimeManager());
        }

        public AppComposition()
        {
            Init();
        }
    }
}
