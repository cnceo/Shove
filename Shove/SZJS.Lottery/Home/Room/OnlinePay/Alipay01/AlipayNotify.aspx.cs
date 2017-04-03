﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Net;

//本页不做任何数据处理
public partial class Home_Room_OnlinePay_Alipay01_AlipayNotify : SitePageBase
{
    SystemOptions so = new SystemOptions();
    string bankPay = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            new Log("System").Write("支付宝支付：AlipayNotify.aspx:接收到了通知！支付编号为：" + Request.Form["out_trade_no"]);

            //***************************************************************

            string subject_t = Request.Form["subject"];                //商品名称，用户存放用户ID     
            subject_t = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), subject_t);

            string partner = "";
            string key = "";

            if ((subject_t.IndexOf("_alipay") < 0) && (so["OnlinePay_Alipay_UserNumber1"].ToString("") != ""))
            {
                //纯网关
                partner = so["OnlinePay_Alipay_UserNumber1"].ToString("");  //卖家商户号
                key = so["OnlinePay_Alipay_MD5Key1"].ToString("");             //MD5-Key 
            }
            else
            {
                //支付宝
                partner = so["OnlinePay_Alipay_UserNumber"].ToString("");  //卖家商户号
                key = so["OnlinePay_Alipay_MD5Key"].ToString("");             //MD5-Key 
            }

            string _input_charset = "utf-8";

            string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";
            alipayNotifyURL = alipayNotifyURL + "service=notify_verify&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            string responseTxt = Get_Http(alipayNotifyURL, 120000);
            int i;
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;

            //进行排序；
            string[] Sortedstr = Alipay.Gateway.Utility.BubbleSort(requestarr);

            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (Request.Form[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.Form[Sortedstr[i]]);
                    }
                    else
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "&");

                    }
                }
            }

            prestr.Append(key);

            string mysign = Alipay.Gateway.Utility.GetMD5(prestr.ToString(), _input_charset);
            string sign = Request.Form["sign"];
            string trade_status = Request.Form["trade_status"];
            string trade_no = Request.Form["trade_no"];              //支付宝交易号
            string out_trade_no = Request.Form["out_trade_no"];      //自己交易号
            //string payment_type = Request.Form["payment_type"];      //支付类型
            string subject = Request.Form["subject"];                //商品名称，用户存放用户ID
            string subject_UnEncrypt = (Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), subject)).Split('_')[0];
            bankPay = (Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), subject)).Split('_')[2];//支付方式，刘美慧添加
            double total_fee = double.Parse(Request.Form["total_fee"].ToString());       //支付金额
            string seller_email = Request.Form["seller_email"];      //卖家账号

            if (seller_email != so["OnlinePay_Alipay_UserName"].ToString("") && seller_email != so["OnlinePay_Alipay_UserName_Email"].ToString(""))
            {
                new Log("System").Write("在线支付：支付用户信息验证失败！(Notify)");
                Response.Write("fail");     //返回给支付宝消息，失败

                Response.End();

                return;
            }

            if (mysign == sign && responseTxt == "true" && trade_status == "TRADE_FINISHED")   //验证支付发过来的消息，签名是否正确
            {

                Users user;

                if (_User == null)
                {
                    user = new Users(_Site.ID)[_Site.ID, Shove._Convert.StrToLong(subject_UnEncrypt, -1)];
                }
                else
                {
                    user = new Users(_Site.ID)[_Site.ID, _User.ID];
                }

                if (user == null)
                {
                    new Log("System").Write("在线支付：写入返回数据出错！(Notify)" + "支付号：" + out_trade_no);
                    Response.Write("fail");
                    Response.End();

                    return;
                }

                if (_User == null)
                {
                    _User = new Users(_Site.ID)[_Site.ID, user.ID];
                }

                if (WriteUserAccount(_User, out_trade_no.ToString(), total_fee.ToString(), "系统交易号：" + out_trade_no.ToString() + ",支付宝交易号：" + trade_no.ToString()))
                {

                    new Log("System").Write("在线支付：充值成功！(Notify)" + "支付号：" + out_trade_no);

                    Response.Write("success");     //返回给支付宝消息，成功
                    Response.End();

                    return;
                }
                else
                {
                    new Log("System").Write("在线支付：写入返回数据出错！(Notify)" + "支付号：" + out_trade_no);
                    Response.Write("fail");
                    Response.End();

                    return;
                }
            }
            else
            {
                new Log("System").Write("在线支付：签名不正确！(Notify)" + "支付号：" + out_trade_no + "responseTxt:" + responseTxt);
                Response.Write("fail");
                Response.End();

                return;
            }
        }
        catch (Exception Ex)
        {
            new Log("System").Write(Ex.Message);
            Response.Write("fail");
            Response.End();

            return;
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        base.OnLoad(e);
    }

    #endregion

    //获取远程服务器ATN结果
    public String Get_Http(String a_strUrl, int timeout)
    {
        string strResult;
        try
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
            myReq.Timeout = timeout;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            Stream myStream = HttpWResp.GetResponseStream();
            StreamReader sr = new StreamReader(myStream, Encoding.Default);
            StringBuilder strBuilder = new StringBuilder();
            while (-1 != sr.Peek())
            {
                strBuilder.Append(sr.ReadLine());
            }

            strResult = strBuilder.ToString();
        }
        catch (Exception exp)
        {

            strResult = "错误：" + exp.Message;
        }

        return strResult;
    }

    private bool WriteUserAccount(Users _User, string orderid, string amount, string Memo)
    {
        double Money = Shove._Convert.StrToDouble(amount, 0);
        if (Money == 0)
        {
            return false;
        }

        double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Money - Math.Round(Money / (FormalitiesFeesScale + 1), 2);
        Money -= FormalitiesFees;

        string ReturnDescription = "";
        bool ok = (_User.AddUserBalance(Money, FormalitiesFees, orderid, getBankName(bankPay), Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(orderid, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log("System").Write("在线支付：返回的交易号找不到对应的数据" + Memo);

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log("System").Write("在线支付：对应的数据未处理" + Memo);

                return false;
            }
        }

        return ok;
    }

    //根据支付方式来获取相应的中文说明
    private string getBankName(string bankCode)
    {

        string bankName = "支付宝";

        switch (bankCode.ToUpper())
        {
            case "ALIPAY":
                bankName = "支付宝";
                break;

            case "ICBCB2C":
                bankName = "中国工商银行";
                break;
            case "GDB":
                bankName = "广东发展银行";
                break;
            case "CEBBANK":
                bankName = "中国光大银行";
                break;
            case "CCB":
                bankName = "中国建设银行";
                break;
            case "COMM":
                bankName = "中国交通银行";
                break;
            case "ABC":
                bankName = "中国农业银行";
                break;
            case "SPDB":
                bankName = "上海浦发银行";
                break;
            case "SDB":
                bankName = "深圳发展银行";
                break;
            case "CIB":
                bankName = "兴业银行";
                break;
            case "HZCBB2C":
                bankName = "杭州银行";
                break;
            case "CMBC":
                bankName = "杭州银行";
                break;
            case "BOCB2C":
                bankName = "中国银行";
                break;
            case "CMB":
                bankName = "中国招商银行";
                break;
            case "CITIC":
                bankName = "中信银行";
                break;
            default:
                bankName = "支付宝";
                break;
        }

        return bankName;

    }
}