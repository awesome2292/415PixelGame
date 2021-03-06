﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VIDE_Data;
using UnityEngine.UI;

public class QuestChartTreeVillage : MonoBehaviour
{
    //QUESTCHARTTREEVILLAGE CLASS
    //This script is responsible for saving and loading progress when the player
    //interacts with other NPCs or Objects
    //Does not include inventory progress (yet)

    public VIDE_Assign assigned;

    
    //Tasks
    static int totalInteractions = 12;
    static List<string> interactedWith = new List<string>();


    void Awake()
    {
        //Will load saved progress in PlayerPrefs
        LoadProgress();
    }

    void OnEnable()
    {
        assigned = GetComponent<VIDE_Assign>();
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

        //player.demo_ItemInventory = items;
    }
}