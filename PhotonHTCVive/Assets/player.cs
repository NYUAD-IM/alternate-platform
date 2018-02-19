using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : Photon.MonoBehaviour {

	public float speed = 10f;


	void Update()
	{
		
		if (photonView.isMine) {
			//InputMovement ();
			InputColorChange();
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
		
		if (Input.GetKey(KeyCode.W))
			rb.MovePosition(rb.position + Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.S))
			rb.MovePosition(rb.position - Vector3.forward * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.D))
			rb.MovePosition(rb.position + Vector3.right * speed * Time.deltaTime);

		if (Input.GetKey(KeyCode.A))
			rb.MovePosition(rb.position - Vector3.right * speed * Time.deltaTime);
	}


	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}

	[PunRPC] void ChangeColorTo(Vector3 color)
	{
		GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z, 1f);

		if (photonView.isMine)
			photonView.RPC("ChangeColorTo", PhotonTargets.OthersBuffered, color);
	}


}