using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class SyntaxAnalysis
    {

        public LexicalAnalysis lex;               //词法分析器
        private SymbolTable symtable;              // 符号表
        public Interpreter ipt;                    //解释器
        public Errors err;                         //错误处理
        public LexicalAnalysis.Word[] wordlist;    //词法分析结果表
        private int usedNum;                       //记录当前读取getsym当前位置
        public int wordNum;                      //词法分析结果表记录数
        public LexicalAnalysis.Word tmpword;     //当前读取的单词

       
        private int pcodeNum;
        private int dx;                 //用于计算每个变量在运行栈中相对本过程基地址的偏移量 
        private int tx;                 //table的指针
        private int cx;                 //pcode的指针
        //first集合
        private List<LexicalAnalysis.SY> decPre;
        private List<LexicalAnalysis.SY> statePre;
        private List<LexicalAnalysis.SY> factorPre;

        public SyntaxAnalysis()
        {
            lex = new LexicalAnalysis();
            symtable = new SymbolTable();
            ipt = new Interpreter();
            err = new Errors();

           
            decPre = new List<LexicalAnalysis.SY>();
            statePre = new List<LexicalAnalysis.SY>();
            factorPre = new List<LexicalAnalysis.SY>();

            decPre.Add(LexicalAnalysis.SY.CONSTSY);
            decPre.Add(LexicalAnalysis.SY.VARSY);
            decPre.Add(LexicalAnalysis.SY.PROCEDURESY);

            factorPre.Add(LexicalAnalysis.SY.IDSY);
            factorPre.Add(LexicalAnalysis.SY.INTSY);
            factorPre.Add(LexicalAnalysis.SY.LPARSY);

            statePre.Add(LexicalAnalysis.SY.BEGINSY);
            statePre.Add(LexicalAnalysis.SY.CALLSY);
            statePre.Add(LexicalAnalysis.SY.IFSY);
            statePre.Add(LexicalAnalysis.SY.WHILESY);
            statePre.Add(LexicalAnalysis.SY.REPEATSY);

        }
        public void Compile()
        {
           
            List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
            next.AddRange(decPre);
            next.AddRange(statePre);
            next.Add(LexicalAnalysis.SY.DOTSY);
            Getsym();
            Block(0, next);
            if (tmpword.Sym != LexicalAnalysis.SY.DOTSY)
                err.AddError(9, tmpword.LineID, tmpword.token);

        }
        //调用词法分析器
        public void GenerateWordlist(String s, int lineID)
        {
            lex.errorcnt = 0;
            lex.LexAnaly(s, lineID);
            if (lex.errorcnt != 0)
            {  
                for(int i = 0; i < lex.errorcnt; i++)
                {
                    err.AddError(lex.errorlist[i].id,lineID, lex.errorlist[i].token);
                }
                
            }
            
        }
        //完成将所有的单词的词法分析后，将lex中的结果转移
        public void FinishWordlist()
        {
            wordlist = lex.Words;
            usedNum = 0;
            wordNum = lex.Wordscnt;
        }
        //取单词表中的下一个单词
        public void Getsym()
        {   
            if (usedNum >= wordNum)
            {
                tmpword.token = "";
                tmpword.Sym = LexicalAnalysis.SY.DEFAULT;
                return;
            }
           
            tmpword = wordlist[usedNum];
            usedNum++;
            if (tmpword.Sym == LexicalAnalysis.SY.DOTSY && usedNum < wordNum - 1)
            {
                err.AddError(26, tmpword.LineID, tmpword.token);   //结束符只能出现在程序结尾
                tmpword = wordlist[usedNum];
                usedNum++;
            }

        }
        //生成目标代码
        //private bool Gen(Pcode.PC f,int l,int a)
        //{
        //    if (pcodeNum > Interpreter.pcodeSize)
        //    {
        //        Console.WriteLine("超过目标代码最大长度！");
        //        return false;
               
        //    }
        //    pcode[pcodeNum].f = f;
        //    pcode[pcodeNum].l = l;
        //    pcode[pcodeNum].a = a;
        //    pcodeNum++;
        //    dx++;
        //    return true;
        //}

        /*分程序部分
         * <分程序> ::= [<常量说明部分>][变量说明部分>][<过程说明部分>]<语句>
         */
        private void Block(int level, List<LexicalAnalysis.SY>followSet)
        {
            List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
            int tmpdx = dx;
            int tmptx = symtable.tablecnt;                //初始化时的符号表的索引tx
            int tmpcx;
            dx = 3;
            symtable.SetAddr(ipt.currentCode, symtable.tablecnt);
            ipt.AddPcode(Pcode.PC.JMP, 0, 0);                  //跳转地址暂时填0
            do
            {
                //<常量说明部分> ::= const<常量定义>{,<常量定义>};
                if (tmpword.Sym == LexicalAnalysis.SY.CONSTSY)
                {
                    Getsym();
                    ConstDeclaration(level);
                    while (tmpword.Sym == LexicalAnalysis.SY.COMMASY)
                    {
                        Getsym();
                        ConstDeclaration(level);
                    }
                    if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                        Getsym();
                    else
                        err.AddError(5, tmpword.LineID, tmpword.token);        //漏掉了分号
                }
                //<变量说明部分>::= var<标识符>{,<标识符>};
                 if (tmpword.Sym == LexicalAnalysis.SY.VARSY)
                {
                    Getsym();
                    VarDeclaration(level);
                    while (tmpword.Sym == LexicalAnalysis.SY.COMMASY)
                    {
                        Getsym();
                        VarDeclaration(level);
                    }
                    if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                        Getsym();
                    else
                    {  
                        if(tmpword.Sym == LexicalAnalysis.SY.ASSIGNSY|| tmpword.Sym == LexicalAnalysis.SY.EQUSY)
                        {
                            err.AddError(27, tmpword.LineID, tmpword.token);   //变量赋值语句不能出现在变量声明语句中
                            Getsym();
                        }
                        
                        else 
                          err.AddError(5, tmpword.LineID, tmpword.token);     //漏掉了分号
                    }
                }
                //<过程说明部分> ::= <过程首部><分程序>;{<过程说明部分>}
                while (tmpword.Sym == LexicalAnalysis.SY.PROCEDURESY)
                {
                    Getsym();
                    if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
                    {
                        symtable.TableFill(tmpword.token, 2, 0, level, ipt.currentCode + 1);
                        Getsym();
                    }
                    else
                    {
                        err.AddError(4, tmpword.LineID, tmpword.token);            //procedure后应为标识符
                    }
                    if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                    {
                        Getsym();
                    }
                    else
                        err.AddError(5, tmpword.LineID, tmpword.token);        //漏掉分号
                    next.Clear();
                    next.AddRange(followSet);
                    next.Add(LexicalAnalysis.SY.SEMISY);
                    Block(level + 1, next);    //递归
                    if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                    {
                        Getsym();
                        next.Clear();
                        next.AddRange(statePre);
                        next.Add(LexicalAnalysis.SY.IDSY);
                        next.Add(LexicalAnalysis.SY.PROCEDURESY);
                        Test(next, followSet, 6);     //过程说明后的符号是否不正确（应是语句开始符或过程定义符）
                    }
                    else
                        err.AddError(5, tmpword.LineID, tmpword.token);    //漏掉分号
                }
                next.Clear();
                next.AddRange(statePre);
                next.Add(LexicalAnalysis.SY.IDSY);
                Test(next, decPre, 7);          //应为语句
            } while (decPre.Contains(tmpword.Sym));
            //SymbolTable.Item item = symtable.table[tmptx];
            ipt.pcodeList[symtable.table[tmptx].address].a = ipt.currentCode;
            symtable.table[tmptx].address = ipt.currentCode;
            symtable.table[tmptx].size = dx;
            tmpcx = ipt.currentCode;
            ipt.AddPcode(Pcode.PC.INT, 0, dx);

            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.SEMISY);
            next.Add(LexicalAnalysis.SY.ENDSY);

            Statement(level, next);
            ipt.AddPcode(Pcode.PC.OPR, 0, 0);
            next = new List<LexicalAnalysis.SY>();
            next.Clear();
            Test(followSet, next, 8);      //程序体内语句部分后的符号是否正确
            dx = tmpdx;
            symtable.tablecnt = tmptx;


        }
        //语句部分，调用8个语句分析子程序
        private void Statement(int level, List<LexicalAnalysis.SY> followSet)
        {   
            
            switch (tmpword.Sym)
            {
                case LexicalAnalysis.SY.IDSY:       
                    IdentState(level, followSet);
                    break;
                case LexicalAnalysis.SY.CALLSY:
                    CallState(level, followSet);
                    break;
                case LexicalAnalysis.SY.BEGINSY:
                    BeginState(level, followSet);
                    break;
                case LexicalAnalysis.SY.IFSY:
                    IfState(level, followSet);
                    break;
                case LexicalAnalysis.SY.WHILESY:
                    WhileState(level, followSet);
                    break;
                case LexicalAnalysis.SY.READSY:
                    ReadState(level, followSet);
                    break;
                case LexicalAnalysis.SY.WRITESY:
                    WriteState(level, followSet);
                    break;
                case LexicalAnalysis.SY.REPEATSY:
                    RepeatState(level, followSet);
                    break;
                
                default:
                    List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                    Test(followSet, next, 19);               //判断语句后的符号是否正确
                    break;
            }
        }
        //ident赋值模块
        private void IdentState(int level, List<LexicalAnalysis.SY> followSet)
        {
            int idx = symtable.Position(tmpword.token);

            if (idx <= 0)
                err.AddError(11, tmpword.LineID, tmpword.token);         //标识符未说明
            else if (symtable.table[idx].type != 1)                    
            {
                err.AddError(12, tmpword.LineID, tmpword.token);        //赋值语句中，赋值号左部标识符属性应是变量
                idx = -1;
            }
            Getsym();
            if (tmpword.Sym == LexicalAnalysis.SY.ASSIGNSY)
                Getsym();   
            else
                err.AddError(13, tmpword.LineID, tmpword.token);       // 赋值语句左部标识符后应是赋值运算符\":=\
                           
            Expression(level, followSet);
            if (idx > 0)
            {
                SymbolTable.Item item = symtable.table[idx];
                ipt.AddPcode(Pcode.PC.STO, level - item.level, item.address);
            }
        }
        //read模块
        private void ReadState(int level, List<LexicalAnalysis.SY> followSet)
        {
            Getsym();
            if (tmpword.Sym == LexicalAnalysis.SY.LPARSY)
            {
                int idx=-1;
                do
                {
                    Getsym();
                    if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
                    {
                        idx = symtable.Position(tmpword.token);
                    }
                    if (idx < 0)
                    {
                        err.AddError(11, tmpword.LineID, tmpword.token);        //标识符未说明
                    }
                    else
                    {
                        SymbolTable.Item item = symtable.table[idx];
                        if (item.type == 1)           //类型为变量
                        {
                            ipt.AddPcode(Pcode.PC.RED, level - item.level, item.address);

                            //ipt.AddPcode(Pcode.PC.STO, level - item.level, item.address);
                        }
                        else
                        {
                            err.AddError(31, tmpword.LineID, tmpword.token);             // read括号内应该是变量标识符
                        }
                    }
                    Getsym();
                } while (tmpword.Sym == LexicalAnalysis.SY.COMMASY);
            }
            else
            {
                err.AddError(40, tmpword.LineID, tmpword.token);          // 应为左括号
            }
            if (tmpword.Sym == LexicalAnalysis.SY.RPARSY)
                Getsym();
            else
                err.AddError(22, tmpword.LineID, tmpword.token);          //表达式中漏掉右括号
        }
        //write模块
        private void WriteState(int level, List<LexicalAnalysis.SY> followSet)
        {
            Getsym();
            if (tmpword.Sym == LexicalAnalysis.SY.LPARSY)
            {
                do
                {
                    Getsym();
                    List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                    next.Clear();
                    next.AddRange(followSet);
                    next.Add(LexicalAnalysis.SY.COMMASY);
                    next.Add(LexicalAnalysis.SY.RPARSY);
                    Expression(level, next);
                    ipt.AddPcode(Pcode.PC.WRT, 0, 0);
                } while (tmpword.Sym == LexicalAnalysis.SY.COMMASY);
            }
            else
            {
                err.AddError(40, tmpword.LineID, tmpword.token);          // 应为左括号
            }
            if (tmpword.Sym == LexicalAnalysis.SY.RPARSY)
                Getsym();
            else
                err.AddError(22, tmpword.LineID, tmpword.token);          //表达式中漏掉右括号
           
        }
        //<过程调用语句> ::= call<标识符>
        private void CallState(int level, List<LexicalAnalysis.SY> followSet)
        {
            Getsym();
            if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
            {
                int idx = symtable.Position(tmpword.token);
                if (idx > 0)
                {
                    SymbolTable.Item item = symtable.table[idx];
                    if (item.type == 2)                  //类型为子程序
                    {
                        Console.WriteLine(item.name + item.address);
                        ipt.AddPcode(Pcode.PC.CAL, level - item.level, item.address);
                    }
                    else
                        err.AddError(15, tmpword.LineID, tmpword.token);     //call不能调用常量或者变量
                }
                else
                    err.AddError(11, tmpword.LineID, tmpword.token);      //标识符未说明
                Getsym();
            }
            else
                err.AddError(14, tmpword.LineID, tmpword.token);        //call后应为标识符
        }
        //<条件语句> ::= if<条件>then<语句>[else<语句>]
        private void IfState(int level, List<LexicalAnalysis.SY> followSet)
        {
            Getsym();
            List<LexicalAnalysis.SY> next =new List<LexicalAnalysis.SY>();
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.THENSY);
            next.Add(LexicalAnalysis.SY.DOSY);
            Condition(level, next);
            if (tmpword.Sym == LexicalAnalysis.SY.THENSY)
                Getsym();
            else
                err.AddError(16, tmpword.LineID, tmpword.token);    //条件语句中缺失了then
            int tmpcx = ipt.currentCode;

            ipt.AddPcode(Pcode.PC.JPC, 0, 0);
            //changed
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.ELSESY);
            Statement(level,next);
            ipt.pcodeList[tmpcx].a = ipt.currentCode;

            if (tmpword.Sym == LexicalAnalysis.SY.ELSESY)
            {
                ipt.pcodeList[tmpcx].a++;
                Getsym();
                int tmpcx1 = ipt.currentCode;
                ipt.AddPcode(Pcode.PC.JMP, 0, 0);
                Statement(level, followSet);
                ipt.pcodeList[tmpcx1].a = ipt.currentCode;
            }
        }
        //<复合语句> ::= begin<语句>{;<语句>}end
        private void BeginState(int level, List<LexicalAnalysis.SY> followSet)
        {
            Getsym();
            List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.SEMISY);
            next.Add(LexicalAnalysis.SY.ENDSY);
            Statement(level, next);

            while (statePre.Contains(tmpword.Sym) || tmpword.Sym == LexicalAnalysis.SY.SEMISY)
            {
                if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                    Getsym();
                
                else
                {
                    err.AddError(10, tmpword.LineID, tmpword.token);       //语句之间漏了分号
                }      
                Statement(level, next);
            }
            if (tmpword.Sym == LexicalAnalysis.SY.ENDSY)                   //丢了end或分号
                Getsym();
            else
                err.AddError(17, tmpword.LineID, tmpword.token);
        }
        //while模块
        private void WhileState(int level, List<LexicalAnalysis.SY> followSet)
        {
            int tmpcx = ipt.currentCode;
            Getsym();
            List<LexicalAnalysis.SY> next =new List<LexicalAnalysis.SY>();
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.DOSY); ;
            Condition(level, next);

            int tmpcx1 = ipt.currentCode;
            ipt.AddPcode(Pcode.PC.JPC, 0, 0);

            if (tmpword.Sym == LexicalAnalysis.SY.DOSY)
                Getsym();
            else
                err.AddError(18, tmpword.LineID, tmpword.token);            //while型循环语句中丢了do
            Statement(level, followSet);
            ipt.AddPcode(Pcode.PC.JMP, 0, tmpcx);
            ipt.pcodeList[tmpcx1].a = ipt.currentCode;
        }
        //repeat模块
        private void RepeatState(int level, List<LexicalAnalysis.SY> followSet)
        {
            int tmpcx = ipt.currentCode;
            Getsym();
            List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.SEMISY);
            next.Add(LexicalAnalysis.SY.UNTILSY);
            Statement(level, next);

            while (statePre.Contains(tmpword.Sym) || tmpword.Sym == LexicalAnalysis.SY.SEMISY)
            {
                if (tmpword.Sym == LexicalAnalysis.SY.SEMISY)
                    Getsym();
                else
                    err.AddError(10, tmpword.LineID,tmpword.token);     //漏了分号
                Statement(level, next);
            }
            if (tmpword.Sym == LexicalAnalysis.SY.UNTILSY)
            {
                Getsym();
                Condition(level, followSet);
                ipt.AddPcode(Pcode.PC.JPC, 0, tmpcx);
            }
            else
                err.AddError(33, tmpword.LineID, tmpword.token);      //缺少until

        }

        //常量声明部分
        private void ConstDeclaration(int level)
        {
            if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
            {
                string name = tmpword.token;
                Getsym();
                if (tmpword.Sym == LexicalAnalysis.SY.EQUSY || tmpword.Sym == LexicalAnalysis.SY.ASSIGNSY)
                {
                    if(tmpword.Sym== LexicalAnalysis.SY.ASSIGNSY)
                    {
                        err.AddError(1, tmpword.LineID, tmpword.token);     //常量定义应是\"=\"而不是\":=\"
                    }
                    Getsym();
                    if (tmpword.Sym == LexicalAnalysis.SY.INTSY)
                    {
                        symtable.TableFill(name, 0, int.Parse(tmpword.token), level, dx);
                        Getsym();
                    }
                    else
                    {
                        err.AddError(2, tmpword.LineID, tmpword.token);      //常数说明中的\"=\"后应是整数
                    }
                }
                else
                    err.AddError(3, tmpword.LineID, tmpword.token);          //常数说明中的标识符后应是\"=\"
            }
            else
                err.AddError(4, tmpword.LineID, tmpword.token);         // const，var，procedure 后应是标识符
        }

        //变量声明部分
        private void VarDeclaration(int level)
        {
            if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
            {
                symtable.TableFill(tmpword.token, 1, 0, level, dx);
                dx++;
                Getsym();
            }
            else
                err.AddError(4, tmpword.LineID, tmpword.token);         // const，var，procedure 后应是标识符          
        }

        //条件部分
        private void Condition(int level, List<LexicalAnalysis.SY> followSet)
        {
            if (tmpword.Sym == LexicalAnalysis.SY.ODDSY)
            {
                Getsym();
                Expression(level, followSet);
                ipt.AddPcode(Pcode.PC.OPR, 0, 6);     //OPR 0 6;栈顶元素的奇偶判断
            }
            else
            {
                List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                next.Clear();
                next.AddRange(followSet);
                next.Add(LexicalAnalysis.SY.EQUSY);        
                next.Add(LexicalAnalysis.SY.UNEQUSY);
                next.Add(LexicalAnalysis.SY.LESSSY);
                next.Add(LexicalAnalysis.SY.LESY);
                next.Add(LexicalAnalysis.SY.GRTSY);
                next.Add(LexicalAnalysis.SY.GESY);
                Expression(level, next);        
                //当前符号是否为=,<>,<,<=,>,>=中的一种
                if(tmpword.Sym==LexicalAnalysis.SY.EQUSY|| tmpword.Sym == LexicalAnalysis.SY.UNEQUSY || tmpword.Sym == LexicalAnalysis.SY.LESSSY ||
                    tmpword.Sym == LexicalAnalysis.SY.LESY || tmpword.Sym == LexicalAnalysis.SY.GRTSY || tmpword.Sym == LexicalAnalysis.SY.GESY)
                {
                    LexicalAnalysis.SY op = tmpword.Sym;
                    Getsym();
                    Expression(level, followSet);
                    ipt.AddPcode(Pcode.PC.OPR, 0, GetOperator(op));
                }
                else
                {
                    err.AddError(20, tmpword.LineID, tmpword.token);
                }
            }
        }

        //表达式部分
        private void Expression(int level, List<LexicalAnalysis.SY> followSet)
        {
            if(tmpword.Sym==LexicalAnalysis.SY.PLUSSY|| tmpword.Sym == LexicalAnalysis.SY.MINUSSY)
            {
                LexicalAnalysis.SY op = tmpword.Sym;
                Getsym();
                List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                next.Clear();
                next.AddRange(followSet);
                next.Add(LexicalAnalysis.SY.PLUSSY);
                next.Add(LexicalAnalysis.SY.MINUSSY);
                Term(level, next);
                
                if (op == LexicalAnalysis.SY.MINUSSY)
                {
                    ipt.AddPcode(Pcode.PC.OPR, 0, 1);          //OPR 0 1;栈顶元素取反
                }
            }
            else
            {
                List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                next.Clear();
                next.AddRange(followSet);
                next.Add(LexicalAnalysis.SY.PLUSSY);
                next.Add(LexicalAnalysis.SY.MINUSSY);
                Term(level, next);
            }

            while(tmpword.Sym == LexicalAnalysis.SY.PLUSSY || tmpword.Sym == LexicalAnalysis.SY.MINUSSY)
            {
                LexicalAnalysis.SY op = tmpword.Sym;
                Getsym();
                List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                next.Clear();
                next.AddRange(followSet);
                next.Add(LexicalAnalysis.SY.PLUSSY);
                next.Add(LexicalAnalysis.SY.MINUSSY);
                Term(level, next);
                ipt.AddPcode(Pcode.PC.OPR, 0, GetOperator(op));
            }
        }

        //项部分
        private void Term(int level, List<LexicalAnalysis.SY> followSet)
        {
            List<LexicalAnalysis.SY> next =new List<LexicalAnalysis.SY>();
            next.Clear();
            next.AddRange(followSet);
            next.Add(LexicalAnalysis.SY.STARSY);
            next.Add(LexicalAnalysis.SY.DIVISY);
            Factor(level,next);
            while (tmpword.Sym == LexicalAnalysis.SY.STARSY || tmpword.Sym == LexicalAnalysis.SY.DIVISY)
            {
                LexicalAnalysis.SY op = tmpword.Sym;
                Getsym();
                Factor(level, next);
                ipt.AddPcode(Pcode.PC.OPR, 0, GetOperator(op));
            }


        }


        //因子部分
        private void Factor(int level, List<LexicalAnalysis.SY> followSet)
        {
            Test(factorPre, followSet, 24);                //判断表达式开始符号
            if (factorPre.Contains(tmpword.Sym)==true)
            {
                if (tmpword.Sym == LexicalAnalysis.SY.IDSY)
                {
                    int idx = symtable.Position(tmpword.token);
                    if (idx >= 0)
                    {
                        SymbolTable.Item item = symtable.table[idx];
                        switch (item.type)
                        {
                            case 0:                                         //取常量值于栈顶
                                ipt.AddPcode(Pcode.PC.LIT, 0, item.value);
                                break;
                            case 1:                                         //取变量值于栈顶
                                ipt.AddPcode(Pcode.PC.LOD, level - item.level, item.address);
                                break;
                            case 2:
                                err.AddError(21, tmpword.LineID, tmpword.token);             //表达式中不能有过程标识符
                                break;
                        }
                    }
                    else
                        err.AddError(11, tmpword.LineID, tmpword.token);   //标识符未说明
                    Getsym();
                }
                else if (tmpword.Sym == LexicalAnalysis.SY.INTSY)
                {
                    int num = int.Parse(tmpword.token);
                    ipt.AddPcode(Pcode.PC.LIT, 0, num);
                    Getsym();
                }
                else if (tmpword.Sym == LexicalAnalysis.SY.LPARSY)         //左括号
                {
                    Getsym();
                    List<LexicalAnalysis.SY> next = new List<LexicalAnalysis.SY>();
                    next.Clear();
                    next.AddRange(followSet);
                    next.Add(LexicalAnalysis.SY.RPARSY);
                    Expression(level, next);
                    if (tmpword.Sym == LexicalAnalysis.SY.RPARSY)     //判断下一个符号是否为右括号
                    {
                        Getsym();
                    }
                    else
                    {
                        err.AddError(22, tmpword.LineID, tmpword.token);
                    }
                }

                List<LexicalAnalysis.SY> next1 = new List<LexicalAnalysis.SY>();
                next1.Clear();
                next1.Add(LexicalAnalysis.SY.LPARSY);
                Test(followSet,next1, 23);    //判断是否有非法符号       
            }

        }

        //检测当前符号合法性，并进行跳读
        private void Test(List<LexicalAnalysis.SY>list1,List<LexicalAnalysis.SY>list2,int errId)
        {
           
            if (!list1.Contains(tmpword.Sym))
            {
                err.AddError(errId, tmpword.LineID, tmpword.token);
                //list1 = list1.Union(list2).ToList(); //合并两个集合
                //while (!list1.Contains(tmpword.Sym)&&usedNum<wordNum)          //跳读，直到读到合法字符
                //{
                //    Getsym();
                //}
                while (!list1.Contains(tmpword.Sym)&&!list2.Contains(tmpword.Sym)&&usedNum<wordNum)
                {
                    Getsym();
                }
                if (usedNum >= wordNum) return;
            }
        }

        //转换为pcode中对应的操作码值
        private int GetOperator(LexicalAnalysis.SY x)
        {
            switch (x)
            {
                case LexicalAnalysis.SY.PLUSSY:
                    return 2;
                case LexicalAnalysis.SY.MINUSSY:
                    return 3;
                case LexicalAnalysis.SY.STARSY:
                    return 4;
                case LexicalAnalysis.SY.DIVISY:
                    return 5;
                case LexicalAnalysis.SY.EQUSY:
                    return 8;
                case LexicalAnalysis.SY.UNEQUSY:
                    return 9;
                case LexicalAnalysis.SY.LESSSY:
                    return 10;
                case LexicalAnalysis.SY.GESY:
                    return 11;
                case LexicalAnalysis.SY.GRTSY:
                    return 12;
                case LexicalAnalysis.SY.LESY:
                    return 13;
            }
            return 0;
        }
    }
}
