using System.Collections;
using System.Net.Sockets;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINetWorkManager : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;

    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;

    void Start()
    {
        ipAddress = "0.0.0.0";
        SetIpAddress(); // Set the Ip to the above address
        //InvokeRepeating("assignPlayerController", 0.1f, 0.1f);
    }

    // Hàm riêng để khởi chạy Host
    public void StartHost()
    {
        // Load scene và chờ cho đến khi nó được tải xong
        _sceneController.LoadScene("Multiplayer Gamemode");
        GetLocalIPAddress();
        StartCoroutine(WaitForSceneLoadAndStartHost());
    }

    // Hàm riêng để khởi chạy Client
    public void StartClient()
    {
        // Load scene và chờ cho đến khi nó được tải xong
        _sceneController.LoadScene("Multiplayer Gamemode");
        ipAddress = ip.text;
        SetIpAddress();
        StartCoroutine(WaitForSceneLoadAndStartClient());
    }

    // Coroutine chờ scene load và bắt đầu Host
    private IEnumerator WaitForSceneLoadAndStartHost()
    {
        // Đăng ký sự kiện để xử lý khi scene mới được tải
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync("Multiplayer Gamemode");
        sceneLoadOperation.allowSceneActivation = false; // Tạm dừng chuyển cảnh cho đến khi ta chuẩn bị xong

        // Hiển thị hiệu ứng chuyển cảnh
        while (!sceneLoadOperation.isDone)
        {
            // Đợi cho đến khi scene tải xong (bắt đầu từ 90%)
            if (sceneLoadOperation.progress >= 0.9f)
            {
                // Có thể thêm code ở đây để hiển thị loading spinner hoặc một thông báo nào đó
                sceneLoadOperation.allowSceneActivation = true; // Kích hoạt chuyển cảnh
            }

            yield return null;
        }

        // Khi scene đã được tải hoàn toàn, bắt đầu Host
        NetworkManager.Singleton.StartHost();
    }

    // Coroutine chờ scene load và bắt đầu Client
    private IEnumerator WaitForSceneLoadAndStartClient()
    {
        // Đăng ký sự kiện để xử lý khi scene mới được tải
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync("Multiplayer Gamemode");
        sceneLoadOperation.allowSceneActivation = false; // Tạm dừng chuyển cảnh cho đến khi ta chuẩn bị xong

        // Hiển thị hiệu ứng chuyển cảnh
        while (!sceneLoadOperation.isDone)
        {
            // Đợi cho đến khi scene tải xong (bắt đầu từ 90%)
            if (sceneLoadOperation.progress >= 0.9f)
            {
                // Có thể thêm code ở đây để hiển thị loading spinner hoặc một thông báo nào đó
                sceneLoadOperation.allowSceneActivation = true; // Kích hoạt chuyển cảnh
            }

            yield return null;
        }

        // Khi scene đã được tải hoàn toàn, bắt đầu Client
        NetworkManager.Singleton.StartClient();
    }


    // ONLY FOR HOST SIDE 
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    // ONLY FOR CLIENT SIDE
    public void SetIpAddress()
    {
        if (transport != null)
        {
            transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = ipAddress;
            Debug.Log("IP Address set to: " + ipAddress);  // Kiểm tra xem IP có được gán đúng không
            ipAddressText.text = ip.ToString();
        }
        else
        {
            Debug.LogError("UnityTransport is not found!");
        }
    }

}
