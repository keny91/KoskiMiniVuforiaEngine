using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using System;


[RequireComponent(typeof(GameControllerScript))]
[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(JumpController))]

//[RequireComponent(typeof(BoxCollider))]

public class PlayerController : MonoBehaviour {

    PlayerCollisionController playerCollider;
    PhysicsController extraFunctionalities;
    JumpController jumpController;
   // JoyStickCustomController joyStickController;

    public int Direction;
     
    public LayerMask collisionMaskGround;

    protected GameObject ObjectController;
    [HideInInspector] public Transform PlayerCapsule;
    GameControllerScript theController;


    // Horizontal Movement
    public float moveSpeed = 20f;
    public Vector3 velocity;
    VelocityLimit VelocityLimits;

    // Acceleration
    float accelerationTimeAirborne = 0.5f;
    float accelerationTimeGrounded = 0.25f;

    // Disable clicking jump in consecutive instances
    bool disabledJumpButton = false;
    private float disabledJumpButtonTime = 0.5f;
    public bool jumpActivated = false;
    private float disabledRotateButton = 0.2f;
    public bool rotateButtonActivated = false;

    // Repulsion on Damage Interaction
    private float HorizontalPushByEnemyDmg = 50f;
    private float VerticalPushByEnemyDmg = 80f;
    private float VerticalPushByEnemyKill = 140f;

    // Parameters that constrain the movement velocity
    private const float xVelocityLimit = 50f;
    private const float zVelocityLimit = 50f;
    private const float FallingVelocityLimit = -50f;
    private const float RisingVelocityLimit = 50f;

    [HideInInspector]public float enemyDeathRepulsionVelY = 20f;


    CrossPlatformInputManager.VirtualAxis theHorizontalAxis;
    CrossPlatformInputManager.VirtualAxis theVerticalAxis;


    private float lastTimeHit;

    // Player Status:
    public bool isInvulnerable = false;
    public float InvulnerableTime = 2f;
    public bool isControllable = true;
    public float RecoveryTime = 0.4f;

    // animator
    private Animator animationElements;

    // Tag Map reference
    [HideInInspector]public TagDatabase theTagReference;

    //Respawn Process
    public void PlayerRespawn()
    {
        gameObject.transform.position = theController.Respawn.position;
        Debug.Log("Respawned");
    }



    /********************************************************/
    /*************      COIN INTERACTION    ****************/
    /********************************************************/

    void collectCoin()
    {

    }

    /********************************************************/
    /*************      ENEMY INTERACTION    ****************/
    /********************************************************/


        /// <summary>
        /// The player object takes a standard amount of damage. It decreases the player life count by one.
        /// It also calls the gameController to perform a check to determine if the game has finished.
        /// </summary>
    public void takeDamage()
    {
        // Decrease life count
        theController.ChangeLiveCount(-1);
        // Turn invulnerability On for a certain amount
        isControllable = false;
        Invoke("ResetUncontrolable", RecoveryTime);
        isInvulnerable = true;
        Invoke("ResetInvulnerability", InvulnerableTime);
        
    }

  


    /********************************************************/
    /*************      PLAYER COLLISIONS    ****************/
    /********************************************************/
   


    void OnTriggerEnter(Collider collision)
    {

        GameObject TheHitObject = collision.gameObject;
        int identifierTag = theTagReference.tagList[TheHitObject.tag];

        // Colliding with a damaging element will cause an inverse Forse push to in the X, Z axis.

        switch (identifierTag)
        {
            default:
                Debug.LogWarning("Hit NON-IDENTIFIED", TheHitObject);
                break;

            case 0:
                Debug.LogWarning("Hit NON-IDENTIFIED", TheHitObject);

                break;

            //Encountered Damaging object
            case 1:
                Debug.Log("Hit Damaging", TheHitObject);
                if (!isInvulnerable)
                {
                    takeDamage();
                    Vector3 colDir = collision.transform.position - gameObject.transform.FindChild("Origin").transform.position;

                    AdditionalMath.EstimateHorizontalRepulsion(colDir, ref velocity, HorizontalPushByEnemyDmg);
                    AdditionalMath.AddVerticalRepulsion(ref velocity, VerticalPushByEnemyDmg);
                }


                break;
            //Encountered Death object
            case 2:
                Debug.Log("Hit Death", TheHitObject);
                PlayerRespawn();
                takeDamage();
                break;
            //Encountered Vulnerable object
            case 3:
                Debug.Log("Hit Vulnerable", TheHitObject);
                    Vector3 ProvisionalVector = new Vector3(0, 0, 0);
                    TheHitObject.transform.parent.gameObject.SetActive(false); // deactivate instead of destroy so you could later reactivate (respawn) him
                    
                    AdditionalMath.AddVerticalRepulsion(ref velocity, VerticalPushByEnemyKill);

                //colliders.Add(collision.collider); // saves the enemy for later respawn

                //myBody.AddForce(0, 130, 0);
                break;
            //Encountered Goal object
            case 4:
                Debug.Log("Hit Goal", TheHitObject);
                theController.GoalReached();
                break;
            //Encountered Collectible object
            case 5:
                Debug.Log("Hit Collectible", TheHitObject);
                Collectible collect = (Collectible)TheHitObject.GetComponent(typeof(Collectible));
                theController.collectibleCollected(collect);
                TheHitObject.SetActive(false); // deactivate instead of destroy so you could later reactivate (respawn) him
                break;
        }

        TheHitObject = null;
    }



