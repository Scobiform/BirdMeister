<img width="160" height="160" src="https://github.com/Scobiform/BirdMeister/blob/master/birdmeister.png" style="float:left">

# BirdMeister Version 0.2.0
Little .NET 6.0 CLI Tool for Twitter to automate deleting Tweets, Lists, User friend Ids and start Filtered Streams. Using Tweetinvi by https://github.com/linvi

« <i>It is a good start for people that get harassed by groups of people on Twitter. Just grab all friend Ids and block all users with your account.</i> »

# Install
1. Register an APP at Twitter Developers (https://developer.twitter.com/)
2. Put your APIKey and Secret in the Seceret manager in Visual Studio

- dotnet user-secrets set "APIKey" "12345"
- dotnet user-secrets set "APISecret" "12345"

# Tweet Database
For the Tweet Database download your Twitter profile archive and drop the including tweet.js into the Data folder. 
Do not forget to make the json a valid json file. Just delete ``window.YTD.tweet.part0 =``

# Filtered Stream
Currently will only retweet tweets with the given keywords - You can add tracks - stop and restart the stream. 

# Screenshots
<img align="center" width="100" height="100" src="https://github.com/Scobiform/BirdMeister/blob/master/menu.png">
