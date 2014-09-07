using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BeeHouse
{
    public partial class FormMain : Form
    {
        private Random random = new Random();

        public FormMain()
        {
            InitializeComponent();
        }

        private void DrawCell(Graphics grp,PointF centerPoint,float radius,double angleOffset)
        {
            //设置随机颜色
            Color colorRandom = Color.FromArgb(
                                                                    this.random.Next(0,256),
                                                                    this.random.Next(0,256),
                                                                    this.random.Next(0,256));
            //30度角
            double anglePer = 2 * Math.PI / 6f;

            PointF[] points = new PointF[6];
            //计算每个点的位置坐标
            for (int index = 0; index < 6; index++)
            {
                PointF point = new PointF();
                point.X = (float)(centerPoint.X + radius * Math.Cos(anglePer * index + angleOffset));
                point.Y = (float)(centerPoint.Y + radius * Math.Sin(anglePer * index + angleOffset));

                points[index] = point;
            }

            Pen pen = new Pen(colorRandom);
            pen.Width = 2;
            //画六边形
            grp.DrawPolygon(pen,points);

            Brush brush = new SolidBrush(colorRandom);
            //画每个六边形的中心小圆
            grp.FillEllipse(brush,new RectangleF(centerPoint.X -4f,
                                                                                     centerPoint.Y - 4f,
                                                                                     2 * 4f,
                                                                                     2 * 4f));
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            tmShow.Start();
            int n = int.Parse(this.txtRowCount.Text);

            float height= this.panHouse.Height;
            float width = this.panHouse.Width;
            float padding = 3f;
            //得到小六边形的外接圆的半径
            float radius = (height - (n + 1) * padding) / (1.5f * n + 0.5f);
            //double angleOffset = 2 * Math.PI / 12f;

            Graphics grp = this.panHouse.CreateGraphics();
            grp.Clear(this.panHouse.BackColor);

            PointF startPoint = new PointF();
            startPoint.X = width / 2f;
            startPoint.Y = padding + radius;
            //调用DrawCell()方法画第一个小六边形
            this.DrawCell(grp, startPoint, radius, Math.PI / 6f);

            //根据每个小正六边形中点的位置画其他小正六边形
            for (int row = 1; row < n; row++)
            {
                PointF centerPointFirst = new PointF();
                centerPointFirst.X = (float)(startPoint.X - (padding / 2f + radius * Math.Cos(Math.PI / 6f)) * row);
                centerPointFirst.Y = (float)(startPoint.Y + ((2 * radius * Math.Sin(Math.PI / 3f) + padding) * Math.Sin(Math.PI / 3f)) * row);

                this.DrawCell(grp, centerPointFirst, radius, Math.PI / 6f);
                for (int column = 1; column <= row; column++)
                {
                    PointF centerPoint_N = new PointF();
                    centerPoint_N.X = (float)(centerPointFirst.X + (2 * radius * Math.Cos(Math.PI / 6f) + padding) * column);
                    centerPoint_N.Y = centerPointFirst.Y;

                    this.DrawCell(grp, centerPoint_N, radius, Math.PI / 6f);
                }
            }
//废弃代码
#region
            //PointF centerPoint_1 = new PointF();
            //centerPoint_1.X = w / 2f;
            //centerPoint_1.Y = padding + radius;

            //this.DrawCell(grp, centerPoint_1, radius, angleOffset);

            //PointF centerPoint_2_1 = new PointF();
            //centerPoint_2_1.X = (float)(centerPoint_1.X - padding / 2f - radius * Math.Cos(Math.PI / 6f));
            //centerPoint_2_1.Y = (float) (centerPoint_1.Y + (2 * radius * Math.Sin(Math.PI /3f) + padding) * Math.Sin(Math.PI/3f) );

            //this.DrawCell(grp, centerPoint_2_1, radius, angleOffset);
            
            //PointF centerPoint_2_2 = new PointF();
            //centerPoint_2_2.X = (float)(centerPoint_1.X + padding / 2f + radius * Math.Cos(Math.PI / 6f));
            //centerPoint_2_2.Y = (float)(centerPoint_1.Y + (2 * radius * Math.Sin(Math.PI / 3f) + padding) * Math.Sin(Math.PI / 3f));
            //this.DrawCell(grp, centerPoint_2_2, radius, angleOffset);

#endregion  
        }

       //时钟，产生动态效果
        private void tmShow_Tick(object sender, EventArgs e)
        {
            this.btnGenerate_Click(null, null);
        }
    }
}
