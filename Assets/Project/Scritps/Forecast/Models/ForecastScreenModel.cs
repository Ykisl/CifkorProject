using System;
using System.Collections.Generic;
using CifkorApp.Screen;
using CifkorApp.Utils;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CifkorApp.Forecast.Models
{
    public class ForecastScreenModel : LoadableScreenModel
    {
        private IForecastSystem _forecastSystem;

        private List<ForecastPeriodDataModel> _forecastData;
        private TimerModel _refreshTimer;

        private const int DATA_REFRESH_TIME = 5;

        public event Action OnForecastDataChanged;

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
            base.Activate();

            _refreshTimer.OnTimerFinished += HandleRefreshTimerFinished;
            _refreshTimer.Initialize(DATA_REFRESH_TIME);

            UpdateForecast().Forget();
        }

        public override void Deactivate()
        {
            _refreshTimer.Deinitialize();
            _refreshTimer.OnTimerFinished -= HandleRefreshTimerFinished;

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
            if (IsLoading)
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
