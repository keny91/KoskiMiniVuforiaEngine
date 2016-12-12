using UnityEngine;
using System.Collections;

public static class AdditionalMath 
{

    /// <summary>
    /// Determine the angle with which a ray has hit a surface.
    /// </summary>
    /// <param name="hit">Rayhit from the ray under analysis.</param>
    /// <returns></returns>
    public static float getSurfaceAngleFromRayHit(RaycastHit hit){
        float angle = Vector3.Angle(hit.normal, Vector3.up);
        return angle;

        }




    /// <summary>
    /// Example: when colliding with an enemy the player will be thrown in the opposite direction
    /// </summary>
    /// <param name="CollisionDirection">From point B to point A.</param>
    /// <param name="velocity">The object´s velocity to be modified.</param>
    /// /// <param name="force">Repulsion modifier.</param>
    public static void EstimateHorizontalRepulsion(Vector3 CollisionDirection, ref Vector3 velocity, float force)
    {
        Vector3 RepulsionDirection = -CollisionDirection.normalized;
        float magnitude = velocity.magnitude;

        if (magnitude < 1)
            magnitude = 1;

        else if (magnitude < 100)
            magnitude = magnitude * 2 / 50;
        else
            magnitude = 2;

        velocity = RepulsionDirection * magnitude * force;

    }


    /// <summary>
    /// Add a verticalForce to levate the object from the ground
    /// </summary>
    /// <param name="velocity">The object´s velocity to be modified.</param>
    /// <param name="force">Repulsion modifier.</param>
    public static void AddVerticalRepulsion(ref Vector3 velocity, float force)
    {

        if (velocity.y<-1)
            velocity.y = 0;
        velocity.y += force;

    }



    /// <summary>
    ///     /// Calculate the vertical and horizontal advance when facing a slope
    /// </summary>
    /// <param name="velocityHorizontal"> Either from the X or the Z axis</param>
    /// <param name="velocityVertical"> In this engine the gravitational pulse is only active in the Y axis</param>
    /// <param name="angleInDegrees">Angle of inclination</param>
    /// /// <param name="Ctrl">Collider controller that will be altered</param>
    public static void SlopeModuleModifier(ref float velocityHorizontal, ref float velocityVertical, float angleInDegrees, ref CollisionControler Ctrl)
    {
        float ClimbModule = Mathf.Abs(velocityHorizontal);

        float ClimbVelocityY = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad) * ClimbModule;
        if(velocityVertical <= ClimbVelocityY)
        {
            // Debug.Log("Jump on a slope");
            velocityVertical = ClimbVelocityY;
            velocityHorizontal = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad) * ClimbModule * Mathf.Sign(velocityHorizontal);
            // Since we are climbing upwards v.y>0 -> set manually: bellow = true; so we can jump
            Ctrl.below = true;
            Ctrl.climbingSlope = true;
        }

    }
}
