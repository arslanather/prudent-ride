using UnityEngine;
using System.Collections;

public class TrafficLightStatus : MonoBehaviour {

	public enum SignalStatus { GO , STOP };

	public SignalStatus signal;

	private TrafficCarAI car;


	void OnTriggerStay(Collider other) {
		if (other.gameObject.CompareTag ("Traffic")) {
			car = other.GetComponent<TrafficCarAI> ();
			if (signal == SignalStatus.STOP) {
				car.onSignalHold = true;
			} else {
				car.onSignalHold = false;
			}
		}
	}
	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Traffic")) {
			car = other.GetComponent<TrafficCarAI> ();
				car.onSignalHold = false;
			}
	}

}
