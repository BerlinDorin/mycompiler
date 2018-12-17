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
            "07. 声明顺序有误，应为[<变量说明部分>][<常量说明部分>] [<过程说明部分>]<语句>",
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
            "26. 变量定义重复",
            "27. 未找到对应过程名",
            "28. 不支持过程的判断",
            "29. 缺少标识符，无法进行条件判断",
            "30. 这个数太大",
            "31. read括号内应该是变量标识符",
            "32. 此处不应该出现过程说明标识符",
            "33. 缺少until",
            "34. 此处应该为标识符",
            "35. 常量说明部分出现在错误位置",
            "36. 常量说明部分多于一个",
            "37. ",
            "38. ",
            "39. ",
            "40. 应为左括号"
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

        public void AddError(int line,int errorid)
        {
            errorMessage += "Line:" + line + "错误信息: " + Errorlist[errorid] + "\r\n";
            errorcount++;
        }
    }
}
