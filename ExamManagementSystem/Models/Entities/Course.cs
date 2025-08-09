using System.Collections.Generic;

namespace ExamManagementSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User? Lecturer { get; set; }
        public List<StudentCourse>? StudentCourses { get; set; }

        public Course() { }
        public Course(string name, User lecturer)
        {
            Name = name;
            Lecturer = lecturer;
            StudentCourses = new List<StudentCourse>();
        }

        public int GetNumverOfStudents()
        {
            return StudentCourses?.Count ?? 0; // ato bisa select count dari StudentCourses
        }
    }
}
