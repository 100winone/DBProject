using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DBProject.Control
{
    class Protocol
    {
        #region Head
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]

        public class HEAD
        {
            public byte STX;                //시작부호(0x02)
            public ushort bid_no;                //장치ID
            public byte opcode;                 //opcode
            public ushort Length;             //데이터길이
        }
        #endregion

       

        #region Tail
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class TAIL
        {
            public byte Checksum;           //에러체크(Head~Tail XOR)
            public byte ETX;                //종료부호(0x03)
        }
        #endregion

        #region BIS_POS
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_POS
        {
            public double pos_x;
            public double pos_y;
        }

        #endregion

        #region BIS_DATE
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_DATE
        {
            public int yyyy;
            public ushort MM;
            public ushort dd;
        }
        #endregion

        #region BIS_TIME
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_TIME
        {
            public ushort hh;
            public ushort mm;
            public ushort ss;
        }
        #endregion

        #region BIS_DEVICE_STAT;
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_DEVICE_STAT
        {
            public byte bit0;
            public byte bit1;
        }
        #endregion

        
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_OP_66
        {
            public BIS_DATE SendDate = new BIS_DATE();
            public BIS_TIME SendTime = new BIS_TIME();          //전송시간 DataTime.Now.ToString"yyyyMMddHHmmss")
            public BIS_DATE OccurDate = new BIS_DATE();   //발생일자
            public BIS_TIME OccurTime = new BIS_TIME();  //발생시간
            public UInt64 Route_id;
            public BIS_POS Pos = new BIS_POS();     //위치정보
            public ushort heading;
            public ushort bus_speed;
            public BIS_DEVICE_STAT device_stat = new BIS_DEVICE_STAT();

            public UInt64 bnode_id;
            public ushort bm_seqno;
            public ushort dptc_seqno;
            public ushort travel_time;
            public char sed_failed;
            public int cdma_grade;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Reserved;           //2
        }
     
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class BIS_OP_67
        {
            public BIS_DATE SendDate = new BIS_DATE();
            public BIS_TIME SendTime = new BIS_TIME();
            public BIS_DATE OccurDate = new BIS_DATE();   //발생일자
            public BIS_TIME OccurTime = new BIS_TIME();  //발생시간
            public UInt64 Route_id;
            public BIS_POS Pos = new BIS_POS();     //위치정보
            public ushort heading;
            public ushort bus_speed;
            public BIS_DEVICE_STAT device_stat = new BIS_DEVICE_STAT();

            public UInt64 bnode_id;
            public ushort brn_seqno;
            public ushort service_time;
            public ushort dptc_seqno;
            public ushort travel_time;
            public ushort nostop;           //무정차 구분- 0:정상, 1: 무정차 
            public char send_failed;
            public int cdma_grade;
       
            public ushort fdoor_time;
            public ushort bdoor_time;
            public ushort in_heading;
            
           [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Reserved;           //3
        }

        //ASCII HEADER
        #region Head
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]

        public class ASCII_HEAD
        {
            public byte [] STX ;                //시작부호(0x02)
            public ushort bid_no;                //장치ID
            public byte opcode;                 //opcode
            public ushort Length;             //데이터길이
        }
        #endregion
    }
}
