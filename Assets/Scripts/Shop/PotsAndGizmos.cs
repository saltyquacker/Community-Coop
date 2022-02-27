//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PotsAndGizmos : MonoBehaviour
//{
//    //Health pots: Adds more time - max 1 pot per chicken
//    //Speed up growth
//    //Sickness Pot: cures chicken ailments
//    //Lay eggs faster??

//    //>>>>When chicken lays egg, put it in new slot with timer instead of name?
//    //All chickens are female
//    //Must buy a rooster?

//    //Purchase chicken page

//    //Rent-A-Roo!
//    //Welcome to our finest selection of coloured cocks!
//    //choice of 3 roos



//    public GameObject shop;
//    bool inputFlag = false;
//    public GameObject hatchery;
//    bool inputHatch = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //Figure out slots again or something
//        //Sickness, FastLay
//        //Choice of Common, Rare, Elite Rooster?
//        //Or just 3 randos





        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        

//        if (Input.GetKeyDown(KeyCode.O))
//        {
//            if (inputFlag == true)
//            {
//                shop.SetActive(false);
//                inputFlag = false;
//            }
//            else if (inputFlag == false)
//            {
//                shop.SetActive(true);
//                inputFlag = true;
//            }
//        }
//        if (Input.GetKeyDown(KeyCode.H))
//        {
//            if (inputHatch == true)
//            {
//                hatchery.SetActive(false);
//                inputHatch = false;
//            }
//            else if (inputHatch == false)
//            {
//                hatchery.SetActive(true);
//                inputHatch = true;
//            }
//        }

//    }
//}
