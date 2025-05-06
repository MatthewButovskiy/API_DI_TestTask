// Scripts/MVC/Dogs/DogsController.cs
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Scripts.Core.Networking;
using Scripts.Services.Dogs;
using Zenject;

namespace Scripts.MVC.Dogs
{
    public class DogsController
    {
        private readonly IDogService  _service;
        private readonly DogsView     _view;
        private readonly RequestQueue _queue;

        [Inject]
        public DogsController(IDogService service, DogsView view, RequestQueue queue)
        {
            _service = service;
            _view    = view;
            _queue   = queue;
        }

        public void Activate()
        {
            _view.ShowLoading(true);
            _queue.Enqueue(RequestType.BreedsList, async token =>
            {
                try
                {
                    var list = await _service.GetBreedsAsync(token);
                    
                    await UniTask.SwitchToMainThread();

                    _view.Clear();
                    foreach (var b in list)
                    {
                        var item = _view.SpawnItem();
                        item.Init(b.Id, b.Name, OnBreedClicked);
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка списка пород: {ex.Message}");
                }
                finally
                {
                    await UniTask.SwitchToMainThread();
                    _view.ShowLoading(false);
                }
            });
        }

        private void OnBreedClicked(string id, string name)
        {
            _queue.CancelCurrentAndClear(r =>
                r.Type == RequestType.BreedsList ||
                r.Type == RequestType.BreedDetail);

            _view.ShowLoading(true);
            _queue.Enqueue(RequestType.BreedDetail, async token =>
            {
                try
                {
                    var info = await _service.GetBreedAsync(id, token);

                    await UniTask.SwitchToMainThread();
                    _view.ShowPopup(info.Name, info.Description);
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка детали породы: {ex.Message}");
                }
                finally
                {
                    await UniTask.SwitchToMainThread();
                    _view.ShowLoading(false);
                }
            });
        }

        public void Deactivate()
        {
            _queue.CancelCurrentAndClear(r =>
                r.Type == RequestType.BreedsList ||
                r.Type == RequestType.BreedDetail);
            _queue.CancelCurrentAndClear();
            _view.ShowLoading(false);
            _view.Clear();
            _view.HidePopup(); 
        }
    }
}
