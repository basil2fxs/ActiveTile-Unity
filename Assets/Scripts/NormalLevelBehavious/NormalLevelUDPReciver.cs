using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Concurrent;

public class NormalLevelUDPReciver : MonoBehaviour
{
    public NormalLevelTileManager tileManager; // Assign in the Unity Editor
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
    public string serverIP = "192.168.0.201"; // The IP address to bind to
    [SerializeField]
    public int serverPort = 2317; // The port to listen on

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
        Task.Run(() => ListenForMessagesAsync()); // Use Task.Run to move execution to a background thread
    }

    private void StopListening()
    {
        isListening = false; // Signal the listening loop to stop

        if (client != null)
        {
            client.Close(); // Properly close and dispose of the UdpClient
            client = null; // Help ensure the object is released for garbage collection
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
                    // This catch block is specifically to handle the case when the UdpClient is disposed
                    // We simply break out of the loop in this case as it's a signal that we're done listening (e.g., due to scene change)
                    // Debug.LogWarning("UdpClient has been disposed. Exiting receive loop.");
                    break;
                }
            }
        }
        catch (Exception e)
        {
            // This is a more general exception catch block for other types of exceptions
            Debug.LogError($"Error in UDP receive loop: {e}");
        }
        finally
        {
            // Always ensure the client is closed in the finally block to handle all exit scenarios
            client?.Close();
        }
    }


    private void ProcessReceivedData(string hexData, int senderPort)
    {
        hexData = hexData.Substring(4); // Assuming your existing logic remains the same

        for (int i = 0; i < hexData.Length / 2; i++)
        {
            string tileStateHex = hexData.Substring(i * 2, 2).ToUpper();
            bool isActive = tileStateHex == "0A";
            int tileIndex = CalculateTileIndex(i, senderPort);

            // Queue the update action
            mainThreadActions.Enqueue(() =>
            {
                tileManager.SetTileState(tileIndex, isActive);
            });
        }
    }

    void Update()
    {
        // Execute all queued actions on the main thread
        while (mainThreadActions.TryDequeue(out Action action))
        {
            action.Invoke();
        }
    }   


    // Placeholder for a method to calculate tile index based on order and sender port
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

        // Implement the logic to calculate the tile index based on the order in the data string and sender port
        // This is highly dependent on how your tiles are organized and mapped to the data
        // For example, if each port corresponds to a different set of tiles:
        return order; // Placeholder calculation
    }
}
