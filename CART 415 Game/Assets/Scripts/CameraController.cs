using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //CAMERACONTRLLER CLASS
    //Used to set the bounds of the camera to the associated map
    //While attaching it to the player

    public GameObject sprite;
    public Transform player;

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

    public bool bounds;

    //Find the sprite and assign the camera's coordinates to the player's
    private void Awake()
    {
        sprite = GameObject.FindGameObjectWithTag("Player");
        player = sprite.transform;    
    }



    //If camera coordinates are within the map dimmensions, the sprite remains centered in the camera view
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
