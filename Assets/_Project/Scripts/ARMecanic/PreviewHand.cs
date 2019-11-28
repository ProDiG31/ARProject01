using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(VideoPlayer))]
public class PreviewHand : MonoBehaviour
{
    public RawImage RawImage;
    public VideoPlayer VideoPlayer;
    private Texture m_RawImageTexture;

    void Start()
    {
        RawImage = GetComponent<RawImage>();
        VideoPlayer = GetComponent<VideoPlayer>();

        VideoPlayer.enabled = false;
        m_RawImageTexture = RawImage.texture;
        VideoPlayer.prepareCompleted += _PrepareCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        if (RawImage.enabled && !VideoPlayer.enabled)
        {
            VideoPlayer.enabled = true;
            VideoPlayer.Play();
        }
        else if (!RawImage.enabled && VideoPlayer.enabled)
        {
            // Stop video playback to save power usage.
            VideoPlayer.Stop();
            RawImage.texture = m_RawImageTexture;
            VideoPlayer.enabled = false;
        }
    }

    private void _PrepareCompleted(VideoPlayer player)
    {
        RawImage.texture = player.texture;
    }
}


