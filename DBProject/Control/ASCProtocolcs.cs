using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DBProject.Control
{
    class ASCProtocolcs
    {
        #region Head
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_HEAD
        {
            //   public char STX;                //시작부호(0x02)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] bid_no;                //장치ID
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] opcode;                //opcode
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] length;             //데이터길이
        }
        #endregion

        #region Tail
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_TAIL
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte Checksum;           //에러체크(Head~Tail XOR)
                                            //  public char ETX;                //종료부호(0x03)
        }
        #endregion

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_OP_66
        {
            public ASC_DATE SendDate = new ASC_DATE();
            public ASC_TIME SendTime = new ASC_TIME();
            public ASC_DATE OccurDate = new ASC_DATE();
            public ASC_TIME OccurTime = new ASC_TIME();
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] Route_id;
            public ASC_POS Pos = new ASC_POS();
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] heading;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] bus_speed;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] bnode_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] brn_seqno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] dptc_seqno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] travel_time;
            public char bop_stat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] cdma_grade;
            public ASC_DEVICE_STAT device_stat = new ASC_DEVICE_STAT();

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] Reserved;
        }

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_OP_67
        {
            public ASC_DATE SendDate = new ASC_DATE();
            public ASC_TIME SendTime = new ASC_TIME();
            public ASC_DATE OccurDate = new ASC_DATE();
            public ASC_TIME OccurTime = new ASC_TIME();
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] Route_id;
            public ASC_POS Pos = new ASC_POS();
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] heading;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] bus_speed;
            public ASC_DEVICE_STAT device_stat = new ASC_DEVICE_STAT();

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public char[] bnode_id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public char[] brn_seqno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] service_time;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] travel_time;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] nostop;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] fdoor_time;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] bdoor_time;
            //public char[] in_heading = new char[3];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] dptc_seqno;
            public char bop_stat;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] cdma_grade;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)] // 원래 Size값 3 >> 에러
            public byte[] Reserved;
        }

        #region ASC_POS
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_POS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] pos_x;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] pos_y;
        }
        #endregion

        #region ASC_DATE
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_DATE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] yyyy;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] MM;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] dd;
        }
        #endregion

        #region ASC_TIME
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_TIME
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] hh;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] mm;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] ss;
        }
        #endregion

        #region ASC_DEVICE_STAT
        [Serializable, StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public class ASC_DEVICE_STAT
        {
            char bit0;
            char bit1;
        }
        #endregion
    }
}
