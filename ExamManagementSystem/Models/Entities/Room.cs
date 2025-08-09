namespace ExamManagementSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }
        public User? Invigilator { get; set; }

        public Room(string number, bool isAvailable, int capacity)
        {
            Number = number;
            IsAvailable = isAvailable;
            Capacity = capacity;
        }
    }
}
