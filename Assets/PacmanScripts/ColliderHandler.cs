using UnityEngine;
using System.Collections;

public class ColliderHandler : MonoBehaviour {
	private MovementController mController;

	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.tag == "Ghost"){
			Debug.Log("Trigger: " + collision.gameObject.name);
			mController.triggerKilledAnimation();
		}else if(collision.gameObject.tag == "Coin"){
			Debug.Log("Collect: " + collision.gameObject.name);
			GameObject.Destroy(collision.gameObject);
			mController.collectCoin();
		}
	}

	void Awake () {
		mController = transform.parent.parent.GetComponent<MovementController>();
	}

	void Update () {
	
	}
}
