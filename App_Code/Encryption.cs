using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for Encryption
/// </summary>
public class Encryption
{
    byte[] initVec;
    byte[] encKey;

    public Encryption()
    {
        encKey = Encoding.ASCII.GetBytes(create16("8d11ee552a8a4ff4be441f59e46860da"));
        xorBytes(encKey, Encoding.ASCII.GetBytes(create16("c8003cf8284843d7b9bc23102008b676")));
        xorBytes(encKey, Encoding.ASCII.GetBytes(create16("a4401c890c6f4c59a0c0496a1c02cff8")));

        initVec = Encoding.ASCII.GetBytes(create8("559666b51df442c9a7e875133c301d9d"));
        xorBytes(initVec, Encoding.ASCII.GetBytes(create8("d6c64913c375430ca4acf189daaca33b")));

    }

    public String create8(String text)
    {
        String temp = "";

        temp = text;
        if (temp.Length > 8)
        {
            temp = temp.Substring(0, 8);
        }
        while (temp.Length < 8)
        {
            if (temp.Length <= 8 - text.Length)
            {
                temp += text;
            }
            else
            {
                temp += text.Substring(0, 8 - temp.Length);
            }
        }
        return temp;
    }

    public String create16(String text)
    {
        String temp = "";

        temp = text;
        if (temp.Length > 16)
        {
            temp = temp.Substring(0, 16);
        }
        while (temp.Length < 16)
        {
            if (temp.Length <= 16 - text.Length)
            {
                temp += text;
            }
            else
            {
                temp += text.Substring(0, 16 - temp.Length);
            }
        }
        return temp;
    }

    public String create32(String text)
    {
        String temp = "";

        temp = text;
        if (temp.Length > 32)
        {
            temp = temp.Substring(0, 32);
        }
        while (temp.Length < 32)
        {
            if (temp.Length <= 32 - text.Length)
            {
                temp += text;
            }
            else
            {
                temp += text.Substring(0, 32 - temp.Length);
            }
        }
        return temp;
    }

    public byte[] xorBytes(byte[] first, byte[] second)
    {
        for (int i = 0; i < first.Length; i++)
        {
            int newint = (Convert.ToInt16(first[i]) ^ Convert.ToInt16(second[i]));
            first[i] = Convert.ToByte(newint);

        }
        return first;
    }

    public String encrypt(String strText)
    {
        ASCIIEncoding textConverter = new ASCIIEncoding();
        byte[] encrypted;
        byte[] toEncrypt;

        //Encrypt the data.
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        MemoryStream msEncrypt = new MemoryStream();

        //Get an encryptor.
        ICryptoTransform encryptor = tdes.CreateEncryptor(encKey, initVec);

        CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        //Convert the data to a byte array.
        toEncrypt = textConverter.GetBytes(strText);

        //Write all data to the crypto stream and flush it.
        csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
        csEncrypt.FlushFinalBlock();

        //Get encrypted array of bytes.
        encrypted = msEncrypt.ToArray();


        
        return (HexEncoding.ToString(encrypted));
    }

    public String decrypt(String strText)
    {
        ASCIIEncoding textConverter = new ASCIIEncoding();
        string roundtrip;
        byte[] fromEncrypt;
        byte[] encrypted;

        //Encrypt the data.
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        ICryptoTransform decryptor = tdes.CreateDecryptor(encKey, initVec);

        int discarded;
        encrypted = HexEncoding.GetBytes(strText, out discarded);
        //Now decrypt the previously encrypted message using the decryptor
        // obtained in the above step.
        MemoryStream msDecrypt = new MemoryStream(encrypted);
        CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        fromEncrypt = new byte[encrypted.Length];

        //Read the data out of the crypto stream.
        csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

        //Convert the byte array back into a string.
        roundtrip = textConverter.GetString(fromEncrypt);

        return (roundtrip.Replace("\0", ""));
    }
}
