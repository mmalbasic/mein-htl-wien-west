namespace MeinHTLWienWest.Services
{
    public class SubmitTicketDTO
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }   
        public TicketStatus Status { get; set; }
        public int Floor { get; set; }
        public Marker ProblemMarker { get; set; }
        public int ClientId { get; set; }
        public List<string> ImagePaths { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; }
    }

    public enum TicketStatus
    {
        Done = 1,
        InWork
    }
}
