using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Code from Unity Reference

    public Transform player;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    public bool bounds;





    //basically, if camera coordinates are within the map dimmensions, then offset.x = 0 and offset.y = 0.
    //else, set the x and y coordinates of the camera to a fixed value that it won't exceed
    void Update()
    {
       
        transform.position = new Vector3(player.position.x, player.position.y, minCameraPos.z); // Camera follows the player with specified offset position
    
        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    
    }
}
