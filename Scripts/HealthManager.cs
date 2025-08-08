using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public int lives;

    public TMP_Text livesText;
    public Transform playerTransform;
    public GameObject gameOverPanel;
    public GameObject Ball;

    [Header("Gameplay Controllers")]
    public GameObject[] gameplayScriptsToDisable;

    private Vector3 startPosition;

    private bool infiniteLives = false;

    private void Start()
    {
        lives = PlayerPrefs.GetInt("Lives", 3);

        if (lives <= 0)
        {
            lives = 3;
            SaveLives();
        }
        else if (lives < 3) {

            lives = 3;
            SaveLives();
        }

        startPosition = playerTransform.position;
        UpdateLivesText();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        foreach (GameObject obj in gameplayScriptsToDisable)
        {
            obj.SetActive(true);
        }
    }

    public void SetInfiniteLives(bool enabled)
    {
        infiniteLives = enabled;

        if (infiniteLives)
        {
            UpdateLivesText();
        }
        else
        {
            UpdateLivesText();
        }
    }

    public void AddLives(int amount)
    {
        if (infiniteLives)
        {
            return;
        }

        lives += amount;
        SaveLives();
        UpdateLivesText();
    }

    public void TakeDamage()
    {
        if (infiniteLives)
        {
            return;
        }

        lives--;

        if (lives < 0)
            lives = 0;

        SaveLives();

        UpdateLivesText();

        if (lives <= 0)
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
            foreach (GameObject obj in gameplayScriptsToDisable)
            {
                obj.SetActive(false);
            }

            if (Ball != null)
                Ball.SetActive(false);
        }
        else
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        playerTransform.position = startPosition;

        Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.bodyType = RigidbodyType2D.Static;
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.position = startPosition;
        }
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            if (infiniteLives)
            {
                livesText.text = "∞";
            }
            else
            {
                livesText.text = "" + lives;
            }
        }
    }

    private void SaveLives()
    {
        if (infiniteLives)
        {
            PlayerPrefs.SetInt("Lives", 9999);         }
        else
        {
            PlayerPrefs.SetInt("Lives", lives);
        }

        PlayerPrefs.Save();
    }
}
