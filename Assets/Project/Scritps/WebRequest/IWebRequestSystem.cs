

namespace CifkorApp.WebRequest
{
    public interface IWebRequestSystem
    {
        void AddRequestToQueue(IWebRequest request);

        void CancelAllRequests();
    }
}
