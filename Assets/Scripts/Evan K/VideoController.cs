using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(BoxCollider2D))]
public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player entered the video trigger zone!");
            videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.Play();
        }
    }
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        // Subscribe to the event that fires when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        gameObject.SetActive(false);
    }
}
