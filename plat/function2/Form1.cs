using plat.function2.function2;
using ProFactory;
using ProOperator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace plat
{
    public partial class Form1 : Form
    {
        private Assembly ass;     //Assembly集合
        private List<double> Basic_kurong = new List<double>();
        private double[,] Basic_laishui;
        private int Basic_maxchukuliuliang;
        private int Basic_maxshuilunji;
        private int Basic_minchukuliuliang;
        private double Basic_N1;
        private double Basic_N2;
        private int Basic_shijian;
        private List<double> Basic_shuiwei = new List<double>();

        //List<List<double>> Basic_laishui = new List<List<double>>();
        private int Basic_shujugeshu;

        private List<double> Basic_xiaxieliuliang = new List<double>();

        //double[] Basic_shuiwei = new double[59];
        //double[] Basic_kurong = new double[59];
        //double[] Basic_xiayoushuiwei = new double[259];
        //double[] Basic_xiaxieliuliang = new double[259];
        //double[,] Basic_laishui = new double[71, 12];
        private List<double> Basic_xiayoushuiwei = new List<double>();

        private DataTable dt1 = new DataTable();

        //分配内存dt1
        private DataTable dt2 = new DataTable();

        private MethodInfo Input;
        private string modelname;
        private object obj;
        private MethodInfo Output;
        private List<string> str = new List<string>();
        private Type type;

        public Form1()   //构造函数
        {
            InitializeComponent();
            abc();
            demo.BringToFront();
            this.StartPosition = FormStartPosition.CenterScreen;   //显示窗体在屏幕中心
            //string[] a= INIOperationClass.INIGetAllSectionNames(DataLayer.cfgFilePath);
            //listBox3.Items.AddRange(INIOperationClass.INIGetAllSectionNames(DataLayer.cfgFilePath));   //调用了一个类 INIOperationClass   获取所有节点名字
        }

        //分配内存dt2
        public void abc()
        {
            dataGridView3.Rows.Add("初水位(m)");
            dataGridView3.Rows.Add("末水位(m)");
            dataGridView3.Rows.Add("库容(百万m³)");
            dataGridView3.Rows.Add("(弃水m3/s)");
            dataGridView3.Rows.Add("出力（MW）");
            dataGridView3.Rows.Add("电量（MW。h）");
            dataGridView3.Rows.Add("出库m3/s");
            dataGridView3.Rows.Add("来水");
        }   //定义属性

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

        private void button1_Click(object sender, EventArgs e)   //计算按钮展示结果
        {
            Operation oper = factory.GetOpre(modelname);
            oper.input(Basic_shuiwei, Basic_kurong, Basic_xiayoushuiwei, Basic_xiaxieliuliang, Basic_laishui, Basic_shijian, Basic_shujugeshu,
                Basic_maxchukuliuliang, Basic_minchukuliuliang, Basic_maxshuilunji, Basic_N1, Basic_N2);
            for (int i = 0; i < 12; i++)   //写初始水位
            {
                dataGridView3.Rows[0].Cells[i + 1].Value = (oper.route1[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++)   //写末端水位
            {
                dataGridView3.Rows[1].Cells[i + 1].Value = (oper.route1[i + 1]).ToString("f2");
            }
            for (int i = 0; i < 12; i++)   //写库容
            {
                dataGridView3.Rows[2].Cells[i + 1].Value = (oper.V[i].ToString());
            }
            for (int i = 0; i < 12; i++)   //写弃水
            {
                dataGridView3.Rows[3].Cells[i + 1].Value = (oper.qishui1[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写出力
            {
                dataGridView3.Rows[4].Cells[i + 1].Value = oper.chuli[i].ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写电量
            {
                dataGridView3.Rows[5].Cells[i + 1].Value = (oper.chuli[i] * 30.5 * 24).ToString("f2");
            }
            double[] chuku = new double[12];
            for (int i = 0; i < 12; i++)   //写出库
            {
                dataGridView3.Rows[6].Cells[i + 1].Value = (oper.chuku[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写电量
            {
                dataGridView3.Rows[7].Cells[i + 1].Value = oper.laishui[oper.shijian - 1, i].ToString("f2");
            }

            int[] X = new int[13];
            for (int i = 0; i < 13; i++)      //绘制折线图，初始化X值
            {
                X[i] = i;
            }
            textBox8.Text = (oper.NNmax * 30.5 * 24).ToString("f2");
            // chart1.Series[0].Points.DataBindXY(X, route1.ToString("f2"));
            // chart1.Series[1].Points.DataBindY(chuli); //绘制折线图
            chart1.Series["Series1"].Points.Clear();
            for (int i = 0; i < 13; i++)
            {
                chart1.Series["Series1"].Points.AddXY(i, oper.route1[i].ToString("f2"));
            }
            chart1.Series["Series2"].Points.Clear();
            for (int i = 0; i < 12; i++)
            {
                chart1.Series["Series2"].Points.AddXY(i + 1, oper.chuli[i].ToString("f2"));
            }
        }

        private void button2_Click(object sender, EventArgs e)   //d读入数据
        {
            openFileDialog1.InitialDirectory = "..\\datebase";
            openFileDialog1.Filter = "allfile|*.*|csv|*.csv";
            openFileDialog2.InitialDirectory = "..\\datebase";
            openFileDialog2.Filter = "allfile|*.*|csv|*.csv";

            MessageBox.Show("现已将数据读入进去");
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    dt1 = InputCSV(openFileDialog1.FileName);
            //}
            dt1 = InputCSV(@"..\Debug\date\Data11.csv");  //读取第二个表的数据
            dataGridView1.DataSource = dt1;
            // MessageBox.Show("再将第二组数据读入进去");
            //if (openFileDialog2.ShowDialog() == DialogResult.OK)
            //{
            //    dt2 = InputCSV(openFileDialog2.FileName);
            //}
            dt2 = InputCSV(@"..\Debug\date\Data22.csv");  //读取第二个表的数据
            dataGridView2.DataSource = dt2;

            Basic_shujugeshu = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox4.Text)).ToString());
            Basic_maxchukuliuliang = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox6.Text)).ToString());
            Basic_minchukuliuliang = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox7.Text)).ToString());
            Basic_maxshuilunji = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox5.Text)).ToString());
            Basic_N1 = Convert.ToDouble(Math.Floor(Convert.ToDecimal(this.textBox3.Text)).ToString());    //初水位
            Basic_N2 = Convert.ToDouble(Math.Floor(Convert.ToDecimal(this.textBox2.Text)).ToString());    //末水位

            // openFileDialog1.ShowDialog();
            // DataTable dt1 = InputCSV1(openFileDialog1.FileName);   //读取第一个表的数据
            // DataTable dt2 = InputCSV2(openFileDialog2.FileName);
            // DataTable dt2 = InputCSV2("C:\\Users\\Jun\\Desktop\\Data111.csv");  //读取第二个表的数据
            // MessageBox.Show(dt1.Rows[3][3].ToString());             //监测数据有没有写到dt中

            //读取数据数组中

            for (int i = 0; i < 59; i++)
            {
                Basic_shuiwei.Add(Convert.ToDouble(dt1.Rows[i][0].ToString()));
                Basic_kurong.Add(Convert.ToDouble(dt1.Rows[i][1].ToString()));
            }
            // MessageBox.Show(dt1.Rows.Count.ToString());
            for (int i = 0; i < 259; i++)
            {
                Basic_xiayoushuiwei.Add(Convert.ToDouble(dt1.Rows[i][3].ToString()));
                Basic_xiaxieliuliang.Add(Convert.ToDouble(dt1.Rows[i][4].ToString()));
            }

            Basic_laishui = new double[dt2.Rows.Count, dt2.Columns.Count];
            for (int i = 0; i < dt2.Columns.Count - 1; i++)
            {
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Basic_laishui[j, i] = Convert.ToDouble(dt2.Rows[j][i + 1].ToString());      //把来水写到一个数组中
                    // MessageBox.Show(shuiwei[2].ToString());      //监测数据有没有写进去
                }
            }

            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    List<double> temp_laishui = new List<double>();
            //    for (int j = 0; j < dt2.Columns.Count; j++)   //result是2行11列的数组
            //    {
            //        temp_laishui.Add(Convert.ToDouble(dt2.Rows[i][j +1].ToString()));
            //    }
            //    Basic_laishui.Add(temp_laishui);
            //}
            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        Console.Write(arrary[i][j] + "\t");
            //    }
            //    Console.WriteLine("");
            //}
        }

        private void button3_Click(object sender, EventArgs e)    //1点击确定选择DLL文件，一次只能选择一个电站
        {
            string selectedModel_1 = listBox1.SelectedItem.ToString();
            //groupBox3.Text = selectedModel_1 + "模型输入";
            //groupBox3.Controls.Clear();
            modelname = selectedModel_1.Replace(".dll", "");
            listBox3.Items.Add(modelname);
            //MessageBox.Show(listBox3.);
        }

        private void button4_Click(object sender, EventArgs e)   //切换到DP算法
        {
            //DP.BringToFront();
            //Short_plan Short_Plan = new Short_plan();
            //Short_Plan.Show();
            //comboBox1.Items.Add();

            str.Clear();
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                str.Add(listBox3.ValueMember);
            }
            string path = @"C:\Users\HuJun123\Desktop\plat\bin\数据\TextFile1.txt";
            StreamWriter write = new StreamWriter("s.txt", true, Encoding.Default);

            write.WriteLine(string.Join(",", str));
            this.Close();
        }

        //todo
        private void button6_Click(object sender, EventArgs e)   //2点击确定选择DLL文件
        {
            //    Operation oper = factory.GetOpre(modelname);
            //    oper.input(Basic_shuiwei, Basic_kurong, Basic_xiayoushuiwei, Basic_xiaxieliuliang, Basic_laishui, Basic_shijian, Basic_shujugeshu,
            //        Basic_maxchukuliuliang, Basic_minchukuliuliang, Basic_maxshuilunji, Basic_N1, Basic_N2);

            //    for (int i = 0; i < 12; i++)   //写初始水位
            //    {
            //        dataGridView3.Rows[0].Cells[i + 1].Value = (oper.route1[i]).ToString("f2");
            //    }
            //    //Input = type.GetMethod("Input");
            //    //Output = type.GetMethod("Output");
        }

        private void button7_Click(object sender, EventArgs e)  //添加DLL
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.IO.Path.GetFullPath(DataLayer.modelDLLPath);//相对路径转绝对路径
            ofd.Filter = "算法动态链接库(*.dll)|*.dll";//ofd.Filter = "Text Document(*.txt)|*.txt|All Files|*.*|我要显示的文件类型(*.exe)|*.exe";
            ofd.ShowDialog();
            string modelname = ofd.SafeFileName.Replace(".dll", "");
            //cfgForm cfg_form = new cfgForm(modelname);
            //cfg_form.ShowDialog();//用show()则close后原窗口操作未同步过来
            Form1 fm_1 = new Form1();
            fm_1.ShowDialog();

            //关闭cfg窗口后，应该刷新listbox的显示
            //listBox3.Items.Clear();
            //listBox3.Items.AddRange(INIOperationClass.INIGetAllSectionNames(DataLayer.cfgFilePath));
        }

        //把数据读入进去并赋值
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  //选择确定计算第几年
        {
            Basic_shijian = comboBox1.SelectedIndex + 1;
        }

        private void Form1_Load(object sender, EventArgs e)   //窗口加载之前的数据准备
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll");
            string[] files = Directory.GetFiles(path);
            foreach (string item in files)
            {
                listBox1.Items.Add(Path.GetFileName(item));
            }

            comboBox1.Items.Clear();
            for (int i = 1; i < 72; i++)
            {
                comboBox1.Items.Add("第" + i.ToString() + "年");
            }
            comboBox1.SelectedIndex = 0;
        }

        //读取EXCEL文件函数
    }
}