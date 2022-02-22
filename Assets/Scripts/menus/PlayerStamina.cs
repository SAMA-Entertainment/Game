using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public ThirdPersonMovement movement;
    public Slider progressBar;
    
    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        float ratio = movement.Stamina / movement.maxstamina;
        progressBar.value = ratio;
    }
    
}
