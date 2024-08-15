using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class LimboManager : MonoBehaviour
{
    public static int countVideosWatched;
    private Gargola[] gargolas;
    [SerializeField] private bool completoN1, completoN2, completoN3;

    private void Awake()
    {
        gargolas = FindObjectsOfType<Gargola>();


        UserData.completoNivel1 = completoN1;
        UserData.completoNivel2 = completoN2;
        UserData.completoNivel3 = completoN3;

    }
    private void Start()
    {
        //if (UserData.terminoLimbo)
        //{
        //    foreach (var g in gargolas)
        //    {
        //        g.videoPlayerPlane.SetActive(true);
        //        g.videoPlayerPlane.GetComponent<Animator>().enabled = true;
        //        ulong lastFrame = g.videoPlayer.frameCount - 1;
        //        g.videoPlayer.frame = (long)lastFrame;
        //        g.videoPlayer.Play();
        //        g.videoPlayer.Pause();
        //    }
        //}
    }

}
