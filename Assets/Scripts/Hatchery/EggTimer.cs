using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class EggTimer : MonoBehaviour
{
    private float timeRemaining = 100;
    private GameObject timerText;
    private bool timerIsRunning;
    private int num;
    private string name;
    private string parserName;
    private GameObject eggObject;
    private bool readyToHatch;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        readyToHatch = false;
        //timerText = GetComponent<Text>();
        //Debug.Log(timerText);
        //string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        //Debug.Log(niceTime);
        //Get # of current object
       
        //timerText.GetComponent<Text>().text = "HERE";
       // 

    }
    private void Update()
    {
        name = this.gameObject.name;
        parserName = name.Replace("egg", "");
        num = int.Parse(parserName);
        timerText = GameObject.Find("time" + num);
        Debug.Log(timerText.name);
        if (this.gameObject.GetComponent<SpriteRenderer>().enabled == true&&timerIsRunning ==true)
        {

            //string timerName = this.gameObject.name;
            if (timeRemaining > 0)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60F);
                int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);
                string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
                timeRemaining -= Time.deltaTime;
                timerText.GetComponent<Text>().text = niceTime;
                //timerText.GetComponent<Text>().text= timeRemaining.ToString();
            }
            else
            {
                Debug.Log("Ready to Hatch!");
                timerText.GetComponent<Text>().text = "HATCHING";
                readyToHatch = true;
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }




        //READY TO HATCH ONCLICK EVENTS
    }
}
