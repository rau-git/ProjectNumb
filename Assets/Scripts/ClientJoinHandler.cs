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
        _transportManager.SetServerBindAddress(_serverField.text, 0);
        _transportManager.SetClientAddress(_clientField.text);
    }
}
