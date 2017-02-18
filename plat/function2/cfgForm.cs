using plat.function2.function2;
using System;
using System.Windows.Forms;

namespace plat
{
    public partial class cfgForm : Form
    {
        private string dllname;//模型名字

        public cfgForm()
        {
            InitializeComponent();
        }

        public cfgForm(string modelname)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;//居中
            dllname = modelname;
            label8.Text = "添加算法模型：" + dllname;
        }

        /// <summary>
        /// 确定，并写配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //写文件
            if (System.IO.File.Exists(DataLayer.cfgFilePath))//检测文件是存在
            {
                INIOperationClass.INIWriteValue(DataLayer.cfgFilePath, dllname, "kurong", "1");
                INIOperationClass.INIWriteValue(DataLayer.cfgFilePath, dllname, "ModelSign", textBox2.Text);

                MessageBox.Show("成功将" + dllname + "模型相关信息添加到配置文件");
            }
            this.Close();
        }
    }
}