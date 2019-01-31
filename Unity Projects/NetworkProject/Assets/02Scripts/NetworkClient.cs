using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class NetworkClient : MonoBehaviour
{
	int port = 13000;
	TcpClient client = null; 
	string message = "hi";
	byte[] data;

	private void Awake()
	{
		
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			Debug.Log("시작");
			StartCoroutine("Connect");
		}
	}

	IEnumerator Connect()
	{
		Debug.Log("접속");
		client = new TcpClient("127.0.0.1", port);
		data = System.Text.Encoding.ASCII.GetBytes(message);
		NetworkStream stream = client.GetStream();
		stream.Write(data, 0, data.Length);
		data = new byte[256];
		string responseData = string.Empty;

		int bytes = stream.Read(data, 0, data.Length);
		responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
		Debug.Log("Received: " + responseData);

		stream.Close();
		client.Close();

		yield return null;
	}
}
