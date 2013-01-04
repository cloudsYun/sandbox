using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// EnterOrderPage.xaml 的交互逻辑
    /// </summary>
    public partial class EnterOrderPage : Page
    {
		ObservableCollection<Order> orderData;

        public EnterOrderPage()
        {
            InitializeComponent();

			orderData = new ObservableCollection<Order>();
			DataGrid_Order.DataContext = orderData;

        }

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			(App.Current as App).action.Update();
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			if (ComboBox_Market.SelectedItem == null)
			{
				return;
			}

			if (ComboBox_Product.SelectedItem == null)
			{
				return;
			}

			if (ComboBox_Period.SelectedItem == null)
			{
				return;
			}

			if (TextBox_Amount.Text == "")
			{
				return;
			}
			else if (int.Parse(TextBox_Amount.Text) == 0)
			{
				return;
			}

			if (TextBox_Price.Text == "")
			{
				return;
			}
			else if (int.Parse(TextBox_Price.Text) == 0)
			{
				return;
			}

			// Add to the order list
			orderData.Add(new Order() {
				产品类型 = (Product) ComboBox_Product.SelectedItem,
				市场类型 = (Market) ComboBox_Market.SelectedItem,
				订单账期 = (ComboBox_Period.SelectedItem as ComboBoxItem).Content as String,
				订单总价 = int.Parse(TextBox_Price.Text).ToString(),
				订购数量 = int.Parse(TextBox_Amount.Text).ToString()
			});

			// Empty all controls
			ComboBox_Market.SelectedIndex = -1;
			ComboBox_Product.SelectedIndex = -1;
			ComboBox_Period.SelectedIndex = -1;
			TextBox_Amount.Text = "0";
			TextBox_Price.Text = "0";
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			if (DataGrid_Order.SelectedIndex == -1)
			{
				return;
			}
			orderData.RemoveAt(DataGrid_Order.SelectedIndex);
		}

		private void DataGrid_ChangeColumnsWidth(object sender, EventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			foreach (DataGridColumn col in dataGrid.Columns)
			{
				col.Width = (dataGrid.ActualWidth - dataGrid.BorderThickness.Left * 2) / dataGrid.Columns.Count;
			}
		}

		private void TextBox_OnlyDigits(object sender, TextChangedEventArgs e)
		{
			//屏蔽中文输入和非法字符粘贴输入
			TextBox textBox = sender as TextBox;
			TextChange[] change = new TextChange[e.Changes.Count];
			e.Changes.CopyTo(change, 0);

			int offset = change[0].Offset;
			if (change[0].AddedLength > 0)
			{
				int num = 0;
				if (!Int32.TryParse(textBox.Text, out num))
				{
					textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
					textBox.Select(offset, 0);
				}
			}
		}

    }
}
