namespace WebApplication2.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserDepartment> UserDepartments { get; set; }
    }
}
