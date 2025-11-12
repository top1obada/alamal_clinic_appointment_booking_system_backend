using ClinicAppointmentsBookingDTO.AppointmentDTO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ClinicAppointmentsBookingDTO.UserDTO;
using System.Security.Claims;

namespace ClinicAppointmentBooking_InterfacesAPIs.AuthorizationActionsFilter
{
    public class ChangeAppointmentAuthorizationActionFilter
        : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
          
            if (context.ActionArguments.TryGetValue("changeStatusDTO", out var dtoObj) &&
                dtoObj is clsChangeAppointmentStatusDTO dto)
            {
                CheckAuthorization(context, dto);
                return;
            }

           
            var dtoFromBody = ReadRequestBody(context);
            if (dtoFromBody != null)
            {
                CheckAuthorization(context, dtoFromBody);
            }
            else
            {
                context.Result = new BadRequestObjectResult(new { error = "Invalid appointment status data" });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          
        }

        private void CheckAuthorization(ActionExecutingContext context, clsChangeAppointmentStatusDTO dto)
        {
            if (dto == null)
            {
                context.Result = new BadRequestObjectResult(new { error = "Invalid appointment status data" });
                return;
            }

            var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (dto.AppointmentStatus == enAppointmentStatus.Rejected
                || dto.AppointmentStatus == enAppointmentStatus.Confirmed
                || dto.AppointmentStatus == enAppointmentStatus.Completed)
            {
                if (userRole != "eDoctor" && userRole != "eReceptionist")
                {
                    context.Result = new UnauthorizedResult();
                }
                return;
            }

            if (dto.AppointmentStatus == enAppointmentStatus.Cancelled)
            {
                if (userRole != "ePatient")
                {
                    context.Result = new UnauthorizedResult();
                }
                return;
            }
        }

        private clsChangeAppointmentStatusDTO ReadRequestBody(ActionExecutingContext context)
        {
            try
            {
                var request = context.HttpContext.Request;

                if (!request.Body.CanRead || request.ContentLength == null || request.ContentLength == 0)
                    return null;

              
                request.EnableBuffering();
                request.Body.Position = 0;

                using var streamReader = new StreamReader(request.Body, leaveOpen: true);
                string requestBody = streamReader.ReadToEnd();

                
                request.Body.Position = 0;

                if (string.IsNullOrWhiteSpace(requestBody))
                    return null;

                return JsonSerializer.Deserialize<clsChangeAppointmentStatusDTO>(
                    requestBody,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading request body: {ex.Message}");
                return null;
            }
        }
    }
}