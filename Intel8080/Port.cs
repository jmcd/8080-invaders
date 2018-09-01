using System;
using System.Diagnostics;

namespace Intel8080
{
    public class Port
    {
        public Func<byte> OnIn;
        public Action<byte> OnOut;

        public void Out(byte b)
        {
            #if DEBUG
            if (OnOut == null)
            {
                Debug.WriteLine("no out defined");
            }
            #endif
            OnOut.Invoke(b);
        }

        public byte In()
        {
            return OnIn.Invoke();
        }
    }
}