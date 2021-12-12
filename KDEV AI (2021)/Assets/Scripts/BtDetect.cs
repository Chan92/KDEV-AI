using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtDetect  : BtNode {
	private Blackboard blackBoard;
	private Transform obj;
	public float detectRadius;
	public float closeRangeDetect;
	public float sightAngle;
	private string targetTypeName;

	public BtDetect(Blackboard _blackBoard, string _newTargetType) {
		blackBoard = _blackBoard;
		obj = blackBoard.GetData<Transform>("ThisTransform");
		detectRadius = blackBoard.GetData<float>("DetectRadius");
		closeRangeDetect = blackBoard.GetData<float>("CloseRangeDetect");
		sightAngle = blackBoard.GetData<float>("SightAngle");
		targetTypeName = _newTargetType;
	}

	public override BtResult Run() {
		Collider[] hitColliders = Physics.OverlapSphere(obj.transform.position, detectRadius);

		foreach(Collider hit in hitColliders) {
			if(hit.transform.tag == targetTypeName) {
				if(CheckDetection(hit.transform)) {
					blackBoard.SetData<Transform>(targetTypeName, hit.transform);
					return BtResult.success;
				}
			}
		}

		Debug.Log("Target " + targetTypeName + " not found!");
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
					Debug.Log("Detected target in close range.");
					return true;
				} else if(Mathf.Abs(angle) < sightAngle) {
					Debug.Log("Detected target in eye sight.");
					return true;
				}
			}
		}

		return false;
	}
}