using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace SandBox
{   //初始化参数结构
    public class IniValue
    {
        public IniValue(int UsrNum)
        {
            this.UsrNum=UsrNum;
        }
        public int UsrNum;//用户编号
        public int IniYear = 0;//初始年份值
        public int IniQuarter = 0;//初始月份值
        public DataTable FinancialTargetIniValueCollection;//财务指标季末记录汇总初始值
        public DataTable ProductIniNum;//产品库存初始值
        //！！背景所需数据初始值可以根据财务指标季末记录获得，不用初始化
        public DataTable ProductReaschIniStatus;//产品研发状态初始化,不用显示
        public DataTable FinanceServiceIni;//筹资记录初始化，更改或不该？
        public DataTable ProductLineIniStatus;//生产线信息
        public DataTable MarketQuaIni;//市场初始资格
        //厂房信息？？
        public DataTable UncollectibleAccountsIni;//应收账款信息
        public DataTable RawMaterialNumIni;//原材料信息 

    }
}
