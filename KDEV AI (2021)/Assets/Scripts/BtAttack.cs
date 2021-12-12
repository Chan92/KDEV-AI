using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtAttack : BtNode {
	private Transform target;
	private float hitStrength;


	public BtAttack(Blackboard blackBoard, string _targetName) {
		target = blackBoard.GetData<Transform>(_targetName);
		hitStrength = blackBoard.GetData<float>("AttackStrength");
	}

	public override BtResult Run() {
		Health targetHealth = target.GetComponent<Health>();
		if(targetHealth) {
			targetHealth.GetDamaged(hitStrength);
			
			Debug.Log("Attacked target " + target.name + " with " + hitStrength + " strength.");
			return BtResult.success;		
		}

		return BtResult.failed;
	}
}