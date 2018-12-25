using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class Interpreter
    {
        public const int stackSize = 1500;
        public const int pcodeSize = 1000;
        public Pcode[] pcodeList;       //pcode指令结果

        public string output;
        public string[] input;
        public int inputptr;
       // public bool inputFlag;
        public bool outputFlag;

        public int[] sta;        //运行栈
        public int currentCode;  //指令寄存器
        public int pc;           //程序地址寄存器
        public int bp;           //基址寄存器
        public int sp;           //栈顶寄存器

        public Interpreter()
        {
            pcodeList = new Pcode[pcodeSize];
            for (int i = 0; i < pcodeSize; i++)
            {
                pcodeList[i] = new Pcode(Pcode.PC.NUL, 0, 0);
            }
            sta = new int[stackSize];
            Array.Clear(sta, 0, stackSize);
           
            currentCode = 0;
            pc = 0;
            bp = 0;
            sp = 0;
        }

        public bool Interpret()
        {
            int runtimeCount=0;
            output = "";
            do
            {
                Pcode currentCode = pcodeList[pc++];

                switch (currentCode.f)
                {
                    case Pcode.PC.LIT:                    //将常量值取到运行栈顶，a是常数值
                        sta[sp++] = currentCode.a;
                        break;
                    case Pcode.PC.OPR:
                        switch (currentCode.a)
                        {
                            case 0:                                                   //OPR 0 0;函数调用结束后的返回
                                sp = bp;
                                pc = sta[sp + 2];
                                bp = sta[sp + 1];
                                break;
                            case 1:                                              //OPR 0 1;栈顶元素取反
                                sta[sp - 1] = -sta[sp - 1];
                                break;
                            case 2:                                                //OPR 0 2;次栈顶与栈顶相加，退两个栈元素，相加值进栈
                                sp--;
                                sta[sp - 1] += sta[sp];
                                break;
                            case 3:                                                 //OPR 0 3;次栈顶减栈顶
                                sp--;
                                sta[sp - 1] -= sta[sp];
                                break;
                            case 4:                                              //OPR 0 4;次栈顶乘以栈顶
                                sp--;
                                sta[sp - 1] = sta[sp - 1] * sta[sp];
                                break;
                            case 5:                                                //OPR 0 5;次栈顶处以栈顶
                                sp--;
                                sta[sp - 1] /= sta[sp];
                                break;
                            case 6:                                                //OPR 0 6;栈顶元素的奇偶判断
                                sta[sp - 1] %= 2; 
                                break;  
                            case 7:                                               //OPR 0 7;次栈顶对栈顶取模，退两个栈元素，余数进栈
                                sp--;
                                sta[sp - 1] %= sta[sp];
                                break;
                            case 8:                                                             //OPR 0 8;==判断相等
                                sp--;
                                sta[sp - 1] = (sta[sp] == sta[sp - 1] ? 1 : 0);
                                break;
                            case 9:                                                                //OPR 0 9;!=判断不相等
                                sp--;
                                sta[sp - 1] = (sta[sp] != sta[sp - 1] ? 1 : 0);
                                break;
                            case 10:                                                               //OPR 0 10;<判断小于
                                sp--;
                                sta[sp - 1] = (sta[sp - 1] < sta[sp] ? 1 : 0);
                                break;
                            case 11:                                                                //OPR 0 11;>=判断大于等于
                                sp--;
                                sta[sp - 1] = (sta[sp - 1] >= sta[sp] ? 1 : 0);
                                break;
                            case 12:                                                                //OPG 0 12;>判断大于
                                sp--;
                                sta[sp - 1] = (sta[sp - 1] > sta[sp] ? 1 : 0);
                                break;
                            case 13:                                                                 //OPG 0 13;<=判断小于等于
                                sp--;
                                sta[sp - 1] = (sta[sp - 1] <= sta[sp] ? 1 : 0);
                                break;
                          
                              
                        }
                        break;
                    case Pcode.PC.LOD:                           //LOD 0 a;将变量值放到栈顶，a是存储空间，l是调用层与说明层的层次差             
                        sta[sp] = sta[Base(currentCode.l, sta, bp) + currentCode.a];
                        sp++;
                        break;
                    case Pcode.PC.STO:                        //STO 0 a; 将栈顶内容送入某变量单元中 ，a是存储空间
                        sp--;
                        sta[Base(currentCode.l, sta, bp) + currentCode.a] = sta[sp];
                        break;
                    case Pcode.PC.CAL:                       //CAL 0 a; 调用过程,a是被调用过程的目标程序入口地址
                        sta[sp] = Base(currentCode.l, sta, bp);
                        sta[sp + 1] = bp;
                        sta[sp + 2] = pc;
                        bp = sp;
                        pc = currentCode.a;
                        break;
                    case Pcode.PC.INT:                    //INT 0 a; 在运行栈中为被调用的过程开辟a个单元的数据区
                        sp += currentCode.a;
                        break;
                    case Pcode.PC.JMP:                   //JMP 0 a; 无条件跳转至a地址
                        pc = currentCode.a;
                        break;
                    case Pcode.PC.JPC:                   //JPC 0 a; 条件跳转，当栈顶布尔值非真则跳转至a地址，否则顺序执行（假转）
                        sp--;
                        if (sta[sp] == 0)
                        {
                            pc = currentCode.a;
                        }
                        break;
                    case Pcode.PC.WRT:                  //WRT 0 a:栈顶值输出至屏幕
                        output += sta[sp - 1].ToString()+ " ";
                        sp--;
                        break;
                    case Pcode.PC.RED:                 //RED 0 a: 读入一个输入置于栈顶
                        if (inputptr >= input.Length) return false;
                        int tmp = int.Parse(input[inputptr++]);
                        sta[sp] = tmp;
                        sta[Base(currentCode.l, sta, bp) + currentCode.a] = sta[sp];
                        break;

                }
                runtimeCount++;
                if (runtimeCount > 1400)
                {
                    StackOverflowException e = new StackOverflowException();
                    throw e;
                }
            } while (pc != 0);

            return true;
        }



        public void AddPcode(Pcode.PC f, int l ,int a)
        {
            pcodeList[currentCode] = new Pcode(f, l, a);
            currentCode++;
        }

        public int Base(int l,int []sta,int bp)
        {
            while (l > 0)
            {
                bp = sta[bp];
                l--;
            }
            return bp;
        }

    }
}
