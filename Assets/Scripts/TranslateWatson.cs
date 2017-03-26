using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v1;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslation;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;

public class TranslateWatson : MonoBehaviour {

	private LanguageTranslator m_Translate = new LanguageTranslator();
	private string m_PhraseToTranslate = "How do I get to the disco?";

	// Here's where we store the translated text!
	private string translatedText = " ";

	private WebCamTextureToCloudVision vision;

	[HideInInspector] public VoiceType curr_voice;

	void Start (){
		vision = GameObject.FindGameObjectWithTag ("Webcam").GetComponent<WebCamTextureToCloudVision> ();
	}

	private void OnGetTranslation(Translations translation){
		if (translation != null && translation.translations.Length > 0) {
			setTranslatedString (translation.translations [0].translation);
			vision.SetTranslated(translatedText);
			Audio.setText(translatedText, curr_voice);
		} else {
			translatedText = "*ERROR*";
		}
	}

	public void Process(string targetLang, string sourceText){
		if (!m_Translate.GetTranslation (sourceText, "en", targetLang, OnGetTranslation)) {
			Debug.Log ("TRANSLATION HAS FAILED");
		}
	}

	public void SetCurrentVoice (VoiceType curr){
		curr_voice = curr;
	}

	public void setTranslatedString(string toSet){
		translatedText = toSet;
	}

	public string getTranslatedText(){
		return translatedText;
	}

	public VoiceType getCurrentVoice(){
		return curr_voice;
	}
}
