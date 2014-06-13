/* 
 * fuZe vjkit
 * 
 * Copyright (C) 2013 Unity Technologies Japan, G.K.
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
using System;
using System.Text;
using System.Net.Sockets;

public class VJOSCClient : MonoBehaviour {
	public string serverHostname = "127.0.0.1";
	public int serverPort = 8000;

	private UdpClient udpClient;

	void Start() {
		udpClient = new UdpClient ();
	}
	
	// Currently, we implement only the basic data types
	public void SendSimpleMessage(string path, int i) {
		byte[] pathByte = Encoding.UTF8.GetBytes(path);

		byte[] datagram = new byte[(pathByte.Length + 1 + 3) / 4 * 4 + 4 + 4];
		
		Buffer.BlockCopy(pathByte, 0, datagram, 0, pathByte.Length);
		
		int p = (pathByte.Length + 1 + 3) / 4 * 4;

		datagram[p++] = (byte)',';
		datagram[p++] = (byte)'i';
		p += 2;

		datagram[p++] = (byte)((uint)i >> 24);
		datagram[p++] = (byte)((uint)i >> 16);
		datagram[p++] = (byte)((uint)i >> 8);
		datagram[p++] = (byte)((uint)i);

		SendMessageBytes (datagram);
	}

	public void SendSimpleMessage(string path, float f) {
		byte[] pathByte = Encoding.UTF8.GetBytes(path);

		byte[] datagram = new byte[(pathByte.Length + 1 + 3) / 4 * 4 + 4 + 4];
		
		Buffer.BlockCopy(pathByte, 0, datagram, 0, pathByte.Length);
		
		int p = (pathByte.Length + 1 + 3) / 4 * 4;

		datagram[p++] = (byte)',';
		datagram[p++] = (byte)'f';
		p += 2;

		byte[] bytes = BitConverter.GetBytes(f);
	    if (BitConverter.IsLittleEndian) {
			datagram[p++] = bytes[3];
			datagram[p++] = bytes[2];
			datagram[p++] = bytes[1];
			datagram[p++] = bytes[0];
		} else {
			datagram[p++] = bytes[0];
			datagram[p++] = bytes[1];
			datagram[p++] = bytes[2];
			datagram[p++] = bytes[3];
		}

		SendMessageBytes (datagram);
	}

	public void SendSimpleMessage(string path, string s) {
		byte[] pathByte = Encoding.UTF8.GetBytes(path);
		byte[] stringByte = Encoding.UTF8.GetBytes(s);
		
		byte[] datagram = new byte[(pathByte.Length + 1 + 3) / 4 * 4 + 4 + (stringByte.Length + 1 + 3) / 4 * 4];
		
		Buffer.BlockCopy(pathByte, 0, datagram, 0, pathByte.Length);
		
		int p = (pathByte.Length + 1 + 3) / 4 * 4;
		
		datagram[p++] = (byte)',';
		datagram[p++] = (byte)'s';
		p += 2;

		Buffer.BlockCopy(stringByte, 0, datagram, p, stringByte.Length);
		
		SendMessageBytes (datagram);
	}

	public void SendSimpleMessage(string path, byte[] b) {
		byte[] pathByte = Encoding.UTF8.GetBytes(path);

		byte[] datagram = new byte[(pathByte.Length + 1 + 3) / 4 * 4 + 4 + 4 + (b.Length + 3) / 4 * 4];
		
		Buffer.BlockCopy(pathByte, 0, datagram, 0, pathByte.Length);
		
		int p = (pathByte.Length + 1 + 3) / 4 * 4;

		datagram[p++] = (byte)',';
		datagram[p++] = (byte)'b';
		p += 2;
		
		datagram[p++] = (byte)(b.Length >> 24);
		datagram[p++] = (byte)(b.Length >> 16);
		datagram[p++] = (byte)(b.Length >> 8);
		datagram[p++] = (byte)(b.Length);
		
		Buffer.BlockCopy(b, 0, datagram, p, b.Length);
		
		SendMessageBytes (datagram);
	}

	private void SendMessageBytes(byte[] bytes) {
		if (enabled) {
			udpClient.Send (bytes, bytes.Length, serverHostname, serverPort);
		}
	}
}
