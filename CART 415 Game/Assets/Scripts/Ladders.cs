using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour
{
    public float ClimbingSpeed;
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag=="Player" && Input.GetKey("up"))
        {
            Debug.Log("player collides with ladder");
            other.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, ClimbingSpeed, 0.0f);
        }

        else if(other.tag=="Player" && Input.GetKey("down"))
        {
            Debug.Log("player collides with ladder");
            other.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, ClimbingSpeed, 0.0f);
        }
    }

}
