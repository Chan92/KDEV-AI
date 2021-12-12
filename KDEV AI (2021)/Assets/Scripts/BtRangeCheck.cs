using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtRangeCheck: BtNode {
	private bool boolCheck;

	public BtRangeCheck(Blackboard blackBoard, string _rangeTargetName, string _rangeTypeName) {
		Transform obj = blackBoard.GetData<Transform>("ThisTransform");
		Transform target = blackBoard.GetData<Transform>(_rangeTargetName);
		Debug.Log(target.name);

		float distance = Vector3.Distance(obj.position, target.position);
		float range = blackBoard.GetData<float>(_rangeTypeName);

		boolCheck = distance <= range;
	}

	public BtRangeCheck(Blackboard blackBoard, Transform _newTarget, string _rangeTypeName) {
		Transform obj = blackBoard.GetData<Transform>("ThisTransform");
		
		float distance = Vector3.Distance(obj.position, _newTarget.position);
		float range = blackBoard.GetData<float>(_rangeTypeName);

		boolCheck = distance <= range;
	}

	public override BtResult Run() {
		if(boolCheck) {
			return BtResult.success;
		} else {
			return BtResult.failed;
		}
	}
}
