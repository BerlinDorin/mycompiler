using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class Pcode
    {
        //10种操作码
        public static String[] PCODE =
        {
            "LIT", "OPR", "LOD", "STO", "CAL",
            "INT", "JMP", "JPC", "RED", "WRT"
        };
        public int f;
        public int l;
        public int a;

        public Pcode(int f, int l, int a)
        {
            this.f = f;
            this.l = l;
            this.a = a;
        }
    }
}
