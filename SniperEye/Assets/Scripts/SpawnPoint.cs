using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public bool IsOcuppied = false;

	void OnTriggerEnter(Collider other)
	{
		IsOcuppied = true;
		if (other.GetComponent<CharacterControlV2> ()) {
			other.GetComponent<CharacterControlV2> ().Crouch = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		IsOcuppied = true;
	}

	void OnTriggerExit(Collider other)
	{
		IsOcuppied = false;
		if (other.GetComponent<CharacterControlV2> ()) {
			other.GetComponent<CharacterControlV2> ().Crouch = false;
		}
	}
}
