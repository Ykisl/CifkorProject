using System;
using System.Collections.Generic;
using System.Threading;
using CifkorApp.Screen;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorApp.Forecast.Models
{
    public class ForecastScreenModel : ScreenModel, ITickable
    {
        private IForecastSystem _forecastSystem;

        private CancellationTokenSource _screenCancellation;

        private bool _isLoading;
        private List<ForecastPeriodModel> _forecastData;

        public event Action OnForecastDataChanged;
        public event Action OnLoadingStateChanged;

        public bool IsLoading
        {
            get => _isLoading;
            protected set
            {
                _isLoading = value;
                OnLoadingStateChanged?.Invoke();
            }
        }

        public IList<ForecastPeriodModel> ForecastData
        {
            get => _forecastData;
        }

        public ForecastScreenModel() : base("Forecast") 
        {
            _forecastData = new List<ForecastPeriodModel>();
        }

        [Inject]
        public void Construct(IForecastSystem forecastSystem)
        {
            _forecastSystem = forecastSystem;
        }

        public override void Activate()
        {
            _screenCancellation = new CancellationTokenSource();
            base.Activate();

            UpdateForecast().Forget();
        }

        public override void Deactivate()
        {
            _screenCancellation?.Cancel();
            _screenCancellation = null;

            base.Deactivate();
        }

        public void Tick()
        {
            if (!_isActive)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            UpdateRefreshTimer(deltaTime);
        }

        public void UpdateRefreshTimer(float delta)
        {
            if (_isLoading)
            {
                return;
            }
        }

        public async UniTask UpdateForecast()
        {
            if(_screenCancellation == null || _screenCancellation.IsCancellationRequested)
            {
                return;
            }

            IsLoading = true;

            var forecastResult = await _forecastSystem.GetForecast(_screenCancellation.Token);
            _forecastData.Clear();
            if (forecastResult != null)
            {
                _forecastData.AddRange(forecastResult);
            }

            OnForecastDataChanged?.Invoke();
            IsLoading = false;
        }
    }
}
