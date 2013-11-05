/* 
 * Copyright (C) 2013 Keijiro Takahashi
 * 
 * Permission is hereby granted, free of charge, to any 
 * person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, subject 
 * to the following conditions: The above copyright notice and 
 * this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Special Thanks:
 * The original software design and architecture of fuZe vjkit 
 * is inspired by Visualizer Studio, created by Altered Reality 
 * Entertainment LLC.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;

public class MidiBridge : MonoBehaviour
{
    #region Configurations
    // Port number used to communicate with Bridge.
    const int portNumber = 52364;
    #endregion

    #region Public interface
    public Queue<MidiMessage> incomingMessageQueue;

    public void Send (int status, int data1, int data2 = 0xff)
    {
        if (tcpClient != null && tcpClient.Connected) {
            smallBuffer [0] = (data2 == 0xff) ? (byte)2 : (byte)3;
            smallBuffer [1] = (byte)status;
            smallBuffer [2] = (byte)data1;
            smallBuffer [3] = (byte)data2;
            try {
                tcpClient.GetStream ().Write (smallBuffer, 0, 4);
            } catch (System.IO.IOException exception) {
                Debug.Log (exception);
            }
        }
    }
    #endregion

    #region Monobehaviour functions
    void Awake ()
    {
        incomingMessageQueue = new Queue<MidiMessage> ();
        smallBuffer = new byte[4];
    }

    void Start ()
    {
        StartCoroutine (ConnectionCoroutine ());
        StartCoroutine (ReceiverCoroutine ());
    }
    #endregion

    #region TCP connection
    // TCP connection.
    TcpClient tcpClient;
    bool isConnecting;

    // A small buffer used for sending messages.
    byte[] smallBuffer;

    // Coroutine for managing the connection.
    IEnumerator ConnectionCoroutine ()
    {
        // "Active Sense" message for heartbeating.
        var heartbeat = new byte[4] {2, 0xff, 0xfe, 0};

        while (true) {
            // Try to open the connection.
            for (var retryCount = 0;; retryCount++) {
                // Start to connect.
                var tempClient = new TcpClient ();
                tempClient.BeginConnect (IPAddress.Loopback, portNumber, ConnectCallback, null);
                // Wait for callback.
                isConnecting = true;
                while (isConnecting) {
                    yield return null;
                }
                // Break if the connection is established.
                if (tempClient.Connected) {
                    tcpClient = tempClient;
                    break;
                }
                // Dispose the connection.
                tempClient.Close ();
                tempClient = null;
                // Show warning and wait a second.
                if (retryCount % 3 == 0) {
                    Debug.LogWarning ("Failed to connect to MIDI Bridge.");
                }
                yield return new WaitForSeconds (1.0f);
            }

            // Watch the connection.
            while (tcpClient.Connected) {
                yield return new WaitForSeconds (1.0f);
                // Send a heartbeat and break if it failed.
                try {
                    tcpClient.GetStream ().Write (heartbeat, 0, heartbeat.Length);
                } catch (System.IO.IOException exception) {
                    Debug.Log (exception);
                }
            }

            // Show warning.
            Debug.LogWarning ("Disconnected from MIDI Bridge.");

            // Close the connection and retry.
            tcpClient.Close ();
            tcpClient = null;
        }
    }

    void ConnectCallback (System.IAsyncResult result)
    {
        isConnecting = false;
    }

    // Coroutine for receiving messages.
    IEnumerator ReceiverCoroutine ()
    {
        byte[] buffer = new byte[2048];

        while (true) {
            // Do nothing if the connection is not ready.
            if (tcpClient == null || !tcpClient.Connected || tcpClient.Available < 4) {
                yield return null;
                continue;
            }

            // Receive data from the socket.
            var available = Mathf.Min ((tcpClient.Available / 4) * 4, buffer.Length);
            var bufferFilled = tcpClient.GetStream ().Read (buffer, 0, available);

            for (var offset = 0; offset < bufferFilled; offset += 4) {
                var length = buffer [offset];
                if (length == 2) {
                    incomingMessageQueue.Enqueue (new MidiMessage (buffer [offset + 1], buffer [offset + 2]));
                } else if (length == 3) {
                    incomingMessageQueue.Enqueue (new MidiMessage (buffer [offset + 1], buffer [offset + 2], buffer [offset + 3]));
                }
            }

            yield return null;
        }
    }
    #endregion

    #region Singleton class interface
    static MidiBridge _instance;
    
    public static MidiBridge instance {
        get {
            if (_instance == null) {
                var go = new GameObject ();
                _instance = go.AddComponent<MidiBridge> ();
                DontDestroyOnLoad (go);
                go.hideFlags = HideFlags.HideInHierarchy;
            }
            return _instance;
        }
    }
    #endregion
}
