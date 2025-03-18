using CifkorApp.Screen;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Zenject;

namespace CifkorApp.DogBreeds.Model
{
    public class DogBreedsScreenModel : LoadableScreenModel
    {
        private IDogBreedsSystem _dogBreedsSystem;

        private List<DogBreedDataModel> _breedsData;

        public event Action OnBreedsDataCahnged;

        public IList<DogBreedDataModel> BreedsData
        {
            get => _breedsData;
        }

        public DogBreedsScreenModel() : base("Dog Breeds") 
        {
            _breedsData = new List<DogBreedDataModel>();
        }

        [Inject]
        private void Construct(IDogBreedsSystem dogBreedsSystem)
        {
            _dogBreedsSystem = dogBreedsSystem;
        }

        public override void Activate()
        {
            base.Activate();
            UpdateBreedsData().Forget();
        }

        public async UniTask UpdateBreedsData()
        {
            if (_screenCancellation == null || _screenCancellation.IsCancellationRequested)
            {
                return;
            }

            IsLoading = true;

            var data = await _dogBreedsSystem.GetBreeds(cancellationToken: _screenCancellation.Token);

            _breedsData.Clear();
            if(data != null)
            {
                _breedsData.AddRange(data);
            }

            OnBreedsDataCahnged?.Invoke();
            IsLoading = false;
        }
    }
}
