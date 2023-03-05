namespace AgileApp.Models
{
    public class WebsocketMessage
    {
        public Payload payload { get; set; }

        public string type { get; set; }
    }

    public class WebsocketMessageLoad
    {
        public List<string> payload { get; set; }

        public string type { get; set; }
    }

    public class Payload
    {
        public string text { get; set; }

        public string date { get; set; }

        public int userId { get; set; }

        public string sender { get; set; }
    }
}
