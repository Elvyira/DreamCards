using System;
using BarcodeScanner;
using BarcodeScanner.Scanner;
using MightyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScannerManager : MonoBehaviour
{
    [SerializeField, FindReadOnly] private TurnManager _turnManager;

    [Debug] public bool debug;
    [SerializeField, ShowIfDebug] private RawImage _rawImage;

    [Line]
    [SerializeField, ShowIfDebug, LabelledSlider("SommeilName", "MaxSommeilIndex")]
    private int _sommeilIndex;

    [SerializeField, ShowIfDebug, LabelledSlider("ObjetName", "MaxObjetIndex")]
    private int _objetIndex;

    private IScanner m_barcodeScanner;
    private Action<string, string> m_onCardScanned;

    public void Init()
    {
        m_barcodeScanner = new Scanner();

        m_barcodeScanner.Camera.Play();
        m_barcodeScanner.Camera.Pause();

        if (debug) m_barcodeScanner.OnReady += OnScannerReady;

        m_onCardScanned = OnCardScanned;
    }

    public void StartScan()
    {
        m_barcodeScanner.Camera.Play();
        m_barcodeScanner.Scan(m_onCardScanned);
    }

    public void StopScan()
    {
        m_barcodeScanner.Camera.Pause();
        m_barcodeScanner.Stop();
    }

    public void UpdateManager()
    {
        m_barcodeScanner.Update();
    }

    private void OnScannerReady(object sender, EventArgs args)
    {
        var scannerCamera = m_barcodeScanner.Camera;

        // Set Orientation & Texture
        var rectTransform = _rawImage.rectTransform;
        rectTransform.localEulerAngles = scannerCamera.GetEulerAngles();
        rectTransform.localScale = scannerCamera.GetScale();
        _rawImage.texture = scannerCamera.Texture;

        // Keep Image Aspect Ratio
        var sizeDelta = rectTransform.sizeDelta;
        var newHeight = sizeDelta.x * scannerCamera.Height / scannerCamera.Width;
        rectTransform.sizeDelta = new Vector2(sizeDelta.x, newHeight);
    }

    private void OnCardScanned(string barCodeType, string barCodeValue) =>
        _turnManager.SelectCard(InstanceManager.EntitiesManager.GetCard(barCodeValue));

#if UNITY_EDITOR

    #region Editor

    private string SommeilName => InstanceManager.EntitiesManager.SommeilEntities[_sommeilIndex].nom;
    private string ObjetName => InstanceManager.EntitiesManager.ObjetEntities[_objetIndex].nom;

    private int MaxSommeilIndex => InstanceManager.EntitiesManager.SommeilEntities.Length - 1;
    private int MaxObjetIndex => InstanceManager.EntitiesManager.ObjetEntities.Length - 1;

    private bool CanSelectSommeil() => debug && EditorApplication.isPlaying && _turnManager.TurnState == TurnState.Sommeil;
    private bool CanSelectObjet() => debug && EditorApplication.isPlaying && _turnManager.TurnState == TurnState.Objet;

    [Button(enabledCallback: "CanSelectSommeil")]
    private void SelectSommeil() => _turnManager.SelectCard(InstanceManager.EntitiesManager.SommeilEntities[_sommeilIndex]);

    [Button(enabledCallback: "CanSelectObjet")]
    private void SelectObjet() => _turnManager.SelectCard(InstanceManager.EntitiesManager.ObjetEntities[_objetIndex]);

    #endregion /Editor

#endif
}