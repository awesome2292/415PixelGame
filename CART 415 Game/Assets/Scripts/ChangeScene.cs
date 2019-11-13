using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Scene newScene;
    public bool leaveScene = false;
    public bool leaveSceneWalk = false;

    // Update is called once per frame

    void Update()
    {
        if (leaveScene && Input.GetKeyDown(KeyCode.I))
        {   
            SceneManager.LoadScene(newScene.handle);
            DontDestroyOnLoad(GameObject.Find("DialogueBox"));
        }

        if (leaveSceneWalk)
        {
            SceneManager.LoadScene(newScene.handle);
            DontDestroyOnLoad(GameObject.Find("DialogueBox"));
        }
    }
}
