using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Xml;

namespace SCM2SCALE
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        private Double MemberBalance = 1000.00;


        //收银员登录
        [WebMethod(Description = "方法：收银员登录。参数：收银员账号，收银员密码，电子秤编号。返回：身份验证结果。", EnableSession = true)]
        public Boolean CashierLogin(String CashierId, String CashierPw, String ScaleNo)
        {
            Session["CashierLoginState"] = true;
            return (bool)Session["CashierLoginState"];
        }

        //获得电子秤热键
        [WebMethod(Description = "方法：获得电子秤热键。必须实现收银员登录。每个商品键的定义包含3个要素：键位、层数、PLUNO。", EnableSession = true)]
        public XmlDocument getHotKeys()
        {
            if (Session["CashierLoginState"] == null || Session["CashierLoginState"].Equals(false))
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                XmlElement root = doc.CreateElement("root");
                doc.AppendChild(root);
                return doc;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(dec);
                XmlElement root = doc.CreateElement("root");
                doc.AppendChild(root);
                

                for (int i = 1; i < 10; i++)
                {
                    XmlNode node = doc.CreateElement("hotkey");
                    XmlElement element1 = doc.CreateElement("key");
                    element1.InnerXml = "key" + i;
                    node.AppendChild(element1);
                    XmlElement element2 = doc.CreateElement("level");
                    element2.InnerXml = "level" + i;
                    node.AppendChild(element2);
                    XmlElement element3 = doc.CreateElement("pluno");
                    element3.InnerXml = "pluno" + i;
                    node.AppendChild(element3);

                    root.AppendChild(node);

                }



               
                return doc;
            }
        }

        //会员登录
        [WebMethod(Description = "方法：会员登录。参数：会员卡号，会员卡密码，电子秤编号。返回：身份验证结果。", EnableSession = true)]
        public Boolean MemberLogin(String MemberId, String MemberPw, String ScaleNo)
        {
            Session["MemberLoginState"] = true;
            return (bool)Session["MemberLoginState"];
        }

        //获得会员卡余额
        [WebMethod(Description = "方法：获得会员卡余额。必须实现会员登录。返回：会员卡余额。-1为验证失败。", EnableSession = true)]
        public Double getMemberBalance()
        {
            if (Session["MemberLoginState"] == null || Session["MemberLoginState"].Equals(false))
            {
                return -1;
            }
            else
            {
                return MemberBalance;
            }

        }


        //结账
        [WebMethod(Description = "方法：结账。必须实现会员登录。参数：费用。返回：结账后会员卡余额。-1为验证失败或结账失败。", EnableSession = true)]
        public Double Checkout(Double Cost)
        {
            if (Session["MemberLoginState"] == null || Session["MemberLoginState"].Equals(false))
            {
                return -1;
            }
            else
            {
                if (MemberBalance >= Cost)
                {
                    return MemberBalance - Cost;
                }
                else
                {
                    return -1;
                }

            }

        }


        //退款
        [WebMethod(Description = "方法：退款。必须同时实现收银员登录和会员登录。参数：费用。返回：退款后后会员卡余额。-1为验证失败或退款失败。", EnableSession = true)]
        public Double Refund(Double Cost)
        {
            if (Session["MemberLoginState"] == null || Session["MemberLoginState"].Equals(false) || Session["CashierLoginState"] == null || Session["CashierLoginState"].Equals(false))
            {
                return -1;
            }
            else
            {
                return MemberBalance + Cost;
            }

        }

        [WebMethod(Description = "方法：上传交易流水。必须实现收银员登录。参数：费用xml形式。返回：暂时没做实现，具体开发时实现。", EnableSession = true)]
        public String uploadSalesAccount(XmlDocument sales)
        {
            if (Session["CashierLoginState"] == null || Session["CashierLoginState"].Equals(false))
            {
                return sales.HasChildNodes.ToString();
            }
            else
            {
                return sales.HasChildNodes.ToString();
            }
          
        }


    }
}
