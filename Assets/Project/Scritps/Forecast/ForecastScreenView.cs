using System.Collections.Generic;
using UnityEngine;
using CifkorApp.Forecast.Models;
using CifkorApp.Screen;
using Zenject;

namespace CifkorApp.Forecast
{
    public class ForecastScreenView : ScreenView
    {
        [SerializeField] private Transform _periodViewRoot;

        private ForecastPeriodView.Factory _viewFactory;

        private List<ForecastPeriodView> _periodViewItems = new List<ForecastPeriodView>();

        [Inject]
        private void Consturct(ForecastPeriodView.Factory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void SetIsLoading(bool isLoading)
        {
            
        }

        public void SetForecastData(IList<ForecastPeriodModel> data)
        {
            ClearForecastData();

            for (int i = 0; i < data.Count; i++)
            {
                var periodModel = data[i];
                var periodView = _viewFactory.Create(periodModel);
                _periodViewItems.Add(periodView);

                periodView.transform.SetParent(_periodViewRoot);
                periodView.transform.localScale = Vector3.one;
                periodView.gameObject.SetActive(true);
            }
        }

        public void ClearForecastData()
        {
            if(_periodViewItems.Count <= 0)
            {
                return;
            }

            foreach (var periodViewItem in _periodViewItems)
            {
                periodViewItem.Dispose();
            }

            _periodViewItems.Clear();
        }
    }
}
