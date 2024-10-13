namespace EtcsServer.Database.Entity
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public IList<Train> Receivers { get; set; }
    }
}
