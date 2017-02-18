using Oracle.ManagedDataAccess.Client;
using plat.function2.function2;
using ProFactory;
using ProOperator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace plat.function2.Short_plan
{
    public partial class Frame_ShortPlan : UserControl
    {
        public int ProcessState = 0;//进程状态0：条件设置1：数据准备2：优化计算
        public List<Station_1> station_RT = new List<Station_1>();
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
        private List<double> Basic_xiayoushuiwei = new List<double>();
        private string conStr = "Data Source=502Share-pc/orcl;User ID =interbs_16;Password=interbs_16;";
        private DataTable dt = new DataTable();
        private DataTable dt1 = new DataTable();

        //分配内存dt1
        private DataTable dt2 = new DataTable();

        private string modelname;
        //分配内存dt2

        public Frame_ShortPlan()
        {
            InitializeComponent();
            DataPrepareUI_Initial();
            abc();
        }

        public void abc()
        {
            dgv_dataresult.Rows.Add("初水位(m)");
            dgv_dataresult.Rows.Add("末水位(m)");
            dgv_dataresult.Rows.Add("库容(百万m³)");
            dgv_dataresult.Rows.Add("(弃水m3/s)");
            dgv_dataresult.Rows.Add("出力（MW）");
            dgv_dataresult.Rows.Add("电量（MW。h）");
            dgv_dataresult.Rows.Add("出库m3/s");
            dgv_dataresult.Rows.Add("来水");
        }

        public void DataPrepareUI_Initial()
        {
            using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
            {
                con.Open();//打开数据库连接
                //and t.riverid=140222
                string sql = "select * from STATIONINFO t where t.inuse=1  order by t.stationid";
                //todo:默认只读取湘江流域的可用电站
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(dt);
                con.Close();//关闭数据库连接
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Station_1 tempstation = new Station_1();
                //Station_1 station_RT = new Station_1();
                //station_RT[i].stationid = Convert.ToInt32(dt.Rows[i]["stationid"]);
                //station_RT[i].stationname = dt.Rows[i]["stationname"].ToString();

                tempstation.stationid = Convert.ToInt32(dt.Rows[i]["stationid"]);
                tempstation.stationname = dt.Rows[i]["stationname"].ToString();
                station_RT.Add(tempstation);
            }


            treeView_stationview.Nodes.Clear();
            for (int i = 0; i < station_RT.Count; i++)
            {
                treeView_stationview.Nodes.Add(station_RT[i].stationname);
                // treeView_stationview.Nodes[i].Checked = true;
            }


            treeView_stationchart.Nodes.Clear();
            for (int i = 0; i < station_RT.Count; i++)
            {
                treeView_stationchart.Nodes.Add(station_RT[i].stationname);
                // treeView_stationview.Nodes[i].Checked = true;
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

        private void button_chart_Click(object sender, EventArgs e)
        {
            //流域图显示

            tableLayoutPanel_DataResult.RowStyles[1].Height = 0;
            panel_stationviewbutton.Visible = false;
            treeView_stationchart.BringToFront();

            button_recover.Visible = false;
            button_simu.Visible = false;

            panel_chart.BringToFront();
            //chart_allriver.BringToFront();
            //chart_pie.BringToFront();
        }

        private void button_DPAll_Click(object sender, EventArgs e)   //全选
        {
            for (int i = 0; i < treeView_stationview.Nodes.Count; i++)
            {
                //for (int j = 0; j < treeView_stationview.Nodes[i].Nodes.Count; j++)
                //{
                //    treeView_stationview.Nodes[i].Nodes[j].Checked = true;
                //}
                treeView_stationview.Nodes[i].Checked = true;
            }
        }

        private void button_DPCon_Click(object sender, EventArgs e)   //反选
        {
            for (int i = 0; i < treeView_stationview.Nodes.Count; i++)
            {
                //for (int j = 0; j < treeView_stationview.Nodes[i].Nodes.Count; j++)
                //{
                //    treeView_stationview.Nodes[i].Nodes[j].Checked = !treeView_stationview.Nodes[i].Nodes[j].Checked;
                //}
                treeView_stationview.Nodes[i].Checked = !treeView_stationview.Nodes[i].Checked;
            }
        }

        private void button_grid_Click(object sender, EventArgs e)
        {
            panel_dataresult.BringToFront();

            //结果表格显示
            tableLayoutPanel_DataResult.RowStyles[1].Height = 225;
            panel_stationviewbutton.Visible = true;
            treeView_stationview.BringToFront();

            button_recover.Visible = true;
            button_simu.Visible = true;
            // grid_out.BringToFront();
            // tableLayoutPanel_Result.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)    //选择
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

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedModel_1 = listBox1.SelectedItem.ToString();
            //groupBox3.Text = selectedModel_1 + "模型输入";
            //groupBox3.Controls.Clear();
            string tempmodelname;
            tempmodelname = selectedModel_1.Replace(".dll", "");
            listBox2.Items.Add(tempmodelname);
            //MessageBox.Show(listBox3.);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selectedModel_2 = listBox2.SelectedItem.ToString();
            //groupBox3.Text = selectedModel_1 + "模型输入";
            //groupBox3.Controls.Clear();
            modelname = selectedModel_2;
            MessageBox.Show("您已选择" + modelname + "算法");

            //DP.BringToFront();
            //Short_plan Short_Plan = new Short_plan();
            //Short_Plan.Show();
            //comboBox1.Items.Add();

            //str.Clear();
            //for (int i = 0; i < listBox2.Items.Count; i++)
            //{
            //    str.Add(listBox3.ValueMember);
            //}
            //string path = @"C:\Users\HuJun123\Desktop\plat\bin\数据\TextFile1.txt";
            //StreamWriter write = new StreamWriter("s.txt", true, Encoding.Default);

            //write.WriteLine(string.Join(",", str));

            //this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Basic_shijian = comboBox1.SelectedIndex + 1;
        }

        private void Frame_ShortPlan_Load(object sender, EventArgs e)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll");

            //openFileDialog2.Filter = "allfile|*.*|csv|*.csv";

            string[] files = Directory.GetFiles(path);
            foreach (string item in files)
            {
                if (item.Last() == 'l')
                {
                    listBox1.Items.Add(Path.GetFileName(item));
                }
            }

            comboBox1.Items.Clear();
            for (int i = 1; i < 72; i++)
            {
                comboBox1.Items.Add("第" + i.ToString() + "年");
            }
            comboBox1.SelectedIndex = 0;
        }

        private void linkLabel_Calc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //splashScreenManager_short.ShowWaitForm();
            //splashScreenManager_short.SetWaitFormCaption("请稍后");
            //splashScreenManager_short.SetWaitFormDescription("正在制作计划");
            //优化计算
            if (ProcessState == 0)
            {
                //getDatafromConditionSetUI();
                //getPrePareDatafromDB();
                DataPrepareUI_Initial();

                // int temp = model.makeplan();
                //if (temp < 0)
                //{
                //    MessageBox.Show("无解，请检查初始条件！");
                //    return;
                //}

                //  DataResultUI_Initial();

                ProcessState = 2;
            }
            else if (ProcessState == 1)
            {
                //int temp = model.makeplan();
                //if (temp < 0)
                //{
                //    MessageBox.Show("无解，请检查初始条件！");
                //    return;
                //}

                //  DataResultUI_Initial();
                ProcessState = 2;
            }
            // splashScreenManager_short.CloseWaitForm();
            //结果表格显示
            tableLayoutPanel_DataResult.RowStyles[1].Height = 225;
            panel_stationviewbutton.Visible = true;
            treeView_stationview.BringToFront();
            // grid_out.BringToFront();
            tableLayoutPanel_Result.BringToFront();

            tableLayoutPanel_DataResult.BringToFront();
            //高亮当前标签
            linkLabel_Prepare.LinkColor = Color.Black;
            linkLabel_Prepare.Font = new Font("微软雅黑", 13, FontStyle.Regular);
            linkLabel_Data.LinkColor = Color.Black;
            linkLabel_Data.Font = new Font("微软雅黑", 13, FontStyle.Regular);
            linkLabel_Calc.LinkColor = SystemColors.MenuHighlight;
            linkLabel_Calc.Font = new Font("微软雅黑", 14, FontStyle.Bold);

            Basic_shujugeshu = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox4.Text)).ToString());
            Basic_maxchukuliuliang = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox6.Text)).ToString());
            Basic_minchukuliuliang = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox7.Text)).ToString());
            Basic_maxshuilunji = Convert.ToInt16(Math.Floor(Convert.ToDecimal(this.textBox5.Text)).ToString());
            Basic_N1 = Convert.ToDouble(Math.Floor(Convert.ToDecimal(this.textBox3.Text)).ToString());    //初水位
            Basic_N2 = Convert.ToDouble(Math.Floor(Convert.ToDecimal(this.textBox2.Text)).ToString());    //末水位

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

            //数据结果展示

            Operation oper = factory.GetOpre(modelname);
            oper.input(Basic_shuiwei, Basic_kurong, Basic_xiayoushuiwei, Basic_xiaxieliuliang, Basic_laishui, Basic_shijian, Basic_shujugeshu,
                Basic_maxchukuliuliang, Basic_minchukuliuliang, Basic_maxshuilunji, Basic_N1, Basic_N2);
            for (int i = 0; i < 12; i++)   //写初始水位
            {
                dgv_dataresult.Rows[0].Cells[i + 1].Value = (oper.route1[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++)   //写末端水位
            {
                dgv_dataresult.Rows[1].Cells[i + 1].Value = (oper.route1[i + 1]).ToString("f2");
            }
            for (int i = 0; i < 12; i++)   //写库容
            {
                dgv_dataresult.Rows[2].Cells[i + 1].Value = (oper.V[i].ToString());
            }
            for (int i = 0; i < 12; i++)   //写弃水
            {
                dgv_dataresult.Rows[3].Cells[i + 1].Value = (oper.qishui1[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写出力
            {
                dgv_dataresult.Rows[4].Cells[i + 1].Value = oper.chuli[i].ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写电量
            {
                dgv_dataresult.Rows[5].Cells[i + 1].Value = (oper.chuli[i] * 30.5 * 24).ToString("f2");
            }
            double[] chuku = new double[12];
            for (int i = 0; i < 12; i++)   //写出库
            {
                dgv_dataresult.Rows[6].Cells[i + 1].Value = (oper.chuku[i]).ToString("f2");
            }
            for (int i = 0; i < 12; i++) // 写电量
            {
                dgv_dataresult.Rows[7].Cells[i + 1].Value = oper.laishui[oper.shijian - 1, i].ToString("f2");
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

        /// <summary>
        /// 
        /// </summary>
        public void GetSelectedTreeview()
        {
            List<string> listName = new List<string>();
            List<int> listStationID = new List<int>();
            List<DataTable> dt_all = new List<DataTable>();
            DataTable tempdt = new DataTable();
            
            for (int i = 0; i < station_RT.Count; i++)
            {

                if (treeView_stationview.Nodes[i].Checked==true)
                {
                  
                    listName.Add(treeView_stationview.Nodes[i].Text.ToString());
                }
            }
            

            if(listName.Count==0)
            {

                MessageBox.Show("您没有选择电站，请重新选择！");
            }



           //listStationID
            //stationName = StationNamecb.SelectedItem.ToString();
            //string dtName = "dt";
            for (int i = 0; i < listName.Count; i++)
            {
                 DataTable dt = new DataTable();
                string sql = "select stationid from STATIONINFO where stationname='" + listName[i].ToString() + "' ";
                OracleConnection con = new OracleConnection(conStr);
                con.Open();//打开数据库连接
                OracleDataAdapter da = new OracleDataAdapter();
                da = new OracleDataAdapter(sql, con);
                da.Fill(dt);
                con.Close();//关闭数据库连
                listStationID.Add(Convert.ToInt32(dt.Rows[0]["STATIONID"]));

            }

            

            for (int i = 0; i < listStationID.Count; i++)
            {
                using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
                {
                   
                    con.Open();//打开数据库连接
                    string sql = "select areainflow , maxz , minz ,maxn , minn ,maxr ,minr,stationid from defaultcondition_time  where stationid ='" + listStationID[i]+ "' ";
                    //todo:默认只读取湘江流域的可用电站
                    OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                    adapter.Fill(tempdt);
                    con.Close();//关闭数据库连接
                }
                dt_all.Add(tempdt);
            }

            //确定行数和列数
            int rows = dt_all[0].Rows.Count+5 ;
            int count = 0;
            //表格 样式
            Grid1.EnableSort = true;
            for (int i = 0; i < dt_all.Count; i++)
            {
                count += 2 + 7 * dt_all.Count;
            }

            Grid1.Redim(rows + 5, count);//分别对应行和列。。。

            Grid1.BackColor = Color.White;
            Grid1.FixedRows = 5;
            Grid1.FixedColumns = 1;
            Grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            //内容单元格 样式Views
            plat.function2.Short_plan.MySourceGridStyle.NormalCellAlternateBackColorView viewNormal = new plat.function2.Short_plan.MySourceGridStyle.NormalCellAlternateBackColorView();
            //第一列表头 样式Views
            plat.function2.Short_plan.MySourceGridStyle.FirstColumnHeaderView viewFirstColumn = new plat.function2.Short_plan.MySourceGridStyle.FirstColumnHeaderView();
            //电站行表头 样式Views
            plat.function2.Short_plan.MySourceGridStyle.StationRowHeaderView viewStationRow = new plat.function2.Short_plan.MySourceGridStyle.StationRowHeaderView();

            //Header Row
            Grid1[0, 0] = new MyHeader("流域|电站");
            Grid1[0, 0].RowSpan = 2;

            Grid1[0, 1] = new MyHeader("全流域");
            Grid1[0, 1].RowSpan = 2;

            Grid1[0, 2] = new MyHeader("湘江流域");
            Grid1[0, 2].ColumnSpan = 7 * dt_all.Count;

            // string[] stationNames = new string[] { "双牌", "南津渡" };
           // string[] TempName1 = new string();

            //最后一行的表头
            Grid1[2, 0] = new MyHeaderBottomFixedRow("时间|数据");
            Grid1[2, 0].RowSpan = 3;

            Grid1[2, 1] = new MyHeader("负荷趋势");
            Grid1[2, 1].RowSpan = 2;
            for (int i = 0; i < dt_all.Count; i++)
            {
                //grid1[1, 2 + i * 7] = new MyHeader(stationNames[i]);
                //用Cell取代MyHeader格式，为了电站名对应单元格不可用鼠标拖动
                Grid1[1, 2 + i * 7] = new SourceGrid.Cells.Cell(listName[i]);
                Grid1[1, 2 + i * 7].View = viewStationRow;

                Grid1[1, 2 + i * 7].ColumnSpan = 7;

                Grid1[2, 2 + i * 7] = new MyHeader("区间入流");
                Grid1[2, 2 + i * 7].RowSpan = 2;
                Grid1[2, 3 + i * 7] = new MyHeader("时段约束");
                Grid1[2, 3 + i * 7].ColumnSpan = 6;

                Grid1[3, 3 + i * 7] = new MyHeader("最高水位");
                Grid1[3, 4 + i * 7] = new MyHeader("最低水位");
                Grid1[3, 5 + i * 7] = new MyHeader("最大出力");
                Grid1[3, 6 + i * 7] = new MyHeader("最小出力");
                Grid1[3, 7 + i * 7] = new MyHeader("最大下泄");
                Grid1[3, 8 + i * 7] = new MyHeader("最小下泄");

                Grid1[4, 2 + i * 7] = new MyHeaderBottomFixedRow("m^3/s");
                Grid1[4, 3 + i * 7] = new MyHeaderBottomFixedRow("m");
                Grid1[4, 4 + i * 7] = new MyHeaderBottomFixedRow("m");
                Grid1[4, 5 + i * 7] = new MyHeaderBottomFixedRow("MW");
                Grid1[4, 6 + i * 7] = new MyHeaderBottomFixedRow("MW");
                Grid1[4, 7 + i * 7] = new MyHeaderBottomFixedRow("m^3/s");
                Grid1[4, 8 + i * 7] = new MyHeaderBottomFixedRow("m^3/s");
            }

            Grid1[4, 1] = new MyHeaderBottomFixedRow("MW");

            DateTime dateTime = new DateTime(2017, 1, 10, 0, 0, 0);

            for (int i = 0; i < dt_all.Count; i++)
            {
                for (int r = 0; r < rows-5; r++)
                {

                    Grid1[r + 5, 0] = new SourceGrid.Cells.Cell(dateTime.ToString("HH:mm"));
                    Grid1[r + 5, 0].View = viewFirstColumn;


                    Grid1[r + 5, 1] = new plat.function2.Short_plan.MyDoubleCell(0, "0.0", false);
                    Grid1[r + 5, 1].View = viewNormal;

                    for (int j = 0; j < 7; j++)
                    {

                        Grid1[r + 5, 2 + 7*i+j] = new plat.function2.Short_plan.MyDoubleCell(Convert.ToDouble(dt_all[i].Rows[r][j]), "0.0", false);
                        Grid1[r + 5, 2 + 7*i+j].View = viewNormal;

                    }

                 //   Grid1.Columns[1].Visible = false;

                    //dateTime = dateTime.AddMinutes(15);//增加15分钟
                    //Grid1.ClipboardMode = SourceGrid.ClipboardMode.All;
                    //Grid1.AutoSizeCells();
                }
            }
          
        }

     


        private void linkLabel_Data_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //数据准备
            if (ProcessState == 0)
            {

                ProcessState = 1;

                MessageBox.Show("现已将数据读入进去");
                GetSelectedTreeview();
                //makeGrid();


                #region 多余的，可以借鉴
                //contextMenuStrip_value.ShowWaitForm();
                //contextMenuStrip_value.SetWaitFormCaption("请稍等");
                //contextMenuStrip_value.SetWaitFormDescription("正在进行数据准备");
                //=========进度过程==========
                //getDatafromConditionSetUI();
                //getPrePareDatafromDB();
                //DataPrepareUI_Initial();

                //==========================
                //contextMenuStrip_value.CloseWaitForm();

               // openFileDialog1.InitialDirectory = "..\\datebase";
               // openFileDialog1.Filter = "allfile|*.*|csv|*.csv";
               // openFileDialog2.InitialDirectory = "..\\datebase";
               // openFileDialog2.Filter = "allfile|*.*|csv|*.csv";

               // if (openFileDialog1.ShowDialog() == DialogResult.OK)
               // {
               //     dt1 = InputCSV(openFileDialog1.FileName);
               // }
               // dt1 = InputCSV(@"..\Debug\date\Data11.csv");  //读取第二个表的数据
               //// dgv_spatial.DataSource = dt1;
               // MessageBox.Show("再将第二组数据读入进去");
               // if (openFileDialog2.ShowDialog() == DialogResult.OK)
               // {
               //     dt2 = InputCSV(openFileDialog2.FileName);
               // }
               // dt2 = InputCSV(@"..\Debug\date\Data22.csv");  //读取第二个表的数据
                //dgv_time.DataSource = dt2; 
                #endregion



                //数据准备界面显示
                //隐藏结果信息显示选择界面
                tableLayoutPanel_DataResult.RowStyles[1].Height = 0;
                // treeView_stationview.BringToFront();
                panel_stationviewbutton.Visible = true;
                panel_Data.BringToFront();
                tableLayoutPanel_DataResult.BringToFront();
                //高亮当前标签...liuc
                linkLabel_Prepare.LinkColor = Color.Black;
                linkLabel_Prepare.Font = new Font("微软雅黑", 13, FontStyle.Regular);
                linkLabel_Data.LinkColor = SystemColors.MenuHighlight;
                linkLabel_Data.Font = new Font("微软雅黑", 14, FontStyle.Bold);
                linkLabel_Calc.LinkColor = Color.Black;
                linkLabel_Calc.Font = new Font("微软雅黑", 13, FontStyle.Regular);
            }
        }

        //定义属性
        private void linkLabel_Prepare_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //条件设置
            if (ProcessState == 2 || ProcessState == 1)
            {
                tableLayoutPanel_DataPrepare.Enabled = false;
                tableLayoutPanel_DataPrepare.BringToFront();
                //高亮当前标签
                linkLabel_Prepare.LinkColor = SystemColors.MenuHighlight;
                linkLabel_Prepare.Font = new Font("微软雅黑", 14, FontStyle.Bold);
                linkLabel_Data.LinkColor = Color.Black;
                linkLabel_Data.Font = new Font("微软雅黑", 13, FontStyle.Regular);
                linkLabel_Calc.LinkColor = Color.Black;
                linkLabel_Calc.Font = new Font("微软雅黑", 13, FontStyle.Regular);
            }
        }

        public class Station_1
        {
            public Int32 stationid;  //电站编号
            public String stationname; //电站名称
        }

        private void button_stationall_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < treeView_stationchart.Nodes.Count; i++)
            {
                
                treeView_stationchart.Nodes[i].Checked = true;
            }
        }

        private void button_stationcon_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < treeView_stationchart.Nodes.Count; i++)
            {
               
                treeView_stationchart.Nodes[i].Checked = !treeView_stationchart.Nodes[i].Checked;
            }
        }

        //读取EXCEL文件函数
    }
}