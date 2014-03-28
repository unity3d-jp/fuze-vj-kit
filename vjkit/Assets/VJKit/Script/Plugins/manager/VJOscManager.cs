using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class VJOscManager : VJAbstractManager {
	public int listenPort = 8000;
	
	private UdpClient udpClient;
	private Osc.Parser osc = new Osc.Parser();
	
	void OnEnable()
	{
		udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, listenPort));
	}
	
	void OnDisable()
	{
		udpClient.Close();
	}
	
	void Update()
	{
		while (udpClient.Available > 0) {
			IPEndPoint remoteEP = null;
			osc.FeedData(udpClient.Receive(ref remoteEP));
		}
		
		while (osc.MessageCount > 0) {
			Osc.Message message = osc.PopMessage();
			
			if (message.data.Length == 1 && message.data[0].GetType() == typeof(float)) {
				foreach (VJOscDataSource oscDataSource in GetComponents<VJOscDataSource>()) {
					if (oscDataSource.address == message.path) {
						oscDataSource.value = (float)message.data[0];
					}
				}
			}
		}
	}
}
