using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net; // IPHostEntry, Dns, IPEndPoint 사용 위해 선언
using System.Threading; // thread 사용 위해 선언
using System.IO; // Binary 사용 위해 선언

class EchoServer
{
	TcpClient refClient;
	private BinaryReader br = null;
	private BinaryWriter bw = null;
	string strValue;

	public EchoServer(TcpClient echoClient)
	{
		refClient = echoClient;
	}

	public void Process()
	{
		NetworkStream ns = refClient.GetStream(); // 데이터 주고 받기
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
			refClient.Close();
			Thread.CurrentThread.Abort();
		}
		catch (IOException ex) // 읽고 쓰는 것에 문제가 생길 경우
		{
			br.Close();
			bw.Close();
			ns.Close();
			ns = null;
			refClient.Close();
			Thread.CurrentThread.Abort();
		}
	}
		
}

public class TcpServer : MonoBehaviour
{
	private TcpListener tcpListener = null;
	public int portNum;
	TcpClient tcpClient = null;

	private void Awake()
	{
		tcpListener = new TcpListener(portNum);
		tcpListener.Start();
		// Dns.GetHostName() 호스트명 알아내기
		// Dns.GetHostEntry() 호스트 정보 가져오기
		// IPHostEntry는 컨테이너 클래스
		IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
		for (int i = 0; i < host.AddressList.Length; i++)
		{
			if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
			{
				// 접속한 주소 출력
				// textBox1.Text = host.AddressList[i].ToString();
				break;
			}
		}
	}

	private void StartButtonClick()
	{
		Thread th = new Thread(new ThreadStart(AcceptClient));
		th.IsBackground = true;
		th.Start();
		//Debug.Log(((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString());
		
	}

	private void CloseButtonClick()
	{
		if(tcpListener != null)
		{
			tcpListener.Stop();
			tcpListener = null;
		}
	}

	private void AcceptClient()
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

			EchoServer echoServer = new EchoServer(tcpClient);
			Thread th = new Thread(new ThreadStart(echoServer.Process))
			{
				IsBackground = true
			};
			th.Start();
		}
	}
}
