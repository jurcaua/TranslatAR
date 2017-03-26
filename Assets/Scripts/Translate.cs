﻿// We need this for parsing the JSON, unless you use an alternative.
// You will need SimpleJSON if you don't use alternatives.
// It can be gotten hither. http://wiki.unity3d.com/index.php/SimpleJSON
using SimpleJSON;
using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;

public class Translate : MonoBehaviour {

	// Should we debug?
	public bool isDebug = false;

	// Here's where we store the translated text!
	private string translatedText = "";

	private WebCamTextureToCloudVision vision;

	[HideInInspector] public VoiceType curr_voice;

	// This is only called when the scene loads.
	void Start () {
		vision = GameObject.FindGameObjectWithTag ("Webcam").GetComponent<WebCamTextureToCloudVision> ();

		curr_voice = VoiceType.fr_FR_Renee;

		// Strictly for debugging to test a few words!
		if(isDebug)
			StartCoroutine (Process ("en","Bonsoir."));
	}

	// We have use googles own api built into google Translator.
	public IEnumerator Process (string targetLang, string sourceText) {
		// We use Auto by default to determine if google can figure it out.. sometimes it can't.
		//string sourceLang = "auto";
		string sourceLang = "en";
		// Construct the url using our variables and googles api.
		string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" 
			+ sourceLang + "&tl=" + targetLang + "&dt=t&q=" + WWW.EscapeURL(sourceText);
//		string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=AIzaSyDcaHwd9Bf4T9Mwen7D2jSwLL0KvVVi3EE=" + sourceLang + "&tl=" + targetLang + 
//		"&dt=t&q=" + WWW.EscapeURL(sourceText);

		// Put together our unity bits for the web call.
		WWW www = new WWW (url);
		// Now we actually make the call and wait for it to finish.
		yield return www;

		// Check to see if it's done.
		if (www.isDone) {
			// Check to see if we don't have any errors.
			if(string.IsNullOrEmpty(www.error)){
				// Parse the response using JSON.
				var N = JSONNode.Parse(www.text);
				// Dig through and take apart the text to get to the good stuff.
				translatedText = N[0][0][0];
				// This is purely for debugging in the Editor to see if it's the word you wanted.
				if (isDebug) {
					print (translatedText);
				}
				// here we have translatedText, do something with it:
				Audio.setText(translatedText, curr_voice);
			
				vision.SetTranslated(translatedText);
			}
		}

	}
	public void SetCurrentVoice (VoiceType curr){
		curr_voice = curr;
	}



	// Exactly the same as above but allow the user to change from Auto, for when google get's all Jerk Butt-y
	public IEnumerator Process (string sourceLang, string targetLang, string sourceText) {
		string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" 
			+ sourceLang + "&tl=" + targetLang + "&dt=t&q=" + WWW.EscapeURL(sourceText);

		WWW www = new WWW (url);
		yield return www;

		if (www.isDone) {
			if(string.IsNullOrEmpty(www.error)){
				var N = JSONNode.Parse(www.text);
				translatedText = N[0][0][0];
				if(isDebug)
					print(translatedText);
			}
		}
	}
}