using System;
using System.Linq;
using Intel8080;

namespace SpaceInvaders
{
    public class Machine
    {
        public const int VideoLogicalWidth = 256;
        public const int VideoLogicalHeight = 224;

        public const int VideoRAMLocation = 0x2400;
        public const int VideoRAMLength = 0x1C00;

        private const double cyclesPerSecond = 2000.0 * 1000;
        public const double framesPerSecond = 60;
        private const double cyclesPerFrame = cyclesPerSecond / framesPerSecond;
        public const int cyclesPerHalfFrame = (int) (cyclesPerFrame / 2);

        public CPU Cpu { get; }

        public readonly Inputs Inputs;

        public Machine(byte[] buffer, Action<int, bool, bool> playSound)
        {
            var mem = new RAM(buffer);

            var ports = Enumerable.Range(0, 8).Select(_ => new Port()).ToList();

            Cpu = new CPU(mem, ports);

            var shiftRegister = new ShiftRegister();

            Inputs = new Inputs();

            var soundStatus = new bool[9];

            ports[0].OnIn = () => Inputs[0];
            ports[1].OnIn = () => Inputs[1];
            ports[2].OnIn = () => Inputs[2];
            ports[3].OnIn = () => shiftRegister.Result();

            ports[2].OnOut = b => shiftRegister.SetOffset(b);
            ports[3].OnOut = b =>
            {
                for (var i = 0; i < 4; i++)
                {
                    var play = ((b >> i) & 1) == 1;

                    if (play != soundStatus[i])
                    {
                        playSound(i, play, i == 0);
                        soundStatus[i] = play;
                    }
                }
            };
            ports[4].OnOut = b => shiftRegister.Write(b);
            ports[5].OnOut = b =>
            {
                for (var i = 0; i < 5; i++)
                {
                    var play = ((b >> i) & 1) == 1;

                    if (play != soundStatus[i])
                    {
                        var soundIndex = i + 4;
                        playSound(soundIndex, play, false);
                        soundStatus[soundIndex] = play;
                    }
                }
            };
            ports[6].OnOut = b => { };
        }
    }
}