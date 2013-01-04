using System;
using System.Collections.Generic;
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
	/// NewYearPlanningPage.xaml 的交互逻辑
	/// </summary>
	public partial class NewYearPlanningPage : Page
	{
		public NewYearPlanningPage()
		{
			InitializeComponent();
		}

		private void Next_Click(object sender, RoutedEventArgs e)
		{
            int year = (int)(App.Current as App).action.year;
            int season = (int)(App.Current as App).action.season;
            season = MainAction.ConvertSeason(season);
            NewSeasonCountingAction newSeasonCountingAction = new NewSeasonCountingAction((App.Current as App).accessDB, year, season, (App.Current as App).action.name);
            if (year == 1)
            {
                newSeasonCountingAction.setInitSeasonCash();
            }
            else
            {
                newSeasonCountingAction.setNewSeasonCash();
            }

			(App.Current as App).action.Update();
		}

	}
}
