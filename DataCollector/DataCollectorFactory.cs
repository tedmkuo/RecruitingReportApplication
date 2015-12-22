using System;

namespace DataCollector
{
    public class DataCollectorFactory
    {
        public static IDataCollector CreateDataCollector(string dataProvider)
        {
            if (string.IsNullOrEmpty(dataProvider))
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            switch (dataProvider.ToLower())
            {
                case "greenhouse":
                    return new GreenhouseDataCollector();
                default:
                    return null;
            }
        }
    }
}
