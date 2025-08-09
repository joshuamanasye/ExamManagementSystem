namespace ExamManagementSystem.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public Exam Exam { get; set; }
        public Student Student { get; set; }
        public int Score { get; set; }

        public Grade() { }
        public Grade(Exam exam, Student student, int score)
        {
            Exam = exam;
            Student = student;
            Score = score;
        }
    }
}
