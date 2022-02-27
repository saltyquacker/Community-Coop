//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ShopEggClick : MonoBehaviour
//{
//    void OnMouseDown()
//    {
//        if (GlobalVar.currencyDollar >= 20 && GlobalVar.eggCount<GlobalVar.eggSpots)
//        {
//            //Debug.Log("Egg Selected.");
//            GlobalVar.eggCount += 1;
//            GlobalVar.currencyDollar -= 20;
//            Debug.Log("EggCount: x" + GlobalVar.eggCount);
//        }
//        else if(GlobalVar.currencyDollar < 20)
//        {
//            Debug.Log("Insufficient Funds to Purchase!");
//        }
//        else if (GlobalVar.eggCount >= GlobalVar.eggSpots)
//        {
//            Debug.Log("Not Enough Space in Hatchery!");
//        }
//    }
//}
