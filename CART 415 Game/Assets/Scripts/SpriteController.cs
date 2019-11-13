using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data; //Import this to use VIDE Dialogue's VD class


public class SpriteController : MonoBehaviour
{

    //SPRITE CONTROLLER CLASS
    //This class is essential to the life of the main sprite (player)
    //It controls the movement of the sprite
    //As well as it's ability to interact with objects within the world

    //Variables
    public UIManagerNPCs diagUI;
    public ChangeScene scene;
    public Animator animator;
    public float WalkingSpeed;
    public float RunningSpeed;

    //Stored current VA when inside a trigger
    public VIDE_Assign inTrigger;


    void Update()
    {
        //Character Movement with Animation
        SpriteMove();

        //Interact with NPCs when pressing I
        if (Input.GetKeyDown(KeyCode.I))
        {
            TryInteract();
        }
    }

    //When the sprite collides with an NPC, assign the NPC's VIDE_Assign node
    //to the sprite
    void OnTriggerEnter2D(Collider2D other)
    {

            if (other.GetComponent<VIDE_Assign>() != null)
            {
                inTrigger = other.GetComponent<VIDE_Assign>();
            }

        if (other.gameObject.tag == "NewScene")
        {
            scene = other.GetComponent<ChangeScene>();
            scene.leaveScene = true;
            Debug.Log("leaveScene = " + scene.leaveScene);
            Debug.Log("newScene = " + scene.newScene);
        }

        else
        {
            scene.leaveScene = false;
            Debug.Log(scene.leaveScene);
        }


    }

    //When no longer colliding with an object, the object no longer becomes
    //interactable
    void OnTriggerExit2D()
    {
        inTrigger = null;

    }
    
    //This function is responsible for the Horizontal movement of the sprite
    void SpriteMove()
    {
        if (!VD.isActive)
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

    //Casts a ray to see if we hit an NPC and, if so, we interact
    void TryInteract()
    {
        /* Prioritize triggers */

        if (inTrigger)
        {            
            diagUI.Interact(inTrigger);
            return;
        }
    }
    }
