using CifkorApp.Screen;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CifkorApp.MessageBox
{
    public class MessageBoxScreenView : ScreenView
    {
        [SerializeField] private TextMeshProUGUI _titileText;
        [SerializeField] private TextMeshProUGUI _contentText;
        [Space]
        [SerializeField] private Button _closeButton;

        public event Action OnClose;

        #region UNITY_EVENTS

        protected virtual void OnEnable()
        {
            _closeButton.onClick.AddListener(HandleCloseButtonClicked);
        }

        protected virtual void OnDisable()
        {
            _closeButton.onClick.RemoveListener(HandleCloseButtonClicked);
        }

        #endregion

        public void Initialize(string title, string content)
        {
            _titileText.text = title;
            _contentText.text = content;
        }

        private void HandleCloseButtonClicked()
        {
            OnClose?.Invoke();
        }
    }
}
