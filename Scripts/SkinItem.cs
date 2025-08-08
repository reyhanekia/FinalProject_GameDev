using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SkinItem : MonoBehaviour
{
    [Header("Skin Info")]
    public int skinID;
    public int requiredScore = 0;
    public bool unlockedByAd = false;
    public bool isUnlocked { get; private set; }

    [Header("UI References")]
    public Button selectButton;
    public GameObject lockPanel;
    public Button adButton;
    public TextMeshProUGUI lockText;

    [Header("Dependencies")]
    public VideoAd VideoAdManager;
    public LevelProgression gameManager;
    public Sprite skinSprite;

    private void Start()
    {
        isUnlocked = PlayerPrefs.GetInt("SkinUnlocked_" + skinID, 0) == 1;

        if (!isUnlocked && !unlockedByAd && gameManager != null)
        {
            int currentScore = gameManager.GetCurrentScore();
            if (currentScore >= requiredScore)
            {
                UnlockSkin();
                SceneManager.LoadScene("Extras");
            }
        }

        if (unlockedByAd && !isUnlocked && adButton != null)
        {
            adButton.gameObject.SetActive(true);
            adButton.onClick.RemoveAllListeners();
            adButton.onClick.AddListener(() =>
            {
                if (VideoAdManager != null)
                {
                    VideoAdManager.ShowAd(() => {
                        UnlockSkin();
                    });
                }
            });
        }
        else if (adButton != null)
        {
            adButton.gameObject.SetActive(false);
        }

        ApplyUIState();
    }


    private void UnlockSkin()
    {
        SetUnlocked(true);
        
    }

    public void SetUnlocked(bool value)
    {
        isUnlocked = value;
        PlayerPrefs.SetInt("SkinUnlocked_" + skinID, value ? 1 : 0);
        PlayerPrefs.Save();
        ApplyUIState();
    }

    private void ApplyUIState()
    {
        if (lockPanel != null)
            lockPanel.SetActive(!isUnlocked);

        if (selectButton != null)
            selectButton.interactable = isUnlocked;

        UpdateLockText();
    }

    public void UpdateLockText()
    {
        if (lockText == null) return;

        if (!isUnlocked)
        {
        }
        else
        {
            int selectedID = PlayerPrefs.GetInt("SelectedSkin", -1);
            lockText.text = (selectedID == skinID) ? "انتخاب شده" : "انتخاب";
        }
    }

    public void RefreshButton(System.Action<int> onSelectCallback)
    {
        if (selectButton == null) return;

        selectButton.onClick.RemoveAllListeners();

        if (isUnlocked)
        {
            selectButton.interactable = true;
            selectButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("SelectedSkin", skinID);
                PlayerPrefs.Save();
                onSelectCallback?.Invoke(skinID);
                UpdateLockText();
            });
        }
        else
        {
            selectButton.interactable = false;
        }

        UpdateLockText();
    }
}
