using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtInfoCheck : BtNode {
	private bool boolCheck;

	public BtInfoCheck(Blackboard blackBoard, string _boolName) {
		boolCheck = blackBoard.GetData<bool>(_boolName);		
	}

	public override BtResult Run() {
		if(boolCheck) {
			return BtResult.success;
		} else {
			return BtResult.failed;
		}
	}
}
