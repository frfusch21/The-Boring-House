using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        POINT3D VRP = new POINT3D();
        POINT3D VPN = new POINT3D();
        POINT3D VUP = new POINT3D();
        POINT3D DOP = new POINT3D();
        POINT3D COP = new POINT3D();
        POINT3D nUnitVectorVPN = new POINT3D();
        POINT3D upUnitVectorVUP = new POINT3D();
        POINT3D vUnitVectorUP = new POINT3D();
        POINT3D u = new POINT3D();
        POINT2D WinMax = new POINT2D();
        POINT2D WinMin = new POINT2D();
        POINT2D WinCenter = new POINT2D();
        POINT3D[] V =
            {
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
            };
        POINT3D[] VR =
        {
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
                new POINT3D(),
        };
        int cX,cY;

        List<double[]> Vt = new List<double[]>() 
        {
            new double[]{1,0,0,0},
            new double[]{0,1,0,0},
            new double[]{0,0,0,0},
            new double[]{0,0,0,1}
        };

        List<double[]> Wt = new List<double[]>()
        {
            new double[]{1,0,0,0},
            new double[]{0,1,0,0},
            new double[]{0,0,1,0},
            new double[]{0,0,0,1}
        };
        List<double[]> St = new List<double[]>()
        {
            new double[]{150,0,0,0},
            new double[]{0,-150,0,0},
            new double[]{0,0,0,0},
            new double[]{300,300,0,1}
        };
        double FP, BP;
        public void set3DPoint(POINT3D P, double x, double y, double z)
        {
            P.x = x;
            P.y = y;
            P.z = z;
            P.w = 1;
        }

        public void set2DPoint(POINT2D P, double x, double y)
        {
            P.x = x;
            P.y = y;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cX = pictureBox1.Width / 2;
            cY = pictureBox1.Height / 2;
            set3DPoint(V[0], -1, -1, -1);
            set3DPoint(V[1], 1, -1, -1);
            set3DPoint(V[2], 1, 0, -1);
            set3DPoint(V[3], 0, 1, -1);
            set3DPoint(V[4], -1, 0, -1);
            set3DPoint(V[5], -1, -1, 1);
            set3DPoint(V[6], 1, -1, 1);
            set3DPoint(V[7], 1, 0, 1);
            set3DPoint(V[8], 0, 1, 1);
            set3DPoint(V[9], -1, 0, 1);

            set3DPoint(VR[0], -1, -1, -1);
            set3DPoint(VR[1], 1, -1, -1);
            set3DPoint(VR[2], 1, 0, -1);
            set3DPoint(VR[3], 0, 1, -1);
            set3DPoint(VR[4], -1, 0, -1);
            set3DPoint(VR[5], -1, -1, 1);
            set3DPoint(VR[6], 1, -1, 1);
            set3DPoint(VR[7], 1, 0, 1);
            set3DPoint(VR[8], 0, 1, 1);
            set3DPoint(VR[9], -1, 0, 1);

        }

        private void button4_Click(object sender, EventArgs e)
        {

                defaultSettings();
                InitParams();
                calcUVN();
                Transformation();
                newPoint();
                draw();

        }
        public void InitParams()
        {
            set3DPoint(VRP, Convert.ToInt32(vrpX.Text), Convert.ToInt32(vrpY.Text), Convert.ToInt32(vrpZ.Text));
            set3DPoint(VPN, Convert.ToInt32(vpnX.Text), Convert.ToInt32(vpnY.Text), Convert.ToInt32(vpnZ.Text));
            set3DPoint(VUP, Convert.ToInt32(vupX.Text), Convert.ToInt32(vupY.Text), Convert.ToInt32(vupZ.Text));
            set3DPoint(COP, Convert.ToInt32(copX.Text), Convert.ToInt32(copY.Text), Convert.ToInt32(copZ.Text));
            set2DPoint(WinMin, Convert.ToInt32(wminX.Text), Convert.ToInt32(wminY.Text));
            set2DPoint(WinMax, Convert.ToInt32(wmaxX.Text), Convert.ToInt32(wmaxY.Text));
            set2DPoint(WinCenter, (WinMin.x + WinMax.x) / 2, (WinMin.y + WinMax.y) / 2);
            set3DPoint(DOP, WinCenter.x - COP.x, WinCenter.y - COP.y, -COP.z);

            FP = Convert.ToInt32(fplane.Text);
            BP = Convert.ToInt32(bplane.Text);
        }

        public void calcUVN()
        {
            object obj1 = UnitVector(VPN.x, VPN.y, VPN.z);
            POINT3D tpoint2 = new POINT3D();
            nUnitVectorVPN = obj1 != null ? (POINT3D)obj1 : tpoint2;
           

            object obj = UnitVector(VUP.x, VUP.y, VUP.z);
            POINT3D tpoint = new POINT3D();
            upUnitVectorVUP = obj != null ? (POINT3D)obj : tpoint;


            double num = DotProduct(nUnitVectorVPN, upUnitVectorVUP);
            POINT3D P = new POINT3D();
            set3DPoint(P, upUnitVectorVUP.x - num * nUnitVectorVPN.x, upUnitVectorVUP.y - num * nUnitVectorVPN.y, upUnitVectorVUP.z - num * nUnitVectorVPN.z);

            object obj2 = UnitVector(P.x, P.y, P.z);
            vUnitVectorUP = obj2 != null ? (POINT3D)obj2 : tpoint;

            u = CrossProduct(vUnitVectorUP, nUnitVectorVPN);
        }

        public object UnitVector(double x, double y, double z)
        {
            double num = Math.Sqrt(x * x + y * y + z * z);
            POINT3D P = new POINT3D();
            set3DPoint(P, x / num, y / num, z / num);
            return (object)P;
        }
        public POINT3D CrossProduct(POINT3D v1, POINT3D v2)
        {
            POINT3D tpoint = new POINT3D();
            tpoint.x = v1.y * v2.z - v1.z * v2.y;
            tpoint.y = v1.z * v2.x - v1.x * v2.z;
            tpoint.z = v1.x * v2.y - v1.y * v2.x;
            return tpoint;
        }
        public double DotProduct(POINT3D v1, POINT3D v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public void Transformation()
        {
            //Set Transformation Matrix
            List<double[]> T1 = new List<double[]>()
            {
                new double[]{ 1,0,0,0},
                new double[]{ 0,1,0,0},
                new double[]{ 0,0,1,0},
                new double[]{ -VRP.x,-VRP.y,-VRP.z, 1},
            };

            List<double[]> T2 = new List<double[]>()
            {
                new double[]{ u.x , vUnitVectorUP.x, nUnitVectorVPN.x, 0},
                new double[]{ u.y , vUnitVectorUP.y, nUnitVectorVPN.y, 0},
                new double[]{ u.z , vUnitVectorUP.z, nUnitVectorVPN.z, 0},
                new double[]{ 0 , 0, 0,1},
            };

            //multiply T1.T2
            Vt = VtMatrixMultiplication(T1, T2);

            List<double[]> T3 = new List<double[]>()
            {
                new double[]{ 1,0,0,0},
                new double[]{ 0,1,0,0},
                new double[]{ -DOP.x/DOP.z,-DOP.y/DOP.z,1,0},
                new double[]{ 0,0,0,1},
            };

            //prev result multiply with T3 and so on
            Vt = VtMatrixMultiplication(Vt, T3);


            List<double[]> T4 = new List<double[]>()
            {
                new double[]{ 1,0,0,0},
                new double[]{ 0,1,0,0},
                new double[]{ 0,0,1,0},
                new double[]{ -WinCenter.x,-WinCenter.y,-FP, 1},
            };

            Vt = VtMatrixMultiplication(Vt, T4);

            List<double[]> T5 = new List<double[]>()
            {
                new double[]{ 2 / (WinMax.x - WinMin.x),0,0,0},
                new double[]{ 0, 2 / (WinMax.y - WinMin.y), 0,0},
                new double[]{ 0,0,1 / (FP - BP),0},
                new double[]{ 0,0,0,1},
            };

            Vt = VtMatrixMultiplication(Vt, T5);

        }
        public List<double[]> VtMatrixMultiplication(List<double[]> m1, List<double[]> m2)
        {
            List<double[]> temp = new List<double[]>()
            {
                new double[]{0,0,0,0},
                new double[]{0,0,0,0},
                new double[]{0,0,0,0},
                new double[]{0,0,0,0}
            };
            for (int i = 0; i<4; i++)
            {
                temp[i][0] = (m1[i][0] * m2[0][0]) + (m1[i][1] * m2[1][0]) + (m1[i][2] * m2[2][0]) + (m1[i][3] * m2[3][0]);
                temp[i][1] = (m1[i][0] * m2[0][1]) + (m1[i][1] * m2[1][1]) + (m1[i][2] * m2[2][1]) + (m1[i][3] * m2[3][1]);
                temp[i][2] = (m1[i][0] * m2[0][2]) + (m1[i][1] * m2[1][2]) + (m1[i][2] * m2[2][2]) + (m1[i][3] * m2[3][2]);
                temp[i][3] = (m1[i][0] * m2[0][3]) + (m1[i][1] * m2[1][3]) + (m1[i][2] * m2[2][3]) + (m1[i][3] * m2[3][3]);

            }
            return temp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            pictureBox1.Image = null;
        }

        public POINT3D[] calcNewPoint(POINT3D[] P, List<double[]> m1)
        {
            POINT3D[] target = 
            {
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
                new POINT3D { },
            };
            for (int i = 0; i<10;i++)
            {
                target[i].x = P[i].x * m1[0][0] + P[i].y * m1[1][0] + P[i].z * m1[2][0] + P[i].w * m1[3][0];
                target[i].y = P[i].x * m1[0][1] + P[i].y * m1[1][1] + P[i].z * m1[2][1] + P[i].w * m1[3][1];
                target[i].z = P[i].x * m1[0][2] + P[i].y * m1[1][2] + P[i].z * m1[2][2] + P[i].w * m1[3][2];
                target[i].w = P[i].x * m1[0][3] + P[i].y * m1[1][3] + P[i].z * m1[2][3] + P[i].w * m1[3][3];
            }

            return target;
        }
        public void newPoint()
        {
            //calculate P'
                VR = calcNewPoint(VR, Wt);
                VR = calcNewPoint(VR, Vt);
                VR = calcNewPoint(VR, St);
                
            /*            MessageBox.Show("P1 : " + VR[0].x + " " + VR[0].y + " " + VR[0].z + "\n" +
                                        "P2 : " + VR[1].x + " " + VR[1].y + " " + VR[1].z + "\n" +
                                        "P3 : " + VR[2].x + " " + VR[2].y + " " + VR[2].z + "\n" +
                                        "P4 : " + VR[3].x + " " + VR[3].y + " " + VR[3].z + "\n" +
                                        "P5 : " + VR[4].x + " " + VR[4].y + " " + VR[4].z + "\n" +
                                        "P6 : " + VR[5].x + " " + VR[5].y + " " + VR[5].z + "\n" +
                                        "P7 : " + VR[6].x + " " + VR[6].y + " " + VR[6].z + "\n" +
                                        "P8 : " + VR[7].x + " " + VR[7].y + " " + VR[7].z + "\n" +
                                        "P9 : " + VR[8].x + " " + VR[8].y + " " + VR[8].z + "\n");*/

        }
        public void draw()
        {
            Bitmap DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            Graphics g = Graphics.FromImage(DrawArea);
            int index1 = 0;
            int index2 = 5;
            for (int i = 0; i < 5; i++)
            {
                //draw back
                if (i != 4)
                    g.DrawLine(Pens.Black, Convert.ToInt32(VR[i].x) , Convert.ToInt32(VR[i].y) , Convert.ToInt32(VR[i + 1].x) , Convert.ToInt32(VR[i + 1].y) );
                else
                    g.DrawLine(Pens.Black, Convert.ToInt32(VR[i].x) , Convert.ToInt32(VR[i].y) , Convert.ToInt32(VR[0].x) , Convert.ToInt32(VR[0].y) );
            }
            do
            {
                //draw other line 
                g.DrawLine(Pens.Black, Convert.ToInt32(VR[index1].x) , Convert.ToInt32(VR[index1].y) , Convert.ToInt32(VR[index2].x) , Convert.ToInt32(VR[index2].y) );
                index1++;
                index2++;
            } while (index1 != 5 && index2 != 10);
            for (int i = 5; i < 10; i++)
            {
                //draw front
                if (i != 9)
                    g.DrawLine(Pens.Red, Convert.ToInt32(VR[i].x) , Convert.ToInt32(VR[i].y) , Convert.ToInt32(VR[i + 1].x) , Convert.ToInt32(VR[i + 1].y) );
                else
                    g.DrawLine(Pens.Red, Convert.ToInt32(VR[i].x) , Convert.ToInt32(VR[i].y) , Convert.ToInt32(VR[5].x) , Convert.ToInt32(VR[5].y) );
            }
            //draw window
            g.DrawLine(Pens.Black, 150, 150, 150, 450);
            g.DrawLine(Pens.Black, 150, 450, 450, 450);
            g.DrawLine(Pens.Black, 450, 450, 450, 150);
            g.DrawLine(Pens.Black, 450, 150, 150, 150);

            pictureBox1.Image = DrawArea;
        }
        public void defaultSettings()
        {
            for (int i = 0; i < 10; i++)
            {
                VR[i].x = V[i].x;
                VR[i].y = V[i].y;
                VR[i].z = V[i].z;
                VR[i].w = V[i].w;
            }

        }

    }
}
