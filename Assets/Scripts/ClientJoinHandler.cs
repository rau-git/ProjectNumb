using TMPro;
using UnityEngine;

public class ClientJoinHandler : MonoBehaviour
{
    [SerializeField]
    private FishyFacepunch.FishyFacepunch _transportManager;
    
    [SerializeField]
    private TextMeshProUGUI _serverField;
    [SerializeField]
    private TextMeshProUGUI _clientField;

    private void Update()
    {
        
        _transportManager.SetServerBindAddress(_serverField.text.ToString(), 0);
        _transportManager.SetClientAddress(_clientField.text.ToString());
        
        /*
        _transportManager.SetServerBindAddress(Steamworks.SteamClient.SteamId.ToString(), 0);
        _transportManager.SetClientAddress(Steamworks.SteamClient.SteamId.ToString());
        */
    }
}
