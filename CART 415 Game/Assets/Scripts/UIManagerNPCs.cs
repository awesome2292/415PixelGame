using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import this to quickly access Unity's UI classes
using VIDE_Data; //Import this to use VIDE Dialogue's VD class

public class UIManagerNPCs : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject instructionsBox;
    public GameObject itemPopUp;
    public SpriteController player;

    public Text NPC_text; //References
    public Text NPC_name;
    public Text[] PLAYER_text; //References

    public KeyCode continueButton; //Button to continue

    IEnumerator NPC_TextAnimator;

    private List<Text> currentChoices = new List<Text>();

    private bool keyDown = false;
    public bool paused = true;
    bool dialoguePaused = false;

    public void Start()
        {

        dialogueBox.GetComponent<CanvasGroup>().alpha = 0.0f;

            //Disable UI when starting just in case
            NPC_text.gameObject.SetActive(false);
            NPC_name.gameObject.SetActive(false);
            foreach (Text t in PLAYER_text)
                t.transform.parent.gameObject.SetActive(false);

            //Subscribe to some events and Begin the Dialogue
            VD.OnNodeChange += UpdateUI;
            VD.OnEnd += End;
            //VD.BeginDialogue(GetComponent<VIDE_Assign>()); //This is the first most important method when using VIDE
        }

    public void Begin(VIDE_Assign dialogue)
    {
        VD.BeginDialogue(dialogue); //Begins dialogue, will call the first OnNodeChange
        //Debug.Log("Entered Begin");
    }

    public void Interact(VIDE_Assign dialogue)
    {
        if (!VD.isActive)
        {
            Begin(dialogue);
            //Debug.Log(dialogue);
        }

    }

    //Check if a dialogue is active and if we are NOT in a player node in order to continue
    public void Update()
        {


        if (Input.anyKey && paused)
        {
            Debug.Log("Unpause");
            instructionsBox.SetActive(false);
            paused = false;
        }

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
              
                    Start();
            }
        }

        //This method is called by the UI Buttons! Check their button component in the Inspector!
        public void SelectChoiceAndGoToNext(int playerChoice)
        {
            keyDown = true;
            VD.nodeData.commentIndex = playerChoice; //Setting this when on a player node will decide which node we go next
            // //Disable item popup and disable pause
            //if (itemPopUp.activeSelf)
            //{
            //    dialoguePaused = false;
            //    itemPopUp.SetActive(false);
            //}
            //
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
                dialogueBox.GetComponent<CanvasGroup>().alpha = 1.0f;
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
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0.0f;
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


    void GiveItem(int itemIndex)
    {
        player.ItemInventory.Add(player.Items[itemIndex]);
        itemPopUp.SetActive(true);
        string text = "You've got a <color=yellow>" + player.Items[itemIndex] + "</color>!";
        itemPopUp.transform.GetChild(0).GetComponent<Text>().text = text;
        dialoguePaused = true;
    }



    #region DIALOGUE CONDITIONS 

  //  //DIALOGUE CONDITIONS --------------------------------------------
  //
  //  //When this returns true, it means that we did something that alters the progression of the dialogue
  //  //And we don't want to call Next() this time
  //  bool PreConditions(VIDE_Assign dialogue)
  //  {
  //      var data = VD.nodeData;
  //
  //      if (VD.isActive) //Stuff we check while the dialogue is active
  //      {
  //          //Check for extra variables
  //          //This one finds a key named "item" which has the value of the item thats gonna be given
  //          //If there's an 'item' key, then we will assume there's also an 'itemLine' key and use it
  //          if (!data.isPlayer)
  //          {
  //              if (data.extraVars.ContainsKey("item") && !data.dirty)
  //              {
  //                  if (data.commentIndex == (int)data.extraVars["itemLine"])
  //                  {
  //                      if (data.extraVars.ContainsKey("item++")) //If we have this key, we use it to increment the value of 'item' by 'item++'
  //                      {
  //                          Dictionary<string, object> newVars = data.extraVars; //Clone the current extraVars content
  //                          int newItem = (int)newVars["item"]; //Retrieve the value we want to change
  //                          newItem += (int)data.extraVars["item++"]; //Change it as we desire
  //                          newVars["item"] = newItem; //Set it back   
  //                          VD.SetExtraVariables(25, newVars); //Send newVars through UpdateExtraVariable method
  //                      }
  //
  //                      //If it's CrazyCap, check his stock before continuing
  //                      //If out of stock, change override start node
  //                      if (VD.assigned.alias == "CrazyCap")
  //                          if ((int)data.extraVars["item"] + 1 >= player.Items.Count)
  //                              VD.assigned.overrideStartNode = 28;
  //
  //
  //                      if (!player.ItemInventory.Contains(player.Items[(int)data.extraVars["item"]]))
  //                      {
  //                          GiveItem((int)data.extraVars["item"]);
  //                          return true;
  //                      }
  //                  }
  //              }
  //          }
  //          else
  //          {
  //              if (data.extraVars.ContainsKey("outCondition"))
  //              {
  //                  if (data.extraVars.ContainsKey("condInfo"))
  //                  {
  //                      int[] nodeIDs = VD.ToIntArray((string)data.extraVars["outCondition"]);
  //                      if (VD.assigned.interactionCount < nodeIDs.Length)
  //                          VD.SetNode(nodeIDs[VD.assigned.interactionCount]);
  //                      else
  //                          VD.SetNode(nodeIDs[nodeIDs.Length - 1]);
  //                      return true;
  //                  }
  //              }
  //
  //          }
  //      }
  //      else //Stuff we do right before the dialogue begins
  //      {
  //          //Get the item from CrazyCap to trigger this one on Charlie
  //          if (dialogue.alias == "Mama")
  //          {
  //              if (player.ItemInventory.Count > 0 && dialogue.overrideStartNode == -1)
  //              {
  //                  dialogue.overrideStartNode = 16;
  //                  return false;
  //              }
  //          }
  //      }
  //      return false;
  //  }
    #endregion
}
