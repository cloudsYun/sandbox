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
	/// ManageShortTermLoanPage.xaml 的交互逻辑
    /// </summary>
    public partial class ManageShortTermLoanPage : Page
    {
		ShortTermLoanAction shortTermLoanAction;

		public ManageShortTermLoanPage()
        {
            InitializeComponent();

			int year = (int)(App.Current as App).action.year;
			int season = (int)(App.Current as App).action.season;
			season = MainAction.ConvertSeason(season);
			shortTermLoanAction = new ShortTermLoanAction((App.Current as App).accessDB, year, season, (App.Current as App).action.name);
			DataTable shortTermLoan = new DataTable();
			//shortTermLoan = shortTermLoanAction.getShortTermLoan(); //获得已有的贷款数据
			//处理，显示
			if (season != 1)
			{
				NewSeasonCountingAction newSeasonCountingAction = new NewSeasonCountingAction((App.Current as App).accessDB, year, season, (App.Current as App).action.name);

				newSeasonCountingAction.setNewSeasonCash();
			}
        }

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			(App.Current as App).action.Update();
		}
    }
}
