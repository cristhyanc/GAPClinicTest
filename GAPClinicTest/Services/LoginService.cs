using GAPClinicTest.DTO;
using GAPClinicTest.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GAPClinicTest.Services
{
    public class LoginService
    {
        RestClient _restService;
        string _serverUrl;
        public LoginService( string serverUrl)
        {
            _restService = new RestClient("");
            _serverUrl = serverUrl;
        }

        public async Task<LoginDetails> LogIn(string password, string user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var details = new { Password = password, User=user};
            return await _restService.MakeApiCall<LoginDetails>($"{_serverUrl}/Login", HttpMethod.Post, details, cancellationToken);
        }
    }
}
