using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class TransportMaterialAction : MainAction
    {
        public TransportMaterialAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {

        }
        public DataTable getTransportMaterial() //获得所有未到库的订单信息
        {
            return db.getTransportMaterial(useNum);
        }
        public bool transportMaterial()  //完成入库
        {
            DataTable material = new DataTable();
            DataTable materialOrder = new DataTable();
            materialOrder = getTransportMaterial();
            DataTable materialInfo = new DataTable();
            materialInfo= db.getMaterialInfo();
            //处理相关信息
            //资金不够，返回false
            int cash = getCash();
            int money = materialOrder.Rows.Count;
            if (money < cash)
            {
                return false;
            }
			
			// !!!

            for (int i = 0; i < materialOrder.Rows.Count; i++)
            {
                string orderID = materialOrder.Rows[i]["原材料编号"].ToString();     //完成入库
                db.setMaterialGeting(orderID,year,season);
            }
            
            return true;
        }


        public void addMaterialOrders(DataTable newOrders)
        {
            db.addMaterial(newOrders);
        }
    }
}
