using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProOperator;
using System.IO;
using System.Reflection;


namespace ProFactory
{
    public class factory
    {
        public static Operation GetOpre(string type)
        {
            Operation oper = null;
            //获得debug下plus文件夹所有的路径
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dll");
            string[] files = Directory.GetFiles(path, "*.dll");
            foreach (string file in files)
            {
                Assembly ass = Assembly.LoadFile(file);
                //获得程序集中所有元数据
                Type[] types = ass.GetExportedTypes();
                foreach (Type tt in types)
                {
                    if (tt.IsSubclassOf(typeof(Operation)))
                    {
                        oper = (Operation)Activator.CreateInstance(tt);
                    }
                    if (type == oper.type)
                    {
                        return oper;
                    }
                    else
                    {
                        oper = null;
                    }
                }
            }
            return oper;
        }

    }
}
