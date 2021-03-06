﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TS3QueryLib.Core;
using TS3QueryLib.Core.Server;
using System.Diagnostics;
using System.IO;
using System.Net;
using YoutubePlaylists;
using YoutubeExtractor;
using System.Linq;

namespace TS3MusicBot
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        public AsyncTcpDispatcher QueryDispatcher; // Query object
        public QueryRunner QR; // used to run all the queries on the server

        public uint channelID; // used for storing musicbot client side ID
        public uint clientID; // get's client ID's for commands
        public string botLocation; // gets location of the bot

        public string targetHost; // target teamspeak host
        public ushort targetPort; // target teamspeak port
        public string username; // username for serverquery
        public string password; // password for serverquery
        public string uniqueID; // unique teamspeak ID

        public int retrySong;
        public bool repeatSong = false;

        private speech speaker;
        private playlist playlistRequest;
        private autoDJ autoPlayer;
        private Random Random;

        public string currentSongTitle = "Nothing playing!"; // will be used to store song titles
        public string currentSongURL = "Well, nothings playing.. So.. Nothing?"; // will be used to store song URL's

        public AxAXVLC.AxVLCPlugin2 playSongs; // VLC object reference for the one in the form
        public List<VideoInfo> songURL; // used to store songs
        public List<string> songLoc; // song names location
        public List<string> youtubePlaylistVideos;
        public showHelp helpMessages;

        public commands userCommands;

        public bool isStart = true; // boolean used for checking if the programs started

        public void getLogin()
        {
            Console.Clear();
            if (File.Exists("config.ini"))
            {
                string[] config = File.ReadAllLines("config.ini");
                targetHost = config[0];
                targetPort = ushort.Parse(config[1]);
                username = config[2];
                password = config[3];
                uniqueID = config[4];
                playlistRequest = new playlist(config[5]);
            }
            else
            {
                string[] config = new string[6];

                Console.WriteLine("Cannot find your config.ini, please enter your credentials and they will be saved for next time");
                Console.WriteLine("Server IP: ");
                config[0] = Console.ReadLine();
                Console.WriteLine("Serverquery port: ");
                config[1] = Console.ReadLine();
                Console.WriteLine("Serverquery username: ");
                config[2] = Console.ReadLine();
                Console.WriteLine("Serverquery password: ");
                config[3] = Console.ReadLine();
                Console.WriteLine("Musicbot unique ID (google to find out how to get this!)");
                config[4] = Console.ReadLine();
                Console.WriteLine("Your google API key: leave empty if you don't have one.");
                config[5] = Console.ReadLine();

                File.WriteAllLines("config.ini", config);
                Console.Clear();
                Console.WriteLine("Thank you, these will be stored inside config.ini for future boots! :)");
                Console.WriteLine("Booting..");
                getLogin();
            }
            Start();
        }

        public void Start()
        {
            //DoChatBackgroundWorker.RunWorkerAsync();
            DoBoot();
        }

        private void DoBoot()
        {
            QueryDispatcher = new AsyncTcpDispatcher(targetHost, targetPort); // initialises querydispatcher object
            QueryDispatcher.ReadyForSendingCommands += QueryDispatcher_ReadyForSendingCommands; // sets event for sending commands
            QueryDispatcher.ServerClosedConnection += QueryDispatcher_ServerClosedConnection; // sets closed connection event
            QueryDispatcher.Connect(); // connects to the server :)
        }

        private void QueryDispatcher_ServerClosedConnection(object sender, EventArgs e)
        {
            if (QR != null) QR.Dispose(); // if the queryrunner active, dispose of it
            QueryDispatcher = null; // nullify the connection to the querydispatcher, closing all events too
            QR = null; // nullify the queryrunner just to be safe
            Start(); // restart the server
            string logReportString = "Server connection closed. rebooting it. - " + System.DateTime.Now.ToString("h:mm:ss tt") + ":" + System.DateTime.Now.Date.ToString() + Environment.NewLine;
            logMessage(logReportString);
        }

        private void QueryDispatcher_ReadyForSendingCommands(object sender, EventArgs e)
        {
            QR = new QueryRunner(QueryDispatcher); // initialise queryrunner
            QR.Login(username, password); // login using given username and password
            if (playSongs == null) // if the player hasn't been initialised, this prevents the song skipping midway through if it's playing :)
            {
                initiatePlayer(); // initialise the music player in the form (if you have a different way than VLC please tell me! haha)
            }
            QR.SelectVirtualServerById(1); // select the first server, (alter if you have multiple servers)
            QR.SendRaw("clientupdate client_nickname=Jack's\\sMusicBot"); // sets the nickname for the music bot
            initiateCommands(); // run initialise commands method
            if (isStart) // if is starting for the first time
            {
                welcomeMessage(); // run the welcome message
                isStart = false; // disable the boolean, I know this can be done in the constructor but I cant be bothered to change this!
            }
        }

        public void welcomeMessage()
        {
            QR.SendGlobalMessage("Jack's Musicbot is currently located in: " + botLocation + "!"); // globally tell the server where the music bot is
            QR.SendGlobalMessage("Jack's Musicbot is now ready to take requests! ♥"); // Ready to take requests global message
            checkChannel.Enabled = true; // enables checking of music bot movements
        }

        public void initiateCommands()
        {
            var getMusicBotID = QR.GetClientIdsByUniqueId(uniqueID); // gets the client side music bot by the ID
            if (getMusicBotID.ResponseText.Contains("1281")) // if the music bot isnt there
            {
                string path = "";
                if (File.Exists(@"C:\Program Files(x86)\TeamSpeak 3 Client\ts3client_win32.exe"))
                {
                    path = @"C:\Program Files(x86)\TeamSpeak 3 Client\ts3client_win32.exe";
                }
                else if (File.Exists(@"C:\Program Files\TeamSpeak 3 Client\ts3client_win64.exe"))
                {
                    path = @"C:\Program Files\TeamSpeak 3 Client\ts3client_win64.exe";
                }
                else
                {
                    Console.WriteLine("Cannot find TS3Client location");
                    Console.ReadLine();
                    Application.Exit();
                    return;
                }
                Process.Start(path); // start music bot
                System.Threading.Thread.Sleep(6000); // sleep for 6 seconds, giving it time to boot up and auto join
                initiateCommands(); // recheck for the bot location
            }
            else
            {
                clientID = 0; // initialise the clientID as 0
                foreach (var ID in getMusicBotID) { clientID = ID.ClientId; } // gets the music bot client ID
                QR.Notifications.ChannelMessageReceived += getNotification; // sets notifications for the requests
                var getMusicBotLocation = QR.GetClientInfo(clientID); // gets client info about the music bot in other to get its location
                channelID = getMusicBotLocation.ChannelId; // sets global variable channelID to the bots location
                botLocation = QR.GetChannelInfo(channelID).Name; // gets the name of the channel the bot is located in
                var whoami = QR.SendWhoAmI(); // asks the server who it is, and stores all that in the whoami variable
                QR.MoveClient(whoami.ClientId, channelID); // moves the serverquery to the channel where the music bot is
                var registeredForNotifications = QR.RegisterForNotifications(TS3QueryLib.Core.Server.Entities.ServerNotifyRegisterEvent.TextChannel); // registers for notifications
                var checkChannel = QR.RegisterForNotifications(TS3QueryLib.Core.Server.Entities.ServerNotifyRegisterEvent.TextServer); // check channel notifications
                if (registeredForNotifications.IsErroneous) { Console.WriteLine(registeredForNotifications.ErrorMessage); } // if it fails to register (for some reason)
                else
                {
                    Console.Clear();
                    Console.WriteLine("Connected using user: " + username);
                    sqUsername.Text += username;
                    Console.WriteLine("Nickname: " + whoami.ClientNickName);
                    sqNickname.Text += whoami.ClientNickName;
                    Console.WriteLine("Connected on host: " + targetHost + ":" + targetPort);
                    serverHost.Text += targetHost + ":" + targetPort;
                    Console.WriteLine("Connected to server: " + QR.GetServerInfo().Name);
                    serverName.Text += QR.GetServerInfo().Name;
                    Console.WriteLine("Connected Room: " + botLocation);
                    channelName.Text += botLocation;
                    Console.WriteLine("Successfully registered for notifications!");
                }
            }
        }

        public void getNotification(object sender, TS3QueryLib.Core.Server.Notification.EventArgs.MessageReceivedEventArgs e)
        {
            string user = e.InvokerNickname; // gets the user who initated the request
            string message = e.Message; // gets the user's request
            uint clientid = e.InvokerClientId; // gets their client ID
            generateCommands(user, clientid, message); // passes info to method to sort out!
        }
        public void initiatePlayer()
        {

            playSongs = axVLCPlugin21; // sets playsongs reference to the VLC object
            playSongs.volume = 60; // sets audio volume to 100
            songURL = new List<VideoInfo> { }; // initialises song list
            songLoc = new List<string> { }; // initialises the song names to be stored
            youtubePlaylistVideos = new List<string> { };
            autoPlayer = new autoDJ();
            helpMessages = new showHelp();
            playSongs.video.fullscreen = false; // sets fullscreen to false
            playSongs.AutoLoop = false; // sets autoloop to false!
            userCommands = new commands();
            userCommands.updateCommands();
            playSongs.Visible = false;
            playSongs.CtlVisible = false;
            playSongs.SuspendLayout();
        }
        public void generateCommands(string user, uint clientID, string getCommand)
        {
            try
            {
                if (getCommand.Trim().IndexOf('!') == 0)
                {
                    string command = "";
                    if (getCommand.Trim().Contains(" ")) command = getCommand.Trim().Substring(0, getCommand.Trim().IndexOf(' ')); // if theres a space
                    else command = getCommand.Trim();

                    switch (command)
                    {
                        case "!play":
                            addToQueue(user, getCommand);
                            break;
                        case "!skip":
                            skipSong();
                            break;
                        case "!song":
                            currentSong();
                            break;
                        case "!blacklist":
                            if (isAdmin(clientID))
                            {
                                if (blacklist.addToBlacklist(user, getCommand))
                                {
                                    QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Video Blacklisted.");
                                }
                            }
                            else
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "You are not authorised to blacklist videos.");
                                return;
                            }
                            break;
                        case "!remove":
                            if (isAdmin(clientID))
                            {
                                if (blacklist.removeFromBlacklist(user, getCommand))
                                {
                                    QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Video removed from blacklist.");
                                }
                            }
                            else
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "You are not authorised to whitelist videos.");
                                return;
                            }
                            break;
                        case "!anthem":
                            addToQueue(null, "https://www.youtube.com/watch?v=HouWHLN9JYE");
                            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "All rise for our national anthem.");
                            break;
                        case "!help":
                            var userID = clientID;
                            foreach (string message in helpMessages.howToUse())
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Client, userID, message);
                            }
                            break;
                        case "!repeat":
                            repeat();
                            break;
                        case "!clear":
                            clearLists();
                            break;
                        case "!auto":
                            if (autoPlayer.Auto())
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Nightcore is autoplaying!");
                            }
                            else
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Nightcore stopped!");
                            }
                            break;
                        case "!gold":
                            wowgold goldPrice = new wowgold();
                            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Current price for 1 WoW token: " + goldPrice.getWoWGold());
                            break;
                        case "!say":
                            if (speaker == null)
                            {
                                speaker = new speech();
                            }

                            string speech = getCommand.ToLower();
                            speech = speech.Replace("!say", "").Trim();
                            speaker.textToSpeech(speech);
                            break;
                        case "!playlist":
                            try
                            {
                                youtubePlaylistVideos.Clear();
                                string playlistID = getCommand.Replace("!playlist", "").Trim();
                                if(playlistID.Trim() == "")
                                {
                                    return;
                                }
                                string[] IDs = playlistRequest.getVideos(playlistID);
                                foreach (string ID in IDs)
                                {
                                    youtubePlaylistVideos.Add("http://youtube.com/watch?v=" + ID);
                                }
                                if (playSongs.playlist.items.count == 0)
                                {
                                    getNextSong();
                                }
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Playlist added, to remove do !clear");
                            }
                            catch (Exception e)
                            {
                                logMessage(e.Message);
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "I've broken!");
                            }
                            break;
                        case "!bind":
                            string bindedcommand = getCommand.Replace("!bind", "").Trim();
                            string trigger = bindedcommand.Substring(0, bindedcommand.IndexOf(" "));
                            string thecommand = bindedcommand.Replace(trigger, "").Trim();
                            if(userCommands.AddCommand(trigger, thecommand) == false)
                            {
                                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Command could not be binded.");
                            }
                            break;
                        case "!flip":
                            if(Random == null)
                            {
                                Random = new Random();
                            }
                            string resp = Random.Next(2) == 1 ? "heads" : "tails";
                            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "A virtual coin has been flipped, and it landed on: " + resp);
                            break;
                        case "!define":
                            string term = getCommand.Substring(getCommand.IndexOf(' '));
                            string response = urbandefine.Response(term);
                            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, response);
                            break;                            
                        default:
                            string mycommand = userCommands.getCommand(command);
                            if (mycommand.Contains("</3")) QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, mycommand);
                            else generateCommands("MusicBot", QR.SendWhoAmI().ClientId, mycommand);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                logMessage(e.Message);
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "I've broken!");
            }
        }

        private void skipSong()
        {
            restartPlayer();
            if (repeatSong) // if repeat is enabled
            {
                repeat(); // disable repeat!
            }
            if (autoPlayer.autoplay)
            {
                autoPlayer.Auto();
            }
            getNextSong();
        }

        private void logMessage(string message)
        {
            File.AppendAllText("log.txt", message);
        }

        private bool isAdmin(uint clientID)
        {
            var client = QR.GetClientInfo(clientID);
            if (client.ServerGroups.Contains(6)) // 6 is the server admin group.
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public void addToQueue(string user, string getCommand)
        {
            if (autoPlayer.autoplay)
            {
                autoPlayer.Auto();
            }

            string vidURL = getCommand.Replace("!play ", ""); // replaces to make a user friendly string
            try
            {
                if (vidURL.Contains("?t="))
                {
                    vidURL = vidURL.Remove(vidURL.LastIndexOf('?'), vidURL.Length - vidURL.LastIndexOf('?'));
                }
                else if (vidURL.Contains("#t="))
                {
                    vidURL = vidURL.Remove(vidURL.LastIndexOf('#'), vidURL.Length - vidURL.LastIndexOf('#'));
                }
                if (blacklist.checkBlacklist(vidURL))
                {
                    QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Video is blacklisted."); // tell them songs already there
                    return;
                }
                var videos = DownloadUrlResolver.GetDownloadUrls(vidURL, true);
                videos = videos.Select(x => x).Where(x => x.VideoType == VideoType.Mp4 && x.AudioType != AudioType.Unknown).ToList();
                var video = videos.OrderBy(x => x.AudioBitrate).FirstOrDefault();
                if (songLoc.Contains(vidURL)) // if song is already in the playlist (to prevent spamming :P)
                {
                    QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Song is already in the queue!"); // tell them songs already there
                    return;
                }
                if (user != null)
                    QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, user + " has requested: " + video.Title); // tells the channel who requested what
                if (playSongs.playlist.items.count == 0) // if nothing is playing
                {
                    playNow(video); // play song now!
                    currentSongTitle = video.Title; // sets current song to video title
                    currentSongURL = vidURL; // sets current video url to video url
                }
                else
                {
                    songURL.Add(video); // add to song list
                    songLoc.Add(vidURL); // adds video to song list
                }
                retrySong = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Sorry, " + user + " but your request cannot be fulfilled! :(");
                string logReportString = user + " - " + getCommand + " - " + System.DateTime.Now.ToString("h:mm:ss tt") + ":" + System.DateTime.Now.Date.ToString() + Environment.NewLine;
                logMessage(logReportString);
                return;

            } // if theres an error, tell them it can't be played.
        }

        public void playNow(VideoInfo vid)
        {
            playSongs.playlist.add(vid.DownloadUrl); // add song to VLC
            playSongs.playlist.next(); // skip to the song added
            playSongs.playlist.play(); // play song!
            if (repeatSong == false)
            {
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Now playing: " + vid.Title); // tells the channel what's now playing
            }
        }

        public void playNextSong(int val)
        {
            try
            {
                restartPlayer();  // restarts the player, helps to combat errors that happen (AXVLC is buggy!)
                var song = songURL[val]; // sets song variable to contain all the information about the song selected
                currentSongURL = songLoc[val]; // sets the current song URL to the song URL saved
                playSongs.playlist.add(song.DownloadUrl); // adds the song URI to VLC
                playSongs.playlist.next(); // skips to the next song, simples
                playSongs.playlist.play(); // plays the next song!
                currentSongTitle = song.Title; // sets the current song title to the current song title
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Now playing: " + song.Title); // again, let's people know what's playing
                songURL.RemoveAt(val); // removes the currently played song to the list
                songLoc.RemoveAt(val); // removes the current song URL from the list
            }
            catch (Exception) { restartPlayer(); } // if something breaks, restart the player :P
        }

        public void currentSong()
        {
            string current = currentSongTitle; // gets current song
            string url = currentSongURL; // gets current url
            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Current song is: " + current); // tells channel current song title
            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Here's the YouTube URL: " + url); // tells channel current song url
        }

        public void restartPlayer()
        {
            playSongs.playlist.stop(); // stops currently playing
            playSongs.playlist.items.clear(); // clears the list incase somethings stayed in there (sneaky ninja videos!)
            if (playSongs.InvokeRequired)
            {
                playSongs.Invoke((MethodInvoker)(() => playSongs.Refresh()));
            }
            else
            {
                playSongs.Refresh(); // full refresh of the UI and properties
            }
            Console.Clear(); // clears console (trust me this makes it TONS easier to see)
        }

        public void getNextSong()
        {
            try
            {
                if (repeatSong == true) // if repeating is anabled
                {
                    restartPlayer(); // reset player 
                    addToQueue(null, currentSongURL); // rerun current song
                }
                else if (youtubePlaylistVideos.Count != 0)
                {
                    string nextSongFromPlaylist = youtubePlaylistVideos[0];
                    youtubePlaylistVideos.RemoveAt(0);
                    restartPlayer();
                    addToQueue(null, nextSongFromPlaylist);
                }
                else
                {
                    songURL.TrimExcess();
                    if (songURL.Count > 0)
                    {
                        playNextSong(0);
                    }


                }
            }
            catch (System.ArgumentOutOfRangeException) { restartPlayer(); } // if theres no songs left, restart the player
        }

        private void checkChannel_Tick(object sender, EventArgs e)
        {
            var getMusicBotLocation = QR.GetClientInfo(clientID); // gets the music bots location
            var newChannel = getMusicBotLocation.ChannelId; // gets the possible new channel location
            var whoami = QR.SendWhoAmI(); // gets the whois for the Serverquery location
            if (newChannel != whoami.ChannelId) // if the server query isn't in the same room as the bot
            {
                QR.MoveClient(whoami.ClientId, newChannel); // switch serverquery room
                channelID = newChannel; // set the channel ID to the new channel
                channelName.Text = "Channel name: " + botLocation;
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Musicbot is now listening to this channel!"); // tell the new channel the music bot is ready!
            }
        }

        private void encounteredError(object sender, EventArgs e)
        {
            string logReportString = e.ToString() + " - " + System.DateTime.Now.ToString("h:mm:ss tt") + ":" + System.DateTime.Now.Date.ToString() + Environment.NewLine;
            logMessage(logReportString);
            getNextSong();
        } // song has ended or VLC as errored, get the next song

        private void songFinished(object sender, EventArgs e)
        {
            restartPlayer();
            getNextSong();
        } // song has ended or VLC as errored, get the next song

        public void repeat()
        {
            if (repeatSong)
            {
                repeatSong = false;
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Repeat disabled!");
            }
            else
            {
                repeatSong = true;
                QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Repeat enabled for " + currentSongTitle + "!");
            }
        }

        public void clearLists()
        {
            songURL.Clear(); //clear song url's
            songLoc.Clear(); // clear song locations
            youtubePlaylistVideos.Clear();
            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Song list cleared!"); // tell users the list cleared! :P
        }

        private void checkForUpdates_Tick(object sender, EventArgs e)
        {
            using (WebClient wc = new WebClient())
            {

                try
                {
                    string s = wc.DownloadString("https://github.com/JackRyder/TS3MusikBot/commit/master");
                    int indexOf = s.IndexOf("<span class=\"sha user-select-contain\">");
                    s = s.Substring(indexOf, 200);
                    if (!File.Exists(@"latest.txt"))
                    {
                        File.WriteAllText("latest.txt", s);
                    }
                    else
                    {
                        if (File.ReadAllText("latest.txt") != s)
                        {
                            File.Delete("latest.txt");
                            QR.SendTextMessage(TS3QueryLib.Core.CommandHandling.MessageTarget.Channel, channelID, "Updating Jack's music bot to new version.");
                            if (autoPlayer.autoplay)
                            {
                                autoPlayer.Auto();
                            }
                            Process.Start("TS3MusikUpdater.exe");
                        }
                    }
                }
                catch (Exception ee)
                {
                    logMessage(ee.Message);
                }

            }
        }
    
        private void DoChatBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DoBoot();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            getLogin();
        }
    }
}
