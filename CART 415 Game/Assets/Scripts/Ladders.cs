using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour
{

    //LADDERS CLASS
    //Responsible for the vertical movement of the sprite when it collides with a ladder or rope
    //in combination with the up or down arrow keys

    //This variable controls how fast the player climbs
    public float ClimbingSpeed;


    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag=="Player" && (Input.GetKey(KeyCode.W) || Input.GetKey("up")))
        {
            //Debug.Log("player collides with ladder");
            other.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, ClimbingSpeed, 0.0f);
        }

    }

}
