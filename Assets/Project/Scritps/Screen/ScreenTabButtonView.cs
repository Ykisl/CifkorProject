using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CifkorApp.Screen
{
    public class ScreenTabButtonView : MonoBehaviour, IPoolable<ScreenModel, IMemoryPool>, IDisposable
    {
        [SerializeField] private Button _tabButton;
        [SerializeField] private TextMeshProUGUI _tabText;

        private ScreenModel _model;
        private IMemoryPool _pool;

        public event Action<ScreenTabButtonView> OnTabClicked;

        public class Factory : PlaceholderFactory<ScreenModel ,ScreenTabButtonView> { }

        #region UNITY_EVENTS

        protected virtual void OnEnable()
        {
            _tabButton.onClick.AddListener(HandleButtonClicked);
        }

        protected virtual void OnDisable()
        {
            _tabButton.onClick.RemoveListener(HandleButtonClicked);
        }

        #endregion

        public virtual void Initialize(ScreenModel model)
        {
            _model = model;
            if(_model == null)
            {
                return;
            }

            _tabText.text = model.ScreenName;
        }

        public void SetIsSelected(bool isSelected)
        {
            _tabButton.interactable = !isSelected;
        }

        private void HandleButtonClicked()
        {
            OnTabClicked?.Invoke(this);
        }

        #region IPoolable

        public void OnSpawned(ScreenModel model, IMemoryPool pool)
        {
            _pool = pool;
            Initialize(model);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
            OnTabClicked = null;
            _pool = null;
        }

        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        #endregion
    }
}
