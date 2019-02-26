using System;
using Newtonsoft.Json.Linq;

namespace MyBackgroundChanger
{
    internal class TimeChecker
    {
        private readonly ApiCall _api = new ApiCall();
        private const string Url = "https://api.sunrise-sunset.org/json?lat=51.571915&lng=4.768323&date=today";

        public DateTime LocalTime()
        {
            return DateTime.Now;
        }

        public DateTime GetSunriseTime()
        {
            var response = _api.Get(Url);
            var jsonObject = JObject.Parse(response);

            return DateTime.Parse(jsonObject["results"]["sunrise"].ToString());
        }

        public DateTime GetSolarNoonTime()
        {
            var response = _api.Get(Url);
            var jsonObject = JObject.Parse(response);

            return DateTime.Parse(jsonObject["results"]["solar_noon"].ToString());
        }

        public DateTime GetTwilightEnd()
        {
            var response = _api.Get(Url);
            var jsonObject = JObject.Parse(response);

            return DateTime.Parse(jsonObject["results"]["astronomical_twilight_end"].ToString());
        }
    }
}
