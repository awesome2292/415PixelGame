using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //MUSICPLAYER CLASS
    //Detects if there exist multiple music players in a class and 
    //deletes them while keeping the original

    private void Awake()
    {
        int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
        if (numMusicPlayers != 1)
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
