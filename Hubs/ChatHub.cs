using ChatApp.Data;
using ChatApp.Services;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;


namespace ChatApp.Hubs;

// Hub for real-time chat
// injects sentiment analysis service
public class ChatHub(ChatContext context, SentimentAnalysisService sentimentService) : Hub
{
    private readonly ChatContext _context = context;
    private readonly SentimentAnalysisService _sentimentService = sentimentService;

    // method to send messages to other users
    public async Task SendMessage(string user, string message)
    {
        // evaluate sentiment from service
        var sentiment = await _sentimentService.AnalyzeSentimentAsync(message);

        // saves message to db
        var chatMessage = new ChatMessage
        {
            UserName = user,
            MessageText = message,
            Sentiment = sentiment,
            Timestamp = DateTime.UtcNow
        };

        _context.ChatMessages.Add(chatMessage);
        await _context.SaveChangesAsync();

        // sends action about receiving message for client to react and add this message in real-time
        await Clients.All.SendAsync("ReceiveMessage", user, message, sentiment);
    }
}
