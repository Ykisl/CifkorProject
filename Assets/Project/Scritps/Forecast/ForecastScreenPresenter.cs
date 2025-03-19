using CifkorApp.Forecast.Models;
using CifkorApp.Screen;
using UnityEngine;

namespace CifkorApp.Forecast
{
    public class ForecastScreenPresenter : LoadableScreenPresenter<ForecastScreenModel, ForecastScreenView>
    {
        #region UNITY_EVENTS

        protected virtual void Update()
        {
            var deltaTime = Time.deltaTime;
            _model.Update(deltaTime);
        }

        #endregion

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _model.OnForecastDataChanged += HandleForecastDataChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _model.OnForecastDataChanged -= HandleForecastDataChanged;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _view.SetForecastData(_model.ForecastData);
        }

        private void HandleForecastDataChanged()
        {
            _view.SetForecastData(_model.ForecastData);
        }
    }
}
