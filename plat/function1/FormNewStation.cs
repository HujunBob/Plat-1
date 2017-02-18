using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace plat.function1
{
    public partial class FormNewStation : Form
    {
        public String riverid = "140222";
        public String stationId = "";

        //默认湘江流域
        public String stationName = "";

        public string typeId = "";
        public string typename = "";
        private BindingSource bs_DownRZ;
        private BindingSource bs_plantExpectEnd;
        private BindingSource bs_PlantQHN;
        private BindingSource bs_PlantType;
        private BindingSource bs_PlantType1;
        private BindingSource bs_UpZV;

        //连接数据库的钥匙
        private string conStr = "Data Source=502Share-pc/orcl;User ID =interbs_16;Password=interbs_16;";

        private OracleDataAdapter da_DownRZ;
        private OracleDataAdapter da_plantExpectEnd;
        private OracleDataAdapter da_PlantQHN;
        private OracleDataAdapter da_PlantType;
        private OracleDataAdapter da_PlantType1;
        private OracleDataAdapter da_UpZV;
        private DataIO dataIO = new DataIO();

        //-----------Down_RZ-----------------
        private DataTable dt_DownRZ;

        //-----------PlantExpectEnd--------------
        private DataTable dt_plantExpectEnd;

        //-----------PlantQHN------------------
        private DataTable dt_PlantQHN;

        //-----------PlantType--------------
        private DataTable dt_PlantType;

        //-----------PlantType1--------------
        private DataTable dt_PlantType1;

        //------------UP_ZV----------------
        private DataTable dt_UpZV;

        //private int Step = 0;//在不同的阶段
        //------------Basic----------------
        //DataTable dt_Basic = new DataTable();
       

        public FormNewStation()
        {
            InitializeComponent();


            cbb_adjust.SelectedIndex = 0;
            panel2_basicInfo.BringToFront();
            //考虑到没有csv数据时，增加空行的问题，对每个表增加相应的列名
            dt_UpZV = new DataTable();
            bs_UpZV = new BindingSource();
            da_UpZV = new OracleDataAdapter();

            //绑定水位库容 bs、dt、dgv
            bs_UpZV.DataSource = dt_UpZV;
            dgv_UpZV.DataSource = bs_UpZV;
            dt_UpZV.Columns.Add("Z");
            dt_UpZV.Columns.Add("V");

            //dt_UpZV.Columns.Add("STATIONID");
            //dt_UpZV.Columns.Add("ID");
            //dgv_UpZV.Columns["STATIONID"].Visible = false;
            //dgv_UpZV.Columns["ID"].Visible = false;

            dt_DownRZ = new DataTable();
            bs_DownRZ = new BindingSource();
            da_DownRZ = new OracleDataAdapter();

            //绑定下游水位和下泄 bs、dt、dgv
            bs_DownRZ.DataSource = dt_DownRZ;
            dgv_DownRZ.DataSource = bs_DownRZ;
            dt_DownRZ.Columns.Add("R");
            dt_DownRZ.Columns.Add("Z");
            //dt_DownRZ.Columns.Add("STATIONID");
            //dt_DownRZ.Columns.Add("ID");

            //  机组Q H R
            dt_PlantQHN = new DataTable();
            bs_PlantQHN = new BindingSource();
            da_PlantQHN = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_PlantQHN.DataSource = dt_PlantQHN;
            dgv_PlantQHN.DataSource = bs_PlantQHN;
            dt_PlantQHN.Columns.Add("H");
            dt_PlantQHN.Columns.Add("Q");
            dt_PlantQHN.Columns.Add("N");



            //dt_PlantType = new DataTable();
            //bs_PlantType = new BindingSource();
            //da_PlantType = new OracleDataAdapter();
            ////绑定bs、dt、dgv

            dt_plantExpectEnd = new DataTable();
            bs_plantExpectEnd = new BindingSource();
            da_plantExpectEnd = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_plantExpectEnd.DataSource = dt_plantExpectEnd;
            dgv_plantExpectEnd.DataSource = bs_plantExpectEnd;
            dt_plantExpectEnd.Columns.Add("H");
            dt_plantExpectEnd.Columns.Add("MaxN");
            dt_plantExpectEnd.Columns.Add("MinN");

            //-----------PlantType1--------------
            dt_PlantType1 = new DataTable();
            bs_PlantType1 = new BindingSource();
            da_PlantType1 = new OracleDataAdapter();


            dgv_planttype.DataSource = null;//没有这一句，每切换一个电站就多绑定一次
            OracleConnection con = new OracleConnection(conStr);
            String sql = "select * from planttypeinfo ";
            da_PlantType1 = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder = new OracleCommandBuilder(da_PlantType1);

            da_PlantType1.Fill(dt_PlantType1);
            bs_PlantType1.DataSource = dt_PlantType1;

            //设置机组名称列
            DataGridViewTextBoxColumn ColumnPlant = new DataGridViewTextBoxColumn();
            ColumnPlant.DataPropertyName = "PLANTNAME";
            ColumnPlant.HeaderText = "机组名称";
            ColumnPlant.Width = 100;
            dgv_planttype.Columns.Add(ColumnPlant);

            //设置机组类型列
            DataGridViewComboBoxColumn ColumnPlantType = new DataGridViewComboBoxColumn();
            ColumnPlantType.DataPropertyName = "TYPEID";
            ColumnPlantType.HeaderText = "机组类型";
            ColumnPlantType.DataSource = bs_PlantType1;
            ColumnPlantType.DisplayMember = "TYPENAME";
            ColumnPlantType.ValueMember = "TYPEID";
            ColumnPlantType.Width = 120;
            ColumnPlantType.FlatStyle = FlatStyle.Flat;
            ColumnPlantType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgv_planttype.Columns.Add(ColumnPlantType);
        }


        public delegate void MyDelegate();
        //定义事件
        public event MyDelegate MyEvent;

        public void GetPlantInfo()
        {
            //得到机组类型ID

            DataTable dt_PlantBasic = new DataTable();
            string sql = "select * from PLANTTYPEINFO where typeid='" + typeId + "' ";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_PlantBasic = new OracleDataAdapter();
            da_PlantBasic = new OracleDataAdapter(sql, con);
            da_PlantBasic.Fill(dt_PlantBasic);
            con.Close();//关闭数据库连接
            //taForm_c = new New_station();

            Maxratednnn.Text = dt_PlantBasic.Rows[0]["RATEDN"].ToString();
            MaxQQQ.Text = dt_PlantBasic.Rows[0]["MAXQ"].ToString();

            DataTable dt_HQN = new DataTable();
            string sql_1 = "select H,Q,N from PLANTHQN where typeid='" + typeId + "'";
            OracleConnection con_1 = new OracleConnection(conStr);
            con_1.Open();//打开数据库连接
            OracleDataAdapter da_HQN = new OracleDataAdapter();
            da_HQN = new OracleDataAdapter(sql_1, con_1);
            da_HQN.Fill(dt_HQN);
            con.Close();//关闭数据库连接
            dgv_PlantQ.DataSource = dt_HQN;

            DataTable dt_Expectedn = new DataTable();
            sql = "select H,MAXN,MINN from PLANTEXPECTEDN where  typeid='" + typeId + "'";
            con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_Plantexpect = new OracleDataAdapter();
            da_Plantexpect = new OracleDataAdapter(sql, con);
            da_Plantexpect.Fill(dt_Expectedn);
            con.Close();//关闭数据库连接
            dgv_PlantExpectedn.DataSource = dt_Expectedn;

            DataView dv = dt_HQN.DefaultView;
            //排序，升序根据两个标准
            dv.Sort = "H asc,N asc";
            dt_HQN = null;
            dt_HQN = new DataTable();
            dt_HQN = dv.ToTable();

            CHART_HQN0.Series.Clear();
            double H = -999;
            for (int i = 0; i < dt_HQN.Rows.Count; i++)
            {
                if (Convert.ToDouble(dt_HQN.Rows[i]["H"]) != H)
                {
                    H = Convert.ToDouble(dt_HQN.Rows[i]["H"]);
                    CHART_HQN0.Series.Add(H.ToString());
                    CHART_HQN0.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    CHART_HQN0.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_HQN.Rows[i]["N"]), Convert.ToDouble(dt_HQN.Rows[i]["Q"]));
                }
                else
                {
                    CHART_HQN0.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_HQN.Rows[i]["N"]), Convert.ToDouble(dt_HQN.Rows[i]["Q"]));
                }
            }
        }

        //跳转到上一步
        private void button1_Click_4(object sender, EventArgs e)
        {
            dt_plantExpectEnd.Clear();
            dt_PlantQHN.Clear();
            panel5_plant.BringToFront();
        }

        /// <summary>
        /// 导入水位库容数据  Upzv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "..\\数据";//todo...20161023:相对路径有问题   已解决
            openFileDialog1.Filter = "allfile|*.*|csv|*.csv";
            // MessageBox.Show("现将第一组数据读入进去");
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //dt_UpZV = dataIO.InputCSV(openFileDialog1.FileName);
                dataIO.InputCSV1(openFileDialog1.FileName, ref dt_UpZV);
            }
            //todo:dt更新后，竟然需要重新绑定，dgv才会更新
            //bs_UpZV.DataSource = dt_UpZV;
            //dgv_UpZV.DataSource = bs_UpZV;

            //liuc...20161022:排序用到BindingSource
            bs_UpZV.Sort = "Z";                                  //画图 chart
            chart_UpZV.Series[0].Points.Clear();
            chart_UpZV.DataSource = dt_UpZV;
            chart_UpZV.Series[0].XValueMember = dt_UpZV.Columns[0].ColumnName;
            chart_UpZV.Series[0].YValueMembers = dt_UpZV.Columns[1].ColumnName;
            dgv_UpZV.CellValueChanged += new DataGridViewCellEventHandler(dgv_UpZV_CellValueChanged);   //改变值时，字体变红
            chart_UpZV.GetToolTipText += new EventHandler<ToolTipEventArgs>(chartToolTiptool1);         //直接在图上可以看到数据
        }

        //导入尾水数据
        private void button12_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = "..\\数据";
            openFileDialog2.Filter = "allfile|*.*|csv|*.csv";
            // MessageBox.Show("现将第二组数据读入进去");
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                dataIO.InputCSV1(openFileDialog2.FileName, ref dt_DownRZ);
            }
            //bs_DownRZ.DataSource = dt_DownRZ;
            //dgv_DownRZ.DataSource = bs_DownRZ;
            //liuc...20161022:排序用到BindingSource
            bs_DownRZ.Sort = "Z";
            chart_DownRZ.Series[0].Points.Clear();
            chart_DownRZ.Series[0].XValueMember = "R";
            chart_DownRZ.Series[0].YValueMembers = "Z";
            chart_DownRZ.DataSource = dt_DownRZ;
            dgv_DownRZ.CellValueChanged += new DataGridViewCellEventHandler(dgv_DownRZ_CellValueChanged);  //改变值时，字体变红
            chart_DownRZ.GetToolTipText += new EventHandler<ToolTipEventArgs>(chartToolTiptool2);           //直接在图上可以看到数据
        }

        //上一步跳转到尾水水位上去
        private void button13_Click(object sender, EventArgs e)
        {
            panel4_weishui.BringToFront();
        }

        //完成按钮  增加到数据库   这个不只是机组的，所有的数据源
        private void button14_Click(object sender, EventArgs e)
        {

            Commit();

            MessageBox.Show("保存成功");
            if (MyEvent != null)
                MyEvent();//引发事件,DB写完，将对应电站图标放在地图上

            this.Close();//关闭窗体

            #region 给新增电站制作计划赋初值time&spatial

            ///* 初始化该电站约束信息 及初始条件*/
            //// 长期一年按月默认约束
            //for (int i = 0; i < 12; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (0).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}
            //// 长期一年按旬默认约束
            //for (int i = 0; i < 36; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (1).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            //// 中期调度
            //for (int i = 0; i < 1; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i).ToString() + "," + (2).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            //// 短期调度
            //for (int i = 0; i < 96; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
            //    + stationId + "," + (i + 1).ToString() + "," + (3).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            ////设置默认约束值

            //sql = "update DEFAULTCONDITION_TIME set AREAINFLOW=0,MAXZ= " + (edt_normalz.Text).ToString()
            //+ ", MINZ= " + (edt_deadz.Text).ToString() + ",MAXR= " + (edt_stationmaxr.Text).ToString()
            //+ ", MINR= " + (edt_stationminr.Text).ToString() + ",MINN= 0 ,MAXN= " + (edt_guaranteedn.Text).ToString()
            //+ ", TYPICALN = " + (edt_guaranteedn.Text).ToString() + ", MAXDELTAZ=1, MAXDELTAR=500 where stationid = " + stationId;
            //OracleHelper.ExecuteSql(sql);

            ////设置初始条件
            ////List<double> tempstartz = new List<double>();
            ////List<double> tempendz = new List<double>();
            ////List<double> temptotale = new List<double>();
            //double[] tempstartz = new double[4];
            //double[] tempendz = new double[4];
            //double[] temptotale = new double[4];
            //for (int i = 0; i < 4; i++)
            //{
            //    if (i == 3)
            //    {
            //        temptotale[i] = double.Parse(edt_guaranteedn.Text) * 24.0;
            //    }
            //    else
            //    {
            //        temptotale[i] = 0;
            //    }

            //    if (cbb_adjust.SelectedIndex > 5)
            //    {
            //        tempstartz[i] = double.Parse(edt_normalz.Text);
            //        tempendz[i] = double.Parse(edt_normalz.Text);
            //    }
            //    else if (cbb_adjust.SelectedIndex > 3)
            //    {
            //        if (i == 0 || i == 1)
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text);
            //            tempendz[i] = double.Parse(edt_normalz.Text);
            //        }
            //        else if (i == 2)
            //        {
            //            tempstartz[i] = double.Parse(edt_deadz.Text);
            //            tempendz[i] = double.Parse(edt_deadz.Text);
            //        }
            //        else
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //            tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //        }
            //    }
            //    else
            //    {
            //        if (i == 0 || i == 1)
            //        {
            //            tempstartz[i] = double.Parse(edt_deadz.Text);
            //            tempendz[i] = double.Parse(edt_deadz.Text);
            //        }
            //        else
            //        {
            //            tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //            tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
            //        }
            //    }

            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    sql = "insert into DEFAULTCONDITION_SPATIAL(STATIONID,STARTZ,ENDZ,PLANTYPE,TOTALE) values("
            //    + stationId + "," + (tempstartz[i]).ToString() + "," + (tempendz[i]).ToString()
            //    + "," + (i).ToString() + "," + (temptotale[i]).ToString() + ")";
            //    OracleHelper.ExecuteSql(sql);
            //}

            #endregion 给新增电站制作计划赋初值time&spatial

            //----------------------------------------------------------------------
        }


        public bool Commit()
        {

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (dgv_PlantQ.Rows[i].Cells[j].Value != null || dgv_PlantExpectedn.Rows[i].Cells[j].Value != null)
                    {
                        this.Close();
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据未加载，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            this.Close();
                        }

                        break;
                    }
                    break;
                }
            }

            //OracleHelper OracleHelper = new OracleHelper();
            //-------------------------保存基础数据到DB-------------------------------

            //插入电站基础信息
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            string sql;//插入电站的sql语句
            sql = "insert into STATIONINFO t (stationname,adjusttype,normalz,deadz,riverid,plantsmaxq,stationmaxr,stationminr,ratedn,guaranteedn,kvalue,floodlimitz,floodmaxz,designfloodz,checkfloodz,deadv,activev,totalv,inuse) values ('" +
                edt_stationName.Text + "'," + (cbb_adjust.SelectedIndex).ToString() + "," + (edt_normalz.Text).ToString() +
                "," + (edt_deadz.Text).ToString() + "," + riverid + "," + (edt_plantsmaxq.Text).ToString() +
                "," + (edt_stationmaxr.Text).ToString() + "," + (edt_stationminr.Text).ToString() +
                "," + (edt_ratedn.Text).ToString() + "," + (edt_guaranteedn.Text).ToString() +
                "," + (edt_kvalue.Text).ToString() + "," + (edt_floodlimitz.Text).ToString() +
                "," + (edt_floodmaxz.Text).ToString() + "," + (edt_designz.Text).ToString() +
                "," + (edt_checkz.Text).ToString() + "," + (edt_deadv.Text).ToString() +
                "," + (edt_activev.Text).ToString() + "," + (edt_totalv.Text).ToString() + "," + (1).ToString() + ")";
            OracleCommand cmd = new OracleCommand(sql, con);
            int row = cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            //得到电站ID
            DataTable dt = new DataTable("dt");
            //string dtName = "dt";
            sql = "select stationid,stationname from STATIONINFO where stationname='" + edt_stationName.Text + "' and inuse=1";
            con.Open();//打开数据库连接
            da_UpZV = new OracleDataAdapter(sql, con);
            da_UpZV.Fill(dt);
            con.Close();//关闭数据库连接
            stationId = dt.Rows[0]["stationid"].ToString();
            stationName = dt.Rows[0]["stationname"].ToString();

            //-------------------------保存upZV数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from upzv where stationid = " + stationId + " order by Z";
            da_UpZV = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder = new OracleCommandBuilder(da_UpZV);
            dt_UpZV.Columns.Add("STATIONID");
            dt_UpZV.Columns.Add("ID");
            for (int i = 0; i < dt_UpZV.Rows.Count; i++)
            {
                dt_UpZV.Rows[i]["STATIONID"] = stationId;
            }
            //提交
            da_UpZV.Update(dt_UpZV);
            dt_UpZV.AcceptChanges();//确定表的状态

            //刷新
            dt_UpZV.Clear();
            da_UpZV.Fill(dt_UpZV);

            //插入机组基础信息
            con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            for (int i = 0; i < dgv_planttype.Rows.Count - 1; i++)
            {
                sql = "insert into PLANTINFO t (plantname,typeid,stationid) values ('" +
                  (dgv_planttype.Rows[i].Cells[0].Value).ToString() + "'," + typeId.ToString() + "," + stationId.ToString() + ")";
                OracleCommand cmd1 = new OracleCommand(sql, con);
                cmd1.ExecuteNonQuery();
            }

            con.Close();//关闭数据库连接

            //-------------------------保存DownRZ数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from DOWNRZ where stationid = " + stationId + " order by Z";
            da_DownRZ = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder1 = new OracleCommandBuilder(da_DownRZ);
            dt_DownRZ.Columns.Add("STATIONID");
            dt_DownRZ.Columns.Add("ID");
            for (int i = 0; i < dt_DownRZ.Rows.Count; i++)
            {
                dt_DownRZ.Rows[i]["STATIONID"] = stationId;
            }
            //提交
            da_DownRZ.Update(dt_DownRZ);
            dt_DownRZ.AcceptChanges();//确定表的状态

            //刷新
            dt_DownRZ.Clear();
            da_DownRZ.Fill(dt_UpZV);
            //----------------------------------------------------------------------

            //todo...liuc：没必要又于完成按钮处又保存一遍plant的三个表格


            #region 写多了的，可以参考下！数据库保存
            //插入机组基础信息
            //con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            //con.Open();//打开数据库连接
            //sql = "insert into PLANTTYPEINFO t (typename,ratedn,maxq) values ('" +
            //    (dt_PlantType.Rows[0][0]).ToString() + "'," + (dt_PlantType.Rows[0][1]).ToString() + "," + (dt_PlantType.Rows[0][2]).ToString() + ")";
            //cmd = new OracleCommand(sql, con);
            //row = cmd.ExecuteNonQuery();
            //con.Close();//关闭数据库连接

            //得到机组ID
            //dt = new DataTable("dt");
            //string dtName = "dt";
            //sql = "select typeid from PLANTTYPEINFO ";
            //sql = "select * from PLANTTYPEINFO where planttypename='" + dt_PlantType.Rows[0][0] + "'";
            //con.Open();//打开数据库连接
            //da_PlantQHN = new OracleDataAdapter(sql, con);
            //da_PlantQHN.Fill(dt);
            //con.Close();//关闭数据库连接
            //typeId = dt.Rows[0]["typeid"].ToString();
            //stationName = dt.Rows[0]["stationname"].ToString();

            //-------------------------保存PlantQHN数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            //sql = "select * from PLANTHQN where typeid = " + typeId + " order by H";
            //da_PlantQHN = new OracleDataAdapter(sql, con);
            //commandbuilder = new OracleCommandBuilder(da_PlantQHN);
            //dt_PlantQHN.Columns.Add("TYPEID");
            //dt_PlantQHN.Columns.Add("ID");
            //for (int i = 0; i < dt_PlantQHN.Rows.Count; i++)
            //{
            //    dt_PlantQHN.Rows[i]["TYPEID"] = typeId;
            //}
            //提交
            //da_PlantQHN.Update(dt_PlantQHN);
            //dt_PlantQHN.AcceptChanges();//确定表的状态

            //刷新
            //dt_PlantQHN.Clear();
            //da_PlantQHN.Fill(dt_PlantQHN);

            //dt_plantExpectEnd = new DataTable();
            //bs_plantExpectEnd = new BindingSource();
            //da_plantExpectEnd = new OracleDataAdapter();

            //-------------------------保存PlantBeforeN数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            //sql = "select * from PLANTEXPECTEND where typeid = " + typeId + " order by H";
            //da_plantExpectEnd = new OracleDataAdapter(sql, con);
            //commandbuilder = new OracleCommandBuilder(da_plantExpectEnd);
            //dt_plantExpectEnd.Columns.Add("TYPEID");
            //dt_plantExpectEnd.Columns.Add("ID");
            //for (int i = 0; i < dt_plantExpectEnd.Rows.Count; i++)
            //{
            //    dt_DownRZ.Rows[i]["TYPEID"] = typeId;
            //}
            //提交
            //da_plantExpectEnd.Update(dt_plantExpectEnd);
            //dt_plantExpectEnd.AcceptChanges();//确定表的状态

            //刷新
            //dt_plantExpectEnd.Clear();
            //da_plantExpectEnd.Fill(dt_plantExpectEnd);
            //----------------------------------------------------------------------

            //插入机组基础信息INFO
            //插入机组基础信息
            //con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            //con.Open();//打开数据库连接
            // string sql = "insert into PLANTTYPEINFO t (typename,ratedn,maxq) values ('" +
            //  (PlantType.Text).ToString() + (RatedN.Text).ToString()+ "'," + (RatedN.Text).ToString() + "," + (MaxQ).ToString() + ")";

            //sql = "insert into PLANTINFO t (typename,typeid,stationid) values ('" +
            //      (PlantTypeName.Text).ToString() + "'," + (typeId).ToString() + "," + (stationId).ToString() + ")";
            //cmd = new OracleCommand(sql, con);
            //row = cmd.ExecuteNonQuery();
            //con.Close();//关闭数据库连接 
            #endregion

            con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            //string sql;//插入电站的sql语句



            for (int i = 0; i < 12; i++)
            {
                sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
                + stationId + "," + (i + 1).ToString() + "," + (0).ToString() + ")";
                cmd = new OracleCommand(sql, con);
                row = cmd.ExecuteNonQuery();
                //OracleHelper.ExecuteSql(sql);
            }
            // 长期一年按旬默认约束
            for (int i = 0; i < 36; i++)
            {
                sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
                + stationId + "," + (i + 1).ToString() + "," + (1).ToString() + ")";
                cmd = new OracleCommand(sql, con);
                row = cmd.ExecuteNonQuery();
                // OracleHelper.ExecuteSql(sql);
            }

            // 中期调度
            for (int i = 0; i < 1; i++)
            {
                sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
                + stationId + "," + (i).ToString() + "," + (2).ToString() + ")";
                cmd = new OracleCommand(sql, con);
                row = cmd.ExecuteNonQuery();
                // OracleHelper.ExecuteSql(sql);
            }

            // 短期调度
            for (int i = 0; i < 96; i++)
            {
                sql = "insert into DEFAULTCONDITION_TIME(STATIONID,TIMENUM,PLANTYPE) values("
                + stationId + "," + (i + 1).ToString() + "," + (3).ToString() + ")";
                // OracleHelper.ExecuteSql(sql);
                cmd = new OracleCommand(sql, con);
                row = cmd.ExecuteNonQuery();
            }


            //设置默认约束值

            sql = "update DEFAULTCONDITION_TIME set AREAINFLOW=0,MAXZ= " + (edt_normalz.Text).ToString()
            + ", MINZ= " + (edt_deadz.Text).ToString() + ",MAXR= " + (edt_stationmaxr.Text).ToString()
            + ", MINR= " + (edt_stationminr.Text).ToString() + ",MINN= 0 ,MAXN= " + (edt_ratedn.Text).ToString()
            + ", TYPICALN = " + (edt_guaranteedn.Text).ToString() + ", MAXDELTAZ=1, MAXDELTAR=500 where stationid = " + stationId;
            //OracleHelper.ExecuteSql(sql);

            cmd = new OracleCommand(sql, con);
            row = cmd.ExecuteNonQuery();
            // con.Close();//关闭数据库连接


            double[] tempstartz = new double[4];
            double[] tempendz = new double[4];
            double[] temptotale = new double[4];
            for (int i = 0; i < 4; i++)
            {
                if (i == 3)
                {
                    temptotale[i] = double.Parse(edt_guaranteedn.Text) * 24.0;
                }
                else
                {
                    temptotale[i] = 0;
                }

                if (cbb_adjust.SelectedIndex > 5)
                {
                    tempstartz[i] = double.Parse(edt_normalz.Text);
                    tempendz[i] = double.Parse(edt_normalz.Text);
                }
                else if (cbb_adjust.SelectedIndex > 3)
                {
                    if (i == 0 || i == 1)
                    {
                        tempstartz[i] = double.Parse(edt_normalz.Text);
                        tempendz[i] = double.Parse(edt_normalz.Text);
                    }
                    else if (i == 2)
                    {
                        tempstartz[i] = double.Parse(edt_deadz.Text);
                        tempendz[i] = double.Parse(edt_deadz.Text);
                    }
                    else
                    {
                        tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
                        tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
                    }
                }
                else
                {
                    if (i == 0 || i == 1)
                    {
                        tempstartz[i] = double.Parse(edt_deadz.Text);
                        tempendz[i] = double.Parse(edt_deadz.Text);
                    }
                    else
                    {
                        tempstartz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
                        tempendz[i] = double.Parse(edt_normalz.Text) * 0.5 + double.Parse(edt_deadz.Text) * 0.5;
                    }
                }

            }
            for (int i = 0; i < 4; i++)
            {

                sql = "insert into DEFAULTCONDITION_SPATIAL(STATIONID,STARTZ,ENDZ,PLANTYPE,TOTALE) values("
                + stationId + "," + (tempstartz[i]).ToString() + "," + (tempendz[i]).ToString()
                + "," + (i).ToString() + "," + (temptotale[i]).ToString() + ")";
                cmd = new OracleCommand(sql, con);
                row = cmd.ExecuteNonQuery();
                con.Close();//关闭数据库连接
            }

            return true;
        }

        //保存机组信息
        private void button15_Click_1(object sender, EventArgs e)
        {
            //插入机组基础信息
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            string sql = "insert into PLANTTYPEINFO t (typename,ratedn,maxq) values ('" +
                   (PlantTypeName.Text).ToString() + "'," + (RatedN.Text).ToString() + "," + (MaxQ.Text).ToString() + ")";
            OracleCommand cmd = new OracleCommand(sql, con);
            int row = cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            //得到机组类型ID
            DataTable dt = new DataTable("dt");
            sql = "select * from PLANTTYPEINFO where typename='" + PlantTypeName.Text + "'";
            con.Open();//打开数据库连接
            da_PlantQHN = new OracleDataAdapter(sql, con);
            da_PlantQHN.Fill(dt);
            con.Close();//关闭数据库连接
            typeId = dt.Rows[0]["typeid"].ToString();
            //stationName = dt.Rows[0]["stationname"].ToString();

            //-------------------------保PlantExpectEnd数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from PLANTEXPECTEDN where typeid = " + typeId + " order by H";
            da_plantExpectEnd = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder1 = new OracleCommandBuilder(da_plantExpectEnd);   //这句到底有什么作用？
            dt_plantExpectEnd.Columns.Add("TYPEID");
            dt_plantExpectEnd.Columns.Add("ID");
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < dt_plantExpectEnd.Rows.Count; i++)
            {
                dt_plantExpectEnd.Rows[i]["TYPEID"] = typeId;
            }
            cmd.Connection = con;
            con.Open();
            //计时
            sw.Stop();
            sw.Start();
            for (int i = 0; i < dt_plantExpectEnd.Rows.Count; i++)
            {
                sql = "insert into PLANTEXPECTEDN(H,maxN,minn,typeid) values(" + dt_plantExpectEnd.Rows[i][0] + ","
                    + dt_plantExpectEnd.Rows[i][1] + "," + dt_plantExpectEnd.Rows[i][2] + "," + dt_plantExpectEnd.Rows[i][3] + ")";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            sw.Stop();
            MessageBox.Show(sw.Elapsed.ToString());
            //提交
            //   da_plantExpectEnd.Update(dt_plantExpectEnd);
            //dt_plantExpectEnd.AcceptChanges();//确定表的状态

            //刷新
            dt_plantExpectEnd.Clear();
            da_plantExpectEnd.Fill(dt_plantExpectEnd);
            //---
            dgv_plantExpectEnd.Columns["TYPEID"].Visible = false;
            dgv_plantExpectEnd.Columns["ID"].Visible = false;

            //-------------------------保存PlantQHN数据到DB-------------------------------
            //得到OracleDataAdapter，OracleCommandbuilder，没有cmb不能自动生成Update里的sql
            sql = "select * from PLANTHQN where typeid = " + typeId + " order by H";
            da_PlantQHN = new OracleDataAdapter(sql, con);
            OracleCommandBuilder commandbuilder = new OracleCommandBuilder(da_PlantQHN);
            dt_PlantQHN.Columns.Add("TYPEID");
            dt_PlantQHN.Columns.Add("ID");
            for (int i = 0; i < dt_PlantQHN.Rows.Count; i++)
            {
                dt_PlantQHN.Rows[i]["TYPEID"] = typeId;
            }
            //提交  怎么运行的
            da_PlantQHN.Update(dt_PlantQHN);
            dt_PlantQHN.AcceptChanges();//确定表的状态

            //刷新
            dt_PlantQHN.Clear();
            da_PlantQHN.Fill(dt_PlantQHN);

            MessageBox.Show("机组保存成功");

            //在dgv_planttype下拉框中更新选择项
            dgv_planttype.DataSource = null;//没有这一句，每切换一个电站就多绑定一次
            con = new OracleConnection(conStr);
            sql = "select * from planttypeinfo ";
            da_PlantType1 = new OracleDataAdapter(sql, con);
            commandbuilder = new OracleCommandBuilder(da_PlantType1);

            da_PlantType1.Fill(dt_PlantType1);
            bs_PlantType1 = new BindingSource();
            bs_PlantType1.DataSource = dt_PlantType1;
            dgv_planttype.DataSource = null;

            //设置机组名称列
            DataGridViewTextBoxColumn ColumnPlant = new DataGridViewTextBoxColumn();
            ColumnPlant.DataPropertyName = "PLANTNAME";
            ColumnPlant.HeaderText = "机组名称";
            ColumnPlant.Width = 100;
            dgv_planttype.Columns.Add(ColumnPlant);

            //设置机组类型列
            DataGridViewComboBoxColumn ColumnPlantType = new DataGridViewComboBoxColumn();
            ColumnPlantType.DataPropertyName = "TYPEID";
            //  ColumnPlantType.HeaderText = "机组类型";
            ColumnPlantType.DataSource = bs_PlantType1;
            ColumnPlantType.DisplayMember = "TYPENAME";
            ColumnPlantType.ValueMember = "TYPEID";
            ColumnPlantType.Width = 120;
            ColumnPlantType.FlatStyle = FlatStyle.Flat;
            ColumnPlantType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            dgv_planttype.Columns.Add(ColumnPlantType);

            panel5_plant.BringToFront();
            dt_plantExpectEnd.Clear();
            dt_PlantQHN.Clear();
        }

        //添加机组类型  预想出力
        private void button18_Click(object sender, EventArgs e)
        {
            openFileDialog3.InitialDirectory = "..\\数据";
            openFileDialog3.Filter = "allfile|*.*|csv|*.csv";
            // MessageBox.Show("现将第四组数据读入进去");
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                dataIO.InputCSV1(openFileDialog3.FileName, ref dt_plantExpectEnd);
            }
            //dgv_plantExpectEnd.DataSource = dt_plantExpectEnd;
            bs_plantExpectEnd.Sort = "H";
        }

        //添加机组类型  QHN 数据
        private void button19_Click(object sender, EventArgs e)
        {
            openFileDialog4.InitialDirectory = "..\\数据";
            openFileDialog4.Filter = "allfile|*.*|csv|*.csv";
            //MessageBox.Show("现将第三组数据读入进去");
            if (openFileDialog4.ShowDialog() == DialogResult.OK)
            {
                dataIO.InputCSV1(openFileDialog4.FileName, ref dt_PlantQHN);
            }
            chart_QHN.Series.Clear();
            double H = -999;
            for (int i = 0; i < dt_PlantQHN.Rows.Count; i++)
            {
                if (Convert.ToDouble(dt_PlantQHN.Rows[i]["H"]) != H)
                {
                    H = Convert.ToDouble(dt_PlantQHN.Rows[i]["H"]);
                    chart_QHN.Series.Add(H.ToString());
                    chart_QHN.Series.Last().ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart_QHN.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_PlantQHN.Rows[i]["N"]), Convert.ToDouble(dt_PlantQHN.Rows[i]["Q"]));
                }
                else
                {
                    chart_QHN.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_PlantQHN.Rows[i]["N"]), Convert.ToDouble(dt_PlantQHN.Rows[i]["Q"]));
                }
            }
        }

        /// <summary>
        /// 基础数据panel点击下一步  判断是否存在控制 Basic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int a = 0;
            foreach (Control c in panel2_basicInfo.Controls)
            {
                if (c is TextBox)  //todo  这里不一定都为Textbox,还有comobox
                {
                    if (c.Text == "")
                    {
                        DialogResult dr = MessageBox.Show("存在空值，是否继续?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            panel3_shuiku.BringToFront();
                            break;
                        }
                        a++;
                    }
                }
            }
            if (a == 0)
            {
                panel3_shuiku.BringToFront();
            }
        }

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //   //插入电站基础信息
        //   OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
        //   con.Open();//打开数据库连接
        //   string sql = "insert into PLANTTYPEINFO t (typename,ratedn,maxq) values ('" +
        //  (dt_PlantType.Rows[0][0]).ToString() + "'," + (dt_PlantType.Rows[0][1]).ToString() + "," + (dt_PlantType.Rows[0][2]).ToString() + ")";
        //   OracleCommand cmd = new OracleCommand(sql, con);
        //  int  row = cmd.ExecuteNonQuery();
        //   con.Close();//关闭数据库连接
        //}
        //跳转到添加机组类型中去
        private void button20_Click(object sender, EventArgs e)
        {
            panel2_PlantType.BringToFront();
        }

        /// <summary>
        /// 取消按钮 basicInfo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 上一步跳转到基础信息panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_UpZV.RowCount; i++)
            {
                for (int j = 0; j < dgv_UpZV.ColumnCount; j++)
                {
                    if (dgv_UpZV.Rows[i].Cells[j].Value != null)
                    {
                        panel2_basicInfo.BringToFront();
                        break;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据未加载，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            panel2_basicInfo.BringToFront();
                            break;
                        }
                        break;
                    }
                }
                break;
            }
        }

        /// <summary>
        /// 下一步跳转到尾水panel  todo 数据监测这一块！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_UpZV.RowCount - 1; i++)
            {
                for (int j = 0; j < dgv_UpZV.ColumnCount - 1; j++)
                {
                    // MessageBox.Show(dgv_UpZV.Rows[0].Cells[0].Value.ToString());
                    if (dgv_UpZV.Rows[i].Cells[j].Value != null)
                    {
                        panel4_weishui.BringToFront();
                        break;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据未加载，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            panel4_weishui.BringToFront();
                            break;
                        }
                        break;
                    }
                }
                break;
            }
        }

        //上一步 判断 跳转到水位库容panel 去
        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_DownRZ.RowCount; i++)
            {
                for (int j = 0; j < dgv_DownRZ.ColumnCount; j++)
                {
                    if (dgv_DownRZ.Rows[i].Cells[j].Value != null)
                    {
                        panel3_shuiku.BringToFront();
                        break;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据未加载，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            panel3_shuiku.BringToFront();
                            break;
                        }
                        break;
                    }
                }
                break;
            }
        }

        //下一步 判断跳转到机组panel 去
        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_DownRZ.RowCount; i++)
            {
                for (int j = 0; j < dgv_DownRZ.ColumnCount; j++)
                {
                    if (dgv_DownRZ.Rows[i].Cells[j].Value != null)
                    {
                        panel5_plant.BringToFront();
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("数据未加载，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.Yes)
                        {
                            panel5_plant.BringToFront();
                        }
                        break;
                    }
                    break;
                }
                break;
            }
        }

        /// <summary>
        /// 添加一个tooltip事件，当鼠标移到线段时显示xy坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 画图 水位库容
        private void chartToolTiptool1(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                // DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("水位：{0:f3}\\n库容：{1:F3}", dt_UpZV.Rows[i][0], dt_UpZV.Rows[i][1]);
            }
        }

        /// <summary>
        /// 添加一个tooltip事件，当鼠标移到线段时显示xy坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 画图 尾水
        private void chartToolTiptool2(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                // DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("水位：{0:f3}\\下泄流量：{1:F3}", dt_DownRZ.Rows[i][0], dt_DownRZ.Rows[i][1]);
            }
        }

        /// <summary>
        /// 消除frame中panel往父控件上靠时的闪烁现象
        /// </summary>
        /// <param name="frame"></param>
        private void ClearFramesOfPanelParent(UserControl frame)
        {
            //测试时间
            //System.Diagnostics.Stopwatch mywatch = new System.Diagnostics.Stopwatch();
            //mywatch.Restart();
            foreach (Control tempUserControl in PanelFrameParent.Controls)
            {
                if (tempUserControl is System.Windows.Forms.UserControl && tempUserControl != frame)
                {
                    //MessageBox.Show("--开始释放：" + tempUserControl.GetType().ToString());
                    System.Windows.Forms.UserControl userControl = (System.Windows.Forms.UserControl)tempUserControl;
                    PanelFrameParent.Controls.Remove(userControl);
                    if (!userControl.IsDisposed)
                    {
                        //MessageBox.Show("释放：" + userControl.Name.ToString());
                        userControl.Dispose();
                    }
                }
            }
        }

        //尾水添加序号事件
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号
                SolidBrush v_SolidBrush = new SolidBrush(dgv_DownRZ.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        //下游尾水数值改变时  数据变红
        private void dgv_DownRZ_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgv_DownRZ.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
        }

        //添加行号  保存所以里面的QHN
        private void dgv_plantExpectEnd_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号
                SolidBrush v_SolidBrush = new SolidBrush(dgv_plantExpectEnd.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        //添加行号
        private void dgv_PlantQHN_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号
                SolidBrush v_SolidBrush = new SolidBrush(dgv_PlantQHN.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void dgv_planttype_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                typeId = dgv_planttype.Rows[e.RowIndex].Cells[1].Value.ToString();

                // //string dtName = "dt";
                // string sql = "select typeid from PLANTTYPEINFO where typename='" + typename + "' ";
                // OracleConnection con = new OracleConnection(conStr);
                // con.Open();//打开数据库连接
                // OracleDataAdapter da = new OracleDataAdapter();
                // da = new OracleDataAdapter(sql, con);
                // OracleCommand cmd = new OracleCommand(sql, con);
                // Int32 id =Convert.ToInt32 ( cmd.ExecuteScalar());

                // da.Fill(dt);
                // con.Close();//关闭数据库连

                //// typeId = dt.Rows[0]["TYPEID"].ToString();
                //typeId = id.ToString();

                GetPlantInfo();
            }
        }

        //添加行号  保存机组类型里面的QHN
        //水位库容数值改变时  数据变红
        private void dgv_UpZV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgv_UpZV.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
        }

        /// <summary>
        /// 水位库容添加序号事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_UpZV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                //添加行号
                SolidBrush v_SolidBrush = new SolidBrush(dgv_UpZV.RowHeadersDefaultCellStyle.ForeColor);
                int v_LineNo = 0;
                v_LineNo = e.RowIndex + 1;

                string v_Line = v_LineNo.ToString();

                e.Graphics.DrawString(v_Line, e.InheritedRowStyle.Font, v_SolidBrush, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        /*
         * 水位库容添加行   右键
         * */

        private void 删除ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            dt_DownRZ.Rows.RemoveAt(dgv_DownRZ.SelectedRows[0].Index);
        }

        private void 删除ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            dt_PlantQHN.Rows.RemoveAt(dgv_UpZV.SelectedRows[0].Index);
        }

        private void 删除ToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            dt_plantExpectEnd.Rows.RemoveAt(dgv_UpZV.SelectedRows[0].Index);
        }

        private void 删除行ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (dgv_UpZV.RowCount > 0)    //hujun
            {
                // int i = dgv_DownRZ.SelectedCells
                dt_UpZV.Rows.RemoveAt(dgv_UpZV.SelectedRows[0].Index);
            }
        }

        private void 添加ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataRow dr = dt_DownRZ.NewRow();

            // dt_UpZV.Rows.Add(dr);
            dt_DownRZ.Rows.InsertAt(dr, 10);
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataRow dr = dt_PlantQHN.NewRow();
            dt_PlantQHN.Rows.InsertAt(dr, 10);
        }

        private void 添加ToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            DataRow dr = dt_plantExpectEnd.NewRow();
            dt_plantExpectEnd.Rows.InsertAt(dr, 10);
        }

        private void 添加行ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataRow dr = dt_UpZV.NewRow();
            dt_UpZV.Rows.Add(dr);
            dt_UpZV.Rows.InsertAt(dr, 1);
            // dt_UpZV .Rows.
            //todo:折中
            //bs_UpZV.MoveLast()
        }

        private void FormNewStation_Load(object sender, EventArgs e)
        {

        }

        /*
         * 水位库容删除行   右键
         * */
        /*
        * 尾水添加行  右键
        * */
        /*
       * 尾水添加行  右键
       * */
    }
}