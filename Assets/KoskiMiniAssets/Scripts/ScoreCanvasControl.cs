using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ScoreCanvasControl : CanvasGroupDisplay {

    public Text ScoreText, LifeText;
    public bool hasScore, hasLife;
    public bool addCosmetic = true;
    // Use this for initialization
    

	public void Start () {

        theDisplayedUI = GetComponent<CanvasGroup>();
        isActive = true;
        checkForElements();

    }
	

    /// <summary>
    /// Find out which UI elements are present.
    /// </summary>
    private void checkForElements()
    {


        try
        {
            ScoreText = (Text)GetComponent<Transform>().Find("ScoreValue").GetComponent<Text>();
            hasScore = true;
        }
        catch(NullReferenceException e)
        {
            //Debug.LogException(e, this);
            Debug.LogWarning("<color=orange>The pointed User Interface has no ScoreCount element. </color>", this);
            hasScore = false;
        }

        try
        {
            LifeText = (Text)GetComponent<Transform>().Find("LifeValue").GetComponent<Text>();
            hasLife = true;
        }
        catch (NullReferenceException e)
        {
            //Debug.LogException(e, this);
            Debug.LogWarning("<color=orange>The pointed User Interface has no LifeCount element. </color>", this);
            hasLife = false;
        }
    }

    /// <summary>
    /// Add "x" as a cosmetic feature after the input value
    /// </summary>
    /// <param name="value">Integer value to be formated</param>
    /// <returns></returns>
    public string ParseToScreenValue(int value)
    {
        string theString;
        if (addCosmetic)
            theString = (value).ToString() + "x";
        else
            theString = (value).ToString();

        return theString;
    }




    /// <summary>
    /// Alter the life count displayed in the UI pointed to
    /// </summary>
    /// <param name="LiveNumber">Number displayed in screen</param>
    public void changeLifeSprite(int LiveNumber)
    {

        if (hasLife)
            LifeText.text = ParseToScreenValue(LiveNumber);
      
    }



    /// <summary>
    /// Alter the score count displayed in the UI pointed to
    /// </summary>
    /// <param name="ScoreNumber">Number displayed in screen</param>

    public void changeScoreSprite(int ScoreNumber)
    {

        if (hasScore)
                ScoreText.text = ParseToScreenValue(ScoreNumber);
    }
}
