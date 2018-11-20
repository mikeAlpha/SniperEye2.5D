using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

	private AIEnemy mAIEnemyRef;

	void Start()
	{
		mAIEnemyRef = GetComponentInParent<AIEnemy> ();
	}

	void Update()
	{
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			mAIEnemyRef.SetTarget (other.transform);
			mAIEnemyRef.Detected = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
	
	}
}
