using System.Collections.Generic;
using UnityEngine;
using CifkorApp.Forecast.Models;
using CifkorApp.Screen;
using Zenject;
using TMPro;

namespace CifkorApp.Forecast
{
    public class ForecastScreenView : ScreenView
    {
        [SerializeField] private Transform _periodViewRoot;
        [SerializeField] private TextMeshProUGUI _loadingText;

        private ForecastPeriodView.Factory _viewFactory;

        private List<ForecastPeriodView> _periodViewItems = new List<ForecastPeriodView>();
        private bool _isLoading;

        [Inject]
        private void Consturct(ForecastPeriodView.Factory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public void SetIsLoading(bool isLoading)
        {
            _isLoading = isLoading;
            UpdateLoadingView();
        }

        public void SetForecastData(IList<ForecastPeriodDataModel> data)
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

            UpdateLoadingView();
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
            UpdateLoadingView();
        }

        private void UpdateLoadingView()
        {
            var isLoadingVisible = _isLoading && _periodViewItems.Count <= 0;
            _loadingText.gameObject.SetActive(isLoadingVisible);
        }
    }
}
