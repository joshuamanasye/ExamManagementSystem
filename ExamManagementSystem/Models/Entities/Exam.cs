using System;

namespace ExamManagementSystem.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public TimeSpan DurationMinutes { get; set; }
        public DateTime Date { get; set; }
        public Room? Room { get; set; }
        public ExamFile? ExamFile { get; set; } 

        public Exam() { }

        public Exam(Course course, TimeSpan durationMinutes)
        {
            Course = course;
            DurationMinutes = durationMinutes;
        }
    }
}
