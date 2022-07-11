using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.API.Contracts
{
    public class ClientResponse<T>
    {
        public static readonly string DefaultErrorMsg = "Sorry, your request could not be completed.";
        public static readonly string SuccesMessage = "Request completed succesfully.";
        public static readonly int SuccessCode = 0;
        public ClientResponse()
        {
            SetDefaults();
        }
        public ClientResponse(System.Net.HttpStatusCode statusCode)
        {
            SetDefaults((int)statusCode);
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }

        private void SetDefaults(int code = 0)
        {
            Code = code;
            Message = (code == 0) ? SuccesMessage : DefaultErrorMsg;
            IsSuccess = (code == 0) ? true : false;
            ExceptionMessage = "";
        }

        public void CopyResponse<TData>(ClientResponse<TData> clientResponse)
        {
            Code = clientResponse.Code;
            Message = clientResponse.Message;
            IsSuccess = clientResponse.IsSuccess;
            ExceptionMessage = clientResponse.ExceptionMessage;
            StackTrace = clientResponse.StackTrace;
            StatusCode = clientResponse.StatusCode;
        }
    }
}
