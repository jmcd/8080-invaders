namespace Intel8080
{
    public class RegisterIndexPair
    {
        private RegisterIndexPair(Registers.Index lhs, Registers.Index rhs)
        {
            LHS = lhs;
            RHS = rhs;
        }

        public Registers.Index LHS { get; }
        public Registers.Index RHS { get; }

        public static readonly RegisterIndexPair PSW = new RegisterIndexPair(Registers.Index.A, Registers.Index.F);
        public static readonly RegisterIndexPair BC = new RegisterIndexPair(Registers.Index.B, Registers.Index.C);
        public static readonly RegisterIndexPair DE = new RegisterIndexPair(Registers.Index.D, Registers.Index.E);
        public static readonly RegisterIndexPair HL = new RegisterIndexPair(Registers.Index.H, Registers.Index.L);
    }
}