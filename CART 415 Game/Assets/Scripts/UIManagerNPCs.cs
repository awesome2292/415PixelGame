using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import this to quickly access Unity's UI classes
using VIDE_Data; //Import this to use VIDE Dialogue's VD class

public class UIManagerNPCs : MonoBehaviour
{
    public GameObject canvas;
    public Text NPC_text; //References
    public Text NPC_name;
    public Text[] PLAYER_text; //References
    public KeyCode continueButton; //Button to continue

    IEnumerator NPC_TextAnimator;

    private List<Text> currentChoices = new List<Text>();

    private bool keyDown = false;

        public void Start()
        {
            canvas = GameObject.Find("Canvas");
            canvas.GetComponent<CanvasGroup>().alpha = 0.0f;

            //Disable UI when starting just in case
            NPC_text.gameObject.SetActive(false);
            NPC_name.gameObject.SetActive(false);
            foreach (Text t in PLAYER_text)
                t.transform.parent.gameObject.SetActive(false);

            //Subscribe to some events and Begin the Dialogue
            VD.OnNodeChange += UpdateUI;
            VD.OnEnd += End;
            VD.BeginDialogue(GetComponent<VIDE_Assign>()); //This is the first most important method when using VIDE
        }

    public void Begin(VIDE_Assign dialogue)
    {
        VD.BeginDialogue(dialogue); //Begins dialogue, will call the first OnNodeChange
        Debug.Log("Entered Begin");
    }

    public void Interact(VIDE_Assign dialogue)
    {
        if (!VD.isActive)
        {
            Debug.Log("Entered Dialogue");
            Begin(dialogue);
        }

    }

    //Check if a dialogue is active and if we are NOT in a player node in order to continue
    public void Update()
        {

        var data = VD.nodeData;
        
        currentChoices = new List<UnityEngine.UI.Text>();

        if (VD.isActive)
            {
            

            if (!data.pausedAction && data.isPlayer)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (data.commentIndex < currentChoices.Count - 1)
                        data.commentIndex++;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (data.commentIndex > 0)
                        data.commentIndex--;
                }
            }

            if (!VD.nodeData.isPlayer && Input.GetKeyUp(continueButton))
                {
                    if (keyDown)
                    {
                        keyDown = false;
                    }
                    else
                    {
                        VD.Next(); //Second most important method when using VIDE
                    }
                }
            }
            else
            {
                if (Input.GetKeyUp(continueButton))
                {
                    Start();
                }
            }
        }

        //This method is called by the UI Buttons! Check their button component in the Inspector!
        public void SelectChoiceAndGoToNext(int playerChoice)
        {
            keyDown = true;
            VD.nodeData.commentIndex = playerChoice; //Setting this when on a player node will decide which node we go next
            VD.Next();
        }

    //Every time VD.nodeData is updated, this method will be called. (Because we subscribed it to OnNodeChange event)
    void UpdateUI(VD.NodeData data)
    {

        if (data.tag.Length > 0)
            NPC_name.text = data.tag;
        else
            NPC_name.text = VD.assigned.alias;


        WipeAll(); //Turn stuff off first

        if (!data.isPlayer) //For NPC. Activate text gameobject and set its text
        {
                canvas.GetComponent<CanvasGroup>().alpha = 1.0f;
                NPC_name.gameObject.SetActive(true);
                NPC_text.gameObject.SetActive(true);
                NPC_text.text = data.comments[data.commentIndex];

            StopAllCoroutines();
            StartCoroutine(TypeSentence(NPC_text.text));


        }
            else //For Player. It will activate the required Buttons and set their text
            {
                for (int i = 0; i < PLAYER_text.Length; i++)
                {
                    if (i < data.comments.Length)
                    {
                        PLAYER_text[i].transform.parent.gameObject.SetActive(true);
                        PLAYER_text[i].text = data.comments[i];
                    }
                    else
                    {
                        PLAYER_text[i].transform.parent.gameObject.SetActive(false);
                    }

                    PLAYER_text[0].transform.parent.GetComponent<Button>().Select();
                }
            }

        }

    IEnumerator TypeSentence(string sentence)
    {
        NPC_text.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            NPC_text.text += letter;
            yield return null;
        }
    }

    //Set all UI references off
    void WipeAll()
        {
            NPC_text.gameObject.SetActive(false);
            NPC_name.gameObject.SetActive(false);
        foreach (Text t in PLAYER_text)
                t.transform.parent.gameObject.SetActive(false);
        }

        //This will be called when we reach the end of the dialogue.
        //Very important that this gets called before we call BeginDialogue again!
        void End(VD.NodeData data)
        {
        canvas.GetComponent<CanvasGroup>().alpha = 0.0f;
        VD.OnNodeChange -= UpdateUI;
            VD.OnEnd -= End;
            VD.EndDialogue(); //Third most important method when using VIDE     
            WipeAll();
        }

        //Just in case something happens to this script
        void OnDisable()
        {
            VD.OnNodeChange -= UpdateUI;
            VD.OnEnd -= End;
        }
    
}
