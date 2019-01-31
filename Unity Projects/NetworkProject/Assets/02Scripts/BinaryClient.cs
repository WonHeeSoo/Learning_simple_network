using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class BinaryClient : MonoBehaviour
{
	TcpClient tcpClient = null;
	NetworkStream ns = null;
	BinaryWriter bw = null;

	bool YesNo;
	int Number;
	float Pi;
	string Message;

	private void Awake()
	{
		tcpClient = new TcpClient("localhost", 3000);

		ns = tcpClient.GetStream();
		
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			using (bw = new BinaryWriter(ns))
			{
				YesNo = true;
				Number = 12;
				Pi = 3.14f;
				Message = "Hello World!";

				bw.Write(YesNo);
				bw.Write(Number);
				bw.Write(Pi);
				bw.Write(Message);
			}

			ns.Close();
			tcpClient.Close();

		}
	}
	/*void OnGUI()
	{
		// Make a background box
		GUI.Box(new Rect(10, 10, 100, 90), YesNo.ToString());
		GUI.Box(new Rect(10, 20, 100, 90), number.ToString());
		GUI.Box(new Rect(10, 30, 100, 90), Pi.ToString());
		GUI.Box(new Rect(10, 40, 100, 90), Message);
	}*/
}