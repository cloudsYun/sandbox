using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class GatheringAction :MainAction
    {
        public GatheringAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }
        public DataTable getAllGathering()      //获得所有未结清的未收款
        {
            return db.getAllGathering(useNum);
        }
        public bool updataAllGathering()        //结清到账的应收款
        {
            return db.UpdateAllGathering(useNum, year,season);
        }
    }
}
