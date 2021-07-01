namespace BirdMeister.Models
{
    public class TweetArchive
    {
        public Tweets[] AllTweets { get; set; }
    }

    public class Tweets
    {
        public Tweet Tweet { get; set; }
    }

    public class Tweet
    {
        public bool Retweeted { get; set; }
        public string Source { get; set; }
        public Entities Entities { get; set; }
        public string[] Display_text_range { get; set; }
        public string Favorite_count { get; set; }
        public string Id_str { get; set; }
        public bool Truncated { get; set; }
        public string Retweet_count { get; set; }
        public string Id { get; set; }
        public bool Possibly_sensitive { get; set; }
        public string Created_at { get; set; }
        public bool Favorited { get; set; }
        public string Full_text { get; set; }
        public string Lang { get; set; }
        public string In_reply_to_user_id { get; set; }
        public string In_reply_to_screen_name { get; set; }
        public string In_reply_to_user_id_str { get; set; }
        public string In_reply_to_status_id_str { get; set; }
        public string In_reply_to_status_id { get; set; }
        public Extended_Entities Extended_entities { get; set; }
        public bool Withheld_copyright { get; set; }
        public string[] Withheld_in_countries { get; set; }
    }

    public class Entities
    {
        public Hashtag[] Hashtags { get; set; }
        public Symbol[] Symbols { get; set; }
        public User_Mentions[] User_mentions { get; set; }
        public Url[] Urls { get; set; }
        public Medium[] Media { get; set; }
    }

    public class Hashtag
    {
        public string Text { get; set; }
        public string[] Indices { get; set; }
    }

    public class Symbol
    {
        public string Text { get; set; }
        public string[] Indices { get; set; }
    }

    public class User_Mentions
    {
        public string Name { get; set; }
        public string Screen_name { get; set; }
        public string[] Indices { get; set; }
        public string Id_str { get; set; }
        public string Id { get; set; }
    }

    public class Url
    {
        public string Urlstring { get; set; }
        public string Expanded_url { get; set; }
        public string Display_url { get; set; }
        public string[] Indices { get; set; }
    }

    public class Medium
    {
        public string Expanded_url { get; set; }
        public string Source_status_id { get; set; }
        public string[] Indices { get; set; }
        public string Url { get; set; }
        public string Media_url { get; set; }
        public string Id_str { get; set; }
        public string Source_user_id { get; set; }
        public string Id { get; set; }
        public string Media_url_https { get; set; }
        public string Source_user_id_str { get; set; }
        public Sizes Sizes { get; set; }
        public string Type { get; set; }
        public string Source_status_id_str { get; set; }
        public string Display_url { get; set; }
    }

    public class Sizes
    {
        public Large Large { get; set; }
        public Thumb Thumb { get; set; }
        public Small Small { get; set; }
        public Medium1 Medium { get; set; }
    }

    public class Large
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Thumb
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Small
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Medium1
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Extended_Entities
    {
        public Medium2[] Media { get; set; }
    }

    public class Medium2
    {
        public string Expanded_url { get; set; }
        public string Source_status_id { get; set; }
        public string[] Indices { get; set; }
        public string Url { get; set; }
        public string Media_url { get; set; }
        public string Id_str { get; set; }
        public string Source_user_id { get; set; }
        public string Id { get; set; }
        public string Media_url_https { get; set; }
        public string Source_user_id_str { get; set; }
        public Sizes1 Sizes { get; set; }
        public string Type { get; set; }
        public string Source_status_id_str { get; set; }
        public string Display_url { get; set; }
        public Video_Info Video_info { get; set; }
        public Additional_Media_Info Additional_media_info { get; set; }
    }

    public class Sizes1
    {
        public Large1 Large { get; set; }
        public Thumb1 Thumb { get; set; }
        public Small1 Small { get; set; }
        public Medium3 Medium { get; set; }
    }

    public class Large1
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Thumb1
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Small1
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Medium3
    {
        public string W { get; set; }
        public string H { get; set; }
        public string Resize { get; set; }
    }

    public class Video_Info
    {
        public string[] Aspect_ratio { get; set; }
        public string Duration_millis { get; set; }
        public Variant[] Variants { get; set; }
    }

    public class Variant
    {
        public string Content_type { get; set; }
        public string Url { get; set; }
        public string Bitrate { get; set; }
    }

    public class Additional_Media_Info
    {
        public bool Monetizable { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Embeddable { get; set; }
        public Call_To_Actions Call_to_actions { get; set; }
    }

    public class Call_To_Actions
    {
        public Visit_Site Visit_site { get; set; }
    }

    public class Visit_Site
    {
        public string Url { get; set; }
    }



}
