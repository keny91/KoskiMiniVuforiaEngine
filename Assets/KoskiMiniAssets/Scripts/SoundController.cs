using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {



    public AudioSource audioS, AudioLevel;
    public AudioClip SoundCoinCollected, SoundPlayerHit, SoundPlayerDeath, Sound1Up;
    public AudioClip[] SoundJump;
    public AudioClip SoundRunning;
    public AudioClip SoundVictory, SoundDefeat;
    public AudioClip AmbientalMusicForLevel;

    // Use this for initialization
    void Start () {
        // Don´t Destroy Background music from Level to Level
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("Music"));
        audioS = gameObject.AddComponent<AudioSource>();
        AudioLevel = gameObject.AddComponent<AudioSource>();
        audioS.clip = AmbientalMusicForLevel;

    }
	
	// Update is called once per frame
	void Update () {


    }


    public void PlayBackGroundMusic()
    {
        AudioLevel = gameObject.AddComponent<AudioSource>();
        AudioLevel.clip = AmbientalMusicForLevel;
        AudioLevel.Play();
    }

    public void StopBackGroundMusic()
    {
        //AudioLevel = gameObject.AddComponent<AudioSource>();
        //AudioLevel.clip = AmbientalMusicForLevel;
        AudioLevel.Stop();
    }

    public void playClip(AudioClip Clip)
    {
        audioS.Stop();
        audioS = gameObject.AddComponent<AudioSource>();
        audioS.clip = Clip;
        audioS.Play();
    }

    public void playClipLooped(AudioClip Clip)
    {
        if (!audioS.isPlaying)
        {
            audioS = gameObject.AddComponent<AudioSource>();
            audioS.loop = true;
            audioS.clip = Clip;
            audioS.Play();
        }

    }


    public void ResumeClip(AudioClip Clip)
    {
        if (!audioS.isPlaying)
            audioS.UnPause();
        else
            Debug.LogWarning("Clip " + audioS.clip.name + " is not playing");
        audioS.UnPause();
    }


    public void StopClip(AudioClip theClip)
    {
        //audioS = gameObject.AddComponent<AudioSource>();
        //audioS.clip = Clip;
        if (audioS.isPlaying && audioS.clip.name == theClip.name)
        {
            audioS.Stop();
        }
            

    }


    public void PlayRandomFrom(AudioClip[] clipList)
    {
        int len = clipList.Length;
        int selected = (int)Random.Range(0, len);
        Debug.LogWarning("ClipsSelected from " + clipList + "is the number "+ selected);
        playClip(clipList[selected]);

    }

    public void PauseClip(AudioClip Clip)
    {
        //audioS = gameObject.AddComponent<AudioSource>();
        //audioS.clip = Clip;
        if (audioS.isPlaying)
            audioS.Pause();
        else
            Debug.LogWarning("Clip " + audioS.clip.name + " is not playing");
    }
}
