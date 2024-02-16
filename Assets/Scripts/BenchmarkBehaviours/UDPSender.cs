using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UDPSender : MonoBehaviour
{
    public TileManager tileManager; // Assign in the Inspector
    public AsyncUDPReceiver asyncUDPReceiver;//referencing the UDP reciever

    public string sourceIP = "192.168.0.201";
    public string destIP = "192.168.0.7";
    public string black = "000000";
    public string blue = "0000FE";
    public int sourcePort = 2317; // Source port to bind to

    private UdpClient udpClient;

    // Timing control variables
    private float timeSinceLastSend = 0f;
    public float desiredFrameRate = 30f; // Desired send rate in frames per second
    private float sendInterval; // Calculated time interval between sends

    void Start()
    {
        // Initialize UdpClient to bind to the specific source port
        udpClient = new UdpClient(sourcePort); // Bind to the source port directly

        // No need to use Connect method for UdpClient when sending to multiple destinations
        // Calculate the interval based on the desired frame rate
        sendInterval = 1.0f / desiredFrameRate;
    }

    void Update()
    {
        // Accumulate the time elapsed since the last frame
        timeSinceLastSend += Time.deltaTime;

        // Check if the accumulated time has reached the send interval
        if (timeSinceLastSend >= sendInterval)
        {
            // Send tile states
            SendTileStates();

            // Reset the timer
            timeSinceLastSend = 0f;
        }
    }

    public void SendTileStates()
    {
        SendTilesStateForPort(asyncUDPReceiver.portTileRanges[0].startTile, asyncUDPReceiver.portTileRanges[0].endTile, asyncUDPReceiver.portTileRanges[0].port);
        SendTilesStateForPort(asyncUDPReceiver.portTileRanges[1].startTile, asyncUDPReceiver.portTileRanges[1].endTile, asyncUDPReceiver.portTileRanges[1].port);
        SendTilesStateForPort(asyncUDPReceiver.portTileRanges[2].startTile, asyncUDPReceiver.portTileRanges[2].endTile, asyncUDPReceiver.portTileRanges[2].port);
    }

    private void SendTilesStateForPort(int startTile, int endTile, int destPort)
    {
        string hexData = "FFFF"; // Starting with the header

        for (int tileIndex = startTile; tileIndex <= endTile; tileIndex++)
        {
            bool isActive = tileManager.GetTileState(tileIndex);
            hexData += isActive ? blue : black; // Simplified data representation
        }

        byte[] byteData = ConvertHexStringToByteArray(hexData);
        try
        {
            // Create an endpoint for the destination address and port
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(destIP), destPort);
            // Send the data
            udpClient.Send(byteData, byteData.Length, endPoint);
            //Debug.Log($"Sent tile states to {destIP}:{destPort}: {BitConverter.ToString(byteData).Replace("-", "")}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to send UDP data: {ex.Message}");
        }
    }

    byte[] ConvertHexStringToByteArray(string hexString)
    {
        int numberChars = hexString.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return bytes;
    }

    void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
