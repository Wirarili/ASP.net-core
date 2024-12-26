namespace aspProject.Models
{
    public class CharacterHistory
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
