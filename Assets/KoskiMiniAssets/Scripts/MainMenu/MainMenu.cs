using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string buildVersion = "1";
    private Text BuildText;


    // Use this for initialization
    void Start () {

        BuildText = (Text)GameObject.Find("BuildID").GetComponent<Text>();
        BuildText.text = buildVersion;

    }
	

    public void StartButton()
    {
        //SceneManager.LoadScene("PreScene1");
        
        GetComponent<Canvas>().enabled = false;
        //LoadingScreen.GetObject().GetComponent<Canvas>().enabled = true;

        //LoadingScreen.GetObject().Load("PreScene1");
        SceneManager.LoadScene("PreScene1");


    }

	// Update is called once per frame
	void Update () {
	
	}
}
