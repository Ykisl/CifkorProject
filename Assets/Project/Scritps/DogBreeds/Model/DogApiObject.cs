using Unity.Plastic.Newtonsoft.Json;

namespace CifkorApp.DogBreeds.Model
{
    public class DogApiObject<TAttributes>
    {
        public string Id;
        public string Type;

        [JsonProperty("attributes")]
        public TAttributes AttributesData;
    }
}
