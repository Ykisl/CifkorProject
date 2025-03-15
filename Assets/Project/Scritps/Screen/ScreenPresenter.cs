using UnityEngine;
using Zenject;

namespace CifkorApp.Screen
{
    public abstract class ScreenPresenter<TModel, TView> : MonoBehaviour where TModel : ScreenModel where TView : ScreenView
    {
        [SerializeField] private TView _view;

        private TModel _model;

        [Inject]
        private void Construct(TModel model)
        {
            _model = model;
            SubscribeEvents();
        }

        #region UNITY_EVENTS

        protected virtual void Start()
        {
            UpdateView();
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeEvents();
        }

        #endregion

        protected virtual void UpdateView()
        {
            SetScreenActiveState(_model.IsActive);
        }

        protected void SetScreenActiveState(bool isScreenActive)
        {
            _view.SetIsScreenActive(isScreenActive);
        }

        protected virtual void SubscribeEvents()
        {
            _model.OnActiveStateChanged += HandleActiveStateChanged;
        }

        protected virtual void UnsubscribeEvents()
        {
            _model.OnActiveStateChanged -= HandleActiveStateChanged;
        }

        private void HandleActiveStateChanged(bool isScreenActive)
        {
            SetScreenActiveState(isScreenActive);
        }
    }
}
