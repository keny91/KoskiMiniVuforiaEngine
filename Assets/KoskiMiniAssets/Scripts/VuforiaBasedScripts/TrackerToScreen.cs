using System.Collections;
using System.Collections.Generic;
using Vuforia;
using UnityEngine;


public class TrackerToScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //StateManager SM;
        TrackableBehaviour a;
        VuforiaUnity.GetProjectionGL();
    }
	
	// Update is called once per frame
	void Update () {

        VideoMode videoMode = CameraDevice::getInstance().getVideoMode(Vuforia::CameraDevice::MODE_DEFAULT);
    }

    Vuforia:: cameraPointToScreenPoint(Vuforia::Vec2F cameraPoint)
    {
        Vuforia::VideoMode videoMode = Vuforia::CameraDevice::getInstance().getVideoMode(Vuforia::CameraDevice::MODE_DEFAULT);
        Vuforia::VideoBackgroundConfig config = Vuforia::Renderer::getInstance().getVideoBackgroundConfig();
        int xOffset = ((int)screenWidth - config.mSize.data[0]) / 2.0f + config.mPosition.data[0];
        int yOffset = ((int)screenHeight - config.mSize.data[1]) / 2.0f - config.mPosition.data[1];
        if (isActivityInPortraitMode)
        {
            // camera image is rotated 90 degrees
            int rotatedX = videoMode.mHeight - cameraPoint.data[1];
            int rotatedY = cameraPoint.data[0];
            return Vuforia::Vec2F(rotatedX * config.mSize.data[0] / (float)videoMode.mHeight + xOffset,
            rotatedY * config.mSize.data[1] / (float)videoMode.mWidth + yOffset);
        }
        else
        {
            return Vuforia::Vec2F(cameraPoint.data[0] * config.mSize.data[0] / (float)videoMode.mWidth + xOffset,
            cameraPoint.data[1] * config.mSize.data[1] / (float)videoMode.mHeight + yOffset);
        }
    }


}
