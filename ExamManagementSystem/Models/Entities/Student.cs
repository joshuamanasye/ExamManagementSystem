using System.Collections.Generic;

namespace ExamManagementSystem.Models
{
    public class Student : User
    {
        public List<StudentCourse> StudentCourses { get; set; }

        public Student(string username, string password) : base(username, password, UserRole.Student)
        {
            StudentCourses = new List<StudentCourse>();
        }
    }
}
