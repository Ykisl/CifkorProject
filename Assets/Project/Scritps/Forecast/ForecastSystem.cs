using System;
using System.Collections.Generic;
using System.Threading;
using CifkorApp.Forecast.Models;
using CifkorApp.Forecast.Web;
using CifkorApp.WebRequest;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CifkorApp.Forecast
{
    public class ForecastSystem : IForecastSystem, IDisposable
    {
        private IWebRequestSystem _webRequestSystem;

        private CancellationTokenSource _serviceCancellationTokenSource;

        public ForecastSystem()
        {
            _serviceCancellationTokenSource = new CancellationTokenSource();
        }

        [Inject]
        private void Consturct(IWebRequestSystem webRequestSystem)
        {
            _webRequestSystem = webRequestSystem;
        }

        public void Dispose()
        {
            _serviceCancellationTokenSource.Cancel();
        }

        public async UniTask<ICollection<ForecastPeriodModel>> GetForecast(CancellationToken cancellationToken = default)
        {
            if (!_serviceCancellationTokenSource.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var requestCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(_serviceCancellationTokenSource.Token, cancellationToken);

                    var getForecastRequset = new GetForecastWebRequest(requestCancellationToken.Token);
                    _webRequestSystem.AddRequestToQueue(getForecastRequset);

                    var result = await getForecastRequset.WaitForResult();
                    if(result.ResultType != EWebRequestResultType.OK)
                    {
                        return null;
                    }

                    var periodModels = new List<ForecastPeriodModel>();
                    foreach(var periodData in result.Data.Properties.Periods)
                    {
                        var periodModel = new ForecastPeriodModel()
                        {
                            Name = periodData.Name,
                            Temperature = periodData.Temperature,
                            TemperatureUnit = periodData.TemperatureUnit
                        };

                        periodModels.Add(periodModel);
                    }

                    return periodModels;
                }
                catch { }
            }

            return null;
        }
    }
}
