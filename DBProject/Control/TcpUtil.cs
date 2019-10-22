using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static DBProject.Control.Protocol;

namespace DBProject.Control
{
    class TcpUtil
    {
        public static Dictionary<string, ResponseBUS> RemoteClients = new Dictionary<string, ResponseBUS>();
        public static Action<string, int> MessageLog;
        public static Action<Exception> ExceptionEvent;
        public static Action<string, string, bool> ChangedClientEvent;

        public static void ActionMessageLog(string msg, int bScreen)
        {
            if (MessageLog != null) MessageLog(msg, bScreen);
        }

        public static void ActionChangedClient(string id, string ip, bool isConnected)
        {
            if (ChangedClientEvent != null) ChangedClientEvent(id, ip, isConnected);
        }

        public static void ActionException(Exception ex)
        {
            if (ExceptionEvent != null) ExceptionEvent(ex);
        }

        public static byte GetBCC(List<byte> dataStream)
        {
            byte bcc = 0x00;

            if (dataStream != null && dataStream.Count > 0)
            {
                for (int i = 0; i < dataStream.Count; i++)
                {
                    bcc ^= dataStream[i];
                }
            }
            return bcc;
        }

        /// <summary>
        /// 통신Endian 옵션
        /// </summary>
        public static bool IsLittleEndian = BitConverter.IsLittleEndian; // AVI장비는 빅엔디안 방식을 사용

