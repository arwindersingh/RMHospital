using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalAllocation.Model
{

    /// <summary>
    /// Handover
    /// Handover class represent a handover that is handed to the nurse. It has
    /// multiple attributes.
    /// </summary>
    public class Handover
    {

        /// <summary>
        /// Gets or sets the handover identifier.
        /// </summary>
        /// <value>The handover identifier.</value>
        [Key]
        public int HandoverID { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        [Required]
        public string PatientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>The name of the patient.</value>
        [Required]
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the alerts.
        /// </summary>
        /// <value>The alerts.</value>
        [Required]
        public string Alerts { get; set; }

        /// <summary>
        /// Gets or sets the bed number.
        /// </summary>
        /// <value>The bed number.</value>
        [Required]
        public string BedNumber { get; set; }

        /// <summary>
        /// Gets or sets the admission date.
        /// </summary>
        /// <value>The admission date.</value>
        [Required]
        public DateTime AdmissionDate { get; set; }

        /// <summary>
        /// Gets or sets the admission unit.
        /// </summary>
        /// <value>The admission unit.</value>
        [Required]
        public string AdmissionUnit { get; set; }

        /// <summary>
        /// Gets or sets the name of the nurse.
        /// </summary>
        /// <value>The name of the nurse.</value>
        public string NurseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        /// <value>The name of the student.</value>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the presenting complaint.
        /// </summary>
        /// <value>The presenting complaint.</value>
        public string PresentingComplaint { get; set; }

        /// <summary>
        /// Gets or sets the past medical history.
        /// </summary>
        /// <value>The past medical history.</value>
        public string PastMedicalHistory { get; set; }

        /// <summary>
        /// Gets or sets the significant events.
        /// </summary>
        /// <value>The significant events.</value>
        public string SignificantEvents { get; set; }

        /// <summary>
        /// A list of issues in the notes
        /// </summary>
        public List<HandoverIssue> HandoverIssues { get; set; }

        /// <summary>
        /// The Isolation section of the handover
        /// </summary> d
        /// <value>The isolation.</value>
        public string Isolation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:HospitalAllocation.Model.Handover"/> swab sent.
        /// </summary>
        /// <value><c>true</c> if swab sent; otherwise, <c>false</c>.</value>
        public bool SwabSent { get; set; }

        /// <summary>
        /// Gets or sets the swab sent date.
        /// </summary>
        /// <value>The swab sent date.</value>
        public DateTime SwabSentDate { get; set; }

    }
}
