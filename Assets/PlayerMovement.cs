using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // setting up variables for player movement
    // movementInput - gets user raw input 
    // atRightWall checks for collision with right bounds of level 
    // atLeftWall checks for collision with left bounds of level 
    // speed manages players speed
    private float movementInput;
    bool atRightWall = false;
    bool atLeftWall = false;
    float speed = 5f;
    private Vector3 originalPos;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = gameObject.transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "LeftWall")
        {
            atLeftWall = true;
        }

        else if (other.tag == "RightWall") 
        {
            atRightWall = true;
        }

        else if (other.tag == "obstacle") 
        {
            gameObject.transform.position = originalPos;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "LeftWall") 
        {
            atLeftWall = false;

        }
        else if (other.tag == "RightWall") 
        {
            atRightWall = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = Input.GetAxis("Horizontal");

        // check if at leftwall or right wall
        if (atLeftWall && (movementInput < 0))
        {
            movementInput = 0;
        }
        if (atRightWall && (movementInput > 0)) 
        {
            movementInput = 0;
        }

        transform.Translate(new Vector3(Time.deltaTime * speed * movementInput, 0, 0), Space.World);
    }
}
