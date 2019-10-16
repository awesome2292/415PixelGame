using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public float WalkingSpeed;
    public float RunningSpeed;

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        transform.position = transform.position + horizontal * WalkingSpeed * Time.deltaTime;
        animator.SetBool("ShiftPressed", false);

        if (Input.GetKey("left shift"))
        {
            transform.position = transform.position + horizontal * RunningSpeed * Time.deltaTime;
            animator.SetBool("ShiftPressed", true);
        }

        //if (Input.GetKey("i"))
        //{
        //    Debug.Log("The Character is interacting with the NPC");
        //}

    }
}
