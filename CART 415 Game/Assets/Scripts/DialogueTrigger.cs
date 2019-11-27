using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   //DIALOGUE TRIGGER CLASS
   //Serves as a mediation code between the player "i" interaction and the display of the dialogue boxes
   //Checks to see if the associated GameObject is an inventory object or an NPC

    //Message that the object or NPC will display when prompted
    public UIManagerNPCs dialogue;

    private void Awake()
    {
        int numCanvas = GameObject.FindGameObjectsWithTag("Canvas").Length;
        if (numCanvas != 1)
        {
            Destroy(gameObject);
        }
        // if more then one music player is in the scene
        //destroy ourselves
        else
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
