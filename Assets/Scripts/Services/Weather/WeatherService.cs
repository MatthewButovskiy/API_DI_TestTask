namespace Scripts.Services.Weather
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Networking;
    using Newtonsoft.Json.Linq;

    public interface IWeatherService
    {
        UniTask<WeatherModel> GetTodayAsync(CancellationToken token);
    }

    public struct WeatherModel
    {
        public string IconUrl;
        public int    TempF;
    }

    public sealed class WeatherService : IWeatherService
    {
        private const string ENDPOINT = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public async UniTask<WeatherModel> GetTodayAsync(CancellationToken token)
        {
            using var req = UnityWebRequest.Get(ENDPOINT);
            await req.SendWebRequest().WithCancellation(token);

            var json  = JObject.Parse(req.downloadHandler.text);
            var first = json["properties"]?["periods"]?[0];
            return new WeatherModel {
                IconUrl = first?["icon"]?.ToString(),
                TempF   = first?["temperature"]?.Value<int>() ?? 0
            };
        }
    }
}