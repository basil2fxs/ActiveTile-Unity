using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;

public class AsyncUDPReceiverNormalLevel : MonoBehaviour
{
    public TileManagerNormalLevel tileManager; //change for tile manager in scene, this links it
    private ConcurrentQueue<Action> mainThreadActions = new ConcurrentQueue<Action>();
    
    [Serializable]
    public struct PortTileRange
    {
        public int port;
        public int startTile;
        public int endTile;
    }
    public PortTileRange[] portTileRanges;

    [SerializeField]
    public string serverIP = "192.168.0.201"; 
    [SerializeField]
    public int serverPort = 2317; 

    private UdpClient client;
    private bool isListening;

    void Start()
    {
        StartListening();
    }

    void OnDestroy()
    {
        StopListening();
    }

    private void StartListening()
    {
        isListening = true;
        Task.Run(() => ListenForMessagesAsync()); 
    }

    private void StopListening()
    {
        isListening = false; 

        if (client != null)
        {
            client.Close(); 
            client = null; 
        }
    }

    private async Task ListenForMessagesAsync()
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        client = new UdpClient(localEndPoint);

        try
        {
            while (isListening)
            {
                try
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    int senderPort = result.RemoteEndPoint.Port;

                    string hexString = BitConverter.ToString(result.Buffer).Replace("-", "");

                    ProcessReceivedData(hexString, senderPort);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in UDP receive loop: {e}");
        }
        finally
        {
            client?.Close();
        }
    }


    private void ProcessReceivedData(string hexData, int senderPort)
    {
        hexData = hexData.Substring(4);

        for (int i = 0; i < hexData.Length / 2; i++)
        {
            string tileStateHex = hexData.Substring(i * 2, 2).ToUpper();
            bool isActive = tileStateHex == "0A";
            int tileIndex = CalculateTileIndex(i, senderPort); //get tile number from 1-tilemax

            
            mainThreadActions.Enqueue(() =>
            {
                tileManager.SetTileState(tileIndex, isActive);
            });
        }
    }

    void Update()
    {
        while (mainThreadActions.TryDequeue(out Action action))
        {
            action.Invoke();
        }
    }   


    private int CalculateTileIndex(int order, int senderPort)
    {
        int portPlace = 0;

        if(senderPort == portTileRanges[0].port)
        {
            portPlace = 0;
        }
        if(senderPort == portTileRanges[1].port)
        {
            portPlace = 1;
        }
        if(senderPort == portTileRanges[2].port)
        {
            portPlace = 2;
        }

        order = order + portTileRanges[portPlace].startTile;

        return order; 
    }
}
