using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace CifkorApp.WebRequest
{
    public abstract class BaseWebRequest<TData> : IWebRequest
    {
        protected CancellationToken _cancellationToken;
        protected CancellationTokenRegistration _cancellationHandler;

        protected WebRequestResult<TData> _result = null;
        protected TaskCompletionSource<WebRequestResult<TData>> _resultTask = new TaskCompletionSource<WebRequestResult<TData>>();

        protected bool _isFinished;

        public event Action<IWebRequest> OnCancellationRequested;

        public bool IsFinished
        {
            get => _isFinished;
        }

        public BaseWebRequest(CancellationToken requestCancellationToken = default)
        {
            _isFinished = false;

            _cancellationToken = requestCancellationToken;
            if(_cancellationToken != default)
            {
                _cancellationHandler = _cancellationToken.Register(Cancel);
            }
        }

        public abstract UnityWebRequest CreateRequest();

        public virtual void SetRequestResult(EWebRequestResultType resultType, DownloadHandler downloadHandler)
        {
            if (_isFinished)
            {
                return;
            }

            if(resultType == EWebRequestResultType.OK && downloadHandler == null)
            {
                resultType = EWebRequestResultType.ERROR;
            }

            if(resultType != EWebRequestResultType.OK)
            {
                var errorResult = new WebRequestResult<TData>(resultType, default);
                FinishRequest(errorResult);
                return;
            }

            var requestResult = ProcessRequestResult(downloadHandler);
            FinishRequest(requestResult);
        }

        public async UniTask<WebRequestResult<TData>> WaitForResult()
        {
            if (_isFinished)
            {
                return _result;
            }

            try
            {
                if (!_cancellationToken.IsCancellationRequested)
                {
                    return await _resultTask.Task.AsUniTask().AttachExternalCancellation(_cancellationToken);
                }
            }
            catch{}

            return new WebRequestResult<TData>(EWebRequestResultType.CANCELLED, default);
        }

        public void Cancel()
        {
            if (_isFinished)
            {
                return;
            }

            SetRequestResult(EWebRequestResultType.CANCELLED, null);
            OnCancellationRequested?.Invoke(this);
        }

        protected abstract WebRequestResult<TData> ProcessRequestResult(DownloadHandler downloadHandler);

        protected void FinishRequest(WebRequestResult<TData> result)
        {
            _isFinished = true;
            _result = result;
            _resultTask.TrySetResult(_result);
        }
    }
}
