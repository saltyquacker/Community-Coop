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
//UnityEngine.Random 2D movement, Chickens/Chicks within Pen area


public class ChickBehaviour : MonoBehaviour
{
    //PEN MAX COORDS
    //TopLeft:  (-10, 4.5)
    //TopRight: ( 10, 4.5)
    //BLeft:    (-10,-4.0)
    //BRight:   ( 10,-4.0)
    //ORIGIN:   ( -8, 2.3) <=COOP
    //White, Yellow Common
    //Brown, Black Uncommon
    //Light Grey, Dark Grey Rare
    //Orange, Green Ultra rare
    
    private Vector2 target;
    private Vector2 position;
    private float speed = 1.5f;
    private float waitRand;
    private bool waiting = false;
    private string colour;
    private string prefabchickName ;
    private int num;
    private string strName;
    private float ageTime;
    public bool deadFlag = false;
    private bool sickFlag;
    private bool sickPercent;
    public bool isAdult;

    public SpriteRenderer spriterenderer;
    public Sprite dead;
    private GameObject chickNum;
    //HANDLES DISPLAY ON CHICKENS ICON AND TEXT
    public Text txtName;
    public Text txtLvl;
    public Text countDown;
    public GameObject streak; //longest living chicken
    public GameObject subStatus; //Is player subscriber
    public GameObject topPlayer; //highest level
    public string whoistop;
    


    public Sprite w; //White
    public Sprite y;  //Yellow
    public Sprite b; //Brown
    public Sprite dg; //DarkGrey
    public Sprite g;  //Green
    public Sprite lg;  //LightGrey
    public Sprite o; //Orange
    public Sprite aW; //White
    public Sprite aY;  //Yellow
    public Sprite aB; //Brown
    public Sprite aDG; //DarkGrey
    public Sprite aG;  //Green
    public Sprite aLG;  //LightGrey
    public Sprite aO; //Orange
    public Sprite aPink; //Pink
    public Sprite aPurp; //Purple
    public Sprite aRed; //Red
    public Sprite aBlue; //Blue




    public int level;
    public DateTime lastevent;
    public TimeSpan lastactivity;
    public DateTime rightnow;
    public Vector2 afkPos = new Vector2 (25.65f, 10.39f);
    public Vector2 spawnPos = new Vector2(-14.74f, 5.64f);
    public GameObject countdownText;
    public int countDownInt;

    //AUDIO
    public AudioSource audioSource;
    public AudioClip cluck1;
    public AudioClip cluck2;
    public AudioClip hatch;
    public int randomCluck;
    public int playornoplay;
    

    //UPDATED MOVEVEMENT
    public string direction;
    private float randomDirection;

    private bool alivebefore = false;
    public int totalAdults;

    

    void Start()
    {
        //Disable all statuses at start
        streak.SetActive(false);
        subStatus.SetActive(false);
        topPlayer.SetActive(false);

       


        //Instantiate chick for the first time
        if (alivebefore == false)
        {
            num = GlobalVar.roster.Count - 1;
            prefabchickName = this.gameObject.name;

            //Regex to extract Prefab # from chicken's name
            name = Regex.Match(prefabchickName, @"\d+").Value;
            ////First Prefab chicken is 0
            if (name == "")
            {

                name = GlobalVar.roster[GlobalVar.roster.Count - 1].Name;

            }

            //Set roster info of current chicken at index
            colour = GlobalVar.roster[GlobalVar.roster.Count - 1].Colour;
            strName = GlobalVar.roster[GlobalVar.roster.Count - 1].Name;
            print(strName + ": " + colour);
            chickNum = GameObject.Find(name);

            txtName.text = strName;

            alivebefore = true;
        }
        //Everything else happens after a reset
        isAdult = false;
        //Get Audio source
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(hatch, 0.8f);

        gameObject.transform.position = spawnPos;


        target = spawnPos;
        //position = gameObject.transform.position;
   
        //target = new Vector2(-2.3f, gameObject.transform.position.y);
        //Spawns at coop so guaranteed to go right
        direction = "right";
       

        countdownText.SetActive(false);


        this.gameObject.GetComponent<Animator>().Play("Yellow");
        chickNum.GetComponent<SpriteRenderer>().sprite = y;


        //Start UnityEngine.Random coroutine AGE() timer (chick duration)
        //Previously 1000,7000 ->200-700
        ageTime = UnityEngine.Random.Range(60.0f, 90.0f);
        StartCoroutine(Age(ageTime));

        //Check for icon statuses
        if (GlobalVar.subPlayers.Contains(strName))
        {
            subStatus.SetActive(true);
        }

    }

