using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARJTalking : MonoBehaviour {

	private static Animator a;
	private static float waitFor = 0;

	// Use this for initialization
	void Start () {
		a = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void setTalking(float len){
		a.SetBool ("Talking", true);
		waitFor = len;
	}

	static IEnumerator WaitForTalking(){
		yield return new WaitForSeconds (waitFor);
		a.SetBool ("Talking", false);
	}
}
