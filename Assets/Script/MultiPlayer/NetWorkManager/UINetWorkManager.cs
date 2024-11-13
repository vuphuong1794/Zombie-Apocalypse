using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINetWorkManager : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;

    // Hàm riêng để khởi chạy Host
    public void StartHost()
    {
        // Load scene và chờ cho đến khi nó được tải xong
        _sceneController.LoadScene("Multiplayer Gamemode");
        StartCoroutine(WaitForSceneLoadAndStartHost());
    }

    // Hàm riêng để khởi chạy Client
    public void StartClient()
    {
        // Load scene và chờ cho đến khi nó được tải xong
        _sceneController.LoadScene("Multiplayer Gamemode");
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
}
