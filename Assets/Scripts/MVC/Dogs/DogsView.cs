using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Scripts.Core.Networking;

namespace Scripts.MVC.Dogs
{
    public class DogsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _contentRoot;
        [SerializeField] private LoadingSpinner _spinner;
        [SerializeField] private PopupPanel     _popup;
        [Inject]   private DogListItem.Pool      _pool;
        
        private readonly List<DogListItem> _activeItems = new List<DogListItem>();

        public void ShowLoading(bool on) => _spinner.gameObject.SetActive(on);

        public void Clear()
        {
            foreach (var item in _activeItems)
            {
                _pool.Despawn(item);
            }
            _activeItems.Clear();
        }

        public DogListItem SpawnItem()
        {
            var item = _pool.Spawn();
            _activeItems.Add(item);
            return item;
        }

        public void ShowPopup(string title, string body) => _popup.Show(title, body);
        public void HidePopup() { _popup.Hide(); }
    }
}