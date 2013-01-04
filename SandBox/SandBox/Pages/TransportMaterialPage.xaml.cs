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
    /// TransportMaterialPage.xaml 的交互逻辑
    /// </summary>
    public partial class TransportMaterialPage : Page
    {
		TransportMaterialAction transportMaterialAction;

        public TransportMaterialPage()
        {
            InitializeComponent();

			int year = (int)(App.Current as App).action.year;
			int season = (int)(App.Current as App).action.season;
			season = MainAction.ConvertSeason(season);
			transportMaterialAction = new TransportMaterialAction((App.Current as App).accessDB, year, season, (App.Current as App).action.name);
			DataTable transportMaterial = new DataTable();
			transportMaterial = transportMaterialAction.getTransportMaterial();
			transportMaterialsDateGrid.ItemsSource = transportMaterial.DefaultView;
        }

		private void Next_Click(object sender, RoutedEventArgs e)
		{
			transportMaterialAction.transportMaterial();
			(App.Current as App).action.Update();
		}
    }
}
