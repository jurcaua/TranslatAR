using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;

public class Audio : MonoBehaviour {

	static TextToSpeech m_TextToSpeech = new TextToSpeech();
	static string m_TestString = "";
	static string m_ExpressiveText = "<speak version=\"1.0\"><prosody pitch=\"150Hz\">Hello! This is the </prosody><say-as interpret-as=\"letters\">IBM</say-as> Watson <express-as type=\"GoodNews\">Unity</express-as></prosody><say-as interpret-as=\"letters\">SDK</say-as>!</speak>";

	void Start ()
	{
		m_TextToSpeech.Voice = VoiceType.en_US_Allison;
		m_TextToSpeech.ToSpeech(m_TestString, HandleToSpeechCallback);

		m_TextToSpeech.Voice = VoiceType.en_US_Allison;
		m_TextToSpeech.ToSpeech(m_ExpressiveText, HandleToSpeechCallback);
	}

	static void HandleToSpeechCallback (AudioClip clip)
	{
		PlayClip(clip);
	}

	private static void PlayClip(AudioClip clip)
	{
		if (Application.isPlaying && clip != null)
		{
			GameObject audioObject = new GameObject("AudioObject");
			AudioSource source = audioObject.AddComponent<AudioSource>();
			source.spatialBlend = 0.0f;
			source.loop = false;
			source.clip = clip;
			source.Play();

			GameObject.Destroy(audioObject, clip.length);
		}
	}
	public static void setText(string toSet) {
		m_TestString = toSet;
		m_TextToSpeech.Voice = VoiceType.en_US_Allison;
		m_TextToSpeech.ToSpeech(m_TestString, HandleToSpeechCallback);
	}
}