using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterControl : MonoBehaviour {

	public bool IsAIPlayer = false;
	public Animator mAnimController;
	public bool running = false;

	public NavMeshAgent mAgent;
	public bool IsAttack = false;
	public bool HasArrived = false;
	public bool IsReset = false;

	public float LookOffsetX = 0.0f;
	public float LookOffsetY = 0.1f;
	public float LookOffsetZ = 0.1f;

	Vector3 destination;
	Vector3 Origin; 
	Vector3 forward;

	public bool IsEngaging = false;

	void Start () {
		mAnimController = GetComponent<Animator> ();
		mAgent = GetComponent<NavMeshAgent> ();
	}
	

	void Update () {
		Move ();
		Attack ();
	}
		 
	void Move()
	{
//		Ray ray;

//		#if UNITY_ANDROID
//		if(Input.touchCount > 0)
//			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//		#endif
		if (IsReset) {
			return;
		}


		RaycastHit hit;

		if (IsAIPlayer) {
			mAgent.destination = GameObject.FindGameObjectWithTag ("Player").transform.position;
			mAnimController.SetBool ("Running", running);

			//Debug.Log ("Remaining Distance==" + mAgent.remainingDistance + " " + "Stopping Distance==" + mAgent.stoppingDistance + " " + (mAgent.remainingDistance <= mAgent.stoppingDistance).ToString());
		}
			
		//if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !IsAIPlayer) {

//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//
//			if (Physics.Raycast (ray, out hit, 100)) {
//			if (hit.collider.tag == "Move") {
//				running = true;
//				mAgent.destination = hit.point;
//			}else {
//					IsAttack = true;
//					return;
//				}
//			}
		//}
		if (!IsAIPlayer) {
			forward = transform.TransformDirection (Vector3.forward) * 5.0f;

			destination = new Vector3 (forward.x + LookOffsetX, forward.y + LookOffsetY, forward.z);
			Origin = new Vector3 (transform.position.x, transform.position.y + LookOffsetY + 0.1f, transform.position.z);

			Debug.DrawRay (Origin, destination, Color.green);
		}

		if (mAgent.remainingDistance <= mAgent.stoppingDistance) {
			if(IsAIPlayer)
				IsAttack = true;

			running = false;
		} else {
			IsAttack = false;
			running = true;
		}

		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved && !IsAIPlayer) || Input.GetKeyDown(KeyCode.RightArrow)) {

			if (IsEngaging)
				return;

			Ray ray = new Ray (Origin, destination);
			if (Physics.Raycast (ray, out hit, forward.magnitude)) {
				if (hit.collider.tag == "Move") {
					mAgent.destination = new Vector3 (hit.point.x + 0.5f, hit.point.y, hit.point.z);
				} else {
					//IsAttack = true;
					return;
				}
			}
		}

		mAnimController.SetBool ("Running" , running);
	}

	void Attack()
	{
//		if (IsAttack) {
			//Debug.Log ("====Firing====");
			mAnimController.SetBool ("Attacking", IsAttack);

			if (!IsAIPlayer) {
				IsAttack = false;
			}

//		} else {
//			mAnimController.SetBool ("Attacking", IsAttack);
			//mFireSSource.Stop ();
			//
			//GetComponent<AudioSource> ().Stop ();
//		}
	}

	public void Death()
	{
		if (IsAIPlayer)
			IsAttack = false;

		if (mAnimController != null) {
			mAnimController.SetLayerWeight (1, 0.0f);
			mAnimController.Play ("Death");
		}
	}

	public void Stop()
	{
		if (IsAIPlayer) {
			IsAttack = false;
			IsReset = true;
		}

		if(mAnimController != null)
			mAnimController.SetLayerWeight (1, 0.5f);
	}

	public void Reset()
	{
		IsReset = false;
	}

	public void SetStoppingDistance(float val)
	{
		if (IsAIPlayer)
			mAgent.stoppingDistance = val;
	}
}