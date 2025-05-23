using System;
using System.Collections.Generic;
using System.Threading;
using CifkorApp.Forecast.Models;
using CifkorApp.Forecast.Web;
using CifkorApp.WebRequest;
using CifkorApp.WebSprite;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CifkorApp.Forecast
{
    public class ForecastSystem : IForecastSystem, IDisposable
    {
        private IWebRequestSystem _webRequestSystem;
        private IWebSpriteSystem _webSpriteSystem;

        private CancellationTokenSource _serviceCancellationTokenSource;

        public ForecastSystem()
        {
            _serviceCancellationTokenSource = new CancellationTokenSource();
        }

        [Inject]
        private void Consturct(
            IWebRequestSystem webRequestSystem,
            IWebSpriteSystem webSpriteSystem
            )
        {
            _webRequestSystem = webRequestSystem;
            _webSpriteSystem = webSpriteSystem;
        }

        public void Dispose()
        {
            _serviceCancellationTokenSource.Cancel();
        }

        public async UniTask<ICollection<ForecastPeriodDataModel>> GetForecast(CancellationToken cancellationToken = default)
        {
            var requestCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(_serviceCancellationTokenSource.Token, cancellationToken);
            if (requestCancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var getForecastRequset = new GetForecastWebRequest(requestCancellationToken.Token);
            _webRequestSystem.AddRequestToQueue(getForecastRequset);

            var result = await getForecastRequset.WaitForResult();
            if (result.ResultType != EWebRequestResultType.OK)
            {
                return null;
            }

            var responseData = result.Data;
            var periodModels = responseData.Properties.Periods;
            foreach (var periodData in periodModels)
            {
                var periodIconSprite = await _webSpriteSystem.GetWebSprite(periodData.IconUrl, requestCancellationToken.Token);
                periodData.IconSprite = periodIconSprite;
            }

            return periodModels;
        }
    }
}
