using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EF6Demo.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal GPA { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public Student()
        {
            IsActive = true;
            EnrollmentDate = DateTime.UtcNow;
        }
    }
} 