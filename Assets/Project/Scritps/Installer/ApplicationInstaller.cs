using CifkorApp.Core;
using CifkorApp.DogBreeds;
using CifkorApp.Forecast;
using CifkorApp.Screen;
using UnityEngine;
using Zenject;

namespace CifkorApp
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private ScreenTabButtonView _tabButtonViewPrefab;

        public override void InstallBindings()
        {
            InstallModels();
            InstallApplicationPools();
        }

        private void InstallModels()
        {
            Container.Bind<ApplicationModel>().AsSingle();
            Container.Bind<ForecastScreenModel>().AsSingle();
            Container.Bind<DogBreedsScreenModel>().AsSingle();
        }

        private void InstallApplicationPools() 
        {
            Container.BindFactory<ScreenModel, ScreenTabButtonView, ScreenTabButtonView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_tabButtonViewPrefab).UnderTransformGroup("[Pool] ScreenTabButtonView");
                });
        }
    }
}
