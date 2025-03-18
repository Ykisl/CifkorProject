using System;
using System.Collections.Generic;
using System.Threading;
using CifkorApp.Screen;
using CifkorApp.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorApp.Forecast.Models
{
    public class ForecastScreenModel : ScreenModel
    {
        private IForecastSystem _forecastSystem;

        private CancellationTokenSource _screenCancellation;

        private bool _isLoading;
        private List<ForecastPeriodDataModel> _forecastData;
        private TimerModel _refreshTimer;

        private const int DATA_REFRESH_TIME = 5;

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

        public IList<ForecastPeriodDataModel> ForecastData
        {
            get => _forecastData;
        }

        public TimerModel RefreshTimer
        {
            get => _refreshTimer;
        }

        public ForecastScreenModel() : base("Forecast") 
        {
            _forecastData = new List<ForecastPeriodDataModel>();
            _refreshTimer = new TimerModel();
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

            _refreshTimer.OnTimerFinished += HandleRefreshTimerFinished;
            _refreshTimer.Initialize(DATA_REFRESH_TIME);

            UpdateForecast().Forget();
        }

        public override void Deactivate()
        {
            _refreshTimer.Deinitialize();
            _refreshTimer.OnTimerFinished -= HandleRefreshTimerFinished;

            _screenCancellation?.Cancel();
            _screenCancellation = null;

            base.Deactivate();
        }

        public void Update(float deltaTime)
        {
            if (!_isActive)
            {
                return;
            }

            UpdateRefreshTimer(deltaTime);
        }

        public void UpdateRefreshTimer(float delta)
        {
            if (_isLoading)
            {
                return;
            }

            _refreshTimer.Update(delta);
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

            _refreshTimer.Reset();
        }

        private void HandleRefreshTimerFinished(TimerModel timer)
        {
            UpdateForecast().Forget();
        }
    }
}
