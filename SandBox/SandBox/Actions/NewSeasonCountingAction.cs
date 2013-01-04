using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.Actions
{
    class NewSeasonCountingAction:MainAction
    {
        /*
         * 每个季度初都要调用setNewSeasonCash函数确定新季度的现金初始值         * 
         * */
        public NewSeasonCountingAction(AccessDB d, int y, int s, string u = "stu")
            : base(d, y, s, u)
        {
        }
        public bool setInitSeasonCash()
        {
            return db.SetNewSeasonFinanceTarget(1,1,useNum);
        }
        public bool setNewSeasonCash()
        {
            return db.SetNewSeasonFinanceTarget(year,season,useNum);
        }
    }
}
