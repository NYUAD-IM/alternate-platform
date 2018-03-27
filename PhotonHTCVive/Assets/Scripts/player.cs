using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : Photon.MonoBehaviour {

	public float speed = 10f;

	PhotonView photonView;

	void Start(){
		photonView = PhotonView.Get (this);
	}

	void Update()
	{
		//InputMovement ();

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

	void InputMovement()
	{
		Rigidbody rb = GetComponent<Rigidbody> ();
		
		if (Input.GetKey (KeyCode.W)) {
			GameObject go = GameObject.Find ("Cube");

			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.up);
			//rb.MovePosition (rb.position + Vector3.forward * speed * Time.deltaTime);

		}
		if (Input.GetKey (KeyCode.S)) {
			GameObject go = GameObject.Find ("Cube");

			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.down);
			//rb.MovePosition (rb.position - Vector3.forward * speed * Time.deltaTime);

		}
		if (Input.GetKey (KeyCode.D)) {
			GameObject go = GameObject.Find ("Cube");

			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.left);
			//rb.MovePosition (rb.position + Vector3.right * speed * Time.deltaTime);
		
		}
		if (Input.GetKey (KeyCode.A)){
			GameObject go = GameObject.Find ("Cube");

			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartMoveTo (Vector3.right);
		//	rb.MovePosition (rb.position - Vector3.right * speed * Time.deltaTime);

		}


		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject go = GameObject.Find ("Cube");

			//if (go.GetComponent<PhotonView> ().isMine)
			go.GetComponent<PhotonView> ().RequestOwnership ();
			go.GetComponent<TransformManager> ().StartColorChange (Vector3.up);
			//else
			//	go.GetComponent<PhotonView> ().RequestOwnership ();
		}
	}




}
