using Intel8080.OperandStrategies;

namespace Intel8080
{
    public partial class CPU
    {
        // INR Increment Register or Memory
        // If register C contains 99H, the instruction: INR C will cause register C to contain 9AH
        private int INR(IByteOperandStrategy op)
        {
            var in8 = op.Read(this);
            var out8 = (byte) (in8 + 1);

            Flags = Flags
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_AC(in8, 1))
                .calc_SZP(out8);

            op.Write(this, out8);
            return op == OpMem ? 10 : 5;
        }

        // DCR Decrement Register or Memory
        // The specified register or memory byte is decremented by one.
        // If the H register contains 3AH, the L register contains 7CH, and memory location 3A7CH contains 40H, 
        // the instruction: OCR M
        // will cause memory location 3A7CH to contain 3FH.
        private int DCR(IByteOperandStrategy op)
        {
            var in8 = op.Read(this);
            var out8 = (byte) (in8 - 1);

            Flags = Flags
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_subAC(in8, 1))
                .calc_SZP(out8);

            op.Write(this, out8);
            return op == OpMem ? 10 : 5;
        }

        // RLC Rotate Accumulator Left
        private int RLC()
        {
            var acc0 = Accumulator;
            var msb = acc0 & 0x80;
            var acc1 = (acc0 << 1) | (msb >> 7);
            Accumulator = (byte) acc1;
            Flags = Flags.Set(FlagIndex.Carry, 0x80 == msb);
            return 4;
        }

        //RRC Rotate Accumulator Right
        private int RRC()
        {
            var acc0 = Accumulator;
            var lsb = acc0 & 0x01;
            var acc1 = (lsb << 7) | (acc0 >> 1);
            Accumulator = (byte) acc1;
            Flags = Flags.Set(FlagIndex.Carry, 0x01 == lsb);
            return 4;
        }

        // RAL
        // Rotate Accumulator Left Through Carry
        // The contents of the accumulator are ro- tated one bit position to the left.
        // The high-order bit of the accumulator replaces the Carry bit, while the Carry bit replaces the high-order (typo?) bit of the accumulator.
        private int RAL()
        {
            var acc = Accumulator;

            var carryIsOn = Flags.IsSet(FlagIndex.Carry);
            bool highOrderBitIsOn = (acc & 0x80) == 0x80;

            var val = (acc << 1) | (carryIsOn ? 1 : 0);

            Accumulator = (byte) val;
            Flags = Flags.Set(FlagIndex.Carry, highOrderBitIsOn);
            return 4;
        }

        // RAR Rotate Acculmulator Righth Through Carry
        // The contents of the accumulator are ro- tated one bit position to the right.
        // The low-order bit of the accumulator replaces the carry bit, while the carry bit replaces the high-order bit of the accumulator
        private int RAR()
        {
            var acc = Accumulator;

            var carryIsOn = Flags.IsSet(FlagIndex.Carry);
            bool lowOrderBitIsOn = (acc & 0x01) == 0x01;

            var val = ((carryIsOn ? 1 : 0) << 7) | (acc >> 1);

            Accumulator = (byte) val;
            Flags = Flags.Set(FlagIndex.Carry, lowOrderBitIsOn);
            return 4;
        }

        // DAA Decimal Adjust Accumulator
        // The eight-bit hexadecimal number in the accumulator is adjusted to form two four-bit binary-coded-decimal digits
        private int DAA()
        {
            var acc16 = (int) Accumulator;
            if ((acc16 & 0x0F) > 0x09 || Flags.IsSet(FlagIndex.AuxCarry))
            {
                Flags = Flags
                    .Set(FlagIndex.AuxCarry,  ((((acc16 & 0x0F) + 0x06) & 0xF0) != 0));
                
                acc16 += 0x06;
                if ((acc16 & 0xFF00) != 0)
                {
                    Flags = Flags.Set(FlagIndex.Carry, true);
                }
            }
            if ((acc16 & 0xF0) > 0x90 || Flags.IsSet(FlagIndex.Carry))
            {
                acc16 += 0x60;
                if ((acc16 & 0xFF00) != 0)
                {
                    Flags = Flags.Set(FlagIndex.Carry, true);
                }
            }
            Flags = Flags.calc_SZP((byte) acc16);

            Accumulator = (byte) acc16;
            return 4;
        }
        

        // CMA Complement Accumulator
        // Each bit of the contents of the accumula- tor is complemented (producing the one's complement).
        private int CMA()
        {
            var acc = Accumulator;
            Accumulator = (byte) ~acc;
            return 4;
        }

        // STC Set Carry
        // The Carry bit is set to one.
        private int STC()
        {
            Flags = Flags.Set(FlagIndex.Carry, true);
            return 4;
        }

        // CMC Complement Carry
        // If the Carry bit = 0, it is set to 1. If the Carry bit = 1, it is reset to O.
        private int CMC()
        {
            Flags = Flags.Toggle(FlagIndex.Carry);
            return 4;
        }

        // ADD Register or Memory To Accumulator
        private int ADD(IByteOperandStrategy operand)
        {
            var in8 = operand.Read(this);
            var out16 = Accumulator + in8;
            
            Flags = Flags
                .Set(FlagIndex.Carry, ((out16 & 0xFF00) != 0)).Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_AC(Accumulator, in8))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return operand == OpMem ? 7 : 4;
        }

        // ADC Register or Memory To Accumulator With Carry
        // The specified byte plus the content of the Carry bit is added to the contents of the accumulator.
        private int ADC(IByteOperandStrategy operand)
        {
            var in8 = operand.Read(this);
            var out16 = Accumulator + in8 + (Flags.IsSet(FlagIndex.Carry) ? 1 : 0);

            var acPred = Flags.IsSet(FlagIndex.Carry) ? AuxCarryPredicates.calc_AC_carry : AuxCarryPredicates.calc_AC;

            Flags = Flags
                .Set(FlagIndex.AuxCarry, acPred(Accumulator, in8))
                .Set(FlagIndex.Carry, ((out16 & 0xFF00) != 0))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return operand == OpMem ? 7 : 4;
        }

        // SUB Subtract Register or Memory From Accumulator
        private int SUB(IByteOperandStrategy operand)
        {
            var in8 = operand.Read(this);
            var out16 = Accumulator - in8;
            
            Flags = Flags
                .Set(FlagIndex.Carry, ((out16 & 0x00FF) >= Accumulator && in8 != 0))
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_subAC(Accumulator, in8))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return operand == OpMem ? 7 : 4;
        }

        //SBB Subtract Register or Memory From Accumulator With Borrow
        private int SBB(IByteOperandStrategy operand)
        {
            var in8 = operand.Read(this);
            var out16 = Accumulator - in8 - (Flags.IsSet(FlagIndex.Carry) ? 1 : 0);

            var acPred = Flags.IsSet(FlagIndex.Carry) ? AuxCarryPredicates.calc_subAC_borrow : AuxCarryPredicates.calc_subAC;
            
            Flags = Flags
                .Set(FlagIndex.AuxCarry, acPred(Accumulator, in8))
                .Set(FlagIndex.Carry, ((out16 & 0x00FF) >= Accumulator && in8 != 0 | Flags.IsSet(FlagIndex.Carry)))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return operand == OpMem ? 7 : 4;
        }

        // ANA Logical and Register or Memory With Accumulator
        private int ANA(IByteOperandStrategy operand)
        {
            var inAcc8 = Accumulator;
            var in8 = operand.Read(this);
            var out8 = (byte)(inAcc8 & in8);

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, (((inAcc8 | in8) >> 3) & 1) == 1);

            return 4;
        }

        //XRA Logical Exclusive-Or Register or Memory With Accumulator (Zero Accumulator)
        private int XRA(IByteOperandStrategy operand)
        {
            var out8 = (byte) (Accumulator ^ operand.Read(this));

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, false);

            return 4;
        }

        // OR
        private int ORA(IByteOperandStrategy operand)
        {
            var out8 = (byte) (Accumulator | operand.Read(this));

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, false);

            return 4;
        }

        // Compare
        private int CMP(IByteOperandStrategy operand)
        {
            var cmp16 = Accumulator - operand.Read(this);
            
            Flags = Flags
                .Set(FlagIndex.Carry, ((cmp16 & 0x00FF) >= Accumulator && operand.Read(this) != 0))
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_subAC(Accumulator, operand.Read(this)))
                .calc_SZP((byte) cmp16);

            return operand == OpMem ? 7 : 4;
        }

        // ADI Add Immediate To Accumulator
        // The byte of immediate data is added to the contents of the accumulator using two's complement arithmetic.
        private int ADI(byte b)
        {
            var out16 = Accumulator + b;
            
            Flags = Flags
                .Set(FlagIndex.Carry, ((out16 & 0xFF00) != 0)).Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_AC(Accumulator, b))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return 7;
        }

        //ACI Add Immediate To Accumulator With Carry
        // The byte of immediate data is added to the contents of the accumulator plus the contents of the carry bit.
        private int ACI(byte b)
        {
            var out16 = Accumulator + b + (Flags.IsSet(FlagIndex.Carry) ? 1 : 0);

            var acPred = Flags.IsSet(FlagIndex.Carry) ? AuxCarryPredicates.calc_AC_carry : AuxCarryPredicates.calc_AC;
            
            Flags = Flags
                .Set(FlagIndex.AuxCarry, acPred(Accumulator, b))
                .Set(FlagIndex.Carry, ((out16 & 0xFF00) != 0))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            return 7;
        }

        //SUI Subtract Immediate From Accumulator
        // The byte of immediate data is subtracted from the contents of the accumulator using two's complement arithmetic.
        private int SUI(byte b)
        {
            var out16 = (ushort) (Accumulator - b);

            Flags = Flags
                .Set(FlagIndex.Carry, ((out16 & 0x00FF) >= Accumulator && b != 0))
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_subAC(Accumulator, b))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            
            return 7;
        }

        // SBI Subtract Immediate from Accumulator With Borrow
        // Description: The Carry bit is publicly added to the byte of immediate data. This value is then subtracted from the accumulator using two's complement arithmetic.
        private int SBI(byte b)
        {
            var out16 = Accumulator - b - (Flags.IsSet(FlagIndex.Carry) ? 1 : 0);

            var acPred = Flags.IsSet(FlagIndex.Carry) ? AuxCarryPredicates.calc_subAC_borrow : AuxCarryPredicates.calc_subAC;
            
            Flags = Flags
                .Set(FlagIndex.AuxCarry, acPred(Accumulator, b))
                .Set(FlagIndex.Carry, ((out16 & 0x00FF) >= Accumulator && b != 0 | Flags.IsSet(FlagIndex.Carry)))
                .calc_SZP((byte) out16);

            Accumulator = (byte) out16;
            
            return 7;
        }

        // ANI And Immediate With Accumulator
        // The byte of immediate data is logically ANDed with the contents of the accumulator. The Carry bit is reset to zero.
        private int ANI(byte b)
        {
            var inAcc8 = Accumulator;
            var out8 = (byte) (inAcc8 & b);

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, (((inAcc8 | b) >> 3) & 1) == 1);
            
            return 7;
        }

        //XRI Exclusive-Or Immediate With Accumulator
        //The byte of immediate data is EXCLU- SIV E-ORed with the contents of the accumulator. The carry bit is set to zero.
        private int XRI(byte b)
        {
            var out8 = (byte) (Accumulator ^ b);

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, false);

            return 7;
        }

        // ONI Or Immediate With Accumulator
        // The byte of immediate data is logically ORed with the contents of the accumulator.
        // The result is stored in the accumulator. The Carry bit is reset to zero, while the Zero, Sign, and Parity bits are set according to the result.
        private int ORI(byte b)
        {
            var out8 = (byte) (Accumulator | b);

            Accumulator = out8;

            Flags = Flags
                .calc_SZP(out8)
                .Set(FlagIndex.Carry, false)
                .Set(FlagIndex.AuxCarry, false);

            return 7;
        }

        // CPI Compare Immediate With Accumulator
        // The byte of immediate data is compared to the contents of the accumulator.
        private int CPI(byte b)
        {
            var cmp16 = Accumulator - b;

            Flags = Flags
                .Set(FlagIndex.Carry, ((cmp16 & 0x00FF) >= Accumulator && b != 0))
                .Set(FlagIndex.AuxCarry, AuxCarryPredicates.calc_subAC(Accumulator, b))
                .calc_SZP((byte) cmp16);

            return 7;
        }
    }
}