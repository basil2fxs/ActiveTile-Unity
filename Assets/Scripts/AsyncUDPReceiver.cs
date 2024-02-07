using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncUDPReceiver : MonoBehaviour
{
    public TileManager tileManager; // Assign in the Unity Editor

    [SerializeField]
    private string serverIP = "192.168.0.201"; // The IP address to bind to
    [SerializeField]
    private int serverPort = 2317; // The port to listen on

    void Start()
    {
        Debug.Log($"Starting UDP listener on {serverIP}:{serverPort}");
        ListenForMessagesAsync();
    }

    private async void ListenForMessagesAsync()
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        using (UdpClient client = new UdpClient(localEndPoint))
        {
            Debug.Log($"Listening for UDP datagrams on {serverIP}:{serverPort}...");
            try
            {
                while (true)
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    string senderAddress = result.RemoteEndPoint.Address.ToString();
                    int senderPort = result.RemoteEndPoint.Port;

                    // Check if the sender's port is one of the expected ports
                    if (senderPort == 21 || senderPort == 22 || senderPort == 23)
                    {
                        string hexString = BitConverter.ToString(result.Buffer).Replace("-", "");
                        Debug.Log($"Received UDP data from {senderAddress}:{senderPort}: {hexString}");

                        // Process the received data
                        ProcessReceivedData(hexString, senderPort);
                    }
                    else
                    {
                        Debug.LogWarning($"Received data from unexpected port {senderPort}. Ignoring.");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in UDP receive loop: {e}");
            }
        }
    }

    private void ProcessReceivedData(string hexData, int senderPort)
    {
        // Assuming the hexData starts with "FC", followed by tile data
        if (!hexData.StartsWith("FC"))
        {
            Debug.LogWarning($"Data does not start with 'FC'. Data: {hexData}");
            return;
        }

        hexData = hexData.Substring(4); // Skip "FC" and the length indicator, assuming 2 characters for length.

        // Example of processing the data: Log each tile's state
        for (int i = 0; i < hexData.Length / 2; i++)
        {
            string tileStateHex = hexData.Substring(i * 2, 2).ToUpper();
            bool isActive = tileStateHex == "0A";

            // Update the specific tile based on its index and active state
            // Assuming a method in TileManager that updates tiles based on index and active state:
            // tileIndex calculation needs to be adjusted based on your game's tile indexing logic
            int tileIndex = CalculateTileIndex(i, senderPort);
            tileManager.UpdateTileState(tileIndex, isActive);
        }
    }

    // Placeholder for a method to calculate tile index based on order and sender port
    private int CalculateTileIndex(int order, int senderPort)
    {
        // Implement the logic to calculate the tile index based on the order in the data string and sender port
        // This is highly dependent on how your tiles are organized and mapped to the data
        // For example, if each port corresponds to a different set of tiles:
        return order; // Placeholder calculation
    }
}
