using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
using System;
//For holding global variables such as chicken information
public static class GlobalVar
{
    //Streamer information
    public static string usernameGlobal, passwordGlobal, channelNameGlobal;

    public static List<string> animalTypes = new List<string> { "Chicken", "Goat", "Sheep", "Cow", "Ostrich", "Bees", "Squirrels", "Raccoons" };
    //eggs,milk,wool,milk,egg,honey!,Nuts?,Trash


    public static string name;

    public static List<ChickenBuilder> roster = new List<ChickenBuilder>();
    public static List<EventLog> eventlog = new List<EventLog>();
    public static List<string> events = new List<string> { "market", "eggrate", "chickenlevel", "chickenexist", "currency", "coopempty","basketempty" };
    public static List<string> subPlayers = new List<string>();
    
    
    public static int[] levels = new int[] { 100, 250, 500, 700, 900, 1200, 1500 };
    public static int mylevel = 0;
    public static int countChickSpawn = 0;
    public static int maxInPen = GlobalVar.roster.Count;
    public static int adultsInPen;
  
    public static int currencyDollar=200;
    public static int AFKHours = 5;

    public static float eggRate;
    public static bool justCollected = false;
    public static int eggInCoop;
    public static int eggBank;
    public static int marketPrice;
    public static int maxEggInCoop=100;
    public static float eggRateMultiplier = 1.0f;
    public static int marketRateAddition = 0;
    public static int truncateDozen;

    public static int eggCount =0; //default number
    public static int eggSpots = 18;
    public static float[] eggTimers = new float[18]; //Keep track of egg timers
    public static bool eggEvent = false;

    //check if we are connected to chat
    public static bool connected;



    //Tracking for icons
    public static int highestLevel=0;
    public static string topPlayer;
    public static string streakPlayer;
    public static DateTime highestAge;


    //Name, colour, baby/adult, 
}

public class ChickenBuilder
{
    public string Name { get; set; }
    public string Colour { get; set; }
    public string BirthDate { get; set; }
    public int Level { get; set; } //This is actually size related, once you get big enough you evolve?
    public string Exists { get; set; } //Alive
    public string LastEvent { get; set; }
}

public class Events
{
    public string Type { get; set; } //market,eggrate,chickenlevel,chickenexist,currency
}

public class EventLog
{
    public string timestamp { get; set; }
    public string eventType { get; set; }
    public string viewername { get; set; }
    public int quantity { get; set; }
    public int price { get; set; }

    public override string ToString(){
        if (eventType == "spawn")
        {
            return timestamp + ": " + viewername + " has hatched! Welcome to the coop!\n\n";
        }
        else if (eventType == "collect")
        {
            return timestamp + ": " + viewername + " has collected " + quantity + " eggs!\n\n";
        }
        else if (eventType == "sold")
        {
            return timestamp + ": " + viewername + " has sold " + quantity + " dozen for $" + price + "!\n\n";
        }
        else if (eventType == "upgrade")
        {
            return timestamp + ": " + viewername + " has upgraded the coop for $"+price+"!\n\n";
        }
        else if (eventType == "levelup")
        {
            return timestamp + ": " + viewername + " just leveled up!\n\n";
        }
        else if (eventType == "online")
        {
            return timestamp + ": " + viewername + " has returned! Welcome back.\n\n";
        }
        else if (eventType == "AFK")
        {
            return timestamp + ": " + viewername + " has gone AFK! We miss you already.\n\n";
        }
        else if (eventType == "dead")
        {
            return timestamp + ": " + viewername + " has died! Bye Forever!\n\n";
        }
        else if (eventType == "market")
        {
            return timestamp + ": the market has temporarily increased!\n\n";
        }
        else if (eventType == "eggRate")
        {
            if(quantity == 0)
            {
                return timestamp + ": the hens are feeling happy! Egg production is increased.\n\n";
            }
            else 
            {
                return timestamp + ": the hens are feeling uneasy! Egg production is decreased.\n\n";
            }
           
        }
        else if (eventType == "chickenlevel")
        {
             return timestamp + ": " + viewername + " has been feeling particularily chipper lately and just leveled up!\n\n";
        }
        else if (eventType == "currency")
        {
            if (quantity < 0)
            {
                return timestamp + ": How unlucky. $" + quantity + "!\n\n";
            }
            else 
            {
                return timestamp + ": Wow So lucky! +$" + quantity + "!\n\n";
            }

        }
        else if(eventType == "coopempty")
        {
            return timestamp + ": We just lost "+quantity+" eggs from the coop!\n\n";
        }
        else if (eventType == "basketempty")
        {
            return timestamp + ": We just lost " + quantity + " eggs from the basket!\n\n";
        }
        else
        {
            return "\n\n";
        }

    }
    public string ToFile()
    {
        return timestamp + "," + viewername + "," + eventType + "," + quantity + "," + price+"\n";
    }

}
public class Global : MonoBehaviour
{
    void Start()
    {
       // Debug.Log("Rows: "+ GlobalVar.slotRow);
        DontDestroyOnLoad(this.gameObject);
        //GlobalVar.currencyDollar = 50;
      
    }
    void Update()
    {
        
    }

  


}
