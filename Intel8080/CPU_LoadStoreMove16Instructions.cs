using Intel8080.OperandStrategies;

namespace Intel8080
{
    public partial class CPU
    {
        private int LXI(IWordOperandStrategy rp, byte lo, byte hi)
        {
            rp.Write(this, (hi << 8) | lo);
            return 10;
        }

        // SHLD Store H and L Direct (16 bit)
        private int SHLD(byte lo, byte hi)
        {
            var loc = (hi << 8) | lo;
            mem[loc + 1] = registers[Registers.Index.H];
            mem[loc] = registers[Registers.Index.L];
            return 16;
        }

        // LHLD Load H And L Direct
        //  If memory locations 25BH and 25CH contain FFH and 03H respectively, the instruction: LHLD 25BH will load the L register with FFH, and will load the H regis- ter with 03H.
        private int LHLD(byte lo, byte hi)
        {
            var loc = (hi << 8) | lo;
            registers[Registers.Index.H] = mem[loc + 1];
            registers[Registers.Index.L] = mem[loc];
            return 16;
        }

        private int POP(RegisterIndexPair rp)
        {
            /*
             * Stack:
             * E rp+1
             * D rp
             */

            registers[rp.RHS] = Pop8();
            registers[rp.LHS] = Pop8();

            if (rp == RegisterIndexPair.PSW)
            {
                /*http://www.vcfed.org/forum/archive/index.php/t-63090.html
                 * Issue #1: Always Clear Bit 5 and Bit 3, and Set Bit 1 for POP PSW
instruction.
                 */
                /*Flags &= (0xff - 0x20 - 0x08);
                Flags |= 2;*/

                Flags &= 0b11010111;
                Flags |= 0b00000010;
            }

            return 10;
        }

        // The most significant 8 bits of data are stored at the memory address one less than the contents of the stack pointer.
        // The least significant 8 bits of data are stored at the memory address two less than the contents of the stack pointer.
        private int PUSH(RegisterIndexPair rp)
        {
            /*
             * Assume that register D contains 8F,
             *             register E contains 9D,
             * and the stack pointer contains 3A2C.
             *
             * 3A2C ?? <-- stackPointer
             *
             * Then the instruction: PUSH D
             *
             * stores the D register at memory address 3A2B, stores the E register at memory address 3A2A, and then
             * decrements the stack pointer by two, leaving the stack pointer equal to 3A2A.
             * 
             * 3A2A 9D (contents of E) <-- stackPointer
             * 3A2B 8F (contents of D)
             * 3A2C ?? 
             */

            Push8(registers[rp.LHS]);
            Push8(registers[rp.RHS]);
            return 11;
        }

        //SPHL Load SP From HAnd L
        // The 16 bits of data held in the Hand L registers replace the contents of the stack pointer SP. The contents of the Hand L registers are unchanged.
        private int SPHL()
        {
            stackPointer = (registers[Registers.Index.H] << 8) | registers[Registers.Index.L];
            /*registers[SP.LHS] = registers[H];
            registers[SP.RHS] = registers[L];*/
            return 5;
        }

        // XCHG Exchange Registers
        // The 16 bits of data held in the Hand L registers are exchanged with the 16 bits of data held in the D and E registers.
        private int XCHG()
        {
            var h1 = Registers.Index.H;
            var h = registers[h1];
            var l1 = Registers.Index.L;
            var l = registers[l1];

            var d = Registers.Index.D;
            registers[h1] = registers[d];
            var e = Registers.Index.E;
            registers[l1] = registers[e];

            registers[d] = h;
            registers[e] = l;
            return 5;
        }

        // XTHL Exchange Stack
        // The contents of the L register are ex- changed with the contents of the memory byte whose address is held in
        // the stack pointer SP.
        // The contents of the H register are exchanged with the contents of the memory byte whose address is one
        // greater than that held in the stack pointer.
        private int XTHL()
        {
            var l1 = Registers.Index.L;
            var l = registers[l1];
            var h1 = Registers.Index.H;
            var h = registers[h1];

            var stackPointer = this.stackPointer;

            var contSp = mem[stackPointer];
            var contSp1 = mem[stackPointer + 1];

            registers[l1] = contSp;
            registers[h1] = contSp1;

            mem[stackPointer] = l;
            mem[stackPointer + 1] = h;
            return 18;
        }
    }
}