﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy();
    }

    private void Destroy()
    {
        if(transform.childCount < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
