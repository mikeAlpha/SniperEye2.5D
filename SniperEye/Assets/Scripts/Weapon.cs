using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour {

	public float FireRange;
	public float FireRate;
	public float FireTimer;

	private AudioSource mFireSSource;

	public AudioClip FireSound;
	public Transform FirePosition;
	public GameObject FirePrefab;

	public CharacterControlV2 ccRef;
	public AIEnemy mAIEnemy;

	private Transform target;
	private Transform source;

	void Start () {

		//Temporary Fix
		if(GetComponentInParent<CharacterControlV2>())
			ccRef = GetComponentInParent<CharacterControlV2> ();

		mFireSSource = GetComponent<AudioSource> ();
		mFireSSource.clip = FireSound;

		Debug.Log ("++++++" + transform.root.name);

		if ((ccRef != null && ccRef.IsAIPlayer) || (GetComponentInParent<CharacterControlV2>() == null)) //Temporary fix for sniper
			mAIEnemy = GetComponentInParent<AIEnemy> ();
	}
	

	void FixedUpdate () {

		RaycastHit hit;

		//if (ccRef.IsAIPlayer && ccRef.IsAttack) {
			//Debug.Log ("===AI Firing===0");

		if (mAIEnemy != null)
			Fire ();
		//}
			
		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !ccRef.IsAIPlayer) || Input.GetKeyDown(KeyCode.RightControl)) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100)) {
				if (hit.collider.tag != "Move")
					Fire();		
			}
		}

		FireTimer += Time.deltaTime;
	}

	void Fire(){
		
		if (FireTimer < FireRate)
			return;
		
		//RaycastHit hit;

		//if(Physics.Raycast(FirePosition.position , FirePosition.right , FireRange)){
		//
		//Debug.Log ("===AI Firing===1");
		mFireSSource.PlayOneShot(FireSound);
		GameObject bullet = (GameObject)Instantiate (FirePrefab, FirePosition.position , FirePosition.rotation);

		if (mAIEnemy != null && mAIEnemy.Detected) {
			target = mAIEnemy.GetTarget ();
			source = transform.root;
		}

		if (target != null && source == null) {
			bullet.SendMessage ("SetTarget", target, SendMessageOptions.RequireReceiver);
			bullet.SendMessage ("SetSource", source, SendMessageOptions.RequireReceiver);
		}

		//if(!ccRef.IsAIPlayer)
		//	bullet.GetComponent<Rigidbody> ().velocity = Vector3.forward * FireRange;
		//else
		//  bullet.GetComponent<Rigidbody> ().AddForce(transform.forward * FireRange);
		//  bullet.GetComponent<Rigidbody> ().velocity = transform.forward * FireRange;

		Destroy (bullet, 3.0f);
		//}

		FireTimer = 0.0f;
	}
		
}
