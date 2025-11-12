using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using ClinicAppointmentBookingBussinessTier.Action.Login.Services;
using ClinicAppointmentBookingBussinessTier.Object.Session.Services;
using ClinicAppointmentsBookingDTO.UserDTO;
using ClinicAppointmentsBookingDTO.SessionDTO;
using ProjectsServices.JWTServices;
using ProjectsServices.RefreshTokenServices;

namespace ClinicAppointmentBooking_InterfacesAPIs.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("Login")]
        [ProducesResponseType(typeof(clsUserTokens), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromHeader(Name = "LoginData")] string LoginRequirementJson)
        {
            if (string.IsNullOrWhiteSpace(LoginRequirementJson))
                return BadRequest("Login requirements are required");

            clsLoginRequirementsDTO loginRequirements;
            try
            {
                loginRequirements = JsonSerializer.Deserialize<clsLoginRequirementsDTO>(LoginRequirementJson);
            }
            catch (Exception)
            {
                return BadRequest("Invalid login requirements format");
            }

            if (string.IsNullOrWhiteSpace(loginRequirements?.UserName))
                return BadRequest("Username is required");

            if (string.IsNullOrWhiteSpace(loginRequirements?.Password))
                return BadRequest("Password is required");

            var loginService = new clsLoginService();
            var loginResult = await loginService.Login(loginRequirements);

            if (loginResult == null)
                return Unauthorized(loginService.ErrorMessage);

            string nativeRefreshToken = clsRefreshTokenHelper.GenerateRefreshToken();
            string hashedRefreshToken = clsRefreshTokenHelper.HashToken(nativeRefreshToken);

            var session = new clsInsertSessionDTO()
            {
                UserID = loginResult.UserID,
                HashedRefreshToken = hashedRefreshToken,
            };

            var saveSessionService = new clsSaveSessionService();
            var saveSessionResult = await saveSessionService.Save(session);

            if (!saveSessionResult)
                return StatusCode(500, saveSessionService.ErrorMessage);

            var claims = new List<Claim>
            {
                new Claim("UserID", loginResult.UserID?.ToString() ),
                new Claim("PersonID", loginResult.PersonID?.ToString() ),
                new Claim("FirstName", loginResult.FirstName ),
                new Claim(ClaimTypes.Role, loginResult.UserRole?.ToString() ),
                new Claim("JoiningDate", ((DateTime)(loginResult.JoiningDate)).ToString("yyyy/MM/dd"))
            };

            if (loginResult.UserBranchID.HasValue)
            {
                claims.Add(new Claim("UserBranchID", loginResult.UserBranchID.Value.ToString()));
            }

            var jwtToken = clsJWTHelper.GenerateJwtToken(claims, clsJWTHelper.GetToken(_configuration));

            return Ok(new clsUserTokens()
            {
                JWTToken = jwtToken,
                RefreshToken = nativeRefreshToken
            });
        }

        [HttpGet("LoginByRefreshToken")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginByRefreshToken([FromHeader(Name = "RefreshToken")] string RefreshToken)
        {
            if (string.IsNullOrWhiteSpace(RefreshToken))
                return BadRequest("Refresh token is required");

            var refreshTokenService = new clsLoginByRefreshTokenService();

            var loginResult = await refreshTokenService.Login(RefreshToken);

            if (loginResult == null)
                return Unauthorized(refreshTokenService.ErrorMessage);

            var claims = new List<Claim>
            {
                new Claim("UserID", loginResult.UserID?.ToString()),
                new Claim("PersonID", loginResult.PersonID?.ToString()),
                new Claim("FirstName", loginResult.FirstName),
                new Claim(ClaimTypes.Role, loginResult.UserRole?.ToString()),
                new Claim("JoiningDate", loginResult.JoiningDate?.ToString())
            };

            if (loginResult.UserBranchID.HasValue)
            {
                claims.Add(new Claim("UserBranchID", loginResult.UserBranchID.Value.ToString()));
            }

            var jwtToken = clsJWTHelper.GenerateJwtToken(claims, clsJWTHelper.GetToken(_configuration));

            return Ok(jwtToken);
            
        }
    }
}