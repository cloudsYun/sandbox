﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SandBox.Actions
{
    class ShortTermLoanAction : MainAction
    {
        public ShortTermLoanAction(AccessDB d, int y, int s, string u = "stu")
            : base(d,y,s,u)
        {

        }
        public DataTable getShortTermLoan()
        {
            return db.getFinanceServiceInfo(useNum, "短期贷款");
        }
        public int updateShortTermLoan()
        {
            //返回 0：资金约不够 1：读写数据库失败 2：成功更新
            DataTable shortTermLoan = new DataTable();
            int cash = getCash();
            shortTermLoan = getShortTermLoan();
            //如果借款时间加还款期限与目前时间相等，在总金额中减去（不足够则破产），并在数据库中删除记录
            for (int i = 0;i<shortTermLoan.Rows.Count;i++)
            {
                int loanYear = int.Parse(shortTermLoan.Rows[i]["筹资年份"].ToString()); //获得借款时间
                int loanSeason = int.Parse(shortTermLoan.Rows[i]["筹资季度"].ToString());//获得借款记录
                int afterSeasons = int.Parse(shortTermLoan.Rows[i]["还款周期"].ToString());//获得借款时长
                int[] time= getTimeNeed(loanYear,loanSeason,afterSeasons);
                if(year == time[0] && season == time[1])        //期限与目前时间是一致的
                {
                    int money = int.Parse(shortTermLoan.Rows[i]["筹资金额"].ToString());
                    if (cash < money)
                    {
                        return 0;               //资金余额少
                    }
                    setCash((cash - money).ToString());
                    string loanID = shortTermLoan.Rows[i]["筹资编号"].ToString();
                    if (!db.deleteFinanceServiceInfo(useNum,loanID))    
                    {
                        return 1;
                    }
                }
            }
            return 2;
        }
        public bool addNewShortTermLoan(string shortFee) //申请短期贷款
        {
            //筹资类型，用户编号，筹资年份，筹资季度，筹资金额，是否结清，已过周期，还款周期
            DataTable ServiceInfo = new DataTable();
            DataColumn column;
            DataRow row;
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "筹资类型";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "用户编号";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "筹资年份";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "筹资季度";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "筹资金额";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "是否结清";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "已过周期";
            ServiceInfo.Columns.Add(column);
            //
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "还款周期";
            ServiceInfo.Columns.Add(column);
            
            // !!!
           
                row = ServiceInfo.NewRow();
                row["筹资类型"] = "短期贷款";
                row["用户编号"] = useNum;
                row["筹资年份"] = year;
                row["筹资季度"] = season;
                row["筹资金额"] = shortFee;
                row["是否结清"] = 0;
                row["已过周期"] = 0;
                row["还款周期"] = 4;
                ServiceInfo.Rows.Add(row);




                if (db.addFinanciServiceInfo(ServiceInfo))
                {
                    int cash = getCash();
                    int remainCash = (cash + int.Parse(shortFee));
                    (App.Current as App).action.WarningBox(remainCash.ToString());
                    return setCash(remainCash.ToString());
                }
                return false;
        }

    }
}
