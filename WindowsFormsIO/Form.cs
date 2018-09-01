using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Intel8080;
using SpaceInvaders;

namespace WindowsFormsApp
{
    public partial class Form : System.Windows.Forms.Form
    {
        private bool closing;
        private readonly Machine machine;

        private Graphics graphics;
        private readonly BMPVideoDriver videoDriver;
        private Rectangle drawingRectangle;

        private readonly MCIWavSoundDriver soundDriver;

        private Speed targetSpeed = new Speed(1);

        private class Speed
        {
            public double Value { get; }
            public int MillisecondsPerFrame { get; }

            public Speed(double d)
            {
                MillisecondsPerFrame = (int) (1000.0 / (Machine.framesPerSecond * d));
                Value = d;
            }
        }

        private ColorTable ColorTable { get; set; }

        public Form(byte[] buffer, string soundDirectoryPath)
        {
            ColorTable = ColorTable.GreenLowerThird;

            InitializeComponent();

            videoDriver = new BMPVideoDriver();
            soundDriver = new MCIWavSoundDriver(soundDirectoryPath);

            machine = new Machine(buffer, PlaySound);
            var cpu = machine.Cpu;

            SetupMenus();

            FormClosing += (sender, args) => { closing = true; };

            KeyDown += (sender, e) =>
            {
                if (KeyboardMapping.TryGetPortAndBit(e.KeyCode, out var portBit))
                {
                    machine.Inputs.SetPortBit(portBit.port, portBit.bit);
                }
            };
            KeyUp += (sender, e) =>
            {
                if (KeyboardMapping.TryGetPortAndBit(e.KeyCode, out var portBit))
                {
                    machine.Inputs.UnsetPortBit(portBit.port, portBit.bit);
                }
            };

            Rectangle ComputeDrawingRect()
            {
                var p = new Point(0, menuStrip1.Bottom);

                return new Rectangle(p, new Size(ClientRectangle.Width, ClientRectangle.Height - p.Y));
            }

            graphics = CreateGraphics();
            drawingRectangle = ComputeDrawingRect();
            Resize += (sender, e) =>
            {
                graphics = CreateGraphics();
                drawingRectangle = ComputeDrawingRect();
            };

            Width -= ClientSize.Width - Machine.VideoLogicalHeight;
            Height -= ClientSize.Height - Machine.VideoLogicalWidth - menuStrip1.Bottom;

            var mainLoopThread = new Thread(() => MainLoop(cpu)) {Priority = ThreadPriority.Highest};

            mainLoopThread.Start();
        }

        private void MainLoop(CPU cpu)
        {
            void ExecuteInstructions(int cycleCount)
            {
                for (var i = 0; i < cycleCount; i += cpu.CycleCountByExecutingNextInstruction()) { }
            }

            var sw = new Stopwatch();

            sw.Start();
            while (!closing)
            {
                var startTime = sw.ElapsedMilliseconds;

                ExecuteInstructions(Machine.cyclesPerHalfFrame);

                cpu.DidInterrupt(OpCode.RST_1);

                ExecuteInstructions(Machine.cyclesPerHalfFrame);

                Redraw();

                cpu.DidInterrupt(OpCode.RST_2);

                var frameTime = (int) (sw.ElapsedMilliseconds - startTime);
                var downtime = targetSpeed.MillisecondsPerFrame - frameTime;
                if (downtime > 0)
                {
                    Thread.Sleep(downtime);
                }
            }
        }

        private void PlaySound(int index, bool start, bool loop)
        {
            if (InvokeRequired)
            {
                TryInvoke((MethodInvoker) delegate { PlaySound(index, start, loop); });
                return;
            }
            if (start)
            {
                soundDriver.Play(index, loop);
            }
            else
            {
                soundDriver.Stop(index, loop);
            }
        }

        private void Redraw()
        {
            if (InvokeRequired)
            {
                TryInvoke((MethodInvoker) Redraw);
                return;
            }

            videoDriver.Render(machine.Cpu.mem, ColorTable);

            graphics.DrawImage(videoDriver.bmp, drawingRectangle);
        }
        

        private void TryInvoke(Delegate method, params object[] args)
        {
            if (closing) { return; }
            try
            {
                Invoke(method, args);
            }
            catch (ObjectDisposedException) { }
            catch (InvalidAsynchronousStateException) { }
        }
    }
}