    void Update()
    {
        //Get highest chicken
        //If I am the highest chicken, give me crown!

        if (GlobalVar.roster[num].Level > GlobalVar.highestLevel)
        {
            //if I am the highest level, set the highest level to my level
            GlobalVar.highestLevel = GlobalVar.roster[num].Level;
            //Based on who gets there first, if a chicken can catch up to levels, they dont get crown status :)
            GlobalVar.topPlayer = GlobalVar.roster[num].Name;
            Debug.Log(GlobalVar.topPlayer + " is now the top player!");
            
        }
        else{
            if(GlobalVar.topPlayer == strName)
            {
                //If any any time, the highest chicken dies, reset
                if (GlobalVar.roster[num].Exists == "DEAD")
                {
                    GlobalVar.highestLevel = 0;
                    GlobalVar.topPlayer = "";
                }
                else
                {
                    topPlayer.SetActive(true);
                }
                
            }
            else
            {
                topPlayer.SetActive(false);
            }
            
        }

        //If I am longest living chicken, give me flame
        //Do this differently than highest level, maybe iterate through all players and if alive, set oldest



        if (GlobalVar.roster[num].Exists == "RESET")
        {
            //Maybe not destroy, fucks with counter
            gameObject.transform.position = spawnPos;
            GlobalVar.roster[num].Exists = "ALIVE";
            GlobalVar.roster[num].Level = 0;
            Start();
            
        }
        else if (GlobalVar.roster[num].Exists == "ALIVE")
        {



            totalAdults = GlobalVar.adultsInPen;
            rightnow = DateTime.Now;
            //Track time alive! 
            //Based on LastEvent
            //Chicken will stay alive for at least the duration of their chick life - testing only
            lastevent = DateTime.Parse(GlobalVar.roster[num].LastEvent);
            lastactivity = rightnow - lastevent;
            //Debug.Log("Time since last activity:"+lastactivity.Seconds);
            if (lastactivity.TotalHours > GlobalVar.AFKHours)
            {
                gameObject.GetComponent<Animator>().enabled = false;
                //Debug.Log("AFK!" + lastactivity.ToString());
                GlobalVar.roster[num].Exists = "AFK";

                //if (gameObject.activeSelf)
                //{
                //gameObject.SetActive(false);
                if (gameObject.transform.position.x != afkPos.x && gameObject.transform.position.y != afkPos.y)
                {
                    GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = "AFK", viewername = GlobalVar.roster[num].Name, quantity = 0, price = 0 });
                    GlobalVar.adultsInPen -= 1;
                    gameObject.transform.position = afkPos;


                }

                //gameObject.transform.position = afkPos;
                //}
            }
            else if (lastactivity.TotalHours < GlobalVar.AFKHours)
            {
                //Display warning countdown on chicken till AFK
                //countDown.enabled = false;
                //if (lastactivity.TotalSeconds > 60)
                // {
                //Debug.Log(60-lastactivity.TotalSeconds);
                //countDown.enabled = true;
                //7200 seconds is 2 hours I guess!
                countDownInt = (int)(7200 - lastactivity.TotalSeconds);
                if (countDownInt < 60)
                {
                    countdownText.SetActive(true);
                    countDown.text = countDownInt.ToString();
                }
                else if (countDownInt > 60)
                {
                    countdownText.SetActive(false);
                }

                // }
                //Debug.Log("ALIVE!" + lastactivity.ToString());
                if (GlobalVar.roster[num].Exists == "AFK")
                {
                    gameObject.transform.position = spawnPos;
                    gameObject.GetComponent<Animator>().enabled = true;
                    GlobalVar.adultsInPen += 1;
                }
                GlobalVar.roster[num].Exists = "ALIVE";
                //If last event was an afk


                if (!waiting)
                {
                    float step = speed * Time.deltaTime;
                    if (Vector2.Distance(transform.position, target) < 0.001f)
                    {
                        randoLocation();
                        StartCoroutine(RestCoroutine());
                       
                    }
                    // move sprite towards the target location
                    //cluck once when moving?





                    transform.position = Vector2.MoveTowards(transform.position, target, step);
                    //yield return new WaitForSeconds(audioSource.clip.length);
                    //audioSource.clip = cluck2;




                }






                level = GlobalVar.roster[num].Level;

                txtLvl.text = level.ToString();

                if (level >= 1)
                {
                    float scale = 2 + (level * 0.08f); //.2 is good for scaling visually
                                                       //No CAP
                    gameObject.transform.localScale = new Vector2(scale, scale);



                }
                //gameObject.SetActive(true);
                //if (!gameObject.activeSelf)
                //{
                //    gameObject.SetActive(true);
                //    GlobalVar.adultsInPen -= 1;
                //}
            }

