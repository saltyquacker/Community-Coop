using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Globalization;
using System.IO;
using System.ComponentModel;
using System.Net.Sockets;
using System;
public class TwitchChat : MonoBehaviour
{
    //Script handles connection for the entirety of the game
    //COMMANDS:
    //!hatch - Spawns a chicken based on user
    //!COLLECT - Collects eggs from coop
    //!SELL - Sells collected eggs
    //!CULL - Kills one chicken at random!
    //!UPGRADE COOP - Upgrades coop? hold more eggs, change sprite
    private ChickenBuilder player = new ChickenBuilder();
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    public GameObject chicken;
    public int counter;
    public DateTime time;
    public TimeSpan differenceTime;
    //public List<ChickenBuilder> roster = new List<ChickenBuilder>();
    public GameObject viewer;
    //Changed these values to private since no one needs to know about them!
    private string username, password, channelName; //Get the password from https://twitchapps.com/tmi
    public Text chatBox;
    //Local list to store usernames
    public List<String> chickNames = new List<String>();

    public AudioSource audio;
    public AudioClip levelupRoo;
    public AudioClip levelupHen;
    public AudioClip kaching;
    // Start is called before the first frame update
    void Start()
    {
        //On load scene, get global variables saved from main menu
        username = GlobalVar.usernameGlobal;
        channelName = GlobalVar.channelNameGlobal;
        password = GlobalVar.passwordGlobal;
        Connect();
    }

