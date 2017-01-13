using UnityEngine;
using System.Collections;

public class ComplexEnemyBehabiour : MonoBehaviour {

    public int Lives;
    public Transform Origin; // always add a beggining and end patrol point.
    public float speed = 2f;

    GameObject thePlayer;
    GameObject EnemyObject;
    GameControllerScript theController;

  
    public Transform[] Waypoints;
    private int curWayPoint;
    public bool doPatrol = true;
    private Vector3 target;
    private Vector3 MoveDirection;
    private Vector3 Velocity;
    private int RouteModifier;



    // Enemies can only take damage
    void TakeEnemyDamage(int value)
    {
        // Decrease life count
        Lives -= value;
        if (Lives <= 0)
            EnemyDestroyed();
    }


    void EnemyDestroyed()
    {
        // ADD PARTICLES / DEATH SEQUENCE
        Destroy(gameObject);
    }

    void Move()
    {
        transform.Translate(Velocity);
    }

    // Use this for initialization
    void Start()
    {
        //gameObject.transform.position = Origin.position;
        //RouteModifier = +1;
        curWayPoint = 0;
        //target = pathPoints[currPathPoint+RouteModifier].position;
        thePlayer = GameObject.Find("Player");
        theController = (GameControllerScript)thePlayer.GetComponent<GameControllerScript>();
        EnemyObject = transform.FindChild("Oval").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (true)
        {
            if (curWayPoint < Waypoints.Length)
            {
                target = Waypoints[curWayPoint].position;
                MoveDirection = target - transform.position;

                //Velocity = GetComponent<Rigidbody>().velocity;
                Debug.LogWarning("MAGNITUDE: " + MoveDirection.magnitude + " Direction: " + MoveDirection.magnitude + " wAypoint: " + curWayPoint);
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
                if (doPatrol)
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
            EnemyObject.transform.LookAt(target);
        }
        else
        {
            GetComponent<Animator>().speed = 0;
        }


    }
}
