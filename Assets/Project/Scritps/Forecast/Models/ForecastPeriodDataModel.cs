using System;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace CifkorApp.Forecast.Models
{
    public class ForecastPeriodDataModel
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

        [JsonProperty("Icon")]
        public string IconUrl;

        [JsonIgnore]
        public Sprite IconSprite;

        public string ShortForecast;
        public string DetailedForecast;

    }
}
