using UnityEngine;
using System.Collections;

public class TrafficLightsController : MonoBehaviour {

	public Renderer [] redSignals = new Renderer[4];
	public Renderer [] yellowSignals = new Renderer[4];
	public Renderer [] greenSignals = new Renderer[4];
	public Material [] redLightMaterial = new Material[2];
	public Material [] yellowLightMaterial = new Material[2];
	public Material[] greenLightMaterial = new Material[2];
	public GameObject[] signalsStats = new GameObject[4];
	private TrafficLightStatus [] status = new TrafficLightStatus[4];
	private int turn = 0; 

	public float delayTime = 60f;

	void Start () {
		for (int i = 0; i < signalsStats.Length; i++) {
			status [i] = signalsStats [i].GetComponent<TrafficLightStatus> ();
		}
		for (int i = 0; i < 4; i++) {
			status [i].signal = TrafficLightStatus.SignalStatus.STOP;
			redSignals [i].material = redLightMaterial [1];
			yellowSignals [i].material = yellowLightMaterial [0];
			greenSignals [i].material = greenLightMaterial [0];
		}

		StartCoroutine (signalCounter ());
	}

	IEnumerator signalCounter(){
		while (true) {


			redSignals [turn].material = redLightMaterial [0];
			yellowSignals [turn].material = yellowLightMaterial [0];
			greenSignals [turn].material = greenLightMaterial [1];
			status [turn].signal = TrafficLightStatus.SignalStatus.GO;

			yield return new WaitForSeconds (delayTime);

			redSignals [turn].material = redLightMaterial [0];
			yellowSignals [turn].material = yellowLightMaterial [1];
			greenSignals [turn].material = greenLightMaterial [0];
			status [turn].signal = TrafficLightStatus.SignalStatus.STOP;

			yield return new WaitForSeconds (2f);

			redSignals [turn].material = redLightMaterial [1];
			yellowSignals [turn].material = yellowLightMaterial [0];
			greenSignals [turn].material = greenLightMaterial [0];
			status [turn].signal = TrafficLightStatus.SignalStatus.STOP;

			turn++;
			if (turn == 4)
				turn = 0;
		}


	}
}
