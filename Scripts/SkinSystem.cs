using UnityEngine;
using System.Collections.Generic;

public class SkinSystem : MonoBehaviour
{
    [Header("Skin Settings")]
    public List<SkinItem> availableSkins;
    public GameObject targetBall;
    public LevelProgression scoreManager;

    private const string SkinSelectKey = "SelectedSkin";
    private const string UnlockKeyPrefix = "SkinUnlocked_";

    void Start()
    {
        EnsureDefaultSkinUnlocked();

        int currentScore = scoreManager != null ? scoreManager.GetCurrentScore() : 0;

        foreach (var skin in availableSkins)
        {
            bool unlocked = PlayerPrefs.GetInt(UnlockKeyPrefix + skin.skinID, 0) == 1;

            if (!skin.unlockedByAd && currentScore >= skin.requiredScore && !unlocked)
            {
                unlocked = true;
                PlayerPrefs.SetInt(UnlockKeyPrefix + skin.skinID, 1);
            }

            skin.SetUnlocked(unlocked);
            skin.RefreshButton(ChooseSkin);
        }

        if (!PlayerPrefs.HasKey(SkinSelectKey))
        {
            PlayerPrefs.SetInt(SkinSelectKey, 0);
            PlayerPrefs.Save();
        }

        ChooseSkin(PlayerPrefs.GetInt(SkinSelectKey, 0));
    }

    private void EnsureDefaultSkinUnlocked()
    {
        if (PlayerPrefs.GetInt(UnlockKeyPrefix + "0", 0) == 0)
        {
            PlayerPrefs.SetInt(UnlockKeyPrefix + "0", 1);
            PlayerPrefs.Save();
        }
    }

    public void ChooseSkin(int skinID)
    {
        SkinItem selectedSkin = availableSkins.Find(s => s.skinID == skinID);

        if (selectedSkin != null && selectedSkin.isUnlocked)
        {
            PlayerPrefs.SetInt(SkinSelectKey, skinID);
            PlayerPrefs.Save();

            BallSkinLoader loader = targetBall.GetComponent<BallSkinLoader>();
            if (loader != null)
            {
                loader.ApplySkin(skinID);
            }


            foreach (var skin in availableSkins)
            {
                skin.UpdateLockText();
                skin.RefreshButton(ChooseSkin);
            }
        }
        else
        {
            Debug.LogWarning($"Skin ID {skinID} is locked or doesn't exist.");
        }
    }

    public void UnlockSkinThroughAd(int skinID)
    {
        SkinItem adSkin = availableSkins.Find(s => s.skinID == skinID);
        if (adSkin != null && !adSkin.isUnlocked)
        {
            adSkin.SetUnlocked(true);
            PlayerPrefs.SetInt(UnlockKeyPrefix + skinID, 1);
            PlayerPrefs.Save();
        }
    }
}
