using System;
using System.Collections.Generic;
using System.IO;

namespace wifi_scans_parcer
{
    class Program
    {
        public class WifiInfo
        {
            public string SSID;
            public string BSSID;
            public string level;
            public int NumBerOfMentions = 0;
            public double avglevel;
        }
        static void Main(string[] args)
        {
            List<WifiInfo> WifiInfoList = new List<WifiInfo>();
            WifiInfo wifiInfo;
            string readPath = @"D:\Projects\Wifi Measurements\J200_0,3.txt";
            //string readPath = @"D:\Projects\test.txt";
            string writePath = @"D:\Projects\AVG Wifi Measurements\J200_0,3AVG.txt";
            int IterCount = 0;
            StreamReader sr = new StreamReader(readPath, System.Text.Encoding.Default);
            string[] rawMass = sr.ReadToEnd().Split("\n");
            sr.Close();
            foreach (string CurrRawMass in rawMass)
            {
                if (CurrRawMass != null)
                {
                    if (CurrRawMass.Split('#').Length != 1)
                    {
                        wifiInfo = new WifiInfo();
                        wifiInfo.SSID = CurrRawMass.Split("#")[0];
                        wifiInfo.BSSID = CurrRawMass.Split("#")[1];
                        wifiInfo.level = CurrRawMass.Split("#")[2];
                        if (wifiInfo.BSSID != "")
                            if (!WifiInfoList.Exists(x => x.BSSID == wifiInfo.BSSID))
                            {
                                wifiInfo.avglevel = Double.Parse(wifiInfo.level.Split("dBm")[0]);
                                wifiInfo.NumBerOfMentions = 1;
                                WifiInfoList.Add(wifiInfo);
                            }
                            else
                            {
                                WifiInfoList.Find(x => x.BSSID.Contains(wifiInfo.BSSID)).avglevel += Convert.ToDouble(wifiInfo.level.Split("dBm")[0]);
                                WifiInfoList.Find(x => x.BSSID.Contains(wifiInfo.BSSID)).NumBerOfMentions++;
                            }
                    }
                    else
                    {
                        IterCount++;
                    }
                }
            }
            Console.WriteLine(IterCount);
            Console.WriteLine(WifiInfoList.Count);
            StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);
            foreach (WifiInfo WI in WifiInfoList)
            {
                if (WI.NumBerOfMentions > IterCount/2)
                {
                    Console.WriteLine(WI.SSID + " " + WI.BSSID + " " + Math.Round(WI.avglevel / WI.NumBerOfMentions, 1) + " " + WI.NumBerOfMentions);
                    //sw.WriteLine(WI.SSID + "#" + Math.Round(WI.avglevel / WI.NumBerOfMentions,1) + "dBm#" + WI.NumBerOfMentions);
                    sw.WriteLine(WI.SSID + "#" + WI.BSSID + "#" + Math.Round(WI.avglevel / WI.NumBerOfMentions, 1) + "dBm#" + WI.NumBerOfMentions);
                }
            }
            sw.Close();
            Console.Read();
        }
    }
}