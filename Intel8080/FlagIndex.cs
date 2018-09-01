namespace Intel8080
{
    public enum FlagIndex
    {
        /*
         7	6	5	4	3	2	1	0
         S	Z	0	A	0	P	1	C
         */
        Carry = 0,

        //  AlwaysOne=1,
        Parity = 2,

        //AlwaysZero0=3,
        AuxCarry = 4,

        // AlwaysZero1=5,
        Zero = 6,
        Sign = 7
    }
}