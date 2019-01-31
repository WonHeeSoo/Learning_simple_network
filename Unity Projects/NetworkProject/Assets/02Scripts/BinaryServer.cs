using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class BinaryServer : MonoBehaviour
{
	TcpListener tcpListener = null;
	TcpClient tcpClient = null;
	NetworkStream ns = null;
	BinaryReader br = null;

	bool YesNo;
	int Number;
	float Pi;
	string Message;

	private void Awake()
	{
		tcpListener = new TcpListener(IPAddress.Any, 3000);
		tcpListener.Start();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			tcpClient = tcpListener.AcceptTcpClient();

			ns = tcpClient.GetStream();
			using (br = new BinaryReader(ns))
			{
				YesNo = br.ReadBoolean();
				Number = br.ReadInt32();
				Pi = br.ReadSingle();
				Message = br.ReadString();
			}
			Debug.Log(YesNo);
			Debug.Log(Number);
			Debug.Log(Pi);
			Debug.Log(Message);

			ns.Close();
			tcpClient.Close();
			tcpListener.Stop();
		}
	}
}