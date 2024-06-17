using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherSegmentTrigger : MonoBehaviour
{
    public GameObject tracker;
    public GameObject trackerold;
    public GameObject progress;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            trackerold.SetActive(false);
            tracker.SetActive(true);
            tracker.GetComponent<DistanceTracker>().enabled = true;
            progress.SetActive(true);
            progress.GetComponent<UI_Progress>().enabled = true;
        }
    }
}
