using System;
using System.Diagnostics;
using Intel8080.OperandStrategies;

namespace Intel8080
{
    public partial class CPU
    {
        private static readonly IByteOperandStrategy OpA = new RegisterByteOperandStrategy(Registers.Index.A);
        private static readonly IByteOperandStrategy OpB = new RegisterByteOperandStrategy(Registers.Index.B);
        private static readonly IByteOperandStrategy OpC = new RegisterByteOperandStrategy(Registers.Index.C);
        private static readonly IByteOperandStrategy OpD = new RegisterByteOperandStrategy(Registers.Index.D);
        private static readonly IByteOperandStrategy OpE = new RegisterByteOperandStrategy(Registers.Index.E);
        private static readonly IByteOperandStrategy OpH = new RegisterByteOperandStrategy(Registers.Index.H);
        private static readonly IByteOperandStrategy OpL = new RegisterByteOperandStrategy(Registers.Index.L);
        private static readonly IByteOperandStrategy OpMem = new MemoryByteOperandStrategy();

        private static readonly IWordOperandStrategy OpBC = new RegisterPairWordOperandStrategy(RegisterIndexPair.BC);
        private static readonly IWordOperandStrategy OpDE = new RegisterPairWordOperandStrategy(RegisterIndexPair.DE);
        private static readonly IWordOperandStrategy OpHL = new RegisterPairWordOperandStrategy(RegisterIndexPair.HL);
        private static readonly IWordOperandStrategy OpSP = new StackPointerWordOperandStrategy();

#if DEBUG
        public bool ShouldWriteDebug = false;
        private int dbgCount;
#endif

