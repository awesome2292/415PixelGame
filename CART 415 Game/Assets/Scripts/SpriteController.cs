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
    //And event triggers with the Tree Village quest

    #region VARIABLES
    //Variables
    public UIManagerNPCs diagUI; //UIManager code that is crucial for the dialogue system to run
    public ChangeScene scene; //The new scene assigned to the portal that the player collides with

    public Animator animator;
    public float WalkingSpeed;
    public float RunningSpeed;
    
    public bool questStarted; //Start the quest when you speak to the Secretary
    public bool talk = true;

    //Stored current VA when inside a trigger
    public VIDE_Assign inTrigger;

    public float moralCompass = 0.5f;

        
    public List<string> Items = new List<string>(); //List for the possible acquireable items
    public List<string> ItemInventory = new List<string>(); //Items that the player posseses in the current playthrough
    private GameObject[] Sprites;

    #endregion

    private void Awake()
    {
        //moralCompass = 0.5f;
        // if more then one Sprite is in the scene
        //destroy the one in the scene
        talk = true;
        Sprites = GameObject.FindGameObjectsWithTag("Player");
        if (Sprites.Length!= 1)
        {
            Destroy(gameObject);
        }        
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        //Destroy the Music player from the title screen
        Destroy(GameObject.Find("Sky_loop"));
    }


    void Update()
    {
        //If a diagUI isn't assigned to the current UIManager from another scene,
        //assign it
        if(diagUI == null)
        {
            diagUI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManagerNPCs>();
        }


        //Character Movement with Animation
        SpriteMove();

        //Interact with NPCs when pressing I
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryInteract();
        }
    }

    #region COLLIDERS
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

        if (other.gameObject.tag == "NewSceneWalk")
        {
            scene = other.GetComponent<ChangeScene>();
            scene.leaveSceneWalk = true;
        }

        else
        {
            scene.leaveSceneWalk = false;
            Debug.Log(scene.leaveScene);
        }


    }

    //If the player remains in a scene portal
    //then change the "leaveScene" boolean to true so that
    //the player is able to leave the scene when I is pressed
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "NewScene")
        {
            scene = other.GetComponent<ChangeScene>();
            scene.leaveScene = true;
        }
    }

    //When no longer colliding with an object, the object no longer becomes
    //interactable
    void OnTriggerExit2D()
    {
        inTrigger = null;
        scene.leaveScene = false;

    }

    #endregion

    //This function is responsible for the Horizontal and Vertical movement of the sprite
    public void SpriteMove()
    {
        if (!VD.isActive)
        {
            animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
            Vector3 horizontal = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
            transform.position = transform.position + horizontal * WalkingSpeed * Time.deltaTime;

            //The sprite walks faster when shift is held down
            if (Input.GetKey("left shift") || Input.GetKey("right shift"))
            {
                transform.position = transform.position + horizontal * RunningSpeed * Time.deltaTime;
            }
        }
    }

    //If the Sprite is colliding with an NPC and the I key is pressed, this function will run
    void TryInteract()
    {
        /* Prioritize triggers */
        if (inTrigger)
        {
            diagUI.Interact(inTrigger);
            return;
        }
    }

    #region EVENT TRIGGERS
    //List of event trigger functions that change the dialogues after speaking to a certain NPC
    //or adding an object to the player's inventory when interacted with
    public void StartQuest(bool quest)
    {
        if (quest)
        {
            Debug.Log("Quest started!");
            var Mama = GameObject.Find("NPC_MamaCockatoo");
            var Hawk = GameObject.Find("NPC_Smugbirb");
            var Grumpy = GameObject.Find("NPC_Grumpy");
            Mama.GetComponent<VIDE_Assign>().overrideStartNode = 17; 
            Hawk.GetComponent<VIDE_Assign>().overrideStartNode = 10;
            Grumpy.GetComponent<VIDE_Assign>().overrideStartNode = 2;
        }

    }

    public void TalkKids(bool quest)
    {
        if (quest)
        {
            Debug.Log("Spoke to Mama!");
            var Smol = GameObject.Find("NPC_SmolCockatoo");
            Smol.GetComponent<VIDE_Assign>().overrideStartNode = 6;
        }

    }

    public void TalkMama(bool quest)
    {
        if (quest)
        {
            Debug.Log("Spoke to her son!");
            var Mama = GameObject.Find("NPC_MamaCockatoo");
            Mama.GetComponent<VIDE_Assign>().overrideStartNode = 8;
        }

    }

    public void TalkGrumpy(bool quest)
    {
        if (quest)
        {
            Debug.Log("Spoke to Grumpy!");
            var Twin1 = GameObject.Find("NPC_Twin1");
            Twin1.GetComponent<VIDE_Assign>().overrideStartNode = 11;
            var Twin2 = GameObject.Find("NPC_Twin2");
            Twin2.GetComponent<VIDE_Assign>().overrideStartNode = 4;
        }

    }
    

    public void GetSeed(bool receive)
    {
        Debug.Log("Entered GetSeed()!");

        if (receive)
        { 

            if (!ItemInventory.Contains("Seed"))
            {
                Debug.Log("Receive the seed!");
                diagUI.GiveItem(0);
            }
        }
    }

    public void GiveSeed(bool give)
    {
        Debug.Log("Entered GiveSeed()!");
        var Grumpy = GameObject.Find("NPC_Grumpy");

        if (give)
        {

            if (ItemInventory.Contains("Seed"))
            {
                var index = ItemInventory.FindIndex(a => a.Contains("Seed"));
                Debug.Log("Give the seed!");
                diagUI.LoseItem(index);
            }

            else if (!ItemInventory.Contains("Seed"))
            {
                VD.EndDialogue();
                Grumpy.GetComponent<VIDE_Assign>().overrideStartNode = 8;
            }
        }
    }


    public void GiveRock(bool give)
    {
        var Hawk = GameObject.Find("NPC_Smugbirb");

        if (give)
        {
            Debug.Log("Entered GiveRock()!");
            if (ItemInventory.Contains("Rock"))
            {
                var index = ItemInventory.FindIndex(a => a.Contains("Rock"));
                Debug.Log("Give the rock!");
                diagUI.LoseItem(index);
            }

            else if (!ItemInventory.Contains("Rock"))
            {
                Debug.Log("Don't have rock!");
                VD.EndDialogue();
                Hawk.GetComponent<VIDE_Assign>().overrideStartNode = 17;
            }
        }
    }

    public void GetRock(bool receive)
    {
        Debug.Log("Entered GetRock()!");

        if (receive)
        {

            if (!ItemInventory.Contains("Rock"))
            {
                Debug.Log("Receive the rock!");
                diagUI.GiveItem(1);
                Destroy(GameObject.Find("Rock"));
            }
        }
    }

    public void GetApple(bool receive)
    {
        Debug.Log("Entered GetApple()!");

        if (receive)
        {

            if (!ItemInventory.Contains("Apple"))
            {
                Debug.Log("Receive the apple!");
                diagUI.GiveItem(2);
            }
        }
    }

    public void LeaveRock(bool leave)
    {
        Debug.Log("Entered LeaveRock()!");

        if (leave)
        {                
         Debug.Log("Leave the rock!");
         Destroy(GameObject.Find("Rock"));
            
        }
    }
    #endregion
}
