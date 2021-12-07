using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BtMoveToTarget : BtNode {
	private Transform obj;
	private NavMeshAgent agent;
	private Transform target;
	private float distanceOffset;

	public BtMoveToTarget(Blackboard blackBoard) {
		obj = blackBoard.GetData<Transform>("ThisTransform");
		target = blackBoard.GetData<Transform>("CurrentTarget");
		distanceOffset = blackBoard.GetData<float>("AnyOffset");
		
		agent = obj.GetComponent<NavMeshAgent>();
	}

	public BtMoveToTarget(Blackboard blackBoard, Transform _newTarget) {
		obj = blackBoard.GetData<Transform>("ThisTransform");
		distanceOffset = blackBoard.GetData<float>("AnyOffset");
		
		target = _newTarget;
		blackBoard.SetData<Transform>("CurrentTarget", target);

		agent = obj.GetComponent<NavMeshAgent>();
	}

	public override BtResult Run() {
		float distance = Vector3.Distance(agent.transform.position, target.position);

		if(distance > distanceOffset) {
			agent.isStopped = false;
			agent.SetDestination(target.position);
			return BtResult.running;
		} else {
			agent.isStopped = true;
			return BtResult.success;
		}
	}
}