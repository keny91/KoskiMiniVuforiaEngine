using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public int value = 1;
    public bool isPowerUp = false;
    public bool requiredToPassLevel = true;
    public int powerUpType = 0;

    public int rotationAmount = 90;
    // hover too?

    GameControllerScript theController;

    // Use this for initialization
    void Start () {
        //theController = GameObject.Find("Player").GetComponent<GameControllerScript>();
        if (GameObject.Find("GameControl").GetComponent<GameControllerScript>())
        {
            theController = GameObject.Find("GameControl").GetComponent<GameControllerScript>();
            Debug.LogError("COIN INITTTTTTTT");
        }
            

    }

    // Update is called once per frame
    void Update() {

        if (theController.gameRunning) { 
            Vector3 rot = transform.rotation.eulerAngles;
        rot.y = rot.y + rotationAmount * Time.deltaTime;
        if (rot.y > 360)
            rot.y -= 360;
        else if (rot.y < 360)
            rot.y += 360;

        transform.eulerAngles = rot;
    }
    }
}
