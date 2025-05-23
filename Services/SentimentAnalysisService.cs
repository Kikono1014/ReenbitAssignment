using Azure;
using Azure.AI.TextAnalytics;

namespace ChatApp.Services;

public class SentimentAnalysisService
{
    private readonly TextAnalyticsClient _client;

    // initializing azure sentiment analysis client via api key
    public SentimentAnalysisService(string endpoint, string apiKey)
    {
        var credentials = new AzureKeyCredential(apiKey);
        _client = new TextAnalyticsClient(new Uri(endpoint), credentials);
    }

    // analyzing sentiment, if any error return "Undetermined" sentiment status
    public async Task<string> AnalyzeSentimentAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "Undetermined";

        try
        {
            // invoking analyze sentiment methos from client
            DocumentSentiment documentSentiment = await _client.AnalyzeSentimentAsync(text);
            return documentSentiment.Sentiment.ToString(); // returning simple string value
        }
        catch (Exception)
        {
            return "Undetermined";
        }
    }
}
