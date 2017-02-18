using plat.function1;
using plat.function2.LongPlan;
using plat.function2.Mid_plan;
using plat.function2.Short_plan;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace plat
{
    public partial class FormMain : Form
    {
        private int iPlanSelectedNum = -1;//0，1，2分别对应短，中，长

        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)  //处理计算按钮
        {

            //制作计划
            SetNavigationButtonsPos(sender);

            switch (iPlanSelectedNum)
            {
                case 0:
                    button_ShortPlan_Click(button_ShortPlan, e);
                    break;

                case 1:
                    button_MidPlan_Click(button_MidPlan, e);
                    break;

                case 2:
                    button_LongPlan_Click(button_LongPlan, e);
                    break;

                default:
                    break;
            }

        }

        private void button_LongPlan_Click(object sender, EventArgs e)
        {
            //长期计划制作
            Cursor = Cursors.WaitCursor;
            this.PanelFrameParent.SuspendLayout();
            Frame_LongPlan frame_LongPlan = new Frame_LongPlan();
            frame_LongPlan.Dock = DockStyle.Fill;
            frame_LongPlan.Parent = PanelFrameParent;
            this.PanelFrameParent.ResumeLayout();
            Application.DoEvents();
            SetPlanButtons(sender);
            iPlanSelectedNum = 2;
            frame_LongPlan.BringToFront();
            Cursor = Cursors.Default;
        }

        private void button_MidPlan_Click(object sender, EventArgs e)
        {
            //中期计划制作
            this.PanelFrameParent.SuspendLayout();
            Cursor = Cursors.WaitCursor;
            SetPlanButtons(sender);
            Frame_MidPlan frame_MidPlan = new Frame_MidPlan();
            frame_MidPlan.Dock = DockStyle.Fill;
            frame_MidPlan.Parent = PanelFrameParent;
            this.PanelFrameParent.ResumeLayout();
            Application.DoEvents();
            iPlanSelectedNum = 1;
            frame_MidPlan.BringToFront();
            Cursor = Cursors.Default;
        }

        private void button_ShortPlan_Click(object sender, EventArgs e)
        {
            //短期计划制作
            this.PanelFrameParent.SuspendLayout();
            Cursor = Cursors.WaitCursor;
            SetPlanButtons(sender);
            Frame_ShortPlan frame_ShortPlan = new Frame_ShortPlan();
            //frame_ShortPlan.Visible = false;
            frame_ShortPlan.Dock = DockStyle.Fill;
            frame_ShortPlan.Parent = PanelFrameParent;
            System.Threading.Thread.Sleep(1000);
            frame_ShortPlan.Visible = true;
            this.PanelFrameParent.ResumeLayout();
            Application.DoEvents();
            System.Threading.Thread.Sleep(300);
            iPlanSelectedNum = 0;
            frame_ShortPlan.BringToFront();
            ClearFramesOfPanelParent(frame_ShortPlan);
            Cursor = Cursors.Default;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SetNavigationButtonsPos(sender);
            panel_Plan.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)   //机组数据管理按钮
        {
            SetNavigationButtonsPos(sender);
            Stationmanage stationmanage = new Stationmanage();
            panel_Plan.Visible = false;
            stationmanage.Show();
        }

        /// <summary>
        /// 消除frame中panel往父控件上靠时的闪烁现象
        /// </summary>
        /// <param name="frame"></param>
        private void ClearFramesOfPanelParent(UserControl frame)
        {
            //测试时间

            foreach (Control tempUserControl in PanelFrameParent.Controls)
            {
                if (tempUserControl is System.Windows.Forms.UserControl && tempUserControl != frame)
                {
                    System.Windows.Forms.UserControl userControl = (System.Windows.Forms.UserControl)tempUserControl;
                    PanelFrameParent.Controls.Remove(userControl);
                    if (!userControl.IsDisposed)
                    {
                        userControl.Dispose();
                    }
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            panel_Plan.Visible = false;
        }

        //设置 导航按钮 显示
        private void SetNavigationButtonsPos(object sender)
        {
            Button tmpButton = (Button)sender;

            //TODO 可否采用这个来精简代码呢  panel_Left.Controls.Find("button1", true);
            if (tmpButton.Name == "button1")
            {
                //展开 制作计划
                panel_Plan.Visible = true;

                this.panel_Left.Controls.Add(this.panel_Plan);
                panel_Plan.Dock = DockStyle.Fill;

                this.panel_Left.Controls.Add(this.button1);
                button1.Dock = DockStyle.Top;

                this.panel_Left.Controls.Add(this.button3);
                button3.Dock = DockStyle.Top;

                this.panel_Left.Controls.Add(this.button2);
                button2.Dock = DockStyle.Bottom;

                this.panel_Left.Controls.Add(this.button4);
                button4.Dock = DockStyle.Bottom;
            }
            else
            {
                //所有按钮居上显示
                panel_Plan.Visible = false;

                this.panel_Left.Controls.Add(this.button4);
                button4.Dock = DockStyle.Top;

                this.panel_Left.Controls.Add(this.button2);
                button2.Dock = DockStyle.Top;

                this.panel_Left.Controls.Add(this.button1);
                button1.Dock = DockStyle.Top;

                this.panel_Left.Controls.Add(this.button3);
                button3.Dock = DockStyle.Top;
            }
        }

        private void SetPlanButtons(object sender)
        {
            this.panel_Plan.SuspendLayout();

            Button tmpButton = (Button)sender;
            foreach (Control tempcon in panel_Plan.Controls)
            {
                if (tempcon.GetType().ToString().Equals("System.Windows.Forms.Button"))
                {
                    Button btn = (System.Windows.Forms.Button)tempcon;
                    btn.BackColor = SystemColors.Window;
                }
            }

            this.panel_Plan.ResumeLayout();
            Application.DoEvents();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetNavigationButtonsPos(sender);
        }
    }
}