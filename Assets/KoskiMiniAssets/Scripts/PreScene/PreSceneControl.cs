using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PreSceneControl : MonoBehaviour
{

    bool Visible = true;
    public GameObject PreSceneObject;
    public CanvasGroupDisplay thePreSceneUI;
    

    /// <summary>
    /// Get the Prescene controller in the scene with a static call.
    /// </summary>
    /// <returns></returns>
    public static PreSceneControl GetPreScene()
    {
        PreSceneControl I;
        try
        {
            I = GameObject.Find("GameControl").GetComponent<PreSceneControl>();
        }
        catch(NullReferenceException e)
        {
            Debug.LogError("No object \"PresceneScenario\" found in the scene." + e.Message);
            Application.Quit();
            I = null;
        }
        return I;
    } 



    /// <summary>
    /// Used this for initialization of PreScene
    /// </summary>

    public void InitPreScene()
    {
        PreSceneObject = GameObject.Find("PresceneScenario");
        thePreSceneUI = GameObject.Find("PreSceneUI").GetComponent<CanvasGroupDisplay>();
        makePreSceneVisible(true);
    }



    /// <summary>
    /// Hide the elements located on the world
    /// </summary>
    public void makePreSceneVisible(bool visible)
    {
        // Hide/show the UI
        if (visible)
        {
            thePreSceneUI.Show();
        }
        else
        {
            thePreSceneUI.Hide();
            //Debug.LogError("Trying to hide");
        }

        // Disable PreSceneObject
        Renderer[] rendererComponents = PreSceneObject.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer component in rendererComponents)
        {
            component.enabled = visible;
        }
        //PreSceneObject.SetActive(visible);
       
    }






}