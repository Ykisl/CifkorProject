
namespace CifkorApp.WebRequest
{
    public class WebRequestResult<TData>
    {
        public readonly EWebRequestResultType ResultType;
        public readonly TData Data;

        public WebRequestResult(EWebRequestResultType resultType, TData data)
        {
            ResultType = resultType;
            Data = data;
        }

        public static implicit operator bool(WebRequestResult<TData> requestResult) 
        {
            return requestResult.ResultType == EWebRequestResultType.OK;
        }
    }
}
