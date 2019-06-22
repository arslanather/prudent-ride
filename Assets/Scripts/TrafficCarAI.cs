using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrafficCarAI : MonoBehaviour {

	public Transform pathMap;
	private List<Transform> pathPoints;
	private int currentPathPoint = 0;
	public WheelCollider []wheels = new WheelCollider[4];
	public Transform[] meshes = new Transform[4];
	public float maxTurn = 15.0f;
	public Transform cog;
	private Rigidbody rb;
	public float maxTorque = 150f;
	public float brakeTorque = 100f;
	public float maxSpeed = 50f;
	public float distanceFromPoint = 20f;
	public float currentSpeed = 0f;
	public float brakeSensorLength = 5f;
	public Transform brakeSensorMid;
	public Transform brakeSensorLeft;
	public Transform brakeSensorRight;
	private bool brakingStatus = false;
	public bool onSignalHold = false;
	private RandomTrafficGenerate generator;

	void Start(){
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = cog.localPosition;
		LoadpathMap ();
	}

	void OnCollisionEnter(Collision collisionInfo) {
		if (collisionInfo.collider.gameObject.CompareTag ("Traffic")) {
			generator = pathMap.GetComponent<RandomTrafficGenerate> ();
			generator.currentTrafficNumber = generator.currentTrafficNumber - 1;
			Destroy (gameObject);
		}
	}

	void Update(){
		BrakeSensor ();
		if (onSignalHold) {
			SignalStop ();
		}
		LoadpathMap ();
		calculateSpeed ();
		UpdateWheelSteer ();
		if ((!brakingStatus)&&(!onSignalHold)) {
			UpdateMotorTorque ();
		}
		UpdateMeshLocations ();
	}

	void UpdateWheelSteer(){
		Vector3 steerVector = transform.InverseTransformPoint (new Vector3 (
			                      pathPoints [currentPathPoint].position.x,
			                      transform.position.y, 
								  pathPoints [currentPathPoint].position.z));
		wheels[0].steerAngle = maxTurn * (steerVector.x / steerVector.magnitude);
		wheels[1].steerAngle = maxTurn * (steerVector.x / steerVector.magnitude);

		if (steerVector.magnitude <= distanceFromPoint) {
			currentPathPoint++;
			if (currentPathPoint >= pathPoints.Count) {
				generator = pathMap.GetComponent<RandomTrafficGenerate> ();
				generator.currentTrafficNumber = generator.currentTrafficNumber - 1;
				Destroy (gameObject);
			}
		}

	}

	void UpdateMotorTorque(){
		if (currentSpeed <= maxSpeed) {
			wheels [2].motorTorque = maxTorque;
			wheels [3].motorTorque = maxTorque;
			wheels [2].brakeTorque = 0f;
			wheels [3].brakeTorque = 0f;
			wheels [0].brakeTorque = 0f;
			wheels [1].brakeTorque = 0f;
		} else {
			wheels [2].motorTorque = 0f;
			wheels [3].motorTorque = 0f;
			wheels [2].brakeTorque = brakeTorque;
			wheels [3].brakeTorque = brakeTorque;
		}
	}

	void calculateSpeed(){
		currentSpeed =  2 * Mathf.PI * wheels [0].rpm * wheels [0].radius * 60 / 1000;
	}

	void LoadpathMap(){
		Transform[] pathTransforms = pathMap.GetComponentsInChildren<Transform> ();

		pathPoints = new List<Transform> ();

		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms [i] != pathMap) {
				pathPoints.Add (pathTransforms [i]);
			}
		}
	}

	void UpdateMeshLocations(){
		for (int i = 0; i < 4; i++) {
			Quaternion rot;
			Vector3 loc;
			wheels[i].GetWorldPose (out loc, out rot);
			meshes [i].position = loc;
			meshes [i].rotation = rot;
		}
	}

	void BrakeSensor(){
		
		RaycastHit hit;

		if (Physics.Raycast (brakeSensorMid.position, brakeSensorMid.forward, out hit, brakeSensorLength)) {
			if (!hit.transform.gameObject.CompareTag ("BrakeZone"))
				brakingStatus = true;
			wheels [2].motorTorque = 0f;
			wheels [3].motorTorque = 0f;
			wheels [2].brakeTorque = brakeTorque;
			wheels [3].brakeTorque = brakeTorque;
			wheels [0].brakeTorque = brakeTorque;
			wheels [1].brakeTorque = brakeTorque;
			Debug.DrawLine (brakeSensorMid.position, hit.point, Color.red);
		} else if (Physics.Raycast (brakeSensorLeft.position, brakeSensorLeft.forward, out hit, brakeSensorLength)) {
			if(!hit.transform.gameObject.CompareTag("BrakeZone"))
			brakingStatus = true;
			wheels [2].motorTorque = 0f;
			wheels [3].motorTorque = 0f;
			wheels [2].brakeTorque = brakeTorque;
			wheels [3].brakeTorque = brakeTorque;
			wheels [0].brakeTorque = brakeTorque;
			wheels [1].brakeTorque = brakeTorque;
			Debug.DrawLine (brakeSensorLeft.position, hit.point, Color.red);
		} else if (Physics.Raycast (brakeSensorRight.position, brakeSensorRight.forward, out hit, brakeSensorLength)) {
			if (!hit.transform.gameObject.CompareTag ("BrakeZone"))
				brakingStatus = true;
			wheels [2].motorTorque = 0f;
			wheels [3].motorTorque = 0f;
			wheels [2].brakeTorque = brakeTorque;
			wheels [3].brakeTorque = brakeTorque;
			wheels [0].brakeTorque = brakeTorque;
			wheels [1].brakeTorque = brakeTorque;
			Debug.DrawLine (brakeSensorRight.position, hit.point, Color.red);
		} else {
			brakingStatus = false;
		}
	}
	void SignalStop(){
		wheels [2].motorTorque = 0f;
		wheels [3].motorTorque = 0f;
		wheels [2].brakeTorque = brakeTorque;
		wheels [3].brakeTorque = brakeTorque;
		wheels [0].brakeTorque = brakeTorque;
		wheels [1].brakeTorque = brakeTorque;
	}
}
