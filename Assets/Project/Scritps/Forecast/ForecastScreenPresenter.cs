using CifkorApp.Forecast.Models;
using CifkorApp.Screen;
using System;
using UnityEngine;

namespace CifkorApp.Forecast
{
    public class ForecastScreenPresenter : ScreenPresenter<ForecastScreenModel, ForecastScreenView>
    {
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _model.OnForecastDataChanged += HandleForecastDataChanged;
            _model.OnLoadingStateChanged += HandleForecastLoadingStateChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _model.OnForecastDataChanged -= HandleForecastDataChanged;
            _model.OnLoadingStateChanged -= HandleForecastLoadingStateChanged;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _view.SetForecastData(_model.ForecastData);
            _view.SetIsLoading(_model.IsLoading);
        }

        private void HandleForecastDataChanged()
        {
            _view.SetForecastData(_model.ForecastData);
        }

        private void HandleForecastLoadingStateChanged()
        {
            _view.SetIsLoading(_model.IsLoading);
        }
    }
}
