using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Physics & Control")]
    [SerializeField] private float forceMultiplier = 0.5f;
    public float ForceMultiplier
    {
        get => forceMultiplier;
        set => forceMultiplier = value;
    }

    private Rigidbody2D rb;
    private bool gameStarted = false;

    private Vector2 swipeStart;
    private bool isSwiping = false;

    [Header("References")]
    public LevelProgression gameManager;
    public AudioSource jumpAudio;

    private HealthManager healthManager;

    private bool invertControl = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        healthManager = FindObjectOfType<HealthManager>();
    }

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            Vector2 swipeEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ProcessSwipe(swipeStart, swipeEnd);
            isSwiping = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            swipeStart = Camera.main.ScreenToWorldPoint(touch.position);
            isSwiping = true;
        }

        if (touch.phase == TouchPhase.Ended && isSwiping)
        {
            Vector2 swipeEnd = Camera.main.ScreenToWorldPoint(touch.position);
            ProcessSwipe(swipeStart, swipeEnd);
            isSwiping = false;
        }
    }

    void ProcessSwipe(Vector2 start, Vector2 end)
    {
        Vector2 swipeVector = end - start;

        if (invertControl)
            swipeVector = -swipeVector;

        if (swipeVector.magnitude < 0.2f) return;

        if (!gameStarted)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            gameStarted = true;
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(swipeVector * forceMultiplier, ForceMode2D.Impulse);

        if (jumpAudio != null)
            jumpAudio.Play();

        if (gameManager != null)
            gameManager.IncreaseScore(1);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && healthManager != null)
        {
            healthManager.TakeDamage();
        }
    }

    public void SetInvertedControl(bool isInverted)
    {
        invertControl = isInverted;
    }
}
