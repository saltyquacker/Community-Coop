using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeProgress : MonoBehaviour
{
  
    public Slider nextUpgrade;
    public Text nextDollar;
    // Start is called before the first frame update
    void Start()
    {
        //Start at level 0


    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVar.mylevel < 6)
        {
            nextDollar.text = "Next Coop Upgrade: $" + GlobalVar.levels[GlobalVar.mylevel];
            int currentAmount = GlobalVar.currencyDollar;
            int nextAmount = GlobalVar.levels[GlobalVar.mylevel];
            float progress = (float)currentAmount / (float)nextAmount;

            //Debug.Log(progress);

            nextUpgrade.value = progress;
        }
        else
        {
            nextDollar.text = "Maximum Coop Upgraded!";
        }


    }
}
