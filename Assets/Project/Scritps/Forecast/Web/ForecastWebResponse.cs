using System;
using System.Collections.Generic;

namespace CifkorApp.Forecast.Web
{
    public class ForecastWebResponse
    {
        public ForecastProperties Properties;
    }

    public class ForecastProperties
    {
        public string Units;
        public string ForecastGenerator;
        public List<ForecastPeriod> Periods;
    }

    public class ForecastPeriod
    {
        public long Number;
        public string Name;
        public DateTimeOffset StartTime;
        public DateTimeOffset EndTime;
        public bool IsDaytime;
        public int Temperature;
        public string TemperatureUnit;
        public string TemperatureTrend;
        public string WindSpeed;
        public string WindDirection;
        public Uri Icon;
        public string ShortForecast;
        public string DetailedForecast;
    }
}
