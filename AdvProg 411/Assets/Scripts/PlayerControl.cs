﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {
	private CharacterController controller;
	private float verticalVelocity;
	private float gravity;
	private float jumpForce;
	public float speed;
	public float health;
	public	bool moveAnim;
	public DeathMenu theDeathScreen;
	float h;
	float v;
	Vector3 movement;
	Vector3 camLookDir;
	BloodSc bs;
	Animator pAnim;
	bool p; 
	public bool enemyEntered = false;
	public Enemy enemy;

	// Use this for initialization
	void Start () {
		moveAnim = false;
		gravity = 14.0f;
		jumpForce = 10.0f;
		speed  = 10.0f;
		health = 100.0f;
		// Enemy

 
	//	Cursor.lockState = CursorLockMode.Locked;	
		bs = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<BloodSc>();
		controller = gameObject.GetComponent<CharacterController> ();
		pAnim = gameObject.GetComponent<Animator> ();
		p = GameObject.FindGameObjectWithTag ("Sun").GetComponent<DayNight> ().paused;
	//	theDeathScreen = 
	//	sword = gameObject.
	}
	// Update is called once per frame
	void Update () {  
		p = GameObject.FindGameObjectWithTag ("Sun").GetComponent<DayNight> ().paused;
		if (!p) {
			Event e = Event.current;
			//	h = Input.GetAxisRaw ("Horizontal");
			//	v = Input.GetAxisRaw ("Vertical");
			if (Input.GetKey (KeyCode.LeftShift)) {
				if (speed >= 20f) {
					speed = 20f;
				} else {
					speed += 0.1f;
					//drain energy true
				}
			} else {
				if (speed <= 7f) {
					speed = 7f;
				} else {
					speed -= 0.2f;
				}
			}

			if (health <= 0) {
				Invoke ("Die", 1);
			}

			if (Input.GetKey (KeyCode.Q)) {
				//	transform.RotateAround (transform.position, new Vector3 (0, 1, 0), 1);
				transform.Rotate (new Vector3 (0, -1, 0));//, Space.World);
			} else if (Input.GetKey (KeyCode.E)) {
				//	transform.RotateAround (transform.position, new Vector3 (0, 1, 0), -1);
				transform.Rotate (new Vector3 (0, 1, 0));//, Space.World);
			}
			if (Input.GetKey (KeyCode.W)) {
				transform.position += transform.forward.normalized * speed * 0.01f;
				moveAnim = true;
				pAnim.SetBool ("moving", moveAnim);
			} else if (Input.GetKey (KeyCode.S)) {
				moveAnim = true;
				pAnim.SetBool ("moving", moveAnim);
				transform.position += transform.forward.normalized * (-1) * speed * 0.01f;
			} else {
				moveAnim = false;
				pAnim.SetBool ("moving", moveAnim);
			}
			//	if (Input.GetKeyDown (KeyCode.Space)) {
			//		gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (0, 300, 0));
			//	}
			if (Input.GetKey (KeyCode.A)) {
				transform.position += transform.right.normalized * (-1) * speed * 0.01f;
			} else if (Input.GetKey (KeyCode.D)) {
				transform.position += transform.right.normalized * speed * 0.01f;
			}
			if (Input.GetKeyDown (KeyCode.F)) {
				bs.TakeDamage ();
				health -= 100f;
			}
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				Attack ();
			}
			if (controller.isGrounded) {
				verticalVelocity = -gravity * Time.deltaTime;
				if (Input.GetKeyDown (KeyCode.Space)) {
					verticalVelocity = jumpForce;
				}
			} else {
				verticalVelocity -= gravity * Time.deltaTime;
			}
			Vector3 moveVector = Vector3.zero;
			moveVector.x = 0.0f;
			moveVector.y = verticalVelocity;
			moveVector.z = 0.0f;
			controller.Move (moveVector * Time.deltaTime);
		}
	}

	public void TakeDamage(int dmg){
		health -= dmg;
		bs.TakeDamage ();
	}
	public void Attack(){
		transform.position += transform.forward.normalized * speed * 0.02f;
		pAnim.SetTrigger ("attack"); 
		if(enemyEntered)
		enemy.takeDamage(50);
	}

	public void Die(){
	//	SceneManager.LoadScene ((SceneManager.GetActiveScene ().buildIndex + 1)%SceneManager.sceneCount); //to next level/
	//	SceneManager.LoadScene(0);	//main menu
	//	Debug.Log(SceneManager.GetSceneAt(0).buildIndex); //log scene index
	//	CancelInvoke ();
		theDeathScreen.gameObject.SetActive(true);
	}
	public void Reset(){
		theDeathScreen.gameObject.SetActive (false);
		SceneManager.LoadScene (1);//main menu
		CancelInvoke();
	}
	public void OnCollisionEnter(Collision Col){ 
	//	Col.collider.GetComponent<Enemy> ().Die();
		Debug.Log( Col.collider.gameObject);
		enemy = Col.collider.gameObject.GetComponent<Enemy> ();
		enemyEntered = true;
	}
	public void OnCollisionExit(Collision Col){ 
		enemyEntered = false;
	}
 
}