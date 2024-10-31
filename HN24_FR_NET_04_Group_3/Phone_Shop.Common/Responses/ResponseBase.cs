using System.Net;

namespace Phone_Shop.Common.Responses
{
    public class ResponseBase
    {
        public int Code { get; set; }
        public string Message { get; set; } = null!;
        public object Data { get; set; } = false;

        public ResponseBase(object data, string message, int code)
        {
            Data = data;
            Message = message;
            Code = code;
        }

        public ResponseBase(object data, string message)
        {
            Data = data;
            Message = message;
            Code = (int)HttpStatusCode.OK;
        }

        public ResponseBase(string message, int code)
        {
            Data = false;
            Message = message;
            Code = code;
        }

        public ResponseBase(object data)
        {
            Data = data;
            Message = string.Empty;
            Code = (int)HttpStatusCode.OK;
        }

    }

}
