using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour {

	Transform elvisTransform;
	Animator animator;
	CharacterController characterController;

	float gravity = -10.0f;
	float moveSpeed = 3.5f;
	float rotationSpeed = 180;
	float rotationSpeedRadians = 80 * (Mathf.PI/180);

	// Use this for initialization
	void Start () {
		elvisTransform = GameObject.FindGameObjectWithTag("Elvis").transform;
		//characterController = GetComponent<CharacterController> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		float distanceAway = Vector3.Distance (transform.position, elvisTransform.position);
		if (distanceAway < 1) {
			// GAME OVER
			Debug.Log("GAME OVER");
		} else if (distanceAway < 5) {
			// start moving towards elvis transform
			float moveStepSize = moveSpeed * Time.deltaTime * (distanceAway / 5);
			Vector3 newDir = Vector3.MoveTowards (transform.position, elvisTransform.position, moveStepSize);
			transform.position = newDir;
			Debug.DrawRay (transform.position, newDir, Color.red);

			float rotationStepSize = rotationSpeedRadians * Time.deltaTime;
			Vector3 targetRot = elvisTransform.position - transform.position;
			Vector3 newRot = Vector3.RotateTowards (transform.forward, targetRot, rotationStepSize, 0.0f);
			transform.rotation = Quaternion.LookRotation (newRot);

			//characterController.Move (newDir);
			animator.SetBool ("Run", true);
			animator.SetBool ("Idle", false);
		} else {
			animator.SetBool ("Idle", true);
			animator.SetBool ("Run", false);
			animator.SetBool ("Fall", false);
		}

		bool isGrounded = true;
		if (!isGrounded) {
			// make character rotate to simulate falling
			//characterController.Move(Vector3(gravity * Time.deltaTime));

			animator.SetBool("Fall", true);
			animator.SetBool("Run", false);
			animator.SetBool("Idle", false);
		}
	}
}
