using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	public AudioClip soundEffect;
	private AudioSource mAudio;

	void Start () {
		mAudio = gameObject.AddComponent<AudioSource>();
	}

}
