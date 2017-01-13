using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PreSceneControl : MonoBehaviour
{

    CanvasGroup info, targetFound, continueButton;
    bool WorldHidden = true;
    public GameObject WorldObject;

    public void OnContinuePressed()
    {
        SceneManager.LoadScene("Main");
    }


    // Use this for initialization
    void Start()
    {

        WorldObject = GameObject.Find("WorldScene");
        continueButton = GameObject.Find("ContinueButton").GetComponent<CanvasGroup>();
        targetFound = GameObject.Find("findTargetText").GetComponent<CanvasGroup>();
        info = GameObject.Find("infoText").GetComponent<CanvasGroup>();
        changeWorldVisible(false);
    }



    /// <summary>
    /// Hide the elements located on the world
    /// </summary>
    public void changeWorldVisible(bool hidTheWorld)
    {

        if (hidTheWorld != WorldHidden) // Do only if it changes status
        {
            try
            {
                if (!hidTheWorld) { // true
                    DetectedMarker();
                }
                else  //false
                {
                    MarkerLost();
                }

                Renderer[] rendererComponents = WorldObject.GetComponentsInChildren<Renderer>(true);
                //Collider[] colliderComponents = WorldObject.GetComponentsInChildren<Collider>(true);

                // Enable rendering:
                foreach (Renderer component in rendererComponents)
                {
                    component.enabled = !hidTheWorld;
                }


                WorldHidden = hidTheWorld;
            }
            catch (UnassignedReferenceException e)
            {
                Debug.Log("Suppressed Error: " + e);
            }

        }

        // WorldObject.FIIIIINISHTHISSHIT;
    }



    void DetectedMarker()
    {
        HideElement(targetFound);
        ShowElement(continueButton);
        ShowElement(info);
    }

    void MarkerLost()
    {
        HideElement(continueButton);
        HideElement(info);
        ShowElement(targetFound);
    }

    void HideElement(CanvasGroup uiElement)
    {
        uiElement.alpha = 0;
        uiElement.interactable = false;
        uiElement.blocksRaycasts = false;
    }

    void ShowElement(CanvasGroup uiElement)
    {
        uiElement.alpha = 1;
        uiElement.interactable = true;
        uiElement.blocksRaycasts = true;
    }

    // Update is called once per frame
    void Update()
    {


    }
}