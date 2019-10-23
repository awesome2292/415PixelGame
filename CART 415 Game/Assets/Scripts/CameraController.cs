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





    //basically, if camera coordinates are within the map dimmensions, the sprite remains centered in the camera view
    //else, set the x and y coordinates of the camera to a fixed value that it won't exceed
    void Update()
    {
        // Camera follows the player with specified out of bounds x and y coords
        transform.position = new Vector3(player.position.x, player.position.y, minCameraPos.z);
    
        //if the camera exceeds the set bounds, then its x and y coords remain fixed
        //until the player returns back in bounds of the map
        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    
    }
}
