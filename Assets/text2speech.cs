using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class text2speech : MonoBehaviour {
	public AudioSource _audio;
	public InputField inputText;

	// Use this for initialization
	void Start () {
		_audio = gameObject.GetComponent<AudioSource> ();
	}

	void Update () {
		
	}

	IEnumerator DownloadTheAudio(){
		string url = "http://api.voicerss.org/?key=9bf74c7ced8d468cbbf77931cf211744=en-us&src=" + inputText.text;
		WWW www = new WWW (url);
		yield return www;

		_audio.clip = www.GetAudioClip (false, true, AudioType.MPEG);
		_audio.Play ();
	}
	public void ButtonClick(){
		StartCoroutine (DownloadTheAudio ());


}
	
	// Update is called once per frame

}
