﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;


/// <summary>
/// Attach this script to the AR camera
/// </summary>
public class JoystickCameraController : MonoBehaviour {

    JoyStickCustomController joyStickController;
    bool XaxisInverted = false;
    bool ZaxisInverted = false;
    int CameraZone = -1;
    int LastCameraZone = -1;

    // Use this for initialization
    void Start () {

        /*
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
        */
    }
	


    /// <summary>
    /// The scene is divided into 4 different regions based on the angular orientation of the camera.
    /// Each of the Horizontal axis X/Z are separated and it controls the inversion of the Joystick to compensate for the change in perspective.
    /// <para>This method automatically detects the region and calls for the axis change.</para>
    /// <para>Change the Zone order HERE if any re-calibration is needed.</para>
    /// </summary>
    public void DetermineZone()
    {
        CameraZone = -1;
        float eulerAngle = transform.rotation.eulerAngles.y;
        //Debug.Log("aaaaaaaaaaaaaaaaaaaaaa:  " + transform.rotation.eulerAngles.y);
        if (315 <= eulerAngle || eulerAngle < 45)
        {
            CameraZone = 1;
        }
        else if(45 <= eulerAngle && eulerAngle < 135)
        {
            CameraZone = 2;
        }
        else if ((135 <= eulerAngle && eulerAngle < 225) || (eulerAngle < 0 && eulerAngle>-90) )
        {
            CameraZone = 3;
        }
        else if ((225 <= eulerAngle && eulerAngle < 315) || (eulerAngle < -90 && eulerAngle > -180))
        {
            CameraZone = 4;
        }
        else
        {
            CameraZone = -1;
        }

        // If the camera zone has changed DO axis Correction
        if (LastCameraZone != CameraZone)
            JoyStickAxisCorrection();
    }



    /// <summary>
    /// Here a compensations is made depending on the zone in which the camera is located.
    /// </summary>
    private void JoyStickAxisCorrection()
    {
        switch (CameraZone)
        {
            default:
                Debug.Log("CAMERA IN ZONE: ERROR");
                break;

            case -1:
                Debug.Log("CAMERA IN ZONE: ERROR");
                break;
            case 1:
                Debug.Log("CAMERA IN ZONE: 1");
                joyStickController.configureJoystick(false, false, true ,true);
                break;
            case 2:
                Debug.Log("CAMERA IN ZONE: 2");
                joyStickController.configureJoystick(true, false, false, true);
                break;
            case 3:
                Debug.Log("CAMERA IN ZONE: 3");
                joyStickController.configureJoystick(true, true, true, true);
                break;
            case 4:
                Debug.Log("CAMERA IN ZONE: 4");
                joyStickController.configureJoystick(false, true, false, true);
                break;
        }
        LastCameraZone = CameraZone;
    }


    /// <summary>
    /// Assign the JoyStick to be compensated
    /// </summary>
    public void SelectJoyStick(JoyStickCustomController theJoyStick)
    {
        joyStickController = theJoyStick;
        Debug.Log("Selected Joystick: " + theJoyStick.transform.name);
    }


    /*
	// Update is called once per frame
	void Update () {
	

	}
    */
}