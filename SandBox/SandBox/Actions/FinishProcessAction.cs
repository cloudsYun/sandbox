using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class FinishProcessAction : MainAction
    {
        public FinishProcessAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }
        public DataTable getProcessingLineState()  //获取所有正在生产的生产线信息
        {
            DataTable allProcessLine = new DataTable();
            allProcessLine = db.getBusyProcessingLine(useNum);
            return allProcessLine;
        }
        public bool updateProcess()
        {
            DataTable allProcessLine = new DataTable();
            allProcessLine = getProcessingLineState();
            //检查所有生产线的状态，完成生产的加入库存，没有完成生产的，更新状态
            DataTable productInStore = db.getProductInStore(useNum);  //获得库存
            //处理库存
            string [] newProductInStore = {"1","1","1","1"};
            db.setProductInStore(newProductInStore);       //更新库存
            string processLineID = "1";
            string state = "1";
            db.setProcessingLineState(processLineID, state);    //更新生产线生产状态


            return true;
        }
    }
}
