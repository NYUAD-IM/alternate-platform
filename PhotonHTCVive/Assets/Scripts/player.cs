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
		InputMovement ();

		if (photonView.isMine) {
			

		}else
		{
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
			
			rb.MovePosition (rb.position + Vector3.forward * speed * Time.deltaTime);
			MoveTo (gameObject.transform.position);
		}
		if (Input.GetKey (KeyCode.S)) {
			rb.MovePosition (rb.position - Vector3.forward * speed * Time.deltaTime);
			MoveTo (gameObject.transform.position);
		}
		if (Input.GetKey (KeyCode.D)) {
			rb.MovePosition (rb.position + Vector3.right * speed * Time.deltaTime);
			MoveTo (gameObject.transform.position);
		}
		if (Input.GetKey (KeyCode.A)){
			rb.MovePosition (rb.position - Vector3.right * speed * Time.deltaTime);
			MoveTo (gameObject.transform.position);
		}


		if (Input.GetKeyDown (KeyCode.Space)) {
			GameObject go = GameObject.Find ("Cube");

			if (go.GetComponent<PhotonView> ().isMine)
				go.GetComponent<TransformManager> ().StartColorChange (Vector3.forward);
			else
				go.GetComponent<PhotonView> ().RequestOwnership ();
		}
	}


	[PunRPC] void MoveTo(Vector3 pos)
	{
		GetComponent<Transform>().position = new Vector3(pos.x,pos.y,pos.z);

		if (photonView.isMine)
			photonView.RPC("MoveTo", PhotonTargets.OthersBuffered, pos);
	}


}