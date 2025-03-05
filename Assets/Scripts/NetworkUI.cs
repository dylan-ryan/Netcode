using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public InputField ipAddressInput;

    void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    void StartClient()
    {
        string ipAddress = ipAddressInput.text;
        if (!string.IsNullOrEmpty(ipAddress))
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, (ushort)7777);
            NetworkManager.Singleton.StartClient();
        }
        else
        {
            Debug.LogError("Invalid IP");
        }
    }
}
