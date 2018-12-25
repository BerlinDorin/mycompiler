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
        private bool isCompile=false; 
        private SyntaxAnalysis syn;
        private bool isInput = false;
        public Form1()
        {
            InitializeComponent();
           
            richTextBox3.Text = "Waiting for your input...\r\n";
        }
      
        private void skinButton1_Click(object sender, EventArgs e)
        {
            //init();
            richTextBox1.Text = "";
            
            richTextBox3.Text = "Waiting....";

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
                    MessageBox.Show("请选择一个有效的文件！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            isCompile = false;
            isInput = false;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {   
            
            syn = new SyntaxAnalysis();
            richTextBox3.Text = "";
            richTextBox2.Text = "";
            richTextBox4.Text = "";
            richTextBox5.Text = "";

            int lineCount = 1;
            foreach (string nextLine in richTextBox1.Lines)
            {
                syn.GenerateWordlist(nextLine, lineCount);           
                lineCount++;     

            }
            syn.FinishWordlist();
            //for (int i= 0;i<syn.wordNum;i++)
            //{
            //    LexicalAnalysis.Word x = syn.wordlist[i];
            //    richTextBox3.Text += x.token +"\t\t"+ x.Sym.ToString() +"\t\t" + x.LineID.ToString()+"\r\n";
            //}
           
             syn.Compile();
            richTextBox2.Text += OutFormat("Num", "f", "l", "a") + "\r\n";
            for (int i = 0; i < syn.ipt.currentCode; i++)
            {
                Pcode  x = syn.ipt.pcodeList[i];
                richTextBox2.Text += OutFormat((i + 1).ToString(), x.f.ToString(), x.l.ToString(), x.a.ToString()) + "\r\n";
            }
            richTextBox3.Text += syn.err.ErrorMessage;
            if (syn.err.Errorcount > 0)
            {
                richTextBox3.Text += "Fails，编译失败！\r\n";
                richTextBox3.Text += "代码段中共产生了" + syn.err.Errorcount.ToString() + "个错误！\r\n";
            }
            else
            {
                richTextBox3.Text += "Congratulations，编译成功！\r\n";
                richTextBox3.Text += "Tips: 如果程序中需要进行输入，需要先输入再解释运行,否则缺失变量将可能导致输出错误结果！\r\n";
            }
           
            isCompile = true;
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
                    MessageBox.Show("保存失败！","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //对Pcode的输出结果进行格式化
        private string OutFormat(string a, string b, string c, string d)
        {
            return string.Format("{0,-6} {1,-8} {2,-8} {3,-8}", a, b, c, d) ;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            richTextBox5.Text = "";
            try
            { 
           
            if(isCompile!=true || syn.err.Errorcount > 0)
            {
                MessageBox.Show("请先完成正确编译！", "ERROR");
                return;
            }
                string str = richTextBox4.Text;
                syn.ipt.input = str.Split(new char[] {' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                // MessageBox.Show(syn.ipt.input[0]+ syn.ipt.input[1]);
                bool flag;
                if (isInput == true)
                {
                    syn.ipt.inputptr = 0;
                    flag = syn.ipt.Interpret();
                    isInput = false;
                }
                syn.ipt.inputptr = 0;
                flag =syn.ipt.Interpret();

                if (flag == true)
                {
                    richTextBox5.Text = syn.ipt.output;
                    richTextBox3.Text = "运行成功!请在输出栏中查看结果";
                }

                else
                {
                    MessageBox.Show("请输入正确数目的变量！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isInput = true;
                }

            }
            catch(StackOverflowException eq)
            {
                MessageBox.Show("RuntimeExceed!程序陷入死循环！", "Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch(Exception ex)
            {             
                MessageBox.Show(ex.ToString(),"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }
    }
}
