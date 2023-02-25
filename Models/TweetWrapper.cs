using Assignment_3.Interface;
using Tweetinvi;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;

namespace Assignment_3.Models
{
    public class TweetWrapper : ITweetWrapper
    {
        public TwitterClient UserClient { get; set; }
        private ITweetable Item { get; set; }

        public TweetWrapper() {
            UserClient = new TwitterClient(@"AAx9UfdCemph0Pg0t8Moq5c6L", 
                "LbhoERpFGjBESYSNjTHuRvE0R80cGxZBx5lJWanM5lFpO2Hs63", 
                "1455230009153503238-WTxQgoYUAQ3D9PTSsUu8stHkmJvuVe", 
                "2ZVnM9tWbCSNAhyJcyC4WPIgiIbUWZ77MTLSx2Qb8TkW3");
        }

        public async Task<TweetV2[]> GetTweetsAsync(ITweetable item)
        {
            Item = item;
            var searchResponse = await UserClient.SearchV2.SearchTweetsAsync(item.SearchTerm());
            return searchResponse.Tweets;
        }
    }
}
