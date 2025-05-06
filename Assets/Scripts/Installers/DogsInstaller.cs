using Zenject;
using Scripts.MVC.Dogs;
using UnityEngine;

public class DogsInstaller : MonoInstaller
{
    [SerializeField] private DogsView    _view;
    [SerializeField] private Transform   _poolParent;
    [SerializeField] private DogListItem _itemPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(_view).AsSingle();
        Container.BindMemoryPool<DogListItem, DogListItem.Pool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(_itemPrefab)
            .UnderTransform(_poolParent);
        Container.BindInterfacesAndSelfTo<DogsController>().AsSingle();
    }
}