using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Networking;
using Zenject;

namespace CifkorApp.WebRequest
{
    public class WebRequestSystem : IWebRequestSystem, IInitializable, IDisposable
    {
        private List<IWebRequest> _requestsQueue;

        private IWebRequest _activeRequest;
        private UnityWebRequest _activeUnityRequest;

        private CancellationTokenSource _queueProcessingCancellationToken;
        private bool _isQueueProcessingActive;


        public void Initialize()
        {
            _requestsQueue = new List<IWebRequest>();
        }

        public void Dispose()
        {
            CancelAllRequests();
        }

        public void AddRequestToQueue(IWebRequest request)
        {
            if (request.IsFinished || _activeRequest == request || _requestsQueue.Contains(request))
            {
                return;
            }

            SubscribeWebRequest(request);
            _requestsQueue.Add(request);

            StartRequestQueueProcessing();
        }

        public void CancelAllRequests()
        {
            StopRequestQueueProcessing();

            var cancelList = new List<IWebRequest>(_requestsQueue);
            foreach(var request in cancelList)
            {
                CancelRequest(request);
            }
        }

        private void CancelRequest(IWebRequest request)
        {
            UnsubscribeWebRequest(request);
            request.Cancel();
            _requestsQueue.Remove(request);

            if (_activeRequest == request)
            {
                _activeUnityRequest.Abort();
            }
        }

        private void StartRequestQueueProcessing()
        {
            if (_isQueueProcessingActive)
            {
                return;
            }

            _queueProcessingCancellationToken = new CancellationTokenSource();
            ProcessRequestQueue(_queueProcessingCancellationToken.Token).Forget();
        }

        private void StopRequestQueueProcessing()
        {
            if (!_isQueueProcessingActive)
            {
                return;
            }

            _queueProcessingCancellationToken?.Cancel();
            if(_activeRequest != null)
            {
                CancelRequest(_activeRequest);
            }
        }

        private async UniTask ProcessRequestQueue(CancellationToken cancellationToken)
        {
            _isQueueProcessingActive = true;

            while (_requestsQueue.Count > 0)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                _activeRequest = _requestsQueue[0];
                _activeUnityRequest = null;
                _requestsQueue.RemoveAt(0);

                if (_activeRequest == null)
                {
                    continue;
                }

                if (_activeRequest.IsFinished)
                {
                    UnsubscribeWebRequest(_activeRequest);
                }

                var resultType = EWebRequestResultType.OK;
                _activeUnityRequest = _activeRequest.CreateRequest();

                try
                {
                    await _activeUnityRequest.SendWebRequest();

                    if (_activeUnityRequest.result != UnityWebRequest.Result.Success)
                    {
                        resultType = EWebRequestResultType.ERROR;
                    }

                    _activeRequest.SetRequestResult(resultType, _activeUnityRequest.downloadHandler);
                }
                catch
                {
                    resultType = EWebRequestResultType.CANCELLED;
                    _activeRequest.SetRequestResult(resultType, null);
                }

                UnsubscribeWebRequest(_activeRequest);
                _activeUnityRequest = null;
                _activeRequest = null;
            }

            _activeRequest = null;
            _activeUnityRequest = null;
            _isQueueProcessingActive = false;
        }

        private void SubscribeWebRequest(IWebRequest request)
        {
            request.OnCancellationRequested += HandleWebRequestCancellationRequested;
        }

        private void UnsubscribeWebRequest(IWebRequest request)
        {
            request.OnCancellationRequested -= HandleWebRequestCancellationRequested;
        }

        private void HandleWebRequestCancellationRequested(IWebRequest request)
        {
            CancelRequest(request);
        }
    }
}
