using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class JoystickCameraController : MonoBehaviour {

    JoyStickCustomController joyStickController;


    // Use this for initialization
    void Start () {

        // Get the joystick
        GameObject joyObject = GameObject.Find("MobileJoystick");
        try
        {
            joyStickController = (JoyStickCustomController)joyObject.GetComponent<JoyStickCustomController>();
            joyStickController.configureJoystick(false, true, true);
        }
        catch (Exception e)
        {
            Debug.LogError(e, joyObject);
        }

    }
	

    private void DetermineZone()
    {
        if(0 <= transform.rotation.y && transform.rotation.y < 90)
        {
            joyStickController.configureJoystick(true, false, true);
        }
        else if(90 <= transform.rotation.y && transform.rotation.y < 180)
        {
            joyStickController.configureJoystick(true, true, true);
        }
        else if ((180 <= transform.rotation.y && transform.rotation.y < 270) || (transform.rotation.y < 0 && transform.rotation.y>-90) )
        {
            joyStickController.configureJoystick(false, true, true);
        }
        else if ((270 <= transform.rotation.y && transform.rotation.y < 360) || (transform.rotation.y < -90 && transform.rotation.y > -180))
        {
            joyStickController.configureJoystick(false, false, true);
        }
    }

	// Update is called once per frame
	void Update () {
	

	}
}
