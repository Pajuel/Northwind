using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Business.Utilities
{
    public class WebUtilities
    {
        public static void MakeAccessible(GridView grid)
        {
            if (grid.Rows.Count > 0)
            {
                //Esto reemplaza <td> con <th> y añade el atributo scope
                grid.UseAccessibleHeader = true;

                //Esto agregará los elementos <thead> y <tbody> 
                grid.HeaderRow.TableSection = TableRowSection.TableHeader;

                //Esto agrega el elemento <tfoot>. Lo elimina si no tiene una fila de pie de página
                grid.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        public static string GetTypeString(Enums.TypeMessage typeMessage)
        {            
            switch (typeMessage)
            {
                case Enums.TypeMessage.Notice:
                    return "info";
                case Enums.TypeMessage.Success:
                    return "success";
                case Enums.TypeMessage.Warning:
                    return "warning";
                default:
                    return "danger";
            }
        }
        public static string GetIconString(Enums.TypeMessage typeMessage)
        {
            switch (typeMessage)
            {
                case Enums.TypeMessage.Notice:
                    return "glyphicon glyphicon-info-sign";
                case Enums.TypeMessage.Success:
                    return "glyphicon glyphicon-ok";
                case Enums.TypeMessage.Warning:
                    return "glyphicon glyphicon-alert";
                default:
                    return "glyphicon-exclamation-sign";
            }
        }
        public static void ShowNotify(Control control, Exception ex, Enums.TypeMessage strType, string strTitle = "")
        {
            ShowNotify(control, "Ocurrio un error: " + ex.Message, strType, strTitle);
        }

        public static void ShowNotify(Control control, string strMessage, Enums.TypeMessage strType, string strTitle = "")
        {
            try
            {
                string strPageTitle = control.Page.Title;
                if (string.IsNullOrEmpty(strPageTitle))
                {
                    var title = control.Page.Master?.FindControl("TitleApp") as System.Web.UI.HtmlControls.HtmlTitle;
                    if (title != null)
                    {
                        strPageTitle = title.Text.Trim();
                    }
                    else
                    {
                        strPageTitle = "Mensaje";
                    }
                }
                
                ScriptManager.RegisterStartupScript(control, control.GetType(), "Message",
                    "$.notify({" +
                    "icon: '" + GetIconString(strType) + "'," +
                    "title: '<strong>" + HttpUtility.JavaScriptStringEncode((string.IsNullOrEmpty(strTitle) ? SiteMap.CurrentNode != null ? SiteMap.CurrentNode.Title : strPageTitle : strTitle)) + ":</strong>'," +
                    "message: '" + HttpUtility.JavaScriptStringEncode(strMessage.Trim()) + "'" +
                    "},{" +
                    "type: '" + GetTypeString(strType) + "'" +
                    "});"
                    , true);
            }
            catch { }
        }
    }
}
