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
      
        public Form1()
        {
            InitializeComponent();
            richTextBox2.Text = "行数            单词              类别                       值\r\n";
            richTextBox3.Text = "Waiting for your input...\r\n";
        }
       SyntaxAnalysis syn = new SyntaxAnalysis();
        private void skinButton1_Click(object sender, EventArgs e)
        {
            //init();
            richTextBox1.Text = "";
            richTextBox2.Text = "行数            单词              类别                       值\r\n";
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
                    MessageBox.Show("请选择一个有效的文件！");
                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
           
            richTextBox3.Text = "";

            richTextBox2.Text = "行数            单词              类别                       值\r\n";
     
            int lineCount = 1;
            foreach (string nextLine in richTextBox1.Lines)
            {
                syn.GenerateWordlist(nextLine, lineCount);
                showerror(lineCount);
                lineCount++;
                
               
            }
            
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

        //对单词表的输出结果进行格式化
        private string LexOutFormat(string a, string b, string c, string d)
        {
            return string.Format("{0,-15} {1,-15} {2,-24} {3,-15}", a, b, c, d) + "\r\n";
        }
        //输出词法分析结果
        private void LexShow()
        {
            for (int i = 0; i < lex.Wordscnt; i++)
            {
               LexcialAnalysis.Word tmp = lex.Words[i];
                if (tmp.Sym >= 1 && tmp.Sym <= 16)
                {

                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "关键字" + "<" + Enum.GetName(typeof(LexcialAnalysis.SY), tmp.Sym) + ">", tmp.token);
                }

                else if (tmp.Sym >= 17 && tmp.Sym <= 22)
                {

                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "分界符" + "<" + Enum.GetName(typeof(LexcialAnalysis.SY), tmp.Sym) + ">", tmp.token);
                }

                else if (tmp.Sym >= 23 && tmp.Sym <= 33)
                {
                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "运算符" + "<" + Enum.GetName(typeof(LexcialAnalysis.SY), tmp.Sym) + ">", tmp.token);
                }

                else if (tmp.Sym == 34)
                {

                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "常数" + "<" + Enum.GetName(typeof(LexcialAnalysis.SY), tmp.Sym) + ">", Convert.ToString(Int32.Parse(tmp.token), 2) + "<二进制>");
                }
            
                else if (tmp.Sym == 35)
                {
                    richTextBox2.Text += LexOutFormat(tmp.LineID.ToString(), tmp.token, "标识符" + "<" + Enum.GetName(typeof(LexcialAnalysis.SY), tmp.Sym) + ">", tmp.token);
                }

            }
        }
        private void showerror(int lineID)
        {


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
