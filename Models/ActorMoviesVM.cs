using Tweetinvi.Models.V2;

namespace Assignment_3.Models
{
    public class ActorMoviesVM
    {
        public Actor Actor { get; set; }
        public List<Movie> Movies { get; set; }

        public List<InternalTweet> Tweets { get; set; }
    }
}
