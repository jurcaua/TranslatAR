using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageChooser : MonoBehaviour {

	private Dropdown langList;
	private WebCamTextureToCloudVision webcam;

	// Use this for initialization
	void Start () {
		langList = GetComponent<Dropdown> ();

		webcam = GameObject.FindGameObjectWithTag ("Webcam").GetComponent<WebCamTextureToCloudVision> ();

		langList.onValueChanged.AddListener(delegate {
			ChangeLanguage(langList);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void ChangeLanguage(Dropdown target){
		string language = target.captionText.text;

		if (language == "French") {
			webcam.SetCurrentLanguage ("fr");

		} else if (language == "Spanish") {
			webcam.SetCurrentLanguage ("es");

		} else if (language == "German") {
			webcam.SetCurrentLanguage ("de");

		} else if (language == "Italian") {
			webcam.SetCurrentLanguage ("it");

		} else if (language == "Portugal") {
			webcam.SetCurrentLanguage ("pt");

		}

			
	}
}
