using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plat
{
     public delegate void DelText(double[] z,double[] v,double[] dq,double[] dz,double[,] q, double cz,double mz,int npoint, int time,double maxl,double maxQ,double minQ,double K,double maxZ,double minZ,double maxN,double minN);
    public partial class FormDP : Form
    {
        private DelText _del;
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
        
        public FormDP(DelText del)
        {
            InitializeComponent();
            cmbTime.Items.Clear();
            for (int i = 1; i < 72; i++)
            {
                cmbTime.Items.Add("第" + i.ToString() + "年");
            }
            cmbTime.SelectedIndex = 0;
            this._del = del;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double ucz = Convert.ToDouble(txtCZ.Text.Trim());
            double umz = Convert.ToDouble(txtMZ.Text.Trim());
            int unpoint = Convert.ToInt32(txtPoint.Text.Trim());
            int utime = cmbTime.SelectedIndex + 1;
            double umaxl = Convert.ToDouble(txtMaxl.Text.Trim());
            double umaxQ = Convert.ToDouble(txtMaxQ.Text.Trim());
            double uminQ = Convert.ToDouble(txtMinQ.Text.Trim());
            double uK = Convert.ToDouble(txtK.Text.Trim());
            double umaxZ = Convert.ToDouble(txtMaxZ.Text.Trim());
            double uminZ = Convert.ToDouble(txtMinZ.Text.Trim());
            double umaxN = Convert.ToDouble(txtMaxN.Text.Trim());
            double uminN =Convert.ToDouble(txtMinN.Text.Trim());
            double[] uz = new double[59] ;
            double[] uv= new double[59];
            double[] udq=new double[259];
            double[] udz=new double[259];
            double[,] uq = new double[71,12];
            for (int i = 0; i <59 ; i++)
            {
                uz[i] = Convert.ToDouble(dt1.Rows[i][0].ToString());
                uv[i] = Convert.ToDouble(dt1.Rows[i][1].ToString());
            }
            for (int i = 0; i < 259; i++)
			{
			 udq[i]= Convert.ToDouble(dt1.Rows[i][3].ToString());
             udz[i] = Convert.ToDouble(dt1.Rows[i][4].ToString());
			}
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 71; j++)
                {
                    uq[j, i] = Convert.ToDouble(dt2.Rows[j][i + 1].ToString());
                }
                
            }
            this._del(uz,uv,udq,udz,uq,ucz, umz, unpoint, utime, umaxl, umaxQ, uminQ,uK,umaxZ,uminZ,umaxN,uminN);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.InitialDirectory = "..\\数据";
            openFileDialog1.Filter = "allfile|*.*|csv|*.csv";
            openFileDialog2.InitialDirectory = "..\\数据";
            openFileDialog2.Filter = "allfile|*.*|csv|*.csv";

            MessageBox.Show("读取第一组数据");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                dt1 = InputCSV(openFileDialog1.FileName);
            }
           MessageBox.Show("读取第二组数据");
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {

                dt2 = InputCSV(openFileDialog2.FileName);
            }
          
         }
        public DataTable InputCSV(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)    //有一个赋值语句
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)      //第一列
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }      
    }
}
