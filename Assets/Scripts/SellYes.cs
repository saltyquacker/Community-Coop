using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellYes : MonoBehaviour
{
    public void ClickYes()
    {
        GlobalVar.eggBank -= (GlobalVar.truncateDozen * 12);
        GlobalVar.currencyDollar += (GlobalVar.truncateDozen * GlobalVar.marketPrice);
        //Debug.Log("Clicked Yes");
    }
}
