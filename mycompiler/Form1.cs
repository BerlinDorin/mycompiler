using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
namespace mycompiler
{  

    public partial class Form1 : CCSkinMain
    {
        //SY对应names中的含义
        public enum SY
        {
            DEFAULT, CONSTSY, VARSY, PROCEDURESY, ODDSY, IFSY,
            THENSY, ELSESY, WHILESY, DOSY, CALLSY, BEGINSY,
            ENDSY, REPEATSY, UNTILSY, READSY, WRITESY,
            COMMASY, SEMISY, LPARSY, RPARSY, COLONSY, DOTSY,
            PLUSSY, MINUSSY, STARSY, DIVISY, EQUSY,
            LESSSY, GRTSY, ASSIGNSY, UNEQUSY, LESY, GESY, INTSY, DOUSY, IDSY
        }
        public static String[] names =
                                { "","const", "var", "procedure", "odd", "if",
                                "then", "else", "while", "do", "call", "begin",
                                "end", "repeat", "until", "read", "write",
                                 ",", ";", "(", ")", ":", ".",
                                 "+", "-", "*", "/","=",
                                  "<", ">", ":=", "<>", "<=", ">="
                                };

        public struct Word
        {
            public String token;     //存放单词字符串
                                     // public int Num;           //存放整数值
                                     // public double Dou;        //存储小数值
            public int Sym;            //存储当前字符串编码
            public int LineID;        //存储当前字符串所在行
        }

        private void init()
        {
            words = new Word[16211];
        }

        public int pos;
        //private String str;      //存放源程序字符串
        private char ch;         //当前读取的字符
        public int wordscnt;     
        private Word[] words;

