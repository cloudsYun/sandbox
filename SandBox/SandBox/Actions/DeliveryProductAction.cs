using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class DeliveryProductAction :MainAction
    {
        public DeliveryProductAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }
        public DataTable getAllUnDeliveryOrders()
        {
            return db.getAllUnDeliveryOrders(useNum);
        }
        public DataTable getProductInStore()
        {
            return db.getProductInStore(useNum);
        }
        public bool deliveryOrders(string OrderID)
        {
            DataTable anOrder = new DataTable();
            DataTable productInStore = new DataTable();
            productInStore = getProductInStore();
            string orderType = anOrder.Rows[0]["产品编号"].ToString();
            string orderNumber = anOrder.Rows[0]["数量"].ToString();
            /*
            for (int i = 0; i < productInStore.Columns.Count; i++)
            {
                string type = productInStore.Columns["产品编号"][i].ToString();
                int numberInStore = int.Parse(productInStore.Columns["产品数量"][i].ToString());
                if (type == orderType && int.Parse(orderNumber) < numberInStore)
                {
                    db.setProductInStore(type, (numberInStore - int.Parse(orderNumber)).ToString());
                }
            }
             * */
            return true;
        }
    }
}
