using CifkorApp.Screen;
using Unity.Plastic.Newtonsoft.Json.Serialization;

namespace CifkorApp.MessageBox.Model
{
    public class MessageBoxScreenModel : ScreenModel
    {
        private string _titile;
        private string _text;

        public event Action OnDataChanged;

        public string Title
        {
            get => _titile;
            set
            {
                _titile = value;
                OnDataChanged?.Invoke();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnDataChanged?.Invoke();
            }
        }

        public void Show(string title, string text)
        {
            _titile = title;
            _text = text;

            OnDataChanged?.Invoke();
            Activate();
        }
    }
}
