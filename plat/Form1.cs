using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using ProFactory;
using ProOperator;

namespace plat
{
    public partial class Form1 : Form
    {
        double[] z = new double[59];
        double[] v = new double[59];
        double[] dq = new double[259];
        double[] dz = new double[259];
        double[,] q = new double[71, 12];
        double cz;
        double mz;
        int npoint;
        int time;
        double maxl;
        double maxQ;
        double minQ;
        double K;
        double maxZ;
        double minZ;
        double maxN;
        double minN;
        public DataTable dt0;
        double[] chuli = new double[12];
        double[] qishui1 = new double[12];
        double[] route1 = new double[13];

        public Form1()
        {
            InitializeComponent();
            //获取dll全路径
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll");
            string[] files = Directory.GetFiles(path);
            //将算法名称添加到listProgram中
            foreach (string item in files)
            {
                listProgram.Items.Add(Path.GetFileName(item));
            }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            FormDP frm = new FormDP(Tochange);
            frm.Show();

        }
        /// <summary>
        /// 定义一个函数利用委托将FormDP的值传递到Form1中
        /// </summary>
        /// <param name="fcz"></param>
        /// <param name="fmz"></param>
        /// <param name="fnpoint"></param>
        /// <param name="ftime"></param>
        /// <param name="fmaxl"></param>
        /// <param name="fmaxv"></param>
        /// <param name="fminv"></param>
        public void Tochange(double[] fz, double[] fv, double[] fdq, double[] fdz, double[,] fq, double fcz, double fmz, int fnpoint, int ftime, double fmaxl, double fmaxQ, double fminQ, double fK, double fmaxZ, double fminZ, double fmaxN, double fminN)
        {
            for (int i = 0; i < 59; i++)
            {
                z[i] = fz[i];
                v[i] = fv[i];
            }
            for (int i = 0; i < 259; i++)
            {
                dq[i] = fdq[i];
                dz[i] = fdz[i];
            }
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 71; j++)
                {
                    q[j, i] = fq[j, i];
                }

            }
            cz = fcz;
            mz = fmz;
            npoint = fnpoint;
            time = ftime;
            maxl = fmaxl;
            maxQ = fmaxQ;
            minQ = fminQ;
            K = fK;
            maxZ = fmaxZ;
            minZ = fminZ;
            maxN = fmaxN;
            minN = fminN;

        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            string file = null;
            //导入Excl表格
            openfile.InitialDirectory = "..\\datebase";
            openfile.Filter = "csv|*.csv|xls|*.xls";
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                file = openfile.FileName;
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            Operation oper = factory.GetOpre("DP");

            oper.InExcel(z, v, dz, dq, q);
           
            oper.Input(cz,mz,npoint,time,maxl,K,maxN,minN,maxZ,minZ,chuli,qishui1,route1,maxQ,minQ);
            oper.calculate();
            double i = oper.NNmax;
            MessageBox.Show(Convert.ToString(i));
        }
    }
}
