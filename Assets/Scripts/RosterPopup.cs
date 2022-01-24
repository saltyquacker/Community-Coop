using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class RosterPopup : MonoBehaviour
////Roster like manager scene
//{
//    bool visible = false;

//    public GameObject slots;


//    // Start is called before the first frame update
//    void Start()
//    {
//        slots = GameObject.Find("SlotGrid");

//        Debug.Log("Rows: " + GlobalVar.slotRow);
//        slots.GetComponent<SpriteRenderer>().size = new Vector2(8, GlobalVar.slotRow);
     
        

//    }

//    // Update is called once per frame
//    void Update()
//    {

//        //Display Slot grid with chickens each slot
//        GameObject rosterPanel = GameObject.Find("RosterPanel");
//        GameObject slotGrid = GameObject.Find("SlotGrid");

//        if (Input.GetKeyDown(KeyCode.I))
//        {
//            if (visible == false)
//            {
//                //Roster doesnt show chickens past 7? Pen works so its fine. can trouble shoot 
//                //Originally had maxInPen-1 for some weird reason
//                //Debug.Log("In Pen: " + GlobalVar.maxInPen);
//                for (int i = 0; i < GlobalVar.maxInPen; i++)
//                {
//                    GameObject chickNum = GameObject.Find("s" + i);
//                    chickNum.GetComponent<SpriteRenderer>().enabled = true;
//                    //Debug.Log("at chick#" + i);
//                }

//                rosterPanel.GetComponent<SpriteRenderer>().enabled = true;
//                slotGrid.GetComponent<SpriteRenderer>().enabled = true;
//                visible = true;

//            }
//            else if (visible == true)
//            {
//                for (int i = 0; i < GlobalVar.maxInPen; i++)
//                {
//                    GameObject chickNum = GameObject.Find("s" + i);
//                    chickNum.GetComponent<SpriteRenderer>().enabled = false;
//                }

//                rosterPanel.GetComponent<SpriteRenderer>().enabled = false;
//                slotGrid.GetComponent<SpriteRenderer>().enabled = false;
//                visible = false;
//            }
//        }

//        //Changing y position depending on int div
//        //Slot POS Y VALUES
//        //7 -0.11
//        //6 0.488
//        //5 1.089
//        //4 1.702
//        //3 2.301
//        //2 2.905
//        //1 3.493
//        float sl1 = (float)3.493;
//        float sl2 = (float)2.905;
//        float sl3 = (float)2.301;
//        float sl4 = (float)1.702;
//        float sl5 = (float)1.089;
//        float sl6 = (float)0.488;
//        float sl7 = (float)-0.110;

//        float xVal = (float)-0.340;
//        float zVal = (float)-3.200;
//        if (GlobalVar.slotRow == 1)
//        {
//            slots.transform.position = new Vector3(xVal, sl1, zVal);
//        }
//        else if(GlobalVar.slotRow == 2)
//        {
//            slots.transform.position = new Vector3(xVal, sl2, zVal);
//        }
//        else if(GlobalVar.slotRow == 3)
//        {
//            slots.transform.position = new Vector3(xVal, sl3, zVal);
//        }
//        else if(GlobalVar.slotRow == 4)
//        {
//            slots.transform.position = new Vector3(xVal, sl4, zVal);
//        }
//        else if(GlobalVar.slotRow == 5)
//        {
//            slots.transform.position = new Vector3(xVal, sl5, zVal);
//        }
//        else if (GlobalVar.slotRow == 6)
//        {
//            slots.transform.position = new Vector3(xVal, sl6, zVal);
//        }
//        else if (GlobalVar.slotRow == 7)
//        {
//            slots.transform.position = new Vector3(xVal, sl7, zVal);
//        }

//    }
//}
