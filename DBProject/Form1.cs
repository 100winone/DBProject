using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DBProject.Control;
using static DBProject.Control.ASCProtocolcs;
using static DBProject.Control.Protocol;


namespace DBProject
{
    public partial class Form1 : Form
    {
        Form2 f2 = new Form2();
        public static ArrayList busnum = new ArrayList();
        public static string dbsource;
        public static string idstr, pwstr, strip, strport;
        public Dictionary<string, Loglist> dictionary = new Dictionary<string, Loglist>();
        public static ArrayList packetlist = new ArrayList();
        public static string sql;
        public DataTable dt;
        public static bool isbreak;
        public static bool isrun;
        TcpClient tc;
        NetworkStream ns1;
        //int i;
        //String connectionString;
        //OracleConnection con = new OracleConnection();
        //OracleCommand cmd = new OracleCommand();
        //OracleDataReader dr;
        //DataTable dtOutHistory = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Form2.check == 1)   // 버스번호 선택 o
            {
                if (Form2.sel == 0)    // 기본전송
                {
                    if (Form2.tt == 0)     // BUSBRNINHISTORY
                    {
                        sql = "select* from BUSBRNINHISTORY where BID_NO IN (" + Form2.bbus + ") AND BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                    if (Form2.tt == 1)      // BUSBRNOUTHISTORY
                    {
                        sql = "select* from BUSBRNOUTHISTORY where BID_NO IN (" + Form2.bbus + ") AND BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                    if (Form2.tt == 2)     // 테이블 둘다 전송
                    {
                        sql = @"SELECT*FROM(SELECT BID_NO, BRN_OCCURYMDHMS, BRN_RCVROUTE, BRT_ID, BNODE_ID, BRN_SEQNO, NULL, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, 
BRN_SERVICETIME, BRN_SPEED, BRN_X, BRN_Y, BRN_FDOORTIME, BRN_BDOORTIME, BRN_NONSTOP, BRN_INYMDHMS, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 
FROM BUSBRNOUTHISTORY 
UNION ALL SELECT BID_NO, BRN_OCCURYMDHMS, NULL, BRT_ID, BNODE_ID, BRN_SEQNO, BRS_SEQNO, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, NULL, 
BRN_SPEED, BRN_X, BRN_Y, NULL, NULL, NULL, NULL, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 FROM BUSBRNINHISTORY)
 where BID_NO IN (" + Form2.bbus + ") AND BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                }
                if (Form2.sel == 1)    // 시간별 전송
                {
                    if (Form2.tt == 0)     // BUSBRNINHISTORY
                    {
                        sql = "select* from BUSBRNINHISTORY where BID_NO IN (" + Form2.bbus + ") AND BRN_OCCURYMDHMS like '" + Form2.date + "%' ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                    if (Form2.tt == 1)      // BUSBRNOUTHISTORY
                    {
                        sql = "select* from BUSBRNOUTHISTORY where BID_NO IN (" + Form2.bbus + ") AND BRN_OCCURYMDHMS like '" + Form2.date + "%' ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                    if (Form2.tt == 2)     // 테이블 둘다 전송
                    {
                        sql = @"SELECT*FROM(SELECT BID_NO, BRN_OCCURYMDHMS, BRN_RCVROUTE, BRT_ID, BNODE_ID, BRN_SEQNO, NULL, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, 
BRN_SERVICETIME, BRN_SPEED, BRN_X, BRN_Y, BRN_FDOORTIME, BRN_BDOORTIME, BRN_NONSTOP, BRN_INYMDHMS, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 
FROM BUSBRNOUTHISTORY 
UNION ALL SELECT BID_NO, BRN_OCCURYMDHMS, NULL, BRT_ID, BNODE_ID, BRN_SEQNO, BRS_SEQNO, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, NULL, 
BRN_SPEED, BRN_X, BRN_Y, NULL, NULL, NULL, NULL, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 FROM BUSBRNINHISTORY)
 where BID_NO IN (" + Form2.bbus + ") ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                }
            }
            else                    // 버스번호 선택 x >> 전체전송
            {
                if (Form2.sel == 0)    // 기본전송
                {
                    if (Form2.tt == 0)     // BUSBRNINHISTORY
                    {
                        sql = "select*from BUSBRNINHISTORY where BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                    if (Form2.tt == 1)      // BUSBRNOUTHISTORY
                    {
                        sql = "select*from BUSBRNOUTHISTORY where BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                    if (Form2.tt == 2)     // 테이블 둘다 전송
                    {
                        sql = @"SELECT*FROM(SELECT BID_NO, BRN_OCCURYMDHMS, BRN_RCVROUTE, BRT_ID, BNODE_ID, BRN_SEQNO, NULL, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, 
BRN_SERVICETIME, BRN_SPEED, BRN_X, BRN_Y, BRN_FDOORTIME, BRN_BDOORTIME, BRN_NONSTOP, BRN_INYMDHMS, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 
FROM BUSBRNOUTHISTORY 
UNION ALL SELECT BID_NO, BRN_OCCURYMDHMS, NULL, BRT_ID, BNODE_ID, BRN_SEQNO, BRS_SEQNO, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, NULL, 
BRN_SPEED, BRN_X, BRN_Y, NULL, NULL, NULL, NULL, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 FROM BUSBRNINHISTORY)
where BRN_OCCURYMDHMS like '" + Form2.date + "%'";
                    }
                }
                if (Form2.sel == 1)    // 시간별 전송
                {
                    if (Form2.tt == 0)     // BUSBRNINHISTORY
                    {
                        sql = "select* from BUSBRNINHISTORY where BRN_OCCURYMDHMS like '" + Form2.date + "%' ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                    if (Form2.tt == 1)      // BUSBRNOUTHISTORY
                    {
                        sql = "select* from BUSBRNOUTHISTORY where BRN_OCCURYMDHMS like '" + Form2.date + "%' ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                    if (Form2.tt == 2)     // 테이블 둘다 전송
                    {
                        sql = @"SELECT*FROM(SELECT BID_NO, BRN_OCCURYMDHMS, BRN_RCVROUTE, BRT_ID, BNODE_ID, BRN_SEQNO, NULL, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, 
BRN_SERVICETIME, BRN_SPEED, BRN_X, BRN_Y, BRN_FDOORTIME, BRN_BDOORTIME, BRN_NONSTOP, BRN_INYMDHMS, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 
FROM BUSBRNOUTHISTORY 
UNION ALL SELECT BID_NO, BRN_OCCURYMDHMS, NULL, BRT_ID, BNODE_ID, BRN_SEQNO, BRS_SEQNO, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, NULL, 
BRN_SPEED, BRN_X, BRN_Y, NULL, NULL, NULL, NULL, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 FROM BUSBRNINHISTORY)
where BRN_OCCURYMDHMS like'" + Form2.date + "%' ORDER BY BRN_OCCURYMDHMS ASC";
                    }
                }
            }
            DBConnect dbConnect = new DBConnect();
            dt = dbConnect.GetTable(sql);
            
            try
            {
                if (dt == null)
                {
                    listBox1.Items.Add("연결실패");
                }
                    
                else
                {
                    listBox1.Items.Add("연결성공");
                }
                    
            }
            catch (Exception)
            {
                listBox1.Items.Add("ERROR");
            }
            button2.Enabled = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            button5.Enabled = true;
            LoadDB();        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            Form2.date = null;
            Form2.sel = 0;
            Form2.tt = 0;
            Form2.asc = 0;
            string da = "5000";
            Form2.delay = da;
            isbreak = false;
            isrun = false;
            string url = Environment.CurrentDirectory + @"..\..\Config.xml";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);

                XmlNodeList xnList = xml.SelectNodes("Config");

                foreach (XmlNode xn in xnList)
                {
                    idstr = xn["DBID"].InnerText;
                    pwstr = xn["DBPW"].InnerText;
                    strip = xn["IP"].InnerText;
                    strport = xn["PORT"].InnerText;
                    dbsource = xn["DBDataSource"].InnerText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("XML 문제발생 \r\n" + ex);
            }
            sql = @"SELECT*FROM(SELECT BID_NO, BRN_OCCURYMDHMS, BRN_RCVROUTE, BRT_ID, BNODE_ID, BRN_SEQNO, NULL, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, 
BRN_SERVICETIME, BRN_SPEED, BRN_X, BRN_Y, BRN_FDOORTIME, BRN_BDOORTIME, BRN_NONSTOP, BRN_INYMDHMS, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 
FROM BUSBRNOUTHISTORY 
UNION ALL SELECT BID_NO, BRN_OCCURYMDHMS, NULL, BRT_ID, BNODE_ID, BRN_SEQNO, BRS_SEQNO, BRN_DETECTSEQNO, BRN_TOTALTIME, BRN_TRAVELTIME, NULL, 
BRN_SPEED, BRN_X, BRN_Y, NULL, NULL, NULL, NULL, BRN_SENDYMDHMS, BRN_RCVYMDHMS, BRN_ANGLE, CDMA_GRADE, BRN_X2, BRN_Y2 FROM BUSBRNINHISTORY)
where BRN_OCCURYMDHMS like '2019101408595%' ORDER BY BID_NO ASC";

            DBConnect dbConnect = new DBConnect();
            dt = dbConnect.GetTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!busnum.Contains(Convert.ToString(dt.Rows[i][0])))
                    busnum.Add(dt.Rows[i][0]);
            }
        }


        public void LoadDB()
        {

            Connect();
            String[] arr = new string[dt.Columns.Count];

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                for(int z = 0; z < dt.Columns.Count; z++)
                {
                    if (isrun == false) break;

                    arr[z] = dt.Rows[i][z].ToString();
                    if (z == dt.Columns.Count - 1)
                    {


                        if (Form2.asc == 0)
                        {

                            HEAD head = new HEAD();

                            head.STX = 0x02;

                            if (Form2.tt == 0)
                            {
                                head.opcode = 0x66;
                                head.Length = ushort.Parse(Marshal.SizeOf<BIS_OP_66>().ToString());
                            }
                            if (Form2.tt == 1)
                            {
                                head.opcode = 0x67;
                                head.Length = ushort.Parse(Marshal.SizeOf<BIS_OP_67>().ToString());
                            }
                            if (Form2.tt == 2)
                            {

                                if (string.IsNullOrEmpty(dt.Rows[i][14].ToString())) //BUSBRNINHISTORY
                                {
                                    head.opcode = 0x66;
                                    head.Length = ushort.Parse(Marshal.SizeOf<BIS_OP_66>().ToString());
                                }
                                else                                  //BUSBRNOUTHISTORY
                                {
                                    head.opcode = 0x67;
                                    head.Length = ushort.Parse(Marshal.SizeOf<BIS_OP_67>().ToString());
                                }
                            }

                            head.bid_no = ushort.Parse(dt.Rows[i][0].ToString());

                            BIS_OP_66 body = new BIS_OP_66();
                            BIS_OP_67 body2 = new BIS_OP_67();

                            if (Form2.tt == 0)
                            {
                                BIS_DATE snd_date = new BIS_DATE();
                                BIS_TIME snd_time = new BIS_TIME();
                                BIS_DATE ocr_date = new BIS_DATE();
                                BIS_TIME ocr_time = new BIS_TIME();
                                snd_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                snd_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                snd_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                body.SendDate = snd_date;
                                snd_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                snd_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                snd_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                body.SendTime = snd_time;
                                ocr_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                ocr_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                ocr_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                body.OccurDate = ocr_date;
                                ocr_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                ocr_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                ocr_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                body.OccurTime = ocr_time;
                                body.Route_id = UInt64.Parse(dt.Rows[i][2].ToString());
                                BIS_POS pos = new BIS_POS();

                                double posx = double.Parse(dt.Rows[i][10].ToString());
                                posx *= 100;
                                pos.pos_x = long.Parse(posx.ToString());
                                // pos.pos_y = double.Parse(dt.Rows[i][11].ToString());
                                double posy = double.Parse(dt.Rows[i][11].ToString());
                                posy *= 100;
                                pos.pos_y = long.Parse(posy.ToString());
                                body.Pos = pos;
                                body.heading = ushort.Parse(dt.Rows[i][14].ToString());
                                body.bus_speed = ushort.Parse(dt.Rows[i][9].ToString());
                                body.bnode_id = UInt64.Parse(dt.Rows[i][3].ToString());
                                body.bm_seqno = ushort.Parse(dt.Rows[i][4].ToString());
                                body.dptc_seqno = ushort.Parse(dt.Rows[i][6].ToString());
                                body.travel_time = ushort.Parse(dt.Rows[i][8].ToString());
                                body.sed_failed = char.Parse(dt.Rows[i][7].ToString());  ////
                                                                                         // body.cdma_grade = int.Parse(arr[15]);   //DB값이 null이라 오류,,
                                body.Reserved = new byte[2];
                            }
                            if (Form2.tt == 1)
                            {
                                BIS_DATE snd_date = new BIS_DATE();
                                BIS_TIME snd_time = new BIS_TIME();
                                BIS_DATE ocr_date = new BIS_DATE();
                                BIS_TIME ocr_time = new BIS_TIME();
                                snd_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                snd_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                snd_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                body2.SendDate = snd_date;
                                snd_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                snd_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                snd_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                body2.SendTime = snd_time;
                                ocr_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                ocr_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                ocr_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                body2.OccurDate = ocr_date;
                                ocr_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                ocr_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                ocr_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                body2.OccurTime = ocr_time;
                                body2.Route_id = UInt64.Parse(dt.Rows[i][3].ToString());
                                BIS_POS pos = new BIS_POS();

                                double posx = double.Parse(dt.Rows[i][11].ToString());
                                posx *= 100;
                                pos.pos_x = long.Parse(posx.ToString());
                                double posy = double.Parse(dt.Rows[i][12].ToString());
                                posy *= 100;
                                pos.pos_y = long.Parse(posy.ToString());
                                body2.Pos = pos;

                                body2.heading = ushort.Parse(dt.Rows[i][19].ToString());////////////////////////////////////////
                                body2.bus_speed = ushort.Parse(dt.Rows[i][10].ToString());


                                body2.bnode_id = UInt64.Parse(dt.Rows[i][4].ToString());
                                body2.brn_seqno = ushort.Parse(dt.Rows[i][5].ToString());
                                body2.service_time = ushort.Parse(dt.Rows[i][9].ToString());
                                body2.travel_time = ushort.Parse(dt.Rows[i][8].ToString());
                                body2.nostop = ushort.Parse(dt.Rows[i][15].ToString());
                                body2.fdoor_time = ushort.Parse(dt.Rows[i][13].ToString());
                                body2.bdoor_time = ushort.Parse(dt.Rows[i][14].ToString());
                                body2.in_heading = ushort.Parse(dt.Rows[i][19].ToString());/////////////////////////////////////////////
                                body2.dptc_seqno = ushort.Parse(dt.Rows[i][6].ToString());
                                body2.send_failed = char.Parse(dt.Rows[i][7].ToString()); // BRN_TOTALTIME?
                                                                                          // body2.cdma_grade = int.Parse(arr[20]); 
                                body2.Reserved = new byte[2];
                            }
                            if (Form2.tt == 2)
                            {
                                if (head.opcode == 0x66) //BUSBRNINHISTORY
                                {
                                    BIS_DATE snd_date = new BIS_DATE();
                                    BIS_TIME snd_time = new BIS_TIME();
                                    BIS_DATE ocr_date = new BIS_DATE();
                                    BIS_TIME ocr_time = new BIS_TIME();
                                    snd_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                    snd_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                    snd_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                    body.SendDate = snd_date;
                                    snd_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                    snd_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                    snd_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                    body.SendTime = snd_time;
                                    ocr_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                    ocr_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                    ocr_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                    body.OccurDate = ocr_date;
                                    ocr_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                    ocr_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                    ocr_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                    body.OccurTime = ocr_time;
                                    body.Route_id = UInt64.Parse(dt.Rows[i][3].ToString());
                                    BIS_POS pos = new BIS_POS();

                                    double posx = double.Parse(dt.Rows[i][12].ToString());
                                    posx *= 100;
                                    pos.pos_x = long.Parse(posx.ToString());
                                    // pos.pos_y = double.Parse(dt.Rows[i][11].ToString());
                                    double posy = double.Parse(dt.Rows[i][13].ToString());
                                    posy *= 100;
                                    pos.pos_y = long.Parse(posy.ToString());
                                    body.Pos = pos;
                                    body.heading = ushort.Parse(dt.Rows[i][20].ToString());
                                    body.bus_speed = ushort.Parse(dt.Rows[i][11].ToString());
                                    body.bnode_id = UInt64.Parse(dt.Rows[i][4].ToString());
                                    body.bm_seqno = ushort.Parse(dt.Rows[i][5].ToString());
                                    body.dptc_seqno = ushort.Parse(dt.Rows[i][7].ToString());
                                    body.travel_time = ushort.Parse(dt.Rows[i][9].ToString());
                                    body.sed_failed = char.Parse(dt.Rows[i][8].ToString());  ////
                                                                                             // body.cdma_grade = int.Parse(arr[15]);   //DB값이 null이라 오류,,
                                    body.Reserved = new byte[2];
                                }
                                if (head.opcode == 0x67)                                  //BUSBRNOUTHISTORY
                                {
                                    BIS_DATE snd_date = new BIS_DATE();
                                    BIS_TIME snd_time = new BIS_TIME();
                                    BIS_DATE ocr_date = new BIS_DATE();
                                    BIS_TIME ocr_time = new BIS_TIME();
                                    snd_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                    snd_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                    snd_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                    body2.SendDate = snd_date;
                                    snd_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                    snd_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                    snd_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                    body2.SendTime = snd_time;
                                    ocr_date.yyyy = int.Parse(DateTime.Now.ToString("yyyy"));
                                    ocr_date.MM = ushort.Parse(DateTime.Now.ToString("MM"));
                                    ocr_date.dd = ushort.Parse(DateTime.Now.ToString("dd"));
                                    body2.OccurDate = ocr_date;
                                    ocr_time.hh = ushort.Parse(DateTime.Now.ToString("hh"));
                                    ocr_time.mm = ushort.Parse(DateTime.Now.ToString("mm"));
                                    ocr_time.ss = ushort.Parse(DateTime.Now.ToString("ss"));
                                    body2.OccurTime = ocr_time;
                                    body2.Route_id = UInt64.Parse(dt.Rows[i][3].ToString());
                                    BIS_POS pos = new BIS_POS();

                                    double posx = double.Parse(dt.Rows[i][12].ToString());
                                    posx *= 100;
                                    pos.pos_x = long.Parse(posx.ToString());
                                    double posy = double.Parse(dt.Rows[i][13].ToString());
                                    posy *= 100;
                                    pos.pos_y = long.Parse(posy.ToString());
                                    body2.Pos = pos;

                                    body2.heading = ushort.Parse(dt.Rows[i][20].ToString());////////////////////////////////////////
                                    body2.bus_speed = ushort.Parse(dt.Rows[i][11].ToString());


                                    body2.bnode_id = UInt64.Parse(dt.Rows[i][4].ToString());
                                    body2.brn_seqno = ushort.Parse(dt.Rows[i][5].ToString());
                                    body2.service_time = ushort.Parse(dt.Rows[i][10].ToString());
                                    body2.travel_time = ushort.Parse(dt.Rows[i][9].ToString());
                                    body2.nostop = ushort.Parse(dt.Rows[i][16].ToString());
                                    body2.fdoor_time = ushort.Parse(dt.Rows[i][14].ToString());
                                    body2.bdoor_time = ushort.Parse(dt.Rows[i][15].ToString());
                                    body2.in_heading = ushort.Parse(dt.Rows[i][20].ToString());/////////////////////////////////////////////
                                    body2.dptc_seqno = ushort.Parse(dt.Rows[i][7].ToString());
                                    body2.send_failed = char.Parse(dt.Rows[i][8].ToString()); // BRN_TOTALTIME?
                                                                                              // body2.cdma_grade = int.Parse(arr[20]); 
                                    body2.Reserved = new byte[2];
                                }
                            }
                            TAIL tail = new TAIL();
                            tail.ETX = 0x03;

                            byte bCheckSum = 0x00;

                            List<byte> sendmsg = new List<byte>();

                            sendmsg.AddRange(TcpUtil.ObjectToByte(head));
                            if (Form2.tt == 0)
                                sendmsg.AddRange(TcpUtil.ObjectToByte(body));
                            if (Form2.tt == 1)
                                sendmsg.AddRange(TcpUtil.ObjectToByte(body2));
                            if (Form2.tt == 2)
                            {
                                if (head.opcode == 0x66)
                                {
                                    sendmsg.AddRange(TcpUtil.ObjectToByte(body));
                                }
                                if (head.opcode == 0x67)
                                {
                                    sendmsg.AddRange(TcpUtil.ObjectToByte(body2));
                                }
                            }
                            for (int a = 0; a < sendmsg.Count; a++)
                            {
                                bCheckSum ^= sendmsg[a];
                            }
                            tail.Checksum = bCheckSum;

                            sendmsg.AddRange(TcpUtil.ObjectToByte(tail));

                            while(isbreak == true)
                            {
                                Delay(100);
                            }
                            if (isrun == false)
                                break;

                            var sb = new System.Text.StringBuilder();
                            for (int k = 0; k < sendmsg.Count; k++)
                            {
                                sb.AppendLine(sendmsg[k].ToString());
                            }

                            listBox1.Items.Add(sb.ToString());
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            BISSendMsg(sendmsg.ToArray());
                            Delay(Convert.ToInt32(Form2.delay));
                        }

                        else
                        {
                        
                            ASC_HEAD head = new ASC_HEAD();
                            if (Form2.tt == 0)
                            {
                                head.opcode = cut(2, "66");
                                head.length = cut(4, Marshal.SizeOf<ASC_OP_66>().ToString());
                            }
                            if (Form2.tt == 1)
                            {
                                head.opcode = cut(2, "67");
                                head.length = cut(4, Marshal.SizeOf<ASC_OP_67>().ToString());
                            }
                            if (Form2.tt == 2)
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][14].ToString())) //BUSBRNINHISTORY
                                {
                                    head.opcode = cut(2, "66");
                                    head.length = cut(4, Marshal.SizeOf<ASC_OP_66>().ToString());
                                }
                                else                                  //BUSBRNOUTHISTORY
                                {
                                    head.opcode = cut(2, "67");
                                    head.length = cut(4, Marshal.SizeOf<ASC_OP_67>().ToString());
                                }
                            }
                            head.bid_no = cut(4, dt.Rows[i][0].ToString());

                            ASC_OP_66 body = new ASC_OP_66();
                            ASC_OP_67 body2 = new ASC_OP_67();

                            if (Form2.tt == 0)
                            {
                                ASC_DATE snd_date = new ASC_DATE();
                                ASC_TIME snd_time = new ASC_TIME();
                                ASC_DATE ocr_date = new ASC_DATE();
                                ASC_TIME ocr_time = new ASC_TIME();
                                snd_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                snd_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                snd_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                body.SendDate = snd_date;
                                snd_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                snd_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                snd_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                body.SendTime = snd_time;
                                ocr_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                ocr_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                ocr_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                body.OccurDate = ocr_date;
                                ocr_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                ocr_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                ocr_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                body.OccurTime = ocr_time;
                                body.Route_id = cut(10, dt.Rows[i][2].ToString());
                                ASC_POS pos = new ASC_POS();

                                double posx = double.Parse(dt.Rows[i][10].ToString());
                                posx *= 100;
                                pos.pos_x = cut(8, posx.ToString());

                                double posy = double.Parse(dt.Rows[i][11].ToString());
                                posy *= 100;
                                pos.pos_y = cut(8, posy.ToString());

                                body.Pos = pos;
                                body.heading = cut(3, dt.Rows[i][14].ToString());
                                body.bus_speed = cut(3, dt.Rows[i][9].ToString());
                                body.bnode_id = cut(10, dt.Rows[i][3].ToString());
                                body.brn_seqno = cut(3, dt.Rows[i][4].ToString());
                                body.dptc_seqno = cut(2, dt.Rows[i][6].ToString());
                                body.travel_time = cut(4, dt.Rows[i][8].ToString());
                                //body.bop_stat;  ////
                                // body.cdma_grade = cut(4, dt.Rows[i][15].ToString());   //DB값이 null이라 오류,,
                                // body.Reserved = new byte[2];
                            }

                            if (Form2.tt == 1)
                            {
                                ASC_DATE snd_date = new ASC_DATE();
                                ASC_TIME snd_time = new ASC_TIME();
                                ASC_DATE ocr_date = new ASC_DATE();
                                ASC_TIME ocr_time = new ASC_TIME();
                                snd_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                snd_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                snd_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                body2.SendDate = snd_date;
                                snd_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                snd_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                snd_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                body2.SendTime = snd_time;
                                ocr_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                ocr_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                ocr_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                body2.OccurDate = ocr_date;
                                ocr_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                ocr_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                ocr_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                body2.OccurTime = ocr_time;
                                body2.Route_id = cut(10, dt.Rows[i][3].ToString());
                                ASC_POS pos = new ASC_POS();

                                double posx = double.Parse(dt.Rows[i][10].ToString());
                                posx *= 100;
                                pos.pos_x = cut(8, posx.ToString());
                                double posy = double.Parse(dt.Rows[i][11].ToString());
                                posy *= 100;
                                pos.pos_y = cut(8, posy.ToString());
                                body2.Pos = pos;


                                body2.heading = cut(3, dt.Rows[i][19].ToString());////////////////////////////////////////
                                body2.bus_speed = cut(3, dt.Rows[i][10].ToString());
                                body2.bnode_id = cut(10, dt.Rows[i][4].ToString());
                                body2.brn_seqno = cut(3, dt.Rows[i][5].ToString());
                                body2.service_time = cut(4, dt.Rows[i][9].ToString());
                                body2.travel_time = cut(4, dt.Rows[i][8].ToString());
                                body2.nostop = cut(1, dt.Rows[i][15].ToString());
                                body2.fdoor_time = cut(4, dt.Rows[i][13].ToString());
                                body2.bdoor_time = cut(4, dt.Rows[i][14].ToString());
                                //  body2.in_heading = cut(3, dt.Rows[i][19].ToString());/////////////////////////////////////////////
                                body2.dptc_seqno = cut(2, dt.Rows[i][6].ToString());
                                // body2.send_failed = char.Parse(dt.Rows[i][7].ToString()); // BRN_TOTALTIME?
                                //body2.cdma_grade = cut(4, dt.Rows[i][20].ToString());
                                body2.Reserved = new byte[2];


                            }

                            if (Form2.tt == 2)
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][14].ToString())) //BUSBRNINHISTORY
                                {
                                    ASC_DATE snd_date = new ASC_DATE();
                                    ASC_TIME snd_time = new ASC_TIME();
                                    ASC_DATE ocr_date = new ASC_DATE();
                                    ASC_TIME ocr_time = new ASC_TIME();
                                    snd_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                    snd_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                    snd_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                    body.SendDate = snd_date;
                                    snd_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                    snd_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                    snd_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                    body.SendTime = snd_time;
                                    ocr_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                    ocr_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                    ocr_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                    body.OccurDate = ocr_date;
                                    ocr_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                    ocr_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                    ocr_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                    body.OccurTime = ocr_time;
                                    body.Route_id = cut(10, dt.Rows[i][3].ToString());
                                    ASC_POS pos = new ASC_POS();

                                    double posx = double.Parse(dt.Rows[i][12].ToString());
                                    posx *= 100;
                                    pos.pos_x = cut(8, posx.ToString());
                                    double posy = double.Parse(dt.Rows[i][13].ToString());
                                    posy *= 100;
                                    pos.pos_y = cut(8, posy.ToString());
                                    body.Pos = pos;
                                    body.heading = cut(3, dt.Rows[i][20].ToString());
                                    body.bus_speed = cut(3, dt.Rows[i][11].ToString());
                                    body.bnode_id = cut(10, dt.Rows[i][4].ToString());
                                    body.brn_seqno = cut(3, dt.Rows[i][5].ToString());
                                    body.dptc_seqno = cut(2, dt.Rows[i][7].ToString());
                                    body.travel_time = cut(4, dt.Rows[i][9].ToString());
                                    //body.bop_stat;  ////
                                    // body.cdma_grade = cut(4, dt.Rows[i][15].ToString());   //DB값이 null이라 오류,,
                                    // body.Reserved = new byte[2];
                                }
                                else                                 //BUSBRNOUTHISTORY
                                {
                                    ASC_DATE snd_date = new ASC_DATE();
                                    ASC_TIME snd_time = new ASC_TIME();
                                    ASC_DATE ocr_date = new ASC_DATE();
                                    ASC_TIME ocr_time = new ASC_TIME();
                                    snd_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                    snd_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                    snd_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                    body2.SendDate = snd_date;
                                    snd_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                    snd_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                    snd_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                    body2.SendTime = snd_time;
                                    ocr_date.yyyy = cut(2, DateTime.Now.ToString("yyyy"));
                                    ocr_date.MM = cut(2, DateTime.Now.ToString("MM"));
                                    ocr_date.dd = cut(2, DateTime.Now.ToString("dd"));
                                    body2.OccurDate = ocr_date;
                                    ocr_time.hh = cut(2, DateTime.Now.ToString("hh"));
                                    ocr_time.mm = cut(2, DateTime.Now.ToString("mm"));
                                    ocr_time.ss = cut(2, DateTime.Now.ToString("ss"));
                                    body2.OccurTime = ocr_time;
                                    body2.Route_id = cut(10, dt.Rows[i][3].ToString());
                                    ASC_POS pos = new ASC_POS();

                                    double posx = double.Parse(dt.Rows[i][12].ToString());
                                    posx *= 100;
                                    pos.pos_x = cut(8, posx.ToString());
                                    double posy = double.Parse(dt.Rows[i][13].ToString());
                                    posy *= 100;
                                    pos.pos_y = cut(8, posy.ToString());
                                    body2.Pos = pos;


                                    body2.heading = cut(3, dt.Rows[i][20].ToString());////////////////////////////////////////
                                    body2.bus_speed = cut(3, dt.Rows[i][11].ToString());
                                    body2.bnode_id = cut(10, dt.Rows[i][4].ToString());
                                    body2.brn_seqno = cut(3, dt.Rows[i][5].ToString());
                                    body2.service_time = cut(4, dt.Rows[i][10].ToString());
                                    body2.travel_time = cut(4, dt.Rows[i][9].ToString());
                                    body2.nostop = cut(1, dt.Rows[i][16].ToString());
                                    body2.fdoor_time = cut(4, dt.Rows[i][14].ToString());
                                    body2.bdoor_time = cut(4, dt.Rows[i][15].ToString());
                                    //  body2.in_heading = cut(3, dt.Rows[i][20].ToString());/////////////////////////////////////////////
                                    body2.dptc_seqno = cut(2, dt.Rows[i][7].ToString());
                                    // body2.send_failed = char.Parse(dt.Rows[i][7].ToString()); // BRN_TOTALTIME?
                                    //body2.cdma_grade = cut(4, dt.Rows[i][20].ToString());
                                    body2.Reserved = new byte[2];
                                }
                            }

                            ASC_TAIL tail = new ASC_TAIL();
                            // tail.ETX = 'E';

                            byte bCheckSum = 0x00;

                            List<byte> sendmsg = new List<byte>();


                            sendmsg.AddRange(TcpUtil.ObjectToByte(head));
                            if (Form2.tt == 0)
                                sendmsg.AddRange(TcpUtil.ObjectToByte(body));

                            if (Form2.tt == 1)
                                sendmsg.AddRange(TcpUtil.ObjectToByte(body2));
                            if (Form2.tt == 2)
                            {
                                if (string.IsNullOrEmpty(dt.Rows[i][14].ToString())) //BUSBRNINHISTORY
                                {
                                    sendmsg.AddRange(TcpUtil.ObjectToByte(body));
                                }
                                else                                 //BUSBRNOUTHISTORY
                                {
                                    sendmsg.AddRange(TcpUtil.ObjectToByte(body2));
                                }
                            }
                            for (int a = 0; a < sendmsg.Count; a++)
                            {
                                bCheckSum ^= sendmsg[a];
                            }
                            tail.Checksum = bCheckSum;



                            //    sendmsg.AddRange(TcpUtil.ObjectToByte(tail));


                            while(isbreak == true)
                            {
                                Delay(100);
                            }
                            if (isrun == false) break;

                            var sb = new System.Text.StringBuilder();
                            SendMsg("S");
                            for (int k = 0; k < sendmsg.Count; k++)
                            {

                                sb.AppendLine(sendmsg[k].ToString());
                                SendMsg(sendmsg[k].ToString());
                            }
                            SendMsg(bCheckSum.ToString());
                            SendMsg("E");


                            listBox1.Items.Add(sb.ToString());
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;

                            Delay(Convert.ToInt32(Form2.delay));

                        }
                    }
                }
            }
        }

        private void SendMsg(string msg)
        {
            try
            {
                if(tc.Connected == true)
                {
                    string a = msg;
                    byte[] sendbuf = Encoding.ASCII.GetBytes(a);
                    NetworkStream stream = tc.GetStream();
                    stream.Write(sendbuf, 0, sendbuf.Length);
                }
            }
            catch (Exception ex)
            {
                isrun = false;
                tc.Close();
                MessageBox.Show(ex.Message);
            }
        }
        private void BISSendMsg(byte[] msg)
        {

            try
            {
                if (tc.Connected == true)
                {
                    isrun = true;

                    byte[] sendbuf = msg;

                    NetworkStream stream = tc.GetStream();
                    stream.Write(sendbuf, 0, sendbuf.Length);
                    //Delay(500);
                    // Delay(Convert.ToInt32(Form2.delay));

                }
            }

            catch (Exception ex)
            {
                isrun = false;
                tc.Close();
                MessageBox.Show(ex.Message);
            }
        }
        public static char[] cut(int a, string table)
        {
            char[] result = new char[a];
            uint temp = uint.Parse(table);
            for(int i = 0; i < a; a--)
            {
                result[a - 1] = Convert.ToChar(temp % 10);
                temp = temp / 10;
            }
            return result;
        }
        
        private bool Connect()
        {
            bool bResult = false;
            try
            {
                tc = new TcpClient(strip, Convert.ToInt32(strport));
                bResult = tc.Connected;
                if (tc.Connected)
                {
                    isrun = true;
                    ns1 = tc.GetStream();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");

            }

            return bResult;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isrun == true)
            {
                picBoxDBState.Image = Properties.Resources.On;
            }
            else
            {
                picBoxDBState.Image = Properties.Resources.Off;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isbreak == true)
                isbreak = false;
            else
                isbreak = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sql = null;
            dt.Clear();
            listBox1.Items.Clear();
            tc.Close();
            isrun = false;
            isbreak = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        public static byte[] ToByteArray(String hex)
        {
            byte[] bytes = null;
            bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
            button1.Enabled = true;
        }
        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        
    }
}
