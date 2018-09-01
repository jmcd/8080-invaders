namespace Intel8080
{
    public partial class CPU
    {
        // RNZ Return If Not Zero
        // If the Zero bit is zero, a return operation is performed.
        private int RNZ()
        {
            var zeroBitIsOne = Flags.IsSet(FlagIndex.Zero);
            var zeroBitIsZero = !zeroBitIsOne;
            if (zeroBitIsZero)
            {
                Return();
                return 11;
            }
            return 5;
        }

        //JNZ Jump If Not Zero
        // If the Zero bit is zero, program execu- tion continues at the memory address adr.
        private int JNZ(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Zero, false, hi, lo);
            return 10;
        }

        private int JMP(byte lo, byte hi)
        {
            Jump(lo, hi);
            return 10;
        }

        //  If the Zero bit is one, a call operation is performed to subroutine sub.
        private int CNZ(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Zero, false, hi, lo) ? 17 : 11;
        }

        // The contents of the program counter are pushed onto the stack, providing a return address for later use by a RETURN instruction.
        // Program execution continues at memory address: OOOOOOOOOOEXPOOOB
        private int RST(byte threeBitExp)
        {
            PushPc();
            /*registers[PC.LHS] = 0;
            registers[PC.RHS] = (threeBitExp << 3).ToByte();*/
            programCounter = (threeBitExp << 3);
            return 11;
        }

        // RZ Return If Zero
        // If the Zero bit is one, a return operation is performed. ?????
        private int RZ()
        {
            var zeroBitIsOne = Flags.IsSet(FlagIndex.Zero);
            if (zeroBitIsOne)
            {
                Return();
                return 11;
            }
            return 5;
        }

        private int RET()
        {
            Return();
            return 10;
        }

        //JZ Jump If Zero
        // If the zero bit is one, program execution continues at the memory address adr.
        private int JZ(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Zero, true, hi, lo);
            //TestExpectation(3, OpCode.JZ_adr);
            return 10;
        }

        // CZ Call If Zero
        // Description: If the Zero bit is zero, a call operation is performed to subroutine sub.
        private int CZ(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Zero, true, hi, lo) ? 17 : 11;
        }

        public virtual int CALL(byte lo, byte hi)
        {
            Call(lo, hi);
            return 17;
        }

        // RNC Return If No Carry
        // If the carry bit is zero, a return operation is performed.
        private int RNC()
        {
            return ReturnIfFlag(FlagIndex.Carry, false) ? 11 : 5;
        }

        // JNC Jump If No Carry
        // If the Carry bit is zero, program execu- tion continues at the memory address adr.
        private int JNC(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Carry, false, hi, lo);
            return 10;
        }

        // CNC Call If No Carry
        // If the Carry bit is zero, a call operation is performed to subroutine sub.
        private int CNC(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Carry, false, hi, lo) ? 17 : 11;
        }

        // Return If Carry
        // If the Carry bit is one, a return operation is performed.
        private int RC()
        {
            return ReturnIfFlag(FlagIndex.Carry, true) ? 11 : 5;
        }

        // JC Jump If Carry
        // If the Carry bit is one, program execu- tion continues at the memory address adr.
        private int JC(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Carry, true, hi, lo);
            return 10;
        }

        // CC Call If Carry
        // Description: If the Carry bit is one, a call operation is performed to subroutine sub.
        private int CC(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Carry, true, hi, lo) ? 17 : 11;
        }

        // RPO Return If Parity Odd
        // If the Parity bit is zero (indicating odd parity), a return operation is performed.
        private int RPO()
        {
            return ReturnIfFlag(FlagIndex.Parity, false) ? 11 : 5;
        }

        // JPO Jump If Parity Odd
        // If the Parity bit is zero (indicating a re- sult with odd parity), program execution conti nues at the memory address adr.
        private int JPO(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Parity, false, hi, lo);
            return 10;
        }

        // CPO Call If Parity Odd
        // If the Parity bit is zero (indicating odd parity), a call operation is performed to // subroutine sub.
        private int CPO(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Parity, false, hi, lo) ? 17 : 11;
        }

        // RPE Return If Parity Even
        // If the Parity bit is one (indicating even parity), a return operation is performed.
        private int RPE()
        {
            return ReturnIfFlag(FlagIndex.Parity, true) ? 11 : 5;
        }

        // PCHL Load Program Counter
        // The contents of the H register replace the most significant 8 bits of the program counter,
        // and the contents of the L register replace the least significant 8 bits of the program counter.
        // This causes program execution to continue at the address contained in the Hand L registers.
        private int PCHL()
        {
            programCounter = (registers[Registers.Index.H] << 8) | registers[Registers.Index.L];
            /*registers[PC.LHS] = registers[H];
            registers[PC.RHS] = registers[L];*/
            return 5;
        }

        // JPE Jump If Parity Even
        // If the parity bit is one (indicating a result with even parity), program execution continues at the mem- ory address adr.
        private int JPE(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Parity, true, hi, lo);
            return 10;
        }

        // CPE Call If Parity Even
        // If the Parity bit is one (indicating even parity), a call operation is performed to // subroutine sub.
        private int CPE(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Parity, true, hi, lo) ? 17 : 11;
        }

        //RP Return If Plus
        // If the Sign bit is zero (indicating a posi- tive result). a return operation is performed.
        private int RP()
        {
            return ReturnIfFlag(FlagIndex.Sign, false) ? 11 : 5;
        }

        // JP Jump If Positive
        // If the sign bit is zero, (indicating a posi- tive result), program execution continues at the memory address adr.
        private int JP(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Sign, false, hi, lo);
            return 10;
        }

        // CP Call If Plus
        // Description: If the Sign bit is zero (indicating a posi- tive result), a call operation // is performed to subroutine sub.
        private int CP(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Sign, false, hi, lo) ? 17 : 11;
        }

        // RM Return If Minus
        // If the Sign bit is one (indicating a minus result), a return operation is performed.
        private int RM()
        {
            return ReturnIfFlag(FlagIndex.Sign, true) ? 11 : 5;
        }

        // JM Jump If Minus
        // If the Sign bit is one (indicating a nega- tive result), program execution continues at the memory address adr.
        private int JM(byte lo, byte hi)
        {
            JumpIfFlag(FlagIndex.Sign, true, hi, lo);
            return 10;
        }

        // CM Call If Minus
        // If the Sign bit is one (indicating a minus result), a call operation is performed to subrouti ne sub.
        private int CM(byte lo, byte hi)
        {
            return DidCallBasedOnFlag(FlagIndex.Sign, true, hi, lo) ? 17 : 11;
        }
    }
}