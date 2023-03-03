using Assignment_3.Models;
using Tweetinvi.Models.V2;

namespace Assignment_3.Interface
{
    public interface ITweetWrapper
    {
        public Task<List<InternalTweet>> GetTweetsAsync(ITweetable item);
        public List<InternalTweet> GetAllTweetsInternal();
       public double GetTotalSentiment();
    }
}
