using Unity.Plastic.Newtonsoft.Json;
using CifkorApp.Utils;

namespace CifkorApp.DogBreeds.Model
{
    public class DogBreedDataModel : DogApiObject<DogBreedDataModel.Attributes>
    {
        public class Attributes
        {
            public string Name;

            public IntRange Life;
            public IntRange MaleWeight;
            public IntRange FemaleWeight;

            public string Description;

            [JsonProperty("hypoallergenic")]
            public bool IsHypoallergenic;
        }
    }
}
