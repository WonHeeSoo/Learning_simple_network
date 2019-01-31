using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class SocketServer : MonoBehaviour
{
	Thread mSocketThread;
	volatile bool mKeepReading = false;

	Socket listener;
	Socket handler;
	

	private void Start()
	{
		// if true, player is running the application when is in the background.
		Application.runInBackground = true;
		startServer();
	}

	void startServer()
	{
		mSocketThread = new System.Threading.Thread(networkCode);
		mSocketThread.IsBackground = true;
		mSocketThread.Start();
	}

	private string getIPAddress()
	{
		IPHostEntry host;
		string localIP = "";
		host = Dns.GetHostEntry(Dns.GetHostName());
		foreach(IPAddress ip in host.AddressList)
		{
			if(ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
			}
		}
		return localIP;
	}

	void networkCode()
	{
		byte[] bytes = null;

		// host running the application;
		Debug.Log("IP " + getIPAddress().ToString());
		IPAddress[] ipArray = Dns.GetHostAddresses(getIPAddress());
		IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 1755);

		// Create a TCP/IP socket.
		listener = new Socket(ipArray[0].AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		try
		{
			listener.Bind(localEndPoint);
			listener.Listen(10);

			// Start listening for connections.
			while (true)
			{
				mKeepReading = true;

				// Program is suspended while waiting for an incoming connection.
				Debug.Log("Waiting for Connection");
				handler = listener.Accept();
				Debug.Log("Client Connected");

				// An incoming connection needs to be processed.
				while (mKeepReading)
				{
					bytes = new byte[1024];
					int bytesRec = handler.Receive(bytes);


					////////////////////////
					// use bytes, bytesRec



					if (bytesRec <= 0)
					{
						mKeepReading = false;
						handler.Disconnect(true);
						break;
					}

					if (bytesRec < bytes.Length)
					{
						//isRefresh = true;

						break;
					}

					System.Threading.Thread.Sleep(1);
				}

				System.Threading.Thread.Sleep(1);
			}
		}
		catch (System.Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	void stopServer()
	{
		mKeepReading = false;

		//stop thread
		if (mSocketThread != null)
		{
			mSocketThread.Abort();
		}

		if (handler != null && handler.Connected)
		{
			handler.Disconnect(false);
			Debug.Log("Disconnected!");
		}
	}

	void OnDisable()
	{
		stopServer();
	}
}
