using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {


	private const string roomName = "VRlab";
	private RoomInfo[] roomsList;
	//private byte numPlayers = 8;
	public GameObject playerprefab;
	public GameObject headsetcubeprefab;
	public GameObject capsulehand;
	public GameObject spawnPoint1;
	public GameObject spawnPoint2;

	public Transform[] spawnPoints;

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
		PhotonNetwork.autoJoinLobby = true;

		spawnPoints = new Transform[2];


		spawnPoints[0] = spawnPoint1.transform;
		//spawnPoints[0].position = Vector3.zero;
	
		spawnPoints[1] = spawnPoint2.transform;
		//spawnPoints[1].position = Vector3.zero;
	
	}
	
	// Update is called once per frame
	void Update () {

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

		Debug.Log ("Players: " + PhotonNetwork.countOfPlayers);

		StartCoroutine (WaitForRig ());

		Transform spawnLocation;
		if (PhotonNetwork.countOfPlayers == 1) {
			spawnLocation = spawnPoints [0];
		} else {
			spawnLocation = spawnPoints [1];
		}

		Debug.Log (spawnLocation);

		Debug.Log ("Connected to Room");
		Debug.Log ("Creating a player");

		//playerprefab is a camera rig for HTC Vive
		GameObject.Instantiate (playerprefab,spawnPoints[PhotonNetwork.countOfPlayers-1].position , Quaternion.identity);

	}

	IEnumerator WaitForRig(){


		Debug.Log(PhotonNetwork.countOfPlayers);

		yield return new WaitForSeconds (1);



		//Find headset and instaniate cube ON NETWORK -- set headset as cube's parent
		GameObject headset = GameObject.Find ("Camera (eye)");
		GameObject photonCube = PhotonNetwork.Instantiate(headsetcubeprefab.name, headset.transform.position, Quaternion.identity, 0);
		photonCube.transform.SetParent (headset.transform);

		//Find the controllers and instantiate capsules ON NETOWRK -- set controllers as the parents of the capsules

		GameObject controllerLeft = GameObject.Find ("Controller (left)/Model");
		Debug.Log (controllerLeft);
		GameObject capsuleHandLeft = PhotonNetwork.Instantiate(capsulehand.name, controllerLeft.transform.position, Quaternion.identity, 0);
		capsuleHandLeft.transform.SetParent (controllerLeft.transform);
		//capsuleHandLeft.transform.position = Vector3.zero;

		//Now for right controller

		GameObject controllerRight = GameObject.Find("Controller (right)/Model");
		GameObject capsuleHandRight = PhotonNetwork.Instantiate(capsulehand.name, controllerRight.transform.position, Quaternion.identity, 0);
		capsuleHandRight.transform.SetParent (controllerRight.transform);
		//capsuleHandRight.transform.position = Vector3.zero;


		//Debug.Log ("left: " + controllerLeft + " right: " + controllerRight);



	}
}







