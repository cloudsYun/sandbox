using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class DevelopProductAction :MainAction
    {
        public DevelopProductAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }
        public DataTable getDevelopProductInfo()    //获得产品研发状态
        {
            return db.getDevelopProductInfo(useNum);
        }
        public DataTable getProductInfo()   //获得产品信息
        {
            return db.getProductInfo();
        }
        public bool startANewDevelop(string type)
        {
            //判断没有研发成功
            //设定为在研
            //设定研发时间
            return true;
        }
        public bool updateDevelopState()
        {
            //已投资年限加1
            //如果投资年限达到产品对应的时间，则设定为研发成功
            return true;
        }
    }
}
