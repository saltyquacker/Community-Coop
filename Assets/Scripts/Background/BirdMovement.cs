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
public class BirdMovement : MonoBehaviour
{
    private Vector2 target;
    private Vector2 position;
    private float speed = 2.0f;
    private bool waiting = false;
    private float waitRand;
    private float randomDirection;
    private float randomColourFloat;
    private string randomColour;

    public string direction;
    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.transform.position;
        direction = "right";
    }

    // Update is called once per frame
    void Update()
    {

        if (!waiting)
        {
            float step = speed * Time.deltaTime;
            if (Vector2.Distance(transform.position, target) < 0.001f)
            {
                randoLocation();
                StartCoroutine(RestCoroutine());
            }



            transform.position = Vector2.MoveTowards(transform.position, target, step);




        }

        if (direction == "left")
        {
           // gameObject.GetComponent<SpriteRenderer>().sprite = pausedleft;
        }
        else if (direction == "right")
        {
           // gameObject.GetComponent<SpriteRenderer>().sprite = pausedright;
        }
     

    }



    void randoLocation()
    {
        randomDirection = UnityEngine.Random.Range(0.0f, 1.0f); //start at left or start at right
        if (direction == "right")
        {
            direction = "left";
            target = new Vector2(-22.84f, UnityEngine.Random.Range(-4.74f, 7.27f));


        }
        else if (direction =="left")
        {
            direction = "right";
            target = new Vector2(6.44f, UnityEngine.Random.Range(-4.74f, 7.27f));
           
        }

        //Change colour too
        randomColourFloat = UnityEngine.Random.Range(0.0f, 4.0f);
        if (randomColourFloat < 1.0f)
        {
            randomColour = "redbird";
        }
        else if (randomColourFloat > 1.0f&& randomColourFloat < 2.0f)
        {
            randomColour = "bluebird";
        }
        else if (randomColourFloat > 2.0f && randomColourFloat < 3.0f)
        {
            randomColour = "brownbird";
        }
        else if (randomColourFloat > 4.0f)
        {
            randomColour = "whitebird";
        }

    }

    IEnumerator RestCoroutine()
    {
        waiting = true;
        waitRand = UnityEngine.Random.Range(5.0f, 60.0f);

        //Disable animator
        gameObject.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(waitRand);

        //Enable animator after wait
        gameObject.GetComponent<Animator>().enabled = true;
   
        gameObject.GetComponent<Animator>().Play(randomColour);
        if (direction == "right")
        {
            gameObject.GetComponent<SpriteRenderer>().flipX =true;
        }
        else if (direction == "left")
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        
        waiting = false;
    }
}
