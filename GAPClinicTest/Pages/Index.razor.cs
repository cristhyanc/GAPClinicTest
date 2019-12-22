using GAPClinicTest.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.EJ2.Blazor.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.Pages
{
    public class IndexBase : ComponentBase, IDisposable
    {

        public string Password { get; set; }
        public string User { get; set; }
        public string ServerError { get; set; }
        public EjsDialog ejsDialog { get; set; }
        [Inject] LoginService _loginService { get; set; }

        public async Task Login()
        {
            try
            {
                var result = await _loginService.LogIn(Password, User);
                Shared.AppSettings.APITOKEN = result.Token;
               await ejsDialog.Hide();
            }
            catch (Exception ex)
            {
                ServerError = ex.Message;
            }
        }

        public void Dispose()
        {
           
        }
    }
}
