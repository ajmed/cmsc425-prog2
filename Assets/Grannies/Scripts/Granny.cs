using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour {

	Transform elvisTransform;
	Animator animator;
	GameManager gameManager;
	CharacterController characterController;

	Vector3 newPos = Vector3.zero;
	float moveSpeed = 3.5f;
	static float rotationSpeed = 35.0f;
	float rotationSpeedRadians = rotationSpeed * (Mathf.PI/180);

	Vector3 gravity = new Vector3 (0.0f, -0.15f, 0.0f);
	float fallingRotation = -1f;

	float followRadius = 7.0f;

	bool grannyFell = false;

	// Use this for initialization
	void Start () {
		elvisTransform = GameObject.FindGameObjectWithTag("Elvis").transform;
		gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		characterController = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
	}

	bool IsGrounded () {
		var bounds = characterController.bounds;
		float distToGround = bounds.extents.y + 0.1f;
		Vector3 down = transform.TransformDirection (Vector3.down);
		return Physics.Raycast (transform.position, down, distToGround);
	}

	// Update is called once per frame
	void Update () {
		newPos = Vector3.zero;

		float distanceAway = Vector3.Distance (transform.position, elvisTransform.position);
		if (distanceAway < followRadius) {
			// start moving towards elvis transform
			//Vector3 newPos = Vector3.MoveTowards (transform.position, elvisTransform.position, moveStepSize);
			//newPos -= transform.position;
			//newPos = transform.TransformDirection (newPos);
			//Debug.DrawRay (transform.position, newPos, Color.red);

			Vector3 targetRot = elvisTransform.position - transform.position;
			float rotationStepSize = rotationSpeedRadians * Time.deltaTime;
			Vector3 newRot = Vector3.RotateTowards (transform.forward, targetRot, rotationStepSize, 0.0f);
			transform.rotation = Quaternion.LookRotation (newRot);

			float moveStepSize = moveSpeed * Time.deltaTime * (distanceAway / followRadius);
			newPos = moveStepSize * transform.forward;

			// Chase
			animator.SetBool ("Run", true);
			animator.SetBool ("Fall", false);
			animator.SetBool ("Idle", false);
		} else {
			// Idle
			animator.SetBool ("Idle", true);
			animator.SetBool ("Run", false);
			animator.SetBool ("Fall", false);
		}

		if (IsGrounded ()) {
			if (distanceAway < 1.5) { // GAME OVER
				gameManager.OnGameLost (null);
			}
		} else {
			animator.SetBool("Fall", true);
			animator.SetBool("Run", false);
			animator.SetBool("Idle", false);

			transform.rotation *= Quaternion.Euler(fallingRotation, 0, 0);
			transform.position += gravity;
		}

		characterController.Move(newPos);

		if (transform.position.y < -3 && !grannyFell) {
			grannyFell = true;
			gameManager.OnGrannyFallen (null);
		}
	}
}
