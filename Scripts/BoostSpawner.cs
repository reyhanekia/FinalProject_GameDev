using UnityEngine;

public class BoostSpawner : MonoBehaviour
{
    public GameObject[] BoostItems;
    public float spawnInterval = 10f;
    public float safeMargin = 0.5f;

    private float timer;
    private Camera mainCamera;
    private GameObject currentPowerUp;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    void SpawnPowerUp()
    {
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
        }

        int index = Random.Range(0, BoostItems.Length);
        GameObject prefab = BoostItems[index];

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        float halfWidth = 0.5f;
        float halfHeight = 0.5f;

        if (sr != null)
        {
            halfWidth = sr.bounds.size.x / 2f;
            halfHeight = sr.bounds.size.y / 2f;
        }

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX = mainCamera.transform.position.x - camWidth + halfWidth + safeMargin;
        float maxX = mainCamera.transform.position.x + camWidth - halfWidth - safeMargin;

        float minY = mainCamera.transform.position.y - camHeight + halfHeight + safeMargin;
        float maxY = mainCamera.transform.position.y + camHeight - halfHeight - safeMargin;

        Vector2 spawnPos = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        currentPowerUp = Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
