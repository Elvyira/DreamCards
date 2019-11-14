using System;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using UnityEngine;
using UnityEngine.UI;

public class ScannerManager : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private RawImage _rawImage;

    private IScanner m_barcodeScanner;
    private Action<string, string> m_onCardScanned;

    private void Start()
    {
        // Create a basic scanner
        m_barcodeScanner = new Scanner();
        var scannerCamera = m_barcodeScanner.Camera;
        scannerCamera.Play();

        // Display the camera texture through a RawImage
        m_barcodeScanner.OnReady += (sender, arg) =>
        {
            // Set Orientation & Texture
            var rectTransform = _rawImage.rectTransform;
            rectTransform.localEulerAngles = scannerCamera.GetEulerAngles();
            rectTransform.localScale = scannerCamera.GetScale();
            _rawImage.texture = scannerCamera.Texture;

            // Keep Image Aspect Ratio
            var sizeDelta = rectTransform.sizeDelta;
            var newHeight = sizeDelta.x * scannerCamera.Height / scannerCamera.Width;
            rectTransform.sizeDelta = new Vector2(sizeDelta.x, newHeight);
        };

        // Init callback
        m_onCardScanned = OnCardScanned;
    }

    private void Update()
    {
        m_barcodeScanner.Update();
    }

    public void Scan()
    {
        Debug.Log("Start scan!");
        m_barcodeScanner.Scan(m_onCardScanned);
    }

    public void Stop()
    {
        m_barcodeScanner.Stop();
    }

    private void OnCardScanned(string barCodeType, string barCodeValue)
    {
        m_barcodeScanner.Stop();
        Debug.Log("Scanned!");

        _turnManager.SelectCard(EntitiesDatabase.GetCard(barCodeValue));
    }
}