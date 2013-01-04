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
            int useNum = db.GetUserNumber(useName);
            int cash = int.Parse(db.getCash(0, 0, useNum)); //获得上一季度现金
            return setCash(cash.ToString());
        }
        public bool setNewSeasonCash()
        {
            int previousSeason = 0;
            int previousYear = 0;
            if(season  == 1)
            {
                previousSeason = 4;
                previousYear = year -1;
            }
            else
            {
                previousSeason = season -1;
                previousYear = year;
            }
            int useNum = db.GetUserNumber(useName);
            int cash = int.Parse(db.getCash(previousYear, previousSeason, useNum)); //获得上一季度现金
            return setCash(cash.ToString());
        }
    }
}
