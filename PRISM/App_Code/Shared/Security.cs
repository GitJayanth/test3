using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;


namespace HyperCatalog.Shared
{
  public class Crypter
  {
    private static string _LastError = string.Empty;
    public static string LastError
    {
      get{return _LastError;}
    }

    public Crypter()
    {
    }

    public static String Encrypt(String pass)
    {
      return Encrypt(pass, "&%#@?,:*");
    }

    public static String Decrypt(String pass)
    {
      return Decrypt(pass, "&%#@?,:*");
    }

    private static String Encrypt(String strText, String strEncrKey )
    {
      byte[] bKey = new byte [8] ;
      byte[] IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
      _LastError = string.Empty;
      try 
      {
        bKey = System.Text.Encoding.UTF8.GetBytes( strEncrKey.Substring(0,8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(bKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Convert.ToBase64String(ms.ToArray());

      }
      catch (Exception ex) 
      {
        _LastError = ex.ToString();
        return "";
      }
    }


    private static String Decrypt(String strText, String strEncrKey )
    {
      byte[] bKey = new byte [8] ;
      byte[] IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};
      _LastError = string.Empty;
      try 
      {
        bKey = System.Text.Encoding.UTF8.GetBytes( strEncrKey.Substring(0,8));
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
        MemoryStream ms = new MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(bKey, IV), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        return encoding.GetString(ms.ToArray());
      }
      catch (Exception ex) 
      {
        _LastError = ex.ToString();
        return "";
      }
    }
  }
}