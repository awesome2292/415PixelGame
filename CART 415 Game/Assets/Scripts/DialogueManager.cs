using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    //DIALOGUE MANAGER CLASS
    //This class is responsible for all the display of the content within the dialogues
    //and the dialogue box itself

    //Display Text for the name and message
    public Text nameText, dialogueText;
    public bool hidden; //still don't know if this is needed or not (responsible for 'hiding' the box)
    private GameObject dialogueBox;

    //This queue allows for a new array of sentences to form each time you interact with the object
    private Queue<string> sentences;



    // We create a new queue
    void Start()
    {
        sentences = new Queue<string>();

        if (dialogueBox == null)
        {
            dialogueBox = GameObject.Find("DialogueBox");
        }

    }

    //Tried to make this function where you could press 'o', and the sentences would change
    //everytime you pressed the button. Still doesn't work...
    public void KeyNextSentence()
    {
        if (Input.GetKey("o"))
        {
            Debug.Log("NextSentence should be displayed");
            DisplayNextSentence();
        }
    }

    //The StartDialogue function is responsible for initiating the start message each time you press 'i'
    //And then, by clicking the 'continue' button, the next sentences will display one after the other
    public void StartDialogue(Dialogue dialogue)
    {

        nameText.text = dialogue.name;
        sentences.Clear();
        dialogueBox.SetActive(true);
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        
        }

    //This makes sure that the dialoague box disappears
    public void DisableDialogueBox()
    {
        dialogueBox.SetActive(false);
    }
    
    //DisplayNextSentences displays every new sentence in the queue based on a Coroutine Time sequence
    //If you run out of sentences, then the dialogue will 'end'
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            
            EndDialogue();
            return;
        }
            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));

    }

    //This function serves to create the typewriter effect when displaying the sentence
    //by incrementing each character after the other
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    //This function 'ends' the dialogue, which will disable the dialogue box
    //and have it disappear until prompted again
    void EndDialogue() 
    {
        DisableDialogueBox();
     Debug.Log("End of conversation");
     
    
    }


}
