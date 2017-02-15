using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrakerControl  {

    CanvasGroupDisplay LostUI;


    void Initialize()
    {
        LostUI = GameObject.Find("LostTrackingMenu").GetComponent<CanvasGroupDisplay>();
    }

    void DetectedMarker()
    {

    }

    void MarkerLost()
    {

    }

}
