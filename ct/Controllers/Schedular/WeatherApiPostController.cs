using ct.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CTGroupAppWebApplication.Controllers.Scheduler
{
    public class WeatherApiPostController : ApiController
    {
        [HttpGet]
        async public Task<object> WeatherApi()
        {
            try
            {
                var url = "http://api.openweathermap.org/data/2.5/weather?q=Ludhiana&appid=ba1867e55d9b844a450630ce75240904";

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla");

                HttpResponseMessage res = await client.GetAsync(url);

                if (res.IsSuccessStatusCode)
                {
                    string jsonstring = await res.Content.ReadAsStringAsync();
                    WeatherInfoModel.RootWeather data = JsonConvert.DeserializeObject<WeatherInfoModel.RootWeather>(jsonstring);

                    WeatherInfo weather = new WeatherInfo
                    {
                        Humidity = data.Main.Humidity.ToString(),
                        Name = data.Name,
                        Pressure = data.Main.Pressure.ToString(),
                        Speed = data.Wind.Speed.ToString(),
                        temp = data.Main.Temp.ToString()
                    };

                    int weatherid;

                    //using (CTUmsEntitiesApp entities = new CTUmsEntitiesApp())
                    //{
                        //entities.Add(weather);
                        //entities.SaveChanges();

                     //   weatherid = weather.Id;

                       // int len = data.WeatherView.Length;

                       // for (int i = 0; i < len; i++)
                      //  {
                     //       data.WeatherView[i].WeatherId = weatherid;
                     //   }

                     //   entities.WeatherInfoMasters.AddRange(data.WeatherView);
                     //   entities.SaveChanges();

                      //  return Request.CreateResponse(HttpStatusCode.Created);
                   // }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed,ex.ToString());
            }

            return Request.CreateResponse(HttpStatusCode.ExpectationFailed);
        }
    }

    
}
