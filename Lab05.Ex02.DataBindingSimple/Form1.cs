﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab05.Ex02.DataBindingSimple
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private BindingSource productsBindingSource;

        private void Form1_Load(object sender, EventArgs e)
        {
            productsTableAdapter1.Fill(northwindDataSet1.Products);
            ProductIDTextBox.DataBindings.Add("Text", productsBindingSource, "ProductID");
            ProductNameTextBox.DataBindings.Add("Text", productsBindingSource, "ProductName");    
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            productsBindingSource.MovePrevious();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            productsBindingSource.MoveNext();

        }
    }
}
