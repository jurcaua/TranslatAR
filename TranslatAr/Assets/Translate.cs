// We need this for parsing the JSON, unless you use an alternative.
// You will need SimpleJSON if you don't use alternatives.
// It can be gotten hither. http://wiki.unity3d.com/index.php/SimpleJSON
using SimpleJSON;
using UnityEngine;
using System.Collections;

public class Translate : MonoBehaviour {

	// Should we debug?
	public bool isDebug = false;

	// Here's where we store the translated text!
	private string translatedText = "";

	// This is only called when the scene loads.
	void Start () {
		// Strictly for debugging to test a few words!
		if(isDebug)
			StartCoroutine (Process ("en","Bonsoir."));
	}

	// We have use googles own api built into google Translator.
	public IEnumerator Process (string targetLang, string sourceText) {
		// We use Auto by default to determine if google can figure it out.. sometimes it can't.
		string sourceLang = "auto";
		// Construct the url using our variables and googles api.
		string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" 
			+ sourceLang + "&tl=" + targetLang + "&dt=t&q=" + WWW.EscapeURL(sourceText);

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
				if(isDebug)
					print(translatedText);
			}
		}
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