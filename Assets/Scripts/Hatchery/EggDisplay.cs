using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Net;


public class EggDisplay : MonoBehaviour
{


    //3x6 nest grid
    private float timeLeft = 30.0f;
    //timeLeft -= Time.deltaTime;
    public GameObject eggTimer;
    public GameObject nestObject;
    public GameObject eggObject;

    void Update()
    {
        
        
        for(int j=0;j<GlobalVar.eggSpots; j++)
        {
            nestObject = GameObject.Find("nest"+j);
            nestObject.GetComponent<SpriteRenderer>().enabled = true;
        }

        for (int i = 0; i < GlobalVar.eggCount; i++)
        {
            //Debug.Log("NEST" + i);
            eggTimer = GameObject.Find("time" + i);
            //eggTimer.SetActive(true);
            //Debug.Log("TIMER: " + eggTimer.name + " ENABLED");
           // eggTimer.GetComponent<Text>().enabled = true;
            eggObject = GameObject.Find("egg" + i);
            eggObject.GetComponent<SpriteRenderer>().enabled = true;
            //eggObject.GetComponent<EggDisplay>().enabled = true;

        }


    }
}
