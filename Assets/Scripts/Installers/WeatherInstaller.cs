using Zenject;
using Scripts.MVC.Weather;
using UnityEngine;

public class WeatherInstaller : MonoInstaller
{
    [SerializeField] private WeatherView _view;
    public override void InstallBindings()
    {
        Container.BindInstance(_view).AsSingle();
        Container.BindInterfacesAndSelfTo<WeatherController>().AsSingle();
    }
}