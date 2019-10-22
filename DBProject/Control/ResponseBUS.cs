using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using static DBProject.Control.Protocol;

namespace DBProject.Control
{
    class ResponseBUS
    {
        public string Clientip;
        private NetworkStream stream;
        #region Send ACK
        /// <summary>
        /// Ack를 보낸다.
        /// </summary>
        public void SendAck(byte OpCode)
        {
           
        }
        #endregion

        #region Send
        /// <summary>
        /// TCP 통신을 보낸다.
        /// </summary>
        public bool Send(byte OpCode, byte[] cSendBuf, uint nSize)
        {
            //if (IsDispose || !tcpClient.Connected) return false;

            try
            {
                List<byte> sendmsg = new List<byte>();
                HEAD head = new HEAD();

                head.STX = 0x02;
                head.bid_no = 0x66;
                // head.DeviceID = ushort.Parse(m_DeviceID);
                head.opcode = OpCode;
                head.Length = (ushort)nSize;

                sendmsg.AddRange(TcpUtil.ObjectToByte(head));

                if (cSendBuf != null)
                    sendmsg.AddRange(cSendBuf);

                TAIL tail = new TAIL();
                //tail.Reserved = new byte[2];
                //Array.Clear(tail.Reserved, 0, 2);
                tail.ETX = 0x03;

                byte bCheckSum = 0x00;
                for (int i = 0; i < sendmsg.Count; i++)
                {
                    bCheckSum ^= sendmsg[i];
                }
                tail.Checksum = bCheckSum;
                sendmsg.AddRange(TcpUtil.ObjectToByte(tail));

                stream.Write(sendmsg.ToArray(), 0, sendmsg.Count);
                MakeLog(string.Format("[SendData] [Send0x{0:x2}] {1}", OpCode, TcpUtil.ToHexString(sendmsg.ToArray())), 0);
                //MakeLog(string.Format("[SendData] - {0}", sendmsg.Count), 1);
            }
            catch (Exception ex)
            {
                TcpUtil.ActionException(ex);
            }
            return true;
        }
        #endregion

        #region 로그
        public void MakeLog(string sLog, int bScreen)
        {
            string sMsg;

            sMsg = string.Format("[BISCS][{0}] {1}", Clientip, sLog);
            TcpUtil.MessageLog(sMsg, bScreen);
        }
        #endregion

    }
}
