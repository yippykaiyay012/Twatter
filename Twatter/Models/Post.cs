using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Twatter.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TwatVotes { get; set; }
        public int Retwats { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        // public int TwatLevel { get; set; }
        public int FontSize { get; set; }

        private const int MaxFontSize = 80;

        public Post()
        {

        }

        public Post(string content, string userId)
        {
            Content = content;
            UserId = userId;
            FontSize = SetFontSize(this.Content);
        }

        public static int SetFontSize(string content)
        {

            var twatWords = new List<string>
            {
                "avocado","macbook","starbucks","blog","blogging","pumpkin"
            };

            var fontSize = Convert.ToDouble(MaxFontSize);
            var words = content.ToLower().Split(' ');


            foreach (var word in words)
            {
                if (twatWords.Contains(word))
                {
                    // decrease font size by 50%
                    fontSize *= 0.50;
                }
            }

            var finalFontSize = Convert.ToInt32(fontSize);



            return finalFontSize;
        }


        public static void GetZoomLevel()
        {

        }
    }
}
