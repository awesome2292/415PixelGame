using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   //DIALOGUE TRIGGER CLASS
   //Serves as a mediation code between the player "i" interaction and the display of the dialogue boxes
   //Checks to see if the associated GameObject is an inventory object or an NPC

    //Message that the object or NPC will display when prompted
    public Dialogue dialogue;

    //Bools to check if GameObject is either NPC or Object
    public bool talks;
    public bool item;

    //TRIGGER DIALOGUE
    //Call the StartDialogue() function from the Dialogue Manager
    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
