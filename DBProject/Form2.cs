using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
namespace DBProject
{
    public partial class Form2 : Form
    {

        public static int sel; // 전송 종류 선택
        public static int tt; // 데이터 선택
        public static int asc; // ascii or binary
        public static string bbus;

        public ArrayList nbus = new ArrayList();
        public ArrayList nbbus = new ArrayList();
        int i;
        Boolean checkAll = true;
        public Form2()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    nbus.Add(i);
                    continue;
                }
            }

            for (i = 0; i < nbus.Count; i++)
            {
                nbbus.Add(Form1.busnum[Convert.ToInt32(nbus[i])]);
            }

            int z = 0;
            for(i = 0; i < nbbus.Count; i++)
            {
                if (z == 0)
                {
                    bbus += nbbus[i];
                    z++;
                }
                else bbus = bbus + ", " + nbbus[i];


            }

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            checkedListBox1.Items.Clear();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            
            bbus = null;
            for (int i = 0; i < Form1.busnum.Count; i++)
            {
                checkedListBox1.Items.Add(Form1.busnum[i]);
            }

            comboBox1.Items.Add("기본 전송");
            comboBox1.Items.Add("시간별 전송");
            comboBox1.Items.Add("버스번호별 전송");
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("BUSBRNINHISTORY");
            comboBox2.Items.Add("BUSBRNOUTHISTORY");
            comboBox2.Items.Add("둘다 전송");
            comboBox2.SelectedIndex = 0;

            comboBox3.Items.Add("Binary 전송");
            comboBox3.Items.Add("ASCII 전송");
            comboBox3.SelectedIndex = 0;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sel = comboBox1.SelectedIndex;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tt = comboBox2.SelectedIndex;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            asc = comboBox3.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkAll)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, true);
                checkAll = false;
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                checkAll = true;
            }
        }

        
    }
}
