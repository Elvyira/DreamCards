using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer _videoPlayer;

    private VideoClip m_idleClip;

    private void Start()
    {
        _videoPlayer.loopPointReached += LoopReached;
    }

    public void Play(VideoClip clip)
    {
        _videoPlayer.isLooping = false;
        _videoPlayer.clip = clip;
        _videoPlayer.Play();
    }

    public void Play(VideoClip entryClip, VideoClip idleClip)
    {
        _videoPlayer.isLooping = true;
        _videoPlayer.clip = entryClip;
        m_idleClip = idleClip;
        _videoPlayer.Play();
    }

    public void Stop()
    {
        _videoPlayer.Stop();
    }

    private void LoopReached(VideoPlayer player)
    {
        if (_videoPlayer.clip.GetInstanceID() != m_idleClip.GetInstanceID())
            _videoPlayer.clip = m_idleClip;
    }


    #region Editor

#if UNITY_EDITOR
    private void OnValidate()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
    }
#endif

    #endregion /Editor
}