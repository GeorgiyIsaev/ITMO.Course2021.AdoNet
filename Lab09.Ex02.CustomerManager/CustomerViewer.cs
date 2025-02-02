﻿using Lab09.Ex01.CodeFirstDLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab09.Ex01.CustomerManager
{
    public partial class CustomerViewer : Form
    {
        public CustomerViewer()
        {
            InitializeComponent();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SampleContext>());
        }
        SampleContext context;
        byte[] Ph;

        private void buttonFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                Image bm = new Bitmap(diag.OpenFile());

                ImageConverter converter = new ImageConverter();
                Ph = (byte[])converter.ConvertTo(bm, typeof(byte[]));
            }

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
      
                Customer customer = new Customer
                {
                    FirstName = this.textBoxfirstname.Text.ToString(),
                    LastName = this.textBoxlastname.Text.ToString(),
                    Email = this.textBoxmail.Text.ToString(),
                    Age = Int32.Parse(this.textBoxage.Text.ToString()),
                    Photo = Ph,
                    Orders = orderlistBox.SelectedItems.OfType<Order>().ToList()
                };
                context.Customers.Add(customer);
                context.SaveChanges();
                textBoxfirstname.Text = String.Empty;
                textBoxlastname.Text = String.Empty;
                textBoxmail.Text = String.Empty;
                textBoxage.Text = String.Empty;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.ToString());
            }
        }

        private void Output()
        {
            if (this.CustomerradioButton.Checked == true)
                GridView.DataSource = context.Customers.ToList();
            else if (this.OrderradioButton.Checked == true)
                GridView.DataSource = context.Orders.ToList();
            else if (this.ViporderradioButton.Checked == true)
                GridView.DataSource = context.VipOrders.ToList();
        }

        private void buttonOut_Click(object sender, EventArgs e)
        {
            var query = from b in context.Customers
                        orderby b.FirstName
                        select b;
            customerList.DataSource = query.ToList();
            Output();
        }

        private void CustomerViewer_Load(object sender, EventArgs e)
        {
            context = new SampleContext();
            context.Orders.Add(new Order { ProductName = "Аудио", Quantity = 12, PurchaseDate = DateTime.Parse("12.01.2016") });
            context.Orders.Add(new Order { ProductName = "Видео", Quantity = 22, PurchaseDate = DateTime.Parse("10.01.2016") });
            context.VipOrders.Add(new VipOrder { ProductName = "Авто", Quantity = 101, PurchaseDate = DateTime.Parse("10.01.2016"), status = "Высокий" });
            context.SaveChanges();
            orderlistBox.DataSource = context.Orders.ToList();
        }

        private void GridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GridView.CurrentRow == null) return;
            var customer = GridView.CurrentRow.DataBoundItem as Customer;
            if (customer == null) return;
            labelid.Text = Convert.ToString(customer.CustomerId);
            textBoxCustomer.Text = customer.ToString();

            textBoxfirstname.Text = customer.FirstName;
            textBoxlastname.Text = customer.LastName;
            textBoxmail.Text = customer.Email;
            textBoxage.Text = Convert.ToString(customer.Age);

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (labelid.Text == String.Empty) return;

            var id = Convert.ToInt32(labelid.Text);
            var customer = context.Customers.Find(id);
            if (customer == null) return;

            customer.FirstName = this.textBoxfirstname.Text;
            customer.LastName = this.textBoxlastname.Text;
            customer.Email = this.textBoxmail.Text;
            customer.Age = Int32.Parse(this.textBoxage.Text);

            context.Entry(customer).State = EntityState.Modified;

            context.SaveChanges();
            Output();

        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int id;
            if(Int32.TryParse(labelid.Text, out id))
            {
                var customer = context.Customers.Find(id);

                context.Entry(customer).State = EntityState.Deleted;
                context.SaveChanges();
                Output();
                labelid.Text = "Id";
            }
            else
            {
                MessageBox.Show("Не выран объект для удаления!");
            }
        }
    }
}
