using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    private void Start()
    {
        List<PlayerData> playerDataList = new List<PlayerData>();
        playerDataList.Add(new PlayerData("Player1", 100));

        string json = JsonConvert.SerializeObject(playerDataList.ToArray());

        string filePath = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(filePath, json);

        Debug.Log("Данные сохранены в JSON: " + filePath);
    }

    public string GenerateData()
    {
        bool isVPN = IsVpn();
        string id = SystemInfo.deviceUniqueIdentifier;
        string lang = Application.systemLanguage.ToString();
        string batteryLevel = (SystemInfo.batteryLevel * 100).ToString();
        bool batteryStatus = SystemInfo.batteryStatus == BatteryStatus.Charging;
        bool batteryFull = SystemInfo.batteryStatus == BatteryStatus.Full;

        string endData = $"{{\"userData\": " +
            $"{{ \"agQG\": {isVPN.ToString().ToLower()}, " +
            $"\"qGaw\": \"{id}\", \"LGaB\": \"{lang}\", " +
            $"\"isCh\": {batteryStatus.ToString().ToLower()}," +
            $" \"isFu\": {batteryFull.ToString().ToLower()}, " +
            $"\"BLel\": \"{batteryLevel}\" }} }}";

        return endData;
    }

    public bool IsVpn()
    {
        bool isVPN = false;

        if (NetworkInterface.GetIsNetworkAvailable())
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in interfaces)
            {
                if (Interface.OperationalStatus == OperationalStatus.Up)
                {
                    if (((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback))
                        || Interface.Description.Contains("VPN") || Interface.Description.Contains("vpn"))
                    {
                        IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                        isVPN = true;
                    }
                }
            }
        }

        return isVPN;
    }
}

[System.Serializable]   
public class PlayerData
{
    public string playerName;
    public int playerScore;

    public PlayerData(string name, int score)
    {
        playerName = name;
        playerScore = score;
    }
}