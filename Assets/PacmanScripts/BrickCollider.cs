using UnityEngine;
using System.Collections;

public class BrickCollider : MonoBehaviour {
	public BrickType brickType;
	private BoxCollider mCollider;
	public bool isGrounded = false;

	public BoxCollider getCollider(){
		return mCollider;
	}

	void Awake(){
		mCollider = GetComponent<BoxCollider>();
	}

}
