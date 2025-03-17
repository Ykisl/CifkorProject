using CifkorApp.Core.Model;
using CifkorApp.Screen;
using UnityEngine;
using Zenject;

namespace CifkorApp.Core
{
    public class ApplicationPresenter : MonoBehaviour
    {
        [SerializeField] private ApplicationView _applicationView;

        private ApplicationModel _model;

        public ApplicationModel Model
        {
            get => _model;
        }

        [Inject]
        private void Construct(ApplicationModel model)
        {
            _model = model;
            SubscribeEvent();
        }

        #region UNITY_EVENTS

        protected virtual void Start()
        {
            _model.Initialize();
            UpdateView();
        }

        protected virtual void OnDestory()
        {
            UnsubscribeEvent();
        }

        #endregion

        private void SubscribeEvent()
        {
            _model.OnActiveScreenChanged += HandleActiveScreenCahnged;

            _applicationView.OnScreenSelected += HandleViewTabScreenSelected;
        }

        private void UnsubscribeEvent()
        {
            _model.OnActiveScreenChanged -= HandleActiveScreenCahnged;

            _applicationView.OnScreenSelected -= HandleViewTabScreenSelected;
        }

        private void UpdateView()
        {
            _applicationView.Initialize(_model);
        }

        private void HandleActiveScreenCahnged(ScreenModel activeScreen)
        {
            UpdateView();
        }

        private void HandleViewTabScreenSelected(ScreenModel screen)
        {
            _model.SetActiveScreen(screen);
        }
    }
}
