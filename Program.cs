using BirdMeister.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.AspNet;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Parameters;
using System.Reflection;

namespace BirdMeister
{
    class Program
    {
        static int indexMainMenu = 0;
        static TwitterClient _userClient;
        static ITwitterList[] _userLists;
        static long[] _friendIds;
        static int _membersAddedCount;
        static string _tweetsArchive;
        static IFilteredStream _stream;
        public Program(TwitterClient userclient, ITwitterList[] userLists, long[] friendIds, int membersAddedCount, 
            string tweetsArchive, IFilteredStream stream)
        {
            _userClient = userclient;
            _userLists = userLists;
            _friendIds = friendIds;
            _membersAddedCount = membersAddedCount;
            _tweetsArchive = tweetsArchive;
            _stream = stream;
        }
        static async Task Main(string[] args)
        {

            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Plugins.Add<AspNetPlugin>();

            // Create Data Folder
            await CreateDirectory();

            try
            {
                // Authenticate Twitter
                await AuthTwitter();

                Console.Clear();

                var user = await _userClient.Users.GetAuthenticatedUserAsync();

                // Draw Menu
                List<string> menuItems = new()
                {
                    "┈----------------------Lists",
                    "┈GetUserLists",
                    "┈CopyUserList",
                    "┈AddIdsToList",
                    "┈DeleteIdFromList",
                    "┈----------------------Users",
                    "┈GetUserDetails",
                    "┈GetUserFriendIds",
                    "┈BlockIds",
                    "┈UnblockAllIds",
                    "┈FollowIds",
                    "┈---------------------Tweets",
                    "┈CreateTweetDatabase",
                    "┈DeleteTweets",
                    "┈--------------------Account",
                    "┈DeleteTimeline",
                    "┈UnFollowAll",
                    "┈UnfavTweets",
                    "┈--------------------Streams",
                    "┈StartFilteredKeyWordsStream",
                    "┈Exit"
                };

                Console.CursorVisible = false;
                while (true)
                {
                    Console.WriteLine("┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈");
                    Console.WriteLine("┈BirdMeister                               ");
                    Console.WriteLine("┈Logged in as: " + user.ScreenName);
                    Console.WriteLine("┈Followers count " + user.FollowersCount);
                    Console.WriteLine("┈Friends count " + user.FriendsCount);
                    Console.WriteLine("┈Favourites count " + user.FavoritesCount);
                    Console.WriteLine("┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈\n");

                    Console.WriteLine("┈Menu:");
                    string selectedMenuItem = DrawMainMenu(menuItems);

                    switch (selectedMenuItem)
                    {
                        case "┈GetUserLists":
                            Console.WriteLine("From which username you want to grab all lists?");
                            var screenName = Console.ReadLine();
                            Parallel.Invoke(async () => await GetUsersLists(screenName));
                            break;

                        case "┈CopyUserList":
                            Console.WriteLine("Which lists you want to copy to your account?");
                            var count = 0;

                            foreach (var list in _userLists)
                            {
                                Console.WriteLine("{0} {1} ", count, list.Name);
                                count++;
                            }

                            var listNumber = Console.ReadLine();

                            Parallel.Invoke(async () => await CopyUsersList(_userLists[Convert.ToInt32(listNumber)]));
                            break;
                        case "┈GetUserDetails":
                            Console.WriteLine("From which username you want to grab all lists?");
                            var screenNameUser = Console.ReadLine();
                            Parallel.Invoke(async () => await GetUserDetails(screenNameUser));
                            break;

                        case "┈GetUserFriendIds":
                            Console.WriteLine("From which username you want to grab all friends?");
                            var screenNameUserFriendsIds = Console.ReadLine();
                            Parallel.Invoke(async () => await GetUserFriends(screenNameUserFriendsIds));
                            break;

                        case "┈AddIdsToList":
                            Parallel.Invoke(async () => await AddIdsToList());
                            break;

                        case "┈DeleteIdFromList":
                            Console.WriteLine("Which Id do you want to delete from the list?");
                            var userId = Console.ReadLine();
                            Parallel.Invoke(async () => await DeleteIdFromList(userId));
                            break;

                        case "┈BlockIds":
                            Parallel.Invoke(async () => await BlockIds());
                            break;

                        case "┈UnblockAllIds":
                            Parallel.Invoke(async () => await UnblockAllIds());
                            break;

                        case "┈FollowIds":
                            Parallel.Invoke(async () => await FollowIds());
                            break;

                        case "┈CreateTweetDatabase":
                            Parallel.Invoke(async () => await CreateTweetDatabase());
                            break;

                        case "┈DeleteTweets":
                            Parallel.Invoke(async () => await DeleteTweets());
                            break;

                        case "┈UnfavTweets":
                            Parallel.Invoke(async () => await UnfavTweets());
                            break;

                        case "┈UnFollowAll":
                            Parallel.Invoke(async () => await UnFollowAll());
                            break;

                        case "┈DeleteTimeline":
                            Parallel.Invoke(async () => await DeleteTimeline());
                            break;

                        case "┈StartFilteredKeyWordsStream":
                            Console.WriteLine("Please enter the keyword you want to stream...");
                            var keyword = Console.ReadLine();
                            Console.Clear();
                            // Start filtered stream as parallel
                            Parallel.Invoke(async () => await StartFilteredStream(keyword));
                            break;

                        case "-Exit":
                        default:
                            continue;           
                         }
                }
            }
            catch (TwitterAuthException ex)
            {
                Console.WriteLine("TwitterAuthException was thrown: " + ex);
            }
        }
        static string DrawMainMenu(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i == indexMainMenu)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(items[i]);
                }
                else
                {
                    Console.WriteLine(items[i]);
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo ckey = Console.ReadKey();

            if (ckey.Key == ConsoleKey.DownArrow)
            {
                if (indexMainMenu == items.Count - 1) { }
                else { indexMainMenu++; }
            }
            else if (ckey.Key == ConsoleKey.UpArrow)
            {
                if (indexMainMenu <= 0) { }
                else { indexMainMenu--; }
            }
            else if (ckey.Key == ConsoleKey.LeftArrow)
            {
                Console.Clear();
            }
            else if (ckey.Key == ConsoleKey.RightArrow)
            {
                Console.Clear();
            }
            else if (ckey.Key == ConsoleKey.Enter)
            {
                return items[indexMainMenu];
            }
            else
            {
                return "";
            }

            Console.Clear();
            return "";
        }
        static async Task CreateDirectory()
        {
            string path = "Data";

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                await Task.Delay(1);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }
        static async Task AuthTwitter()
        {
            var configuration = new ConfigurationBuilder()
               .AddUserSecrets(Assembly.GetExecutingAssembly())
               .Build();

            var APIKey = configuration["APIKey"];
            var APISecret = configuration["APISecret"];

            Console.WriteLine($"The secret value is: {APIKey}");
            Console.WriteLine($"The secret value is: {APISecret}");

            var appClient = new TwitterClient(APIKey, APISecret);

            // Start the authentication process
            var authenticationRequest = await appClient.Auth.RequestAuthenticationUrlAsync();

            // Go to the URL so that Twitter authenticates the user and gives him a PIN code.
            Process.Start(new ProcessStartInfo(authenticationRequest.AuthorizationURL)
            {
                UseShellExecute = true
            });

            // Ask the user to enter the pin code given by Twitter
            Console.WriteLine("Please enter the code and press enter.");
            var pinCode = Console.ReadLine();

            // With this pin code it is now possible to get the credentials back from Twitter
            var userCredentials = await appClient.Auth.RequestCredentialsFromVerifierCodeAsync(pinCode, authenticationRequest);

            // You can now save those credentials or use them as followed
            var userClient = new TwitterClient(userCredentials);
            var user = await userClient.Users.GetAuthenticatedUserAsync();

            Console.WriteLine("Congratulation you have authenticated the user: " + user);

            _userClient = userClient;

        }
        static async Task GetUserDetails(string screenNameUser)
        {
            var userId = _userClient.Users.GetUserAsync(screenNameUser);

            Console.WriteLine("Getting user details..."
                +"\n┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈"
                + "\n Id: \t" + userId.Result.Id
                + "\n Screenname: \t" + userId.Result.ScreenName
                + "\n Name: \t" + userId.Result.Name
                + "\n Location: \t" + userId.Result.Location
                + "\n Created at: \t" + userId.Result.CreatedAt
                + "\n┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈"
                + "\n Description: \t" + userId.Result.Description
                + "\n┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈"
                + "\n Favorites: \t" + userId.Result.FavoritesCount
                + "\n Followers: \t" + userId.Result.FollowersCount
                + "\n Friends: \t" + userId.Result.FriendsCount
                + "\n Listed: \t" + userId.Result.ListedCount
                );

            Console.WriteLine("Hit enter to continue...");
            Console.ReadKey();
            await Task.CompletedTask.ConfigureAwait(true);
        }
        static async Task GetUsersLists(string screenName)
        {
            var userId = _userClient.Users.GetUserAsync(screenName);

            var userLists = await _userClient.Lists.GetListsOwnedByUserAsync(userId.Result.Id);

            _userLists = userLists;

            foreach(var list in userLists)
            {
                Console.WriteLine(list.Name);
            }
        }
        static async Task CopyUsersList(ITwitterList list)
        {
            var getListMembers = await _userClient.Lists.GetMembersOfListAsync(list.Id);

            var listName = list.Id;

            // Check if tweetids.txt is already existing
            if (File.Exists("Data/" + listName.ToString() + ".txt"))
                {
                    Console.WriteLine(">>> Skipping database creation because file already exists");
                }
                else
                {
                    // else gett all tweetids and store them in

                    using (StreamWriter writer = new StreamWriter("Data/" + listName.ToString() + ".txt"))
                    {
                        foreach (var member in getListMembers)
                        {
                            Console.WriteLine(">>> Writing " + member.Id );
                            writer.WriteLine(member.Id);
                        }
                    }
                    await Task.Delay(500);
                }
        }
        static async Task GetUserFriends(string screenName)
        {
            var userId = _userClient.Users.GetUserAsync(screenName);

            var friendIds = await _userClient.Users.GetFriendIdsAsync(userId.Result.Id);

            _friendIds = friendIds;

            if (File.Exists("Data/" + screenName.ToString() + ".txt"))
            {
                Console.WriteLine(">>> Skipping friends database creation because file already exists");
            }
            else
            {
                // else get all tweetids and store them in
                using (StreamWriter writer = new($"Data/"+screenName.ToString() + ".txt"))
                {
                    foreach (var member in friendIds)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        Console.WriteLine(">>> Writing " + member);
                        writer.WriteLine(member);
                    }
                }

                Console.WriteLine(">>> Succesfully created friendId database of user: " + screenName);

                await Task.Delay(500);
            }
        }
        static async Task BlockIds()
        {
            var sourceDir = @"Data\";

            Console.WriteLine("Which Id database do you want to import, \n" +
                "please type the filename (without .txt) and hit ENTER.\n ");

            var fileName = Console.ReadLine();

            var userIds = File.ReadAllLines(sourceDir + fileName + ".txt");

            if (File.Exists(sourceDir + fileName + "_backup.txt"))
            {
                Console.WriteLine(">>> Skipping backup creation because file already exists...");
            }
            else
            {
                Console.WriteLine(">>> Creating backup in data folder...");
                File.Copy(Path.Combine(sourceDir, fileName + ".txt"), Path.Combine(sourceDir, fileName + "_backup.txt"));
            }

            // Random interval for Sleep Thread
            var rand = new Random();

            foreach (string user in userIds)
            {
                // Make today Database entry
                // Rename membersAddedCount to the unique Indetifier
                Console.WriteLine("Blocked users today: " + _membersAddedCount);
                if (_membersAddedCount >= 850)
                {
                    Console.WriteLine("You will hit account limits for adding new members soon. Wait for 12 hours. \n#" +
                        "Hit Enter to continue...");
                    Console.ReadKey();
                }
                else
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(rand.Next(5, 10)));
                        Console.WriteLine("Adding {0} to the list ", user);
                        await _userClient.Users.BlockUserAsync(Convert.ToInt64(user));

                        userIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", userIds);
                    }
                    catch (TwitterException ex)
                    {
                        Console.WriteLine("TwitterException: " + ex);

                        userIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", userIds);

                    }
                    _membersAddedCount++;
                }

            }
            await Task.Delay(5000);
        }
        static async Task UnblockAllIds()
        {

            // Get currently blocked Ids
            Console.WriteLine("Getting all blocked users...");
            var blocked = await _userClient.Users.GetBlockedUsersAsync();
            Console.WriteLine("Trying to unblock all users that were found...");
            foreach (var user in blocked)
            {
                Console.WriteLine("Unblocking user with the id: " + user.Id);
                await _userClient.Users.UnblockUserAsync(user.Id);
                Console.WriteLine("Waiting for 20 Seconds because of rate limits ");
                await Task.Delay(TimeSpan.FromSeconds(20));
            }
        }
        static async Task FollowIds()
        {
            var sourceDir = @"Data\";

            Console.WriteLine("Which Id database do you want to import, \n" +
                "please type the filename (without .txt) and hit ENTER.\n ");

            var fileName = Console.ReadLine();

            var userIds = File.ReadAllLines(sourceDir + fileName + ".txt");

            if (File.Exists(sourceDir + fileName + "_backup.txt"))
            {
                Console.WriteLine(">>> Skipping backup creation because file already exists...");
            }
            else
            {
                Console.WriteLine(">>> Creating backup in data folder...");
                File.Copy(Path.Combine(sourceDir, fileName + ".txt"), Path.Combine(sourceDir, fileName + "_backup.txt"));
            }

            // Random interval
            var rand = new Random();

            foreach (string user in userIds)
            {
                // Make today Database entry
                // Rename membersAddedCount to the unique Indetifier
                Console.WriteLine("Added users today: " + _membersAddedCount);
                if (_membersAddedCount >= 850)
                {
                    Console.WriteLine("You will hit account limits for adding new members soon. Wait for 12 hours. \n#" +
                        "Hit Enter to continue...");
                    Console.ReadKey();
                }
                else
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(rand.Next(5, 10)));
                        Console.WriteLine("Followed {0} ", user);
                        await _userClient.Users.FollowUserAsync(Convert.ToInt64(user));

                        userIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", userIds);
                    }
                    catch (TwitterException ex)
                    {
                        Console.WriteLine("TwitterException: " + ex);

                        userIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", userIds);

                    }
                    _membersAddedCount++;
                }

            }
            await Task.Delay(5000);
        }
        static async Task AddIdsToList()
        {
            Console.WriteLine("Please provide the userlist ID to which you want to import the user ids to");

            var listId = Console.ReadLine();
            var sourceDir = @"Data\";

            Console.WriteLine("Which Id database do you want to import, \n" +
                "please type the filename (without .txt) and hit ENTER.\n ");

            var fileName = Console.ReadLine();

            var memberIds = File.ReadAllLines(sourceDir + fileName +".txt");

            if (File.Exists(sourceDir + fileName + "_backup.txt"))
            {
                Console.WriteLine(">>> Skipping backup creation because file already exists...");
            }
            else
            {
                Console.WriteLine(">>> Creating backup in data folder...");
                File.Copy(Path.Combine(sourceDir, fileName + ".txt"), Path.Combine(sourceDir, fileName + "_backup.txt"));
            }

            // Random interval for Task Delay
            var rand = new Random();

            foreach (string member in memberIds)
            {
                // Make today Database entry
                Console.WriteLine("Added Members today: " + _membersAddedCount );
                if (_membersAddedCount >= 850)
                {
                    Console.WriteLine("You will hit account limits for adding new members soon. Wait for 12 hours. \n#" +
                        "Hit Enter to continue...");
                    Console.ReadKey();
                }
                else
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(rand.Next(5, 10)));
                        Console.WriteLine("Adding {0} to the list ", member);
                        await _userClient.Lists.AddMemberToListAsync(Convert.ToInt64(listId), Convert.ToInt64(member));

                        memberIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", memberIds);
                    }
                    catch (TwitterException ex)
                    {
                        Console.WriteLine("TwitterException: " + ex);

                        memberIds = File.ReadAllLines(sourceDir + fileName + ".txt").Skip(1).ToArray();
                        File.WriteAllLines(sourceDir + fileName + ".txt", memberIds);

                    }
                    _membersAddedCount++;
                }
            }
            await Task.Delay(5000);
        }
        static async Task DeleteIdFromList(string Id)
        {
            Console.WriteLine("Please provide the userlist ID from which you want to delete the user Ids");

            var listId = Console.ReadLine();

            Console.WriteLine("Trying to delete the Id");
            await _userClient.Lists.RemoveMemberFromListAsync(Convert.ToInt64(listId), Convert.ToInt64(Id));
            Console.WriteLine("Deleted...");
        }
        static async Task StartFilteredStream(string keyword)
        {
            // Currently it is possible to start 2 streams per user.
            try
            {
                TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
                {
                    // lets delay all operations from this client by 2 seconds
                    Task.Delay(TimeSpan.FromSeconds(5));
                };

                // Waiting for rate limits
                TweetinviEvents.WaitingForRateLimit += (sender, args) =>
                {
                    Console.WriteLine($"\n Waiting for rate limits... ");
                    Task.Delay(TimeSpan.FromHours(3));
                };

                // subscribe to application level events
                TweetinviEvents.BeforeExecutingRequest += (sender, args) =>
                {
                    // application level logging
                    Console.WriteLine($"\n >>> Event: " + args.Url + "\n");
                };

                // For a client to be included in the application events you will need to subscribe to this client's events
                TweetinviEvents.SubscribeToClientEvents(_userClient);

                // Start Stream
                var stream = _userClient.Streams.CreateFilteredStream();

                // Add keyword
                stream.AddTrack(keyword);
                stream.AddLanguageFilter("en");

                // Only match the addfollows
                stream.MatchOn = MatchOn.HashTagEntities;

                // Get notfified about shutdown of the stream
                stream.StallWarnings = true;

                // Filterlevel of sensitive tweets
                stream.FilterLevel = StreamFilterLevel.Low;

                stream.KeepAliveReceived += async (sender, args) =>
                {
                    var streamstate = stream.StreamState;
                    Console.WriteLine(streamstate.ToString());
                    await Task.Delay(1);
                };

                stream.LimitReached += async (sender, eventReceived) =>
                {
                    Console.WriteLine("===========> Stream Warning... " + eventReceived.NumberOfTweetsNotReceived);

                    await Task.Delay(1);
                };

                stream.WarningFallingBehindDetected += async (sender, args) =>
                {
                    Console.WriteLine($">_ Warning falling behind...");
                    await Task.Delay(1000).ConfigureAwait(true);
                };

                stream.UnmanagedEventReceived += async (sender, args) =>
                {
                    Console.WriteLine($">_ Unmanged Event...");
                    await Task.Delay(1000).ConfigureAwait(true);
                };

                stream.LimitReached += async (sender, args) =>
                {
                    Console.WriteLine($">_ Limit reached...");
                    await Task.Delay(1000).ConfigureAwait(true);
                };

                stream.DisconnectMessageReceived += async (sender, args) =>
                {
                    Console.WriteLine($">_ Stream disconnected...");
                    stream.Stop();
                    Task.Delay(20000).Wait();
                    await stream.StartMatchingAllConditionsAsync().ConfigureAwait(true);

                };

                stream.NonMatchingTweetReceived += async (sender, args) =>
                {
                    var tweet = args.Tweet;

                    Console.WriteLine($"\n >>> Non matching tweet received... " + "" + tweet.Id);

                    await Task.Delay(TimeSpan.FromMinutes(1));

                };

                stream.MatchingTweetReceived += async (sender, args) =>
                {
                    var tweet = args.Tweet;
                    var hashTagCount = tweet.Entities.Hashtags.Count;

                    if (args.MatchOn == stream.MatchOn)
                    {
                        if (tweet.IsRetweet == true)
                        {
                            Console.WriteLine($"\n >>> is retweet... " + "" + tweet.Id);
                        }
                        else if (hashTagCount > 3)
                        {
                            Console.WriteLine($"\n >>> banned for hashtagcount... " + "" + tweet.Id);
                        }
                        else if (tweet.Media.Count >= 1)
                        {
                            Console.WriteLine("Like and retweet tweet ID " + tweet.Id);

                            await _userClient.Tweets.FavoriteTweetAsync(tweet);
                            await _userClient.Tweets.PublishRetweetAsync(tweet);
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(60));

                };
                await stream.StartMatchingAllConditionsAsync();
            }
            catch (TwitterException ex)
            {
                Console.WriteLine("TwitetrException: " + ex);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"\n>>> Socket Exception... " + ex);
            }
            catch (EndOfStreamException ex)
            {
                Console.WriteLine($"\n>>> Unexpected end of stream..." + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"<_ " + ex);
            }
        }
        static async Task CreateTweetDatabase()
        {
            Console.WriteLine("Please save your tweet.js from your downloaded Twitter archive to the Data folder and hit [ENTER]");
            Console.ReadKey();

            // Check if tweetids.txt is already existing
            if (File.Exists("Data/tweetids.txt"))
            {
                Console.WriteLine(">>> Skipping database creation because file already exists");
            }
            else
            {
                // else get all tweetids and store them in

                // Getting all tweets from tweet.js
                Console.WriteLine(">>> Getting Tweets from tweet.js now");

                _tweetsArchive = File.ReadAllText("Data/tweet.js");
                var TweetArchive = JsonSerializer.Deserialize<List<Tweets>>(_tweetsArchive);

                using (StreamWriter writer = new("Data/tweetids.txt"))
                {
                    foreach (var tweets in TweetArchive)
                    {
                        Console.WriteLine(">>> Writing " + tweets.Tweet.Id + " to tweetids.txt");
                        writer.WriteLine(tweets.Tweet.Id);
                    }
                }
                await Task.Delay(500);
            }
        }
        static async Task DeleteTweets()
        {

            // if file not existing get the last tweets from the timeline...
            // needs reWork
            //

            var orderTweets = File.ReadAllLines("Data/tweetids.txt");

            foreach (string tweetid in orderTweets)
            {
                try
                {
                    Console.WriteLine("\n >>> Deleting Tweet with Id: " + tweetid);

                    
                    var tweet = _userClient.Tweets.GetTweetAsync(Convert.ToInt64(tweetid));

                    if (tweet.Result.IsRetweet)
                    {
                        Console.WriteLine("Tweet is a retweet");
                        // Destroying retweet
                        Console.WriteLine(">>> trying to destroy retweet");
                        await _userClient.Tweets.DestroyRetweetAsync(Convert.ToInt64(tweetid));
                        Console.WriteLine(">>> Success... ");
                    }
                    else
                    {
                        // Destroying Tweet
                        Console.WriteLine(">>> trying to destroy Tweet");
                        await _userClient.Tweets.DestroyTweetAsync(Convert.ToInt64(tweetid));
                        Console.WriteLine(">>> Success... ");
                    }
                    // Updating tweet database
                    Console.WriteLine(">>> Updating the tweet database... ");
                    orderTweets = File.ReadAllLines("Data/tweetids.txt").Skip(1).ToArray();
                    File.WriteAllLines("Data/tweetids.txt", orderTweets);

                    // Wait for N seconds for rate limits
                    Console.WriteLine(">>> Waiting for 15 seconds for rate limits");
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                catch(TwitterException ex)
                {
                    Console.WriteLine("TwitetrException " + ex);
                }
                catch(ArgumentNullException ex)
                {
                    Console.WriteLine("ArgumentNullException " + ex);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception " + ex);

                }

            }

        }
        static async Task UnfavTweets()
        {
            var user = _userClient.Users.GetAuthenticatedUserAsync().Result;
            var favedTweets = await _userClient.Tweets.GetUserFavoriteTweetsAsync(user.Id);

            foreach(var tweet in favedTweets)
            {
                Console.WriteLine("Unfaving Tweet: " + tweet.Id);
                await _userClient.Tweets.UnfavoriteTweetAsync(tweet);

                await Task.Delay(TimeSpan.FromSeconds(15));
            }

            await Task.Delay(100);
        }
        static async Task UnFollowAll()
        {
            
            // Get authenticated user => Move to global 
            var authenticatedUser = await _userClient.Users.GetAuthenticatedUserAsync();

            // Get all friend ids
            var friendsDatabase = _userClient.Users.GetFriendsAsync(authenticatedUser.ScreenName);

            foreach (var friend in friendsDatabase.Result)
            {
                Console.WriteLine(">>> Unfollow friend with the Id " + friend.Id);
                await _userClient.Users.UnfollowUserAsync(friend.Id);

                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            Console.WriteLine("\n>>> Unfollowed all fetched friend Ids");
        }
        static async Task DeleteTimeline()
        {
            var user = await _userClient.Users.GetAuthenticatedUserAsync();

            var timeline = await _userClient.Timelines.GetHomeTimelineAsync();

            foreach(var tweet in timeline)
            {

                if (!tweet.IsRetweet)
                {
                    Console.WriteLine("Deleteing Tweet" + tweet.Id);
                    await _userClient.Tweets.DestroyTweetAsync(tweet.Id);
                }
                else
                {
                    Console.WriteLine("Deleteing Retweet" + tweet.Id);
                    await _userClient.Tweets.DestroyRetweetAsync(tweet.Id);
                }
            }
        }
    }
}
