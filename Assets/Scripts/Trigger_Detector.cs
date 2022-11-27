﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Detector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.name.Equals("Player_Object"))
        {
            GetComponent<AudioSource>().Play();
        }
    }


}
