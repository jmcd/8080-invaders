namespace Intel8080
{
    public partial class CPU
    {
        private int NOP()
        {
            return 4;
        }

        private int HLT()
        {
            return 7;
        }

        // OUT 2 will send A to port 2
        private int OUT(byte b)
        {
            var a = Accumulator;
            ports[b].Out(a);
            return 10;
        }

        // IN 3 will put the value of port 3 into register A
        private int IN(byte b)
        {
            var a = ports[b].In();
            Accumulator = a;
            return 10;
        }

        public bool interruptsAreEnabled = true;

        // DI Disable Interrupts
        private int DI()
        {
            interruptsAreEnabled = false;
            return 4;
        }

        private int EI()
        {
            interruptsAreEnabled = true;
            return 4;
        }
    }
}