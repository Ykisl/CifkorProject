using UnityEngine;
using CifkorApp.WebRequest;
using UnityEngine.Networking;
using System.Threading;

namespace CifkorApp
{
    public class GetTextureWebRequest : BaseWebRequest<Texture>
    {
        private string _textureUrl;

        public GetTextureWebRequest(string textureUrl, CancellationToken requestCancellationToken = default) : base(requestCancellationToken)
        {
            _textureUrl = textureUrl;
        }

        public override UnityWebRequest CreateRequest()
        {
            return UnityWebRequestTexture.GetTexture(_textureUrl);
        }

        protected override WebRequestResult<Texture> ProcessRequestResult(DownloadHandler downloadHandler)
        {
            var texture = ((DownloadHandlerTexture)downloadHandler).texture;
            return new WebRequestResult<Texture>(EWebRequestResultType.OK, texture);
        }
    }
}
