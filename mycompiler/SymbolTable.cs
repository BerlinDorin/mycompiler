using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mycompiler
{
    class SymbolTable
    {
        public int tablecnt;

        public struct Item
        {
            public String name;             //记录的名字
            public int type;                //记录的类型,有3种类型：constant(0), variable(1), procedure(2)
            public int value;               //记录的值
            public int level;               //记录所在的层
            public int address;             //记录的地址
            public int size;                //需要分配的空间，当记录类型为procedure时使用，默认值为0
        };
        public Item []table;        //符号表

        public SymbolTable()
        {
            tablecnt = 0;
            table = new Item[100];
            
        }

        /*向符号表中添加一条记录*/
        public void tableFill(String name, int type, int value, int level, int dx)
        {   
            Item tmp=new Item();            
            tmp.name = name;
            tmp.type = type;
            tmp.value = value;
            tmp.level = level;
            tmp.address = dx;
            table[tablecnt] = tmp;
            tablecnt++;
        }


        /*查找标识符在符号表中的位置*/
        public int locate(string name)
        {
           
            for (int i = tablecnt - 1; i >= 0; i--)
            {
                if (table[i].name == name)
                    return i;
            }
            return -1;
        }

        public void setAddr(int addr, int pos)
        {
            Item temp = table[pos];
            temp.address = addr;
            table[pos] = temp;
        }




    }
}
