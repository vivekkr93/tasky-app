namespace Tasky.Api.Model
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
