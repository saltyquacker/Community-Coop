using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;
//For holding global variables such as chicken information
public static class GlobalVar
{
    public static string name;

    public static List<ChickenBuilder> roster = new List<ChickenBuilder>();
    public static List<EventLog> eventlog = new List<EventLog>();
    public static int[] levels = new int[] { 2000, 6000, 10000, 25000, 55000, 100000, 250000 };
    public static int mylevel = 0;
    public static int countChickSpawn = 0;
    public static int maxInPen = GlobalVar.roster.Count;
    public static int adultsInPen;
  
    public static int currencyDollar=200;
    public static int AFKHours = 2;

    public static float eggRate;
    public static bool justCollected = false;
    public static int eggInCoop;
    public static int eggBank;
    public static int marketPrice;
    public static int maxEggInCoop=24;
  
    public static int truncateDozen;

    public static int eggCount =0; //default number
    public static int eggSpots = 18;
    public static float[] eggTimers = new float[18]; //Keep track of egg timers

    //check if we are connected to chat
    public static bool connected;


    public static bool roosterBuff; //OnClick() roo buff true



    //Name, colour, baby/adult, 
}

public class ChickenBuilder
{
    public string Name { get; set; }
    public string Colour { get; set; }
    public string BirthDate { get; set; }
    public int Level { get; set; } //This is actually size related, once you get big enough you evolve?
    public bool Exists { get; set; } //Alive
    public string LastEvent { get; set; }
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