        /// <summary>
        /// 2개의 바이트배열이 같은지 비교한다.
        /// </summary>
        /// <param name="array1">첫번째 바이트</param>
        /// <param name="array2">두번째 바이트</param>
        /// <returns>결과</returns>
        public static bool CompareByteArray(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;
            else
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 바이트 배열을 index부터 byte로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>Char</returns>
        public static byte ConvertByte(byte[] input, ref uint index)
        {
            byte[] temp = GetBytesTo<byte>(input, ref index);
            return temp[0];
        }

        /// <summary>
        /// 바이트 배열을 index부터 sbyte로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>Char</returns>
        public static sbyte ConvertsByte(byte[] input, ref uint index)
        {
            byte[] temp = GetBytesTo<sbyte>(input, ref index);
            return Convert.ToSByte(temp[0]);
        }

        /// <summary>
        /// 바이트 배열을 Char로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>Char</returns>
        public static char ConvertChar(byte[] input)
        {
            return BitConverter.ToChar(input, 0);
        }

        /// <summary>
        /// 바이트 배열을 index부터 Char로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>Char</returns>
        public static char ConvertChar(byte[] input, ref uint index)
        {
            byte[] temp = GetBytesTo<char>(input, ref index);
            return BitConverter.ToChar(temp, 0);
        }

        /// <summary>
        /// Char를 바이트배열로 변환
        /// </summary>
        /// <param name="input">Char</param>
        /// <returns>바이트배열</returns>
        public static byte[] ConvertChar(char input)
        {
            byte[] result = BitConverter.GetBytes(input);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 정수32로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수32</returns>
        public static int ConvertInt(byte[] input)
        {
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return BitConverter.ToInt32(input, 0);
        }

        /// <summary>
        /// 바이트 배열을 index부터 정수32로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수32</returns>
        public static int ConvertInt(byte[] input, ref uint index)
        {
            byte[] temp = GetBytesTo<int>(input, ref index);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }

        /// <summary>
        /// 정수32를 바이트배열로 변환
        /// </summary>
        /// <param name="input">정수32</param>
        /// <returns>바이트배열</returns>
        public static byte[] ConvertInt(int input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 정수32로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수32</returns>
        public static uint ConvertUInt(byte[] input)
        {
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return BitConverter.ToUInt32(input, 0);
        }

        /// <summary>
        /// 바이트 배열을 정수32로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수32</returns>
        public static uint ConvertUInt(byte[] input, ref uint index)
        {
            byte[] temp = GetBytesTo<uint>(input, ref index);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(temp);
            return BitConverter.ToUInt32(temp, 0);
        }

        /// <summary>
        /// 정수32를 바이트배열로 변환
        /// </summary>
        /// <param name="input">정수32</param>
        /// <returns>바이트배열</returns>
        public static byte[] ConvertUInt(uint input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        public static byte[] ConvertUInt64(UInt64 input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 정수16로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수16</returns>
        public static short ConvertShort(byte[] input)
        {
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return BitConverter.ToInt16(input, 0);
        }

        /// <summary>
        /// 바이트 배열을 정수16로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>정수16</returns>
        public static short ConvertShort(byte[] input, ref uint index)
        {
            byte[] Temp = GetBytesTo<short>(input, ref index);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(Temp);
            return BitConverter.ToInt16(Temp, 0);
        }

        public static ushort ConvertUShort(byte[] input, ref uint index)
        {
            byte[] Temp = GetBytesTo<ushort>(input, ref index);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(Temp);
            return BitConverter.ToUInt16(Temp, 0);
        }

        /// <summary>
        /// 정수16를 바이트배열로 변환
        /// </summary>
        /// <param name="input">정수16</param>
        /// <returns>바이트배열</returns>
        public static byte[] ConvertShort(short input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        public static byte[] ConvertUShort(ushort input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 double값으로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>double</returns>
        public static double ConvertDouble(byte[] input)
        {
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return BitConverter.ToDouble(input, 0);
        }

        /// <summary>
        /// 바이트 배열을 index부터 double값으로 변환
        /// </summary>
        /// <param name="input">바이트배열</param>
        /// <returns>double</returns>
        public static double ConvertDouble(byte[] input, ref uint index)
        {
            byte[] Temp = GetBytesTo<double>(input, ref index);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(Temp);
            return BitConverter.ToDouble(Temp, 0);
        }

        /// <summary>
        /// double값을 바이트 배열로 변환
        /// </summary>
        /// <param name="input">double</param>
        /// <returns>바이트</returns>
        public static byte[] ConvertDouble(double input)
        {
            byte[] result = BitConverter.GetBytes(input);
            if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 UTF8 문자로 변환
        /// </summary>
        /// <param name="input">바이트 배열</param>
        /// <returns>UTF8 문자</returns>
        public static string ConvertUTF8(byte[] input)
        {
            //if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return Encoding.UTF8.GetString(input);
        }

        /// <summary>
        /// UTF8 문자를 바이트 배열로 변환
        /// </summary>
        /// <param name="input">UTF8 문자</param>
        /// <returns>바이트 배열</returns>
        public static byte[] ConvertUTF8(string input)
        {
            byte[] result = Encoding.UTF8.GetBytes(input);
            //if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        /// <summary>
        /// 바이트 배열을 UTF8 문자로 변환
        /// </summary>
        /// <param name="input">바이트 배열</param>
        /// <returns>UTF8 문자</returns>
        public static string ConvertANSI(byte[] input)
        {
            //if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(input);
            return Encoding.Default.GetString(input);
        }

        /// <summary>
        /// ANSI 문자를 바이트 배열로 변환
        /// </summary>
        /// <param name="input">ANSI 문자</param>
        /// <returns>바이트 배열</returns>
        public static byte[] ConvertANSI(string input)
        {
            byte[] result = Encoding.Default.GetBytes(input);
            //if (BitConverter.IsLittleEndian != IsLittleEndian) Array.Reverse(result);
            return result;
        }

        public static int ReverseInt(int input)
        {
            return BitConverter.ToInt32(ConvertInt(input), 0);
        }

        public static uint ReverseUInt(uint input)
        {
            return BitConverter.ToUInt32(ConvertUInt(input), 0);
        }

        public static short ReverseShort(short input)
        {
            return BitConverter.ToInt16(ConvertShort(input), 0);
        }

        public static double ReverseDouble(double input)
        {
            return BitConverter.ToDouble(ConvertDouble(input), 0);
        }

        /// <summary>
        /// 바이트 배열을 헥사문자열로 반환
        /// </summary>
        /// <param name="bin_data">바이트 배열</param>
        /// <returns>헥사문자열</returns>
        public static string ToHexString(IEnumerable<byte> bin_data)
        {
            StringBuilder result = new StringBuilder();

            foreach (byte ch in bin_data)
            {
                result.AppendFormat("{0:X2}", ch);
            }
            //result.Remove(result.Length - 1, 1);

            return result.ToString();
        }

        public static string ToHexString(IEnumerable<byte> bin_data, uint length)
        {
            StringBuilder result = new StringBuilder();
            int i = 0;
            foreach (byte ch in bin_data)
            {
                result.AppendFormat("{0:X2}", ch);
                i++;
                if (i >= length)
                    break;
            }
            //result.Remove(result.Length - 1, 1);

            return result.ToString();
        }

        /// <summary>
        /// 객체를 직렬화하여 바이트 배열로 반환
        /// </summary>
        /// <param name="o">객체</param>
        /// <returns>바이트 배열</returns>
        public static byte[] SerializeData(Object o)
        {
            byte[] result = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, o);
                    result = ms.ToArray();
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Runtime.Serialization.SerializationException == false)
                    throw ex;
            }
            return result;
        }

        /// <summary>
        /// 바이트 배열을 직렬화 하여 객체로 반환
        /// </summary>
        /// <param name="theByteArray">바이트 배열</param>
        /// <returns>객체</returns>
        public static object DeserializeData(byte[] theByteArray)
        {
            object result = null;
            try
            {
                using (MemoryStream ms = new MemoryStream(theByteArray))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    ms.Position = 0;
                    result = bf.Deserialize(ms);
                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Runtime.Serialization.SerializationException == false)
                    throw ex;
            }
            return result;
        }

        //public static object ByteToObject(byte[] rawdatas, object anytype)
        //{
        //    //unsafe
        //    {
        //        fixed (byte* fixed_buffer = rawdatas)
        //        {
        //            Marshal.PtrToStructure((IntPtr)fixed_buffer, anytype);
        //        }
        //    }

        //    return anytype;
        //}

        public static byte[] ObjectToByte(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            byte[] rawdatas = new byte[rawsize];
            unsafe
            {
                fixed (byte* fixed_buffer = rawdatas)
                {
                    Marshal.StructureToPtr(anything, (IntPtr)fixed_buffer, false);
                }
            }
            return rawdatas;
        }

        public static string GetMyIPAddress()
        {
            IPHostEntry host = Dns.Resolve(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();

            return myip;
        }

        public static int GetHeaderHostIP(string sOrgIP, ref byte[] sHeaderIP)
        {
            string[] strTemp;

            StringBuilder strb = new StringBuilder();

            if (sOrgIP == "")
            {
                return 0;
            }

            strTemp = sOrgIP.Split('.');

            for (int i = 0; i < strTemp.Length; i++)
            {
                if (i != 0)
                    strb.Append(".");
                strb.Append(strTemp[i].PadLeft(3, '0'));
            }
            strb.Append("-");

            sHeaderIP = ConvertANSI(strb.ToString());

            return 1;
        }

        public static string IntToHex(uint nValue, uint nLength)
        {
            string sRet;
            sRet = string.Format("{0:x" + nLength.ToString() + "}", nValue);
            return sRet;
        }

        public static string GetCSNHex(uint CSN)
        {
            return IntToHex(CSN == 0xFFFFFFFF ? 0 : CSN, 8);
        }

        public static byte[] GetBytesToBytes(byte[] buf, ref uint index, uint size)
        {
            byte[] Temp = new byte[size];
            Array.Copy(buf, index, Temp, 0, size);
            index += size;
            return Temp;
        }

        public static char[] GetBytesToChars(byte[] buf, ref uint index, uint size)
        {
            char[] Temp = new char[size];
            Array.Copy(buf, index, Temp, 0, size);
            index += size;
            return Temp;
        }

        public static byte[] GetBytesTo<T>(byte[] buf, ref uint index)
        {
            uint TypeSize = 0;

            switch (typeof(T).Name)
            {
                case "Byte":
                case "SByte":
                    TypeSize = 1;
                    break;
                case "Int16":
                case "UInt16":
                case "Char":
                    TypeSize = 2;
                    break;
                case "Int32":
                case "UInt32":
                    TypeSize = 4;
                    break;
                case "Double":
                case "Int64":
                case "UInt64":
                    TypeSize = 8;
                    break;
                default:
                    return null;
            }

            byte[] Temp = new byte[TypeSize];
            try
            {
                Array.Copy(buf, index, Temp, 0, TypeSize);
                index += TypeSize;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Temp;
        }

        public static byte[] GetObjectBytes(object anything)
        {
            List<byte> bytes = new List<byte>();
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bytes.ToArray();
        }

        public static void GetBytesObject(byte[] buf, object anything, ref uint index, uint nSize)
        {
            try
            {
                #region HEAD
                if (anything is HEAD)
                {
                    HEAD Hd = anything as HEAD;
                    Hd.STX = TcpUtil.ConvertByte(buf, ref index);
                    // Hd.Version = TcpUtil.ConvertByte(buf, ref index);
                    // Hd.DeviceID = TcpUtil.ConvertUShort(buf, ref index);
                    Hd.opcode = TcpUtil.ConvertByte(buf, ref index);
                    // Hd.Reserved = TcpUtil.ConvertByte(buf, ref index);
                    Hd.Length = TcpUtil.ConvertUShort(buf, ref index);
                }
                #endregion

                #region TAIL
                if (anything is TAIL)
                {
                    TAIL Tl = anything as TAIL;
                    //  Tl.Reserved = TcpUtil.GetBytesToBytes(buf, ref index, 2);
                    Tl.Checksum = TcpUtil.ConvertByte(buf, ref index);
                    Tl.ETX = TcpUtil.ConvertByte(buf, ref index);
                }
                #endregion

                //#region ACK
                //if (anything is BIS_OP_66)
                //{
                //    BIS_OP_66 op66 = anything as BIS_OP_66;
                //    //op66.SendDate = TcpUtil.ConvertUInt(buf, ref index);
                //    //op66.SendTime = TcpUtil.ConvertUInt(buf, ref index);
                //    op66.OccurDate = TcpUtil.ConvertUInt(buf, ref index);
                //    op66.OccurTime = TcpUtil.ConvertUInt(buf, ref index);
                //    op66.bus_speed = TcpUtil.ConvertByte(buf, ref index);
                //    op66.Pos = TcpUtil.ConvertUInt(buf, ref index);
                //    op66.heading = TcpUtil.ConvertByte(buf, ref index);

                //    op66.bus_speed = TcpUtil.ConvertByte(buf, ref index);
                //    op66.device_stat = TcpUtil.ConvertUInt(buf, ref index);

                //    op66.bnode_id = TcpUtil.ConvertUInt(buf, ref index);
                //    op66.bm_seqno = TcpUtil.ConvertByte(buf, ref index);
                //    op66.dptc_seqno = TcpUtil.ConvertByte(buf, ref index);
                //    op66.travel_time = TcpUtil.ConvertByte(buf, ref index);
                //    op66.cdma_grade = TcpUtil.ConvertInt(buf, ref index);

                //    op66.Reserved = TcpUtil.GetBytesToBytes(buf, ref index, 2);
                //}
                //#endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}
