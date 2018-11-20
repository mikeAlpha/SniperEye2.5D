 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float DamageAmount;
	public GameObject DamageEffect;
	public float Speed;

	GameObject Player;
	Transform target;
	Transform source;

	void Start () {
		//Player = GameObject.FindGameObjectWithTag ("Player");
		//target = new Vector3 (Player.transform.position.x, Player.transform.position.y + 0.5f, Player.transform.position.z);
	}

	void Update () {
		//transform.position = Vector3.MoveTowards (transform.position, target, Speed * Time.deltaTime);
		
	}

	public void SetTarget(Transform val){
		target = val;
	}

	public void SetSource(Transform val){
		source = val;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
			return;
		
		var cHealth = other.gameObject.GetComponent<CharacterHealth> ();

		if (cHealth != null) {
			cHealth.currentHealth -= DamageAmount;
		}

		Debug.Log (other.name);

		Destroy (gameObject);
	}
}
