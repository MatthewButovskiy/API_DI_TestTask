using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Scripts.MVC.Weather;
using Scripts.MVC.Dogs;

public class TabController : MonoBehaviour
{
    [SerializeField] private Button   _weatherBtn;
    [SerializeField] private Button   _dogsBtn;
    [SerializeField] private GameObject _weatherPanel;
    [SerializeField] private GameObject _dogsPanel;

    [Inject] private WeatherController _weatherController;
    [Inject] private DogsController    _dogsController;

    void Awake()
    {
        _weatherBtn.onClick.AddListener(ShowWeather);
        _dogsBtn  .onClick.AddListener(ShowDogs);
    }
    void Start()
    {
        ShowWeather();
    }
    private void ShowWeather()
    {
        _weatherPanel.SetActive(true);
        _dogsPanel   .SetActive(false);
        _dogsController.Deactivate();
        _weatherController.SetActive(true);
    }

    private void ShowDogs()
    {
        _dogsPanel   .SetActive(true);
        _weatherPanel.SetActive(false);
        _weatherController.SetActive(false);
        _dogsController.Activate();
    }
}