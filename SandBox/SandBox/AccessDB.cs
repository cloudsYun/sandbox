using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.OleDb;

namespace SandBox
{
    public class AccessDB
    {
        public const string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=CompanyInfo_v3.mdb";


        public OleDbConnection conn;


        public AccessDB()
        {
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //增加新用户,新用户自动编号，
        public bool AddNewUserList(string usr_name, string psw, string ip) 
        {
            string table = "用户列表";
            string sql = "insert into "+table+" (用户名称,用户密码,IP地址) "+" values ('"+usr_name+"','"+psw+"','"+ip+"')";

            Console.WriteLine(sql);
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        //获取用户编号
        public int GetUserNumber(string usr_name)
        {
            string sql = "SELECT 用户编号 FROM 用户列表 WHERE 用户名称='" + usr_name+"'";
            Console.WriteLine(sql);
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);

            if (datatable.Rows.Count == 0)//无该用户
            {
                return 0;
            }
            else//返回用户编号
            {
                int usr_num = int.Parse(datatable.Rows[0][0].ToString());
                return usr_num;
            }
        }

        //读取默认初始值..参数结构如何设计
        public IniValue GetDefaultIniValue(int UsrNum) 
        {
            IniValue IniValueEntity = new IniValue(UsrNum);
            string sql;
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter;
            //读取财务指标季末记录汇总初始值并匹配目标格式
            IniValueEntity.FinancialTargetIniValueCollection = new DataTable();
            IniValueEntity.FinancialTargetIniValueCollection.Columns.Add("财务指标编号");
            IniValueEntity.FinancialTargetIniValueCollection.Columns.Add("年份");
            IniValueEntity.FinancialTargetIniValueCollection.Columns.Add("季度");
            IniValueEntity.FinancialTargetIniValueCollection.Columns.Add("用户编号");
            IniValueEntity.FinancialTargetIniValueCollection.Columns.Add("财务指标值");

            sql = "SELECT 财务指标编号,默认初始值 FROM 财务指标信息汇总";
            adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);
            
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.FinancialTargetIniValueCollection.NewRow();
                row["财务指标编号"] = datatable.Rows[i][0];
                row["年份"] = 0;
                row["季度"] = 0;
                row["用户编号"] = UsrNum;
                row["财务指标值"] = datatable.Rows[i][1];
                IniValueEntity.FinancialTargetIniValueCollection.Rows.Add(row);

            }
            //Console.WriteLine(IniValueEntity.FinancialTargetIniValueCollection.Rows[20][4].ToString());//显示是否读取成功

            //产品库存初始值
            IniValueEntity.ProductIniNum = new DataTable();
            IniValueEntity.ProductIniNum.Columns.Add("用户编号");
            IniValueEntity.ProductIniNum.Columns.Add("产品编号");
            IniValueEntity.ProductIniNum.Columns.Add("产品数量");
            sql = "SELECT 产品编号,初始库存量 FROM 产品信息";
            adapter = new OleDbDataAdapter(sql, conn);
            datatable=new DataTable();
            adapter.Fill(datatable);
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.ProductIniNum.NewRow();
                row["用户编号"] = UsrNum;
                row["产品编号"] = datatable.Rows[i][0];
                row["产品数量"] = datatable.Rows[i][1];
                IniValueEntity.ProductIniNum.Rows.Add(row);
            }
            //产品研发状态初始化,不用显示
            IniValueEntity.ProductReaschIniStatus = new DataTable();
            IniValueEntity.ProductReaschIniStatus.Columns.Add("用户编号");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("产品编号");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("已投资年限");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("是否研发成功");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("投资起始年份");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("投资起始季度");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("投资结束年份");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("投资结束季度");
            IniValueEntity.ProductReaschIniStatus.Columns.Add("是否正在研发");
            for (int i = 0; i < 4; i++)
            {
                DataRow row = IniValueEntity.ProductReaschIniStatus.NewRow();
                if (i == 0)
                {
                    row["用户编号"] = UsrNum;
                    row["产品编号"] = i + 1;
                    row["已投资年限"] = 0;
                    row["是否研发成功"] = 1; //row["是否研发成功"] = true;
                    row["投资起始年份"] = 0;
                    row["投资起始季度"] = 0;
                    row["投资结束年份"] = 0;
                    row["投资结束季度"] = 0;
                    row["是否正在研发"] = 0; //row["是否正在研发"] = false;

                }
                else
                {
                    row["用户编号"] = UsrNum;
                    row["产品编号"] = i + 1;
                    row["已投资年限"] = 0;
                    row["是否研发成功"] = 0; //row["是否研发成功"] = false;
                    row["投资起始年份"] = 0;
                    row["投资起始季度"] = 0;
                    row["投资结束年份"] = 0;
                    row["投资结束季度"] = 0;
                    row["是否正在研发"] = 0; //row["是否正在研发"] = false;
                }
                IniValueEntity.ProductReaschIniStatus.Rows.Add(row);
            }

