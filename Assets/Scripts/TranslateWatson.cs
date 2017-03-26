using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v1;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslation;

public class TranslateWatson : MonoBehaviour {

	private LanguageTranslator m_Translate = new LanguageTranslator();
	private string m_PhraseToTranslate = "How do I get to the disco?";

	void Start ()
	{
		Debug.Log("English Phrase to translate: " + m_PhraseToTranslate);
		bool check = m_Translate.GetTranslation(m_PhraseToTranslate, "en", "es", OnGetTranslation);
		Debug.Log (check + " this is the one");
	}

	private void OnGetTranslation(Translations translation)
	{
		Debug.Log ("got here!");
		if (translation != null && translation.translations.Length > 0)
			Debug.Log("Spanish Translation: " + translation.translations[0].translation);
			print("THIS IS DISO I THINK" + translation.translations[0].translation);

	}


	// Update is called once per frame
	void Update () {
		
	}
}
