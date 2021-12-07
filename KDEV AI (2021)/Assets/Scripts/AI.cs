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
		blackBoard.SetData<Transform>("CurrentTarget", waypoints[0]);

		blackBoard.SetData<float>("DetectRadius", 10f);
		blackBoard.SetData<float>("CloseRangeDetect", 5f);
		blackBoard.SetData<float>("SightAngle", 20f);
		blackBoard.SetData<float>("ChaseRange", 10f);
		blackBoard.SetData<float>("AttackRange", 3f);
		
		blackBoard.SetData<float>("AnyOffset", 0.3f);
	
		
		blackBoard.SetData<float>("", 0f);
	}

	void CreateTree() {
		BtSquence sqDetect = new BtSquence(
					new BtDebug("detect start"),
					new BtDetect(blackBoard),
					//new BtInverter(new BtDetect(blackBoard)),
					new BtDebug("detect end")
				);
		BtSquence sqPatrol = new BtSquence(
					new BtDebug("start patrol"),
					new BtWait(1f),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtWait(1f),
					new BtMoveToTarget(blackBoard, waypoints[GetNextWayPoint()]),
					new BtDebug("end patrol")
				);


		tree = new BtSquence(
				new BtDebug("STARTING"),
				sqDetect,
				sqPatrol
				);
	}

	int GetNextWayPoint() {
		wayPointId = (wayPointId + 1) % waypoints.Length;

		return wayPointId;
	}

	private void Update() {
		tree.Run();
	}
}
