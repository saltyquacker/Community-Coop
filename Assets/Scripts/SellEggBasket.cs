using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SellEggBasket : MonoBehaviour
{
    public bool isVisible = false;
    
    public GameObject popup;
    public Text sellQuestion;
    public GameObject sellButton;

    public void OnMouseDown()
    {
        
        if(isVisible == true)
        {
            isVisible = false;
        }
        else
        {
            isVisible = true;
        }
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        GlobalVar.truncateDozen = (int)GlobalVar.eggBank / 12;
        if(GlobalVar.truncateDozen == 0)
        {
            sellQuestion.text = "You do not have enough eggs to sell!";
            sellButton.SetActive(false);
        }
        else
        {
            sellQuestion.text = "Do you want to sell " + GlobalVar.truncateDozen + " dozen eggs?";
            sellButton.SetActive(true);

        }


        if (isVisible == true)
        {
            popup.SetActive(true);
        }
        else
        {
            popup.SetActive(false);
        }

    }
}
