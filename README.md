<img align="center" width="100" height="100" src="https://github.com/Scobiform/BirdMeister/blob/master/birdmeister.png">

# BirdMeister Version 0.1.0
Little .NET 5.0 CLI Tool for Twitter to automate Deleting Tweets, Lists, User friend Ids and start Filtered Streams. Using Tweetinvi by https://github.com/linvi

«It is a good start for people that get harassed by groups of people on Twitter. Just grab all friend Ids and block all users with your account.»

# Install
1. Register an APP at Twitter Developers (https://developer.twitter.com/)
2. Put your APIKey and Secret in the Seceret manager in Visual Studio

- dotnet user-secrets set "APIKey" "12345"
- dotnet user-secrets set "APISecret" "12345"

# Tweet Database
For the Tweet Database download your Twitter profile archive and drop the including tweet.js into the Data folder. 
Do not forget to make the json a valid json file. (will automate that later)

# Filtered Stream
The Tweets will open up in your Standard Browser

