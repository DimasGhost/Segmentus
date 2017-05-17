using System;

namespace Segmentus
{
    struct Bitmask128
    {
        public static readonly Bitmask128 Zero = new Bitmask128(0, 0);

        public ulong l, r;
        
        public Bitmask128(ulong l, ulong r) { this.l = l; this.r = r; }

        public Bitmask128(int firstTrueBitsCnt)
        {
            if (firstTrueBitsCnt >= 64)
            {
                l = ulong.MaxValue;
                if (firstTrueBitsCnt == 128)
                    r = ulong.MaxValue;
                else
                    r = ((ulong)1 << (firstTrueBitsCnt - 64)) - 1;
            }
            else
            {
                l = ((ulong)1 << firstTrueBitsCnt) - 1;
                r = 0;
            }
        }

        public bool this[int i]
        {
            get
            {
                if (i < 64)
                    return ((l & ((ulong)1 << i)) > 0);
                else
                    return ((r & ((ulong)1 << (i - 64))) > 0);
            }
            set
            {
                if (this[i] == value)
                    return;
                if (i < 64)
                    l ^= ((ulong)1 << i);
                else
                    r ^= ((ulong)1 << (i - 64));
            }
        }

        public static Bitmask128 operator &(Bitmask128 a, Bitmask128 b)
            => new Bitmask128(a.l & b.l, a.r & b.r);

        public override int GetHashCode()
        {
            return (int)(l % int.MaxValue);
        }
    }
}