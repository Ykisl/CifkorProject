using CifkorApp.Screen;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CifkorApp.Core
{
    public class ApplicationView : MonoBehaviour
    {
        [SerializeField] private Transform _tabsRoot;

        private ScreenTabButtonView.Factory _tabButtonFactory;

        private Dictionary<ScreenTabButtonView, ScreenModel> _screenTabs = new Dictionary<ScreenTabButtonView, ScreenModel>();

        public event Action<ScreenModel> OnScreenSelected;

        [Inject]
        private void Construct(
            ScreenTabButtonView.Factory tabButtonFactory
            )
        {
            _tabButtonFactory = tabButtonFactory;
        }

        #region UNITY_EVENTS

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {
            
        }

        #endregion

        public void Initialize(ApplicationModel model)
        {
            SetTabs(model.ActiveScreen, model.Screens);
        }

        public void SetTabs(ScreenModel activeTabScreen, ICollection<ScreenModel> avalibleScreens)
        {
            ClearTabs();

            foreach(var screen in avalibleScreens)
            {
                var isSelected = activeTabScreen != null && activeTabScreen == screen;

                var screenTabButton = _tabButtonFactory.Create(screen);
                _screenTabs.Add(screenTabButton, screen);

                screenTabButton.SetIsSelected(isSelected);

                screenTabButton.OnTabClicked += HandleTabClicked;

                screenTabButton.transform.SetParent(_tabsRoot);
                screenTabButton.transform.localScale = Vector3.one;

                screenTabButton.gameObject.SetActive(true);
            }
        }

        public void ClearTabs()
        {
            if(_screenTabs.Count <= 0)
            {
                return;
            }

            foreach(var tabButton in _screenTabs.Keys)
            {
                tabButton.OnTabClicked -= HandleTabClicked;
                tabButton.Dispose();
            }

            _screenTabs.Clear();
        }

        private void HandleTabClicked(ScreenTabButtonView tabView)
        {
            var screenModel = _screenTabs[tabView];
            OnScreenSelected?.Invoke(screenModel);
        }
    }
}
