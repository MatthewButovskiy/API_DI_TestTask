using Scripts.Core.Networking;
using Scripts.Services.Dogs;
using Scripts.Services.Weather;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        var queue = new RequestQueue();
        Container.BindInstance(queue).AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<WeatherService>().AsSingle();
        Container.BindInterfacesAndSelfTo<DogService>().AsSingle();
    }
}