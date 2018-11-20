using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIEnemy : MonoBehaviour {

	private Transform targetToFollow;
	Vector2 origin;
	public enum EnemyType{SNIPER,ASSAULT,OTHERS};
	public EnemyType Type;

	public bool Detected = false;

	private CharacterControlV2 ccRef;

	void Start () {
		origin = transform.position;
		ccRef = GetComponent<CharacterControlV2> ();
	}

	void FixedUpdate () {
		if (targetToFollow != null) {
			if (Type == EnemyType.SNIPER)
				transform.LookAt (targetToFollow);
			else {
				
			}
		}
	}

	public Transform GetTarget()
	{
		return targetToFollow;
	}

	public void SetTarget(Transform target)
	{
		targetToFollow = target;
		ccRef.EnemyDetected (targetToFollow);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			if (Type == EnemyType.SNIPER) {
				Debug.Log ("Name=====" + other.name);
				targetToFollow = other.transform;
			} 
		}
	}

	void OnTriggerExit(Collider other)
	{
		
	}
}
