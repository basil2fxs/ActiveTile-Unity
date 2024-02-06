using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncUDPReceiver : MonoBehaviour
{
    public TileManager tileManager;

    [Serializable]
    public struct PortTileRange
    {
        public int port;
        public int startTile;
        public int endTile;
    }

    public PortTileRange[] portTileRanges;

    private void Start()
    {
        foreach (var range in portTileRanges)
        {
            ListenForMessagesAsync(range.port, range.startTile, range.endTile);
        }
    }

    private async void ListenForMessagesAsync(int port, int startTile, int endTile)
    {
        using (var client = new UdpClient(port))
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            try
            {
                while (true)
                {
                    var result = await client.ReceiveAsync();
                    string hexString = BitConverter.ToString(result.Buffer).Replace("-", "");
                    if (hexString.StartsWith("FC"))
                    {
                        ProcessReceivedData(hexString, startTile, endTile);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error receiving UDP data on port {port}: {e.Message}");
            }
        }
    }

    private void ProcessReceivedData(string hexData, int startTile, int endTile)
    {
        hexData = hexData.Substring(2); // Skip "FC"
        int lengthIndicator = int.Parse(hexData.Substring(0, 2), NumberStyles.HexNumber) * 2; // Number of characters to represent tile states
        hexData = hexData.Substring(2); // Move past the length indicator

        string tileStates = hexData.Substring(0, lengthIndicator);

        // Iterate through the tile states to identify and log stepped-on tiles
        for (int i = 0; i < tileStates.Length / 2; i++)
        {
            string tileStateHex = tileStates.Substring(i * 2, 2);
            if (tileStateHex == "0A") // If the tile is being stepped on
            {
                int tileIndex = startTile + i;
                Debug.Log($"Tile {tileIndex} is being stepped on.");
            }
        }

        // Update tiles within the specified range
        tileManager.UpdateTilesInRange(tileStates, startTile, endTile);
    }
}
