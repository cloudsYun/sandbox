using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SandBox
{
	/// <summary>
	/// App.xaml 的交互逻辑
	/// </summary>
	public partial class App : Application
	{
		public Actions.AppAction action;
        public AccessDB accessDB;

		public App()
		{ 
			action = new Actions.AppAction();
            accessDB = new AccessDB();
		}
	}

}