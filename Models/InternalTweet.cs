using Tweetinvi.Models.V2;
using VaderSharp2;

namespace Assignment_3.Models
{
    public class InternalTweet
    {
        public TweetV2 Tweet { get; set; }
        public double Sentiment { get; set; }
        public string Text { get {  return Tweet.Text; } }
        SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();

        public InternalTweet(TweetV2 tweet)
        {
            Tweet = tweet;
            Sentiment = analyzer.PolarityScores(tweet.Text).Compound;

        }
    }
}
