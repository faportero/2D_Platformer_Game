using UnityEngine;

public class MusicTransitionTrigger : MonoBehaviour
{
    public string newMusicName;
    public float transitionDuration = 2.0f;
    public int musicSourceIndex = 1; // 0 for musicSource1, 1 for musicSource2

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play the new music on the specified source
            AudioManager.Instance.PlayMusic(newMusicName, musicSourceIndex);

            // Initiate the transition between music sources
            AudioManager.Instance.TransitionMusic(transitionDuration);
        }
    }
}
