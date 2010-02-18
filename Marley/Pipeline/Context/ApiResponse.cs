using System.Net;

namespace Marley.Pipeline.Context
{
    public class ApiResponse : IResponse
    {
        #region IResponse Members

        public WebResponse Response { get; set; }
        public string Data { get; set; }

        #endregion
    }
}