using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour {


	public NavMeshSurface[] nSurface;

	void Start () {

		for (int i = 0; i < nSurface.Length; i++) {
			nSurface[i].BuildNavMesh ();
		}
	}
}
