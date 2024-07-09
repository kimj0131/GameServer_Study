using DummyClient;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    ServerSession _session = new ServerSession();

    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
    }

    void Start()
    {
        // DNS (Domain Name System)
        string host = Dns.GetHostName();
        IPHostEntry iPHost = Dns.GetHostEntry(host);
        IPAddress ipAddr = iPHost.AddressList[0];
        IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

        // Connector를 이용해 연결
        Connector connector = new Connector();

        connector.Connect(endPoint,
            () => { return _session; },
            1);
    }

    void Update()
    {
        // 프레임내에 들어온 모든 패킷 처리
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach (IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);
    }
}
