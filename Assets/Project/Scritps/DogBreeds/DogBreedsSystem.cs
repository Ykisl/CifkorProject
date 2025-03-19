using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using CifkorApp.DogBreeds.Model;
using CifkorApp.DogBreeds.Web;
using CifkorApp.WebRequest;
using Zenject;
using static Codice.CM.WorkspaceServer.DataStore.WkTree.WriteWorkspaceTree;

namespace CifkorApp.DogBreeds
{
    public class DogBreedsSystem : IDogBreedsSystem
    {
        private IWebRequestSystem _webRequestSystem;

        private CancellationTokenSource _screenCancellation;

        private bool _isLoading;

        [Inject]
        private void Construct(IWebRequestSystem webRequestSystem)
        {
            _webRequestSystem = webRequestSystem;
        }

        public async UniTask<DogBreedDataModel> GetBreed(string breedId, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var requset = new GetDogBreedWebRequest(breedId, cancellationToken);
            _webRequestSystem.AddRequestToQueue(requset);

            var result = await requset.WaitForResult();
            if (result.ResultType != EWebRequestResultType.OK)
            {
                return null;
            }

            var responseData = result?.Data;
            return responseData?.Data;
        }

        public async UniTask<ICollection<DogBreedDataModel>> GetBreeds(int? pageIndex = null, CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var requset = new GetDogBreedsWebRequest(pageIndex, cancellationToken);
            _webRequestSystem.AddRequestToQueue(requset);

            var result = await requset.WaitForResult();
            if (result.ResultType != EWebRequestResultType.OK)
            {
                return null;
            }

            var responseData = result?.Data;
            return responseData?.Data;
        }
    }
}
