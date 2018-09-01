namespace SpaceInvaders
{
    public static class InputsConvenience
    {
        public static void SetShipCount(this Inputs inputs, int value) => inputs[2] |= (byte) (value - 3);

        public static int GetShipCount(this Inputs inputs) => 3 + (inputs[2] & 3);

        public static void SetExtraShipScore(this Inputs inputs, int value)
        {
            if (value == 1000)
            {
                inputs[2] |= 8;
            }
            else
            {
                inputs[2] &= 0xff - 8;
            }
        }

        public static int GetExtraShipScore(this Inputs inputs) => (inputs[2] & 8) > 0 ? 1000 : 1500;

        public static void SetPortBit(this Inputs inputs, int port, int bit)
        {
            inputs[port] = (byte) (inputs[port] | (1 << bit));
        }

        public static void UnsetPortBit(this Inputs inputs, int port, int bit)
        {
            inputs[port] = (byte) (inputs[port] & ~(1 << bit));
        }
    }
}