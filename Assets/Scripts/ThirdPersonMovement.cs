using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        // normalized: removes the ability to move faster in diagonals
        
        Vector3 moveVector = Vector3.zero;
 
        if (controller.isGrounded == false)
        {
            moveVector += Physics.gravity;
        }

        if (dir.magnitude >= 0.1f) // enough movement
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move((moveVector + movDir.normalized * speed) * Time.deltaTime);
        }
        else
        {
            controller.Move(moveVector * Time.deltaTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        RollingMikuniController mikuniController =
            other.gameObject.GetComponent<RollingMikuniController>();
        if (mikuniController != null)
        {
            // Debug.Log("Mikuni detected");
            mikuniController.PlayerNear(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit " + other.gameObject.name);
    }
}
