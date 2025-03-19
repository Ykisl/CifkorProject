using CifkorApp.DogBreeds.Model;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace CifkorApp.DogBreeds
{
    public interface IDogBreedsSystem
    {
        UniTask<DogBreedDataModel> GetBreed(string breedId, CancellationToken cancellationToken = default);
        UniTask<ICollection<DogBreedDataModel>> GetBreeds(int? pageIndex = null, CancellationToken cancellationToken = default);
    }
}
