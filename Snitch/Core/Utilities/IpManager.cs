using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Snitch.Core.Utilities
{
    public class IpManager
    {

        public static string GetLocalIp()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                try
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address.ToString();
                }
                catch
                {
                    return "127.0.0.1";
                }

            }
        }
        public static string GetTeredoIP()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface I in nics)
            {
                if (I.Name.Contains("Teredo"))
                {
                    foreach (UnicastIPAddressInformation ip in I.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.IsIPv6Teredo)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return "";
        }
        public static string GetTeredoIP(string ConnectionText)
        {
            List<string> IpList = GetListIp(ConnectionText);
            string TeredoIp = "";
            foreach (string ip in IpList)
            {
                if (ip.Contains("2001:"))
                {
                    TeredoIp = ip;
                    break;
                }
            }
            return TeredoIp;
        }
        public static string[] GetMIP(string ConnectionText)
        {
            List<string> IpList = GetListIp(ConnectionText);
            IpList.Reverse();
            return IpList.ToArray();
        }
        public static List<string> GetListIp(string ConnectionText)
        {
            string[] strM = ConnectionText.Split(new[] { "<L" }, StringSplitOptions.None);
            List<string> IpList = new List<string>();
            foreach (string str in strM)
            {
                if (str.Contains("N=\""))
                {
                    string tmp = str.Split(new[] { "N=\"" }, StringSplitOptions.None)[1];
                    tmp = tmp.Split(new[] { "\"" }, StringSplitOptions.None)[0];
                    IpList.Add(tmp);
                }
            }
            return IpList;
        }
    }
}
