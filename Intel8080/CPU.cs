using System.Collections.Generic;

namespace Intel8080
{
    public partial class CPU
    {
        public int programCounter;
        public int stackPointer;

        public readonly RAM mem;
        private readonly IList<Port> ports;

        public readonly Registers registers = new Registers();

        private byte Flags
        {
            get => registers[Registers.Index.F];
            set => registers[Registers.Index.F] = value;
        }

        private byte Accumulator
        {
            get => registers[Registers.Index.A];
            set => registers[Registers.Index.A] = value;
        }

        public CPU(RAM mem, IList<Port> ports)
        {
            this.mem = mem;
            this.ports = ports;
            Flags = 2;
        }

        private byte Read()
        {
            var result = mem[programCounter];
            programCounter += 1;
            return result;
        }

        private bool DidCallBasedOnFlag(FlagIndex flagIndex, bool shouldBeOne, byte locHi, byte locLo)
        {
            if (Flags.IsSet(flagIndex) != shouldBeOne)
            {
                return false;
            }

            Call(locLo, locHi);
            return true;
        }

        private void JumpIfFlag(FlagIndex flagIndex, bool shouldBeOne, byte locHi, byte locLo)
        {
            if (Flags.IsSet(flagIndex) == shouldBeOne)
            {
                Jump(locLo, locHi);
            }
        }

        private bool ReturnIfFlag(FlagIndex flagIndex, bool shouldBeOne)
        {
            var flagIsOne = Flags.IsSet(flagIndex);
            if (flagIsOne == shouldBeOne)
            {
                Return();
                return true;
            }

            return false;
        }

        private void Call(byte lo, byte hi)
        {
            PushPc();
            Jump(lo, hi);
        }

        private void PushPc()
        {
            var lhs = (byte) (programCounter >> 8);
            var rhs = (byte) programCounter;
            Push8(lhs);
            Push8(rhs);
        }

        private void Return()
        {
            var rhs = Pop8();
            var lhs = Pop8();
            Jump(rhs, lhs);
        }

        private void Jump(byte lo, byte hi)
        {
            programCounter = (hi << 8) | lo;
        }

        private void Push8(byte b)
        {
            stackPointer = (stackPointer - 1) & 0xffff;
            mem[stackPointer] = b;
        }

        private byte Pop8()
        {
            var result = mem[stackPointer];
            stackPointer++;
            return result;
        }

        private OpCode? ConsumeInterrupt()
        {
            if (!interrupt.HasValue)
            {
                return null;
            }

            var result = interrupt.Value;
            interrupt = null;
            interruptsAreEnabled = true;
            return result;
        }

        private void Interrupt(OpCode opCode)
        {
            interrupt = opCode;
            interruptsAreEnabled = false;
        }

        public bool DidInterrupt(OpCode opCode)
        {
            if (interruptsAreEnabled)
            {
                Interrupt(opCode);
                return true;
            }
            return false;
        }

        private OpCode? interrupt;
    }
}