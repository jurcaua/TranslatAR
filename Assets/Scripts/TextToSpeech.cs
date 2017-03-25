using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Net;


public class TextToSpeech : MonoBehaviour {

	void Start (){
		string remoteUri = "https://translate.google.com/translate_tts?ie=UTF-8&q=hello%20i%27m%20alex&tl=en&total=1&idx=0&textlen=14&tk=579643.952699&client=t";
//		string remoteUri = "https://translate.google.com/translate_tts?ie=UTF-8&q=apple&tl=en&total=1&idx=0&textlen=5&tk=938543.525078&client=t";
//		string remoteUri = "https://translate.google.com/translate_tts?ie=UTF-8&q=salut%comment&tl=fr&total=1&idx=0&textlen=19&tk=585687.966382&client=t";

		string fileName = "boobs.mp3", myStringWebResource = null;
		// Create a new WebClient instance.
		WebClient myWebClient = new WebClient();
		// Concatenate the domain with the Web resource filename.
		myStringWebResource = remoteUri + fileName;
		print (myStringWebResource);
//		Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
		// Download the Web resource and save it into the current filesystem folder.
		myWebClient.DownloadFile(remoteUri,fileName);		
//		Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
//		Console.WriteLine("\nDownloaded file saved in the following file system folder:\n\t" + Application.StartupPath);
	}
}