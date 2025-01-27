using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EF6Demo.Data;
using EF6Demo.Models;

namespace EF6Demo.Repositories
{
    public class StudentRepository : IDisposable
    {
        private readonly SchoolContext _context;
        private bool _disposed = false;

        public StudentRepository()
        {
            _context = new SchoolContext();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.IsActive = false;  // Soft delete
            return await UpdateStudentAsync(student);
        }

        public async Task<IEnumerable<Student>> GetStudentsByGPARangeAsync(decimal minGPA, decimal maxGPA)
        {
            return await _context.Students
                .Where(s => s.IsActive && s.GPA >= minGPA && s.GPA <= maxGPA)
                .OrderByDescending(s => s.GPA)
                .ToListAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
} 