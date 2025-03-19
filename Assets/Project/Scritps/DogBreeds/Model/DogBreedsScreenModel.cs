using CifkorApp.MessageBox.Model;
using CifkorApp.Screen;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Zenject;

namespace CifkorApp.DogBreeds.Model
{
    public class DogBreedsScreenModel : LoadableScreenModel
    {
        private IDogBreedsSystem _dogBreedsSystem;
        private MessageBoxScreenModel _messageBoxScreen;

        private List<DogBreedDataModel> _breedsData;

        private DogBreedDataModel _loadingBreed;
        private CancellationTokenSource _loadBreedInfoCancellation;


        public event Action OnBreedsDataCahnged;
        public event Action OnLoadingBreedCahnged;

        public IList<DogBreedDataModel> BreedsData
        {
            get => _breedsData;
        }

        public DogBreedDataModel LoadingBreed 
        {
            get => _loadingBreed;
        }

        public DogBreedsScreenModel() : base("Dog Breeds") 
        {
            _breedsData = new List<DogBreedDataModel>();
            _loadBreedInfoCancellation = new CancellationTokenSource();
        }

        [Inject]
        private void Construct(
            IDogBreedsSystem dogBreedsSystem,
            MessageBoxScreenModel messageBoxScreen
            )
        {
            _dogBreedsSystem = dogBreedsSystem;
            _messageBoxScreen = messageBoxScreen;
        }

        public override void Activate()
        {
            base.Activate();
            UpdateBreedsData().Forget();
        }

        public void SelectBreedModel(DogBreedDataModel breedModel)
        {
            if(breedModel == null || !_breedsData.Contains(breedModel))
            {

                return;
            }

            LoadBreedInfo(breedModel).Forget();
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

        private async UniTask LoadBreedInfo(DogBreedDataModel breedModel)
        {
            if(_loadBreedInfoCancellation != null)
            {
                _loadBreedInfoCancellation?.Cancel();
                await UniTask.WaitUntil(() => _loadBreedInfoCancellation.IsCancellationRequested || _screenCancellation.IsCancellationRequested);
            }

            if (_screenCancellation.IsCancellationRequested)
            {
                return;
            }

            _loadBreedInfoCancellation = new CancellationTokenSource();

            _loadingBreed = breedModel;
            var loadInfoCancellation = CancellationTokenSource.CreateLinkedTokenSource(_loadBreedInfoCancellation.Token, _screenCancellation.Token);

            // I assume that the test involves receiving new server data instead of using old ones
            SetLoadingBreed(breedModel);

            var loadedBreedModel = await _dogBreedsSystem.GetBreed(breedModel.Id, loadInfoCancellation.Token);
            ShowBreedInfo(loadedBreedModel);

            SetLoadingBreed(null);
        }

        private void ShowBreedInfo(DogBreedDataModel breedModel)
        {
            if(breedModel == null)
            {
                return;
            }

            var breedName = breedModel.AttributesData.Name;
            var lifeRange = breedModel.AttributesData.Life;

            var infoContentBuilder = new StringBuilder();
            infoContentBuilder.AppendLine(breedModel.AttributesData.Description);
            infoContentBuilder.AppendLine();
            infoContentBuilder.AppendLine($"Life range: {lifeRange.Min} - {lifeRange.Max}");

            _messageBoxScreen.Show(breedName, infoContentBuilder.ToString());
        }

        private void SetLoadingBreed(DogBreedDataModel breedModel)
        {
            _loadingBreed = breedModel;
            OnLoadingBreedCahnged?.Invoke();
        }
    }
}
