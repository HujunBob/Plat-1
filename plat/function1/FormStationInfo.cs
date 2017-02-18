using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace plat.function1
{
    public partial class FormStationInfo : Form
    {

        private string conStr = "Data Source=502Share-pc/orcl;User ID =interbs_16;Password=interbs_16;";

        //public int selectIndex;//指示该电站在流域电站数组里面的序号
        public Int32 stationID;

        //获取该电站的ID
        public string stationName;

        //获取该电站的ID
        public Int32 typeId;

        public string typename;

        private DataIO dataIO = new DataIO();

        //-----------Basic-----------------
        private DataTable dt_Basic;
        private BindingSource bs_Basic;
        private OracleDataAdapter da_Basic;


        //-----------Down_RZ-----------------
        private DataTable dt_DownRZ;
        private BindingSource bs_DownRZ;
        private OracleDataAdapter da_DownRZ;


        //-----------Plant------------------
        private DataTable dt_Plant;
        private BindingSource bs_Plant;
        private OracleDataAdapter da_Plant;


        //-----------PlantType--------------
        private DataTable dt_PlantType;
        private BindingSource bs_PlantType;
        private OracleDataAdapter da_PlantType;


        //-----------UpZV-----------------
        private DataTable dt_UpZV;
        private BindingSource bs_UpZV;
        private OracleDataAdapter da_UpZV;


        //-----------supply-----------------
        private DataTable dt_supply;
        private BindingSource bs_supply;
        private OracleDataAdapter da_supply;





        private DataTable dt1 = new DataTable("Datas");
        private int lock_a = 0;

        public FormStationInfo()
        {
            InitializeComponent();


            dt_Basic = new DataTable();
            bs_Basic = new BindingSource();
            da_Basic = new OracleDataAdapter();
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

            dt_PlantType = new DataTable();
            bs_PlantType = new BindingSource();
            da_PlantType = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_PlantType.DataSource = dt_PlantType;
            dgv_PlantType.DataSource = bs_PlantType;
            dt_PlantType.Columns.Add("机组名称");
            dt_PlantType.Columns.Add("机组类型");

            //  机组Q H R
            dt_Plant = new DataTable();
            bs_Plant = new BindingSource();
            da_Plant = new OracleDataAdapter();
            //绑定bs、dt、dgv
            bs_Plant.DataSource = dt_Plant;
            dgv_Plant555.DataSource = bs_Plant;
            dt_Plant.Columns.Add("H");
            dt_Plant.Columns.Add("Q");
            dt_Plant.Columns.Add("N");
            bs_Plant.DataSource = dt_Plant;
            dgv_Plant555.DataSource = bs_Plant;

            dt_supply = new DataTable();
            bs_supply = new BindingSource();
            da_supply = new OracleDataAdapter();
            //绑定水位库容 bs、dt、dgv
            bs_supply.DataSource = dt_supply;
            dgv_supply.DataSource = bs_supply;


            


            //dt_UpZV.Columns.Add("Z");
            //dt_UpZV.Columns.Add("V");


        }

        public void GetBasiciInfo()
        {
            //从数据库读取dt_Basic数据
            DataTable dt_Basic = new DataTable();
            //string dtName = "dt";
            string sql = "select * from STATIONINFO where stationid='" + stationID + "' ";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_Basic = new OracleDataAdapter();
            da_Basic = new OracleDataAdapter(sql, con);
            da_Basic.Fill(dt_Basic);
            con.Close();//关闭数据库连接
            //taForm_c = new New_station();

            string sql_1 = "select Z,V from UPZV where stationid='" + stationID + "'";
            OracleConnection con_1 = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_UpZV = new OracleDataAdapter();
            da_UpZV = new OracleDataAdapter(sql_1, con_1);
            da_UpZV.Fill(dt_UpZV);
            con.Close();//关闭数据库连接

           
            string sql_2 = "select R,Z from DOWNRZ where  stationid='" + stationID + "'";
            OracleConnection con_2 = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_DownRZ = new OracleDataAdapter();
            da_DownRZ = new OracleDataAdapter(sql_2, con_2);
            da_DownRZ.Fill(dt_DownRZ);
            con.Close();//关闭数据库连接

           //将数据写到界面上
            Checkz.Text = dt_Basic.Rows[0]["CHECKFLOODZ"].ToString();
            Designz.Text = dt_Basic.Rows[0]["DESIGNFLOODZ"].ToString();
            Floodmaxz.Text = dt_Basic.Rows[0]["FLOODMAXZ"].ToString();
            Normalz.Text = dt_Basic.Rows[0]["NORMALZ"].ToString();
            Floodlimitz.Text = dt_Basic.Rows[0]["FLOODLIMITZ"].ToString();
            Deadz.Text = dt_Basic.Rows[0]["DEADZ"].ToString();
            Totalv.Text = dt_Basic.Rows[0]["TOTALV"].ToString();
            Activev.Text = dt_Basic.Rows[0]["ACTIVEV"].ToString();
            Deadv.Text = dt_Basic.Rows[0]["DEADV"].ToString();
            Guaranteedn.Text = dt_Basic.Rows[0]["GUARANTEEDN"].ToString();
            Plantsmaxq.Text = dt_Basic.Rows[0]["PLANTSMAXQ"].ToString();
            Stationmaxr.Text = dt_Basic.Rows[0]["STATIONMAXR"].ToString();
            AdjustType.SelectedIndex = Convert.ToInt32(dt_Basic.Rows[0]["ADJUSTTYPE"]);
            XishuN.Text = Convert.ToString(8);

            //画图 chart
            bs_UpZV.Sort = "Z";                                 
            chart_UpZV.Series[0].Points.Clear();
            chart_UpZV.Series[0].XValueMember = "V";
            chart_UpZV.Series[0].YValueMembers = "Z";
            chart_UpZV.DataSource = dt_UpZV;

            bs_DownRZ.Sort = "Z";
            chart_DownRZ.Series[0].Points.Clear();
            chart_DownRZ.Series[0].XValueMember = "R";
            chart_DownRZ.Series[0].YValueMembers = "Z";
            chart_DownRZ.DataSource = dt_DownRZ;



            //临时读取的数据dt_plant_2
            DataTable dt_plant_2 = new DataTable();
            sql = "select plantname,typeid from PLANTINFO where  stationid='" + stationID + "'";
            con.Open();//打开数据库连接
            OracleDataAdapter da_Plant_2 = new OracleDataAdapter();
            da_Plant_2 = new OracleDataAdapter(sql, con);
            da_Plant_2.Fill(dt_plant_2);
            con.Close();//关闭数据库连接

            //临时读取的数据dt_plant_2
            DataTable dt_plant_2_1 = new DataTable();
            sql = "select typename from PLANTTYPEINFO where  typeid='" + dt_plant_2.Rows[0]["typeid"].ToString() + "'";
            con.Open();//打开数据库连接
            OracleDataAdapter da_Plant_2_1 = new OracleDataAdapter();
            da_Plant_2_1 = new OracleDataAdapter(sql, con);
            da_Plant_2_1.Fill(dt_plant_2_1);
            con.Close();//关闭数据库连接

            
            DataTable dt_plant_22 = new DataTable();
            dt_plant_22.Columns.Add("机组名称");
            dt_plant_22.Columns.Add("机组类型");
            //DataRow dr = dt_plant_22.NewRow();
            for (int i = 0; i < dt_plant_2.Rows.Count; i++)
            {
                dt_plant_22.Rows.Add();
                dt_plant_22.Rows[i][0] = dt_plant_2.Rows[i]["plantname"].ToString();
                dt_plant_22.Rows[i][1] = dt_plant_2_1.Rows[0]["typename"].ToString().ToString();
            }

            dgv_PlantType.DataSource = dt_plant_22;
            string plantid = dt_plant_2.Rows[0]["typeid"].ToString();

            DataTable dt_plant_3 = new DataTable();
            sql = "select * from PLANTTYPEINFO where  typeid='" + plantid + "'";
            con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_Basic333 = new OracleDataAdapter();
            da_Basic333 = new OracleDataAdapter(sql, con);
            da_Basic333.Fill(dt_plant_3);
            con.Close();//关闭数据库连接

            maxratednn.Text = dt_plant_3.Rows[0]["RATEDN"].ToString();
            MaxQQ1.Text = dt_plant_3.Rows[0]["MAXQ"].ToString();

            DataTable dt_plant_4 = new DataTable();
            sql = "select  * from PLANTHQN where  typeid='" + plantid + "'";
            con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_Basic44 = new OracleDataAdapter();
            da_Basic44 = new OracleDataAdapter(sql, con);
            da_Basic44.Fill(dt_plant_4);
            con.Close();//关闭数据库连接

            DataView dv = dt_plant_4.DefaultView;
            dv.Sort = "H asc,N asc";
            dt_plant_4 = null;
            dt_plant_4 = new DataTable();
            dt_plant_4 = dv.ToTable();

            dgv_Plant555.DataSource = dt_plant_4;

            chart_Q000.Series.Clear();

            double H = -999;

            for (int i = 0; i < dt_plant_4.Rows.Count; i++)
            {
                if (Convert.ToDouble(dt_plant_4.Rows[i]["H"]) != H)
                {
                    H = Convert.ToDouble(dt_plant_4.Rows[i]["H"]);
                    chart_Q000.Series.Add(H.ToString());
                    chart_Q000.Series[H.ToString()].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart_Q000.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_plant_4.Rows[i]["N"]), Convert.ToDouble(dt_plant_4.Rows[i]["Q"]));
                }
                else
                {
                    chart_Q000.Series[H.ToString()].Points.AddXY(Convert.ToDouble(dt_plant_4.Rows[i]["N"]), Convert.ToDouble(dt_plant_4.Rows[i]["Q"]));
                }
            }

            //todo

           // string sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140222 order by t.stationid";

           // textBox16.Text = stationName;



            string sql_4 = "select timenum ,areainflow,maxz,minz,maxn,minn,maxr,minr from defaultcondition_time t where t.stationid='" + stationID + "' and t.PLANTYPE = 0";
            OracleConnection con_4 = new OracleConnection(conStr);
            con.Open();//打开数据库连接

            OracleDataAdapter da_supply = new OracleDataAdapter();
            da_supply = new OracleDataAdapter(sql_4, con_4);
            
            da_supply.Fill(dt_supply);
            //dt_supply.Columns[0].ColumnName = "hj";

            //dt_supply.Columns.Add("时段");
            //dt_supply.Columns.Add("区间入流");
            //dt_supply.Columns.Add("最高水位");
            //dt_supply.Columns.Add("最低水位");
            //dt_supply.Columns.Add("最大出力");
            //dt_supply.Columns.Add("最小出力");
            //dt_supply.Columns.Add("最大下泄");
            //dt_supply.Columns.Add("最小下泄");

            con.Close();//关闭数据库连接

           // textBox17.Text=

            dgv_supply.DataSource = dt_supply;

        }

        //更新电站基本信息
        private void button1_Click(object sender, EventArgs e)
        {
            da_Basic.Update(dt_Basic);
        }

        //更新水位库容曲线
        private void button2_Click(object sender, EventArgs e)
        {
            da_UpZV.Update(dt_UpZV);
        }

        //更新RZ曲线
        private void button3_Click(object sender, EventArgs e)
        {
            da_DownRZ.Update(dt_DownRZ);
        }

        private void FormStationInfo_Load_1(object sender, EventArgs e)
        {
            Checkz.Enabled = false;
            Designz.Enabled = false;
            Floodmaxz.Enabled = false;
            Floodmaxz.Enabled = false;
            Normalz.Enabled = false;
            Floodlimitz.Enabled = false;
            Deadz.Enabled = false;
            Totalv.Enabled = false;
            Activev.Enabled = false;
            Deadv.Enabled = false;
            Guaranteedn.Enabled = false;
            Plantsmaxq.Enabled = false;
            Stationmaxr.Enabled = false;
            AdjustType.Enabled = false;
            XishuN.Enabled = false;
            //taForm_c = new New_station();

            Dictionary<string, int> A = new Dictionary<string, int>();

            DataTable dt_StationInfo = new DataTable();
            string sql = "select stationname from STATIONINFO ";//where stationid='" + stationID + "' and inuse=1";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da_StationInfo = new OracleDataAdapter();
            da_StationInfo = new OracleDataAdapter(sql, con);
            da_StationInfo.Fill(dt_StationInfo);
            con.Close();//关闭数据库连接

            StationNamecb.Items.Clear();
            for (int i = 0; i < dt_StationInfo.Rows.Count; i++)
            {
                A.Add(dt_StationInfo.Rows[i][0].ToString(), i);
                StationNamecb.Items.Add(dt_StationInfo.Rows[i][0]);
            }
            StationNamecb.SelectedIndex = A[stationName];


            comboBox4.Items.Clear();
            comboBox4.Items.Add("长期月调度");
            comboBox4.Items.Add("长期旬调度");

            comboBox4.Items.Add("中期调度");
            comboBox4.Items.Add("短期调度");
            comboBox4.SelectedIndex = 0;


            //taForm_c = new New_station();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (lock_a == 0)
            {
                pictureBox2.Image = Image.FromFile("..\\图片\\unlock_24.png");
                // stationName1.Enabled = true;
                Checkz.Enabled = true;
                Designz.Enabled = true;
                Floodmaxz.Enabled = true;
                Floodmaxz.Enabled = true;
                Normalz.Enabled = true;
                Floodlimitz.Enabled = true;
                Deadz.Enabled = true;
                Totalv.Enabled = true;
                Activev.Enabled = true;
                Deadv.Enabled = true;
                Guaranteedn.Enabled = true;
                Plantsmaxq.Enabled = true;
                Stationmaxr.Enabled = true;
                AdjustType.Enabled = true;
                XishuN.Enabled = true;
                lock_a = 1;
            }
            else if (lock_a == 1)
            {
                pictureBox2.Image = Image.FromFile("..\\图片\\lock_24.png");
                // stationName1.Enabled = false;
                Checkz.Enabled = false;
                Designz.Enabled = false;
                Floodmaxz.Enabled = false;
                Floodmaxz.Enabled = false;
                Normalz.Enabled = false;
                Floodlimitz.Enabled = false;
                Deadz.Enabled = false;
                Totalv.Enabled = false;
                Activev.Enabled = false;
                Deadv.Enabled = false;
                Guaranteedn.Enabled = false;
                Plantsmaxq.Enabled = false;
                Stationmaxr.Enabled = false;
                AdjustType.Enabled = false;
                XishuN.Enabled = false;
                lock_a = 0;
            }
        }

        private void StationNamecb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            stationName = StationNamecb.SelectedItem.ToString();
            //string dtName = "dt";
            string sql = "select stationid from STATIONINFO where stationname='" + stationName + "' ";
            OracleConnection con = new OracleConnection(conStr);
            con.Open();//打开数据库连接
            OracleDataAdapter da = new OracleDataAdapter();
            da = new OracleDataAdapter(sql, con);
            da.Fill(dt);
            con.Close();//关闭数据库连

            stationID = Convert.ToInt32(dt.Rows[0]["STATIONID"]);

            textBox16.Text = stationName.ToString();
            GetBasiciInfo();
            //可能不需要
        }
    }
}