            //筹资记录初始化，更改或不该？
            IniValueEntity.FinanceServiceIni = new DataTable();
            //IniValueEntity.FinanceServiceIni.Columns.Add("筹资编号");//自动编号
            IniValueEntity.FinanceServiceIni.Columns.Add("筹资类型");
            IniValueEntity.FinanceServiceIni.Columns.Add("用户编号");
            IniValueEntity.FinanceServiceIni.Columns.Add("筹资年份");
            IniValueEntity.FinanceServiceIni.Columns.Add("筹资季度");
            IniValueEntity.FinanceServiceIni.Columns.Add("筹资金额");
            IniValueEntity.FinanceServiceIni.Columns.Add("是否结清");
            IniValueEntity.FinanceServiceIni.Columns.Add("已过周期");
            IniValueEntity.FinanceServiceIni.Columns.Add("还款周期");
            sql = "SELECT 筹资类型,筹资金额,周期 FROM 筹资初始状态表";
            adapter = new OleDbDataAdapter(sql, conn);
            datatable = new DataTable();
            adapter.Fill(datatable);
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.FinanceServiceIni.NewRow();
                //row["筹资编号"] = i+1;
                row["筹资类型"] = datatable.Rows[i][0];
                row["用户编号"] = UsrNum;
                row["筹资年份"] = 1;
                row["筹资季度"] = 1;
                row["筹资金额"] = datatable.Rows[i][1];
                row["是否结清"] = 0;//row["是否结清"] = false;
                row["已过周期"] = 0;
                row["还款周期"] = datatable.Rows[i][2];
                IniValueEntity.FinanceServiceIni.Rows.Add(row);
            }

            //生产线信息
            IniValueEntity.ProductLineIniStatus = new DataTable();
            //IniValueEntity.ProductLineIniStatus.Columns.Add("生产线编号");//自动编号
            IniValueEntity.ProductLineIniStatus.Columns.Add("生产线类型");
            IniValueEntity.ProductLineIniStatus.Columns.Add("厂房编号");
            IniValueEntity.ProductLineIniStatus.Columns.Add("当前产品生产时长");
            IniValueEntity.ProductLineIniStatus.Columns.Add("产品编号");
            IniValueEntity.ProductLineIniStatus.Columns.Add("用户编号");
            IniValueEntity.ProductLineIniStatus.Columns.Add("获取年份");
            IniValueEntity.ProductLineIniStatus.Columns.Add("获取季度");
            IniValueEntity.ProductLineIniStatus.Columns.Add("失去年份");
            IniValueEntity.ProductLineIniStatus.Columns.Add("失去季度");
            IniValueEntity.ProductLineIniStatus.Columns.Add("残值");
            IniValueEntity.ProductLineIniStatus.Columns.Add("已建设周期");
            IniValueEntity.ProductLineIniStatus.Columns.Add("转产完成周期");
            IniValueEntity.ProductLineIniStatus.Columns.Add("转产目标产品编号");
            IniValueEntity.ProductLineIniStatus.Columns.Add("生产线状态");
            IniValueEntity.ProductLineIniStatus.Columns.Add("余值");
            IniValueEntity.ProductLineIniStatus.Columns.Add("建设开始年份");
            IniValueEntity.ProductLineIniStatus.Columns.Add("建设开始季度");
            IniValueEntity.ProductLineIniStatus.Columns.Add("转产开始年份");
            IniValueEntity.ProductLineIniStatus.Columns.Add("转产开始季度");
            sql = "SELECT 生产线编号,生产线类型,厂房编号,当前产品生产时长,产品编号,生产线状态 FROM 生产线初始状态";
            adapter = new OleDbDataAdapter(sql, conn);
            datatable = new DataTable();
            adapter.Fill(datatable);
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.ProductLineIniStatus.NewRow();
                //row["生产线编号"] = datatable.Rows[i][0];
                row["生产线类型"] = datatable.Rows[i][1];
                row["厂房编号"] = datatable.Rows[i][2];
                row["当前产品生产时长"] = datatable.Rows[i][3];
                row["产品编号"] = datatable.Rows[i][4];
                row["用户编号"] = UsrNum;
                row["获取年份"] = 0;
                row["获取季度"] = 0;
                row["失去年份"] = 0;
                row["失去季度"] = 0;
                row["失去年份"] = 0;
                if(i<3)
                {
                    row["残值"]=1;
                }
                else if(i==3)
                {
                    row["残值"]=2;
                }
                else {row["残值"]=0;}
                row["已建设周期"] = 0; 
                row["转产完成周期"] = 0;
                row["转产目标产品编号"] = 0;
                row["生产线状态"] = datatable.Rows[i][5];
                if(i<3)
                {
                    row["余值"]=5;
                }
                else if(i==3)
                {
                    row["余值"]=8;
                }
                else {row["余值"]=0;}
                row["建设开始年份"] = 0;
                row["建设开始季度"] = 0;
                row["转产开始年份"] = 0;
                row["转产开始季度"] = 0;

                IniValueEntity.ProductLineIniStatus.Rows.Add(row);
            }
            //市场初始资格
            IniValueEntity.MarketQuaIni=new DataTable();
            IniValueEntity.MarketQuaIni.Columns.Add("用户编号");
            IniValueEntity.MarketQuaIni.Columns.Add("市场编号");
            IniValueEntity.MarketQuaIni.Columns.Add("已投资年限");
            IniValueEntity.MarketQuaIni.Columns.Add("是否获取准入资格");
            IniValueEntity.MarketQuaIni.Columns.Add("投资起始年份");
            IniValueEntity.MarketQuaIni.Columns.Add("完成投资年份");
            IniValueEntity.MarketQuaIni.Columns.Add("是否正在投资");
            for (int i = 0; i < 5; i++) 
            {
                DataRow row = IniValueEntity.MarketQuaIni.NewRow();
                row["用户编号"] = UsrNum;
                row["市场编号"] = i + 1;
                row["已投资年限"] = 0;
                if (i == 0) row["是否获取准入资格"] = 1;//row["是否获取准入资格"] = true;
                else row["是否获取准入资格"] = 0;//row["是否获取准入资格"] = false;
                row["投资起始年份"] = 0;
                row["完成投资年份"] = 0;
                row["是否正在投资"] = 0; //row["是否正在投资"] = false;
                IniValueEntity.MarketQuaIni.Rows.Add(row);
            }
            //厂房信息？？
            //应收账款信息
            IniValueEntity.UncollectibleAccountsIni = new DataTable();
            //IniValueEntity.UncollectibleAccountsIni.Columns.Add("应收款信息编号");//自动编号
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("用户编号");//
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("财务指标编号");
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("收款周期");
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("已过时间");
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("是否完成收款");
            IniValueEntity.UncollectibleAccountsIni.Columns.Add("应收款金额");
            sql = "SELECT 财务指标编号,收款周期,已过时间,是否完成收款,应收款金额 FROM 应收款初始信息表";
            adapter = new OleDbDataAdapter(sql, conn);
            datatable = new DataTable();
            adapter.Fill(datatable);
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.UncollectibleAccountsIni.NewRow();
                //row["应收款信息编号"] = i+1;
                row["用户编号"] = UsrNum;
                row["财务指标编号"] = datatable.Rows[i][0];
                row["收款周期"] = datatable.Rows[i][1];
                row["已过时间"] = datatable.Rows[i][2];
                row["是否完成收款"] = Convert.ToInt32(datatable.Rows[i][3]); 
                row["应收款金额"] = datatable.Rows[i][4];
                IniValueEntity.UncollectibleAccountsIni.Rows.Add(row);
            }
            //原材料信息 
            IniValueEntity.RawMaterialNumIni = new DataTable();
            IniValueEntity.RawMaterialNumIni.Columns.Add("原材料编号");
            IniValueEntity.RawMaterialNumIni.Columns.Add("用户编号");
            IniValueEntity.RawMaterialNumIni.Columns.Add("原材料数量");
            IniValueEntity.RawMaterialNumIni.Columns.Add("订单数量");
            IniValueEntity.RawMaterialNumIni.Columns.Add("在途数量");
            sql = "SELECT 原材料编号,初始原料数量,初始原料订单 FROM 原材料信息";
            adapter = new OleDbDataAdapter(sql, conn);
            datatable = new DataTable();
            adapter.Fill(datatable);
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                DataRow row = IniValueEntity.RawMaterialNumIni.NewRow();
                row["原材料编号"] = datatable.Rows[i][0];
                row["用户编号"] = UsrNum;
                row["原材料数量"] = datatable.Rows[i][1];
                row["订单数量"] = datatable.Rows[i][2];
                row["在途数量"] = 0;

                IniValueEntity.RawMaterialNumIni.Rows.Add(row);
            }
            return IniValueEntity;
        }
        //保存新初始值
        public bool SetIniValue(int UsrNum, IniValue NewIniValue) 
        { 
            //插入财务指标信息
            InsertIniValue("财务指标季末记录", NewIniValue.FinancialTargetIniValueCollection);
            //插入产品库存初始值
            InsertIniValue("产品库存", NewIniValue.ProductIniNum);
            //插入产品研发状态
            InsertIniValue("产品研发状态", NewIniValue.ProductReaschIniStatus);
            //筹资记录
            InsertIniValue("筹资记录", NewIniValue.FinanceServiceIni);
            //生产线信息
            InsertIniValue("生产线状态", NewIniValue.ProductLineIniStatus);
            //市场初始资格
            InsertIniValue("市场准入状态", NewIniValue.MarketQuaIni);
            //应收账款
            InsertIniValue("应收款信息", NewIniValue.UncollectibleAccountsIni);
            //原材料信息
            InsertIniValue("原材料状态信息", NewIniValue.RawMaterialNumIni);
            return true; 
        }
        public bool InsertIniValue(string TableName, DataTable RecordData)
        {
            string ColumnsNameCombine = " (";;
            
            for (int i = 0; i < RecordData.Columns.Count;i++ )
            {
                if (i == 0)
                {
                    ColumnsNameCombine += RecordData.Columns[i].ColumnName;
                }
                else
                {
                    ColumnsNameCombine += ",";
                    ColumnsNameCombine += RecordData.Columns[i].ColumnName;
                }
              
            }
            ColumnsNameCombine += ") ";
            for (int i = 0; i < RecordData.Rows.Count; i++)
            {
                string ValuesCombine = " values (";
                for (int j = 0; j < RecordData.Columns.Count; j++)//将值转化为字符串
                {
                    if (j == 0)
                    {
                        ValuesCombine += "'";
                        ValuesCombine += RecordData.Rows[i][j].ToString();
                        ValuesCombine += "'";
                    }
                    else
                    {
                        ValuesCombine += ",'";
                        ValuesCombine += RecordData.Rows[i][j].ToString();
                        ValuesCombine += "'";
                    }
                }
                ValuesCombine += ")";
                string sql = "insert into " + TableName + ColumnsNameCombine + ValuesCombine;
                Console.WriteLine(sql);
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            
            return true;
        }

        public bool FinanceTargetCheck(int year, int season, int UsrNum)//year!=0
        {
            string sql = "SELECT 财务指标编号,财务指标值 FROM 财务指标季末记录 WHERE (年份="
                        + year.ToString() + ") AND (季度=" + season.ToString() + ") AND (用户编号=" + UsrNum.ToString() + ")";
            Console.WriteLine(sql);
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);
            //毛利
            datatable.Rows[2][1] = int.Parse(datatable.Rows[1][1].ToString()) - int.Parse(datatable.Rows[0][1].ToString());
            //综合费用
            datatable.Rows[3][1] = int.Parse(datatable.Rows[32][1].ToString()) + int.Parse(datatable.Rows[33][1].ToString()) + int.Parse(datatable.Rows[34][1].ToString()) +
                int.Parse(datatable.Rows[35][1].ToString()) + int.Parse(datatable.Rows[36][1].ToString()) + int.Parse(datatable.Rows[37][1].ToString());
            //折旧前利润
            datatable.Rows[4][1] = int.Parse(datatable.Rows[2][1].ToString()) - int.Parse(datatable.Rows[3][1].ToString());
            //支付利息前利润
            datatable.Rows[6][1] = int.Parse(datatable.Rows[4][1].ToString()) - int.Parse(datatable.Rows[5][1].ToString());
            //税前利润
            datatable.Rows[9][1] = int.Parse(datatable.Rows[6][1].ToString()) - int.Parse(datatable.Rows[7][1].ToString()) - int.Parse(datatable.Rows[8][1].ToString());
            //所得税
            datatable.Rows[10][1] = (int)(double.Parse(datatable.Rows[9][1].ToString()) * 0.33);
            //净利润
            datatable.Rows[11][1] = int.Parse(datatable.Rows[9][1].ToString()) - int.Parse(datatable.Rows[10][1].ToString());
            //应缴税金
            datatable.Rows[22][1] = int.Parse(datatable.Rows[10][1].ToString());
            //利润留存
            if(year==1)
            {
                string sql_new = "SELECT 财务指标编号,财务指标值 FROM 财务指标季末记录 WHERE (年份=0) AND (季度=0) AND (用户编号=" + UsrNum.ToString() + ") AND (财务指标编号=24)";
                DataTable datatable_new = new DataTable();
                OleDbDataAdapter adapter_new = new OleDbDataAdapter(sql_new, conn);
                adapter.Fill(datatable_new);
                datatable.Rows[24][1]=int.Parse(datatable.Rows[11][1].ToString())+int.Parse(datatable_new.Rows[0][1].ToString());
            }
            else
            {
                string sql_new = "SELECT 财务指标编号,财务指标值 FROM 财务指标季末记录 WHERE (年份="
                        + (year-1).ToString() + ") AND (季度=" + 4.ToString() + ") AND (用户编号=" + UsrNum.ToString() + ")"+ ") AND (财务指标编号=24)";
                DataTable datatable_new = new DataTable();
                OleDbDataAdapter adapter_new = new OleDbDataAdapter(sql_new, conn);
                adapter.Fill(datatable_new);
                datatable.Rows[24][1]=int.Parse(datatable.Rows[11][1].ToString())+int.Parse(datatable_new.Rows[0][1].ToString());
            }
            //年度净利
            datatable.Rows[25][1] = int.Parse(datatable.Rows[11][1].ToString());
            //流动资产合计
            datatable.Rows[26][1] = int.Parse(datatable.Rows[12][1].ToString()) + int.Parse(datatable.Rows[13][1].ToString()) + int.Parse(datatable.Rows[14][1].ToString())
                + int.Parse(datatable.Rows[15][1].ToString()) + int.Parse(datatable.Rows[16][1].ToString());
            //固定资产合计
            datatable.Rows[27][1] = int.Parse(datatable.Rows[17][1].ToString()) + int.Parse(datatable.Rows[18][1].ToString()) + int.Parse(datatable.Rows[19][1].ToString());
            //资产总计
            datatable.Rows[28][1] = int.Parse(datatable.Rows[26][1].ToString()) + int.Parse(datatable.Rows[27][1].ToString());
            //负债合计
            datatable.Rows[29][1] =int.Parse(datatable.Rows[20][1].ToString()) + int.Parse(datatable.Rows[21][1].ToString()) + int.Parse(datatable.Rows[22][1].ToString());
            //固所有者权益合计
            datatable.Rows[30][1] =int.Parse(datatable.Rows[23][1].ToString()) + int.Parse(datatable.Rows[24][1].ToString()) + int.Parse(datatable.Rows[25][1].ToString());
            //负债和所有者权益总计
            datatable.Rows[31][1] = int.Parse(datatable.Rows[29][1].ToString()) + int.Parse(datatable.Rows[30][1].ToString());
            //
            return true;
        }
        //***************连云*************************//
        //获取资产信息的函数？？


        //****现金处理****
        public string getCash(int year,int season,int UsrNum) //获取现金
        {
            string sql = "SELECT 财务指标值 FROM 财务指标季末记录 WHERE ((年份=" 
                        + year.ToString() + ") AND (季度=" + season.ToString() + ") AND (用户编号=" + UsrNum.ToString() + ") AND (财务指标编号=13))";
            Console.WriteLine(sql);
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);

            if (datatable.Rows.Count == 0)//无该现金值
            {
                return "-1";
            }
            else//返回现金值
            {
                int cash = int.Parse(datatable.Rows[0][0].ToString());
                return cash.ToString();
            }
        }
        public bool setCash(string cash,int year,int season,int UsrNum)    //设定新的现金值
        {
            int cash_num = int.Parse(cash);
            string sql = "UPDATE 财务指标季末记录 SET 财务指标值="+cash.ToString()+" WHERE (年份=" + year.ToString() + ") AND (季度=" + season.ToString() + ") AND (用户编号="
                + UsrNum.ToString() + ") AND (财务指标编号=13)";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.ExecuteScalar();
            
            return true;
        }



        //***订单处理****


            //老师输入的订单信息
        /*
        public bool recordAdvertiseMentInfo(DataTable newOrders) //记录订单信息
        {
            //!!!!!newOrders的列名有  产品编号，市场编号，数量，账期，售价
            InsertIniValue("产品订单信息", newOrders);
            return true;
        }
         * */
        public DataTable getProductOrders() //返回所有订单数据
        {
            string sql = "select * from 产品订单信息";
            DataTable data = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(data);
            return data;
        }
        public DataTable getProductOrders(string orderID) //返回某个订单数据
        {
            //（完成测试）
            string sql = "select * from 产品订单信息 where 订单编号="+orderID;
            DataTable data = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(data);
            return data;
        }
        public bool addNewOrders(DataTable orders)  //插入新的订单
        {
            //!!!!!orders的列名有  产品编号，市场编号，数量，账期，售价(完成测试)
            try
            {
                InsertIniValue("产品订单信息", orders);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool deleteOrders(string orderID)    //删除编号为orderID的订单
        {
            string sql = "DELETE FROM 产品订单信息 WHERE 订单编号=" + orderID;
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.ExecuteScalar();
            return true;
        }
        
            //某用户订单信息
        public DataTable getAUnDeliveryOrders(string orderId,int UsrNum)   //获得orderID的订单信息
        {
            string sql = "select * from 订单获取完成状态 where (订单编号=" + orderId+") AND (用户编号="+UsrNum.ToString()+")";
            DataTable anOrder = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(anOrder);
            return anOrder;
        }
             
        public DataTable getAllUnDeliveryOrders(int UsrNum)  //获得所有的未完成订单
        {
            string sql = "select * from 订单获取完成状态 where (订单是否完成=false) AND (用户编号="+UsrNum.ToString()+")";
            DataTable anOrder = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(anOrder);
            return anOrder;
        }


        public string getDuty()     //获得税金??
        {

            return "1";
        }


        //贷款信息
        public DataTable getShortTermLoan(int UsrNum) //获得所有的短期贷款信息（包括借款金额、时间、还款期限等）
        {
            return getFinanceServiceInfo(UsrNum, "短期贷款");

        }

        public DataTable getFinanceServiceInfo(int UsrNum, string ServiceKind)//获取某用户某种类型的贷款
        {
            string sql = "SELECT * FROM 筹资记录 WHERE (筹资类型=" + ServiceKind + ") AND (用户编号=" + UsrNum.ToString() + ")";
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);
            return datatable;
        }
        public bool deleteFinanceServiceInfo(int UsrNum, string ServiceCode)//删除某条贷款信息
        //ServiceCode是筹资编号
        {
            string sql = "DELETE FROM 筹资记录 WHERE (筹资编号=" + ServiceCode + ") AND (用户编号=" + UsrNum.ToString() + ")";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.ExecuteScalar();
            return true;
        }
        public bool addFinanciServiceInfo(DataTable ServiceInfo)//添加一条贷款记录
        {//筹资类型，用户编号，筹资年份，筹资季度，筹资金额，是否结清，已过周期，还款周期
            InsertIniValue("筹资记录", ServiceInfo);
            return true;
        }
        public DataTable getTransportMaterial(int UsrNum) //获得所有未到库的原材料订单
        {
            string sql = "SELECT * FROM 原料订单信息 WHERE (用户编号=" + UsrNum.ToString() + ") AND (是否已到库=false)";
            DataTable unGetMaterial = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(unGetMaterial);

            //处理
            return unGetMaterial;
        }
        /*
        public bool deleteShortTermLoan(string shortloanID) //删除一条短期贷款记录
        {
            return true;
        }
        public bool addShortTermLoan(string[] shortloan)   //在数据库中增加一条借款记录
        {
            return true;
        }
        */

        //原材料信息
        public bool addMaterial(DataTable Order)
        {//order:原材料编号，用户编号，数量，下单年份，下单季度，是否到库
            InsertIniValue("原料订单信息", Order);

            //加入订单的同时更新原材料状态信息中对应的 订单数量信息
            string getSql = "SELECT 订单数量 FROM 原材料状态信息 WHERE （原材料编号=" + Order.Rows[0][0].ToString() + ") AND (用户编号=" + Order.Rows[0][1].ToString() +
                ")";
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(getSql, conn);
            adapter.Fill(datatable);
            int OrderNum = int.Parse(datatable.Rows[0][0].ToString()) + int.Parse(Order.Rows[0][2].ToString());
            string setSql = "UPDATE 原材料信息 SET 订单数量=" + OrderNum.ToString() + "WHERE （原材料编号=" + Order.Rows[0][0].ToString() + ") AND (用户编号=" + Order.Rows[0][1].ToString() +
                ")";
            OleDbCommand cmd = new OleDbCommand(setSql, conn);
            cmd.ExecuteScalar();
            return true;
        }
        public bool setMaterialGeting(string ordersID, int year, int season)  //设定对应ID的原材料到库
        {
            string sql = "Select * FROM 原料订单信息 WHERE 原料订单编号=" + ordersID;
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(datatable);
            string GetMaterialCountSql = "SELECT 原材料数量,订单数量,在途数量 FROM 原材料状态信息 WHERE (原材料编号=" + datatable.Rows[0][1].ToString() + ") AND (用户编号="
                + datatable.Rows[2][0].ToString() + ")";
            DataTable temp_datatable = new DataTable();
            OleDbDataAdapter temp_adapter = new OleDbDataAdapter(GetMaterialCountSql, conn);
            int CurrentNum = int.Parse(temp_datatable.Rows[0][0].ToString());
            int OrderNum = int.Parse(temp_datatable.Rows[0][1].ToString());
            int OnTheWayNum = int.Parse(temp_datatable.Rows[0][2].ToString());

            int IncreaseNum = int.Parse(datatable.Rows[3][0].ToString());


            string UpdateMaterial1CountSql = "UPDATE 原料状态信息 SET 原材料数量=" + (CurrentNum + IncreaseNum).ToString()
                + ",订单数量=" + (OrderNum - IncreaseNum).ToString() + "WHERE (原材料编号=" + datatable.Rows[0][1].ToString() + ") AND (用户编号="
                + datatable.Rows[2][0].ToString() + ")";

            string UpdateMaterial2CountSql = "UPDATE 原料状态信息 SET 原材料数量=" + (CurrentNum + IncreaseNum).ToString()
                + ",在途数量=" + (OnTheWayNum - IncreaseNum).ToString() + "WHERE (原材料编号=" + datatable.Rows[0][1].ToString() + ") AND (用户编号="
                + datatable.Rows[2][0].ToString() + ")";

            string set_sql = "UPDATE 原料订单信息 SET 是否到库=true WHERE 原料订单编号=" + ordersID;

            int OrderYear = int.Parse(datatable.Rows[0][4].ToString());
            int OrderSeason = int.Parse(datatable.Rows[0][5].ToString());
            int SeasonNum = (year - OrderYear) * 4 + (season - OrderSeason);
            if (int.Parse(datatable.Rows[0][1].ToString()) <= 2) //
            {
                if (SeasonNum >= 1)
                {
                    if (bool.Parse(datatable.Rows[0][6].ToString()) == false)
                    {
                        OleDbCommand cmd = new OleDbCommand(set_sql, conn);
                        cmd.ExecuteScalar();
                        cmd = new OleDbCommand(UpdateMaterial1CountSql, conn);//更新原料状态信息
                        cmd.ExecuteScalar();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else
            {
                if (SeasonNum >= 2)
                {
                    if (bool.Parse(datatable.Rows[0][6].ToString()) == false)
                    {
                        OleDbCommand cmd = new OleDbCommand(set_sql, conn);
                        cmd.ExecuteScalar();
                        cmd = new OleDbCommand(UpdateMaterial2CountSql, conn);//更新原料状态信息
                        cmd.ExecuteScalar();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }

        }

        public bool UpdateOnWayNum(int UsrNum)//更新在途信息，用于新一季度时，
        {
            string getSql = "SELECT 订单数量,在途数量 FROM 原材料状态信息 WHERE 用户编号=" + UsrNum.ToString();
            DataTable datatable = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(getSql, conn);
            adapter.Fill(datatable);

            datatable.Rows[2][1] = int.Parse(datatable.Rows[2][1].ToString()) + int.Parse(datatable.Rows[2][0].ToString());
            datatable.Rows[2][0] = 0;
            datatable.Rows[3][1] = int.Parse(datatable.Rows[3][1].ToString()) + int.Parse(datatable.Rows[3][0].ToString());
            datatable.Rows[3][0] = 0;

            string Update2Sql = "UPDATE 原材料状态信息 SET 订单数量=" + datatable.Rows[2][0].ToString() + ",在途数量=" + datatable.Rows[2][1]
                + "WHERE 用户编号=" + UsrNum.ToString() + " AND 原材料编号=3";
            OleDbCommand cmd = new OleDbCommand(Update2Sql, conn);
            cmd.ExecuteScalar();

            string Update3Sql = "UPDATE 原材料状态信息 SET 订单数量=" + datatable.Rows[3][0].ToString() + ",在途数量=" + datatable.Rows[3][1]
                + "WHERE 用户编号=" + UsrNum.ToString() + " AND 原材料编号=4";
            cmd = new OleDbCommand(Update3Sql, conn);
            cmd.ExecuteScalar();

            return true;

        }

        public DataTable getMaterialInStore()           //获得所有原材料的库存剩余数量
        {
            DataTable material = new DataTable();
            //处理
            return material;
        }
        public DataTable getMaterialInfo()          //获得原材料的静态信息
        {
            DataTable materialInfo = new DataTable();
            //处理
            return materialInfo;
        }
        public bool setMaterialInStore(string[] material)   //设定四种原材料的库存剩余信息
        {
            return true;
        }
        public bool addNewMaterialOrders(string rawID, string number, string year, string season)   //给出原材料编号和原材料个数
        {
            //用户编号随便设吧回头再说
            return true;
        }

        //厂房信息
        public DataTable getPlantInfomation()       //获得厂房的静态信息
        {
            DataTable plantInfo = new DataTable();
            string sql = "select * from 厂房信息";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(plantInfo);
            return plantInfo;
        }
        public DataTable getAllPlant()
        {
            DataTable allPlant = new DataTable();
            //处理,金钱
            string sql = "select * from 玩家厂房资产";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(allPlant);
            return allPlant;
        }
        public DataTable getAPlant(string plantID)
        {
            DataTable aPlant = new DataTable();
            //处理,金钱
            string sql = "select * from 玩家厂房资产 where 厂房编号 = '" + plantID + "'";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sql, conn);
            adapter.Fill(aPlant);
            return aPlant;
        }

        //生产线信息
        public DataTable getBusyProcessingLine()       //获得所有正在生产的生产线详细信息 
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public bool setProcessingLineState(string processLineID, string state)                //设定生产线状态(ID,当前产品生产时长)
        {
            //产品时长为0表示空闲？
            return true;
        }
        public DataTable getProductionLineInfo()        //获得生产线的静态信息
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public DataTable getAProcessingLineState(string lineID) //获得ID为lineID的生产线状态（仅有一条）
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public bool deleteAProcessingLine(string lineID)       //删除一个ID为lineID的生产线
        {
            return true;
        }
        public DataTable getFreeProductLine()           //获得所有空闲的生产线
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public DataTable getAllProductLine()    //获得所有的生产线列表
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public DataTable getProductTypeInfo(string productType)     //获得对应生产线类型的所有信息数据
        {
            DataTable temp = new DataTable();

            return temp;
        }
       
        //产品库存信息
        public DataTable getProductInStore()        //获得所有的库存信息
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public bool setProductInStore(string[] productInStore)  //输入为每种产品(共四种)的新库存数值。给对应库存值设定为新数
        {
            return true;
        }

        
        //应收款信息
        public DataTable getAllGathering()       //获得所有未结清的未收款
        {
            DataTable temp = new DataTable();

            return temp;
        }
        public bool setGathering(string gatheringID)    //将对应的ID的应收款项设定为结清
        {
            return true;
        }

        public bool setProductLineState(string productlineID, string productId, string producingTime) //开始下一批生产后，设定新的生产线状态
        {
            return true;
        }

        

        //产品信息
        public bool setProductInStore(string type, string newNumber)    //设定库存中type类型产品的新数量
        {
            return true;
        }
        public DataTable getDevelopProductInfo()        //获得产品研发状态
        {
            DataTable developProductInfo = new DataTable();

            return developProductInfo;
        }
        public DataTable getProductInfo()               //获得产品信息
        {
            DataTable productInfo = new DataTable();

            return productInfo;
        }

        //市场准入状态
        public DataTable getMarketAccessInfo()      //查询市场准入状态表
        {
            DataTable accessInfo = new DataTable();

            return accessInfo;
        }
        public DataTable getMarketInfo()            //获得市场信息
        {
            DataTable marketInfo = new DataTable();

            return marketInfo;
        }
        public bool setMarketAccessInfo(string marketID, string marketTime, string productName, string year)    //设定新的市场状态，开始新的市场开辟
        {
            //市场类型、开拓所需时间、产品名称、开始年份
            return true;
        }
        public bool setMarketAccessState(string marketID)
        {
            //更新已经投资的时间
            return true;
        }

        //=================1-4晚饭后添加的函数============


        internal DataTable getLongTermLoan()                        //获得长期贷款
        {
            throw new NotImplementedException();
        }

        internal bool deleteLongTermLoan(string p)                  //删除一条长期贷款记录
        {
            throw new NotImplementedException();
        }

        internal bool addLongTermLoan(string[] longLoanInfo)        //在数据库中增加一条借款记录
        {
            throw new NotImplementedException();
        }


        internal DataTable getAllUnDeliveryOrders()
        {
            throw new NotImplementedException();
        }
    }
}
