﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class _Default : System.Web.UI.Page 
{
    private DataTable menu;
    private DataTable order;
    private String selected_Order = string.Empty;

    public _Default()
    {
        this.menu = this.CreateMenu();
        this.order = this.CreateOrderSchema();
        //InitializeOrder();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            this.selected_Order = ddlMenu.Text;
        }
        this.set_Events();
        this.menu = this.CreateMenu();
        this.PopulateDropdown(ddlMenu);
        this.CreateOrderSchema();
        this.GridView1.DataSource = this.order;
        this.GridView1.DataBind();
    }

    private void set_Events()
    {
        this.btnAdd.Click += btnAdd_Click;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtQty.Text.Trim() == String.Empty)
        {
            Response.Write(ResponseError("Quantity is required."));
        }
        else
        {
            try
            {
                this.order = GetOrders(ref Orders);
            }
            catch (Exception ex)
            {
                Response.Write( ResponseError( ex.Message));
                //Response.Write(ex.StackTrace);
            }
        }
        GridView1.DataSource = this.order;
        GridView1.DataBind();
    }

    private string ResponseError(string message)
    {
        return "<p style='color:red;'>" + message + "</p>";
    }

    //private void InitializeOrder()
    //{
    //    this.order.Columns.Add("Product Name", Type.GetType("System.String"));
    //    this.order.Columns.Add("Price", Type.GetType("System.Double"));
    //    this.order.Columns.Add("Qty", Type.GetType("System.Int32"));
    //    this.order.Columns.Add("Amount", Type.GetType("System.Double"));
    //}

    private DataTable CreateMenu()
    {
        DataTable tbl = new DataTable();
        DataColumn[] key = new DataColumn[1];
        key[0] = tbl.Columns.Add("id", typeof(Int32));
        tbl.Columns.Add("name", typeof(String));
        tbl.Columns.Add("price", typeof(Double));

        tbl.PrimaryKey = key;

        tbl.Rows.Add(1,"Blueberry Cheesecake", 125.00D);
        tbl.Rows.Add(2,"Bottled Water", 20.00D);
        tbl.Rows.Add(3,"Chocolate Cake", 95.00D);
        tbl.Rows.Add(4,"Hot Coffee", 30.00D);
        tbl.Rows.Add(5,"Lemonade", 45.00D);
        return tbl;
    }

    private DataTable CreateOrderSchema()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.AddRange(new DataColumn[]{
            new DataColumn("Product",typeof(String))
            ,new DataColumn("Quantity",typeof(Int32))
            ,new DataColumn("Amount",typeof(Double))
            ,new DataColumn("Delete",typeof(CheckBoxField))
        });
        return tbl;
    }

    private DataTable GetOrders(ref HiddenField hidOrders)
    {
        DataTable tbl = this.CreateOrderSchema();

        var json_orders = (ArrayList)JSON.JsonDecode(hidOrders.Value);
        Int32 qty;
        if (Int32.TryParse(txtQty.Text.Trim(), out qty) && qty>0)
        {
            var e = from Hashtable o in json_orders
                    where o["productid"].ToString() == this.selected_Order
                    select o;
            var existing_order = new ArrayList(e.ToArray());
            if (existing_order.Count > 0)
            {
                try
                {
                    json_orders.Remove((Hashtable)existing_order[0]);
                    ((Hashtable)existing_order[0])["quantity"] = Int32.Parse(((Hashtable)existing_order[0])["quantity"].ToString()) + qty;
                    json_orders.Add(((Hashtable)existing_order[0]));
                }
                catch { throw; }
            }
            else
            {
                Hashtable new_order = new Hashtable();
                new_order["productid"] = this.selected_Order;
                new_order["quantity"] = qty;
                json_orders.Add(new_order);
            }
            foreach (System.Collections.Hashtable order in json_orders)
            {
                DataRow menurow = this.menu.Rows.Find(order["productid"].ToString());
                tbl.Rows.Add(
                    menurow["name"].ToString(),
                    order["quantity"],
                    ((double)menurow["price"]) * Double.Parse(order["quantity"].ToString()),
                    new CheckBoxField() { Visible = true, DataField="Delete" });
            }
        }
        else
        {
            throw new Exception("Quantity must be a positive whole number");
        }
        hidOrders.Value = JSON.JsonEncode(json_orders);
        return tbl;
    }

    private void PopulateDropdown(DropDownList ddl)
    {
        ddl.DataSource = this.menu;
        ddl.DataValueField = "id";
        ddl.DataTextField = "name";
        ddl.DataBind();
        if (this.selected_Order!=string.Empty)
        {
            ddl.SelectedIndex = Int32.Parse(this.selected_Order) - 1;
        }
    }




}