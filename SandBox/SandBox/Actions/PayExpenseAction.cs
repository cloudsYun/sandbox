using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.Actions
{
    class PayExpenseAction : MainAction
    {
        public PayExpenseAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {
        }
        public bool submitExpense()      //支付行政管理费用
        {
            int cash = getCash();
            //获得行政管理费用
            int manageMoney = 1;
            return (setCash((cash - manageMoney).ToString()));
        }
    }
}
