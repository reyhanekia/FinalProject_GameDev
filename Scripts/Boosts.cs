using UnityEngine;

public class Boosts : MonoBehaviour
{
    public BoostType BoostType;
    public float duration = 5f;

    [Header("Optional Effects")]
    public AudioClip collectSound;

    [Tooltip("If assigned, the collectSound will be played using this AudioSource.")]
    public AudioSource sfxSource;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Ball"))
        {
            isCollected = true;

            if (collectSound != null)
            {
                if (sfxSource != null)
                {
                    sfxSource.PlayOneShot(collectSound);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(collectSound, transform.position);
                }
            }

            LevelProgression.current.ActivateBoost(BoostType, duration);

            Destroy(gameObject);
        }
    }
}
