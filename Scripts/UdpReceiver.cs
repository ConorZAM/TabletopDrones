using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;
using System;

public class UdpReceiver : MonoBehaviour
{
    // The database class, used to store the messages and manage the drones in the scene
    public DroneDatabase database;

    // Using text inputs for the port number, will accept messages from any ip address
    public TMP_Text portText;
    public int port;

    // This IP address is used to open the port, we send a message to this IP to try and open
    // the port
    public string ip = "192.168.1.160";


    // UDP Client settings, need a timeout here because we're using Receive instead of Begin/End Receive
    UdpClient listener;
    byte[] receivedBytes;
    public int receiveTimeoutDurationMs = 2000;
    IPEndPoint groupEP;

    // Thread
    Thread listenThread;
    public bool running;

    private void Start()
    {
        // Set the settings menu port text to match the port we start with
        portText.text = port.ToString();

        // Set up the connection details
        groupEP = new IPEndPoint(IPAddress.Any, port);
        listener = new UdpClient(groupEP);

        // Start the listening thread
        StartListening();
    }

    // This function is called when the class is disabled or destroyed
    // Usually when the application is closed - just make sure to end the thread
    private void OnDisable()
    {
        running = false;
        listener.Close();
    }

    private int newPort;
    public void StartListening()
    {
        // Get the port number from the input field in the settings menu
        newPort = int.Parse(portText.text);

        //HoloDebugLogger.Instance.LogMessage("Starting listen thread");

        // If the thread is already running, stop listening first
        if (listenThread != null) { StopThread(); }

        // Receive on a separate thread so Unity doesn't freeze waiting for data
        ThreadStart threadStarter = new ThreadStart(StartReceiver);
        listenThread = new Thread(threadStarter);
        listenThread.Start();
    }


    

    private void StopThread()
    {
        //HoloDebugLogger.Instance.LogMessage("Stopping listen thread");

        // Break the listen loop
        running = false;

        // Wait for the thread to finish
        listenThread.Join();

        // The listener is closed at the end of the listen thread
    }

    // Thread version
    void StartReceiver()
    {
        //HoloDebugLogger.Instance.LogMessage("Starting receiver");

        // Open the UDP socket
        // Sending data on the port might actually open the port for use so that we can
        // receive data as well. It seems to have worked, so I would keep this in
        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ip).MapToIPv4(), port);
        byte[] data = Encoding.UTF8.GetBytes("hello");
        listener.Send(data, data.Length, RemoteIpEndPoint);

        // Listen on the port from any IP address

        // Need to make sure we're not trying to start listening on the same port
        // This might not be necessary, we can probably just make sure to have a
        // call to the stop listening function here instead
        if (newPort != port)
        {
            groupEP = new IPEndPoint(IPAddress.Any, port);
            listener = new UdpClient(groupEP);
        }

        // Set a time out so we can exit the thread even if there is no data coming in
        // This is required as we're not using async receive, so listener.Receive() will block the
        // thread indefinitely until data is received.
        listener.Client.ReceiveTimeout = 5000;  // in miliseconds

        // Start the connection, running can be set to false elsewhere to terminate this thread
        running = true;
        while (running)
        {
            GetPacket();
        }

        // Close the listener when running is set to false
        listener.Close();
    }

    void GetPacket()
    {
        //HoloDebugLogger.Instance.LogMessage("Running Connection...");

        // Need the try catch here to handle the timeout on Receive
        try
        {
            receivedBytes = listener.Receive(ref groupEP);
            database.ParseMessage(receivedBytes);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            //HoloDebugLogger.Instance.LogMessage(e.ToString());
        }

    }
}
