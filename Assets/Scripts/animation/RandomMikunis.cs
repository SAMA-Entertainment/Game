using System;
using System.Collections;
using System.Collections.Generic;
using mikunis;
using UnityEngine;

public class RandomMikunis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision c)
    {
        GameObject gm = c.gameObject;
        if (gm.CompareTag("Mikuni"))
        {
            Rigidbody rb = gm.GetComponent<Rigidbody>();
            Mikuni mikuni = gm.GetComponent<Mikuni>();
            rb.isKinematic = true;
            rb.freezeRotation = true;
        }
    }
}
