using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textblinker : MonoBehaviour
{
    public Text blinktext;
    public static float blinktime = 1.0f;

    private bool IsTextOn = true;
    private float timestore = blinktime;

    // Start is called before the first frame update
    void Start()
    {
        blinktext.text = "Press anything to begin!";
    }

    // Update is called once per frame
    void Update()
    {
        blinktime -= Time.deltaTime;
        if (blinktime <= 0 && IsTextOn == true)
        {
            blinktext.text = "";
            blinktime = timestore;
            IsTextOn = false;
        }

        else if(blinktime <= 0 && IsTextOn == false)
        {
            blinktext.text = "Press anything to begin!";
            blinktime = timestore;
            IsTextOn = true;
        }


        
    }
}
