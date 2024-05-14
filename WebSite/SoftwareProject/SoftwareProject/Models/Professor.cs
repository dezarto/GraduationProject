using System;

namespace GraduationProjectFirebaseVersion.Models
{
    public class Professor
    {


        public int Id { get; set; }


        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Department { get; set; }

        public ICollection<AvailableDateTimeSlot> AvailableDateTimeSlots { get; set; }


    }
}
