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

public class FrogMovement : MonoBehaviour
{

    private Vector2 target;
    private Vector2 position;
    private float speed = 1.5f;
    private bool waiting = false;
    private float waitRand;
    private float randomDirection;
    public Sprite pausedleft;
    public Sprite pausedright;
    public Sprite pausedfront;
    public Sprite pausedback;
    public string direction;
    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.transform.position;
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
            gameObject.GetComponent<SpriteRenderer>().sprite = pausedleft;
        }else if(direction == "right")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pausedright;
        }
        else if (direction =="front")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pausedfront;
        }
        else if (direction == "back")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = pausedback;
        }

    }



    void randoLocation()
    {
        randomDirection = UnityEngine.Random.Range(0.0f, 1.0f); //x or y
        if (randomDirection > 0.5f)
        {
            target = new Vector2(UnityEngine.Random.Range(14.52f, -24.3f), gameObject.transform.position.y);
            if(target.x > gameObject.transform.position.x)
            {
                direction = "right";
            }
            else
            {
                direction = "left";
            }
            
        }
        else if (randomDirection <0.5f)
        {
            target = new Vector2(gameObject.transform.position.x, UnityEngine.Random.Range(-6.84f, 10.81f));
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

    IEnumerator RestCoroutine()
    {
        waiting = true;
        waitRand = UnityEngine.Random.Range(5.0f, 60.0f);

        //Disable animator
        gameObject.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(waitRand);

        //Enable animator after wait
        gameObject.GetComponent<Animator>().enabled = true;
        if (direction == "left")
        {
            gameObject.GetComponent<Animator>().Play("leftfrog");
        }
        else if (direction == "right")
        {
            gameObject.GetComponent<Animator>().Play("rightfrog");
        }
        else if (direction == "front")
        {
            gameObject.GetComponent<Animator>().Play("frontfrog");
        }
        else if (direction == "back")
        {
            gameObject.GetComponent<Animator>().Play("backfrog");
        }
        waiting = false;
    }

}
