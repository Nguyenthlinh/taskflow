namespace TaskApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; }= string.Empty;
        public string Role { get; set; }="User"; // Default role is "User"

        // 1 User có NHIỀU TaskItem (Mối quan hệ 1-Nhiều)
        public List<TaskItem> Tasks { get; set; } = new();

    }
}
