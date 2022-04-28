using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ct.Models

    {
        public class WeatherInfoModel
        {
            public class RootWeather
            {
                [JsonProperty("weather")]

                //private Weather  weather1
                public WeatherInfoMaster[] WeatherView { get; set; }


                [JsonProperty("main")]
                public Main Main { get; set; }

                [JsonProperty("wind")]
                public Wind Wind { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

            }
            public class Main
            {
                [JsonProperty("temp")]
                private double temp;
                public int Temp
                {
                    get
                    {
                        return Convert.ToInt16(temp - 273.15);

                    }
                    set
                    {
                    }
                }

                [JsonProperty("pressure")]
                public long Pressure { get; set; }

                [JsonProperty("humidity")]
                public long Humidity { get; set; }
            }

            public partial class Wind
            {
                [JsonProperty("speed")]
                public double Speed { get; set; }
            }

        }
    }
}
