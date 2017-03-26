using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;

public class LanguageChooser : MonoBehaviour {

	private Dropdown langList;
	private WebCamTextureToCloudVision webcam;
	private Translate translate;

	// Use this for initialization
	void Start () {
		langList = GetComponent<Dropdown> ();

		webcam = GameObject.FindGameObjectWithTag ("Webcam").GetComponent<WebCamTextureToCloudVision> ();

		translate = GameObject.FindGameObjectWithTag ("Translate").GetComponent<Translate> ();

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
			translate.SetCurrentVoice (VoiceType.fr_FR_Renee);
			// set teh french guy

		} else if (language == "Spanish") {
			webcam.SetCurrentLanguage ("es");
			translate.SetCurrentVoice (VoiceType.es_ES_Enrique);
			// set the spanish guy...

		} else if (language == "German") {
			webcam.SetCurrentLanguage ("de");
			translate.SetCurrentVoice (VoiceType.de_DE_Dieter);

		} else if (language == "Italian") {
			webcam.SetCurrentLanguage ("it");
			translate.SetCurrentVoice (VoiceType.it_IT_Francesca);

		} else if (language == "Portuguese") {
			webcam.SetCurrentLanguage ("pt");
			translate.SetCurrentVoice (VoiceType.pt_BR_Isabela);

		}
	}
}
