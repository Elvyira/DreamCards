using System;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using UnityEngine;
using UnityEngine.UI;

public class ScannerManager : MonoBehaviour
{
    [SerializeField, Manager] private TurnManager _turnManager;
    [SerializeField] private RawImage _rawImage;

    private IScanner m_barcodeScanner;
    private Action<string, string> m_onCardScanned;

    public void Init()
    {
        InitScanner();
        InitStream(m_barcodeScanner.Camera.Texture);
        InitCallback(OnCardScanned);
    }

    public void InitScanner()
    {
        m_barcodeScanner = new Scanner();
        m_barcodeScanner.Camera.Play();
    }

    public void InitStream(Texture texture)
    {
        m_barcodeScanner.OnReady += (sender, arg) =>
        {
            var scannerCamera = m_barcodeScanner.Camera;

            // Set Orientation & Texture
            var rectTransform = _rawImage.rectTransform;
            rectTransform.localEulerAngles = scannerCamera.GetEulerAngles();
            rectTransform.localScale = scannerCamera.GetScale();
            _rawImage.texture = texture;

            // Keep Image Aspect Ratio
            var sizeDelta = rectTransform.sizeDelta;
            var newHeight = sizeDelta.x * scannerCamera.Height / scannerCamera.Width;
            rectTransform.sizeDelta = new Vector2(sizeDelta.x, newHeight);
        };
    }

    public void InitCallback(Action<string, string> callback) => m_onCardScanned = callback;

    public void UpdateManager() => m_barcodeScanner.Update();

    public void Scan() => m_barcodeScanner.Scan(m_onCardScanned);

    public void Stop() => m_barcodeScanner.Stop();

    private void OnCardScanned(string barCodeType, string barCodeValue)
    {
        m_barcodeScanner.Stop();
        Debug.Log(barCodeValue);

        _turnManager.SelectCard(EntitiesDatabase.GetCard(barCodeValue));
    }
}