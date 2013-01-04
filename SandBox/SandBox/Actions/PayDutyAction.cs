using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.Actions
{
    class PayDutyAction : MainAction
    {
        /*
         * 支付应付税
         * */
        public PayDutyAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {

        }
        private int getDuty()       //获得税款信息
        {
            return int.Parse(db.getDuty());
        }
        public bool payDuty()    //从总金额中减去
        {
            int tax = getDuty();
            int cash = getCash();
            if (tax > cash)
            {
                return false;
            }
            return setCash((cash - tax).ToString());
        }
    }
}
