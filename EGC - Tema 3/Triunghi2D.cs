using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenTK_immediate_mode
{
    public class Triunghi2D
    {
        private readonly int speedOfColor = 5;

        Vector3[] vect;
        Color [] colors = {Color.Blue, Color.Aquamarine, Color.Yellow};
        private int[] alpha = {255, 255, 255};

        public Triunghi2D()
        {
            vect = new Vector3[3];
        }

        public void SetPoints(Vector3[] vect)
        {
            for (int i = 0; i < 3; i++)
            {
                this.vect[i] = vect[i];
            }
        }

        public void DrawAt(Vector3 startPos)
        {
            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(colors[0]);
            GL.Vertex3(vect[0] + startPos);

            GL.Color3(colors[1]);
            GL.Vertex3(vect[1] + startPos);

            GL.Color3(colors[2]);
            GL.Vertex3(vect[2] + startPos);

            GL.End();
        }

        public void SetColorForEachPoint(int index, Color color)
        {
            colors[index] = color;
        }

        public void IncrementColorForEachPoint(int index, int witchColor)
        {
            switch (witchColor)
            {
                case 0:
                    colors[index] = Color.FromArgb(alpha[index], (colors[index].R + speedOfColor) % 255, colors[index].G, colors[index].B);
                    break;
                case 1:
                    colors[index] = Color.FromArgb(alpha[index], colors[index].R, (colors[index].G + speedOfColor) % 255, colors[index].B);
                    break;
                case 2:
                    colors[index] = Color.FromArgb(alpha[index], colors[index].R, colors[index].G, (colors[index].B + speedOfColor) % 255);
                    break;
                case 3:
                    alpha[index] -= speedOfColor;
                    if (alpha[index] < 0)
                    {
                        alpha[index] += 255;
                    }
                    colors[index] = Color.FromArgb(alpha[index], colors[index].R, colors[index].G, (colors[index].B + speedOfColor) % 255);

                    break;
            }


        }

    }
}