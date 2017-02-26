using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour {
	public float jetpackForce = 75.0f;
	public float forwardMovementSpeed = 3.0f;
	private Rigidbody2D mouseRigidbody2D;

	public Transform groundCheckTransform;
	private bool grounded;
	public LayerMask groundCheckLayerMask;
	Animator animator;

	public ParticleSystem jetpack;
	private bool dead = false;

	private uint coins = 0;
	public Texture2D coinIconTexture;
	public AudioClip coinCollectSound;

	public AudioSource jetpackAudio;
	public AudioSource footstepsAudio;

	void FixedUpdate() {
		bool jetpackActive = Input.GetButton("Jump");

		jetpackActive = jetpackActive && !dead;

		if(jetpackActive){
			mouseRigidbody2D.AddForce(new Vector2(0, jetpackForce));
		}

		if(!dead){
			Vector2 newVelocity = mouseRigidbody2D.velocity;
			newVelocity.x = forwardMovementSpeed;
			mouseRigidbody2D.velocity = newVelocity;
		}

		UpdateGroundedStatus();
		AdjustJetpack(jetpackActive);
		AdjustFootstepsAndJetpackSound(jetpackActive);

	}

	// Use this for initialization
	void Start () {
		mouseRigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

	}

	void UpdateGroundedStatus() {
		grounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
		animator.SetBool("grounded",grounded);
	}

	void AdjustJetpack(bool jetpackActive){
		ParticleSystem.EmissionModule emissioModule = jetpack.emission;
		emissioModule.enabled = !grounded;
		emissioModule.rateOverTime = jetpackActive ? 300.0f : 75.0f;

	}

	// called when mouse collides with any laser
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.CompareTag("Coins"))
			CollectCoin(collider);
		else
			HitByLaser(collider);
	}

	void HitByLaser(Collider2D laserCollider){
		if(!dead){
			laserCollider.gameObject.GetComponent<AudioSource>().Play();
		}
		dead = true;
		animator.SetBool("dead",true);
	}

	void CollectCoin(Collider2D coinCollider){
		coins++;
		Destroy(coinCollider.gameObject);
		AudioSource.PlayClipAtPoint(coinCollectSound,transform.position);
	}

	void OnGUI(){
		DisplayCoinsCount();
		DisplayRestartButton();
	}

	void DisplayCoinsCount(){
		Rect coinIconRect = new Rect(10, 10, 32, 32);
		GUI.DrawTexture(coinIconRect, coinIconTexture);

		GUIStyle style = new GUIStyle();
		style.fontSize = 30;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.yellow;

		Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
		GUI.Label(labelRect,coins.ToString(), style);
	}

	void DisplayRestartButton(){
		if(dead && grounded) {
			Rect buttonRect = new Rect (Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f,
			Screen.height*0.1f);
			if(GUI.Button(buttonRect, "Tap to restart!")){
				// Application.LoadLevel(Application.loadedLevelName);
				SceneManager.LoadScene("RocketMouse");
			};
		}
	}

	void AdjustFootstepsAndJetpackSound(bool jetpackActive){
    footstepsAudio.enabled = !dead && grounded;

    jetpackAudio.enabled =  !dead && !grounded;
    jetpackAudio.volume = jetpackActive ? 1.0f : 0.5f;
}


}
