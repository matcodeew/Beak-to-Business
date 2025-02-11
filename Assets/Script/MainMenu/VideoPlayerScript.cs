using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{
    public void StartVideo()
    {
        GetComponent<VideoPlayer>().url = System.IO.Path.Combine (Application.streamingAssetsPath,"Grim.mp4");
        GetComponent<VideoPlayer>().Play();
    }
}
