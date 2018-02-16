using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Lykke.Service.Affiliate.Models
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
