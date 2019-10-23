using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

//DIALOGUE CLASS
//Allows the devs to control the message inputs they can have for each object

    //Name of Object or NPC
    public string name;

    //Changes the size of the text boxes for each sentence
    [TextArea(3, 10)]

    //Content for each message
    public string[] sentences;
   }
