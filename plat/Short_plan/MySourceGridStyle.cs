using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plat.function2.Short_plan
{
    class MySourceGridStyle
    {
        public class FirstColumnHeaderView : SourceGrid.Cells.Views.ColumnHeader
        {
            public FirstColumnHeaderView()
            {
                //第一列格式
                //viewFirstColumn.Border = cellBorder;

                DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
                //backHeader.BackColor = Color.FromArgb(248, 248, 248);//选中的颜色
                backHeader.BackColor = ColorTranslator.FromHtml("#f3eff7");//ColorTranslator.FromHtml("#c6ddf4");//AliceBlue
                backHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;
                //backHeader.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.FromArgb(192, 192, 192), 1), new DevAge.Drawing.BorderLine(Color.FromArgb(192, 192, 192), 1));
                this.Background = backHeader;
                this.ForeColor = Color.Black;
                //viewFirstColumn.Font = new Font(FontFamily.GenericSansSerif, 10);
                this.Font = new Font("Times New Roman", 10, FontStyle.Bold);

                this.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;//居中对齐
            }
        }

        //liuc...20161031:设置行首电站名单元格的格式（为了使不能用鼠标拖动，设置其格式为Cell而非MyHeader）
        public class StationRowHeaderView : SourceGrid.Cells.Views.RowHeader
        {
            public StationRowHeaderView()
            {
                DevAge.Drawing.VisualElements.RowHeader backHeader = new DevAge.Drawing.VisualElements.RowHeader();
                backHeader.BackColor = Color.FromArgb(248, 248, 248);
                backHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;
                this.Background = backHeader;
                this.ForeColor = Color.Black;
                this.Font = new Font("微软雅黑", 9);
                this.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            }
        }

        public class NormalCellAlternateBackColorView : SourceGrid.Cells.Views.Cell
        {
            public NormalCellAlternateBackColorView()
            {
                //内容格式Views
                DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);//(Color.DarkKhaki, 1);
                DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);

                setCellBackColorAlternate(ColorTranslator.FromHtml("#f3eff7"), Color.White);//dceef8(Color.White, Color.AliceBlue);FromArgb(212, 229, 247)
                this.Font = new Font("Times New Roman", 10);
                this.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                this.Border = cellBorder;
            }
            private void setCellBackColorAlternate(Color firstColor, Color secondColor)
            {
                FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
                SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
            }

            private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
            public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
            {
                get { return mFirstBackground; }
                set { mFirstBackground = value; }
            }

            private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
            public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
            {
                get { return mSecondBackground; }
                set { mSecondBackground = value; }
            }

            protected override void PrepareView(SourceGrid.CellContext context)
            {
                base.PrepareView(context);

                if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                    Background = FirstBackground;
                else
                    Background = SecondBackground;
            }
        }


        //长期使用的
        public class BackColorView : SourceGrid.Cells.Views.Cell
        {
            public BackColorView()
            {

                //内容格式Views
                DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);//(Color.DarkKhaki, 1);
                DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
                setCellBackColorAlternate(Color.Red);//ColorTranslator.FromHtml("#f3eff7"), 
                //setCellBackColorAlternate(ColorTranslator.FromHtml("#f3eff7"), Color.White);//dceef8(Color.White, Color.AliceBlue);FromArgb(212, 229, 247)
                this.Font = new Font("Times New Roman", 10);
                this.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                this.Border = cellBorder;
            }
            private void setCellBackColorAlternate(Color secondColor)//Color firstColor, 
            {
                //FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(firstColor);
                SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(secondColor);
            }
            //private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
            //public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
            //{
            //    get { return mFirstBackground; }
            //    set { mFirstBackground = value; }
            //}

            private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
            public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
            {
                get { return mSecondBackground; }
                set { mSecondBackground = value; }
            }
            protected override void PrepareView(SourceGrid.CellContext context)
            {
                base.PrepareView(context);

                //if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                //    Background = FirstBackground;
                //else
                Background = SecondBackground;
            }
        }
    }
    public class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value)
            : base(value)
        {
            //1 Header Row
            //ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            backHeader.BackColor = Color.FromArgb(248, 248, 248);
            backHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.Solid;//颜色格式：不渐变
            //backHeader.BackgroundColorStyle = DevAge.Drawing.BackgroundColorStyle.None;//不显示BackColor

            //backHeader.BackColor = Color.FromArgb(135, 184, 222); //ColorTranslator.FromHtml("#c6ddf4");//ColorTranslator.FromHtml("#f3eff7");// Color.FromArgb(135, 184, 222);

            //backHeader.Border = new DevAge.Drawing.RectangleBorder(new DevAge.Drawing.BorderLine(Color.FromArgb(192, 192, 192), 1), new DevAge.Drawing.BorderLine(Color.FromArgb(192, 192, 192), 1));
            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.Black; //ColorTranslator.FromHtml("#4f4545");
            //viewColumnHeader.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            viewColumnHeader.Font = new Font("微软雅黑", 9);
            viewColumnHeader.WordWrap = true;

            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.White, 1);//(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
            //viewColumnHeader.Border = cellBorder;

            //SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            //view.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            //view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            View = viewColumnHeader;

            AutomaticSortEnabled = false;
        }
    }
    public class MyHeaderBottomFixedRow : MyHeader
    {
        public MyHeaderBottomFixedRow(object value)
            : base(value)
        {
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(ColorTranslator.FromHtml("#a8a3a3"), 1);//(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(DevAge.Drawing.BorderLine.NoBorder, border, DevAge.Drawing.BorderLine.NoBorder, DevAge.Drawing.BorderLine.NoBorder);
            View.Border = cellBorder;
        }
    }

    /// <summary>
    /// 创建浮点型cell类
    /// </summary>
    public class MyDoubleCell : SourceGrid.Cells.Cell
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatString">数值格式</param>
        /// <param name="canEdit">是否可编辑</param>
        public MyDoubleCell(string formatString, bool canEdit)
            : base()
        {
            SourceGrid.Cells.Views.Cell myview = new SourceGrid.Cells.Views.Cell();
            myview.BackColor = Color.White;//背景色
            myview.Font = new Font("Times New Roman", 10);
            myview.ForeColor = Color.Black;//字体颜色
            myview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;//居中 

            this.View = myview;

            //编辑器
            SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));
            //按键判断，是否为数字或者小数点
            numericEditor.KeyPress += delegate(object sender, KeyPressEventArgs keyArgs)
            {
                bool isValid = char.IsNumber(keyArgs.KeyChar) || keyArgs.KeyChar == (char)Keys.Back ||
                    (keyArgs.KeyChar == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0] && numericEditor.GetEditedValue().ToString().IndexOf(".") == -1);
                keyArgs.Handled = !isValid;

            };
            //格式化数值
            DevAge.ComponentModel.Converter.NumberTypeConverter numberFormatCustom = new DevAge.ComponentModel.Converter.NumberTypeConverter(typeof(double));
            numberFormatCustom.Format = formatString;
            numericEditor.TypeConverter = numberFormatCustom;

            //是否可编辑
            numericEditor.EnableEdit = canEdit;
            this.Editor = numericEditor;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">cell的值</param>
        /// <param name="formatString">数值格式</param>
        /// <param name="canEdit">是否可编辑</param>
        public MyDoubleCell(double value, string formatString, bool canEdit)
            : this(formatString, canEdit)
        {
            this.Value = value;
        }


    }

    /// <summary>
    /// 创建string型cell
    /// </summary>
    public class MyStringCell : SourceGrid.Cells.Cell
    {
        public MyStringCell(string value)
            : base(value)
        {
            SourceGrid.Cells.Views.Cell myview = new SourceGrid.Cells.Views.Cell();
            myview.BackColor = Color.White;
            myview.ForeColor = Color.Black;
            myview.Font = new Font("微软雅黑", 10);
            myview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            this.View = myview;
        }
    }

    /// <summary>
    /// cell编辑控制器类
    /// </summary>
    public class EditEndedEvent : SourceGrid.Cells.Controllers.ControllerBase
    {
        public delegate void EditValueDelegate(SourceGrid.Position position);
        public EditValueDelegate getEditValue;

        object value;
        //开始编辑事件
        public override void OnEditStarted(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnEditStarted(sender, e);
            value = sender.Value;//原来的值
        }
        //编辑完成事件
        public override void OnEditEnded(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnEditEnded(sender, e);
            //判定编辑的值是否变化
            if (!object.Equals(sender.Value, value))
            {
                sender.Cell.View.ForeColor = Color.DarkOrange;//字体变颜色
                if (getEditValue != null)
                {
                    getEditValue(sender.Position);//委托
                }
            }


            //if (sender.Grid.Name=="grid_time"||sender.Grid.Name=="grid_out")
            //{
            //    if (sender.Position.Row < sender.Grid.Rows.Count - 1)
            //    {
            //        SourceGrid.Position p = new SourceGrid.Position(sender.Position.Row + 1, sender.Position.Column);
            //        sender.Grid.Selection.Focus(p, true);
            //    }                
            //}
        }

        public override void OnValueChanged(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnValueChanged(sender, e);
            sender.Cell.View.ForeColor = Color.DarkOrange;//字体变颜色

            if (getEditValue != null)
            {
                getEditValue(sender.Position);//委托
            }
        }

        public override void OnMouseMove(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseMove(sender, e);
            //判定是否显示ContextMenuStrip
            if (sender.Grid.Name == "grid_time" || sender.Grid.Name == "grid_out")
            {
                if (sender.Grid.Selection.IsSelectedCell(sender.Position) && sender.Cell.Editor.EnableEdit)
                {
                    sender.Grid.ContextMenuStrip.Items[0].Visible = true;
                    sender.Grid.ContextMenuStrip.Items[1].Visible = true;
                }
                else
                {
                    sender.Grid.ContextMenuStrip.Items[0].Visible = false; ;
                    sender.Grid.ContextMenuStrip.Items[1].Visible = false;
                }
            }
        }
    }

    /// <summary>
    /// cellcheckbox编辑控制器类
    /// </summary>
    public class CellCheckBoxEvent : SourceGrid.Cells.Controllers.CheckBox
    {
        public delegate void CheckBoxDelegate(SourceGrid.Position position, bool state);
        public CheckBoxDelegate getCheckBoxValue;
        public override void OnClick(SourceGrid.CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
            sender.Value = !Convert.ToBoolean(sender.Value);
            if (getCheckBoxValue != null)
            {
                getCheckBoxValue(sender.Position, Convert.ToBoolean(sender.Value));
            }
        }
    } 
}
