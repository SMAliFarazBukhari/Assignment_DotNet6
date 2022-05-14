using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_DotNet6.Entities
{
    public class Visit
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime VisitDate { get; set; }

        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }

        [ForeignKey("DoctorId")]

        public Doctor Doctor { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
       
    }
}
