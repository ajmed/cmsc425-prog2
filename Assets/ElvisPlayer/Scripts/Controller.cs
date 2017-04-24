using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Controller : MonoBehaviour {

	NavMeshAgent agent;
	Animator animator;

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator> ();
	}

	void Update() {

		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;

			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
				agent.destination = hit.point;
			}
		}

		if (agent.remainingDistance > 0) {
			animator.SetBool ("Run", true);
			animator.SetBool ("Idle", false);
		} else {
			animator.SetBool ("Run", false);
			animator.SetBool ("Idle", true);
		}
	}
}
