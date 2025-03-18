using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using Unity.Plastic.Newtonsoft.Json;
using CifkorApp.WebRequest;
using CifkorApp.DogBreeds.Model;

namespace CifkorApp.DogBreeds.Web
{
    public class GetDogBreedsWebRequest : BaseWebRequest<GetDogBreedsWebRequest.Response>
    {
        public class Response
        {
            public List<DogBreedDataModel> Data;
        }

        private const string TARGET_URL = "https://dogapi.dog/api/v2/breeds";

        private int? _pageIndex;

        public GetDogBreedsWebRequest(int? pageIndex = null, CancellationToken cancellationToken = default)
        {
            _pageIndex = pageIndex;
            if (_pageIndex.HasValue && _pageIndex.Value < 1)
            {
                _pageIndex = 1;
            }
        }

        public override UnityWebRequest CreateRequest()
        {
            var apiUrl = GetBreedsApiUrl(_pageIndex);
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

        private string GetBreedsApiUrl(int? pageIndex)
        {
            var targetUrl = TARGET_URL;
            if(pageIndex != null)
            {
                targetUrl += $"?page[number]={pageIndex}";
            }

            return targetUrl;
        }
    }
}
