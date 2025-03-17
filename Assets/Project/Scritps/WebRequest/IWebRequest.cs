using System;
using UnityEngine.Networking;

namespace CifkorApp.WebRequest
{
    public interface IWebRequest
    {
        event Action<IWebRequest> OnCancellationRequested;

        bool IsFinished { get; }

        UnityWebRequest CreateRequest();

        void SetRequestResult(EWebRequestResultType resultType, DownloadHandler downloadHandler);

        void Cancel();
    }
}
