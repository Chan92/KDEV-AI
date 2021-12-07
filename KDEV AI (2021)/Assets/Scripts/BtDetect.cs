using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtDetect  : BtNode {
	public float detectRadius;
	public float closeRangeDetect;
	public float sightAngle;
	private Transform obj;

	public BtDetect(Blackboard blackBoard) {
		obj = blackBoard.GetData<Transform>("ThisTransform");
		detectRadius = blackBoard.GetData<float>("DetectRadius");
		closeRangeDetect = blackBoard.GetData<float>("CloseRangeDetect");
		sightAngle = blackBoard.GetData<float>("SightAngle");
	}

	public override BtResult Run() {
		Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, detectRadius);

		foreach(Collider hit in hitColliders) {
			if(hit.transform.tag == "Player") {
				if(CheckDetection(hit.transform)) {
					return BtResult.success;
				}
			}
		}

		Debug.Log("not found!");
		return BtResult.failed;
	}

	bool CheckDetection(Transform intruder) {
		float distance = Vector3.Distance(obj.position, intruder.position);
		Vector3 direction = obj.position - intruder.position;
		float angle = Vector3.Angle(obj.forward, direction);

		RaycastHit hit;
		if(Physics.Linecast(obj.position, intruder.position, out hit)) {
			if(hit.transform == intruder) {
				if(distance < closeRangeDetect) {
					Debug.Log("detected: close range");
					return true;
				} else if(Mathf.Abs(angle) < sightAngle) {
					Debug.Log("detected: in eye sight");
					return true;
				}
			}
		}

		return false;
	}
}