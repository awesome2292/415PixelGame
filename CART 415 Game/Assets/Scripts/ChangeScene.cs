using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Scene newScene;
    public Transform SpawnPointTree;
    public Transform SpawnPointRight;
    public GameObject player;
    public bool leaveScene = false;
    public bool leaveSceneWalk = false;

    // Update is called once per frame

    void Update()
    {
        
        if (leaveScene && Input.GetKeyDown(KeyCode.I))
        {   
            SceneManager.LoadScene(newScene.handle);
            player.transform.position = SpawnPointTree.position;

        }

        if (leaveSceneWalk)
        {
            SceneManager.LoadScene(newScene.handle);
            player.transform.position = SpawnPointRight.position;
        }
    }


}
