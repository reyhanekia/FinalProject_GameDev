using UnityEngine;

public class BallSkinLoader : MonoBehaviour
{
    public Sprite[] skinSprites;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplySkin(PlayerPrefs.GetInt("SelectedSkin", 0));
    }

    public void ApplySkin(int skinID)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (skinID < 0 || skinID >= skinSprites.Length)
            skinID = 0;

        if (spriteRenderer != null && skinSprites[skinID] != null)
            spriteRenderer.sprite = skinSprites[skinID];
        else
            Debug.LogWarning("BallSkinLoader: Skin not applied correctly.");
    }
}
