using System;
using System.Collections;
using System.Collections.Generic;
using mikunis;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    private float _turnSmoothVelocity;
    
    void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", transform.position);
        // Get Input From the player
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized; 
        // Normalize the vector so the speed is constant
        // Here you can modify the speed (sprint)
        
        
        // Add gravity force
        Vector3 moveVector = Vector3.zero;
        if (controller.isGrounded == false)
            moveVector += Physics.gravity;

        if (dir.magnitude >= 0.1f) // enough movement
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            animator.SetFloat("Speed", speed);
            controller.Move((moveVector + movDir.normalized * speed) * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            controller.Move(moveVector * Time.deltaTime);
        }
    }

    /**
     * This function is used to trigger the "Fleeing" state of nearby mikunis. Nearby mikunis
     * are all mikunis within the collider on the current GameObject.
     */
    void OnTriggerStay(Collider other)
    {
        Mikuni mikuniController = other.gameObject.GetComponent<Mikuni>();
        if (mikuniController != null)
        {
            // Debug.Log("Mikuni detected");
            mikuniController.PlayerNear(transform);
        }
    }
}
