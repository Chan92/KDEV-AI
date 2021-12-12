using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
	private BtNode tree;
	public Transform guard;
	public Transform[] waypoints;
	public int startWayPointId = 0;
	private int wayPointId;

	private Blackboard blackBoard;

	public Transform debugPlayer;
	public Transform debugWeapon;

	private void Start() {
		wayPointId = startWayPointId - 1;
		if(wayPointId > waypoints.Length) {
			wayPointId = waypoints.Length - 1;
		}

		SetData();
		CreateTree();
	}

	void SetData() {
		blackBoard = new Blackboard();

		blackBoard.SetData<Transform>("ThisTransform", guard);
		blackBoard.SetData<Transform>("Player", debugPlayer);
		blackBoard.SetData<Transform>("Weapon", debugWeapon);



		//TODO: change to FALSE after btObtain is finished
		blackBoard.SetData<bool>("HasWeapon", true); 

		blackBoard.SetData<float>("DetectRadius", 10f);
		blackBoard.SetData<float>("CloseRangeDetect", 5f);
		blackBoard.SetData<float>("SightAngle", 20f);
		
		blackBoard.SetData<float>("ChaseRange", 10f);
		blackBoard.SetData<float>("AttackRange", 3f);
		blackBoard.SetData<float>("AttackStrength", 10f);

		blackBoard.SetData<float>("AnyOffset", 0.3f);
	}

	void CreateTree() {
		BtSquence sqDetectIntruder = new BtSquence(
					new BtDebug("detect start"),
					new BtDetect(blackBoard, "Player"),
					new BtDebug("detect end")
				);

		//TODO: replace this with parallel detect + patrol?
		BtSquence sqPatrol = new BtSquence(
					new BtDebug("start patrol"),
					new BtWait(1f), new BtInverter(sqDetectIntruder),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f), new BtInverter(sqDetectIntruder),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f), new BtInverter(sqDetectIntruder),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f), new BtInverter(sqDetectIntruder),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f), new BtInverter(sqDetectIntruder),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtDebug("end patrol")
				);	
		
		BtSquence sqAttack = new BtSquence(
					new BtRangeCheck(blackBoard, "Player", "AttackRange"), 
					new BtAttack(blackBoard, "Player")
				);

		BtSquence sqChaseIntruder = new BtSquence(
					new BtRangeCheck(blackBoard, waypoints[GetNextWayPoint()], "ChaseRange"),
					new BtMoveToTarget(blackBoard, "Player")
				);

		BtSelector slStartOffence = new BtSelector(
					sqAttack,
					sqChaseIntruder
				);

		BtSquence sqObtainWeapon = new BtSquence(
					new BtDetect(blackBoard, "Weapon"), 
					new BtMoveToTarget(blackBoard, "Weapon"), 
					new BtObtain(blackBoard, "Weapon")
				);
		
		BtSelector sqPrepareOffence = new BtSelector(
					new BtInfoCheck(blackBoard, "HasWeapon"),
					sqObtainWeapon,
					slStartOffence
				);

		BtSelector slOffence = new BtSelector(
					sqPrepareOffence,
					slStartOffence
				);

		tree = new BtSelector(
					sqPatrol,
					slOffence
				);
	}

	private void Update() {
		tree.Run();
	}

	int GetNextWayPoint() {
		wayPointId = (wayPointId + 1) % waypoints.Length;

		return wayPointId;
	}
}
