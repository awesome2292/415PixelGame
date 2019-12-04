using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
   //DIALOGUE TRIGGER CLASS
   //Checks if there are more than one Canvas objects in the scene
   //If so, destroy the clones

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
