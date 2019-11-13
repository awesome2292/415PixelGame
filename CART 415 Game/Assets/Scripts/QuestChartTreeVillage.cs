using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VIDE_Data;
using UnityEngine.UI;

public class QuestChartTreeVillage : MonoBehaviour
{
    public static VIDE_Assign assigned;

    public GameObject questChartContainer;
    public GameObject ovGameObject;
    public GameObject peGameObject;

    //Tasks
    static int totalInteractions = 12;
    //static int cylinderGuyTotal = 9;
    static List<string> interactedWith = new List<string>();
    //static List<int> cylinderGuyInteractions = new List<int>();


    void Awake()
    {
        //Will load saved progress in PlayerPrefs
        LoadProgress();
    }

    void OnEnable()
    {
        assigned = GetComponent<VIDE_Assign>();
    }

    void Update()
    {
        //This is checking if the QuestChart UI Manager is 
        if (gameObject != null)
            if (questChartContainer.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.X))
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
            }
    }

    // Will Use the SetVisible method to switch the visibility of a comment
    // When a comment is not visible, its content will not be included in the nodeData arrays
    // The method will also add info to an ExtraVariables key to mark the completion of a quest
    public static void SetQuest(int quest, bool visible)
    {
        VD.SetVisible(assigned.assignedDialogue, 0, quest, visible);
        Dictionary<string, object> newEV = VD.GetExtraVariables(assigned.assignedDialogue, 1);
        newEV["complete"] += "[" + quest.ToString() + "]";
        VD.SetExtraVariables(assigned.assignedDialogue, 1, newEV);
    }

    //Check some of the Quests completion
    public static void CheckTaskCompletion(VD.NodeData data)
    {
        if (VD.assigned == null) return;

        if (!interactedWith.Contains(VD.assigned.gameObject.name))
            interactedWith.Add(VD.assigned.gameObject.name);

        //Check
        // 0 Talk to Everyone
        // 1 Listen to CylinderGuy
        // 2 Get all items from Crazy Cap
        // 3 Threaten Charlie

        if (interactedWith.Count == totalInteractions) SetQuest(0, false);
    }

    //Set Charlie quest
    public void SetSecretaryQuestStarted()
    {
        SetQuest(1, false);
    }

    public static void SaveProgress()
    {
        var player = GameObject.Find("Player").GetComponent<VIDEDemoPlayer>();

        List<string> items = player.demo_ItemInventory;
        PlayerPrefs.SetInt("interactedWith", interactedWith.Count);
        PlayerPrefs.SetInt("example_ItemInventory", items.Count);

        for (int i = 0; i < interactedWith.Count; i++)
        {
            PlayerPrefs.SetString("interWith" + i.ToString(), interactedWith[i]);
        }

        for (int i = 0; i < items.Count; i++)
        {
            PlayerPrefs.SetString("item" + i.ToString(), items[i]);
        }
    }

    public static void LoadProgress()
    {
        var player = GameObject.Find("Player").GetComponent<VIDEDemoPlayer>();

        if (!PlayerPrefs.HasKey("interactedWith")) return;

        List<string> items = new List<string>(); ;

        interactedWith = new List<string>();

        for (int i = 0; i < PlayerPrefs.GetInt("interactedWith"); i++)
        {
            interactedWith.Add(PlayerPrefs.GetString("interWith" + i.ToString()));
        }


        for (int i = 0; i < PlayerPrefs.GetInt("example_ItemInventory"); i++)
        {
            items.Add(PlayerPrefs.GetString("item" + i.ToString()));
        }

        player.demo_ItemInventory = items;
    }
}