using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealth : MonoBehaviour {
	//public float MaxHealth = 100;
	private float CurrentHealth;


	void Start () {
		CurrentHealth = 100;
	}
		
	void OnCollisionEnter(Collision col)
	{
		//	CurrentHealth = MaxHealth - Damage;
		if (col.gameObject.name == "PlayerV2") {
			//CurrentHealth = MaxHealth - 30;
			CurrentHealth = 0;
		}
	}
	void Update () {
		if (CurrentHealth <=0)
		{
			Destroy (gameObject);
	}
}
}


	    