        public Form1()
        {
            InitializeComponent();
            richTextBox3.Text = "Waiting for your input...\r\n";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //添加单词表成员
        private void insert(String s,int sy,int lineID)
        {
            words[wordscnt].token = s;
            words[wordscnt].Sym = sy;
            words[wordscnt].LineID = lineID;
            wordscnt++;
        }
        //Judge实现对token进行判断（无符号整数/浮点数/关键字/标识符） 然后存入单词表
        private void Judge(String s, int lineID)
        {
            int len = s.Length;
            int Num;
            double Dou;
            bool Numflag = false, Douflag = false;
            Numflag = int.TryParse(s, out Num);
            Douflag = double.TryParse(s, out Dou);
           
            if (Numflag == true)        //token为无符号整数
            {
                insert(s, 34, lineID);
                
            }
            else if (Douflag == true)    //token为浮点数
            {
                insert(s, 35, lineID);
            }
            else
            {
                if (s[0] >= '0' && s[0] <= '9')        //如果token以数字开头且后面出现字母，则报错
                {
                     richTextBox3.Text += "Your input \"" + s + "\" is an invalid identifier in Line " + lineID.ToString();
                     richTextBox3.Text += "\r\n";
                }
                else
                {
                    int t = reserver(s);         //调用reserver识别token为关键字或者标识符，返回t作为识别码
                    insert(s, t, lineID);        
                }
            }
        }
        private void LexAnaly(String s,int lineID)
        {
            
            int len = s.Length;
            String tmp = "";
            bool tokenFlag = true;
         
            for (int i = 0; i < len; i++)
            {
                ch = s[i];
                //ch 为 " ", "\n", "\r", "\t":
                if (isSpace() || isNewline() || isTab())        
                {

                    //判断是否读完一个完整token,进入对token的含义分析函数Judge
                    if (!tokenFlag&&tmp != "")
                    {
                        Judge(tmp, lineID);

                        tokenFlag = true;
                        tmp = "";
                    }
                }
                //ch 为 数字或字母： 
                else if (isLetter()|| isDigit())
                {
                    tmp += ch.ToString();
                    tokenFlag = false;
                }
                //ch为",",":","(",")":
                else if (isLpar() || isRpar() || isComma() || isSemi())
                {
                    if (!tokenFlag && tmp != "")
                    {
                        Judge(tmp, lineID);

                        tokenFlag = true;
                        tmp = "";
                    }
                    if (ch == ',') insert(",", 17, lineID);
                    else if (ch == ';') insert(";", 18, lineID);
                    else if (ch == '(') insert("(", 19, lineID);
                    else if (ch == ')') insert(")", 20, lineID);

                }
                //ch 为"." :
                else if (isDot())
                {
                    if(i==len-1||i==0)   //如果是行首或者行尾，排除为浮点数的可能性，先对之前的token进行处理，然后添加入单词表
                    {
                        if (!tokenFlag && tmp != "")
                        {
                            Judge(tmp, lineID);

                            tokenFlag = true;
                            tmp = "";
                        }

                        insert(".",22, lineID);
                    }
                    else if(s[i-1]>='0'&&s[i-1]<='9'&&s[i+1]>='0'&&s[i+1]<='9')
                    {
                        tmp += ".";
                    }
                    else
                    {
                        richTextBox3.Text += "Your input \"" + ch.ToString() + "\" is invalid in Line " + lineID.ToString();
                        richTextBox3.Text += "\r\n";
                    }
                }
                //ch 为 "+" ,"-","*","/" :
                //先对之前的token进行处理，然后识别类别添加入单词表
                else if (isSimpleOperator() != 0)
                {
                    if (!tokenFlag && tmp != "")
                    {
                        Judge(tmp, lineID);

                        tokenFlag = true;
                        tmp = "";
                    }
                    int t = isSimpleOperator();
                    insert(ch.ToString(), t, lineID);
                }
                //ch 为"<",">","<>" :
                //先对之前的token进行处理，然后识别类别（"<",">","<>"）添加入单词表
                else if (isGrt() || isLess())
                {
                        if (!tokenFlag && tmp != "")
                        {
                            Judge(tmp, lineID);

                            tokenFlag = true;
                            tmp = "";
                        }
                        if (i > 0 && s[i - 1] == '<' && s[i] == '>')
                        {
                            insert("<>", 31, lineID);
                        }
                        else if (i == len - 1 || (s[i + 1] != '=' && (s[i] == '>' || (s[i] == '<' && s[i + 1] != '>'))))
                        {
                            if (ch == '<')
                            {
                                insert("<", 28, lineID);
                            }
                            else insert(">", 29, lineID);
                        }

                    
                }
                // ch 为 "=" :
                //先对之前的token进行处理，然后识别类别（":=",">=","<=","="）添加入单词表
                else if (isEqu())
                {
                    if (!tokenFlag && tmp != "")
                    {
                        Judge(tmp, lineID);

                        tokenFlag = true;
                        tmp = "";
                    }
                    if (i > 0 && s[i - 1] == ':')
                    {
                        insert(":=",30, lineID);
                    }
                    else if (i > 0 && (s[i - 1] == '<' || s[i - 1] == '>'))
                    {
                        if (s[i - 1] == '<') insert("<=",32, lineID);
                        if (s[i - 1] == '>') insert(">=",33, lineID);
                    }
                    else
                    {
                        insert("=",27, lineID);
                    }
                }
                //ch =":"
                //根据文法可知，":"与"="需组合使用，先判断若不是则需要提出错误提示
                else if (isColon())
                {
                    if (i < len - 2 && s[i + 1] == '=') { ; }
                    else
                    {
                        richTextBox3.Text += "Your input \"" + ch.ToString() + "\" is should be followed by \"=\" in Line " + lineID.ToString();
                        richTextBox3.Text += "\r\n";
                    }
                }
                else
                {
                     richTextBox3.Text += "Your input \""+ch.ToString()+"\" is invalid in Line "+lineID.ToString();
                     richTextBox3.Text += "\r\n";
                   
                }

            }
            if (!tokenFlag && tmp != "")
            {
                Judge(tmp, lineID);
                tokenFlag = true;
                tmp = "";
            }
           
        }
        //对单词表的输出结果进行格式化
        private string LexOutFormat(string a,string b,string c,string d)
        {
            return string.Format("{0,-15} {1,-15} {2,-24} {3,-15}", a, b, c,d) + "\r\n";
        }
        //输出单词表作为结果
        private void LexShow()
        {
            for(int i = 0; i < wordscnt; i++)
            {
                Word tmp = words[i];
                if (tmp.Sym >= 1 && tmp.Sym <= 16)
                {

                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "关键字" + "<" + Enum.GetName(typeof(SY), tmp.Sym) + ">", tmp.token);
                }

                else if (tmp.Sym >= 17 && tmp.Sym <= 22)
                {

                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "分界符" + "<" + Enum.GetName(typeof(SY), tmp.Sym) + ">", tmp.token);
                }

                else if (tmp.Sym >= 23 && tmp.Sym <= 33)
                {
                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "运算符"+"<"+Enum.GetName(typeof(SY),tmp.Sym)+">", tmp.token);
                }
               
