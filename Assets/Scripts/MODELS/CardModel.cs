﻿using System;
using BarcodeScanner;
using BarcodeScanner.Parser;
using MightyAttributes;
using UnityEngine;

public abstract class CardModel : ScriptableObject
{
    [Header("Données carte"), ValidateInput("IndexValid", "index is invalid")]
    public byte index;

    public string nom;
    [ReadOnly] public string qrLink;
    [SerializeField, AssetOnly] private Texture2D _codeImage;

    [SerializeField, AssetOnly] private Sprite _cardSprite;
    [AssetOnly] public AudioClip audioClip;

    public Sprite CardSprite => _cardSprite;
    
#if UNITY_EDITOR
    [Button]
    private void Scan()
    {
        var parser = new ZXingParser();
        
        var result = parser.Decode(_codeImage.GetPixels32(), _codeImage.width, _codeImage.height);
        qrLink = result?.Value;
    }
    
    protected abstract Type AssetType { get; }
    
    private bool IndexValid(byte index)
    {
        foreach (var asset in EditModeUtility.FindAssetsOfType(AssetType))
        {
            if (!(asset is CardModel model)) return false;
            
            if (model.qrLink == qrLink) continue;
            if (model.index == index) return false;
        }
        return true;
    }
#endif
}