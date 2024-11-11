using System;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scanner.Helpers;

namespace Scanner.Controllers
{
    public class CheckUserController : BaseApiController
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly string _domain = "Elamir";

        public CheckUserController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet("CheckUser")]
        public async Task<ActionResult<AuthenticationResponse>> CheckUser(string Username, string Password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return BadRequest(new AuthenticationResponse
                {
                    IsValid = false,
                    Message = "Username or password cannot be empty."
                });
            }

            bool isValid = await AuthenticateUserAsync(Username, Password);

            if (isValid)
            {
                return Ok(new AuthenticationResponse
                {
                    IsValid = true,
                    Message = "User authentication successful."
                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponse
                {
                    IsValid = false,
                    Message = "Invalid credentials."
                });
            }
        }

        private async Task<bool> AuthenticateUserAsync(string Username, string Password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier(_domain)))
                    {
                        ldapConnection.AuthType = AuthType.Basic;
                        ldapConnection.Timeout = TimeSpan.FromSeconds(10);

                        var networkCredential = new NetworkCredential($"{_domain}\\{Username}", Password);
                        ldapConnection.Bind(networkCredential);

                        return true;
                    }
                }
                catch (LdapException ex)
                {
                    Console.WriteLine($"LDAP Error: {ex.Message}");
                    return false;
                }
            });
        }

        [HttpGet("CheckUserLoginTracking")]
        public async Task<ActionResult<AuthenticationResponse>> CheckUserLoginTracking(
            string Username, 
            string Password, 
            string AppName, 
            string LoginLong, 
            string LoginLat)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                return BadRequest(new AuthenticationResponse
                {
                    IsValid = false,
                    Message = "Username or password cannot be empty."
                });
            }

            bool isValid = await AuthenticateUserAsync(Username, Password);

            if (isValid)
            {
                var parameters = new 
                {
                    UserName = Username,
                    AppName,
                    LoginLong,
                    LoginLat
                };

                var result = await _databaseHelper
                    // .ExecuteStoredProcedureAsync<bool>("AppsLoginInsert", "dbax", "ReportManager", "sa", "123456", parameters, isQuery: false);
                    .ExecuteStoredProcedureAsync<bool>("AppsLoginInsert", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

                bool isTracked = result.FirstOrDefault();

                return Ok(new AuthenticationResponse
                {
                    IsValid = isTracked,
                    Message = isTracked ? "User authenticated and login tracked." : "Failed to track login."
                });

            }
            else
            {
                return Unauthorized(new AuthenticationResponse
                {
                    IsValid = false,
                    Message = "Invalid credentials."
                });
            }
        }
    }

    public class AuthenticationResponse
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
}
