using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;


public class JoyStickCustomController : Joystick {

    public bool InvertX = false;
    public bool InvertY = false;
    public bool SingleAxisInput = false;
    public bool Swapped = false;

    private GameObject JoystickObject;
    protected Vector3 JoystickObject_m_StartPos;
    public bool PointerIsDown = false;


    public void configureJoystick(bool X, bool Y, bool swap,bool Single)
    {
        //Debug.LogWarning("JOYSTICK DONE");
        InvertX = X;
        InvertY = Y;
        Swapped = swap;
        SingleAxisInput = Single;
    }


    // Use this for initialization
    void Start () {

        JoystickObject = transform.FindChild("MobileJoystick").gameObject;

        int wid = Screen.width;
        int hei = Screen.height;

        MovementRange = wid / 13;

        
        JoystickObject_m_StartPos = JoystickObject.transform.position; ;
        CreateVirtualAxes();
    }


    public override void OnDrag(PointerEventData data)
    {
        Vector3 newPos = Vector3.zero;


        int delta;
        if (m_UseX)
        {
            delta = (int)(data.position.x - m_StartPos.x);
            //delta = Mathf.Clamp(delta, - MovementRange, MovementRange); // modified
            newPos.x = delta;
        }

        if (m_UseY)
        {
            delta = (int)(data.position.y - m_StartPos.y);
            //delta = Mathf.Clamp(delta, -MovementRange, MovementRange); // modified
            newPos.y = delta;
        }

        Vector3 value = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + JoystickObject_m_StartPos;


            JoystickObject.transform.position = value;

        /*
            if (newPos.x >= newPos.y)
            {
                newPos.y = 0;
            }
            else if (newPos.x < newPos.y)
                newPos.x = 0;
*/
        /* ADDED Modification    <--- Might not be updating the 2D visually*/
 


        value = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
        //JoystickObject.transform.position = value;
        UpdateVirtualAxes(value);
    }


    public override void OnPointerUp(PointerEventData data)
    {
        //transform.position = m_StartPos;
        JoystickObject.transform.position = JoystickObject_m_StartPos;
        UpdateVirtualAxes(m_StartPos);
        PointerIsDown = false;
    }

    public override void OnPointerDown(PointerEventData data)
    {
      //  Debug.LogWarning("POINTER IS DOWN DETECTED");
        if (!PointerIsDown)
        {
            
            m_StartPos = data.position;
          //  Debug.LogWarning("Selected dragging center" + m_StartPos);
            PointerIsDown = true;
        }
            
    }

    protected new void UpdateVirtualAxes(Vector3 value)
    {

        if(value == m_StartPos)
        {
            m_HorizontalVirtualAxis.Update(0);
            m_VerticalVirtualAxis.Update(0);
            return;
        }

        

        var delta = m_StartPos - value;
        
        delta.y = -delta.y;

        delta /= MovementRange;
        if (m_UseX)
        {
            if(Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                m_HorizontalVirtualAxis.Update(-delta.x);
        }

        if (m_UseY)
        {
            if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y))
                m_VerticalVirtualAxis.Update(delta.y);
        }

       //Debug.LogWarning("__AXIS X: " + -delta.x + " ___AXIS Y: " + delta.y);

    }



    /// <summary>
    /// Depending on the configuration of the Joystick controller.
    /// If "InvertedX"/ "InvertedY" invert the corresponding axis input.
    /// If "SingleAxisInput": the output will be a vector with only the largest axis; the other will be set to 0. Any non-null value outputed this way will be either 0.5 or 1. 
    /// The regular vector is returned otherwise.
    /// </summary>
    /// <returns> Returns the vector2 with both axis represented as a -1 to 1 value.</returns>
    public Vector2 getAxisOutput()
    {
        float valueX = 0;
        float valueY = 0;
        Vector2 Output;
        if(m_UseX)
            valueX = CrossPlatformInputManager.GetAxisRaw(horizontalAxisName);
        if(m_UseY)
            valueY = CrossPlatformInputManager.GetAxisRaw(verticalAxisName);

        Debug.LogWarning(valueX + "___" + valueY);

        float x = Mathf.Abs(valueX);
        float y = Mathf.Abs(valueY);

        if (InvertX)
            valueX *= -1;
        if (InvertY)
            valueY *= -1;

        if (SingleAxisInput)
        {
            if (x > y)
            {
                valueX = (x > 0.5) ? 1f* Mathf.Sign(valueX) : 0.5f* Mathf.Sign(valueX);
                valueY = 0;
            }
                
            if (x < y)
            {
                valueY = (y > 0.5) ? 1f* Mathf.Sign(valueY) : 0.5f* Mathf.Sign(valueY);
                valueX = 0;
            }
                
        }

        if(Swapped)
            Output = new Vector2(valueY, valueX);
        else       
            Output = new Vector2(valueX, valueY);


      //  Debug.LogError("AXIS OUTPUT: " + Output);

        return Output;
    }



}
