using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamTextureToCloudVision : MonoBehaviour {

	private string url = "https://vision.googleapis.com/v1/images:annotate?key=";
	private string apiKey = "AIzaSyDcaHwd9Bf4T9Mwen7D2jSwLL0KvVVi3EE";
	public float captureIntervalSeconds = 5.0f;
	public int requestedWidth = 640;
	public int requestedHeight = 480;
	public FeatureType featureType = FeatureType.FACE_DETECTION;
	public int maxResults = 10;

	[HideInInspector] public string curr_lang = "fr";

	public GameObject canvas;
	public Transform textSpawnPoint;
	public Text translatedSpawnPoint;
	public GameObject label;
	private GameObject currentLabel;
	private TranslateWatson translate;

	WebCamTexture webcamTexture;
	Texture2D texture2D;
	Dictionary<string, string> headers;

	[System.Serializable]
	public class AnnotateImageRequests {
		public List<AnnotateImageRequest> requests;
	}

	[System.Serializable]
	public class AnnotateImageRequest {
		public Image image;
		public List<Feature> features;
	}

	[System.Serializable]
	public class Image {
		public string content;
	}

	[System.Serializable]
	public class Feature {
		public string type;
		public int maxResults;
	}

	[System.Serializable]
	public class ImageContext {
		public LatLongRect latLongRect;
		public List<string> languageHints;
	}

	[System.Serializable]
	public class LatLongRect {
		public LatLng minLatLng;
		public LatLng maxLatLng;
	}

	[System.Serializable]
	public class AnnotateImageResponses {
		public List<AnnotateImageResponse> responses;
	}

	[System.Serializable]
	public class AnnotateImageResponse {
		public List<FaceAnnotation> faceAnnotations;
		public List<EntityAnnotation> landmarkAnnotations;
		public List<EntityAnnotation> logoAnnotations;
		public List<EntityAnnotation> labelAnnotations;
		public List<EntityAnnotation> textAnnotations;
	}

	[System.Serializable]
	public class FaceAnnotation {
		public BoundingPoly boundingPoly;
		public BoundingPoly fdBoundingPoly;
		public List<Landmark> landmarks;
		public float rollAngle;
		public float panAngle;
		public float tiltAngle;
		public float detectionConfidence;
		public float landmarkingConfidence;
		public string joyLikelihood;
		public string sorrowLikelihood;
		public string angerLikelihood;
		public string surpriseLikelihood;
		public string underExposedLikelihood;
		public string blurredLikelihood;
		public string headwearLikelihood;
	}

	[System.Serializable]
	public class EntityAnnotation {
		public string mid;
		public string locale;
		public string description;
		public float score;
		public float confidence;
		public float topicality;
		public BoundingPoly boundingPoly;
		public List<LocationInfo> locations;
		public List<Property> properties;
	}

	[System.Serializable]
	public class BoundingPoly {
		public List<Vertex> vertices;
	}

	[System.Serializable]
	public class Landmark {
		public string type;
		public Position position;
	}

	[System.Serializable]
	public class Position {
		public float x;
		public float y;
		public float z;
	}

	[System.Serializable]
	public class Vertex {
		public float x;
		public float y;
	}

	[System.Serializable]
	public class LocationInfo {
		LatLng latLng;
	}

	[System.Serializable]
	public class LatLng {
		float latitude;
		float longitude;
	}

	[System.Serializable]
	public class Property {
		string name;
		string value;
	}

	public enum FeatureType {
		TYPE_UNSPECIFIED,
		FACE_DETECTION,
		LANDMARK_DETECTION,
		LOGO_DETECTION,
		LABEL_DETECTION,
		TEXT_DETECTION,
		SAFE_SEARCH_DETECTION,
		IMAGE_PROPERTIES
	}

	public enum LandmarkType {
		UNKNOWN_LANDMARK,
		LEFT_EYE,
		RIGHT_EYE,
		LEFT_OF_LEFT_EYEBROW,
		RIGHT_OF_LEFT_EYEBROW,
		LEFT_OF_RIGHT_EYEBROW,
		RIGHT_OF_RIGHT_EYEBROW,
		MIDPOINT_BETWEEN_EYES,
		NOSE_TIP,
		UPPER_LIP,
		LOWER_LIP,
		MOUTH_LEFT,
		MOUTH_RIGHT,
		MOUTH_CENTER,
		NOSE_BOTTOM_RIGHT,
		NOSE_BOTTOM_LEFT,
		NOSE_BOTTOM_CENTER,
		LEFT_EYE_TOP_BOUNDARY,
		LEFT_EYE_RIGHT_CORNER,
		LEFT_EYE_BOTTOM_BOUNDARY,
		LEFT_EYE_LEFT_CORNER,
		RIGHT_EYE_TOP_BOUNDARY,
		RIGHT_EYE_RIGHT_CORNER,
		RIGHT_EYE_BOTTOM_BOUNDARY,
		RIGHT_EYE_LEFT_CORNER,
		LEFT_EYEBROW_UPPER_MIDPOINT,
		RIGHT_EYEBROW_UPPER_MIDPOINT,
		LEFT_EAR_TRAGION,
		RIGHT_EAR_TRAGION,
		LEFT_EYE_PUPIL,
		RIGHT_EYE_PUPIL,
		FOREHEAD_GLABELLA,
		CHIN_GNATHION,
		CHIN_LEFT_GONION,
		CHIN_RIGHT_GONION
	};

	public enum Likelihood {
		UNKNOWN,
		VERY_UNLIKELY,
		UNLIKELY,
		POSSIBLE,
		LIKELY,
		VERY_LIKELY
	}

	// Use this for initialization
	void Start () {

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
		}

		textSpawnPoint.gameObject.SetActive (false);
		//translatedSpawnPoint.gameObject.SetActive (false);

		currentLabel = Instantiate (label, textSpawnPoint.position, textSpawnPoint.rotation, canvas.transform) as GameObject;

		translate = GameObject.FindGameObjectWithTag ("Translate").GetComponent<TranslateWatson> ();

		//CameraDevice.Instance.SetFrameFormat (PixelFormat, true);

		//Application.RequestUserAuthorization (UserAuthorization.WebCam);

		headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json; charset=UTF-8");

		if (apiKey == null || apiKey == "")
			Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");

		WebCamDevice[] devices = WebCamTexture.devices;
		for (var i = 0; i < devices.Length; i++) {
			Debug.Log (devices [i].name);
		}
		if (devices.Length > 0) {
			webcamTexture = new WebCamTexture(devices[0].name, requestedWidth, requestedHeight);
			//webcamTexture = new WebCamTexture(devices[0].name);
			Renderer r = GetComponent<Renderer> ();
			if (r != null) {
				Material m = r.material;
				if (m != null) {
					m.mainTexture = webcamTexture;
				}
			}
			webcamTexture.Play();
			StartCoroutine("Capture");
		}	
	}

	// Update is called once per frame
	void Update () {
		if (webcamTexture != null || webcamTexture.didUpdateThisFrame)
		{
			Renderer r = GetComponent<Renderer> ();
			if (r != null) {
				Material m = r.material;
				if (m != null) {
					m.mainTexture = webcamTexture;
				}
			}
		}
		if (!webcamTexture.isPlaying) {
			webcamTexture.Play ();
		}
	}

	private IEnumerator Capture() {
		while (true) {
			if (this.apiKey == null)
				yield return null;

			yield return new WaitForSeconds(captureIntervalSeconds);

			Color[] pixels = webcamTexture.GetPixels();
			if (pixels.Length == 0)
				yield return null;
			if (texture2D == null || webcamTexture.width != texture2D.width || webcamTexture.height != texture2D.height) {
				texture2D = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
			}

			texture2D.SetPixels(pixels);
			// texture2D.Apply(false); // Not required. Because we do not need to be uploaded it to GPU
			byte[] jpg = texture2D.EncodeToJPG();
			string base64 = System.Convert.ToBase64String(jpg);
			#if UNITY_WEBGL	
			Application.ExternalCall("post", this.gameObject.name, "OnSuccessFromBrowser", "OnErrorFromBrowser", this.url + this.apiKey, base64, this.featureType.ToString(), this.maxResults);
			#else

			AnnotateImageRequests requests = new AnnotateImageRequests();
			requests.requests = new List<AnnotateImageRequest>();

			AnnotateImageRequest request = new AnnotateImageRequest();
			request.image = new Image();
			request.image.content = base64;
			request.features = new List<Feature>();

			Feature feature = new Feature();
			feature.type = this.featureType.ToString();
			feature.maxResults = this.maxResults;

			request.features.Add(feature); 

			requests.requests.Add(request);

			string jsonData = JsonUtility.ToJson(requests, false);
			if (jsonData != string.Empty) {
				Debug.Log(this.url);
				Debug.Log(this.apiKey);
				string url = this.url + this.apiKey;
				Debug.Log(url);
				byte[] postData = System.Text.Encoding.Default.GetBytes(jsonData);
				using(WWW www = new WWW(url, postData, headers)) {
					yield return www;
					Debug.Log(www.text);
					if (www.error == null) {
						Debug.Log(www.text.Replace("\n", "").Replace(" ", ""));
						AnnotateImageResponses responses = JsonUtility.FromJson<AnnotateImageResponses>(www.text);
						// SendMessage, BroadcastMessage or someting like that.
						Sample_OnAnnotateImageResponses(responses);
						if (responses.responses.Count > 0 && responses.responses[0].labelAnnotations.Count > 0){

							// make a new text object and set the text to it
							Destroy(currentLabel);
							currentLabel = Instantiate (label, textSpawnPoint.position, textSpawnPoint.rotation, canvas.transform)  as GameObject;
							currentLabel.GetComponent<Text>().text = responses.responses[0].labelAnnotations[0].description;

							// translate it
							translate.Process(curr_lang, currentLabel.GetComponent<Text>().text);
						}
					} else {
						Debug.Log("Error: " + www.error);
					}
				}
			}
			#endif
		}
	}

	#if UNITY_WEBGL
	void OnSuccessFromBrowser(string jsonString) {
	Debug.Log(jsonString);	
	AnnotateImageResponses responses = JsonUtility.FromJson<AnnotateImageResponses>(jsonString);
	Sample_OnAnnotateImageResponses(responses);
	}

	void OnErrorFromBrowser(string jsonString) {
	Debug.Log(jsonString);	
	}
	#endif

	/// <summary>
	/// A sample implementation.
	/// </summary>
	void Sample_OnAnnotateImageResponses(AnnotateImageResponses responses) {
		if (responses.responses.Count > 0) {
			if (responses.responses[0].faceAnnotations != null && responses.responses[0].faceAnnotations.Count > 0) {
				Debug.Log("joyLikelihood: " + responses.responses[0].faceAnnotations[0].joyLikelihood);
			}
		}
	}

	public void SetTranslated(string toSet){
		translatedSpawnPoint.text = toSet;
	}

	public void SetCurrentLanguage (string curr){
		curr_lang = curr;
	}
}
