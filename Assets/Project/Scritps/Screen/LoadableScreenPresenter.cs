
namespace CifkorApp.Screen
{
    public class LoadableScreenPresenter<TModel, TView> : ScreenPresenter<TModel, TView> where TModel : LoadableScreenModel where TView : LoadableScreenView
    {
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _model.OnLoadingStateChanged += HandleLoadingStateChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _model.OnLoadingStateChanged -= HandleLoadingStateChanged;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            UpdateViewLoadingState();
        }

        protected virtual void UpdateViewLoadingState()
        {
            _view.SetIsLoading(_model.IsLoading);
        }

        protected virtual void HandleLoadingStateChanged()
        {
            UpdateViewLoadingState();
        }
    }
}
