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
using UnityEngine.SceneManagement;


public class InitialTwitchConnect : MonoBehaviour
{
    //Only for initial connection on login screen. Just to make sure Twitch will respond!

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    public string username, password, channelName; //Get the password from https://twitchapps.com/tmi

    public Button submit;
    public InputField userName;
    //channelName will be cast to lowercase username, not sure if channel names can be different than usernames?
    public InputField oAuthPassword;
    public Text authStatus;


    private bool connected=false;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = submit.GetComponent<Button>();
        btn.onClick.AddListener(ConnectOnClick);
        //username.onValueChange.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
   
    void ConnectOnClick()
    {
        Debug.Log("Submitting....");
        ValueChangeCheckAuthToken();
        ValueChangeCheckUser();
        //Build a simple coroutine to attempt to connect to twitch chat
     
        
        StartCoroutine(Load());
       
      
    }
    //Check input fields
    public void ValueChangeCheckUser()
    {
        username = userName.text;
        channelName = username.ToLower();
    }
    public void ValueChangeCheckAuthToken()
    {
        password = oAuthPassword.text;

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
        //  GlobalVar.eventlog.Add(new EventLog() { timestamp = time.ToString(), eventType = "reconnecting...", viewername = "", quantity = 0, price = 0 });

        //Only Read chat once after connecting or attempting to connect
    
    }


    private void ReadChat()
    {

        //print(twitchClient.Available);

        if (twitchClient.Available > 0)
        {

            print("Available...");
            var message = reader.ReadLine();
            //print(message);
            Debug.Log(message);
            //if (message.Contains("PRIVMSG"))
            //{

            if (message.Contains("GLHF!"))
            {
                print("CONNECTED");
                connected = true;
                authStatus.text = "Connected! Loading the coop...";
                StartCoroutine(LoadNextScene());
            }
            else 
            {
                connected = false;
                authStatus.text = "Invalid Authentication. Try again.";
            }


            //   }



        }
        else
        {
            authStatus.text = "Twitch Chat currently unavailable...";
        }

    }


    IEnumerator Load()
    {
        authStatus.text = "Trying to Connect...";
        Connect();
        yield return new WaitForSeconds(1);
        //authStatus.text =  "Attempting to Read Chat";
        ReadChat();
    }

    //Only loads scene if credentials are valid!
    IEnumerator LoadNextScene()
    {
        GlobalVar.usernameGlobal = username;
        GlobalVar.channelNameGlobal = channelName;
        GlobalVar.passwordGlobal = password;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Pen");
        Debug.Log("Scene loaded");
    }


 
}
