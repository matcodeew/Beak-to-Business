using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    public void StartVideo()
    {
        GetComponent<VideoPlayer>().Play();
    }
}
