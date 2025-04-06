namespace MeinHTLWienWest.Services
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public int Floor { get; set; }
        public Marker ProblemMarker { get; set; }
        public string ClientEmail { get; set; }
        public DateTime Timestamp { get; set; }
        public List<byte[]> Images { get; set; } = new List<byte[]>();
    }
}
