using System;

namespace CifkorApp.Screen
{
    public class ScreenModel
    {
        protected string _screenName;
        protected bool _isActive = false;

        public event Action<bool> OnActiveStateChanged;

        public string ScreenName
        {
            get => _screenName;
        }

        public bool IsActive
        {
            get => _isActive;
        }

        public ScreenModel()
        {
            _screenName = string.Empty;
        }

        public ScreenModel(string screenName)
        {
            _screenName = screenName;
        }

        public virtual void Activate()
        {
            _isActive = true;
            OnActiveStateChanged?.Invoke(_isActive);
        }

        public virtual void Deactivate()
        {
            _isActive = false;
            OnActiveStateChanged?.Invoke(_isActive);
        }
    }
}
