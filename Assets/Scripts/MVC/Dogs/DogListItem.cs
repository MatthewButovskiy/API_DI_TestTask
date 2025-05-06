using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace Scripts.MVC.Dogs
{
    public class DogListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        private string _id;
        private System.Action<string,string> _onClick;
        private Button _button;

        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClick);
        }

        public void Init(string id, string name, System.Action<string,string> onClick)
        {
            _id = id;
            _label.text = name;
            _onClick = onClick;
        }

        public void OnClick() => _onClick?.Invoke(_id, _label.text);

        public class Pool : MonoMemoryPool<DogListItem> { }
    }
}