using CifkorApp.Forecast.Models;
using System.Collections.Generic;

namespace CifkorApp.Forecast.Web
{
    public class GetForecastWebResponse
    {
        public class PropertiesData
        {
            public string Units;
            public string ForecastGenerator;
            public List<ForecastPeriodDataModel> Periods;
        }

        public PropertiesData Properties;
    }
}
