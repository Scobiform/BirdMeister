namespace BirdMeister.Models
{
    public class TweetArchive
    {
        public Tweets[] AllTweets { get; set; }
    }

    public class Tweets
    {
        public Tweet tweet { get; set; }
    }

    public class Tweet
    {
        public bool retweeted { get; set; }
        public string source { get; set; }
        public Entities entities { get; set; }
        public string[] display_text_range { get; set; }
        public string favorite_count { get; set; }
        public string id_str { get; set; }
        public bool truncated { get; set; }
        public string retweet_count { get; set; }
        public string id { get; set; }
        public bool possibly_sensitive { get; set; }
        public string created_at { get; set; }
        public bool favorited { get; set; }
        public string full_text { get; set; }
        public string lang { get; set; }
        public string in_reply_to_user_id { get; set; }
        public string in_reply_to_screen_name { get; set; }
        public string in_reply_to_user_id_str { get; set; }
        public string in_reply_to_status_id_str { get; set; }
        public string in_reply_to_status_id { get; set; }
        public Extended_Entities extended_entities { get; set; }
        public bool withheld_copyright { get; set; }
        public string[] withheld_in_countries { get; set; }
    }

    public class Entities
    {
        public Hashtag[] hashtags { get; set; }
        public Symbol[] symbols { get; set; }
        public User_Mentions[] user_mentions { get; set; }
        public Url[] urls { get; set; }
        public Medium[] media { get; set; }
    }

    public class Hashtag
    {
        public string text { get; set; }
        public string[] indices { get; set; }
    }

    public class Symbol
    {
        public string text { get; set; }
        public string[] indices { get; set; }
    }

    public class User_Mentions
    {
        public string name { get; set; }
        public string screen_name { get; set; }
        public string[] indices { get; set; }
        public string id_str { get; set; }
        public string id { get; set; }
    }

    public class Url
    {
        public string urlstring { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public string[] indices { get; set; }
    }

    public class Medium
    {
        public string expanded_url { get; set; }
        public string source_status_id { get; set; }
        public string[] indices { get; set; }
        public string url { get; set; }
        public string media_url { get; set; }
        public string id_str { get; set; }
        public string source_user_id { get; set; }
        public string id { get; set; }
        public string media_url_https { get; set; }
        public string source_user_id_str { get; set; }
        public Sizes sizes { get; set; }
        public string type { get; set; }
        public string source_status_id_str { get; set; }
        public string display_url { get; set; }
    }

    public class Sizes
    {
        public Large large { get; set; }
        public Thumb thumb { get; set; }
        public Small small { get; set; }
        public Medium1 medium { get; set; }
    }

    public class Large
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Thumb
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Small
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Medium1
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Extended_Entities
    {
        public Medium2[] media { get; set; }
    }

    public class Medium2
    {
        public string expanded_url { get; set; }
        public string source_status_id { get; set; }
        public string[] indices { get; set; }
        public string url { get; set; }
        public string media_url { get; set; }
        public string id_str { get; set; }
        public string source_user_id { get; set; }
        public string id { get; set; }
        public string media_url_https { get; set; }
        public string source_user_id_str { get; set; }
        public Sizes1 sizes { get; set; }
        public string type { get; set; }
        public string source_status_id_str { get; set; }
        public string display_url { get; set; }
        public Video_Info video_info { get; set; }
        public Additional_Media_Info additional_media_info { get; set; }
    }

    public class Sizes1
    {
        public Large1 large { get; set; }
        public Thumb1 thumb { get; set; }
        public Small1 small { get; set; }
        public Medium3 medium { get; set; }
    }

    public class Large1
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Thumb1
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Small1
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Medium3
    {
        public string w { get; set; }
        public string h { get; set; }
        public string resize { get; set; }
    }

    public class Video_Info
    {
        public string[] aspect_ratio { get; set; }
        public string duration_millis { get; set; }
        public Variant[] variants { get; set; }
    }

    public class Variant
    {
        public string content_type { get; set; }
        public string url { get; set; }
        public string bitrate { get; set; }
    }

    public class Additional_Media_Info
    {
        public bool monetizable { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool embeddable { get; set; }
        public Call_To_Actions call_to_actions { get; set; }
    }

    public class Call_To_Actions
    {
        public Visit_Site visit_site { get; set; }
    }

    public class Visit_Site
    {
        public string url { get; set; }
    }



}
