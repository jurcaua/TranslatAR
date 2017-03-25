using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v1;


public class translation : MonoBehaviour {

	private LanguageTranslator m_Translate = new LanguageTranslator();
	private string m_PhraseToTranslate = "How do I get to the disco?";

	void Start ()
	{
		Debug.Log("English Phrase to translate: " + m_PhraseToTranslate);
		m_Translate.GetTranslation(m_PhraseToTranslate, "en", "es", OnGetTranslation);
	}

	private void OnGetTranslation(Translations translation)
	{
		if (translation != null && translation.translations.Length > 0)
			Debug.Log("Spanish Translation: " + translation.translations[0].translation);
	}
}
