using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;


public class JoyStickCustomController : Joystick {

    public bool InvertX = false;
    public bool InvertY = false;
    public bool SingleAxisInput = false;
    public bool Swapped = false;


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
        int wid = Screen.width;
        int hei = Screen.height;

        MovementRange = wid / 13;

        m_StartPos = transform.position;
        CreateVirtualAxes();
    }


    public new void OnDrag(PointerEventData data)
    {
        Vector3 newPos = Vector3.zero;

        if (m_UseX)
        {
            int delta = (int)(data.position.x - m_StartPos.x);
            //delta = Mathf.Clamp(delta, - MovementRange, MovementRange); // modified
            newPos.x = delta;
        }

        if (m_UseY)
        {
            int delta = (int)(data.position.y - m_StartPos.y);
            //delta = Mathf.Clamp(delta, -MovementRange, MovementRange); // modified
            newPos.y = delta;
        }

        /* ADDED Modification    <--- Might not be updating the 2D visually*/
        if (newPos.x >= newPos.y)
        {
            newPos.y = 0;
        }
        else if (newPos.x < newPos.y)
            newPos.x = 0;


        transform.position = Vector3.ClampMagnitude(new Vector3(newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos;
        UpdateVirtualAxes(transform.position);
    }



    protected new void UpdateVirtualAxes(Vector3 value)
    {
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

        return Output;
    }



}
