
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace CifkorApp.WebSprite
{
    public interface IWebSpriteSystem
    {
        public UniTask<Sprite> GetWebSprite(string spriteUrl, CancellationToken cancellationToken = default);
    }
}
