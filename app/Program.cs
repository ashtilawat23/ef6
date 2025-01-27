using System;
using System.Threading.Tasks;
using EF6Demo.Models;
using EF6Demo.Repositories;

namespace EF6Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (var repository = new StudentRepository())
                {
                    // Add some sample students
                    Console.WriteLine("Adding sample students...");
                    await AddSampleStudents(repository);

                    // Get all students
                    Console.WriteLine("\nAll active students:");
                    var students = await repository.GetAllStudentsAsync();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName} - GPA: {student.GPA:F2}");
                    }

                    // Get students with GPA between 3.0 and 4.0
                    Console.WriteLine("\nHonor roll students (GPA 3.0-4.0):");
                    var honorStudents = await repository.GetStudentsByGPARangeAsync(3.0m, 4.0m);
                    foreach (var student in honorStudents)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName} - GPA: {student.GPA:F2}");
                    }

                    // Update a student
                    var firstStudent = await repository.GetStudentByIdAsync(1);
                    if (firstStudent != null)
                    {
                        Console.WriteLine($"\nUpdating GPA for {firstStudent.FirstName}...");
                        firstStudent.GPA = 3.9m;
                        await repository.UpdateStudentAsync(firstStudent);
                    }

                    // Soft delete a student
                    Console.WriteLine("\nSoft deleting a student...");
                    await repository.DeleteStudentAsync(2);

                    // Show remaining active students
                    Console.WriteLine("\nRemaining active students:");
                    students = await repository.GetAllStudentsAsync();
                    foreach (var student in students)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName} - GPA: {student.GPA:F2}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task AddSampleStudents(StudentRepository repository)
        {
            var students = new[]
            {
                new Student
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(2000, 1, 15),
                    Address = "123 Main St",
                    GPA = 3.5m
                },
                new Student
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(2001, 3, 20),
                    Address = "456 Oak Ave",
                    GPA = 3.8m
                },
                new Student
                {
                    FirstName = "Mike",
                    LastName = "Johnson",
                    Email = "mike.johnson@example.com",
                    DateOfBirth = new DateTime(2000, 7, 10),
                    Address = "789 Pine Rd",
                    GPA = 3.2m
                }
            };

            foreach (var student in students)
            {
                await repository.AddStudentAsync(student);
            }
        }
    }
} 