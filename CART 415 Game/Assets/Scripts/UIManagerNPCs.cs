using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Import this to quickly access Unity's UI classes
using VIDE_Data; //Import this to use VIDE Dialogue's VD class

public class UIManagerNPCs : MonoBehaviour
{
    public GameObject dialogueBox;
    public GameObject itemPopUp;
    public SpriteController player;
    public ChangeScene scene;

    public Text NPC_text; //References
    public Text NPC_name;
    public Text item_text;
    public Text[] PLAYER_text; //References

    public KeyCode continueButton; //Button to continue

    IEnumerator NPC_TextAnimator;

    private List<Text> currentChoices = new List<Text>();

    private bool keyDown = false;
    public bool paused = true;
    bool dialoguePaused = false;

    void Awake()
    {
        // VD.LoadDialogues(); //Load all dialogues to memory so that we dont spend time doing so later
        //An alternative to this can be preloading dialogues from the VIDE_Assign component!
        int numUIManagers = GameObject.FindGameObjectsWithTag("UIManager").Length;
        if (numUIManagers != 1)
        {
            Destroy(gameObject);
        }
        // if more then one music player is in the scene
        //destroy ourselves
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteController>();
        

        //Loads the saved state of VIDE_Assigns and dialogues.
        VD.LoadState("Tree Village", true);
    }

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
    }

    public void Interact(VIDE_Assign dialogue)
    {
        var doNotInteract = PreConditions(dialogue);
        if (doNotInteract) return;

        if (!VD.isActive)
        {
            Begin(dialogue);
        }

    }

    //Check if a dialogue is active and if we are NOT in a player node in order to continue
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteAll();
            if (System.IO.Directory.Exists(Application.dataPath + "/VIDE/saves"))
            {
                System.IO.Directory.Delete(Application.dataPath + "/VIDE/saves", true);
                #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
                #endif
            }

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
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

            //Disable item popup and disable pause
            if (itemPopUp.activeSelf && Input.GetKeyDown(continueButton))
            {
                Debug.Log(itemPopUp.activeSelf);
                dialoguePaused = false;
                itemPopUp.SetActive(false);
            }

        }

        else
        {

            //Disable item popup and disable pause
            if (itemPopUp.activeSelf && Input.GetKeyDown(continueButton))
            {
                Debug.Log(itemPopUp.activeSelf);
                dialoguePaused = false;
                itemPopUp.SetActive(false);
            }

            Start();
        }
    }

    //This method is called by the UI Buttons! Check their button component in the Inspector!
    public void SelectChoiceAndGoToNext(int playerChoice)
    {
        keyDown = true;
        VD.nodeData.commentIndex = playerChoice; //Setting this when on a player node will decide which node we go next

        if (!dialoguePaused) //Only if
        {
            VD.Next(); //We call the next node and populate nodeData with new data. Will fire OnNodeChange.
        }
        
    }

    //Every time VD.nodeData is updated, this method will be called. (Because we subscribed it to OnNodeChange event)
    void UpdateUI(VD.NodeData data)
    {

        if (data.tag.Length > 0)
            NPC_name.text = data.tag;
        else
            NPC_name.text = VD.assigned.alias;


        WipeAll(); //Turn stuff off first

        PostConditions(data);

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
        item_text.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            NPC_text.text += letter;
            item_text.text += letter;
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
    public void End(VD.NodeData data)
    {
        dialogueBox.GetComponent<CanvasGroup>().alpha = 0.0f;
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
        VD.EndDialogue(); //Third most important method when using VIDE     

        VD.SaveState("Tree Village", true); //Saves VIDE stuff related to EVs and override start nodes
        WipeAll();
    }

    //Just in case something happens to this script
    void OnDisable()
    {
        VD.OnNodeChange -= UpdateUI;
        VD.OnEnd -= End;
    }


    public void GiveItem(int itemIndex)
    {
        player.ItemInventory.Add(player.Items[itemIndex]);
        itemPopUp.SetActive(true);
        string text = "You got a " + player.Items[itemIndex] + "!";
        item_text.text = text;
        dialoguePaused = true;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(item_text.text));        
    }

    public void LoseItem(int itemIndex)
    {
        player.ItemInventory.Remove(player.Items[itemIndex]);
        itemPopUp.SetActive(true);
        string text = "You gave the " + player.ItemInventory[itemIndex] + "!";
        item_text.text = text;
        dialoguePaused = true;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(item_text.text));
    }



    #region DIALOGUE CONDITIONS 

    //DIALOGUE CONDITIONS --------------------------------------------

    //When this returns true, it means that we did something that alters the progression of the dialogue
    //And we don't want to call Next() this time
    bool PreConditions(VIDE_Assign dialogue)
    {
        var data = VD.nodeData;

        if (VD.isActive) //Stuff we check while the dialogue is active
        {
            //Check for extra variables
            //This one finds a key named "item" which has the value of the item thats gonna be given
            //If there's an 'item' key, then we will assume there's also an 'itemLine' key and use it
            if (!data.isPlayer)
            {
                if (data.extraVars.ContainsKey("item") && !data.dirty)
                {
                    if (data.commentIndex == (int)data.extraVars["itemLine"])
                    {
                        if (data.extraVars.ContainsKey("item++")) //If we have this key, we use it to increment the value of 'item' by 'item++'
                        {
                            Dictionary<string, object> newVars = data.extraVars; //Clone the current extraVars content
                            int newItem = (int)newVars["item"]; //Retrieve the value we want to change
                            newItem += (int)data.extraVars["item++"]; //Change it as we desire
                            newVars["item"] = newItem; //Set it back   
                            VD.SetExtraVariables(25, newVars); //Send newVars through UpdateExtraVariable method
                        }

                        if (!player.ItemInventory.Contains(player.Items[(int)data.extraVars["item"]]))
                        {
                            GiveItem((int)data.extraVars["item"]);
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (data.extraVars.ContainsKey("outCondition"))
                {
                    if (data.extraVars.ContainsKey("condInfo"))
                    {
                        int[] nodeIDs = VD.ToIntArray((string)data.extraVars["outCondition"]);
                        if (VD.assigned.interactionCount < nodeIDs.Length)
                            VD.SetNode(nodeIDs[VD.assigned.interactionCount]);
                        else
                            VD.SetNode(nodeIDs[nodeIDs.Length - 1]);
                        return true;
                    }
                }

            }
        }
        return false;
    }


    void PostConditions(VD.NodeData data)
    {
        //Don't conduct extra variable actions if we are waiting on a paused action
        if (data.pausedAction) return;

        if (!data.isPlayer) //For player nodes
        {
            //Replace [WORDS]
            ReplaceWord(data);

            //Checks for extraData that concerns font size (Grumpy)
            if (data.extraData[data.commentIndex].Contains("fs"))
            {
                int fSize = 60;

                string[] fontSize = data.extraData[data.commentIndex].Split(","[0]);
                int.TryParse(fontSize[1], out fSize);
                NPC_text.fontSize = fSize;
            }
            else
            {
                NPC_text.fontSize = 32;
            }
        }
    }

    void ReplaceWord(VD.NodeData data)
    {
        if (data.comments[data.commentIndex].Contains("[NAME]"))
            data.comments[data.commentIndex] = data.comments[data.commentIndex].Replace("[NAME]", VD.assigned.gameObject.name);

        if (data.comments[data.commentIndex].Contains("[WEAPON]"))
            data.comments[data.commentIndex] = data.comments[data.commentIndex].Replace("[WEAPON]", player.ItemInventory[0].ToLower());
    }
    #endregion

}
