using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Threading.Tasks;
using System.IO;

public class QuantityCanvasOnPen : MonoBehaviour
{
    //public Text blueQ;
    //public Text greenQ;
    //public Text pinkQ;
    public Text currency;
    public Text coopQ;
    public Text basketQ;
    public Text dozensQ;
    public Text marketP;
    public Text eventLog;
    public Text chatstatus;
    public GameObject chatstatusBack;
    public int max = -1;

    public Text listPlayers;
    public Text totalPlayers;
    public Text totalPlayersActive;
    public string listString;
    // Start is called before the first frame update
    void Start()
    {
        listString = "";
    }

    // Update is called once per frame
    void Update()
    {
        //Update list of players and info!

        for (int p = 0; p < GlobalVar.roster.Count; p++)
        {
            listString += p.ToString() + ": " + GlobalVar.roster[p].Name + " - " + GlobalVar.roster[p].Colour + " - " + GlobalVar.roster[p].Level + " - " + GlobalVar.roster[p].BirthDate + " - " + GlobalVar.roster[p].LastEvent + " - " + GlobalVar.roster[p].Exists + "\n\n";

        }
        listPlayers.text = listString;
        listString = "";

        totalPlayers.text = "Total Hatched: "+GlobalVar.roster.Count.ToString();
        totalPlayersActive.text = "Total Active: " +GlobalVar.adultsInPen.ToString();

        if (GlobalVar.connected == true)
        {
            chatstatus.text = "CONNECTED, GLHF!";
            chatstatusBack.GetComponent<SpriteRenderer>().material.color = new Color(33,191,30);

        }
        else
        {
            chatstatus.text = "RECONNECTING...";
            chatstatusBack.GetComponent<SpriteRenderer>().material.color = new Color(156, 10, 0);
        }
        //blueQ.text = "x" + GlobalVar.bluePot;
        //greenQ.text = "x" + GlobalVar.greenPot;
        //pinkQ.text = "x" + GlobalVar.pinkPot;
        currency.text = "$" + GlobalVar.currencyDollar;
        coopQ.text = GlobalVar.eggInCoop + "/" + GlobalVar.maxEggInCoop;
        basketQ.text = GlobalVar.eggBank + "";
        dozensQ.text = (GlobalVar.eggBank / 12).ToString();
        marketP.text = "$" + GlobalVar.marketPrice+ "/dozen";

        //if an event gets added
        //Debug.Log("HELLO!");

        if (max != GlobalVar.eventlog.Count-1&&max>=1)
        {
            string newItem = GlobalVar.eventlog[max].ToFile();
            //print("ADDING 1 TO FILE!");
            //Debug.Log("PRE-WRITING: " + newItem);
            ExampleAsync(newItem);
        }
       
    
        max = GlobalVar.eventlog.Count-1;

        if (max >= 0){
            //probably terribel code, outputs last 4 events
            if (max >= 3)
            {
                eventLog.text = GlobalVar.eventlog[max].ToString() + GlobalVar.eventlog[max - 1].ToString() + GlobalVar.eventlog[max - 2].ToString() + GlobalVar.eventlog[max - 3].ToString();
            }
            if (max == 2)
            {
                eventLog.text = GlobalVar.eventlog[max].ToString() + GlobalVar.eventlog[max - 1].ToString() + GlobalVar.eventlog[max - 2].ToString();
            }
            if (max == 1)
            {
                eventLog.text = GlobalVar.eventlog[max].ToString() + GlobalVar.eventlog[max - 1].ToString();
            }
            if (max == 0)
            {
                eventLog.text = GlobalVar.eventlog[max].ToString();
            }
        }
        


    }

    public static async Task ExampleAsync(string item)
    {
        File.AppendAllText("testtext.txt", item);
        //Debug.Log("WRITING: " + item);
    }
}
