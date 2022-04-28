using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ct.Models
{
    public class Administration
    {
        //Leave Request for Day scholars
        public class StudentGatePassModel
        {
            public string Id { get; set; }
            public string StudentName { get; set; }
            public string RegistrationNo { get; set; }
            public string RegistrationNumber { get; set; }
            public string UniversitySchool { get; set; }
            public string UniversityName { get; set; }
            public string ReasonForLeave { get; set; }
            public string Date { get; set; }
            public string FromTime { get; set; }
            public string ToTime { get; set; }
            public string Hostel { get; set; }
        }

        public class RMSQueries
        {
            public string MessageType { get; set; }
            public string Category { get; set; }
            public string Subcategory { get; set; }
            public string MeetingId { get; set; }
            public string Block { get; set; }
            public string RoomNo { get; set; }
            public string ContactNo { get; set; }
            public string DealingPerson { get; set; }
            public string TicketNo { get; set; }
            public string UserId { get; set; }
            public string Description { get; set; }
            public string Remarks { get; set; }
            public string Status { get; set; }
            public string IsDisplayForRMS { get; set; }
            public string SendBy { get; set; }
            public string Number { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string FloorNo { get; set; }
        }

        //leaves
        public class Leave
        {
            public int Id { get; set; }
            public string UniqueNo { get; set; }
            public Nullable<int> LeaveCategory { get; set; }
            public string LeaveReason { get; set; }
            public Nullable<int> LeavePeriod { get; set; }
            public string LeaveAddress { get; set; }
            public string DaySpan { get; set; }
            public string StartDate { get; set; }
            public string LastDate { get; set; }
            public string HalfDaySpan { get; set; }
            public string HalfLeaveDate { get; set; }
            public string ShortLeaveDate { get; set; }
            public string ShortLeaveTimeSlot { get; set; }
            public string Status { get; set; }
            public Nullable<bool> IsSanctioned { get; set; }
            public Nullable<bool> IsApproved { get; set; }
            public string RemarksBySanctioning { get; set; }
            public string RemarksByRecommending { get; set; }
            public Nullable<double> NoOfDays { get; set; }
            public string SanctionedBy { get; set; }
            public string SanctionedOn { get; set; }
            public string ApprovedBy { get; set; }
            public string ApprovedOn { get; set; }
            public string EntryDate { get; set; }
            public string EntryBy { get; set; }
            public string ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<double> LeaveBalance { get; set; }
            public string EarnedLeaveAvailedOn { get; set; }
            public Nullable<double> LeaveBalanceLeft { get; set; }
            public Nullable<bool> IsPDL { get; set; }
            public string PdlId { get; set; }
            public string Attachment { get; set; }
            public string AdjustmentTo { get; set; }
            public string ImageBytes { get; set; }
            public string FileName { get; set; }
 
        }

        public partial class EmployeeDPR
        {
            public int id { get; set; }
            public string UniqueNo { get; set; }
            public System.DateTime DPRDate { get; set; }
            public string Task { get; set; }
            public string TaskDetail { get; set; }
            public string Status { get; set; }
            public System.DateTime EntryOn { get; set; }
            public Nullable<System.DateTime> ModifiedOn { get; set; }
            public Nullable<int> TaskId { get; set; }
            public Nullable<double> ManhoursConsumed { get; set; }
            public Nullable<int> TaskQuantity { get; set; }
        }

    }
}