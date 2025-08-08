using UnityEngine;

public class InfiniteLivesUnlocker : MonoBehaviour
{
    public HealthManager healthManager;
    public int requiredClicks = 5;
    private int clickCount = 0;

    private void OnMouseDown()
    {
        clickCount++;
        Debug.Log($"Clicked {clickCount} times.");

        if (clickCount >= requiredClicks)
        {
            UnlockInfiniteLives();
            clickCount = 0;
        }
    }

    private void UnlockInfiniteLives()
    {

        if (healthManager != null)
        {
            healthManager.SetInfiniteLives(true);
        }
        else
        {
            Debug.LogWarning("HealthManager reference is missing!");
        }
    }
}
