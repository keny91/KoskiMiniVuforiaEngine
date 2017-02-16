using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {


    public Texture2D texture;
    private Canvas theTexture;
    static LoadingScreen instance;
    public string levelToLoad;
    public int IDLevelToLoad;
    public float WaitingTime = 3f;
    protected GameObject continueButton;
    AsyncOperation async;

    /*
    public void onButtonClick()
    {
        StartCoroutine(DisplayLoadingScreen(levelToLoad));
    }
    */


    void Start()
    {
        DontDestroyOnLoad(this);
        

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        theTexture = GetComponent<Canvas>();
        //theTexture. = texture;
        continueButton = GameObject.Find("ContinueB");
        continueButton.SetActive(false);

    }


    /// <summary>
    /// Initialization, finding the object which will carry out the loading process.
    /// </summary>
    /// <returns></returns>
    public static LoadingScreen GetObject(){

        return GameObject.Find("LoadingScreen").GetComponent<LoadingScreen>();
        }

    /*
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameObject.AddComponent<GUITexture> ().enabled = false;
        GetComponent<GUITexture>().texture = texture;
        transform.position = new Vector3(0.5f, 0.5f, 0.0f);
        DontDestroyOnLoad(this);
    }
    */


        /// <summary>
        /// Making this static we can call it withoud initializing the class
        /// </summary>
        /// <param name="index"></param>
    public void Load(string level)
    {

        levelToLoad = level;
        theTexture.enabled = true;  // Load the menu
        //LoadLevel(index);

        StartCoroutine(LoadLevel(level));
        //theTexture.enabled = false;
    }




    /// <summary>
    /// Hide the load screen
    /// </summary>
    public void HideLoadingScreen()
    {
        theTexture.enabled = false;
        GetComponent<GraphicRaycaster>().enabled = false;
        GetComponent<Canvas>().enabled = false;
    }


    /// <summary>
    /// show the loading screen
    /// </summary>
    public void ShowLoadingScreen()
    {
        theTexture.enabled = true;
        GetComponent<GraphicRaycaster>().enabled = true;
        GetComponent<Canvas>().enabled = true;
    }



    /// <summary>
    /// Call to start loading a level
    /// </summary>
    /// <param name="name">Name of the scene containing the level</param>
    /// <returns></returns>
    IEnumerator LoadLevel(string name)
    {
        float ThePresent = Time.time;
        float TheNewPresent = Time.time;
        async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;
        while (!async.isDone && TheNewPresent-ThePresent< WaitingTime)
        {
            TheNewPresent = Time.time;
            int progress = ((int)(async.progress * 100));
            //map.SetPixels(0, 0, progress, maxHeight, filler);
            //Debug.Log(TheNewPresent - ThePresent);
           // if (progress > 85)
               // EnableContinue();
            //map.Apply();
            yield return (0);
        }


        Debug.Log("Loading complete");
        SceneManager.LoadScene(levelToLoad);
        //EnableContinue();

        //PhotonNetwork.isMessageQueueRunning = false;
    }


    /// <summary>
    /// Call to start loading a level
    /// </summary>
    /// <param name="sceneID">Id of the scene containing the level</param>
    /// <returns></returns>
    IEnumerator LoadLevel(int sceneID)
    {
        async = SceneManager.LoadSceneAsync(sceneID);
        
        while (!async.isDone)
        {
            int progress = ((int)(async.progress * 100));
            Debug.Log(progress);
            yield return (0);
            if(progress>85)
                EnableContinue();
        }

        levelToLoad = SceneManager.GetSceneAt(sceneID).name;
        Debug.Log("Loading complete");
        

        //PhotonNetwork.isMessageQueueRunning = false;
    }


    /// <summary>
    /// The continue button is enabled, allowing to proceed past the loading screen.
    /// </summary>
    public void EnableContinue()
    {
        continueButton.SetActive(true);
    }

    /// <summary>
    /// The continue button is enabled, allowing to proceed past the loading screen.
    /// </summary>
    public void DisableContinue()
    {
        continueButton.SetActive(false);
    }

    /// <summary>
    /// Configure the class to change the next level to load.
    /// </summary>
    /// <param name="newScene"></param>
    public void SetNextScene(string newScene)
    {
        levelToLoad = newScene;
    }

    /// <summary>
    /// Action that occurs after pressing the "Continue" button.
    /// </summary>
    public void OnContinueButton()
    {
        if (levelToLoad != null)
        {
            async.allowSceneActivation = true;
            
            
        }           
        else
            Debug.LogError("No Scene was set to load.");

        //levelToLoad = null; // Reset level
        DisableContinue();
        //HideLoadingScreen();
    }

}
