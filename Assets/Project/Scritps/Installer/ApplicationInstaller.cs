using CifkorApp.Core.Model;
using CifkorApp.DogBreeds.Model;
using CifkorApp.Forecast;
using CifkorApp.Forecast.Models;
using CifkorApp.Screen;
using CifkorApp.WebRequest;
using UnityEngine;
using Zenject;

namespace CifkorApp
{
    public class ApplicationInstaller : MonoInstaller
    {
        [SerializeField] private ScreenTabButtonView _tabButtonViewPrefab;
        [SerializeField] private ForecastPeriodView _forecastPreiodViewPrefab;

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
        }

        private void InstallApplicationPools() 
        {
            Container.BindFactory<ScreenModel, ScreenTabButtonView, ScreenTabButtonView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_tabButtonViewPrefab).UnderTransformGroup("[Pool] ScreenTabButtonView");
                });
            
            Container.BindFactory<ForecastPeriodModel, ForecastPeriodView, ForecastPeriodView.Factory>()
                .FromMonoPoolableMemoryPool(pool =>
                {
                    pool.WithInitialSize(2).FromComponentInNewPrefab(_forecastPreiodViewPrefab).UnderTransformGroup("[Pool] ForecastPeriodView");
                });
        }

        private void InstallSystems()
        {
            Container.BindInterfacesAndSelfTo<WebRequestSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<ForecastSystem>().AsSingle();
        }
    }
}
