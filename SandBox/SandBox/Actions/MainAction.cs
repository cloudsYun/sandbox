using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBox.Actions
{
    class MainAction
    {
        protected AccessDB db;
        protected int year;
        protected int season;
        protected string useName;
        protected int useNum;
        public MainAction(AccessDB d,int y,int s,string u  = "stu")
        {
            db = d;
            year = y;
            season = s;
            useName = u;
            useNum = db.GetUserNumber(useName);
        }
        public int getCash()
        {
            int useNum = db.GetUserNumber(useName);
            string s = db.getCash(year, season, useNum);
            return int.Parse(s);
        }
        public bool setCash(string cash)
        {
            int useNum = db.GetUserNumber(useName);
            return db.setCash(cash,year,season,useNum);
        }
        public int[] getTimeNeed(int preYear,int preSeason,int afterSeasons)
        {
            int[] theTime = new int[2];
            int temp = preSeason + afterSeasons;
            theTime[0] = preYear + temp / 4;
            theTime[1] = temp - 4 * (temp / 4);
            return theTime;
        }
        public static int ConvertSeason(int season)
        {
            if (season == 0)
            {
                season = 1;
            }
            if (season == 5)
            {
                season = 4;
            }
            return season;
        }
    }
}
