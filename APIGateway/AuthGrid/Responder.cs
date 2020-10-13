using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
    public class Responder
    {
        public int ResponderId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    //public class AuthResponse
    //{
    //    public string Token { get; set; }
    //    public string RefreshToken { get; set; }
    //    public APIResponseStatus Status { get; set; }
    //}

}
