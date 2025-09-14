namespace WebApplication2.Entity
{
    public class UserDepartment
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

    }
}
