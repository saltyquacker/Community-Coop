using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Linq;

public class BoardRoster : MonoBehaviour
{
    //Each square update chicken + prospective names

    private string boxName;
    private string boxNameNum;
    private int boxNum;
    private int maxBox;
    SpriteRenderer sprite;
    void Start()
    {
        ////Grab box# from name
        //boxName = this.gameObject.name;

        //boxNameNum = Regex.Match(boxName, @"\d+").Value;

        //boxNum = int.Parse(boxNameNum);
        ////Debug.Log("Box #" + boxNum);
        //maxBox = GlobalVar.maxInPen -1 ;
        //if ( boxNum <= maxBox )
        //{
        //    sprite = GetComponent<SpriteRenderer>();
        //    sprite.color = new Color(255,255,255,255);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
