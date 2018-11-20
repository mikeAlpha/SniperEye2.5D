using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class CharacterControlV2 : MonoBehaviour{

	public float runSpeed;
	public float walkSpeed;
	public float moveSpeed;
	public float crouchSpeed;
	public bool Crouch;
	private bool running;

	Rigidbody rb;
	Animator anim;

	private bool faceRight;
	public bool IsAIPlayer;
	private AIEnemy mAIEnemy;
	private Transform detectedPlayer;
	private bool firstDetection;
	public float detectionTime;
	public float stoppingDistance;
	float startRunTime;
	private bool HasArrived;

	void Start () {

		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		faceRight = true;
		Crouch = false;
		running = false;

		if (IsAIPlayer) {
			firstDetection = false;
			HasArrived = false;
			mAIEnemy = GetComponent<AIEnemy> ();
			anim.SetBool ("Stop" , true);
		}			
	}
	

	void Update () {
		
	}

	void FixedUpdate(){

		NPCMove ();
		PlayerMove ();
		Attack ();
	}

	public void PlayerMove()
	{
		if (IsAIPlayer)
			return;

		float move = Input.GetAxis ("Horizontal");

//		if (move < 0)
//			return;

		if (move > 0 && !faceRight)
			Flip ();
		else if (move < 0 && faceRight)
			Flip ();

		anim.SetBool ("Crouch", Crouch);

		if (Crouch) {
			moveSpeed = crouchSpeed;
			anim.SetFloat ("speed", 0.05f);
			rb.velocity = new Vector3 (move * moveSpeed, rb.velocity.y, 0);
		}

		moveSpeed = runSpeed;
		anim.SetFloat ("speed", Mathf.Abs (move));
		rb.velocity = new Vector3 (move * moveSpeed, rb.velocity.y, 0);
	}

	public void NPCMove()
	{
		Debug.Log ("===" + transform.name);
		if (detectedPlayer != null && IsAIPlayer && mAIEnemy != null) {
			if (detectedPlayer.position.x < transform.position.x && faceRight)
				Flip ();
			else if(detectedPlayer.position.x > transform.position.x && !faceRight)
				Flip();

			if (mAIEnemy.Detected && faceRight)
				rb.velocity = new Vector3 (moveSpeed, rb.velocity.y, 0);
			else if(mAIEnemy.Detected && !faceRight)
				rb.velocity = new Vector3 ((-1) * moveSpeed, rb.velocity.y, 0);

			if (!firstDetection) {
				startRunTime = Time.time + detectionTime;
				firstDetection = true;
			}

			if (!running && mAIEnemy.Detected) {
				if (startRunTime < Time.time) {
					anim.SetBool ("Walk" , false);
					anim.SetFloat ("Move", 1.0f);
					moveSpeed = runSpeed;
					running = true;
				} else {
					moveSpeed = walkSpeed;
				}
			}

			Debug.Log ("Distance====" + Vector3.Distance(detectedPlayer.position,transform.position));
			Rigidbody playerRB = detectedPlayer.GetComponent<Rigidbody>();

			if (Vector3.Distance (detectedPlayer.position, transform.position) < stoppingDistance){
				if (playerRB.velocity == Vector3.zero && ((detectedPlayer.position.x + stoppingDistance) - transform.position.x) > -0.05f) {
					rb.velocity = Vector3.zero;
					anim.SetBool ("Stop", true);
					anim.SetFloat ("Move", 0.0f);
					running = false;
				} else {
					moveSpeed = runSpeed;
					anim.SetBool ("Walk" , false);
					anim.SetBool ("Stop", false);
					anim.SetFloat ("Move", -1.0f);
					rb.velocity = new Vector3 (2, rb.velocity.y, 0);
					running = true;
				}

			} else if (Vector3.Distance (detectedPlayer.position, transform.position) >= stoppingDistance) {
				if (playerRB.velocity == Vector3.zero && ((detectedPlayer.position.x + stoppingDistance) - transform.position.x) < 0.05f) {
					rb.velocity = Vector3.zero;
					anim.SetBool ("Stop", true);
					anim.SetFloat ("Move", 0.0f);
					running = false;
				} else {
					moveSpeed = runSpeed;
					anim.SetBool ("Walk" , false);
					anim.SetBool ("Stop", false);
					anim.SetFloat ("Move", 1.0f);
					rb.velocity = new Vector3 (-moveSpeed, rb.velocity.y, 0);
					running = true;
				}
			}
		}
	}

	void Attack()
	{
		
	}
		
	public void Death()
	{
		anim.Play ("Death");
	}

	public void EnemyDetected(Transform targetTransform)
	{
		anim.SetBool ("Walk", true);
		anim.SetBool ("Stop", false);


		detectedPlayer = targetTransform;
		if (detectedPlayer.position.x < transform.position.x && faceRight)
			Flip ();
		else if(detectedPlayer.position.x > transform.position.x && !faceRight)
			Flip();

	}

	void Flip(){
		faceRight = !faceRight;
		Vector3 zScale = transform.localScale;
		zScale.z *= -1;
		transform.localScale = zScale;
	}
}
