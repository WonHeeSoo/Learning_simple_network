using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetworkServer : MonoBehaviour
{
	int port = 13000;
    string ip = "127.0.0.1";

    TcpListener server = null;
    IPAddress localAddr = null;

	byte[] bytes = null;
	string data = null;

	Thread th = null;

	private void Awake()
	{
		localAddr = IPAddress.Parse(ip); // IPv4 주소
		server = new TcpListener(localAddr, port); // 서버

		bytes = new byte[256]; // 데이터 읽기 용 버퍼
		data = null;

		th = new Thread(new ThreadStart(StartServerFunc)) // Thread
		{
			IsBackground = true // 주 스레드와 함께 종료
		};
	}

    private void Start()
    {
        th.Start();

    }
    
	void StartServerFunc()
	{
		try
		{
			server.Start(); // 클라이언트 요청 수신 대기
			
			while (true)
			{
				Debug.Log("Waiting for a connection...");
				TcpClient client = server.AcceptTcpClient();
				Debug.Log("Connected!");

				data = null;

				NetworkStream stream = client.GetStream();

				int i;

				while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
				{
					//data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
					data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
					Debug.Log("Received : " + data);

					// 문자열의 복사본을 대문자로 변환
					//data = data.ToUpper();

					//byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
					byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);
					stream.Write(msg, 0, msg.Length);
					Debug.Log("Sent : " + data);
				}

				client.Close();
			}
		}
		catch(SocketException e)
		{
			Debug.Log("SocketException : " + e);
		}
		finally
		{
			// Stop listening for new clients
			server.Stop();
		}
		
	}
}
