﻿using HostelProject.mvvm.viewmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HostelProject.mvvm.view
{
    /// <summary>
    /// Логика взаимодействия для SettingYearPage.xaml
    /// </summary>
    public partial class SettingYearPage : Page
    {
        public SettingYearPage(MainVM mainVM)
        {
            InitializeComponent();
            var vm = ((SettingYearPageVM)DataContext);
            vm.SetMainVM(mainVM);
        }
    }
}
