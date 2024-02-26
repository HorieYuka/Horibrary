using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate.Base
{
    internal class MathFunc
    {

        // 2 Dimensional Convolution Method from MATLAB, Porting in C# Simply. (conv2 / same)
        private static double[,] Conv_2D(double[,] Dim1, double[,] Dim2)
        {
            int Dim1_H = Dim1.GetLength(0);
            int Dim1_W = Dim1.GetLength(1);
            int Dim2_H = Dim2.GetLength(0);
            int Dim2_W = Dim2.GetLength(1);

            int Center_H = (int)Math.Floor((double)(Dim2_H + 1) / 2);
            int Center_W = (int)Math.Floor((double)(Dim2_W + 1) / 2);

            // Four Location Pixels of Center
            int Center_L = Center_W - 1;
            int Center_R = Dim2_W - Center_W;
            int Center_U = Center_H - 1;
            int Center_D = Dim2_H - Center_H;

            double[,] Ref = new double[Dim1_H + Center_U + Center_D, Dim1_W + Center_L + Center_R];
            for (int X = Center_U; X < Dim1_H + Center_U; X++)
            {
                for (int Y = Center_L; Y < Center_L + Dim1_W; Y++)
                {
                    Ref[X, Y] = Dim1[X - Center_U, Y - Center_L];
                }
            }

            double[,] Dim2_Rot90 = new double[Dim2_H, Dim2_W];
            Dim2_Rot90 = Rot90(Dim2);

            double[,] Out = new double[Dim1_H, Dim1_W];
            for (int X = 0; X < Dim1_H; X++)
            {
                for (int Y = 0; Y < Dim1_W; Y++)
                {
                    for (int i = 0; i < Dim2_H; i++)
                    {
                        for (int j = 0; j < Dim2_W; j++)
                        {
                            Out[X, Y] += (Ref[i + X, j + Y] * Dim2_Rot90[i, j]);
                        }
                    }
                }
            }

            return Out;
        }

        // Bilinear Interpolation
        private static double[,] BiIntPol(double[,] Matrix, double Scale)
        {
            int Height = Matrix.GetLength(0);
            int Width = Matrix.GetLength(1);

            Height = (int)Math.Round(Height * Scale);
            Width = (int)Math.Round(Width * Scale);

            double[,] Buf = new double[Height, Width];

            for (int i = 1; i < Height + 1; i++)
            {
                double Y = (1 / Scale * i) + (0.5 * (1 - 1 / Scale));
                for (int j = 1; j < Width + 1; j++)
                {
                    double X = (1 / Scale * j) + (0.5 * (1 - 1 / Scale));
                    if (X < 1) X = 1;
                    else if (X > Math.Sqrt(Matrix.Length) - 0.001) X = Math.Sqrt(Matrix.Length) - 0.001;
                    if (Y < 1) Y = 1;
                    else if (Y > Math.Sqrt(Matrix.Length) - 0.001) Y = Math.Sqrt(Matrix.Length) - 0.001;
                    int X_1 = (int)Math.Floor(X);
                    int Y_1 = (int)Math.Floor(Y);
                    int X_2 = X_1 + 1;
                    int Y_2 = Y_1 + 1;

                    // Calculate each location of 4 Neighbor Pixels
                    double NP_1 = Matrix[Y_1 - 1, X_1 - 1];
                    double NP_2 = Matrix[Y_1 - 1, X_2 - 1];
                    double NP_3 = Matrix[Y_2 - 1, X_1 - 1];
                    double NP_4 = Matrix[Y_2 - 1, X_2 - 1];

                    // Calculate each weight of 4 Neighbor Pixels
                    double PW_1 = (Y_2 - Y) * (X_2 - X);
                    double PW_2 = (Y_2 - Y) * (X - X_1);
                    double PW_3 = (X_2 - X) * (Y - Y_1);
                    double PW_4 = (Y - Y_1) * (X - X_1);

                    Buf[i - 1, j - 1] = PW_1 * NP_1 + PW_2 * NP_2 + PW_3 * NP_3 + PW_4 * NP_4;
                }
            }

            return Buf;
        }

        // Transpose Matrix
        private static T[,] Transpose<T>(T[,] Input)
        {
            T[,] Out = new T[Input.GetLength(0), Input.GetLength(1)];

            for (int i = 0; i < Input.GetLength(0); i++)
            {
                for (int j = 0; j < Input.GetLength(1); j++)
                {
                    Out[j, i] = Input[i, j];
                }
            }

            return Out;
        }

        // Rotate Matrix for 90 degree
        private static T[,] Rot90<T>(T[,] Mat)
        {
            int Mat_H = Mat.GetLength(0);
            int Mat_W = Mat.GetLength(1);
            T[,] Out = new T[Mat_H, Mat_W];

            for (int X = 0; X < Mat_H; X++)
            {
                int X_2 = Mat_H - 1 - X;
                for (int Y = 0; Y < Mat_W; Y++)
                {
                    int Y_2 = Mat_W - 1 - Y;
                    Out[X, Y] = Mat[X_2, Y_2];
                }
            }

            return Out;
        }

        // Flip Maxtrix from Left to Right.
        private static T[,] FlipLR<T>(T[,] Input)
        {
            T[,] Out = new T[Input.GetLength(0), Input.GetLength(1)];

            for (int i = 0; i < Input.GetLength(0); i++)
            {
                for (int j = 0; j < Input.GetLength(1); j++)
                {
                    Out[i, Input.GetLength(1) - j - 1] = Input[i, j];
                }
            }

            return Out;
        }
    }
}
