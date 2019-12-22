using GAPClinicTest.DTO;
using GAPClinicTest.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public async Task<Patient> GetPatient(Guid patientID, CancellationToken cancellationToken = default(CancellationToken))
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = patientID.ToString();
            return await _restService.MakeApiCall<Patient>($"{_serverUrl}/Patient/GetById", HttpMethod.Get, queryString, cancellationToken);
        }

        public async Task<bool> Save(Patient patient, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _restService.MakeApiCall($"{_serverUrl}/Patient/", HttpMethod.Post, patient, cancellationToken);
        }

        public async Task<bool> DeletePatient(Guid patientID, CancellationToken cancellationToken = default(CancellationToken))
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = patientID.ToString();
            return await _restService.MakeApiCall($"{_serverUrl}/Patient/Delete", HttpMethod.Delete, queryString, cancellationToken);
        }
    }
}
