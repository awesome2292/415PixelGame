using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    
//SPRITE CONTROLLER CLASS
//This class is essential to the life of the main sprite (player)
//It controls the movement of the sprite
//As well as it's ability to interact with objects within the world

    //Variables
    public Animator animator;
    public float WalkingSpeed;
    public float RunningSpeed;

    //Inputs the current interacted object into a variable
    public GameObject currentInterObj = null;
    public DialogueTrigger currentInterObjScript = null;


    void Update()
    {
        //Character Movement with Animation
        SpriteMove();
       
        if(currentInterObjScript == null)
        {
            return;
        }

        // Check if interactable object is an NPC
        //If so, trigger the dialogue box
        if (currentInterObjScript.talks && Input.GetKey("i"))
        {
            currentInterObjScript.TriggerDialogue();       
        }

        //Check if interactable object is an Item
        if (currentInterObjScript.item && Input.GetKey("i"))
        {
            currentInterObjScript.TriggerDialogue();
        }


    }

    //When the Sprite collides with an object tagged "interNPC", enter all acquired information about the object
    //into the currentInterObj variables
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("interNPC"))
        {
            //Debug.Log(other.name);
            currentInterObj = other.gameObject;
            //Debug.Log(currentInterObj);
            currentInterObjScript = currentInterObj.GetComponent<DialogueTrigger>();
            //Debug.Log(currentInterObjScript);
        }
        
        
    }

    //This function is responsible for the Horizontal movement of the sprite
    void SpriteMove()
    {
                animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
                animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
                Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
                transform.position = transform.position + horizontal * WalkingSpeed * Time.deltaTime;

        //The sprite walks faster when shift is held down
                if (Input.GetKey("left shift"))
                {
                    transform.position = transform.position + horizontal * RunningSpeed * Time.deltaTime;
                }
    }    
}
