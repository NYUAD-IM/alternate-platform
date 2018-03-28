using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script syncronizes player position, movement, as well as input across Photon network
public class player : Photon.MonoBehaviour {

	public float speed = 10f;

	PhotonView photonView;

	void Start(){
		photonView = PhotonView.Get (this);
	}

	void Update()
	{
		if (!photonView.isMine) {
			SyncedMovement();
		}
	}

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		if (stream.isWriting)
		{
			stream.SendNext(rb.position);
			stream.SendNext(rb.velocity);
		}
		else
		{
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			Vector3 syncVelocity = (Vector3)stream.ReceiveNext();

			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rb.position;
		}
	}

	private void SyncedMovement()
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		syncTime += Time.deltaTime;
		rb.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	// Following code allows synchronized object movement & color change via keyboard input
	void InputMovement()
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		// keyboard WASD input to move cube
		if (Input.GetKey (KeyCode.W)) {
			GameObject go = GameObject.Find ("Cube");
			// Uncomment following line to restrict player requesting gameObject ownership
			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.up);
		}

		if (Input.GetKey (KeyCode.S)) {
			GameObject go = GameObject.Find ("Cube");
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.down);
		}
		if (Input.GetKey (KeyCode.D)) {
			GameObject go = GameObject.Find ("Cube");
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.left);
		}
		if (Input.GetKey (KeyCode.A)){
			GameObject go = GameObject.Find ("Cube");
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.right);
		}

		// keyboard space to change cube color
		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject go = GameObject.Find ("Cube");
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartColorChange (Vector3.up);
		}
	}
}
