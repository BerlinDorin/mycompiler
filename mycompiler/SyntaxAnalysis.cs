using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class SyntaxAnalysis
    {
        public enum SY
        {
            DEFAULT, CONSTSY, VARSY, PROCEDURESY, ODDSY, IFSY,
            THENSY, ELSESY, WHILESY, DOSY, CALLSY, BEGINSY,
            ENDSY, REPEATSY, UNTILSY, READSY, WRITESY,
            COMMASY, SEMISY, LPARSY, RPARSY, COLONSY, DOTSY,
            PLUSSY, MINUSSY, STARSY, DIVISY, EQUSY,
            LESSSY, GRTSY, ASSIGNSY, UNEQUSY, LESY, GESY, INTSY, IDSY
        }
        public static String[] names =
                                { "","const", "var", "procedure", "odd", "if",
                                "then", "else", "while", "do", "call", "begin",
                                "end", "repeat", "until", "read", "write",
                                 ",", ";", "(", ")", ":", ".",
                                 "+", "-", "*", "/","=",
                                  "<", ">", ":=", "<>", "<=", ">="
                                };
        private LexcialAnalysis lex;   //词法分析器
        private SymbolTable symtable;  // 符号表
        public Errors err;             //错误处理
        public LexcialAnalysis.Word[] wordlist;    //词法分析结果表
        private int usedNum;                       //记录当前读取getsym当前位置
        private int wordNum;                      //词法分析结果表记录数
        private LexcialAnalysis.Word tmpsym;     //当前读取的单词

        private Pcode[] pcode;
        private int pcodeNum;
        private int dx;                 //当前作用域的堆栈帧大小
        private void init()
        {
            pcode = new Pcode[2000];
        }
        //调用词法分析器
        public void GenerateWordlist(String s, int lineID)
        {
            lex.LexAnaly(s,  lineID);
        }
        //完成将所有的单词的词法分析后，将lex中的结果转移
        private void FinishWordlist()
        {
            wordlist = lex.Words;
            usedNum = 0;
            wordNum = lex.Wordscnt;
        }

        private bool Getsym()
        {
            if (usedNum >= wordNum )
                return false;
            tmpsym = wordlist[usedNum];
            usedNum++;
            return true;
        }

        private bool Gen(int f,int l,int a)
        {
            if (pcodeNum > 2000)
            {
                Console.WriteLine("超过符号表最大长度！");
                return false;
               
            }
            pcode[pcodeNum].f = f;
            pcode[pcodeNum].l = l;
            pcode[pcodeNum].a = a;
            pcodeNum++;
            dx++;
            return true;
        }

    }
}
