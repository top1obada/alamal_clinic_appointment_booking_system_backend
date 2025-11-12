using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.Services;
using ClinicAppointmentBookingDataTier.ObjectData.RejectingCauseData;
using ClinicAppointmentsBookingDTO.AppointmentDTO.RejectingCauseDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Appointment.RejectingCause.Services
{
    public class clsSaveRejectingCauseService : clsServiceError, ISaveService<clsRejectingCause>
    {
        public async Task<bool> Save(clsRejectingCause RejectingCause)
        {
            switch (RejectingCause.SaveMode)
            {
                case DataTypes.clsModes.enSaveMode.eAdd:
                    {
                        var error = await clsRejectingCauseData.InsertRejectingCause(RejectingCause.RejectingCauseDTO);

                        if (error != null)
                        {
                            ErrorMessage = $"Failed to add rejection cause - {error}";
                            return false;
                        }

                        RejectingCause.SaveMode = DataTypes.clsModes.enSaveMode.eUpdate;

                        return true;
                    }

                case DataTypes.clsModes.enSaveMode.eUpdate:
                    {
                        var error = await clsRejectingCauseData.UpdateRejectingCause(RejectingCause.RejectingCauseDTO);

                        if (error != null)
                        {
                            ErrorMessage = $"Failed to update rejection cause - {error}";
                            return false;
                        }

                        return true;
                    }

                default:
                    {
                        ErrorMessage = $"Invalid save mode: {RejectingCause.SaveMode}";
                        return false;
                    }
            }
        }
    }
}