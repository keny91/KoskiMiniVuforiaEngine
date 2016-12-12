using UnityEngine;
using System.Collections;

public class PhysicsController {

    float accelerationTimeAirborne;
    float accelerationTimeGrounded;
    float velocityXsmoothing, velocityZsmoothing;

    public PhysicsController(float accelerationAirborne, float accelerationGrounded)
    {
        accelerationTimeAirborne = accelerationAirborne;
        accelerationTimeGrounded = accelerationGrounded;
    }

    public float gravityEstimation(float JumpHeight, float timeToFullJump)
    {
        float gravity = -(JumpHeight * 2) / Mathf.Pow(timeToFullJump, 2);
        Debug.Log("<color=green> Gravity has been set to: </color>" + gravity);
        return gravity;
    }

    public float JumpVelocityEstimation(float gravity, float timeToFullJump)
    {
        float jumpVelocity = Mathf.Abs(gravity) * timeToFullJump;
        Debug.Log("<color=green> jumpVelocity has been set to: </color>" + jumpVelocity);
        return jumpVelocity;
    }

    
    public void EstimateHorizontalRepulsion(Vector3 CollisionDirection, ref Vector3 velocity)
    {
        Vector3 RepulsionDirection = -CollisionDirection.normalized;
        float magnitude = velocity.magnitude;

        velocity = RepulsionDirection * magnitude * 20;

    }



    /// <summary>
    /// Specify movements (start/target) and specify the axis in which are, so they can be smoothed
    /// </summary>
    /// <param name="velocity">Current velocity</param>
    /// <param name="targetVelocity">Velocity we aim to achieve</param>
    /// <param name="axis">Which axis expresed with 'x','y' or 'z' can be concatenated in a single string.</param>
    /// <param name="OnGround">Boolean depicting which modifier should be use to smooth the velocity</param>
    public void VelocityTransition(ref Vector3 velocity, Vector3 targetVelocity, char[] axis, bool OnGround)
    {
        float SmoothMod;
        SmoothMod =(OnGround) ?  accelerationTimeGrounded : accelerationTimeAirborne;

        for(int i = 0; i< axis.Length; i++)
        {

            if (axis[i] == 'z' || axis[i] == 'Z')
                velocity.z = Mathf.SmoothDamp(velocity.z, targetVelocity.z, ref velocityZsmoothing, SmoothMod);
            else if (axis[i] == 'x' || axis[i] == 'X')
                velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity.x, ref velocityXsmoothing, SmoothMod);
            else if (axis[i] == 'y' || axis[i] == 'Y')
                velocity.y = Mathf.SmoothDamp(velocity.x, targetVelocity.y, ref velocityXsmoothing, SmoothMod);
            else
                Debug.LogWarning("<color=yellow> Invalid input in VelocityTransition /<char/>: </color>" + axis[i]);
        }
    }

}

