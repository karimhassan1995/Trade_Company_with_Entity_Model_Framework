﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Expired_Items : Form
    {
        public Expired_Items()
        {
            InitializeComponent();
        }
        Model1 model = new Model1();
        private void Expired_Items_Load(object sender, EventArgs e)
        {
            foreach (Stock i in model.Stocks)
            {
                comboBox1.Items.Add(i.Stock_Id);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" )
            {
                int STOCK_ID = int.Parse(comboBox1.Text);
                DateTime date = DateTime.Now.Date;
                var report = (from sp in model.Supply_permission
                              join s in model.Stocks
                              on sp.Stock_Id equals s.Stock_Id
                              join si in model.Stock_Item
                              on new { p1 = sp.Stock_Id, p2 = sp.Item_Id } equals new { p1 = si.Stock_id, p2 = si.Item_id }
                              join i in model.Items
                              on si.Item_id equals i.Item_Id
                              where DbFunctions.DiffDays(date ,sp.Expiray_Date) <= 120 && si.Stock_id == STOCK_ID
                              select new
                              {
                                  Stock_ID = si.Stock_id,
                                  Stock_Name = s.Stock_Name,
                                  item_ID = si.Item_id,
                                  Item_Name = i.Item_Name,
                                  quantity = si.Quantity,
                                  production_Date = sp.production_Date,
                                  Expiary_Date = sp.Expiray_Date,
                                  Remains_days_to_expire = DbFunctions.DiffDays(date ,sp.Expiray_Date)
                              });
                listBox1.Items.Clear();
                foreach (var stoc in report)
                {
                    listBox1.Items.Add("Item with id " + stoc.item_ID + " and name " + stoc.Item_Name + " in Stock number  " + stoc.Stock_ID + " and name " + stoc.Stock_Name + " with quantaty " + stoc.quantity + " and production_date " + stoc.production_Date );
                    if (stoc.Remains_days_to_expire > 0)
                    {
                        listBox1.Items.Add(" and expiary_date " + stoc.Expiary_Date + " and number of days that remains to expire = " + stoc.Remains_days_to_expire);
                    }
                    else if(stoc.Remains_days_to_expire <0) { listBox1.Items.Add(" and the item expired"); }
                   
                }
            }
            else { MessageBox.Show("Plz enter the id"); }
        }
    }
}