            //SET LEVEL***********
            //UP SIZE*******

        }
        else if (GlobalVar.roster[num].Exists == "DEAD")
        {
            gameObject.transform.position = afkPos;
            GlobalVar.roster[num].Level = 0;
        }




    }

    void randoLocation()
    {
        if (isAdult == true)
        {

            GetComponent<AudioSource>().PlayOneShot(cluck1, 0.1f);
            randomDirection = UnityEngine.Random.Range(0.0f, 1.0f); //x or y
            if (randomDirection > 0.5f)
            {
                target = new Vector2(UnityEngine.Random.Range(-16.72944f, 4.05f), gameObject.transform.position.y);
                if (target.x > gameObject.transform.position.x)
                {
                    direction = "right";
                }
                else
                {
                    direction = "left";
                }

            }
            else if (randomDirection < 0.5f)
            {
                target = new Vector2(gameObject.transform.position.x, UnityEngine.Random.Range(-3.33f, 6.18f));
                if (target.y > gameObject.transform.position.y)
                {
                    direction = "back";
                }
                else
                {
                    direction = "front";
                }
            }

        }
       else if(isAdult ==false)
       {
            target = new Vector2(UnityEngine.Random.Range(-16.72944f, 4.05f), UnityEngine.Random.Range(-3.33f, 6.18f));
       }
        

    }


    //Rest before moving and changing locations
    IEnumerator RestCoroutine()
    {
        waiting = true;
        waitRand = UnityEngine.Random.Range(10.0f, 30.0f);

        //Disable animator
        gameObject.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(waitRand);

        //Enable animator after wait
        gameObject.GetComponent<Animator>().enabled = true;
        waiting = false;
        if (isAdult == true)
        {
            ColourPicker(colour, direction);
        }

    }

    //Timer as soon as chick is 'born'
    IEnumerator Age(float wait)
    {
        yield return new WaitForSeconds(wait);
        //What should I do now? Age!
        //GlobalVar.roster[num, colour, "1"];
        isAdult = true;
        GlobalVar.adultsInPen += 1;
        //GlobalVar.chicksInPen -= 1;
        //RESET TARGET SO IT DOESNT DO FUNNY THINGS when growing up
        target = new Vector2(gameObject.transform.position.x-0.5f, gameObject.transform.position.y);
        direction = "left";
        if (colour == "white")
        {
            this.gameObject.GetComponent<Animator>().Play("aW");
            chickNum.GetComponent<SpriteRenderer>().sprite = aW;

        }
        else if (colour == "yellow")
        {
            this.gameObject.GetComponent<Animator>().Play("aY");
            chickNum.GetComponent<SpriteRenderer>().sprite = aY;
        }
        else if (colour == "brown")
        {
            this.gameObject.GetComponent<Animator>().Play("aBr");
            chickNum.GetComponent<SpriteRenderer>().sprite = aB;
        }
        else if (colour == "dGrey")
        {
            this.gameObject.GetComponent<Animator>().Play("aDG");
            chickNum.GetComponent<SpriteRenderer>().sprite = aDG;
        }
        else if (colour == "green")
        {
            this.gameObject.GetComponent<Animator>().Play("aGr");
            chickNum.GetComponent<SpriteRenderer>().sprite = aG;
        }
        else if (colour == "lGrey")
        {
            this.gameObject.GetComponent<Animator>().Play("aLG");
            chickNum.GetComponent<SpriteRenderer>().sprite = aLG;
        }
        else if (colour == "orange")
        {
            this.gameObject.GetComponent<Animator>().Play("aO");
            chickNum.GetComponent<SpriteRenderer>().sprite = aO;
        }
        else if (colour == "pink")
        {
            this.gameObject.GetComponent<Animator>().Play("aPink");
            chickNum.GetComponent<SpriteRenderer>().sprite = aPink;
        }
        else if (colour == "purple")
        {
            this.gameObject.GetComponent<Animator>().Play("aPurp");
            chickNum.GetComponent<SpriteRenderer>().sprite = aPurp;
        }
        else if (colour == "blue")
        {
            this.gameObject.GetComponent<Animator>().Play("aBlue");
            chickNum.GetComponent<SpriteRenderer>().sprite = aBlue;
        }
        else if (colour == "red")
        {
            this.gameObject.GetComponent<Animator>().Play("aRed");
            chickNum.GetComponent<SpriteRenderer>().sprite = aRed;
        }

        //  StartCoroutine(Death());
        //GlobalVar.roster[GlobalVar.roster.Count - 1].Exists = "RESET";
        //GlobalVar.eventlog.Add(new EventLog() { timestamp = rightnow.ToString(), eventType = "dead", viewername = GlobalVar.roster[GlobalVar.roster.Count - 1].Name});


    }
   
    void ColourPicker(string col,string dir)
    {
      
        if (colour == "white")
        {
            

            if(direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftwhite");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftwhite");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if(direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aW");
            }
            else if(direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backwhite");
              
            }

        }
        else if (colour == "yellow")
        {
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftyellow");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftyellow");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("addY");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backyellow");

            }
        }
        else if (colour == "brown")
        {

            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftbrown");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftbrown");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aBr");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backbrown");

            }
        }
        else if (colour == "dGrey")
        {
      
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftdgrey");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftdgrey");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aDG");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backdgrey");

            }
        }
        else if (colour == "green")
        {
           // this.gameObject.GetComponent<Animator>().Play("aGr");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftgreen");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftgreen");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aGr");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backgreen");

            }
        }
        else if (colour == "lGrey")
        {
            //this.gameObject.GetComponent<Animator>().Play("aLG");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftlgrey");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftlgrey");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aLG");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backlgrey");

            }
        }
        else if (colour == "orange")
        {
            //this.gameObject.GetComponent<Animator>().Play("aO");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftorange");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftorange");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aO");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backorange");

            }
        }
        else if (colour == "pink")
        {
            //this.gameObject.GetComponent<Animator>().Play("aPink");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftpink");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftpink");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aPink");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backpink");

            }
        }
        else if (colour == "purple")
        {
            
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftpurple");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftpurple");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aPurp");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backpurple");

            }
        }
        else if (colour == "blue")
        {
           // this.gameObject.GetComponent<Animator>().Play("aBlue");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftblue");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftblue");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aBlue");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backblue");

            }
        }
        else if (colour == "red")//LAST RECOLOUR
        {
            this.gameObject.GetComponent<Animator>().Play("aRed");
            if (direction == "left")
            {
                this.gameObject.GetComponent<Animator>().Play("leftwhite");
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (direction == "right")
            {
                this.gameObject.GetComponent<Animator>().Play("leftwhite");
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (direction == "front")
            {
                this.gameObject.GetComponent<Animator>().Play("aRed");
            }
            else if (direction == "back")
            {
                this.gameObject.GetComponent<Animator>().Play("backwhite");

            }
        }

    }

}
