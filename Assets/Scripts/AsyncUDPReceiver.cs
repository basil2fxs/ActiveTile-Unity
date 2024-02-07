using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncUDPReceiver : MonoBehaviour
{
    public TileManager tileManager; // Assign in the Unity Editor
    
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

    void Start()
    {
        //Debug.Log($"Starting UDP listener on {serverIP}:{serverPort}");
        ListenForMessagesAsync();
    }

    private async void ListenForMessagesAsync()
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        using (UdpClient client = new UdpClient(localEndPoint))
        {
            //Debug.Log($"Listening for UDP datagrams on {serverIP}:{serverPort}...");
            try
            {
                while (true)
                {
                    UdpReceiveResult result = await client.ReceiveAsync();
                    int senderPort = result.RemoteEndPoint.Port;

                    string hexString = BitConverter.ToString(result.Buffer).Replace("-", "");
                    //Debug.Log($"Received UDP data from {senderAddress}:{senderPort}: {hexString}");

                    // Process the received data
                    ProcessReceivedData(hexString, senderPort);
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
            tileManager.SetTileState(tileIndex, isActive);
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
