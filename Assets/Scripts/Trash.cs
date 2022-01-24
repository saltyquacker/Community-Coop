using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{

    public int max;
    // Start is called before the first frame update
    public void OnMouseDown()
    {
        
        Debug.Log("Trash clicked.");
        int count = 0;
        bool deadFlag = false;
        int i = 0;
        Debug.Log("Started with: " + count);
        while (count!=max)//for(; ;)
        {
            try
            {
                GameObject chickenPreNum = GameObject.Find(count + "");
                //select chickens based on count
                ChickBehaviour cs = chickenPreNum.GetComponent<ChickBehaviour>();
                deadFlag = cs.deadFlag;
                //try
                //{
                //    ChickBehaviour cs = chickenPreNum.GetComponent<ChickBehaviour>();
                //    deadFlag = cs.deadFlag;
                //}
                //catch
                //{
                //    Debug.Log("Couldn't get script chicken #" + chickenPreNum); 

                //}




                if (deadFlag == true)
                {
                    Debug.Log("Dead chicken:" + chickenPreNum);
                    Destroy(chickenPreNum);
                    GlobalVar.maxInPen--;
                }
                else if (deadFlag == false)
                {
                    Debug.Log("Live chicken:" + chickenPreNum);
                }
                else
                {
                    Debug.Log("Chicken:" + chickenPreNum + " not found");
                }

            }catch
            {
                //Do nothing
            }
            count++;
            
        }
        Debug.Log("Finished with: "+count);

    }
    void Start()
    {
        max = GlobalVar.maxInPen;
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalVar.maxInPen > max)//If maximum chickens ever change, just check them anyways
        {
            max = GlobalVar.maxInPen;
        }
    }
}
