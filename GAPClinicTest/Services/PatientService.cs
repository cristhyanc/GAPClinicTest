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
    public class PatientService
    {
        RestClient _restService;
        string _serverUrl;
        public PatientService(string token, string serverUrl)
        {
            _restService = new RestClient(token);
            _serverUrl = serverUrl;
        }

        public async Task<IEnumerable<Patient>> GetPatients(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _restService.MakeApiCall<IEnumerable<Patient>>($"{_serverUrl}/Patient", HttpMethod.Get, null, cancellationToken);
        }
    }
}
