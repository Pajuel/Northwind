using System;
using Business.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Northwind.Productos
{
	public partial class Productos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtName.Text = txtPrice.Text = string.Empty;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var parameters = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(txtName.Text))
                    parameters.Add("ProductName LIKE ?", "%" + txtName.Text + "%");

                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                    parameters.Add("UnitPrice = ? ", Convert.ToDecimal(txtPrice.Text));

                grdProducts.DataSource = Business.Products.ProductBC.GetProducts(parameters);
                grdProducts.DataBind();
            }
            catch (Exception ex)
            {
                WebUtilities.ShowNotify(this, ex, Enums.TypeMessage.Error);
            }
        }

        protected void grdProducts_PreRender(object sender, EventArgs e)
        {
            WebUtilities.MakeAccessible((GridView)sender);
        }
    }
}