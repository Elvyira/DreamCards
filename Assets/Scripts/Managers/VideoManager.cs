using System;
using MightyAttributes;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoManager : MonoBehaviour
{
    [SerializeField, GetComponent, ReadOnly] private VideoPlayer _videoPlayer;

    private VideoClip m_idleClip;
    private Action m_clipOverAction;

    public void Init() => _videoPlayer.loopPointReached += LoopReached;

    public void Play(VideoClip clip, Action clipOverAction = null)
    {
        _videoPlayer.isLooping = false;
        _videoPlayer.clip = clip;
        _videoPlayer.Play();

        m_clipOverAction = clipOverAction;
    }

    public void Play(VideoClip entryClip, VideoClip idleClip, Action entryClipOverAction = null)
    {
        _videoPlayer.isLooping = true;
        _videoPlayer.clip = entryClip;
        m_idleClip = idleClip;
        _videoPlayer.Play();

        m_clipOverAction = entryClipOverAction;
    }

    public void Stop() => _videoPlayer.Stop();

    private void LoopReached(VideoPlayer player)
    {
        if (_videoPlayer.clip.GetInstanceID() == m_idleClip.GetInstanceID()) return;
        
        if (_videoPlayer.isLooping) _videoPlayer.clip = m_idleClip;
        m_clipOverAction?.Invoke();
    }
}