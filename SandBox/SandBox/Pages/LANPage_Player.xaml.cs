﻿using System;
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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SandBox.Pages
{
	/// <summary>
	/// LANPage_Player.xaml 的交互逻辑
	/// </summary>
	public partial class LANPage_Player : Page
	{
		public LANPage_Player()
		{
			InitializeComponent();
		}

		private void ConnectServer_Click(object sender, RoutedEventArgs e)
		{
			// if succeeded
			Storyboard myStoryboard = this.FindResource("Background_Picture_Brighter") as Storyboard;
			myStoryboard.Begin();
		}
		
		private void StartGame_Click(object sender, RoutedEventArgs e)
		{

		}

		private void HostAddress_Changed(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			if (TextBox_HostAddress.Text == "")
			{
				TextBox_HostAddress_Bg.Visibility = Visibility.Visible;
			}
			else
			{
				TextBox_HostAddress_Bg.Visibility = Visibility.Hidden;
			}
		}
	}
}
