using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

namespace Intel8080.Tests
{
    public class ThirdPartyMachineCodeTestProgramRunner
    {
        [Fact]
        public void Run_cpudiag_bin()
        {
            Exercise("cpudiag.bin", "CPU IS OPERATIONAL");
        }

        [Fact]
        public void Run_CPUTEST_COM()
        {
            Exercise("CPUTEST.COM", @"
DIAGNOSTICS II V1.2 - CPU TEST
COPYRIGHT (C) 1981 - SUPERSOFT ASSOCIATES

ABCDEFGHIJKLMNOPQRSTUVWXYZ
CPU IS 8080/8085
BEGIN TIMING TEST
END TIMING TEST
CPU TESTS OK
");
        }

        [Fact]
        public void Run_8080EX1_COM()
        {
            Exercise("8080EX1.COM", @"8080 instruction exerciser (KR580VM80A CPU)
dad <b,d,h,sp>................  OK
aluop nn......................  OK
aluop <b,c,d,e,h,l,m,a>.......  OK
<daa,cma,stc,cmc>.............  OK
<inr,dcr> a...................  OK
<inr,dcr> b...................  OK
<inx,dcx> b...................  OK
<inr,dcr> c...................  OK
<inr,dcr> d...................  OK
<inx,dcx> d...................  OK
<inr,dcr> e...................  OK
<inr,dcr> h...................  OK
<inx,dcx> h...................  OK
<inr,dcr> l...................  OK
<inr,dcr> m...................  OK
<inx,dcx> sp..................  OK
lhld nnnn.....................  OK
shld nnnn.....................  OK
lxi <b,d,h,sp>,nnnn...........  OK
ldax <b,d>....................  OK
mvi <b,c,d,e,h,l,m,a>,nn......  OK
mov <bcdehla>,<bcdehla>.......  OK
sta nnnn / lda nnnn...........  OK
<rlc,rrc,ral,rar>.............  OK
stax <b,d>....................  OK
Tests complete
");
        }

        [Fact]
        public void Run_8080PRE_COM()
        {
            Exercise("8080PRE.COM", "8080 Preliminary tests complete");
        }

        private static void Exercise(string programFilename, string expt)
        {
            var mem = ConstructWithContent(programFilename);
            var cpmOutput = "";
            var cpu = new CPUWithCPMOutputTrap(mem, (s, terminator) =>
            {
                cpmOutput += s + terminator;

                // Hack to make the 8080EX1.COM fail ASAP, because this test takes a loooooong time to run
                if (Regex.IsMatch(cpmOutput, @"ERROR \*\*\*\* crc expected:.{8} found:.{8}"))
                {
                    throw new Exception(cpmOutput);
                }
            });

            while (cpu.programCounter != 0)
            {
                cpu.CycleCountByExecutingNextInstruction();
            }

            string Normalize(string s) =>
                Regex.Replace(
                    Regex.Replace(s
                        , @"[^\u000c-\u007F]+", string.Empty)
                    , @"\r+", "\n").Trim();

            var expected = Normalize(expt);
            var actual = Normalize(cpmOutput);

            Debug.WriteLine(cpmOutput);
            Debug.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        private static RAM ConstructWithContent(string programFilename)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            var stream = executingAssembly.GetManifestResourceStream($"{executingAssembly.GetName().Name}.{programFilename}");

            var buffer = new byte[0x10000];
            stream.Read(buffer, 0x100, (int) stream.Length);

            buffer[5] = 0xc9; //return from CP/M output routine

            var mem = new RAM(buffer);
            return mem;
        }
    }
}