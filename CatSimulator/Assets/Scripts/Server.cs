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
	public static string returnValue = "";
	public static bool readyToUser = false;
	public static int frame = 0;
	public static string fdata = "";
	private int state=0;

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
			string str = null;
			state = 0;
			// Enter the listening loop.
			while(true)
			{
				Thread.Sleep(10);

				if(state == 0){
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
				//StreamWriter swriter=new StreamWriter(stream);
				}
				int i;

				// Loop to receive all the data sent by the client.
				while(state == 0 && ((i = stream.Read(bytes, 0, bytes.Length))!=0))
				{  
					//Debug.Log("0");
					// Send back a response.
					//stream.Write(msg1, 0, msg1.Length);
					// Translate data bytes to a ASCII string.
					str = System.Text.Encoding.UTF8.GetString (bytes);
					if(str[0] == 'R'){	//ready
						Debug.Log("Ready");
						//ㅇㅕㄱㅣㄱㅏ ㅌㅔㅅㅡㅌㅡ ㅍㅏㅇㅣㄹ
						//ㅇㅣㄱㅓ ㄷㅐㅅㅣㄴㅇㅔ 
						readyToUser = true;
						//msg1 = System.Text.Encoding.ASCII.GetBytes(""+frame);
						//stream.Write(msg1, 0, msg1.Length);
						state++;
					}
				}
				while(state == 1){
					//Debug.Log("1");
					//Debug.Log(frame + ":" + data);
					if(frame>1 && fdata.Length>0){
						Debug.Log(frame+":"+fdata);
						if(frame==3){
							returnValue = "";
							msg1 = System.Text.Encoding.UTF8.GetBytes(fdata);
							stream.Write(msg1, 0, msg1.Length);
							stream.Flush();
							state++;
							}
						}
					frame=0;
					fdata="";
				}

				while(state == 2 && ((i = stream.Read(bytes, 0, bytes.Length))!=0))
				{ 
					//Debug.Log("2");
					// Send back a response.
					//stream.Write(msg1, 0, msg1.Length);
					// Translate data bytes to a ASCII string.
					str = System.Text.Encoding.UTF8.GetString (bytes);
					//Debug.Log (11);
					returnValue = str;
					//Debug.Log (str);
					//str = System.Text.UTF8.GetString (bytes);
					//returnValue = (int)Convert.ChangeType(str,typeof(int));
					//Debug.Log(""+returnValue);
					//Debug.Log ("tt");
					state = 1;
				}
					
				Array.Clear(bytes, 0, bytes.Length);
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