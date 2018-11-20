using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks {

	[SerializeField]
	private byte maxPlayers = 4;

	private SpawnPoint[] sp;

	void Start()
	{
		sp = FindObjectsOfType<SpawnPoint> ();
		Connect ();
	}

	public void Connect()
	{
		PhotonNetwork.ConnectUsingSettings ();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log ("Master Connected");
		PhotonNetwork.JoinRandomRoom ();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log ("Master Disconnected");
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log ("Random Room join Failed");
		PhotonNetwork.CreateRoom (null, new RoomOptions{MaxPlayers = maxPlayers});
	}

	public override void OnJoinedRoom()
	{
		Debug.Log ("Room Joined");
		SpawnPlayer ();
	}

	public void SpawnPlayer()
	{
		int l = sp.Length;
		int i = Random.Range (0, l);

		GameObject obj = (GameObject)PhotonNetwork.Instantiate ("Player", sp[i].transform.position, sp[i].transform.rotation, 0, null);
		obj.GetComponentInChildren<CharacterControlV2> ().enabled = true;
		obj.transform.Find ("PlayerCam").gameObject.SetActive (true);
	}
}
