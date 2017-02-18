using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace plat.function2.Short_plan
{
    public partial class Short_plan : Form
    {
        public List<Station_1> station_RT = new List<Station_1>();
        private string conStr = "Data Source=502Share-pc/orcl;User ID =interbasindb;Password=interbasin;";
        private DataTable dt = new DataTable();
        private int iPlanSelectedNum = -1;//0，1，2分别对应短，中，长

        public Short_plan()
        {
            InitializeComponent();
            DataPrepareUI_Initial();
        }

        public void DataPrepareUI_Initial()
        {
            using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
            {
                con.Open();//打开数据库连接
                //and  t.riverid in ('140222' , '140223' ,'140224' ,'142225' )
                string sql = "select * from STATIONINFO t where t.inuse=1  and t.riverid = 140222  order by t.stationid";
                //todo:默认只读取湘江流域的可用电站
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(dt);
                con.Close();//关闭数据库连接
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Station_1 tempstation = new Station_1();
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
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button5_Click(object sender, EventArgs e)
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

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        public class Station_1
        {
            public Int32 stationid;  //电站编号
            public String stationname; //电站名称
        }
    }
}