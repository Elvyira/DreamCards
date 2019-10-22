using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;
using UnityEngine.Video;

public class SimpleDemo : MonoBehaviour {
	public UnityEngine.UI.Image jauge;

    public VideoPlayer videoPlayer;
    public VideoClip videoReine;

	private IScanner BarcodeScanner;
	public Text TextHeader;
	public RawImage Image;
	public AudioSource Audio;

	// Disable Screen Rotation on that screen
	void Awake()
	{
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
	}

	void Start () {
		// Create a basic scanner
		BarcodeScanner = new Scanner();
		BarcodeScanner.Camera.Play();

		// Display the camera texture through a RawImage
		BarcodeScanner.OnReady += (sender, arg) => {
			// Set Orientation & Texture
			Image.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
			Image.transform.localScale = BarcodeScanner.Camera.GetScale();
			Image.texture = BarcodeScanner.Camera.Texture;

			// Keep Image Aspect Ratio
			var rect = Image.GetComponent<RectTransform>();
			var newHeight = rect.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
		};

		// Track status of the scanner
		BarcodeScanner.StatusChanged += (sender, arg) => {
			TextHeader.text = "Status: " + BarcodeScanner.Status;
		};
	}

	/// <summary>
	/// The Update method from unity need to be propagated to the scanner
	/// </summary>
	void Update()
	{
		if (BarcodeScanner == null)
		{
			return;
		}
		BarcodeScanner.Update();
	}

	#region UI Buttons

	public void ClickStart()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Start");
			return;
		}

		// Start Scanning
		BarcodeScanner.Scan(OnCardScanned);
	}


	/////////////
	// C ICI QU'UNE CARTE VIENT D'ETRE SCANNEE
	/////////////
	void OnCardScanned(string barCodeType, string barCodeValue)
	{
		BarcodeScanner.Stop();
		TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;

		// Feedback
		Audio.Play();

		if(barCodeValue == "lareine")
		{
            videoPlayer.clip = videoReine;
            videoPlayer.Play();
            Invoke("StartScan", (float)(videoPlayer.length + 3));
            // ON ACTIVE LA JAUGE
            //jauge.fillAmount = 0;
            //InvokeRepeating("IncreaseJauge", 0, 0.03f);

        }
        else if(barCodeValue == "cauchemard1")
		{
			Debug.Log("texte");
			// maVariableAnim.Play();
		}


		// On vient de scanner une carte
		// on relance le scanner dans 1 seconde
		
	}

	void StartScan()
	{
		// Start Scanning
		BarcodeScanner.Scan(OnCardScanned);
	}

	void IncreaseJauge()
	{
		jauge.fillAmount += 0.005f;
	}




	public void ClickStop()
	{
		if (BarcodeScanner == null)
		{
			Log.Warning("No valid camera - Click Stop");
			return;
		}

		// Stop Scanning
		BarcodeScanner.Stop();
	}

	public void ClickBack()
	{
		// Try to stop the camera before loading another scene
		StartCoroutine(StopCamera(() => {
			SceneManager.LoadScene("Boot");
		}));
	}

	/// <summary>
	/// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
	/// Trying to stop the camera in OnDestroy provoke random crash on Android
	/// </summary>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator StopCamera(Action callback)
	{
		// Stop Scanning
		Image = null;
		BarcodeScanner.Destroy();
		BarcodeScanner = null;

		// Wait a bit
		yield return new WaitForSeconds(0.1f);

		callback.Invoke();
	}

	#endregion
}
