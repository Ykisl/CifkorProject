using CifkorApp.WebRequest;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace CifkorApp.WebSprite
{
    public class WebSpriteSystem : IWebSpriteSystem
    {
        private IWebRequestSystem _webRequestSystem;

        private List<WebSpriteData> _webSpritesCache = new List<WebSpriteData>();

        [Inject]
        public void Construct(IWebRequestSystem webRequestSystem)
        {
            _webRequestSystem = webRequestSystem;
        }

        public async UniTask<Sprite> GetWebSprite(string spriteUrl, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(spriteUrl))
            {
                return null;
            }

            if(!TryGetWebSpriteData(spriteUrl, out var webSpriteData))
            {
                webSpriteData = await LoadWebSprite(spriteUrl);
                if(webSpriteData == null)
                {
                    return null;
                }
            }

            if (webSpriteData.IsLoading)
            {
                await UniTask.WaitWhile(() => webSpriteData.IsLoading || cancellationToken.IsCancellationRequested);
            }

            if (!webSpriteData.IsValid)
            {
                return null;
            }

            return webSpriteData.WebSprite;
        }

        protected bool TryGetWebSpriteData(string spriteUrl, out WebSpriteData spriteData)
        {
            spriteData = _webSpritesCache.FirstOrDefault(x => x.Url == spriteUrl);
            return spriteData != null;
        }

        private async UniTask<WebSpriteData> LoadWebSprite(string spriteUrl, CancellationToken cancellationToken = default)
        {
            var webSpriteData = new WebSpriteData()
            {
                Url = spriteUrl,
                WebSprite = null,
                WebTexture = null,
                IsLoading = true,
                IsValid = false,
            };

            _webSpritesCache.Add(webSpriteData);

            var webTextureRequest = new GetTextureWebRequest(spriteUrl, cancellationToken);
            _webRequestSystem.AddRequestToQueue(webTextureRequest);

            var textureLoadResult = await webTextureRequest.WaitForResult();
            if(textureLoadResult.ResultType != EWebRequestResultType.OK)
            {
                webSpriteData.IsValid = false;
                webSpriteData.IsLoading = false;
                _webSpritesCache.Remove(webSpriteData);
                return webSpriteData;
            }

            var texture = textureLoadResult.Data;

            var spriteRect = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(((Texture2D)texture), spriteRect, Vector2.zero);

            webSpriteData.WebTexture = texture;
            webSpriteData.WebSprite = sprite;
            webSpriteData.IsValid = true;
            webSpriteData.IsLoading = false;

            return webSpriteData;
        }
    }
}
