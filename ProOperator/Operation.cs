using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProOperator
{
    public abstract class Operation
    {
        //public OutData data;
        //public IuputData data_;

        public List<int> result;
        public abstract string type { get; }
        public abstract int[] output();

        //一些基础参数
        public const double K = 7.5;       //K值
        public const int maxshuiwei = 157;
        public const int minshuiwei = 139;
        public const double maxchuli = 900;
        public const double minchuli = 30;

        //public double[,] laishui = new double[71, 12];


        public List<double> shuiwei = new List<double>();
        public List<double> kurong = new List<double>();
        public List<double> xiayoushuiwei = new List<double>();
        public List<double> xiaxieliuliang = new List<double>();
        public double[,] laishui = new double[71, 12];

        //public List<List<double>> laishui = new List<List<double>>();
        public int shijian;
        public int shujugeshu;
        public int maxchukuliuliang;
        public int minchukuliuliang;
        public int maxshuilunji;
        public double N1;
        public double N2;

        //output
        public double[] chuli = new double[12];
        public double[] qishui1 = new double[12];
        public double[] route1 = new double[13];
        public double NNmax;
        public double[] chuku = new double[12];
        public string[] V = new string[12];

        //抽象方法，需要在子类中重写
        public abstract void input(List<double> Basic_shuiwei, List<double> Basic_kurong, List<double> Basic_xiayoushuiwei, List<double> Basic_xiaxieliuliang, double[,] Basic_laishui,
           int Basic_shijian, int Basic_shujugeshu, int Basic_maxchukuliuliang, int Basic_minchukuliuliang, int Basic_maxshuilunji, double Basic_N1, double Basic_N2);


    }
}