                else if (tmp.Sym == 34)
                {
                    
                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "常数" + "<" + Enum.GetName(typeof(SY), tmp.Sym) + ">", Convert.ToString(Int32.Parse(tmp.token), 2)+"<二进制>");
                }
                else if (tmp.Sym == 35)
                {
                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "浮点数" + "<" + Enum.GetName(typeof(SY), tmp.Sym) + ">", tmp.token);
                }
                else if (tmp.Sym == 36)
                {
                     richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(),tmp.token, "标识符" + "<" + Enum.GetName(typeof(SY), tmp.Sym) + ">", tmp.token);
                }

            }
        }
       

        //是否为空格，换行符，回车符
        private bool isSpace()       
        {
            return ch == ' ';
        }
        private bool isNewline()
        {
            return ch == '\n'||ch=='\r';
        }
        private bool isTab()
        {
            return ch == '\t';
        }
        //是否为数字
        private bool isDigit()
        {
            return ch >= '0' && ch <= '9';
        }
        //是否为字母
        private bool isLetter()
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        }
        //是否为运算符或者界符
        private bool isColon()
        {
            return ch == ':';
        }

        private bool isDot()
        {
            return ch == '.';
        }

        private int isSimpleOperator()
        {
            if (ch == '+') return 23;
            else if (ch == '-') return 24;
            else if (ch == '*') return 25;
            else if (ch == '/') return 26;
            else return 0;
        }

        bool isLpar()
        {
            return ch == '(';
        }

        private bool isRpar()
        {
            return ch == ')';
        }

        private bool isComma()
        {
            return ch == ',';
        }

        private bool isSemi()
        {
            return ch == ';';
        }
        
        private bool isEqu()
        {
            return ch == '=';
        }

        private bool isGrt()
        {
            return ch == '>';
        }

        private bool isLess()
        {
            return ch == '<';
        }
        //是否为关键字，并且返回识别码
        private int reserver(string token)
        {
            for (int i = 1; i <= 16; i++)
            {
                if (token == names[i]) return i;
            }
            return 36;      //不是的话返回的是标识符的识别码
        }



        private void skinButton1_Click(object sender, EventArgs e)
        {
            //init();
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            richTextBox3.Text = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "../../";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;   //默认打开txt文件
            openFileDialog1.RestoreDirectory = true;

            var fileContent = string.Empty;
            var filePath = string.Empty;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                try
                {
                    StreamReader reader = new StreamReader(fileStream);
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        richTextBox1.Text += line;
                        richTextBox1.Text += "\r\n";
                    }
                    reader.Close();
                }
                catch (IOException)
                {
                    MessageBox.Show("请选择一个有效的文件！");
                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            init();
            // richTextBox2.Text = "";
            richTextBox3.Text = "";

            richTextBox2.Text = "行数            单词              类别                       值\r\n";
     
            int lineCount = 1;
            foreach (string nextLine in richTextBox1.Lines)
            {
                LexAnaly(nextLine, lineCount);

                lineCount++;

            }
            LexShow();
            richTextBox3.Text += "mission accomplished!!\r\n";
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".txt"; // Default file extension
            saveFileDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            // Show save file dialog box
            var filePath = string.Empty;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = saveFileDialog.FileName;
                try
                {
                    StreamWriter writer = new StreamWriter(filePath);

                    writer.Write(richTextBox2.Text);
                    writer.Close();

                }
                catch (IOException)
                {
                    MessageBox.Show("保存失败！");
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
