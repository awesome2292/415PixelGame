using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Scene newScene;
    public bool leaveScene = false;

    // Update is called once per frame

    void Update()
    {
        if (leaveScene && Input.GetKeyDown(KeyCode.I))
        {   
            SceneManager.LoadScene(newScene.handle);
        }
    }
}
