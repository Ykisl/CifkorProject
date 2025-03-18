using CifkorApp.Utils;
using Newtonsoft.Json;
using System;

namespace CifkorApp.DogBreeds.Model
{
    public class DogBreedData
    {
        public Guid Id;
        public DogBreedAttributesData Attributes;
    }

    public class DogBreedAttributesData
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
