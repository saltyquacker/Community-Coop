using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.ComponentModel;
using System.Net.Sockets;
using System;

using UnityEngine.UI;
public class RandomEvents : MonoBehaviour
{
    public DateTime rightnow;
    public DateTime lastEvent;
    int nextEventIndex = 0;
    string nextEvent = "";
    public TimeSpan sinceLastEvent;
    int goodorbad = 0;
    int randomQuantity = 0;
    float randomQuantityFloat = 0.0f;
    int randomChicken = 0;
    int tempValue = 0;
    string tempName = "";
    public Text nextEventTimer ;

    //List of indexes of players who are alive
    public List<int> alive = new List<int>();
    //POPUP event objects 
    public Text poptitle;
    public Text popbody;
    
    public GameObject popbackobject;
    public GameObject popiconobject;
    public SpriteRenderer popback;
    public SpriteRenderer popicon;

    public Sprite coin;

    public Sprite death;
    public Sprite egg;
    public Sprite whiteChicken;


    public AudioClip eventSound;
    public float fadeSpeed = 1.0f;
    public float waitfade = 7.0f;
    private int countdown = 0;
    // Start is called before the first frame update
    void Start()
    {
        lastEvent = DateTime.Now;


        poptitle = poptitle.GetComponent<Text>();
        popbody = popbody.GetComponent<Text>();
        popback = popback.GetComponent<SpriteRenderer>();
        popicon = popicon.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        alive.Clear();
        //Events only happen if there are adults in the pen
        if (GlobalVar.adultsInPen > 0)
        {
           
            for (int i = 0; i < GlobalVar.roster.Count-1; i++)
            {
                if (GlobalVar.roster[i].Exists == "ALIVE")
                {
                    alive.Add(i);
                }
            }


            //Datetime for eventlogger
            rightnow = DateTime.Now;

            sinceLastEvent = rightnow - lastEvent;

            //countdown =150- sinceLastEvent.Seconds;
            //nextEventTimer.text = "Next Event in: " + countdown.ToString();
            //Debug.Log(sinceLastEvent.ToString());
            if (sinceLastEvent.TotalSeconds > 200)
            {
                //When last event is over alotted time, get new event
                nextEventIndex = UnityEngine.Random.Range(0, GlobalVar.events.Count - 1);

                goodorbad = UnityEngine.Random.Range(0, 1);
                nextEvent = GlobalVar.events[nextEventIndex];

                //Use this to multiply with randomQuantity. Negative means goes down.
                if (goodorbad == 0)//0 is bad!
                {
                    goodorbad = -1;
                }

               
                if (nextEvent == "market")
                {
                    //Adjust once event ends
                    tempValue = GlobalVar.marketPrice;
                    popicon.sprite = coin;
                    //This is actually addition to marketprice
                    randomQuantity = UnityEngine.Random.Range(4, 10);

                    GlobalVar.marketRateAddition = randomQuantity;
                    tempValue = GlobalVar.marketPrice;
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = "", quantity = tempValue, price = 0 });

                    StartCoroutine(ReturnToNormal(120));
                }
                if (nextEvent == "eggrate")
                {
                    popicon.sprite = egg;
                    //Use this to adjust once event ends
                    //tempValue = GlobalVar.eggRate;

                    randomQuantityFloat = UnityEngine.Random.Range(0.5f, 4.0f);

                    GlobalVar.eggRateMultiplier = randomQuantityFloat;
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = "", quantity = 0, price = 0 });
                    StartCoroutine(ReturnToNormal(120));


                }
                if (nextEvent == "chickenlevel")
                {
                    //Remember to only check alive chickens :)
                    popicon.sprite = whiteChicken;
                    randomQuantity = 1;
                    randomChicken = UnityEngine.Random.Range(0, alive.Count - 1);
                    int pickedAlive = alive[randomChicken];
                    GlobalVar.roster[pickedAlive].Level += 1;
                    string tempName = GlobalVar.roster[pickedAlive].Name;

                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = tempName, quantity = randomQuantity, price = 0 });




                }
                if (nextEvent == "chickenexist")
                {
                    //Remember to only check alive chickens :)
                    popicon.sprite = death;
                    randomQuantity = 1;
                    randomChicken = UnityEngine.Random.Range(0, alive.Count - 1);
                    int pickedAlive = alive[randomChicken];
                    GlobalVar.roster[pickedAlive].Exists = "DEAD";
                    tempName = GlobalVar.roster[pickedAlive].Name;

                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = "dead", viewername = tempName, quantity = randomQuantity, price = 0 });

                    GlobalVar.adultsInPen -= 1;


                }

                if (nextEvent == "currency")
                {
                    popicon.sprite = coin;
                    //Might just have positive currency for simplicity :(
                    randomQuantity = UnityEngine.Random.Range(0, 1000);
                        //randomQuantity = randomQuantity; /* * goodorbad;*/
                        GlobalVar.currencyDollar += randomQuantity;
                        GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = "", quantity = randomQuantity, price = 0 });
                    



                }
                if (nextEvent == "coopempty")
                {
                    popicon.sprite = egg;

                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = "", quantity = GlobalVar.eggInCoop, price = 0 });
                    GlobalVar.eggInCoop = 0;
                }
                if (nextEvent == "basketempty")
                {
                    popicon.sprite = egg;
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = nextEvent, viewername = "", quantity = GlobalVar.eggBank, price = 0 });
                    GlobalVar.eggBank = 0;
                }

                lastEvent = rightnow;
                //if (nextEventIndex == 4)
                //{
                //    nextEventIndex = 0;
                //}
                //else
                //{
                //    nextEventIndex += 1;
                //}
                StartCoroutine(ActivatePopup(nextEvent,tempName));
                tempName = "";
            }








        }
        else { 
            nextEventTimer.text = "No Hens!"; 
        }

    }


    IEnumerator ReturnToNormal(float wait)
    {
        yield return new WaitForSeconds(wait);

        GlobalVar.eggRateMultiplier = 1.0f;
        GlobalVar.marketRateAddition = 0;
    }

    IEnumerator ActivatePopup(string currentevent,string name)
    {

        //Make event stuff appear, slowly fade it out
        //Build event
        //EVENTS NOT INCLUDED ON PURPOSE; chickenlevel
        Debug.Log(currentevent);
        if (currentevent != "chickenlevel") {
            //change text body
            GetComponent<AudioSource>().PlayOneShot(eventSound, 0.2f);
            string sentencebuilder = "";
            //Randomize tender every time
            int randomTender = UnityEngine.Random.Range(0, alive.Count - 1);

            //Market
            List<string> marketBuilder = new List<string> { "world", "factory", "food" };
            List<string> eggfooditems = new List<string> { "Raw Egg Milkshakes", "Chocolate Omelette Cookies", "Egg-Stuffed Peppers", "Egg Cereal", "Egg Ice Cream", "Egg-Butter Spread", "Egg White Crackers", "Egg Lasagna", "Egg Facials", "Hot Egg Massages", "Cold Egg Massages" };

            //EggRate
            List<string> eggRateBuilder = new List<string> { "food", "predator", "weather" };
            List<string> predatorPicker = new List<string> { "fox", "raccoon", "possum", "wolf","skunk","ferral cat", };

            List<string> goodFood = new List<string> { "their Tuna Sandwich", "their Wild Berries", "their pair of edible underwear", "their leftover slice of \"Jody's\" Famous cheesecake", "a stolen handful of pears from Neighbour Meg's Pear Tree", "" };
            List<string> badFood = new List<string> { "their Rotten Tuna Sandwich", "their Inedible Wild Berries", "some small pebbles", "a Slice of Rotten Ham", "their Last Bit of Deoderant Stick", "their least favourite pencil"};

            //ChickenExist
            List<string> deathBuilder = new List<string> { "has eaten a bad toad", "has fought and lost to a snake", "has jumped the fence and hasn't returned", "has been pecked to death by the flock", "has eaten a deadly fungus", "was unable to push her last egg", "suffered an unknown injury and didn't make it", "has broken her wing from falling off the coop roof", "has been forever exiled for taking Chicken God too seriously","auditioned for a singing role in the next top film \"Hens\" and got the part" };

            //Currency
            List<string> badcurrencyBuilder = new List<string> { "has forgotten to pay last years taxes", "bought a mouldy bag of unreturnable food" };
            List<string> goodcurrencyBuilder = new List<string> { "has overpaid last years taxes"/*, "has been generously donated to by the local animal hospital"*/, "found a bag of discarded shinies", "found a rock worth more than she is" };
            
            //Empty coop
            //List<string>

            //Empty basket


            //Market
            if (currentevent == "market")//Market done
            {
                int randomSentence = UnityEngine.Random.Range(0, marketBuilder.Count - 1);

                if (randomSentence == 0)
                {
                    sentencebuilder = "The World is running out of eggs! Demand is higher than ever. Market Price increased!";
                }
                else if (randomSentence == 1)
                {
                    sentencebuilder = "The local egg distribution factory has exploded. Demand is higher than ever. Market Price increased!";
                }
                else if (randomSentence == 2)
                {

                    int trendPicker = UnityEngine.Random.Range(0, eggfooditems.Count - 1);

                    sentencebuilder = "The latest trend is " + eggfooditems[trendPicker] + ". Demand is higher than ever. Market Price increased!";
                }

            }
            else if (currentevent == "eggrate")//Egg rate done
            {
                //Based on egg multiplier
                int randomSentence = UnityEngine.Random.Range(0, eggRateBuilder.Count - 1);

           
                if (GlobalVar.eggRateMultiplier > 1.0f)//BAD
                {
                    //Pick a bad food
                   
                    if (randomSentence == 0)
                    {
                        int randomFood = UnityEngine.Random.Range(0, badFood.Count - 1);
                        sentencebuilder = GlobalVar.roster[randomTender].Name + " has shared " + badFood[randomFood] + " with the flock. Everyone had a bite and is feeling a little ill. Egg laying decreased.";
                    }
                    else if (randomSentence == 1)
                    {
                        int randomPred = UnityEngine.Random.Range(0, predatorPicker.Count - 1);
                        sentencebuilder = "The flock has been spooked by a "+ predatorPicker[randomPred]+" way off in the distance! Egg laying decreased.";
                    }
                    else if (randomSentence == 2)
                    {
                     
                        sentencebuilder = "The weather has been particularily cold lately. Egg laying decreased.";
                    }




                }
                else if (GlobalVar.eggRateMultiplier < 1.0f) //GOOD
                {
                    //Pick a good food
                   
                    if (randomSentence == 0)
                    {
                        int randomFood = UnityEngine.Random.Range(0, goodFood.Count - 1);
                        sentencebuilder = GlobalVar.roster[randomTender].Name + " has shared " + goodFood[randomFood] + " with the flock. Everyone had a bite and is feeling extremely energized! Egg laying increased!";
                    }
                    else if (randomSentence == 1)//Not actually predator
                    {
                        sentencebuilder = "The flock has been much happier since the addition of our new Community Coop Tenders! Egg laying increased!";
                    }
                    else if (randomSentence == 2)
                    {

                        sentencebuilder = "The weather has been particularily favourable lately. Egg laying increased!";
                    }





                }
            }
            else if (currentevent == "chickenexist")
            {
                int randomSentence = UnityEngine.Random.Range(0, deathBuilder.Count - 1);
                sentencebuilder = name + " " + deathBuilder[randomSentence] + ". Bye Forever!";
            }
            else if (currentevent == "currency")
            {
                int randomSentence = UnityEngine.Random.Range(0, goodcurrencyBuilder.Count - 1);
               
                sentencebuilder = GlobalVar.roster[randomTender].Name + " " + goodcurrencyBuilder[randomSentence] + "!";
            }
            else if (currentevent == "coopempty")
            {
                int randomAnimal = UnityEngine.Random.Range(0, predatorPicker.Count - 1);
                sentencebuilder = "Oh no! A " + predatorPicker[randomAnimal] + " snuck into the coop and ate all the eggs.";
                //Debug.Log("Coop empty");
                //Just the coop that has issues with counting
                GlobalVar.eggEvent = true;
            }
            else if (currentevent == "basketempty")
            {
              
                sentencebuilder = "Oh no! While " + GlobalVar.roster[randomTender].Name + " was collecting eggs, they dropped the entire basket on the ground!";
                Debug.Log("Basket empty");
            }



            popbody.text = sentencebuilder;

            //change icon






            //Fade popup after population
            poptitle.color = new Color(poptitle.color.r, poptitle.color.g, poptitle.color.b, 1);
            popbody.color = new Color(popbody.color.r, popbody.color.g, popbody.color.b, 1);
            popback.color = new Color(popback.color.r, popback.color.g, popback.color.b, 1);
            popicon.color = new Color(popicon.color.r, popicon.color.g, popicon.color.b, 1);
            yield return new WaitForSeconds(waitfade);
            while (poptitle.color.a > 0.0f)
            {
                poptitle.color = new Color(poptitle.color.r, poptitle.color.g, poptitle.color.b, poptitle.color.a - (Time.deltaTime / waitfade));
                popbody.color = new Color(popbody.color.r, popbody.color.g, popbody.color.b, popbody.color.a - (Time.deltaTime / waitfade));
                popback.color = new Color(popback.color.r, popback.color.g, popback.color.b, popback.color.a - (Time.deltaTime / waitfade));
                popicon.color = new Color(popicon.color.r, popicon.color.g, popicon.color.b, popicon.color.a - (Time.deltaTime / waitfade));
                yield return null;
            }

        }
       
    }

}
