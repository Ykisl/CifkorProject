using CifkorApp.DogBreeds.Model;
using CifkorApp.WebRequest;
using System.Threading;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine.Networking;

namespace CifkorApp.DogBreeds.Web
{
    public class GetDogBreedWebRequest : BaseWebRequest<GetDogBreedWebRequest.Response>
    {
        public class Response
        {
            public DogBreedDataModel Data;
        }

        private const string TARGET_URL = "https://dogapi.dog/api/v2/breeds";

        private string _breedId;

        public GetDogBreedWebRequest(string breedId, CancellationToken cancellationToken = default) : base(cancellationToken)
        {
            _breedId = breedId;
        }

        public override UnityWebRequest CreateRequest()
        {
            if (string.IsNullOrEmpty(_breedId))
            {
                return null;
            }

            var apiUrl = GetBreedsApiUrl(_breedId);
            return UnityWebRequest.Get(apiUrl);
        }

        protected override WebRequestResult<Response> ProcessRequestResult(DownloadHandler downloadHandler)
        {
            string jsonText = downloadHandler.text;
            if (string.IsNullOrEmpty(jsonText))
            {
                return null;
            }

            var response = JsonConvert.DeserializeObject<Response>(jsonText);
            var jsonReuslt = EWebRequestResultType.OK;
            if (response == null)
            {
                jsonReuslt = EWebRequestResultType.ERROR;
            }

            return new WebRequestResult<Response>(jsonReuslt, response);
        }

        private string GetBreedsApiUrl(string breedId)
        {
            var targetUrl = $"{TARGET_URL}/{breedId}";
            return targetUrl;
        }
    }
}
