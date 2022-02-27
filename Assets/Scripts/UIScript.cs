using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text name;
    public GameObject c;
    private string colour;

    public Sprite whiteChick; //White
    public Sprite yChick;  //Yellow
    public Sprite bChick; //Brown
    public Sprite dGChick; //DarkGrey
    public Sprite gChick;  //Green
    public Sprite lGChick;  //LightGrey
    public Sprite oChick; //Orange

    // Start is called before the first frame update
    void Start()
    {
        
        name.text = GlobalVar.roster[0].Name;
        colour = GlobalVar.roster[0].Colour;
        c = GameObject.Find("c"+ 0);




    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(GlobalVar.roster[0,0] + " is "+ GlobalVar.roster[0,1]);
        //"Behe is white"
        Debug.Log("Total: " + GlobalVar.maxInPen);

        for (int i=0; i < GlobalVar.maxInPen; i++){
            //Grab box name + chick name/ num
            GameObject boxName = GameObject.Find("b" + i);
            GameObject chickName = GameObject.Find("c" + i);
            Text tagName = GameObject.Find("n" + i).GetComponent<Text>();

            //For each index#
            string rostName = GlobalVar.roster[i].Name;
            string rostColour = GlobalVar.roster[i].Colour;
            tagName.text = rostName;

            boxName.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            chickName.GetComponent<SpriteRenderer>().enabled = true;
        if (rostColour == "white")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = whiteChick;
            
        }
        else if (rostColour == "yellow")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = yChick;
                
            }
        else if(rostColour == "brown")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = bChick;
                
            }
        else if (rostColour == "dGrey")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = dGChick;
               
            }
        else if (rostColour == "green")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = gChick;
        }
        else if (rostColour == "lGrey")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = lGChick;
        }
        else if (rostColour == "orange")
        {
                chickName.GetComponent<SpriteRenderer>().sprite = oChick;
        }


        }


    }
}
