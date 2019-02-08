using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System;

public class NetworkClient : MonoBehaviour
{
	int port = 13000;
	TcpClient client = null;
	string message1 = "안녕하세요 반갑습니다.";
	string message2 = "Hi, Hello abcdefghijklmnopqrstuvwxyz";
	string message3 = "바자다가사 마나아라하 카타차파";
	string message4 = "뵤벼뱌배베보버바비뷰부브";
	string message5 = ",./;'[]<>?:%^&*()_+-=~`!@#$%^&";
	byte[] data;
	
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.A))
		{
			Connect(message1);
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			Connect(message2);
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			Connect(message3);
		}
		if (Input.GetKeyUp(KeyCode.F))
		{
			Connect(message4);
		}
		if (Input.GetKeyUp(KeyCode.G))
		{
			Connect(message5);
		}
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(10, 10, 100, 90), "Loader Menu");

		if(GUI.Button(new Rect(20, 40, 80, 20), "Message 1"))
		{

		}
	}

	string Connect(string message)
	{
		string responseData = string.Empty;
		try
		{
			client = new TcpClient("127.0.0.1", port);
			//data = System.Text.Encoding.ASCII.GetBytes(message);
			data = System.Text.Encoding.UTF8.GetBytes(message);
			NetworkStream stream = client.GetStream();
			stream.Write(data, 0, data.Length);
			data = new byte[256];
			//string responseData = string.Empty;

			int bytes = stream.Read(data, 0, data.Length);
			//responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
			responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
			Debug.Log("Received: " + responseData);

			stream.Close();
			client.Close();
		}
		catch (ArgumentNullException e)
		{
			Debug.Log("ArgumentNullException: " + e);
		}
		catch (SocketException e)
		{
			Debug.Log("SocketException: " + e);
		}
		return responseData;
	}
}
