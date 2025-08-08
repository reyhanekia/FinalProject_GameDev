using UnityEngine;
using TapsellPlusSDK;
using TapsellPlusSDK.models;

public class BannerAd : MonoBehaviour
{
    public string bannerZoneId = "686020622973e423038e72ea";

    void Start()
    {
        TapsellPlus.Initialize(
            "nspppqflaphasigknkcprffijadeceqgpsecrqmaergonohqeqimregpgdtdqmhjtanini",
            adNetwork => Debug.Log("Tapsell initialized: " + adNetwork),
            error => Debug.LogError("Init error: " + error)
        );

        TapsellPlus.ShowStandardBannerAd(
            bannerZoneId,
            Gravity.Bottom,
            Gravity.Center,
            _ => { },
            _ => { }
        );


    }
}
