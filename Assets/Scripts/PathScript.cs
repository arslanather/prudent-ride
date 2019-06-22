using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathScript : MonoBehaviour {
	public Color lineColor;
	private List<Transform> path = new List<Transform>();

	void OnDrawGizmos(){
		Gizmos.color = lineColor;

		Transform[] pathTransforms = GetComponentsInChildren<Transform> ();

		path = new List<Transform> ();

		for (int i = 0; i < pathTransforms.Length; i++) {
			if (pathTransforms [i] != transform) {
				path.Add (pathTransforms [i]);
			}
		}

		for (int i = 0; i < path.Count; i++) {
			Vector3 currentNode = path [i].position;

			if (i > 0) {
				Vector3 prevNode = path [i - 1].position;
				Gizmos.DrawLine (prevNode, currentNode);
				Gizmos.DrawWireSphere (currentNode, 0.3f);
			}
		}
	}
}
