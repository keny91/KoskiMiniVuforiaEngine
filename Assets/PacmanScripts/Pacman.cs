using UnityEngine;
using System.Collections;
using System;


public class Pacman : MonoBehaviour {

	public Transform pacman;
	public Transform ghost;
	public Transform coins;

	public MovementController pacmanController;
	public MovementController ghostController;

	public float pacmanAnimTime = 3.0f;
	public float ghostAnimStartTime = 2.0f;
	public float ghostAnimEndTime = 5.0f;
	public float coinAnimTime = 5.0f;

	private float mTime = 0f;
	private bool  mStarted = false;
	private bool  mFinished = false;
	private bool  mCoinTriggered = false;
	private Transform[] mCoins;

	public void triggerAnimation(){
		if(mStarted) return;
		mStarted = true;
		mTime = Time.time;
		pacman.gameObject.SetActive(true);
		ghost.gameObject.SetActive(true);
		coins.gameObject.SetActive(true);
	}

	public void pause(){
		if(!mStarted) return;
	}

	void Awake () {
		mCoins = new Transform[coins.childCount];
		for(int i = 0, len = coins.childCount; i < len; i++){
			mCoins[i] = coins.GetChild(i);
			mCoins[i].GetComponent<MeshRenderer>().material.color = new Color(1,1,1,0);
		}

		pacman.gameObject.SetActive(false);
		ghost.gameObject.SetActive(false);
		coins.gameObject.SetActive(false);
	}
	
	void Update () {
		if(!mStarted || mFinished) return;

		float time = Time.time - mTime;
		if(time <= pacmanAnimTime){
			float anim = Easing.easeWithType(EasingType.EaseInAtan,time/pacmanAnimTime);
			pacman.GetComponent<MeshRenderer>().material.color = new Color(1,1,1,anim);
		}

		if(time >= ghostAnimStartTime && time <= ghostAnimEndTime){
			float t = (time - ghostAnimStartTime) / (ghostAnimEndTime - ghostAnimStartTime);	
			if(t >= 0.998f && !mCoinTriggered){
				mCoinTriggered = true;
				coins.gameObject.SetActive(true);
			}
			float anim = Easing.easeWithType(EasingType.EaseOutBounce, t);
			ghost.GetComponent<MeshRenderer>().material.color = new Color(1,1,1,anim);
		}

		if(time > ghostAnimEndTime && time <= (ghostAnimEndTime + coinAnimTime)){
			float t = (time - ghostAnimEndTime) / coinAnimTime;	
			int len = mCoins.Length;
			float anim = Easing.easeWithType(EasingType.EaseInOutBack,t);
			for(int i = 0; i < len; i++){
				mCoins[i].GetComponent<MeshRenderer>().material.color = new Color(1,1,1,anim);
			}

		}

		if(time > ghostAnimEndTime + coinAnimTime+1f){
			pacmanController.enabled = true;
			ghostController.enabled = true;
			mFinished = true;
			pacman.GetComponent<MeshRenderer>().material.color = new Color(1,1,1,1);
			ghost.GetComponent<MeshRenderer>().material.color = new Color(1,1,1,1);
		}

	}
}
