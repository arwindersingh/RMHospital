using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HospitalAllocation.Data.Handover
{
    [DataContract]
    public class HandoverDTO
    {
        public HandoverDTO(string bedNumber,
                           DateTime admissionDate,
                           string admissionUnit,
                           string nurseName,
                           string studentName,
                           string patientId,
                           string patientName,
                           string alerts,
                           string presentingComplaint,
                           string pastMedicalHistory,
                           string significantEvents,
                           HandoverIssueDTO [] handoverIssues,
                           string isolation,
                           bool swabSent,
                           DateTime swabSentDate)
        {
            PatientId = patientId;
            PatientName = patientName;
            Alerts = alerts;
            BedNumber = bedNumber;
            AdmissionDate = admissionDate;
            AdmissionUnit = admissionUnit;
            NurseName = nurseName;
            StudentName = studentName;
            PresentingComplaint = presentingComplaint;
            PastMedicalHistory = pastMedicalHistory;
            SignificantEvents = significantEvents;
            HandoverIssues = handoverIssues;
            Isolation = isolation;
            SwabSent = swabSent;
            SwabSentDate = swabSentDate;
        }

        [DataMember(Name = "patient_id")]
        public string PatientId { get; }

        [DataMember(Name = "patient_name")]
        public string PatientName { get; }

        [DataMember(Name = "alerts")]
        public string Alerts { get; }

        [DataMember(Name = "bed_number")]
        public string BedNumber { get; }

        [DataMember(Name = "admission_date")]
        public DateTime AdmissionDate { get; }

        [DataMember(Name = "admission_unit")]
        public string AdmissionUnit { get; }

        [DataMember(Name = "nurse_name")]
        public string NurseName { get; }

        [DataMember(Name = "student_name")]
        public string StudentName { get; }

        [DataMember(Name = "present_complaint")]
        public string PresentingComplaint { get; }

        [DataMember(Name = "past_medical_history")]
        public string PastMedicalHistory { get; }

        [DataMember(Name = "significant_events")]
        public string SignificantEvents { get; }

        [DataMember(Name = "handover_issues")]
        public HandoverIssueDTO [] HandoverIssues { get; }

        [DataMember(Name = "isolation")]
        public string Isolation { get; }

        [DataMember(Name = "swab_sent")]
        public bool SwabSent { get; }

        [DataMember(Name = "swab_sent_time")]
        public DateTime SwabSentDate { get; }
    }
}
