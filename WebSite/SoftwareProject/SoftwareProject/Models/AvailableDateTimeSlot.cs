using System;


namespace GraduationProjectFirebaseVersion.Models
{
    public class AvailableDateTimeSlot
    {

        public int Id { get; set; }

        public int ProfessorId { get; set; }

        public Professor? Professor { get; set; }

        public DateTime AvailableDate { get; set; }

        public TimeSpan AvailableHour { get; set; }

        public bool isAvailable { get; set; }
    }
}