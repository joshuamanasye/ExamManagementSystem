using ExamManagementSystem.Models;

public class Attendance
{
    public int Id { get; set; }

    public int ExamId { get; set; }
    public Exam Exam { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; }

    public bool IsPresent { get; set; }

    public Attendance() { }

    public Attendance(int examId, int studentId, bool isPresent)
    {
        ExamId = examId;
        StudentId = studentId;
        IsPresent = isPresent;
    }
}
