using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicAppointmentBookingBussinessTier.DataTypes;
using ClinicAppointmentBookingDataTier.ObjectData.RejectingCauseData;
using ClinicAppointmentsBookingDTO.AppointmentDTO;
using ClinicAppointmentsBookingDTO.AppointmentDTO.RejectingCauseDTO;

namespace ClinicAppointmentBookingBussinessTier.Object.Appointment.RejectingCause
{
    public class clsRejectingCause
    {

        public int? AppointmentID { get;internal set; }
        public string? Text { get; set; }
        public DateTime? RejectedDate { get; set; }

        public clsModes.enSaveMode SaveMode { get;internal set; }

        public clsRejectingCauseDTO RejectingCauseDTO
        {
            get {
                return new clsRejectingCauseDTO()
                { AppointmentID = this.AppointmentID, RejectedDate = this.RejectedDate, Text = this.Text }; }

            set
            {
                this.AppointmentID = value.AppointmentID;
                this.RejectedDate = value.RejectedDate;
                this.Text = value.Text;
            }
        }

        

        public clsRejectingCause()
        {
            SaveMode = clsModes.enSaveMode.eAdd;
        }


        private clsRejectingCause(clsRejectingCauseDTO RejectingCauseDTO)
        {

            SaveMode = clsModes.enSaveMode.eUpdate;

            this.RejectingCauseDTO = RejectingCauseDTO;

        }

        public static async Task<clsRejectingCause> Find(int AppointmentID)
        {

            var Result = await clsRejectingCauseData.GetRejectingCauseByAppointment(AppointmentID);

            if (Result == null) return null;

            if (Result.Item1 == null) return null;

            return new clsRejectingCause(Result.Item1);

        }

    }
}
