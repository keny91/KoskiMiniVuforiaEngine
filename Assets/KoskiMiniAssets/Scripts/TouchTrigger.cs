using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTrigger : MonoBehaviour {

    public Material[] materials;//Allows input of material colors in a set size of array;
    public Renderer Rend; //What are we rendering? Input object(Sphere,Cylinder,...) to render.

    private int index = 1;//Initialize at 1, otherwise you have to press the ball twice to change colors at first.
    GameControllerScript theController;


    // Use this for initialization
    void Start()
    {
        theController = GameObject.Find("GameControl").GetComponent<GameControllerScript>();
        //Rend = GetComponent<Renderer>();//Gives functionality for the renderer
        //Rend.enabled = true;//Makes the rendered 3d object visable if enabled;
    }



    void OnMouseDown()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            theController.NextSubScene();
            //Rend.sharedMaterial = materials[index - 1]; //This sets the material color values inside the index
        }
    }
}
