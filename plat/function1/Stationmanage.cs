using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace plat.function1
{
    public partial class Stationmanage : Form
    {
        //当前按键的状态,0:没有 1：增加 2：删除
        public int statusOption = 0;

        //连接数据库的钥匙
        private string conStr = "Data Source=502Share-pc/orcl;User ID =interbs_16;Password=interbs_16;";

        private List<Station> myStations;

        //定义电站数组
        private int position_i = 0;

        private int position_j = 0;

        private FormNewStation staForm_c = null;

        private Station tempstation = new Station();
        public void Getstation(string sql, PictureBox picture)
        {
            DataTable dt = new DataTable();

            using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
            {
                con.Open();//打开数据库连接
                //todo:默认只读取湘江流域的可用电站
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(dt);
                con.Close();//关闭数据库连接
            }
            myStations = new List<Station>();  // 电站数组存储

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Station tempstation = new Station();  //临时变量

                tempstation.stationid = Convert.ToInt32(dt.Rows[i]["stationid"]);
                tempstation.stationname = dt.Rows[i]["stationname"].ToString();

                tempstation.lab_name = new Label();
                tempstation.lab_name.Text = dt.Rows[i]["stationname"].ToString();

                tempstation.picturebox = new PictureBox();
                tempstation.picturebox.Height = 10;
                tempstation.picturebox.Width = 10;
                tempstation.picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
                tempstation.picturebox.SizeMode = PictureBoxSizeMode.StretchImage;

                tempstation.pox = Convert.ToDouble(dt.Rows[i]["relativex"]);
                tempstation.poy = Convert.ToDouble(dt.Rows[i]["relativey"]);

                tempstation.picturebox.Left = Convert.ToInt32(tempstation.pox * picture.Width - tempstation.picturebox.Width / 2.0);
                tempstation.picturebox.Top = Convert.ToInt32(tempstation.poy * picture.Height - tempstation.picturebox.Height / 2.0);
                tempstation.lab_name.Font = new Font("宋体", 8, FontStyle.Regular);
                tempstation.lab_name.Left = Convert.ToInt32(tempstation.pox * picture.Width - tempstation.picturebox.Width / 2.0) + 10;
                tempstation.lab_name.Top = Convert.ToInt32(tempstation.poy * picture.Height - tempstation.picturebox.Height / 2.0);
                tempstation.lab_name.BackColor = Color.Transparent;
                tempstation.picturebox.Parent = picture;
                tempstation.lab_name.Parent = picture;

                tempstation.picturebox.DoubleClick += new System.EventHandler(btn_Doubleclick);    //双击显示查看电站窗口
                tempstation.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(pic_Mousemove);  //移动到上面换个红色图标
                tempstation.picturebox.MouseLeave += new System.EventHandler(pic_Mouseleave);  //移开换个绿色图标
                tempstation.picturebox.Click += new System.EventHandler(pic_mouseclick);
                myStations.Add(tempstation);
            }


        }

        /// <summary>
        /// 窗口初始化，电站位置，电站id，电站名字
        /// 从数据库中读取stationinfo的信息，将其放到picturebox1进行显示。
        /// </summary>
        ///
        public Stationmanage()
        {
            InitializeComponent();

            string sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140222 order by t.stationid";
            Getstation(sql, pictureBox1);

            sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140223 order by t.stationid";
            Getstation(sql, pictureBox2);

            sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140225 order by t.stationid";
            Getstation(sql, pictureBox3);

            sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140224 order by t.stationid";
            Getstation(sql, pictureBox4);


           
        }

        /// <summary>
        /// 双击电站弹出电站信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Doubleclick(object sender, System.EventArgs e)      // 设为查看按钮
        {
            FormStationInfo frm7 = new FormStationInfo();
            for (int i = 0; i < myStations.Count; i++)
            {
                if (myStations[i].picturebox == sender)
                {
                    frm7.stationID = myStations[i].stationid;
                    frm7.stationName = myStations[i].stationname;
                }
            }
            frm7.Show();
            frm7.GetBasiciInfo();
        }

        private void BtnAddStation_Click(object sender, EventArgs e)
        {
            statusOption = 1;
        }

        private void BtnRemoveStation_Click(object sender, EventArgs e)
        {
            statusOption = 2;
        }

        //在newstation窗口点击“完成”后，触发的事件，用于在地图上显示添加电站图标
        //创建新电站后在首页显示新电站信息
        private void c_MyEvent()
        {
            Station tempstation = new Station();

            tempstation.stationid = Convert.ToInt32(staForm_c.stationId);
            tempstation.stationname = staForm_c.stationName.ToString();

            tempstation.lab_name = new Label();
            tempstation.lab_name.Text = staForm_c.stationName.ToString();

            tempstation.picturebox = new PictureBox();
            tempstation.picturebox.Height = 10;
            tempstation.picturebox.Width = 10;
            tempstation.picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
            tempstation.picturebox.SizeMode = PictureBoxSizeMode.StretchImage;

            tempstation.pox = 1.0 * position_i / pictureBox1.Width;
            tempstation.poy = 1.0 * position_j / pictureBox1.Height;

            tempstation.picturebox.Left = position_i;
            tempstation.picturebox.Top = position_j;
            tempstation.lab_name.Left = position_i;
            tempstation.lab_name.Top = position_j - 6;
            tempstation.lab_name.BackColor = Color.Transparent;
            tempstation.picturebox.Parent = pictureBox1;
            tempstation.lab_name.Parent = pictureBox1;

            tempstation.picturebox.DoubleClick += new System.EventHandler(btn_Doubleclick);
            tempstation.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(pic_Mousemove);
            tempstation.picturebox.MouseLeave += new System.EventHandler(pic_Mouseleave);

            myStations.Add(tempstation);

            //把位置信息补充到新的电站信息里面DB     ——>感觉可以不要
            DataTable dt = new DataTable();
            OracleConnection con = new OracleConnection(conStr);//给对象con赋值，建立数据库连接
            con.Open();//打开数据库连接
            //todo:默认只读取湘江流域的可用电站
            string sql = "update stationinfo t set t.relativex = " + (tempstation.pox).ToString() +
                        ", t.relativey = " + (tempstation.poy).ToString() + "where t.stationid= " + tempstation.stationid;
            OracleCommand cmd = new OracleCommand(sql, con);
            cmd.ExecuteNonQuery();
            con.Close();//关闭数据库连接

            statusOption = 0;
        }

        private void panel3_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < myStations.Count; i++)
            {
                pictureBox1.Controls.Remove(myStations[i].picturebox);
                pictureBox1.Controls.Remove(myStations[i].lab_name);
            }
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(conStr))//给对象con赋值，建立数据库连接
            {
                con.Open();//打开数据库连接
                //todo:默认只读取湘江流域的可用电站
                string sql = "select * from STATIONINFO t where t.inuse=1 and t.riverid=140222 order by t.stationid";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(dt);
                con.Close();//关闭数据库连接
            }
            myStations = new List<Station>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Station tempstation = new Station();

                tempstation.stationid = Convert.ToInt32(dt.Rows[i]["stationid"]);
                tempstation.stationname = dt.Rows[i]["stationname"].ToString();

                tempstation.lab_name = new Label();
                tempstation.lab_name.Text = dt.Rows[i]["stationname"].ToString();

                tempstation.picturebox = new PictureBox();
                tempstation.picturebox.Height = 10;
                tempstation.picturebox.Width = 10;
                tempstation.picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
                tempstation.picturebox.SizeMode = PictureBoxSizeMode.StretchImage;

                tempstation.pox = Convert.ToDouble(dt.Rows[i]["relativex"]);
                tempstation.poy = Convert.ToDouble(dt.Rows[i]["relativey"]);

                tempstation.picturebox.Left = Convert.ToInt32(tempstation.pox * pictureBox1.Width - tempstation.picturebox.Width / 2.0);
                tempstation.picturebox.Top = Convert.ToInt32(tempstation.poy * pictureBox1.Height - tempstation.picturebox.Height / 2.0);
                tempstation.lab_name.Font = new Font("宋体", 12, FontStyle.Regular);
                tempstation.lab_name.Left = Convert.ToInt32(tempstation.pox * pictureBox1.Width - tempstation.picturebox.Width / 2.0) + 10;
                tempstation.lab_name.Top = Convert.ToInt32(tempstation.poy * pictureBox1.Height - tempstation.picturebox.Height / 2.0);
                tempstation.lab_name.BackColor = Color.Transparent;
                tempstation.picturebox.Parent = pictureBox1;
                tempstation.lab_name.Parent = pictureBox1;

                tempstation.picturebox.DoubleClick += new System.EventHandler(btn_Doubleclick);    //双击显示查看电站窗口
                tempstation.picturebox.MouseMove += new System.Windows.Forms.MouseEventHandler(pic_Mousemove);  //移动到上面换个红色图标
                tempstation.picturebox.MouseLeave += new System.EventHandler(pic_Mouseleave);  //移开换个绿色图标
                tempstation.picturebox.Click += new System.EventHandler(pic_mouseclick);

                myStations.Add(tempstation);
            }
        }

        /// <summary>
        /// 首先判断按键的状态，删除一个水库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pic_mouseclick(object sender, EventArgs e)
        {
            if (statusOption == 2)
            {
                for (int i = 0; i < myStations.Count; i++)
                {
                    if (myStations[i].picturebox == sender)   //遍历
                    {
                        DialogResult dg = MessageBox.Show("是否确认删除该电站", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dg == DialogResult.Yes)
                        {
                            pictureBox1.Controls.Remove(myStations[i].picturebox);
                            pictureBox1.Controls.Remove(myStations[i].lab_name);
                            using (OracleConnection con = new OracleConnection(conStr))
                            {
                                using (OracleCommand cmd = new OracleCommand())
                                {
                                    con.Open();//打开数据库连接
                                    string sql = "delete from stationinfo where stationid=" + myStations[i].stationid;
                                    cmd.CommandText = sql;   //这几个过程有什么用？
                                    cmd.Connection = con;
                                    cmd.ExecuteNonQuery();
                                    con.Close();//关闭数据库连接
                                }
                            }
                        }
                        break;
                    }
                }
            }
            statusOption = 0;
        }

        //不选择，所有电站变绿
        private void pic_Mouseleave(object sender, System.EventArgs e)//生成鼠标离开事件
        {
            for (int i = 0; i < myStations.Count; i++)
            {
                myStations[i].picturebox.Image = Image.FromFile("..\\图片\\bullet_green3.png");
            }
        }

        //针对已有电站，被选中电站变红
        private void pic_Mousemove(object sender, System.EventArgs e)//生成鼠标移入事件
        {
            for (int i = 0; i < myStations.Count; i++)
            {
                if (myStations[i].picturebox == sender)//pic被选定
                {
                    myStations[i].picturebox.Image = Image.FromFile("..\\图片\\bullet_red3.png");
                    break;
                }
            }
        }

        //添加电站
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (statusOption == 1)//添加电站
            {
                Point formPoint = pictureBox1.PointToClient(Control.MousePosition);//获取相对于picturebox1的鼠标坐标
                position_i = formPoint.X;
                position_j = formPoint.Y;
                staForm_c = new FormNewStation();//实例化b窗体
                staForm_c.MyEvent += new FormNewStation.MyDelegate(c_MyEvent);//监听b窗体事件
                staForm_c.ShowDialog();
                staForm_c.Dispose();
            }
            statusOption = 0;
        }

        /// <summary>
        /// 关闭首页会退出程序，会出现提示是否关闭首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stationmanage_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否退出程序", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        //这一步还没有做
        public class River  //存储流域信息
        {
            //流域编号及名称
            public Int32 riverid;

            public String rivername;

            //流域所包含的电站和上下游关系
            public List<Station> stations;
        };

        //定义一个station类
        public class Station
        {
            public Label lab_name;

            //电站在界面上的展示
            public PictureBox picturebox;

            //电站相对位置坐标
            public double pox;

            public double poy;
            public Int32 stationid;  //电站编号
            public String stationname; //电站名称
        }

        private void Stationmanage_Load(object sender, EventArgs e)
        {
            rivercb.Items.Clear();
            rivercb.Items.Add("湘江流域");
            rivercb.Items.Add("资水流域");
            rivercb.Items.Add("沅水流域");
            rivercb.Items.Add("澧水流域");
            rivercb.SelectedIndex = 0;

            xiangjiang.BringToFront();


        }

        private void rivercb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rivercb.SelectedIndex == 0)
            {
                xiangjiang.BringToFront();
            }
            else if (rivercb.SelectedIndex == 1)
            {
                zishui.BringToFront();
            }
            else if (rivercb.SelectedIndex == 2)
            {
                yuanshui.BringToFront();
            }
            else
            {
                lishui.BringToFront();
            }
        }
    }
}