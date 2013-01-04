using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBox;
using System.Data;

namespace SandBox.Actions
{
    class EnterOrderAction : MainAction
    {
        /*
         * 登记订单
         * */
        public EnterOrderAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }

        public DataTable getAllOrders()     //从数据库中获取所有的老师订单
        {
            DataTable data = new DataTable();
            data = db.getProductOrders();
            return data;
        }
        public bool addNewOrders(DataTable orders)   //增加新订单
        {
            /*
             * 添加订单：
             *数据库：  产品编号，市场编号，数量，账期，售价
             *界面输入：产品类型，市场类型，数量，价格、账期
             * */
            //增加处理
            return db.addNewOrders(orders);
        }
        public bool deleteOrders(string orderID)    //删除一条订单记录
        {
            return db.deleteOrders(orderID);
        }
    }
}
