using GraduationProjectFirebaseVersion.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProjectFirebaseVersion.Models
{
    public class Project
    {

        [Key]

        public int Id { get; set; }

        public string? Type_of_Project { get; set; }

        public string? ProjectTitle { get; set; }

        public string? ProjectDescription { get; set; }

        public string? StudentID { get; set; }

        public string? Project_File { get; set; }

        // Define the foreign key property
        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        public Professor? Professor { get; set; }



    }
}