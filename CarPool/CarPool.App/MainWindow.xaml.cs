﻿using CarPool.App.ViewModels;
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
using System.Windows.Shapes;

namespace CarPool.App { 

    public partial class MainWindow
    {
        public MainWindow(MainViewModel mainViewModel)
        { 
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
