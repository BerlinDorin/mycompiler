using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class Errors
    {
        
        public static string[] Errorlist = new string[]
        {
             "",
            "01. 常量定义应是\"=\"而不是\":=\"",
            "02. 常数说明中的\"=\"后应是整数",
            "03. 常数说明中的标识符后应是\"=\"",
            "04. const，var，procedure 后应是标识符",
            "05. 漏掉了\",\"或\";\"",
            "06. 过程说明后的符号不正确（应是语句开始符或过程定义符）",
            "07. 应为语句开始符号",
            "08. 程序体内语句部分后的符号不正确",
            "09. 程序结尾丢了句号\".\"",
            "10. 语句之间漏了\";",
            "11. 标识符未说明",
            "12. 赋值语句中，赋值号左部标识符属性应是变量",
            "13. 赋值语句左部标识符后应是赋值运算符\":=\"",
            "14. call后应为标识符",
            "15. call不能调用常量或者变量",
            "16. 条件语句中缺失了then",
            "17. 丢了end或\";\"",
            "18. while型循环语句中丢了do",
            "19. 语句后的符号不正确",
            "20. 应为关系运算符",
            "21. 表达式中不能有过程标识符",
            "22. 表达式中漏掉右括号\")\"",
            "23. 因子后的非法符号",
            "24. 表达式不能以此符号开始",
            //补充部分
            "25. 不存在的操作符",
            "26. 结束符只能出现在程序结尾",
            "27. 不能在变量声明语句中尝试给变量赋值",
            "28. ",
            "29. ",
            "30. 这个数太大",
            "31. read括号内应该是变量标识符",
            "32. ",
            "33. 缺少until",
            "34. ",
            "35. \":\"需要和\"=\"一起使用给变量赋值",
            "36. ",
            "37. ",
            "38. ",
            "39. ",
            "40. 应为左括号",
            "41. 标识符不能以数字开头",
            "42. 结束符出现在了不适当的地方",
            "43. 输入了无法被识别的符号"
        };
        private string errorMessage;
        private int errorcount;
        public Errors()
        {
            errorMessage = "";
            errorcount = 0;
        }

        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }
        public int Errorcount { get => errorcount; set => errorcount = value; }

        public void AddError(int errorid,int line,string errorword)
        {
            errorcount++;
            errorMessage += errorcount.ToString()+".  Line" + line.ToString() +":  字符 "+errorword+ "\t  错误信息 : " + Errorlist[errorid] + "\r\n";
        }
    }
}