    /// <summary>
    /// Set the player to controllable
    /// </summary>
    void ResetUncontrolable()
    {
        isControllable = true;
    }

    /// <summary>
    /// Set the player to vulnerable
    /// </summary>
    void ResetInvulnerability()
    {
        isInvulnerable = false;
    }


   
    /// <summary>
    /// Do a traslation of the player object in the world.
    /// </summary>
    /// <param name="velocity">Determines the direction and magnitude of the displacement.</param>
    public void Move(Vector3 velocity)
    {
        

        playerCollider.UpdateRaycastOrigins();
        playerCollider.reset();

        // Limit velocity before the collisions keeps the 
        VelocityLimits.LimitVelocityCheck(ref velocity);
        //Debug.Log("Reajusted velocity." + velocity.y);

        if (velocity.y != 0)
        {
            playerCollider.verticalCollisions(ref velocity);
            //Debug.Log("after collision, Vertical speed is : " + velocity.y);
        }

        if (velocity.x != 0)
        {
            playerCollider.horizontalCollisionsInAxisX(ref velocity);
        }

        if (velocity.z != 0)
        {
            playerCollider.horizontalCollisionsInAxisZ(ref velocity);
        }

        transform.Translate(velocity);
        
    }


    
    /// <summary>
    /// Find out which should be the orientation of the animated object
    /// </summary>
    /// <param name="XZ">Vector containing the X and Z input</param>
    void DetermineOrientation(Vector2 XZ)
    {
        float magX = Mathf.Abs(XZ.x);
        float magZ = Mathf.Abs(XZ.y);
        Vector3 rot = new Vector3();

        if(magX > magZ)
        {
            if(Mathf.Sign(XZ.x) == 1)
                rot.y = 90;
            else
                rot.y = 270;
        }

        else if (magZ >= magX)
        {
            if (Mathf.Sign(XZ.y) == 1)
                rot.y = 0;
            else
                rot.y = 180;

        }




        PlayerCapsule.eulerAngles = rot;
    }


    /// <summary>
    /// If the jump button is disabled, this method will re-enable the
    /// </summary>
    void resetJumpButton()
    {
        disabledJumpButton = false;
    }



    void resetRotateButton()
    {
        rotateButtonActivated = false;
    }






    /********************************************************/
    /*************         START/UPDATE      ****************/
    /********************************************************/


    // Use this for initialization
    void Start ()
    {

        // Set Up Collisions
        //playerCollider = new CollisionControler();  // OR
        playerCollider = (PlayerCollisionController)GetComponent<PlayerCollisionController>();
        playerCollider.Init(4,6,4);  // 6 vertical divisions
        playerCollider.collisionMaskGround.value = 1 << LayerMask.NameToLayer("Ground");
        playerCollider.collisionMaskObjects.value = 1 << LayerMask.NameToLayer("WorldObjects");

        // Access to extra functions stored in an outside class
        extraFunctionalities = new PhysicsController(accelerationTimeAirborne, accelerationTimeGrounded);

        // Access to jump controller
        jumpController = (JumpController)GetComponent<JumpController>();


        // Set Up velocity limit for the player
        VelocityLimits = new VelocityLimit(xVelocityLimit, zVelocityLimit, FallingVelocityLimit, RisingVelocityLimit);

        //Determine the box of the player and the ray colliders
        playerCollider.CalculateRaySpacing();

        //Register ground layer for collisions
        

        // Find Capsule in the prefab.
        PlayerCapsule = transform.FindChild("Capsule");

        //Init Tag Reference List
        theTagReference = new TagDatabase();
        theTagReference.tagList = new Dictionary<string, int>();
        theTagReference.Initialize();

        /*
        // Get the joystick
        GameObject joyObject = GameObject.Find("MobileJoystick");
        try
        {
            joyStickController = (JoyStickCustomController)joyObject.GetComponent<JoyStickCustomController>();
            joyStickController.configureJoystick(false, true, true);
        }
        catch(Exception e)
        {
            Debug.LogError(e, joyObject);
        }
        */

        animationElements = this.GetComponent<Animator>();
        ObjectController = GameObject.Find("GameController");
        theController = GetComponent<GameControllerScript>();

        disabledJumpButtonTime = jumpController.timeToReachJumpHeight / 2;

        // Initialize player position
        PlayerRespawn();
    }


