using UnityEngine;
using UnityEngine.UI;
using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using System;
using System.Threading.Tasks;

public class PurchaseHandler : MonoBehaviour
{
    public string productId;
    public GameObject loadingPanel;
    public HealthManager healthManager;

    private static PurchaseHandler _singletonInstance;
    private Payment _payment;
    private bool _connected = false;

    [Header("Poolakey App Key")]
    [TextArea]
    [SerializeField] private string appKey = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDmfmsx6yupAMazJrEhZlKyPn8mske2MVETvzPJrzMLXokOYyzuyKUSAX1+z/4mOD8e4O7hOU/ByGlEUp7DnYX+l7oGSgsan0rdVKmqVQ/0eNXOEAKGF9dpyHroSLg7YNmQjGuHB7kBcuPAwOnzoI7C0p8GlW0oWBNmtkIFuVMiMJd96mubriyCwxuHD3BPwJZZ3K6JryJ9LuasOq1qzkNCEYM3zsHVVR0/zukIZ5UCAwEAAQ==";

    private void Awake()
    {
        if (_singletonInstance != null && _singletonInstance != this)
        {
            //Destroy(gameObject);
            return;
        }

        _singletonInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void OnEnable()
    {
        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();

        await Init();
    }

    public async void Buy()
    {
        if (string.IsNullOrEmpty(productId)) return;

        loadingPanel?.SetActive(true);

        try
        {
            if (!_connected && !await Init()) return;

            var purchase = await _payment.Purchase(productId);
            if (purchase.status != Status.Success) return;

            var consume = await _payment.Consume(purchase.data.purchaseToken);
            if (consume.status != Status.Success) return;

            ApplyReward(productId);
        }
        finally
        {
            loadingPanel?.SetActive(false);
        }
    }

    private async Task<bool> Init()
    {
        if (_connected && _payment != null) return true;

        try
        {
            var security = SecurityCheck.Enable(appKey);
            _payment = new Payment(new PaymentConfiguration(security));
            var result = await _payment.Connect();
            _connected = result.status == Status.Success;
            return _connected;
        }
        catch { return false; }
    }

    private void ApplyReward(string id)
    {
        if (healthManager == null) return;

        switch (id)
        {
            case "Health_1": healthManager.AddLives(1); break;
            case "Health_2": healthManager.AddLives(5); break;
            case "Health_3": healthManager.AddLives(10); break;
            case "Health_4": healthManager.AddLives(15); break;
            case "Health_5": healthManager.AddLives(20); break;
            case "Health_6": healthManager.AddLives(30); break;
        }
    }

    private void OnApplicationQuit()
    {
        if (_payment != null && _connected)
        {
            _payment.Disconnect();
            _connected = false;
        }
    }
}
