using System;
using MightyAttributes;
using UnityEngine;

[Serializable]
public enum SFXSource : byte
{
    Advance,
    TurnPage,

    Confirm,
    Cancel,

    ReussiteCritique,
    Reussite,
    Echec,
    EchecCritique,
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _dynamicSource;

    [SerializeField, GetComponentsInChildren, SFXClipSources]
    private AudioSource[] _sources;

    [Button]
    public void PlayAdvance() => PlaySource(SFXSource.Advance);

    [Button]
    public void PlayTurnPage() => PlaySource(SFXSource.TurnPage);

    [Button]
    public void PlayConfirm() => PlaySource(SFXSource.Confirm);

    [Button]
    public void PlayCancel() => PlaySource(SFXSource.Cancel);

    [Button]
    public void PlayReussiteCritique() => PlaySource(SFXSource.ReussiteCritique);

    [Button]
    public void PlayReussite() => PlaySource(SFXSource.Reussite);

    [Button]
    public void PlayEchec() => PlaySource(SFXSource.Echec);

    [Button]
    public void PlayEchecCritique() => PlaySource(SFXSource.EchecCritique);

    public void PlaySource(SFXSource source) => PlaySource((byte) source);
    private void PlaySource(byte clipIndex) => _sources[clipIndex].Play();

    public void PlayClip(AudioClip clip) => _dynamicSource.PlayOneShot(clip);
}