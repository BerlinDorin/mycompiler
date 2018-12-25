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
        public enum PC
        {
            LIT, OPR, LOD, STO, CAL,
            INT, JMP, JPC, RED, WRT, NUL
        };
        public static String[] PCODE =
        {
            "LIT", "OPR", "LOD", "STO", "CAL",
            "INT", "JMP", "JPC", "RED", "WRT"
        };
       
        public PC f;     //操作码
        public int l;    //层次差
        public int a;

        public Pcode(PC f, int l, int a)
        {
            this.f = f;
            this.l = l;
            this.a = a;
        }
    }
}
