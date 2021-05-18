using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// /-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\
//
// 							TargetMaker 2.0, Copyright © 2017, Ripcord Development
//										  DEMO_PresetCarousel.cs
//										   info@ripcorddev.com
//
// \-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/-\-/

//ABOUT - This script is for demonstration purposes only and is not needed for the actual functionality of TargetMaker
//	    - This script selects a cursor object from a list and instances it into the scene, demonstrating some of the cursor design options possible with TargetMaker

public class DEMO_PresetCarousel : MonoBehaviour {

	public List<GameObject> cursorPresets;
	float moveSpeed = 0.5f;
	float originPoint;


	void Awake () {

		originPoint = cursorPresets[cursorPresets.Count - 1].transform.position.x;
	}

	void Update () {
	
		for (int x = 0; x < cursorPresets.Count; x++) {
			if (cursorPresets[x].transform.position.x < 5.4f) {

				cursorPresets[x].transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
			}
			else {
				cursorPresets[x].transform.position = new Vector3(originPoint, cursorPresets[x].transform.position.y, cursorPresets[x].transform.position.z);
			}
		}
	}
}