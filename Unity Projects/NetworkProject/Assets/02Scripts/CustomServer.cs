using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets; // TcpListener 사용 위해 선언
using System.Net; // IPHostEntry, Dns, IPEndPoint 사용 위해 선언
using System.Threading; // thread 사용 위해 선언
using System.IO; // Binary 사용 위해 선언

public class CustomServer : MonoBehaviour
{
	private TcpListener tcpListener = null;
	TcpClient tcpClient = null;
	private int portNum = 1755;
	private BinaryReader br = null;
	private BinaryWriter bw = null;

	private void Awake()
	{
		tcpListener = new TcpListener(portNum);
		tcpListener.Start();
		
		// 접속 시작
		Thread th = new Thread(new ThreadStart(AcceptClient))
		{
			IsBackground = true
		};
		th.Start();

		
	}

	void AcceptClient()
	{
		while (true)
		{
			// AcceptTcpClient()
			// TcpClient 생성과 대기상태 유지
			tcpClient = tcpListener.AcceptTcpClient();

			if (tcpClient.Connected)
			{
				string str = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
			}
			
			Thread th = new Thread(new ThreadStart(Process))
			{
				IsBackground = true
			};
			th.Start();
		}
	}

	public void Process()
	{
		string strValue;

		NetworkStream ns = tcpClient.GetStream(); // 데이터 주고 받기
		try
		{
			// 모듈 생성
			br = new BinaryReader(ns);
			bw = new BinaryWriter(ns);

			// 반복
			while (true)
			{
				strValue = br.ReadString();
				bw.Write(strValue);
			}
		}
		catch (SocketException se) // 연결이 끊어질 경우
		{
			br.Close();
			bw.Close();
			ns.Close();
			ns = null;
			tcpClient.Close();
			Thread.CurrentThread.Abort();
		}
		catch (IOException ex) // 읽고 쓰는 것에 문제가 생길 경우
		{
			br.Close();
			bw.Close();
			ns.Close();
			ns = null;
			tcpClient.Close();
			Thread.CurrentThread.Abort();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			for (int i = 0; i < host.AddressList.Length; i++)
			{
				if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
				{
					// 접속한 주소 출력
					Debug.Log(host.AddressList[i].ToString());
					break;
				}
			}
		}
	}
}
