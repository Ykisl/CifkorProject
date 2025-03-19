using UnityEngine;

namespace CifkorApp.Screen
{
    public class LoadableScreenView : ScreenView
    {
        protected bool _isLoading;

        public virtual void SetIsLoading(bool isLoading)
        {
            _isLoading = isLoading;
            UpdateLoadingView();
        }

        protected virtual void UpdateLoadingView() {}
    }
}
