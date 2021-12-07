using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BtPatrol : BtNode {
	private Transform obj;
	private Transform destination;
	private float distanceOffset = 0.5f;
	private float moveSpeed;

	public BtPatrol(Blackboard blackBoard, Transform _destination) {
		obj = blackBoard.GetData<Transform>("ThisTransform");
		destination = _destination;
		moveSpeed = blackBoard.GetData<float>("MoveSpeed");
	}

	public override BtResult Run() {
		obj.LookAt(destination);
		if(Vector3.Distance(obj.position, destination.position) > distanceOffset) {
			obj.position += obj.forward * Time.deltaTime * moveSpeed;
			return BtResult.running;
		} else {
			return BtResult.success;
		}
		//return BtResult.failed;
	}
}