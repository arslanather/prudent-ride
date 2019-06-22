using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour {

	public float maxTorque = 500f;
	public float maxTurn = 30f;
	public float breakSensitivity =  400f;
	public float topSpeed = 80f;
	public float currentSpeed = 0f;

	public Transform []meshes = new Transform[4];
	public WheelCollider []WCollider = new WheelCollider[4];

	private Rigidbody rb;
	public Transform centerOfGravity;

	public Material brakeOn;
	public Material brakeOff;
	public Material reverseOn;
	public Material reverseOff;
	public Material indicatorON;
	public Material indicatorOFF;


	public Renderer brakes;
	public Renderer reverse;
	public Renderer indicatorLeft;
	public Renderer indicatorRight;

	public AudioSource []sounds;
	private enum Engine{ON,OFF};
	private enum GearMod{REVERSE,NORMAL};
	private Engine engineStatus = Engine.ON;
	private GearMod gearStatus = GearMod.NORMAL;
	public Text speedText;

	void Start () {
		sounds = GetComponents<AudioSource> ();
		rb = GetComponent<Rigidbody> ();
		rb.centerOfMass = centerOfGravity.localPosition;
		brakes.material = brakeOff;
		reverse.material = reverseOff;
		indicatorLeft.material = indicatorOFF;
		indicatorRight.material = indicatorOFF;
	}
	void Update () {

		calculateCurrentSpeed ();
		UpdateMeshLocations ();
		updateEngineSoundPitch();

		if (Input.GetKey (KeyCode.DownArrow)) {
			brakes.material = brakeOn;
		} else {
			brakes.material = brakeOff;
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			if (gearStatus == GearMod.NORMAL) {
				gearStatus = GearMod.REVERSE;
				reverse.material = reverseOn;
			} else {
				gearStatus = GearMod.NORMAL;
				reverse.material = reverseOff;
			}
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			if (engineStatus == Engine.ON) {
				engineStatus = Engine.OFF;
				sounds [0].Stop ();
				sounds[1].Play ();
				sounds [2].Stop ();
				WCollider [0].motorTorque = 0f;
				WCollider [1].motorTorque = 0f;
			} else {
				engineStatus = Engine.ON;
				sounds[0].Play ();
				sounds [2].Play ();
			}
		}
	}

	void FixedUpdate(){
		if (currentSpeed <= 50f) {
			WCollider [0].steerAngle = maxTurn * Input.GetAxis ("Horizontal")*0.8f;
			WCollider [1].steerAngle = maxTurn * Input.GetAxis ("Horizontal")*0.8f;
		} else {
			WCollider [0].steerAngle = maxTurn * Input.GetAxis ("Horizontal")*0.2f;
			WCollider [1].steerAngle = maxTurn * Input.GetAxis ("Horizontal")*0.2f;
		}
		if (Input.GetKey (KeyCode.Space)) {
			WCollider [0].motorTorque = 0f;
			WCollider [1].motorTorque = 0f;
		} else {
			if (engineStatus == Engine.ON) {
				if (currentSpeed < topSpeed) {
					if (gearStatus == GearMod.NORMAL) {
						WCollider [0].motorTorque = maxTorque * Mathf.Clamp01 (Input.GetAxis ("Vertical"));
						WCollider [1].motorTorque = maxTorque * Mathf.Clamp01 (Input.GetAxis ("Vertical"));
					} else if (gearStatus == GearMod.REVERSE) {
						WCollider [0].motorTorque = -maxTorque * Mathf.Clamp01 (Input.GetAxis ("Vertical"));
						WCollider [1].motorTorque = -maxTorque * Mathf.Clamp01 (Input.GetAxis ("Vertical"));
					}
				} else {
					WCollider [0].motorTorque = 0f;
					WCollider [1].motorTorque = 0f;
				}
			}
		}

		if (Input.GetKey (KeyCode.DownArrow)) {
			WCollider [0].brakeTorque = breakSensitivity;
			WCollider [1].brakeTorque = breakSensitivity;
			WCollider [2].brakeTorque = breakSensitivity;
			WCollider [3].brakeTorque = breakSensitivity;
		} else if (Input.GetKey (KeyCode.Space)) {
			WCollider [0].brakeTorque = 1000f;
			WCollider [1].brakeTorque = 1000f;
			WCollider [2].brakeTorque = 1000f;
			WCollider [3].brakeTorque = 1000f;
		} else {
			WCollider [0].brakeTorque = 0f;
			WCollider [1].brakeTorque = 0f;
			WCollider [2].brakeTorque = 0f;
			WCollider [3].brakeTorque = 0f;
		}

	}

	void UpdateMeshLocations(){
		for (int i = 0; i < 4; i++) {
			Quaternion rot;
			Vector3 loc;
			WCollider [i].GetWorldPose (out loc, out rot);
			meshes [i].position = loc;
			meshes [i].rotation = rot;
		}
	}

	void calculateCurrentSpeed(){
		currentSpeed = 2 * Mathf.PI * WCollider [0].rpm * WCollider [0].radius * 60 / 1000;
		currentSpeed = Mathf.Round (currentSpeed);
		currentSpeed = Mathf.Abs (currentSpeed);
		speedText.text = currentSpeed + " KMPH";
	}
	void updateEngineSoundPitch(){
		sounds [2].pitch = 0.2f+currentSpeed / topSpeed;
	}
}