    // Update is called once per frame
    void Update()
    {
        time = DateTime.Now;
        //Connect();
        if (GlobalVar.eventlog.Count > 1)
        {
            DateTime laststamp = DateTime.Parse(GlobalVar.eventlog[GlobalVar.eventlog.Count-1].timestamp);
            differenceTime = time - laststamp;
            //Debug.Log(differenceTime.Seconds.ToString());
        }
        //After 30 seconds of innactivity, its assumed that the client disconnected
        if (differenceTime.Seconds == 10)
        {
            GlobalVar.connected = false;
            //Debug.Log("SECONDS IS 30!");
            Connect();
            //GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "reconnecting...", viewername = "", quantity = 0, price = 0 });

            

        }
       
        if (!twitchClient.Connected)
        {
            GlobalVar.connected = false;
            print("Not Connected. Attempting to Connect...");
            //GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "disconnect", viewername = "", quantity = 0, price = 0});

            Connect();
            //print("Connected.");
        }
        else {
            GlobalVar.connected = true;
            
        }
     
        ReadChat();
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        print("PASS " + password);
        writer.WriteLine("NICK " + username);
        print("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        print("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        print("JOIN #" + channelName);
        writer.Flush();
        print("Trying to Connect...");
        GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "reconnecting...", viewername = "", quantity = 0, price = 0 });


    }

    private void ReadChat()
    {
        
        //print(twitchClient.Available);
        
        if (twitchClient.Available > 0)
        {
            //print("Available...");
            var message = reader.ReadLine();
            print(message);
            //Debug.Log(message);
            if (message.Contains("PRIVMSG"))
            {
                if (message.Contains("GLHF!"))
                {
                    GlobalVar.connected = true;
                }
                
                //Get the users name by splitting it from the string
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //Get the users message by splitting it from the string
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                message = message.ToLower();
                print(String.Format("{0}: {1}", chatName, message));
                int counter = 0;
                int revive = 0;
                //If this person is a player, process message, if not, skip and spawn
                for (int i = 0; i < chickNames.Count; i++)
                {
                    if (chickNames[i].Contains(chatName))
                    {
                        //Debug.Log("PRINT THIS");
                        counter++;
                        //Update personal tab
                        GlobalVar.roster[i].LastEvent = time.ToString();

                        //Track time alive! 
                        //Based on LastEvent
                        //Chicken will stay alive for at least the duration of their chick life - testing only
                        if (GlobalVar.roster[i].Exists=="AFK")
                        {
                            //AFK chickens are automatically revived from initial state
                            
                            GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "online", viewername = chatName, quantity = 0, price = 0 });
                            ProcessMessage(i, chatName, message);
                        }
                        else if (GlobalVar.roster[i].Exists == "DEAD")
                        {
                            //If this user happens to be dead, use revive in hatch
                            if (message == "!hatch brown")
                            {
                                GlobalVar.roster[i].Colour = "brown";
                            }
                            else if (message == "!hatch white")
                            {
                                GlobalVar.roster[i].Colour = "white";
                            }
                            else if (message == "!hatch yellow")
                            {
                                GlobalVar.roster[i].Colour = "yellow";
                            }
                            else if (message == "!hatch black")
                            {
                                GlobalVar.roster[i].Colour = "dGrey";
                            }
                            else if (message == "!hatch green")
                            {
                                GlobalVar.roster[i].Colour = "green";
                            }
                            else if (message == "!hatch grey")
                            {
                                GlobalVar.roster[i].Colour = "lGrey";
                            }
                            else if (message == "!hatch orange")
                            {
                                GlobalVar.roster[i].Colour = "orange";
                            }
                            else if (message == "!hatch pink")
                            {
                                GlobalVar.roster[i].Colour = "pink";
                            }
                            else if (message == "!hatch purple")
                            {
                                GlobalVar.roster[i].Colour = "purple";
                            }
                            else if (message == "!hatch blue")
                            {
                                GlobalVar.roster[i].Colour = "blue";
                            }
                            //else if (message == "!hatch red")
                            //{
                            //    GlobalVar.roster[i].Colour = "red";
                            //}
                            else
                            {
                                GlobalVar.roster[i].Colour = "brown";
                            }
                           
                            revive = i;

                        }
                        else
                        {
                            ProcessMessage(i, chatName, message);
                        }
                   
                        

                    }

                }

                if (message.StartsWith("!hatch"))
                {
                    bool exist = false;

                    //Check if user already has a chicken SPAWN CAP

                    if (counter == 1)
                    {
                        exist = true;
                        //If chick exists, reset that prefab

                        if (GlobalVar.roster[revive].Exists == "DEAD")
                        {
                            //Dead chickens reset
                            GlobalVar.roster[revive].Exists = "RESET";
                        
                        }
                    }


                    if (exist == false)
                    {
                        //DEFAULTS TO BROWN

                        print(chatName + " has spawned a chick!");
                        chicken.name = chatName;
                        player.Name = chicken.name;
                        if (message == "!hatch brown")
                        {
                            player.Colour = "brown";
                        }
                        else if (message == "!hatch white")
                        {
                            player.Colour = "white";
                        }
                        else if (message == "!hatch yellow")
                        {
                            player.Colour = "yellow";
                        }
                        else if (message == "!hatch black")
                        {
                            player.Colour = "dGrey";
                        }
                        else if (message == "!hatch green")
                        {
                            player.Colour = "green";
                        }
                        else if (message == "!hatch grey")
                        {
                            player.Colour = "lGrey";
                        }
                        else if (message == "!hatch orange")
                        {
                            player.Colour = "orange";
                        }
                        else if (message == "!hatch pink")
                        {
                            player.Colour = "pink";
                        }
                        else if (message == "!hatch purple")
                        {
                            player.Colour = "purple";
                        }
                        else if (message == "!hatch blue")
                        {
                            player.Colour = "blue";
                        }
                        //else if (message == "!hatch red")
                        //{
                        //    player.Colour = "red";
                        //}
                        else
                        {
                            player.Colour = "brown";
                        }

                        chicken.name = chatName;
                        chickNames.Add(chatName);

                        counter++;
                        GlobalVar.countChickSpawn = counter;
                        Debug.Log("MADE: " + player.Name + " - " + player.Colour);
                        GlobalVar.roster.Add(new ChickenBuilder() { Name = player.Name, Colour = player.Colour, BirthDate = time.ToString(), Level = 0, Exists = "ALIVE", LastEvent = time.ToString() });
                        GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "spawn", viewername = chatName });
                        Instantiate(chicken, new Vector3(-14.74f, 5.64f, 0.0f), Quaternion.identity);
                        player.Name = "";
                        player.Colour = "";
                    }

                }



            }

        }
        

    }



    public void ProcessMessage(int i, string chatName, string message)
    {
        
       
        if (message == "!collect")
        {

            //Collects eggs from coop
            if (GlobalVar.eggInCoop > 0)
            {
                GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "collect", viewername = chatName, quantity = GlobalVar.eggInCoop, price = 0 });
                GlobalVar.eggBank += GlobalVar.eggInCoop;
                GlobalVar.justCollected = true;
            }

        }
        else if (message == "!sell")
        {

            //Sells current dozen at market prices
            if ((GlobalVar.eggBank / 12) > 0)
            {
                audio.PlayOneShot(kaching, 1.0f);
                int tempDozen = GlobalVar.eggBank / 12;
                int totalCurrency = tempDozen * GlobalVar.marketPrice;
                GlobalVar.currencyDollar += totalCurrency;

                GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "sold", viewername = chatName, quantity = tempDozen, price = totalCurrency });
                GlobalVar.eggBank = GlobalVar.eggBank - (tempDozen * 12);
            }




        }
        else if (message == "!cull")
        {
            //Randomly selects a chicken to die, natural something? fox?

        }
        else if (message == "!upgrade")
        {
            if (GlobalVar.mylevel < 6)
            {
                if (GlobalVar.currencyDollar > GlobalVar.levels[GlobalVar.mylevel])
                {
                    audio.PlayOneShot(levelupRoo, 1.0f);
                    //Sufficient funds to purchase!

                    int cost = GlobalVar.levels[GlobalVar.mylevel];
                    GlobalVar.currencyDollar -= GlobalVar.levels[GlobalVar.mylevel];
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "upgrade", viewername = chatName, quantity = 0, price = cost });
                    GlobalVar.mylevel += 1;
                }
            }

        }
        else if (message == "!levelup")
        {


            // Levels up current user's chicken
            //CHAOS could ensue
            if (GlobalVar.currencyDollar >= 50) //Costs 50$ to level up
            {
                audio.PlayOneShot(levelupHen, 1.0f);
                Debug.Log("Can Purchase level for: " + chatName);
                //Find player's hen

                if (GlobalVar.roster[i].Name == chatName)
                {
                    GlobalVar.roster[i].Level += 1;
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "levelup", viewername = chatName, quantity = GlobalVar.roster[i].Level, price = 0 });
                    GlobalVar.currencyDollar -= 50;
                }



            }


        }
    }


}

