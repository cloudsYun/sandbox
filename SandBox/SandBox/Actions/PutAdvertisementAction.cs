using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SandBox.Actions
{
    class PutAdvertisementAction : MainAction
    {
        /*
         * 广告费投放 
         * */
        public PutAdvertisementAction(AccessDB a, int y, int s, string u = "stu")
            : base(a,y,s,u)
        {
            db = a;
        }
        public bool subtracteAdvertisement(string allFee)   //从总金额中减去广告费
        {
            int af = int.Parse(allFee);
            int cash = getCash();
            if (af > cash)
            {
                (App.Current as App).action.WarningBox("投入广告费过多");
                return false;
            }
            cash = cash - af;
            if (setCash(cash.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool recordAdvertisement(string[] advertisementInfo)     //广告费花费如何记录
        {
            return true;
        }
    }
}
