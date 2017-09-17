using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class MessageData{
    public string stringData = "";
    public float mousex = 0;
    public float mousey = 0;
    public int type = 0;

    public static MessageData FromByteArray(byte[] input)
    {
        MemoryStream stream = new MemoryStream(input);
        BinaryFormatter formatter = new BinaryFormatter();

        MessageData data = new MessageData();
        data.stringData = (string)formatter.Deserialize(stream);
        data.mousex = (float)formatter.Deserialize(stream);
        data.mousey = (float)formatter.Deserialize(stream);

        return data;
    }
    public static byte[] ToByteArray(MessageData msg)
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, msg.stringData);
        formatter.Serialize(stream, msg.mousex);
        formatter.Serialize(stream, msg.mousey);
        formatter.Serialize(stream, msg.type);

        return stream.ToArray();
    }
}
