using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace forecastie
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            updateForecastAsync();

            Debug.WriteLine("test");
        }

        public async Task updateForecastAsync()
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();



            Uri requestUri = new Uri("https://api.openweathermap.org/data/2.5/weather?q=moscow&appid=66803bb34c2a6e2cfe7ad7e2beb619ec&units=metric");

            //Send the GET request asynchronously and retrieve the response as a string.
            Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();


                var rootObject = JsonObject.Parse(httpResponse.Content.ToString());


                // имя поля в Json + "Json" 
                var mainJson = JsonObject.Parse(rootObject["main"].ToString());
                var windJson = JsonObject.Parse(rootObject["wind"].ToString());
                //var weatherJson = JsonObject.Parse(rootObject["weather"].ToString());

                
                var sysJson = JsonObject.Parse(rootObject["sys"].ToString());



                temp.Text = mainJson["temp"].ToString() + " °C"; // температура
                pressure.Text = mainJson["pressure"].ToString() + " мм рт.ст."; // давление
                humidity.Text = mainJson["humidity"].ToString() + " %"; // влажность

                wind.Text = windJson["speed"].ToString() + " м/с"; //ветер

                // восход и заход солнца
                //             unixtime от 1970 года           местному времени сервера  в секунды              строку ответа в число            в местное время клиента и оставляем только часы и минуты   
                sunrise.Text = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(sysJson["sunrise"].ToString())).ToLocalTime().ToString("HH:mm");
                sunset.Text = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Convert.ToDouble(sysJson["sunset"].ToString())).ToLocalTime().ToString("HH:mm");


            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
        }
    }
}
