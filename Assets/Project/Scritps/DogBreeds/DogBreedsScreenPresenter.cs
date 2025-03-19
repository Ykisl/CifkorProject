using CifkorApp.DogBreeds.Model;
using CifkorApp.Screen;
using System;

namespace CifkorApp.DogBreeds
{
    public class DogBreedsScreenPresenter : LoadableScreenPresenter<DogBreedsScreenModel, DogBreedsScreenView>
    {
        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            _model.OnBreedsDataCahnged += HandleBreedsDataChanged;
            _model.OnLoadingBreedCahnged += HandleLoadingBreedChanged;

            _view.OnBreedModelSelected += HandleViewBreedModelSelected;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            _model.OnBreedsDataCahnged -= HandleBreedsDataChanged;
            _view.OnBreedModelSelected -= HandleViewBreedModelSelected;
        }

        protected override void UpdateView()
        {
            base.UpdateView();

            _view.SetLoadingBreed(_model.LoadingBreed);
            _view.SetBreedsData(_model.BreedsData);
        }

        private void HandleBreedsDataChanged()
        {
            UpdateView();
        }

        private void HandleViewBreedModelSelected(DogBreedDataModel breedModel)
        {
            _model.SelectBreedModel(breedModel);
        }

        private void HandleLoadingBreedChanged()
        {
            _view.SetLoadingBreed(_model.LoadingBreed);
        }
    }
}
