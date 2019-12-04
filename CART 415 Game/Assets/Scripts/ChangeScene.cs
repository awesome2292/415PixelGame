using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //CHANGESCENE CLASS
    //This script is responsible for changing scenes during gameplay

    public Scene newScene;
    public bool leaveScene = false; //boolean used to make sure that the player can leave the scene when pressing I
    public bool leaveSceneWalk = false; //boolean used to make sure that the player can leave onCollision

    void Update()
    {

        if (leaveScene && Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(newScene.handle);
        }

        if (leaveSceneWalk)
        {
            SceneManager.LoadScene(newScene.handle);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
