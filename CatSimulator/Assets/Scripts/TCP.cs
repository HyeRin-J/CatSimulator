using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class TCP : MonoBehaviour {
	static bool socketReady = false;

	static TcpClient mySocket;
	static NetworkStream theStream;
	static StreamWriter theWriter;
	static StreamReader theReader;
	static IPAddress localAddr = IPAddress.Parse("127.0.0.1");
	static Int32 Port = 9999;

	static bool trigger=false;
    static string frame = "";
    static string data = "";
    public static void setData(int n,string dat){
		trigger = true;
		frame = ""+n;
		data = dat;
    }
	// Use this for initialization
	static void Main() {
        TcpListener server = null;
            try
            {
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, Port);
 
                // Start listening for client requests.
                server.Start();
 
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
 
                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");
 
                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");
 
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();
 
                    int i;
 
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    { 
						if(trigger){
						trigger = false;
							byte[] msg = System.Text.Encoding.Default.GetBytes(frame);
							// Send back a response.
							stream.Write(msg, 0, msg.Length);
							msg = System.Text.Encoding.Default.GetBytes(data);
                        	// Send back a response.
                        	stream.Write(msg, 0, msg.Length);
						}
                    }
 
                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
 
 
            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }
	}
