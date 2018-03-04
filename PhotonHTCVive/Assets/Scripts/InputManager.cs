using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	//This script should be attached to each controller (Controller Left or Controller Right)

	// Getting a reference to the controller GameObject
	private SteamVR_TrackedObject trackedObj;
	// Getting a reference to the controller Interface
	private SteamVR_Controller.Device Controller;

	PhotonView photonView;

	void Start(){
		photonView = PhotonView.Get (this);
	}

	void Awake()
	{
		// initialize the trackedObj to the component of the controller to which the script is attached
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update () {


		Controller = SteamVR_Controller.Input((int)trackedObj.index);



		// Getting the Touchpad Axis
		if (Controller.GetAxis() != Vector2.zero)
		{
			Debug.Log(gameObject.name + Controller.GetAxis());
		}

		// Getting the Trigger press
		if (Controller.GetHairTriggerDown())
		{
			Debug.Log ("heyyy");
			GameObject go = GameObject.Find ("Cube");
			go.GetComponent<PhotonView> ().RequestOwnership ();
			photonView.RPC("GetTrigger",PhotonTargets.All);
			//GetTrigger ();

		}

		// Getting the Trigger Release
		if (Controller.GetHairTriggerUp())
		{
			photonView.RPC("ReleaseTrigger",PhotonTargets.All);
			//ReleaseTrigger ();

		}

		// Getting the Grip Press
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Press");
		}

		// Getting the Grip Release
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Release");
		}
	}

	[PunRPC] void ReleaseTrigger()
	{
		GameObject go = GameObject.Find ("Cube");
		go.transform.parent = null;
		Debug.Log(gameObject.name + " Trigger Release");

		if (photonView.isMine)
			photonView.RPC("ReleaseTrigger", PhotonTargets.OthersBuffered);
	}

	[PunRPC] void GetTrigger()
	{
		GameObject go = GameObject.Find ("Cube");
		//go.GetComponent<PhotonView> ().RequestOwnership ();
		go.transform.SetParent (gameObject.transform);

		Debug.Log(gameObject.name + " Trigger Press");

		if (photonView.isMine)
			photonView.RPC("GetTrigger", PhotonTargets.OthersBuffered);
	}
}