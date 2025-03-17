using System;
using UnityEngine;
using UnityEngine.UI;
using CifkorApp.Forecast.Models;
using Zenject;
using TMPro;

namespace CifkorApp.Forecast
{
    public class ForecastPeriodView : MonoBehaviour, IPoolable<ForecastPeriodModel, IMemoryPool>, IDisposable
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _infoText;

        private ForecastPeriodModel _model;
        private IMemoryPool _pool;

        public class Factory : PlaceholderFactory<ForecastPeriodModel, ForecastPeriodView> { }

        public virtual void Initialize(ForecastPeriodModel model)
        {
            _model = model;
            if (_model == null)
            {
                return;
            }

            var isIconAvalible = _model.Icon != null;
            _iconImage.gameObject.SetActive(isIconAvalible);
            if (isIconAvalible)
            {
                _iconImage.sprite = _model.Icon;
            }

            _infoText.text = $"{_model.Name} - {_model.Temperature}{_model.TemperatureUnit}";
        }

        public void OnSpawned(ForecastPeriodModel model, IMemoryPool pool)
        {
            _pool = pool;
            Initialize(model);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            _pool = null;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }
    }
}
