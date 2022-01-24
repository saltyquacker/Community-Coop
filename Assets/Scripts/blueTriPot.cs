using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class blueTriPot : MonoBehaviour
//{
    //private bool isDragging;
    //private bool inPen;




    //public void OnMouseDown()
    //{
    //    isDragging = true;
    //    Debug.Log("Dragging");
    //}

    //public void OnMouseUp()
    //{
    //    isDragging = false;
    //    Debug.Log("Dropped");
    //    //if(inPen == true)
    //    //{
    //    //  Debug.Log("Pot In Pen");
    //    //}
    //    GlobalVar.bluePot -= 1;
    //    GlobalVar.blueFlag = true;
    //    transform.localPosition = new Vector3(-23.99902f, 4.499817f, 0);

    //}
    //void OnCollisionStay2D(Collision2D pen)
    //{

    //    //inPen = true;
    //    //Debug.Log("Pot In Pen");
    //    if (pen.gameObject.tag == "pen")
    //    {
    //     // inPen = true;
    //      Debug.Log("Triggered");


    //    }
    //}

    ////void OnTriggerExit(Collider pen)
    ////{
    ////    if (pen.name == "PenCollider")
    ////    {
    ////    Debug.Log("Cannot Drop Here");
    ////        inPen = false;
    ////    }
    ////}


    //void Update()
    //{
    //    if (GlobalVar.bluePot < 1)
    //    {
    //      //  Debug.Log("Cannot Drag");
    //    }
    //    else if(GlobalVar.bluePot>1)
    //    {

    //        if (isDragging)
    //        {
    //            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //            transform.Translate(mousePosition);
    //        }
    //    }

    //    if (inPen == true)
    //    {
    //        Debug.Log("Collider Working");
    //    }
    //    //if (isDragging)
    //    //{
    //    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //    //    transform.Translate(mousePosition);
    //    //}

    //}
//    public void OnMouseDown()
//    {
//        if (GlobalVar.bluePot > 0)
//        {
//            Debug.Log("Blue clicked.");
//            GlobalVar.bluePot -= 1;
//        }
//    }


//}
