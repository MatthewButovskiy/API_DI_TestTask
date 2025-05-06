using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.MVC.Weather
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private Image   _icon;
        [SerializeField] private TMP_Text _label;

        public void SetData(Sprite sprite, int tempF)
        {
            _icon.sprite = sprite;
            _label.text  = $"Сегодня – {tempF}°F";
        }
    }
}