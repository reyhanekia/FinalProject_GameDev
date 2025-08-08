using UnityEngine;
using TapsellPlusSDK;
using System;
using UnityEngine.SceneManagement;

public class VideoAd : MonoBehaviour
{
    [Header("Tapsell Zone ID")]
    public string zoneId = "686016892973e423038e72e0";

    [Header("Reward Settings")]
    public int rewardSkinID = -1;
    public SkinSystem skinSystem;
    

    private string responseId;

    void Start()
    {
        InitializeTapsell();
    }

    void InitializeTapsell()
    {
        TapsellPlus.Initialize(
            "nspppqflaphasigknkcprffijadeceqgpsecrqmaergonohqeqimregpgdtdqmhjtanini",
            adNetwork =>
            {
                Debug.Log("Tapsell initialized: " + adNetwork);
            },
            error =>
            {
                Debug.LogError("Tapsell init error: " + error);
            });
    }
    public void ShowAdFromButton()
    {
        Debug.Log("ShowAdFromButton called");
        ShowAd(() =>
        {
            Debug.Log("Ad watched successfully from UI Button.");

            if (skinSystem != null && rewardSkinID >= 0)
            {
                skinSystem.UnlockSkinThroughAd(rewardSkinID);
                SceneManager.LoadScene("Extras");
                Debug.Log("Unlocked skin ID: " + rewardSkinID);
            }
        });
    }

    public void ShowAd(Action onRewardCallback)
    {
        TapsellPlus.RequestRewardedVideoAd(zoneId,
            adModel =>
            {
                responseId = adModel.responseId;

                TapsellPlus.ShowRewardedVideoAd(responseId,
                    onOpen => { },
                    onReward =>
                    {
                        Debug.Log("Reward received");
                        onRewardCallback?.Invoke();
                    },
                    onClose => { },
                    onError =>
                    {
                        Debug.LogError("Ad show error");
                    });
            },
            error =>
            {
                Debug.LogError("Ad request error: " + error);
            });
    }
}
