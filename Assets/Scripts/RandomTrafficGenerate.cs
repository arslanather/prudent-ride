using UnityEngine;
using System.Collections;

public class RandomTrafficGenerate : MonoBehaviour {

	public GameObject[] traffic;
	public int currentTrafficNumber = 0;
	public int maxTrafficNumber = 10;
	public float minDelay= 8f;
	public float maxDelay= 15f;
	void Start () {
		StartCoroutine (generator ());
	}
	
	IEnumerator generator(){
		yield return new WaitForSeconds (Random.Range(0,6));
		while (true) {

			if (currentTrafficNumber < maxTrafficNumber) {
				int ran = Random.Range (0, traffic.Length);
				GameObject trafficInstance = Instantiate (traffic [ran], transform.position, transform.rotation) as GameObject;
				TrafficCarAI temp = trafficInstance.GetComponent<TrafficCarAI> ();
				temp.pathMap = transform;
				currentTrafficNumber++;
			}

			yield return new WaitForSeconds (Random.Range(minDelay,maxDelay));
		}
		
	}
}
