using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// Attach this script to the placeHolders in the Subscene. They will be the triggers to shift to the next screen.
/// </summary>
public class TouchTrigger : MonoBehaviour {

    public Material[] materials;//Allows input of material colors in a set size of array;
    public Renderer Rend; //What are we rendering? Input object(Sphere,Cylinder,...) to render.

    private int index = 1;//Initialize at 1, otherwise you have to press the ball twice to change colors at first.
    GameControllerScript theController;


    // Use this for initialization
    void Start()
    {
        theController = GameObject.Find("GameControl").GetComponent<GameControllerScript>();
    }




    /// <summary>
    /// We attach an onMouseDown event to trigger the onTouchFunction
    /// </summary>
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            theController.NextSubScene();
        }
    }
}
