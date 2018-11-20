using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour {

	public float currentHealth;
	public float MaxHealth;
	public bool IsAlive = true;

	private CharacterControl ccRef;

	void Start(){
		ccRef = GetComponent<CharacterControl> ();
		currentHealth = MaxHealth;
	}

	void Update () {
		if (currentHealth <= 0) {
			Die ();		
		}
	}

	void Die(){
		IsAlive = false;
		ccRef.Death ();
	}
}
