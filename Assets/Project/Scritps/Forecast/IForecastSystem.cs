using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using CifkorApp.Forecast.Models;

namespace CifkorApp.Forecast
{
    public interface IForecastSystem
    {
        UniTask<ICollection<ForecastPeriodModel>> GetForecast(CancellationToken cancellationToken = default);
    }
}
