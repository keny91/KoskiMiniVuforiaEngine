using UnityEngine;
using System.Collections;
using Vuforia;

public class CameraFocus : MonoBehaviour
{

    private bool mVuforiaStarted = false;

    // Use this for initialization
    void Start()
    {
        VuforiaAbstractBehaviour qcar = (VuforiaAbstractBehaviour)FindObjectOfType(typeof(VuforiaAbstractBehaviour));
        if (qcar)
        {
            qcar.RegisterVuforiaStartedCallback(OnQCARStarted);
        }
        else
        {
            Debug.Log("Failed to find QCARBehaviour in current scene");
        }
    }

    private void OnQCARStarted()
    {
        Debug.Log("Vuforia has started.");
        mVuforiaStarted = true;

        // Enable focus mode:
        bool autofocusOK = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        if (autofocusOK)
        {
            Debug.Log("Successfully enabled Continuous Autofocus mode");
        }
        else
        {
            // set a different focus mode (for example, FOCUS_NORMAL):
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);

            // Other possible options:
            // CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_MACRO);
            // CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_INFINITY);
        }
    }
}