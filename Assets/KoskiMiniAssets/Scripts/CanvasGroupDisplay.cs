using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupDisplay : MonoBehaviour {

    protected CanvasGroup theDisplayedUI;
    public bool isActive;

	// Use this for initialization
	void Start () {
        theDisplayedUI = GetComponent<CanvasGroup>();
        isActive = true;
        Debug.LogWarning("Created menu: ", gameObject);
    }
	

    /// <summary>
    ///  Hide and stop interactivity
    /// </summary>
    public void Hide()
    {
        isActive = false;
        theDisplayedUI.alpha = 0;
        theDisplayedUI.interactable = false;
    }
    /// <summary>
    /// Display interface and return interactivity
    /// </summary>
    public void Show()
    {
        isActive = true;
        theDisplayedUI.alpha = 1;
        theDisplayedUI.interactable = true;
    }


}
