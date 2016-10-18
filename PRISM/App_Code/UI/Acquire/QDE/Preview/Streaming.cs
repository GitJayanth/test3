using System;
using System.IO;
using System.Web;

//namespace HyperObjects.PublicationManager.Business.Tools
//{
//  /// <summary>
//  /// Description résumée de Streaming.
//  /// </summary>
  public class Streaming
  {
    public static byte[] EmptyByte
    {
      get{return new byte[0];}
    }

    public static byte[] StreamToByte(System.IO.Stream stream)
    {
      if (stream is MemoryStream)
        return ((MemoryStream)stream).ToArray();
      byte[] buffer = EmptyByte;
      if (stream!=null && stream.CanRead)
      {
        Reset(ref stream);
        int readed = 0;
        buffer = new byte[stream.Length];
        while ((readed = stream.Read(buffer,readed,buffer.Length-readed))>0);
      }
      stream.Close();
      return buffer;
    }

    public static string StreamToString(Stream stream)
    {
      return ByteToString(StreamToByte(stream));
    }

    public static Stream ByteToStream(byte[] buffer)
    {
      MemoryStream stream = new MemoryStream();
      stream.Write(buffer,0,buffer.Length);
      return stream;
    }

    public static string ByteToString(byte[] buffer)
    {
      return System.Text.Encoding.UTF8.GetString(buffer,0,buffer.Length);
    }

    public static void WriteStream(HttpResponse response, byte[] array)
    {
      response.BinaryWrite(array);
    }

    public static void WriteStream(HttpResponse response, Stream stream)
    {
      response.BinaryWrite(StreamToByte(stream));
    }


    public static void SendStream(HttpResponse response, Stream stream, string mimeType, string clientFileName)
    {
      response.Clear();
      if (stream!=null && stream.Length>0)
      {
        response.ContentType = mimeType;
        response.AddHeader("Content-disposition","attachment; filename="+clientFileName);
        WriteStream(response,stream);
        stream.Close();
      }
      response.End();
    }

    public static void SendStream(HttpResponse response, FileStream stream, string mimeType, string clientFileName)
    {
      response.Clear();
      if (stream!=null && stream.Length>0)
      {
        response.ContentType = mimeType;
        response.AddHeader("Content-disposition","attachment; filename="+clientFileName);
        response.WriteFile(stream.Handle,0,stream.Length);
        stream.Close();
      }
      response.End();
    }

    public static Stream GetDocumentStream(System.Xml.XmlDocument xmlDoc)
    {
      Stream output = new MemoryStream();
      if (xmlDoc!=null)
        xmlDoc.Save(output);
      Streaming.Reset(ref output);
      return output;
    }

    public static void Reset(ref Stream stream)
    {
      if (stream != null)
      {
        stream.Flush();
        stream.Position = 0;
      }
    }
  }
//}
