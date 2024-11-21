namespace EmployeeServer.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public DateTime Birthday { get; set; }
        public string Sex { get; set; } = string.Empty;
        public bool HasChildren { get; set; }
    }
}
