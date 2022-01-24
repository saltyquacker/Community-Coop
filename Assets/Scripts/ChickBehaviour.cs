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
    private bool isAdult;

    public SpriteRenderer spriterenderer;
    public Sprite dead;
    private GameObject chickNum;
    public Text txtName;
    public Text txtLvl;
    public Text countDown;

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
    public Vector2 spawnPos = new Vector2(-8.10f, 4.16f);
    public GameObject countdownText;
    public int countDownInt;

    //AUDIO
    public AudioSource audioSource;
    public AudioClip cluck1;
    public AudioClip cluck2;
    public AudioClip hatch;
    public int randomCluck;
    public int playornoplay;
    
    //public Slider healthBar;
    //public float health =100;
    //public float deathRate;
    //public float decrement;

    public int totalAdults;

    void Start()
    {
        isAdult = false;
        //Get Audio source
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(hatch, 0.8f);


        num = GlobalVar.roster.Count - 1;
        print(num);
        target = spawnPos;
        position = gameObject.transform.position;
        //Debug.Log(target);
        //target = new Vector2(-3.0f, 3.3f);
        target = new Vector2(UnityEngine.Random.Range(-16.72944f, 4.05f), UnityEngine.Random.Range(-3.33f, 6.18f));
        prefabchickName = this.gameObject.name;
       
        //Regex to extract Prefab # from chicken's name
        name = Regex.Match(prefabchickName, @"\d+").Value;
        ////First Prefab chicken is 0
        if (name == "")
        {
            //int nameInt = GlobalVar.countChickSpawn - 1;
            //name = nameInt.ToString();
            name = GlobalVar.roster[GlobalVar.roster.Count-1].Name;
            //Debug.Log(name);
        }

        //Debug.Log(name);
        //Debug.Log(GlobalVar.countChickSpawn);

        //Set roster info of current chicken at index
        colour = GlobalVar.roster[GlobalVar.roster.Count - 1].Colour;
        strName = GlobalVar.roster[GlobalVar.roster.Count - 1].Name;
        print(strName + ": " + colour);
        chickNum = GameObject.Find(name);

        txtName.text = strName;
        countdownText.SetActive(false);

        // Debug.Log("Death Rate of:" + chickNum + " is " + deathRate);
        //Debug.Log("Using roster:  "+strName +" is "+ colour + "  "+ Time.time);
        //Set colour of current chick in pen
        //SET ALL DEFAULT CHICK COLOURS TO YELLOW!
        this.gameObject.GetComponent<Animator>().Play("Yellow");
        chickNum.GetComponent<SpriteRenderer>().sprite = y;
       

        //Start UnityEngine.Random coroutine AGE() timer (chick duration)
        //Previously 1000,7000 ->200-700
        ageTime = UnityEngine.Random.Range(60.0f, 90.0f);
        StartCoroutine(Age(ageTime));
        Debug.Log(strName + " will age in :" + ageTime + "seconds");
       // decrement = 100;
    }

    void Update()
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
            GlobalVar.roster[num].Exists = false;
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
            if(countDownInt < 60)
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
            if(GlobalVar.roster[num].Exists == false)
            {
                gameObject.transform.position = spawnPos;
                gameObject.GetComponent<Animator>().enabled = true;
                GlobalVar.adultsInPen += 1;
            }
            GlobalVar.roster[num].Exists = true;
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

    void randoLocation()
    {
        if (isAdult == true)
        {


            playornoplay = UnityEngine.Random.Range(0, 1);
            if (playornoplay == 0)
            {
                randomCluck = UnityEngine.Random.Range(0, 1);
                if (randomCluck == 0)
                {
                    audioSource.PlayOneShot(cluck1, 0.7f);
                }
                else
                {
                    audioSource.PlayOneShot(cluck2, 0.7f);

                }
            }//else dont play, just move

        }
        target = new Vector2(UnityEngine.Random.Range(-16.72944f, 4.05f), UnityEngine.Random.Range(-3.33f, 6.18f));

    }


    //Rest before moving and changing locations
    IEnumerator RestCoroutine()
    {
        waiting = true;
        waitRand = UnityEngine.Random.Range(30.0f, 60.0f);

        //Disable animator
        gameObject.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(waitRand);

        //Enable animator after wait
        gameObject.GetComponent<Animator>().enabled = true;
        waiting = false;
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


    }
    //As soon as chick has reached adult hood, wait to die
    //IEnumerator Death()
    //{
    //    float death = UnityEngine.Random.Range(6000.0f,7000.0f); 
        
        
    //    yield return new WaitForSeconds(death);

    // //   Debug.Log(strName + " has died.  " + Time.time);
    //    deadFlag = true;

    //}

}
