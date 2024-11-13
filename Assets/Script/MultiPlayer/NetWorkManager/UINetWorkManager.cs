using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UINetWorkManager : MonoBehaviour
{
    //void OnGUI()
    //{
    //    // Tăng kích thước của khu vực GUI
    //    GUILayout.BeginArea(new Rect(50, 50, 500, 500));

    //    if (!NetworkManager.Singleton.IsClient && NetworkManager.Singleton.IsServer)
    //    {
    //        StartButtons();
    //    }
    //    else
    //    {
    //        StatusLabels();
    //    }

    //    GUILayout.EndArea();
    //}

    //static void StartButtons()
    //{
    //    // Thiết lập style cho nút để tăng kích thước chữ và kích thước tổng thể
    //    GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
    //    buttonStyle.fontSize = 24; // Tăng kích thước chữ
    //    buttonStyle.fixedHeight = 60; // Chiều cao nút
    //    buttonStyle.fixedWidth = 200; // Chiều rộng nút

    //    if (GUILayout.Button("Host", buttonStyle)) NetworkManager.Singleton.StartHost();
    //    if (GUILayout.Button("Client", buttonStyle)) NetworkManager.Singleton.StartClient();
    //    if (GUILayout.Button("Server", buttonStyle)) NetworkManager.Singleton.StartServer();
    //}

    //static void StatusLabels()
    //{
    //    // Thiết lập style cho nhãn để tăng kích thước chữ
    //    GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
    //    labelStyle.fontSize = 24; // Tăng kích thước chữ

    //    var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
    //    GUILayout.Label("Transport: " + NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name, labelStyle);
    //    GUILayout.Label("Mode: " + mode, labelStyle);
    //}

    // Hàm riêng để khởi chạy Host
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    // Hàm riêng để khởi chạy Client
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    // Hàm riêng để khởi chạy Server
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}
