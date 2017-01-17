using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour
{


    public float jumpHeight = 5;
    public float timeToReachJumpHeight = 0.5f;
    // Other kinetic  
    private float gravity;
    private float jumpVelocity;

    private float doubleJumpVelocity;
    public bool doubleJumpEnabled = false;
    public bool canDoubleJump = true;

    public bool jumpPerformed;
    public bool doubleJumpPerformed;

    [HideInInspector]
    public float JumpDelay = 0.3f;


    /// <summary> 
    /// To reach the max height in the desired time, the gravity should have a specific value. 
    /// </summary> 
    /// <param name="JumpHeight">Max height the object should reach</param> 
    /// <param name="timeToFullJump">Time to reach full height</param> 
    /// <returns></returns> 
    private float gravityEstimation(float JumpHeight, float timeToFullJump)
    {
        float gravity = -(JumpHeight * 2) / Mathf.Pow(timeToFullJump, 2);
        Debug.Log("<color=green> Gravity has been set to: </color>" + gravity);
        return gravity;
    }




    /// <summary> 
    /// Acording to previously calculated gravity. This method estimates he starting object velocity to reach the height in the estimated time 
    /// </summary> 
    /// <param name="gravity">Value which continuosly increases speed in the falling process.</param> 
    /// <param name="timeToFullJump">Time to reach maximum height.</param> 
    /// <returns></returns> 
    private float JumpVelocityEstimation(float gravity, float timeToFullJump)
    {
        float jumpVelocity = Mathf.Abs(gravity) * timeToFullJump;
        Debug.Log("<color=green> jumpVelocity has been set to: </color>" + jumpVelocity);
        return jumpVelocity;
    }



    /// <summary> 
    /// Object Executes either a jump or a double jump. 
    /// If the hability to duo 
    /// </summary> 
    public void performJump(ref Vector3 velocity)
    {

        if (!jumpPerformed && !doubleJumpEnabled)
        {
            velocity.y = jumpVelocity;
            jumpPerformed = true;
            EnableDoubleJump();
            doubleJumpEnabled = true;
        }
        else if (doubleJumpEnabled && jumpPerformed && canDoubleJump)
        {
            velocity.y = doubleJumpVelocity;
            doubleJumpEnabled = false;
        }

    }


    /// <summary> 
    /// Triggers the possibility to double jump 
    /// </summary> 
    public void EnableDoubleJump()
    {
        doubleJumpEnabled = true;
    }




    public void AddFallingForce(ref Vector3 velocity)
    {
        velocity.y += gravity * Time.deltaTime;
    }

    /// <summary> 
    /// Reset the jumping parameters to the original state 
    /// </summary> 
    public void resetJump()
    {
        jumpPerformed = false;
        doubleJumpPerformed = false;
        doubleJumpEnabled = false;

    }



    // Use this for initialization 
    void Start()
    {
        //  Set Jump Velocity and Dynamic gravity 
        gravity = gravityEstimation(jumpHeight, timeToReachJumpHeight);
        jumpVelocity = JumpVelocityEstimation(gravity, timeToReachJumpHeight);
        doubleJumpVelocity = jumpVelocity * 0.7f;


    }

    // Update is called once per frame 
    void Update()
    {

    }
}