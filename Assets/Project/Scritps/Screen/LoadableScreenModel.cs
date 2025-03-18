using System;
using System.Threading;

namespace CifkorApp.Screen
{
    public class LoadableScreenModel : ScreenModel
    {
        protected CancellationTokenSource _screenCancellation;

        private bool _isLoading;

        public event Action OnLoadingStateChanged;

        public bool IsLoading
        {
            get => _isLoading;
            protected set
            {
                _isLoading = value;
                OnLoadingStateChanged?.Invoke();
            }
        }

        public LoadableScreenModel(string screenName) : base(screenName) {}

        public override void Activate()
        {
            _screenCancellation = new CancellationTokenSource();
            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _screenCancellation?.Cancel();
            _screenCancellation = null;
        }
    }
}
