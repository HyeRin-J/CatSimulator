using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;

public class Server : MonoBehaviour {
	private bool mRunning;
	public static string msg = "";

	public Thread mThread;
	public TcpListener tcp_Listener = null;
	public TcpClient client;
	public NetworkStream stream;

	private static int frame = 0;
	private static string buffer = "";

	void Awake() {
		mRunning = true;
		ThreadStart ts = new ThreadStart(Receive);
		mThread = new Thread(ts);
		mThread.Start();
		print("Thread done...");
	}
	void Start(){
		mRunning = true;
	}

	public void stopListening() {
		mRunning = false;
	}

	public static void setData(int iframe,string ibuffer){
		frame = iframe;
		buffer = ibuffer;
	}


	void Receive()
	{  
		tcp_Listener=null;  
		try
		{


			// TcpListener server = new TcpListener(port);
			tcp_Listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 9999);

			// Start listening for client requests.
			tcp_Listener.Start();

			// Buffer for reading data
			byte[] bytes = new byte[1024];
			byte[] msg1 = new byte[1024];
			string data = null;

			// Enter the listening loop.
			while(true)
			{
				Thread.Sleep(10);

				Debug.Log("Waiting for a connection... ");

				// Perform a blocking call to accept requests.
				// You could also user server.AcceptSocket() here.
				client = tcp_Listener.AcceptTcpClient();


				if(client!=null){

					Debug.Log("Connected!");
					//isConnection=true;
					//client.Close();
					//break;

				}
				data = null;

				stream = client.GetStream();
				StreamWriter swriter=new StreamWriter(stream);
				int i;

				// Loop to receive all the data sent by the client.
				while((i = stream.Read(bytes, 0, bytes.Length))!=0)
				{  
					//msg1 = System.Text.Encoding.ASCII.GetBytes(prevdata);

					// Send back a response.
					//stream.Write(msg1, 0, msg1.Length);
					// Translate data bytes to a ASCII string.
					string str = System.Text.Encoding.UTF8.GetString (bytes);
					if(str[0] == 'R'){	//ready
						Debug.Log("Ready");
						//ㅇㅕㄱㅣㄱㅏ ㅌㅔㅅㅡㅌㅡ ㅍㅏㅇㅣㄹ
						//ㅇㅣㄱㅓ ㄷㅐㅅㅣㄴㅇㅔ 

						stream.Write(msg1, 0, msg1.Length);
					}
					Debug.Log (11);
					Debug.Log (str);


					Array.Clear(bytes, 0, bytes.Length);
					//data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
					
				}
			}
		}
		catch(SocketException e)
		{
			Debug.Log("SocketException:"+ e);
		}
		finally
		{
			// Stop listening for new clients.
			stopListening();
		}
	}
	void Update() {

	}

	void OnApplicationQuit() { // stop listening thread
		stopListening();// wait for listening thread to terminate (max. 500ms)
		mThread.Join(500);
	}
}