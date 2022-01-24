using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockMarket : MonoBehaviour
{
    //Stock Price starts at 2$ a dozen, can show price per egg too

    public int currentPricePerDozen;
    public float countdownToPriceChange =500;
    public int flux;
    public bool posNeg;
    public float hour = 5.0f;
    public int previousPricePerDozen;
    public Sprite up;
    public Sprite down;
    public Sprite none;
    public GameObject stockVisual;
    void Start()
    {
        currentPricePerDozen = 2;


    }

    // Update is called once per frame
    void Update()
    {
        
        countdownToPriceChange -= Time.deltaTime * hour;

        if (countdownToPriceChange < 1)
        {
            countdownToPriceChange = 500;

            if (currentPricePerDozen >= 4)
            {
                flux = (int)Random.Range(-3.0f, 1.0f);  //Up or down 1 $
                currentPricePerDozen += flux;
            }
            else if (currentPricePerDozen > 2)
            {
                flux = (int)Random.Range(-3.0f, 3.0f);  //Up or down 1 $
                currentPricePerDozen += flux;
            }
            else if (currentPricePerDozen <= 2)
            {
                flux = (int)Random.Range(0.0f, 3.0f);  //Up or down 1 $
                currentPricePerDozen += flux;
            }

           
            if (previousPricePerDozen > currentPricePerDozen)
            {
                //stock going down
                stockVisual.GetComponent<SpriteRenderer>().sprite = down;
                Debug.Log("STOCK GO DOWN");
            }
            else if (previousPricePerDozen < currentPricePerDozen)
            {
                //stock going up
                stockVisual.GetComponent<SpriteRenderer>().sprite = up;
                Debug.Log("STOCK GO UP");

            }
            else if(previousPricePerDozen==currentPricePerDozen)
            {
                stockVisual.GetComponent<SpriteRenderer>().sprite = none;
                Debug.Log("STOCK GO NONE");
            }
            Debug.Log("Current Price per Dozen: " + currentPricePerDozen+ "$");
            previousPricePerDozen = currentPricePerDozen;
        }
        

        GlobalVar.marketPrice = currentPricePerDozen;

    }
}
