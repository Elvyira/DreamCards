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
        m_barcodeScanner.Camera.Play();

        // Display the camera texture through a RawImage
        m_barcodeScanner.OnReady += (sender, arg) =>
        {
            // Set Orientation & Texture
            var rectTransform = _rawImage.rectTransform;
            rectTransform.localEulerAngles = m_barcodeScanner.Camera.GetEulerAngles();
            rectTransform.localScale = m_barcodeScanner.Camera.GetScale();
            _rawImage.texture = m_barcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            var sizeDelta = rectTransform.sizeDelta;
            var newHeight = sizeDelta.x * m_barcodeScanner.Camera.Height / m_barcodeScanner.Camera.Width;
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
        m_barcodeScanner.Scan(m_onCardScanned);
    }


    private void OnCardScanned(string barCodeType, string barCodeValue)
    {
        m_barcodeScanner.Stop();
        
        _turnManager.SelectCard(EntitiesDatabase.GetCard(barCodeValue));
    }
}