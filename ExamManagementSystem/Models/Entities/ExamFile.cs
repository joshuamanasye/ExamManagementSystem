namespace ExamManagementSystem.Models
{
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class ExamFile
    {
        public int Id { get; set; }
        public string? FilePath { get; set; }
        public ApprovalStatus Status { get; set; }
        public User? QuestionMaker { get; set; }

        public ExamFile() { }

        public ExamFile(string filePath, User questionMaker)
        {
            FilePath = filePath;
            Status = ApprovalStatus.Pending;
            QuestionMaker = questionMaker;
        }
    }
}
