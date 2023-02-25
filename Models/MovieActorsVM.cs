using Tweetinvi.Models.V2;

namespace Assignment_3.Models
{
    public class MovieActorsVM
    {
        public Movie Movie { get; set; }
        public List<Actor> Actors { get; set; }
        public TweetV2[] Tweets { get; set; }
    }
}
