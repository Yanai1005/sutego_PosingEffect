using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;
using Newtonsoft.Json.Linq; // JSON�f�[�^�̃p�[�X�Ɏg�p

public class HandReceiver : MonoBehaviour
{
    IPManager Ipmanager;
    public string serverIP;
    public int serverPort = 50008;
    TcpClient client;
    NetworkStream stream;
    Thread receiveThread;
    string receivedData;

    void Start()
    {
        Ipmanager = GetComponent<IPManager>();
        serverIP = Ipmanager.localIP;
        try
        {
            Debug.Log("HandReceiver: TCP�ڑ�����");
            client = new TcpClient(serverIP, serverPort);
            stream = client.GetStream();
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError($"TCP�ڑ��̊J�n���ɃG���[���������܂���: {e.Message}");
        }
    }

    void ReceiveData()
    {
        byte[] bytes = new byte[1024];
        while (client.Connected)
        {
            int length = stream.Read(bytes, 0, bytes.Length);
            if (length <= 0) continue;
            receivedData = Encoding.UTF8.GetString(bytes, 0, length);
            ProcessData(receivedData);
        }
    }

    void ProcessData(string jsonData)
    {
        // JSON�f�[�^���p�[�X
        JObject json = JObject.Parse(jsonData);
        // �����Ŏ�̃����h�}�[�N�f�[�^������
        // ��: json["hand_0_landmark_0"]["x"] ��X���W���擾
        Debug.Log("Received hand data: " + jsonData);
    }

    void OnApplicationQuit()
    {
        receiveThread.Abort();
        client.Close();
    }
}
