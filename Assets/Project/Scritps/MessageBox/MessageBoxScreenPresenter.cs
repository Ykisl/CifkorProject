using CifkorApp.MessageBox.Model;
using CifkorApp.Screen;
using UnityEngine;

namespace CifkorApp.MessageBox
{
    public class MessageBoxScreenPresenter : ScreenPresenter<MessageBoxScreenModel, MessageBoxScreenView>
    {
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _model.OnDataChanged += HandleModelDataChanged;
            _view.OnClose += HandleViewClose;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _model.OnDataChanged -= HandleModelDataChanged;
            _view.OnClose -= HandleViewClose;
        }

        protected override void UpdateView()
        {
            base.UpdateView();
            _view.Initialize(_model.Title, _model.Text);
        }

        private void HandleModelDataChanged()
        {
            UpdateView();
        }

        private void HandleViewClose()
        {
            _model.Deactivate();
        }
    }
}
