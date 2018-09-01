using Intel8080.OperandStrategies;

namespace Intel8080
{
    public partial class CPU
    {
        // "If register B contains 3F and register C contains 16, 
        // the instruction: STAX B will store the contents of the accumulator at memory location 3F16H."
        private int STAX(RegisterIndexPair rp)
        {
            mem[registers[rp]] = Accumulator;
            return 7;
        }

        // MVI Move immediate data
        // The byte of immediate data is stored in the specified register or memory byte.
        private int MVI(IByteOperandStrategy r, byte val)
        {
            r.Write(this, val);
            return 7;
        }

        // LDAX Load Accumulator
        //   If register D contains 93H and register E contains 8BH, the instruction: LDAX D will load the accumulator from memory location 938BH.
        private int LDAX(RegisterIndexPair rp)
        {
            Accumulator = mem[registers[rp]];
            return 7;
        }

        // STA Stor Accumulator Direct
        // The contents of the accumulator replace the byte at the memory address formed by concatenating HI ADD with LOW ADD.
        private int STA(byte lo, byte hi)
        {
            var acc = Accumulator;
            var loc = (hi << 8) | lo;
            mem[loc] = acc;
            return 7;
        }

        // LDA Load Accumulator Direct
        // The byte at the memory address formed by concatenating HI ADO with LOW ADO replaces the con- tents of the accumulator.
        private int LDA(byte lo, byte hi)
        {
            var loc = (hi << 8) | lo;
            Accumulator = mem[loc];
            return 13;
        }

        // MOV Move register to register
        private int MOV(IByteOperandStrategy rDest, IByteOperandStrategy rSrc)
        {
            rDest.Write(this, rSrc.Read(this));
            return 5;
        }
    }
}