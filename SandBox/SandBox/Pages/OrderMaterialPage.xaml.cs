using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SandBox.Actions;

namespace SandBox.Pages
{
    /// <summary>
    /// OrderMaterialPage.xaml 的交互逻辑
    /// </summary>
    public partial class OrderMaterialPage : Page
    {
		TransportMaterialAction transportMaterialAction;

        public OrderMaterialPage()
        {
            InitializeComponent();
        }

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			int year = (int)(App.Current as App).action.year;
			int season = (int)(App.Current as App).action.season;
			season = MainAction.ConvertSeason(season);
			transportMaterialAction = new TransportMaterialAction((App.Current as App).accessDB, year, season, (App.Current as App).action.name);
			DataTable newOrders = new DataTable();
			//从程序获得所有数据 newOrders=……
			//order:原材料编号，用户编号，数量，下单年份，下单季度，是否到库
			DataColumn column;
			DataRow row;
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "原材料编号";
			newOrders.Columns.Add(column);
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "用户编号";
			newOrders.Columns.Add(column);
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "数量";
			newOrders.Columns.Add(column);
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "下单年份";
			newOrders.Columns.Add(column);
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "下单季度";
			newOrders.Columns.Add(column);
			//
			column = new DataColumn();
			column.DataType = System.Type.GetType("System.String");
			column.ColumnName = "是否已到库";
			newOrders.Columns.Add(column);

			//for (int i = 0; i <= 4; i++)

			row = newOrders.NewRow();
			row["原材料编号"] = 1;
			row["用户编号"] = transportMaterialAction.getUseNum();
			row["数量"] = 2;
			row["下单年份"] = year;
			row["下单季度"] = season;
			row["是否已到库"] = 0;
			newOrders.Rows.Add(row);

			transportMaterialAction.addMaterialOrders(newOrders);

			(App.Current as App).action.Update();
		}
    }
}
