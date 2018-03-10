namespace QnAServiceBot.Mobile.Models
{
    public class Activity
    {
        public string Type { get; set; }
        public string ChannelId { get; set; }
        public string Id { get; set; }
        public UserModel From { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
    }
}