        public int CycleCountByExecutingNextInstruction()
        {
            var opcode = ConsumeInterrupt() ?? (OpCode) Read();

#if DEBUG
            dbgCount++;
            if (dbgCount == 10000000)
            {
                dbgCount = 0;
            }
            if (ShouldWriteDebug)
            {
                var flags = Flags;
                var sp = stackPointer;
                Debug.WriteLine($"{programCounter:x4} op:{(int) opcode:x2} {opcode}");
                Debug.WriteLine($"     c:{(flags.IsSet(FlagIndex.Carry) ? "1" : "0")} p:{(flags.IsSet(FlagIndex.Parity) ? "1" : "0")} ac:{(flags.IsSet(FlagIndex.AuxCarry) ? "1" : "0")} z:{(flags.IsSet(FlagIndex.Zero) ? "1" : "0")} s:{(flags.IsSet(FlagIndex.Sign) ? "1" : "0")}");
                Debug.WriteLine($"     A:{Accumulator:x2} B:{registers[Registers.Index.B]:x2} C:{registers[Registers.Index.C]:x2} D:{registers[Registers.Index.D]:x2} E:{registers[Registers.Index.E]:x2} H:{registers[Registers.Index.H]:x2} L:{registers[Registers.Index.L]:x2}");
                Debug.WriteLine($"     SP:{sp:x4} = {mem[sp]:x2}");
            }
#endif

            switch (opcode)
            {
                case OpCode.NOP:
                    return NOP();
                case OpCode.LXI_B_D16:
                    return LXI(OpBC, Read(), Read());
                case OpCode.STAX_B:
                    return STAX(RegisterIndexPair.BC);
                case OpCode.INX_B:
                    return INX(OpBC);
                case OpCode.INR_B:
                    return INR(OpB);
                case OpCode.DCR_B:
                    return DCR(OpB);
                case OpCode.MVI_B_D8:
                    return MVI(OpB, Read());
                case OpCode.RLC:
                    return RLC();
                case OpCode.NOP1:
                    return NOP();
                case OpCode.DAD_B:
                    return DAD(OpBC);
                case OpCode.LDAX_B:
                    return LDAX(RegisterIndexPair.BC);
                case OpCode.DCX_B:
                    return DCX(OpBC);
                case OpCode.INR_C:
                    return INR(OpC);
                case OpCode.DCR_C:
                    return DCR(OpC);
                case OpCode.MVI_C_D8:
                    return MVI(OpC, Read());
                case OpCode.RRC:
                    return RRC();
                case OpCode.NOP2:
                    return NOP();
                case OpCode.LXI_D_D16:
                    return LXI(OpDE, Read(), Read());
                case OpCode.STAX_D:
                    return STAX(RegisterIndexPair.DE);
                case OpCode.INX_D:
                    return INX(OpDE);
                case OpCode.INR_D:
                    return INR(OpD);
                case OpCode.DCR_D:
                    return DCR(OpD);
                case OpCode.MVI_D_D8:
                    return MVI(OpD, Read());
                case OpCode.RAL:
                    return RAL();
                case OpCode.NOP3:
                    return NOP();
                case OpCode.DAD_D:
                    return DAD(OpDE);
                case OpCode.LDAX_D:
                    return LDAX(RegisterIndexPair.DE);
                case OpCode.DCX_D:
                    return DCX(OpDE);
                case OpCode.INR_E:
                    return INR(OpE);
                case OpCode.DCR_E:
                    return DCR(OpE);
                case OpCode.MVI_E_D8:
                    return MVI(OpE, Read());
                case OpCode.RAR:
                    return RAR();
                case OpCode.NOPB:
                    return NOP();
                case OpCode.LXI_H_D16:
                    return LXI(OpHL, Read(), Read());
                case OpCode.SHLD_adr:
                    return SHLD(Read(), Read());
                case OpCode.INX_H:
                    return INX(OpHL);
                case OpCode.INR_H:
                    return INR(OpH);
                case OpCode.DCR_H:
                    return DCR(OpH);
                case OpCode.MVI_H_D8:
                    return MVI(OpH, Read());
                case OpCode.DAA:
                    return DAA();
                case OpCode.NOP4:
                    return NOP();
                case OpCode.DAD_H:
                    return DAD(OpHL);
                case OpCode.LHLD_adr:
                    return LHLD(Read(), Read());
                case OpCode.DCX_H:
                    return DCX(OpHL);
                case OpCode.INR_L:
                    return INR(OpL);
                case OpCode.DCR_L:
                    return DCR(OpL);
                case OpCode.MVI_L_D8:
                    return MVI(OpL, Read());
                case OpCode.CMA:
                    return CMA();
                case OpCode.NOPC:
                    return NOP();
                case OpCode.LXI_SP_D16:
                    return LXI(OpSP, Read(), Read());
                case OpCode.STA_adr:
                    return STA(Read(), Read());
                case OpCode.INX_SP:
                    return INX(OpSP);
                case OpCode.INR_M:
                    return INR(OpMem);
                case OpCode.DCR_M:
                    return DCR(OpMem);
                case OpCode.MVI_M_D8:
                    return MVI(OpMem, Read());
                case OpCode.STC:
                    return STC();
                case OpCode.NOP5:
                    return NOP();
                case OpCode.DAD_SP:
                    return DAD(OpSP);
                case OpCode.LDA_adr:
                    return LDA(Read(), Read());
                case OpCode.DCX_SP:
                    return DCX(OpSP);
                case OpCode.INR_A:
                    return INR(OpA);
                case OpCode.DCR_A:
                    return DCR(OpA);
                case OpCode.MVI_A_D8:
                    return MVI(OpA, Read());
                case OpCode.CMC:
                    return CMC();
                case OpCode.MOV_B_B:
                    return MOV(OpB, OpB);
                case OpCode.MOV_B_C:
                    return MOV(OpB, OpC);
                case OpCode.MOV_B_D:
                    return MOV(OpB, OpD);
                case OpCode.MOV_B_E:
                    return MOV(OpB, OpE);
                case OpCode.MOV_B_H:
                    return MOV(OpB, OpH);
                case OpCode.MOV_B_L:
                    return MOV(OpB, OpL);
                case OpCode.MOV_B_M:
                    return MOV(OpB, OpMem);
                case OpCode.MOV_B_A:
                    return MOV(OpB, OpA);
                case OpCode.MOV_C_B:
                    return MOV(OpC, OpB);
                case OpCode.MOV_C_C:
                    return MOV(OpC, OpC);
                case OpCode.MOV_C_D:
                    return MOV(OpC, OpD);
                case OpCode.MOV_C_E:
                    return MOV(OpC, OpE);
                case OpCode.MOV_C_H:
                    return MOV(OpC, OpH);
                case OpCode.MOV_C_L:
                    return MOV(OpC, OpL);
                case OpCode.MOV_C_M:
                    return MOV(OpC, OpMem);
                case OpCode.MOV_C_A:
                    return MOV(OpC, OpA);
                case OpCode.MOV_D_B:
                    return MOV(OpD, OpB);
                case OpCode.MOV_D_C:
                    return MOV(OpD, OpC);
                case OpCode.MOV_D_D:
                    return MOV(OpD, OpD);
                case OpCode.MOV_D_E:
                    return MOV(OpD, OpE);
                case OpCode.MOV_D_H:
                    return MOV(OpD, OpH);
                case OpCode.MOV_D_L:
                    return MOV(OpD, OpL);
                case OpCode.MOV_D_M:
                    return MOV(OpD, OpMem);
                case OpCode.MOV_D_A:
                    return MOV(OpD, OpA);
                case OpCode.MOV_E_B:
                    return MOV(OpE, OpB);
                case OpCode.MOV_E_C:
                    return MOV(OpE, OpC);
                case OpCode.MOV_E_D:
                    return MOV(OpE, OpD);
                case OpCode.MOV_E_E:
                    return MOV(OpE, OpE);
                case OpCode.MOV_E_H:
                    return MOV(OpE, OpH);
                case OpCode.MOV_E_L:
                    return MOV(OpE, OpL);
                case OpCode.MOV_E_M:
                    return MOV(OpE, OpMem);
                case OpCode.MOV_E_A:
                    return MOV(OpE, OpA);
                case OpCode.MOV_H_B:
                    return MOV(OpH, OpB);
                case OpCode.MOV_H_C:
                    return MOV(OpH, OpC);
                case OpCode.MOV_H_D:
                    return MOV(OpH, OpD);
                case OpCode.MOV_H_E:
                    return MOV(OpH, OpE);
                case OpCode.MOV_H_H:
                    return MOV(OpH, OpH);
                case OpCode.MOV_H_L:
                    return MOV(OpH, OpL);
                case OpCode.MOV_H_M:
                    return MOV(OpH, OpMem);
                case OpCode.MOV_H_A:
                    return MOV(OpH, OpA);
                case OpCode.MOV_L_B:
                    return MOV(OpL, OpB);
                case OpCode.MOV_L_C:
                    return MOV(OpL, OpC);
                case OpCode.MOV_L_D:
                    return MOV(OpL, OpD);
                case OpCode.MOV_L_E:
                    return MOV(OpL, OpE);
                case OpCode.MOV_L_H:
                    return MOV(OpL, OpH);
                case OpCode.MOV_L_L:
                    return MOV(OpL, OpL);
                case OpCode.MOV_L_M:
                    return MOV(OpL, OpMem);
                case OpCode.MOV_L_A:
                    return MOV(OpL, OpA);
                case OpCode.MOV_M_B:
                    return MOV(OpMem, OpB);
                case OpCode.MOV_M_C:
                    return MOV(OpMem, OpC);
                case OpCode.MOV_M_D:
                    return MOV(OpMem, OpD);
                case OpCode.MOV_M_E:
                    return MOV(OpMem, OpE);
                case OpCode.MOV_M_H:
                    return MOV(OpMem, OpH);
                case OpCode.MOV_M_L:
                    return MOV(OpMem, OpL);
                case OpCode.HLT:
                    return HLT();
                case OpCode.MOV_M_A:
                    return MOV(OpMem, OpA);
                case OpCode.MOV_A_B:
                    return MOV(OpA, OpB);
                case OpCode.MOV_A_C:
                    return MOV(OpA, OpC);
                case OpCode.MOV_A_D:
                    return MOV(OpA, OpD);
                case OpCode.MOV_A_E:
                    return MOV(OpA, OpE);
                case OpCode.MOV_A_H:
                    return MOV(OpA, OpH);
                case OpCode.MOV_A_L:
                    return MOV(OpA, OpL);
                case OpCode.MOV_A_M:
                    return MOV(OpA, OpMem);
                case OpCode.MOV_A_A:
                    return MOV(OpA, OpA);
                case OpCode.ADD_B:
                    return ADD(OpB);
                case OpCode.ADD_C:
                    return ADD(OpC);
                case OpCode.ADD_D:
                    return ADD(OpD);
                case OpCode.ADD_E:
                    return ADD(OpE);
                case OpCode.ADD_H:
                    return ADD(OpH);
                case OpCode.ADD_L:
                    return ADD(OpL);
                case OpCode.ADD_M:
                    return ADD(OpMem);
                case OpCode.ADD_A:
                    return ADD(OpA);
                case OpCode.ADC_B:
                    return ADC(OpB);
                case OpCode.ADC_C:
                    return ADC(OpC);
                case OpCode.ADC_D:
                    return ADC(OpD);
                case OpCode.ADC_E:
                    return ADC(OpE);
                case OpCode.ADC_H:
                    return ADC(OpH);
                case OpCode.ADC_L:
                    return ADC(OpL);
                case OpCode.ADC_M:
                    return ADC(OpMem);
                case OpCode.ADC_A:
                    return ADC(OpA);
                case OpCode.SUB_B:
                    return SUB(OpB);
                case OpCode.SUB_C:
                    return SUB(OpC);
                case OpCode.SUB_D:
                    return SUB(OpD);
                case OpCode.SUB_E:
                    return SUB(OpE);
                case OpCode.SUB_H:
                    return SUB(OpH);
                case OpCode.SUB_L:
                    return SUB(OpL);
                case OpCode.SUB_M:
                    return SUB(OpMem);
                case OpCode.SUB_A:
                    return SUB(OpA);
                case OpCode.SBB_B:
                    return SBB(OpB);
                case OpCode.SBB_C:
                    return SBB(OpC);
                case OpCode.SBB_D:
                    return SBB(OpD);
                case OpCode.SBB_E:
                    return SBB(OpE);
                case OpCode.SBB_H:
                    return SBB(OpH);
                case OpCode.SBB_L:
                    return SBB(OpL);
                case OpCode.SBB_M:
                    return SBB(OpMem);
                case OpCode.SBB_A:
                    return SBB(OpA);
                case OpCode.ANA_B:
                    return ANA(OpB);
                case OpCode.ANA_C:
                    return ANA(OpC);
                case OpCode.ANA_D:
                    return ANA(OpD);
                case OpCode.ANA_E:
                    return ANA(OpE);
                case OpCode.ANA_H:
                    return ANA(OpH);
                case OpCode.ANA_L:
                    return ANA(OpL);
                case OpCode.ANA_M:
                    return ANA(OpMem);
                case OpCode.ANA_A:
                    return ANA(OpA);
                case OpCode.XRA_B:
                    return XRA(OpB);
                case OpCode.XRA_C:
                    return XRA(OpC);
                case OpCode.XRA_D:
                    return XRA(OpD);
                case OpCode.XRA_E:
                    return XRA(OpE);
                case OpCode.XRA_H:
                    return XRA(OpH);
                case OpCode.XRA_L:
                    return XRA(OpL);
                case OpCode.XRA_M:
                    return XRA(OpMem);
                case OpCode.XRA_A:
                    return XRA(OpA);
                case OpCode.ORA_B:
                    return ORA(OpB);
                case OpCode.ORA_C:
                    return ORA(OpC);
                case OpCode.ORA_D:
                    return ORA(OpD);
                case OpCode.ORA_E:
                    return ORA(OpE);
                case OpCode.ORA_H:
                    return ORA(OpH);
                case OpCode.ORA_L:
                    return ORA(OpL);
                case OpCode.ORA_M:
                    return ORA(OpMem);
                case OpCode.ORA_A:
                    return ORA(OpA);
                case OpCode.CMP_B:
                    return CMP(OpB);
                case OpCode.CMP_C:
                    return CMP(OpC);
                case OpCode.CMP_D:
                    return CMP(OpD);
                case OpCode.CMP_E:
                    return CMP(OpE);
                case OpCode.CMP_H:
                    return CMP(OpH);
                case OpCode.CMP_L:
                    return CMP(OpL);
                case OpCode.CMP_M:
                    return CMP(OpMem);
                case OpCode.CMP_A:
                    return CMP(OpA);
                case OpCode.RNZ:
                    return RNZ();
                case OpCode.POP_B:
                    return POP(RegisterIndexPair.BC);
                case OpCode.JNZ_adr:
                    return JNZ(Read(), Read());
                case OpCode.JMP_adr:
                    return JMP(Read(), Read());
                case OpCode.CNZ_adr:
                    return CNZ(Read(), Read());
                case OpCode.PUSH_B:
                    return PUSH(RegisterIndexPair.BC);
                case OpCode.ADI_D8:
                    return ADI(Read());
                case OpCode.RST_0:
                    return RST(0);
                case OpCode.RZ:
                    return RZ();
                case OpCode.RET:
                    return RET();
                case OpCode.JZ_adr:
                    return JZ(Read(), Read());
                case OpCode.NOP6:
                    return NOP();
                case OpCode.CZ_adr:
                    return CZ(Read(), Read());
                case OpCode.CALL_adr:
                    return CALL(Read(), Read());
                case OpCode.ACI_D8:
                    return ACI(Read());
                case OpCode.RST_1:
                    return RST(1);
                case OpCode.RNC:
                    return RNC();
                case OpCode.POP_D:
                    return POP(RegisterIndexPair.DE);
                case OpCode.JNC_adr:
                    return JNC(Read(), Read());
                case OpCode.OUT_D8:
                    return OUT(Read());
                case OpCode.CNC_adr:
                    return CNC(Read(), Read());
                case OpCode.PUSH_D:
                    return PUSH(RegisterIndexPair.DE);
                case OpCode.SUI_D8:
                    return SUI(Read());
                case OpCode.RST_2:
                    return RST(2);
                case OpCode.RC:
                    return RC();
                case OpCode.NOP7:
                    return NOP();
                case OpCode.JC_adr:
                    return JC(Read(), Read());
                case OpCode.IN_D8:
                    return IN(Read());
                case OpCode.CC_adr:
                    return CC(Read(), Read());
                case OpCode.NOP8:
                    return NOP();
                case OpCode.SBI_D8:
                    return SBI(Read());
                case OpCode.RST_3:
                    return RST(3);
                case OpCode.RPO:
                    return RPO();
                case OpCode.POP_H:
                    return POP(RegisterIndexPair.HL);
                case OpCode.JPO_adr:
                    return JPO(Read(), Read());
                case OpCode.XTHL:
                    return XTHL();
                case OpCode.CPO_adr:
                    return CPO(Read(), Read());
                case OpCode.PUSH_H:
                    return PUSH(RegisterIndexPair.HL);
                case OpCode.ANI_D8:
                    return ANI(Read());
                case OpCode.RST_4:
                    return RST(4);
                case OpCode.RPE:
                    return RPE();
                case OpCode.PCHL:
                    return PCHL();
                case OpCode.JPE_adr:
                    return JPE(Read(), Read());
                case OpCode.XCHG:
                    return XCHG();
                case OpCode.CPE_adr:
                    return CPE(Read(), Read());
                case OpCode.NOP9:
                    return NOP();
                case OpCode.XRI_D8:
                    return XRI(Read());
                case OpCode.RST_5:
                    return RST(5);
                case OpCode.RP:
                    return RP();
                case OpCode.POP_PSW:
                    return POP(RegisterIndexPair.PSW);
                case OpCode.JP_adr:
                    return JP(Read(), Read());
                case OpCode.DI:
                    return DI();
                case OpCode.CP_adr:
                    return CP(Read(), Read());
                case OpCode.PUSH_PSW:
                    return PUSH(RegisterIndexPair.PSW);
                case OpCode.ORI_D8:
                    return ORI(Read());
                case OpCode.RST_6:
                    return RST(6);
                case OpCode.RM:
                    return RM();
                case OpCode.SPHL:
                    return SPHL();
                case OpCode.JM_adr:
                    return JM(Read(), Read());
                case OpCode.EI:
                    return EI();
                case OpCode.CM_adr:
                    return CM(Read(), Read());
                case OpCode.NOPA:
                    return NOP();
                case OpCode.CPI_D8:
                    return CPI(Read());
                case OpCode.RST_7:
                    return RST(7);
                default:
                    throw new Exception();
            }
        }
    }
}