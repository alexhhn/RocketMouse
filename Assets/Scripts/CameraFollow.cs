using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject player;       //Public variable to store a reference to the player game object
	private Vector3 offset;         //Private variable to store the offset distance between the player and camera

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;

	}

	// Update is called once per frame
	void Update () {
		 transform.position = new Vector3(player.transform.position.x + 3.5f, transform.position.y, offset.z);

	}
}
