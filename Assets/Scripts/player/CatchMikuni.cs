using System;
using System.Collections;
using System.Collections.Generic;
using mikunis;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CatchMikuni : MonoBehaviour
{
    private bool iscatching;
    private List<Mikuni> mikuniscatched = new List<Mikuni>();
    public GameObject mikuniCounterObject;

    //public Dictionary<string, int> catchedMikuni = new Dictionary<string, int>(); 
    // Start is called before the first frame update
    void Start()
    {
        /*
        catchedMikuni.Add("coconut",0);
        catchedMikuni.Add("carrot", 0);
        catchedMikuni.Add("brown_mushroom",0);
        catchedMikuni.Add("red_mushroom",0);
        */
        iscatching = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Mikuni")) return;
        Debug.Log("Mikuni");
        if (Input.GetKeyDown(KeyCode.P) && !iscatching)
        {
            Mikuni mikunicatch = other.gameObject.GetComponent<Mikuni>();
            mikuniscatched.Add(mikunicatch);
            Destroy(other.gameObject);
            iscatching = true;
        }
        else
        {
            iscatching = false;
        }
        
    }

    private void LateUpdate()
    {
        mikuniCounterObject.GetComponent<TextMeshProUGUI>().text = $"Mikunis: {mikuniscatched.Count}";
    }
}
