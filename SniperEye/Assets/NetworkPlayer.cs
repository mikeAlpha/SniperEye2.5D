using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer :  MonoBehaviourPun, IPunObservable{

	Vector3 realPosition;
	Quaternion realRotation;

	void Start () {
		realPosition = transform.position;
		//realRotation = transform.rotation;
	}
	

	void Update () {
		if (photonView.IsMine) {
		
		} else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else {
			realPosition = (Vector3)stream.ReceiveNext ();
			realRotation = (Quaternion)stream.ReceiveNext ();
		}
	}
}
