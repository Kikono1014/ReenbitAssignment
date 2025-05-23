namespace ChatApp.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string MessageText { get; set; } = "";
    public string Sentiment { get; set; } = "";
    public DateTime Timestamp { get; set; }
}
