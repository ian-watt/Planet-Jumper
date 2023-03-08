using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    public LayerMask gravityMask;
    public float gravityRange;
    public float rotateSpeed;


    void Update()
    {

        //rotate the planet based on a value set in the inspector
        transform.Rotate(0, rotateSpeed, 0);

        //based on a set radius (gravityRange), Physics.CheckSphere returns a boolean to see if there is anything within the sphere that it is looking for (remember layermask)

        //check to see if there is anything within the gravity range of the planet, if there is then apply gravity to it
        if (Physics.CheckSphere(transform.position, gravityRange, gravityMask))
        {
            HandleGravity();
        }
        else if (!Physics.CheckSphere(transform.position, gravityRange, gravityMask))
        {
            
        }

    }

    private void HandleGravity()
    {
        //find the difference between the planet and the player, and apply negative force of that normalized difference multiplied by gravity
        //we normalize the difference here so that gravity isn't weaker or stronger depending on your distance, but that is up for debate really
        Vector3 diff = transform.position - GameManager.Instance.player.transform.position;
        GameManager.Instance.player.rb.AddForce(diff.normalized * GameManager.Instance.gravity);
    }

}

