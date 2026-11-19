namespace dz8lat
{
    public class Employee
    {
        public string Name { get; }
        public string Role { get; }

        public Employee(string name, string role)
        {
            Name = name;
            Role = role;
        }

        public override string ToString() => $"{Name} ({Role})";
    }
}