using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace OpenTK_immediate_mode
{
    class ImmediateMode : GameWindow
    {
        // Some configs -----------
        private double XYZ_SIZE;
        private float zNear = 1;
        private float zFar = 200;

        // Some vars
        private Vector3 eyePos = new Vector3(50, 30, 50);
        private Vector3 lookAtPos = new Vector3(0, 0, 0);
        private Triunghi2D triunghi2D = new Triunghi2D();

        // for mouse
        private int mouseState;
        private double radius = 1;
        private Vector2 mousePos;


        public ImmediateMode() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            //Console.WriteLine("OpenGl versiunea: " + GL.GetString(StringName.Version));
            Title = "OpenGl versiunea: " + GL.GetString(StringName.Version) + " (mod imediat)";
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.DarkGray);


            /*
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Fog(FogParameter.FogColor, new float[] {169, 169, 169});
            GL.Fog(FogParameter.FogDensity, 0.005f);
            GL.Hint(HintTarget.FogHint, HintMode.DontCare);
            GL.Fog(FogParameter.FogStart, 65);
            GL.Fog(FogParameter.FogEnd, 80); 
            GL.Enable(EnableCap.Fog);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            */

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);


            Vector3[] v = new Vector3[3];
            using (StreamReader streamReader = new StreamReader("triunghi.txt"))
            {
                for (int contor = 0; contor < 3; contor++)
                {
                    var coords = streamReader.ReadLine().Trim().Split(' ').ToList();
                    var x = coords.Select(int.Parse).ToList();

                    v[contor].X = x[0];
                    v[contor].Y = x[1];
                    v[contor].Z = x[2];
                }
            }

            triunghi2D.SetPoints(v);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, zNear, zFar);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            UpdateEyeAndLookAt();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
            }

            if (keyboard[Key.R] && keyboard[Key.Number1])
            {
                triunghi2D.IncrementColorForEachPoint(0, 0);
            }
            if (keyboard[Key.G] && keyboard[Key.Number1])
            {
                triunghi2D.IncrementColorForEachPoint(0, 1);

            }
            if (keyboard[Key.B] && keyboard[Key.Number1])
            {
                triunghi2D.IncrementColorForEachPoint(0, 2);

            }

            if (keyboard[Key.R] && keyboard[Key.Number2])
            {
                triunghi2D.IncrementColorForEachPoint(1, 0);

            }
            if (keyboard[Key.G] && keyboard[Key.Number2])
            {
                triunghi2D.IncrementColorForEachPoint(1, 1);

            }
            if (keyboard[Key.B] && keyboard[Key.Number2])
            {
                triunghi2D.IncrementColorForEachPoint(1, 2);

            }

            if (keyboard[Key.R] && keyboard[Key.Number3])
            {
                triunghi2D.IncrementColorForEachPoint(2, 0);

            }
            if (keyboard[Key.G] && keyboard[Key.Number3])
            {
                triunghi2D.IncrementColorForEachPoint(2, 1);

            }
            if (keyboard[Key.B] && keyboard[Key.Number3])
            {
                triunghi2D.IncrementColorForEachPoint(2, 2);
            }

            if (keyboard[Key.A] && keyboard[Key.Number1])
            {
                triunghi2D.IncrementColorForEachPoint(0, 3);
            }

            if (keyboard[Key.A] && keyboard[Key.Number2])
            {
                triunghi2D.IncrementColorForEachPoint(1, 3);

            }

            if (keyboard[Key.A] && keyboard[Key.Number3])
            {
                triunghi2D.IncrementColorForEachPoint(2, 3);

            }

            if (mouse.LeftButton == ButtonState.Released)
            {
                mouseState = 0;
            }
            else if (mouse.LeftButton == ButtonState.Pressed && mouseState == 0)
            {
                mouseState++;
                mousePos = new Vector2(mouse.X, mouse.Y);
            }
            else if (mouse.LeftButton == ButtonState.Pressed)
            {
                var delta = mousePos.Y - mouse.Y;

                if (delta != 0)
                {
                    if (delta < 0)
                    {
                        delta = 1 - 1 / Math.Max((100 + delta), 10);
                    }
                    else
                    {
                        delta = 1 + delta / (delta + 1000);
                    }

                    Console.WriteLine(delta);

                    eyePos.X *= delta;
                    eyePos.Y *= delta;
                    eyePos.Z *= delta;
                    radius *= Math.Sqrt(Math.Pow(eyePos.X, 2) + Math.Pow(eyePos.Y, 2));

                    Matrix4 lookat = Matrix4.LookAt(eyePos.X, eyePos.Y, eyePos.Z, lookAtPos.X, lookAtPos.Y, lookAtPos.Z, 0, 1, 0);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref lookat);

                    mousePos = new Vector2(mouse.X, mouse.Y);

                    Console.WriteLine("Radius: " + radius);

                }

                var deltaPeX = mousePos.X - mouse.X;

                if (deltaPeX != 0)
                {

                }


            }


        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // Codul se scrie mai jos



            DrawAxes();

            DrawObjects();



            // Pana aici
            // Se lucrează în modul DOUBLE BUFFERED - câtă vreme se afișează o imagine randată, o alta se randează în background apoi cele 2 sunt schimbate...
            SwapBuffers();
        }

        private void DrawAxes()
        {
            GL.LineWidth(2.0f);
            GL.PointSize(20.0f);
            GL.Color3(Color.Black);

            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(0,0,0);
            GL.End();

            GL.PointSize(2.0f);


            GL.Begin(PrimitiveType.Lines);

            for (int x = -100; x <= 100; x += 10)
            {
                GL.Vertex3(x, 0, -100);
                GL.Vertex3(x, 0, 100);
            }

            for (int z = -100; z <= 100; z += 10)
            {
                GL.Vertex3(-100, 0, z);
                GL.Vertex3(100, 0, z);
            }

            GL.End();
        }

        private void DrawObjects()
        {

            triunghi2D.DrawAt(new Vector3(0, 5, 0));



        }

        private void UpdateEyeAndLookAt()
        {
            Matrix4 lookat = Matrix4.LookAt(eyePos.X, eyePos.Z, eyePos.Z, lookAtPos.X, lookAtPos.Y, lookAtPos.Z, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }


        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Pentru a schimba culoarea unui colt al triunghiului actionati: ");
            Console.WriteLine("Num1 sau Num2 sau Num3 (deasupra la QWERTY) pentru a selecta coltul");
            Console.WriteLine("Si butonul R G B sau A pentru incrementarea componentei Red, Green, Blue sau Alpha!");


            using (ImmediateMode example = new ImmediateMode())
            {
                example.Run(60.0, 60.0);
            }

        }
    }

}