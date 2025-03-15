using System;
using System.Collections.Generic;
using CifkorApp.DogBreeds;
using CifkorApp.Forecast;
using CifkorApp.Screen;
using Zenject;

namespace CifkorApp.Core
{
    public class ApplicationModel
    {
        private ForecastScreenModel _forecastScreen;
        private DogBreedsScreenModel _dogBreedsScreen;

        private List<ScreenModel> _screens;
        private int _activeScreenIndex = -1;

        public event Action<ScreenModel> OnActiveScreenChanged;

        public ScreenModel ActiveScreen
        {
            get => GetActiveScreen();
            set => SetActiveScreen(value);
        }

        public IList<ScreenModel> Screens
        {
            get => _screens;
        }

        [Inject]
        private void Construct(
            ForecastScreenModel forecastScreen,
            DogBreedsScreenModel dogBreedsScreen
            )
        {
            _forecastScreen = forecastScreen;
            _dogBreedsScreen = dogBreedsScreen;
        }

        public ApplicationModel()
        {
            _screens = new List<ScreenModel>();
        }

        public void Initialize()
        {
            _screens.Clear();

            _screens.Add(_forecastScreen);
            _screens.Add(_dogBreedsScreen);

            SetActiveScreen(_forecastScreen);
        }

        public ScreenModel GetActiveScreen()
        {
            if(_activeScreenIndex < 0 || _screens.Count <= _activeScreenIndex)
            {
                return null;
            }

            return _screens[_activeScreenIndex];
        }

        public void SetActiveScreen(ScreenModel screenModel)
        {
            if(ActiveScreen == screenModel)
            {
                return;
            }

            ActiveScreen?.Deactivate();
            if (screenModel == null || !_screens.Contains(screenModel))
            {
                _activeScreenIndex = -1;
                OnActiveScreenChanged?.Invoke(null);
                return;
            }

            screenModel.Activate();
            _activeScreenIndex = _screens.IndexOf(screenModel);
            OnActiveScreenChanged?.Invoke(screenModel);
        }
    }
}
