using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class MovingCosmetic : MonoBehaviour
{

    //public Transform Origin; // always add a beggining and end patrol point.
    public float speed = 2f;
    private bool CircularRoute = true;

    //GameObject thePlayer;
    GameObject CapsuleObject;
    GameControllerScript theController;

    protected Animator AnimationElement;


    public Transform[] Waypoints;
    private int curWayPoint;
    public bool doPatrol = true;
    private Vector3 target;
    private Vector3 MoveDirection;
    private Vector3 Velocity;
    


    void Move()
    {
        transform.Translate(Velocity);
    }


    protected void GetWayPoints()
    {
        Transform theWayPointContainer = transform.parent.FindChild("PatrolPoints");
        int childNumber = theWayPointContainer.childCount;

        Waypoints = new Transform[childNumber];

        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i] = theWayPointContainer.FindChild("Point" + (i + 1).ToString());
        }

    }



    // Use this for initialization
    void Start()
    {

        curWayPoint = 0;
        //thePlayer = GameObject.Find("Player");
        //theController = (GameControllerScript)thePlayer.GetComponent<GameControllerScript>();
        theController = GameObject.Find("GameControl").GetComponent<GameControllerScript>();
        CapsuleObject = transform.FindChild("Oval").gameObject;
        GetWayPoints();

    }

    // Update is called once per frame
    void Update()
    {

        if (theController.gameRunning)
        {
            GetComponent<Animator>().speed = 1;

            if (     < Waypoints.Length)
            {
                // Select the target depending on the type of route

                target = Waypoints[curWayPoint].position;
                MoveDirection = target - transform.position;

                //Velocity = GetComponent<Rigidbody>().velocity;
                //Debug.LogWarning("MAGNITUDE: " + MoveDirection.magnitude + " Direction: " + MoveDirection.magnitude + " wAypoint: " + curWayPoint);
                if (MoveDirection.magnitude < 1.0f)
                {
                    curWayPoint = curWayPoint + 1;
                    Velocity = MoveDirection.magnitude * MoveDirection.normalized;
                }
                else
                {
                    Velocity = MoveDirection.normalized * speed * Time.deltaTime;
                }
            }
            else
            {
                if (CircularRoute)
                {
                    curWayPoint = 0;
                }
                else
                {
                    Velocity = Vector3.zero;
                }
            }

            /*
            Debug.Log("<color=yellow>To: </color>" + target);
            Debug.Log("<color=red>Position: </color>" + transform.position);
            Debug.Log("<color=green>Direction: </color>" + MoveDirection);
            */

            //GetComponent<Rigidbody>().velocity = Velocity;
            transform.Translate(Velocity);
            //Move();
            CapsuleObject.transform.LookAt(target);
        }
        else
        {
            GetComponent<Animator>().speed = 0;
        }


    }
}




//theTagReference = new TagDatabase();
//theTagReference.tagList = new Dictionary<string, int>();
