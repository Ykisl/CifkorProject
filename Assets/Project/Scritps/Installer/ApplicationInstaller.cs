using CifkorApp.Core.Model;
using CifkorApp.DogBreeds;
using CifkorApp.DogBreeds.Model;
using CifkorApp.Forecast;
using CifkorApp.Forecast.Models;
using CifkorApp.MessageBox.Model;
using CifkorApp.Screen;
using CifkorApp.WebRequest;
using CifkorApp.WebSprite;
using UnityEngine;
using Zenject;

namespace CifkorApp
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private ScreenTabButtonView _tabButtonViewPrefab;
        [SerializeField] private ForecastPeriodView _forecastPreiodViewPrefab;
        [SerializeField] private DogBreedView _dogBreedViewPrefab;

        public override void InstallBindings()
        {
            InstallModels();
            InstallSystems();

            InstallApplicationPools();
        }

        private void InstallModels()
        {
            Container.Bind<ApplicationModel>().AsSingle();
            Container.Bind<ForecastScreenModel>().AsSingle();
            Container.Bind<DogBreedsScreenModel>().AsSingle();

            Container.Bind<MessageBoxScreenModel>().AsSingle();
        }

        private void InstallApplicationPools() 
        {
            Container.BindFactory<ScreenModel, ScreenTabButtonView, ScreenTabButtonView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_tabButtonViewPrefab).UnderTransformGroup("[Pool] ScreenTabButtonView");
                });
            
            Container.BindFactory<ForecastPeriodDataModel, ForecastPeriodView, ForecastPeriodView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_forecastPreiodViewPrefab).UnderTransformGroup("[Pool] ForecastPeriodView");
                });

            Container.BindFactory<DogBreedDataModel, DogBreedView, DogBreedView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_dogBreedViewPrefab).UnderTransformGroup("[Pool] DogBreedView");
                });
        }

        private void InstallSystems()
        {
            Container.BindInterfacesAndSelfTo<WebRequestSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<WebSpriteSystem>().AsSingle();

            Container.BindInterfacesAndSelfTo<ForecastSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<DogBreedsSystem>().AsSingle();
        }
    }
}
