using Tweetinvi.Models.V2;

namespace Assignment_3.Interface
{
    public interface ITweetWrapper
    {
        public Task<TweetV2[]> GetTweetsAsync(ITweetable item);
    }
}
