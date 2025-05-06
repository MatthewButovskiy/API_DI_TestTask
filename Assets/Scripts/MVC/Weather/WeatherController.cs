using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Scripts.Core.Networking;
using Scripts.Services.Weather;
using Zenject;

namespace Scripts.MVC.Weather
{
    public class WeatherController : IInitializable, System.IDisposable
    {
        private readonly IWeatherService _service;
        private readonly WeatherView     _view;
        private readonly RequestQueue    _queue;
        
        
        private CancellationTokenSource _cts;
        private bool _active;

        [Inject]
        public WeatherController(IWeatherService service, WeatherView view, RequestQueue queue)
        {
            _service = service;
            _view    = view;
            _queue   = queue;
        }

        public void Initialize()
        {
            _active = false;
            _cts = new CancellationTokenSource();
        }

        public void SetActive(bool state)
        {
            if (_active == state)
                return;

            _active = state;
            if (state)
            {
                _cts = new CancellationTokenSource();
                ScheduleLoop().Forget();
            }
            else
            {
                _cts.Cancel();
                _queue.CancelCurrentAndClear(r => r.Type == RequestType.Weather);
            }
        }

        private async UniTaskVoid ScheduleLoop()
        {
            while (_active)
            {
                _queue.Enqueue(RequestType.Weather, async token =>
                {
                    var data = await _service.GetTodayAsync(token);
                    var req  = UnityWebRequestTexture.GetTexture(data.IconUrl);
                    await req.SendWebRequest().WithCancellation(token);
                    var tex    = DownloadHandlerTexture.GetContent(req);
                    var sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), Vector2.one*0.5f);
                    await UniTask.SwitchToMainThread();
                    _view.SetData(sprite, data.TempF);
                });
                await UniTask.Delay(5000, cancellationToken: _cts.Token);
            }
        }


        public void Dispose() => _cts.Cancel();
    }
}