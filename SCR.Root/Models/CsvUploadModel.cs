using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCR.Root.Models
{
    public class CsvUploadModel
    {
        public DateTime InsertedOn { get; set; }
        public int RecordType { get; set; }
        public int TransmittalBatchNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string RecordChangeType { get; set; }
        public int MemberId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Title { get; set; }
        public string Salutation { get; set; }
        public string Gender { get; set; }
        public string MailAddress { get; set; }
        public string MailAttnCareOf { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZIPCode { get; set; }
        public string MailZIPCode6 { get; set; }
        public int OfficeId { get; set; }
        public string MemberType { get; set; }
        public DateTime JoinedDate { get; set; }
        public string Status { get; set; }
        public DateTime StatusDate { get; set; }
        public string SocialSecurityNumber { get; set; }
        public int RELicenseNumber { get; set; }
        public DateTime LocalJoinDate { get; set; }
        public string OnlineStatus { get; set; }
        public DateTime OnlineStatusDate { get; set; }
        public int PrimaryAssociationID { get; set; }
        public int PrimaryStateAssociationID { get; set; }
        public int PrimaryFieldofBusiness { get; set; }
        public string EMailAddress { get; set; }
        public DateTime NARDuesPaid { get; set; }
        public DateTime StateDuesPaid { get; set; }
        public string MemberSubclass { get; set; }
        public int CellPhoneAreaCode { get; set; }
        public int CellPhoneNumber { get; set; }
        public string DesignatedRealtor { get; set; }
        public DateTime NRDSInsertDate { get; set; }

        public string csvfile { get; set; }
    }
}