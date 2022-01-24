using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Blue Pot is tied to lay rate
public class CoopEggCount : MonoBehaviour
{ 
    public float eggLocal=0;
    public SpriteRenderer spriteHouse;

    public GameObject spriteBushObject;
    public Sprite h2;
    public Sprite h3;
    public Sprite h4;
    public Sprite h5;
    public Sprite h6;
    public Sprite h7;
    public Vector3 largerHouse = new Vector3(-8.22f, 4.77f,0.5f);
    public Vector2 bushLocation = new Vector2(-7.90f, 2.40f);
    // Start is called before the first frame update
    void Start()
    {
        spriteHouse = gameObject.GetComponent<SpriteRenderer>();
    
    }

    // Update is called once per frame
    void Update()
    {
     
         if (GlobalVar.eggInCoop <= GlobalVar.maxEggInCoop)
         {
             
                //if (GlobalVar.pinkFlag == false)
               // {
           
            GlobalVar.eggRate = (0.01f * GlobalVar.adultsInPen)*(GlobalVar.mylevel+1);
            //Good rate is 0.01f

            // }
            // else
            // {
            //    GlobalVar.eggRate = 0.005f * GlobalVar.adultsInPen*5;

            //  }

            //Random number gen if bought rooster
            if (GlobalVar.justCollected == false)
            {
                if (GlobalVar.eggInCoop < GlobalVar.maxEggInCoop)
                {

                    eggLocal += Time.deltaTime * GlobalVar.eggRate;
                    GlobalVar.eggInCoop = (int)eggLocal;
                }
            }
            else if (GlobalVar.justCollected == true)
            {
                eggLocal = 0;
                GlobalVar.eggInCoop = 0;
                GlobalVar.justCollected = false;
            }
         }

        //Manage coop upgrades
        if (GlobalVar.mylevel == 1) {
            spriteHouse.sprite = h2;
            GlobalVar.maxEggInCoop = 50;
        }
        else if (GlobalVar.mylevel == 2)
        {
            spriteHouse.sprite = h3;
            GlobalVar.maxEggInCoop = 100;
        }
        else if (GlobalVar.mylevel == 3)
        {
            spriteHouse.sprite = h4;
            GlobalVar.maxEggInCoop = 200;
        }
        else if (GlobalVar.mylevel == 4)
        {
            spriteHouse.sprite = h5;
            GlobalVar.maxEggInCoop = 300;
        }
        else if (GlobalVar.mylevel == 5)
        {
            spriteHouse.sprite = h6;
            GlobalVar.maxEggInCoop = 400;
        }
        else if (GlobalVar.mylevel == 6)
        {
            GlobalVar.maxEggInCoop = 500;
            spriteHouse.sprite = h7;
            gameObject.transform.position = largerHouse;
            spriteBushObject.transform.position = bushLocation;
        }


    }

    //void OnMouseDown()
    //{
    //    if (GlobalVar.eggInCoop > 0)
    //    {
    //        GlobalVar.eggBank += GlobalVar.eggInCoop;
    //        Debug.Log(GlobalVar.eggInCoop + " eggs collected;; You now have " + GlobalVar.eggBank + " eggs.");
    //        eggLocal = 0;
    //        GlobalVar.eggInCoop = 0;
    //    }
    //}
}
