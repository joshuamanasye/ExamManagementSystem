namespace ExamManagementSystem.Models
{
    public enum UserRole
    {
        Admin,
        Department, // jurusan
        Scheduler, // penjadwalan 
        QuestionMaker, // pembuat soal
        Printer, // percetakan
        Invigilator, // pengawas
        Lecturer, // dosen
        Student // mahasiswa
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; } // tidak perlu inherit untuk tiap role karena untuk sekarang data dan behaviour tiap role tidak terlalu berbeda

        public User(string username, string password, UserRole role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        public User(int id, string userName, UserRole role)
        {
            Id = id;
            Username = userName;
            Role = role;
        }
    }
}