    /// <summary>
    /// Delay Jump to fit animation.
    /// <para> There might be a better option in the animator.</para>
    /// </summary>
    /// <param name="delayTime"> Time to be delayed</param>
    /// <returns></returns>
    IEnumerator DelayedJump(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // After Delay
        jumpController.performJump(ref velocity);
    }


    public void MoveForward()
    {
        Vector3 targetVelocity = new Vector3();
        targetVelocity.z += moveSpeed;
        extraFunctionalities.VelocityTransition(ref velocity, targetVelocity, "XZ".ToCharArray(), playerCollider.below);
        Move(velocity);
    }


    public void ChangeOrientationClockwise()
    {
        //Quaternion prov = transform.rotation;
        PlayerCapsule.Rotate(0, 90, 0);
        SwitchPlayerDirection(+1);
        //prov.y += 90;
        //transform.rotation = prov;
    }

    public void ChangeOrientationAntiClockwise()
    {
        //Quaternion prov = transform.rotation;
        SwitchPlayerDirection(-1);
        PlayerCapsule.Rotate(0, -90, 0);
        //transform.rotation = prov;
    }


    public void SwitchPlayerDirection(int amount)
    {
        Direction += amount;
        // keep between 1 and 4
        if (Direction > 4)
        {
            Direction = 1; // From 5 to 1
        }
        else if (Direction < 1)
            Direction = 4; // From 0 to 4
    }

    public Vector3 Advance()
    {
        Vector3 vel = new Vector3() ;
        Vector2 oriHelp = new Vector2();
        switch (Direction)
        {
            default:
                Debug.LogWarning("Error at Advance()... Unexpected Value");
                break;

            case 1:
                oriHelp.y = moveSpeed;
                vel.z += moveSpeed;
                break;
            case 2:
                oriHelp.x = moveSpeed;
                vel.x += moveSpeed;
                break;
            case 3:
                oriHelp.y = -moveSpeed;
                vel.z -= moveSpeed;
                break;
            case 4:
                oriHelp.x = -moveSpeed;
                vel.x -= moveSpeed;
                break;
        }

        DetermineOrientation(oriHelp);
        return vel;
    }


    // Update is called once per frame
    void Update()
    {
        if (theController.gameRunning) // Only update player if game is running
        {
            animationElements.speed = 1;
            Vector3 MVector = new Vector3();
            /*
            if (isControllable)
                MVector = joyStickController.getAxisOutput();
                */
            if (isControllable)
            {


                if (CrossPlatformInputManager.GetButtonDown("TurnClockwise"))
                {
                    Debug.Log("Pressed Turn1");
                    ChangeOrientationClockwise();
                    
                }


                if (CrossPlatformInputManager.GetButton("Advance")) // Instead of buttonDown since only returns true for a single frame
                {
                    MVector = Advance();
                    Debug.Log("Pressed Advance");
                }
                    


                //if (CrossPlatformInputManager.GetButtonDown("TurnAntiClockWise"))
                 //   ChangeOrientationAntiClockwise();
            }
            Vector3 targetVelocity = new Vector3();

            jumpController.AddFallingForce(ref velocity);


            targetVelocity.x += moveSpeed * MVector.y;
            targetVelocity.z += moveSpeed * MVector.x;
            extraFunctionalities.VelocityTransition(ref velocity, targetVelocity, "XZ".ToCharArray(), playerCollider.below);


            //    if(playerCollider.ObjectHit.collider !=null)
            //        playerCollider.RayHitCheck(playerCollider.ObjectHit);

            Move(velocity * Time.deltaTime);
            playerCollider.SpeedLimitByCollision(ref velocity);

            Vector2 velXZ = new Vector2(velocity.x, velocity.z);
            animationElements.SetFloat("Speed", velXZ.magnitude);
            DetermineOrientation(velXZ);



            if (!playerCollider.below)
                animationElements.SetBool("OnAir", true);
            else
                animationElements.SetBool("OnAir", false);

            //jumpActivated = CrossPlatformInputManager.GetButtonDown("Jump");
            if (jumpActivated || CrossPlatformInputManager.GetButtonDown("Jump") )
            {
                if (playerCollider.below && !disabledJumpButton)
                {
                    disabledJumpButton = true;
                    Invoke("resetJumpButton", disabledJumpButtonTime);
                    //jumpController.Invoke("performJump", jumpController.JumpDelay);
                    StartCoroutine(DelayedJump(jumpController.JumpDelay));

                    //velocity.y = jumpController.jumpVelocity;
                    animationElements.SetBool("Jumping", true);
                }
                else if (!playerCollider.below && jumpController.doubleJumpEnabled && !disabledJumpButton)
                {
                    jumpController.performJump(ref velocity);
                }

            }
            else if (playerCollider.below)
            {
                jumpController.resetJump();
                animationElements.SetBool("Jumping", false);
            }


            /*
            if (MVector.z != 0)
                transform.eulerAngles = (MVector.z > 0 && MVector.z != 0) ? Vector3.zero : Vector3.up * 180;
    */




        }
        else
        {
            animationElements.speed = 0;
        }

    }

}
