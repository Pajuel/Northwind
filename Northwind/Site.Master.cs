using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;

namespace Northwind
{
    public partial class SiteMaster : MasterPage
    {
        public SiteMapNode CurrentNode;
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? string.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? string.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void foo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            SiteMapNode nodo = e.Item.DataItem as SiteMapNode;
            if (e.Item.DataItem != null)
            {
                e.Item.Visible = false;
            }
            else
            {
                var lista = e.Item.FindControl("list") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (lista != null)
                {
                    if (CurrentNode != null)
                    {
                        lista.Attributes["class"] = nodo == CurrentNode ? "active" : nodo.ChildNodes.Contains(CurrentNode) ? "active open" : CurrentNode.ParentNode != null && nodo.ChildNodes.Contains(CurrentNode.ParentNode) ? "active open" : "";
                    }
                }
                var lbOpcion = e.Item.FindControl("lbOpcion") as HyperLink;
                if (lbOpcion != null)
                {
                    lbOpcion.CssClass = nodo.ChildNodes.Count > 0 ? "dropdown-toggle" : "";
                }
            }
        }

        protected void Repeater_PreRender(object sender, EventArgs e)
        {
            if (((Repeater)sender).Items.Count == 0)
            {
                ((Repeater)sender).Visible = false;
            }
        }
    }

}