using CifkorApp.DogBreeds.Model;
using CifkorApp.Screen;

namespace CifkorApp.DogBreeds
{
    public class DogBreedsScreenPresenter : LoadableScreenPresenter<DogBreedsScreenModel, DogBreedsScreenView>
    {
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();
            _model.OnBreedsDataCahnged += HandleBreedsDataChanged;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();
            _model.OnBreedsDataCahnged -= HandleBreedsDataChanged;
        }

        protected override void UpdateView()
        {
            base.UpdateView();
            _view.SetBreedsData(_model.BreedsData);
        }

        private void HandleBreedsDataChanged()
        {
            UpdateView();
        }
    }
}
