using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {


	private const string roomName = "VRlab";
	private RoomInfo[] roomsList;
	//private byte numPlayers = 8;
	public GameObject playerprefab;
	public GameObject headsetcubeprefab;

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
		PhotonNetwork.autoJoinLobby = true;

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (roomsList);
	}


	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
		else if (PhotonNetwork.room == null)
		{
			// Create Room
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				//PhotonNetwork.CreateRoom(roomName + System.Guid.NewGuid().ToString("N"), true, true, 5); maxPlayers = numPlayers
				PhotonNetwork.CreateRoom(roomName, new RoomOptions(){MaxPlayers = 6, IsVisible = true}, null);

			// Join Room
			if (roomsList != null)
			{
				Debug.Log ("rooms list length: " + roomsList.Length);
				for (int i = 0; i < roomsList.Length; i++)
				{
					
					if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
						PhotonNetwork.JoinRoom(roomsList[i].name);
				}
			}
		}
	}

	void OnReceivedRoomListUpdate()
	{
		
		roomsList = PhotonNetwork.GetRoomList();
	}
	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room");
		Debug.Log ("Creating a player");
		GameObject.Instantiate (playerprefab, Vector3.zero, Quaternion.identity);
		GameObject headset = GameObject.Find ("Camera (eye)");
		GameObject photonCube = PhotonNetwork.Instantiate(headsetcubeprefab.name, Vector3.up * 5, Quaternion.identity, 0);
		photonCube.transform.SetParent (headset.transform);
	}
}
