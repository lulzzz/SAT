using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAT_NISSAN.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class DataLogin
    {
        public string Token { get; set; }
    }

    public class Data
    {
        public List<GetLocationResponse> getLocations { get; set; }
    }
    
    public class SuatResponse
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public Errors errors { get; set; }

    }

    public class SuatResponse2
    {
        public bool success { get; set; }
        public List<GetLocationResponse> data { get; set; }
        public List<Errors> errors { get; set; }

    }

    public class AuthResponse
    {
        public bool success { get; set; }
        public DataLogin data { get; set; }
        public Errors errors { get; set; }

    }
    public class FailAuthResponse
    {
        public bool isSucceded { get; set; }
        public string message { get; set; }
    }
}