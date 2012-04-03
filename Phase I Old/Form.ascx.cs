using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;
using LightSPEED.NetSuite;
using Telerik.Web.UI;
using System.Collections.Generic;
using NDAL.DataTypes;
using Explore.NetSuite.DataAccess;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Website
{
    public partial class Form : baseOAWebControl
    {
        String strID;
        protected void Page_Load(object sender, EventArgs e)
        {

            HtmlControl myBody = (HtmlControl)Page.Master.FindControl("myBody");
            myBody.Attributes.Add("onload", "backButtonOverride()");

            //Page.Trace.IsEnabled = true;
            //Page.Trace.TraceMode = TraceMode.SortByTime;
            //HttpContext.Current.Trace.Warn("FormPage_PageLoad()", string.Format("{0},{1},{2}",
            //                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //                  "info",
            //                  "---Entering Form page------"));

            //UpdateTopHeader();
            //string cookieName = FormsAuthentication.FormsCookieName;
            //HttpCookie authCookie = Context.Request.Cookies[cookieName];

            //FormsAuthenticationTicket authTicket = null;
            //try
            //{
            //    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    string group = authTicket.UserData.ToString();

            //    if (group != "1")
            //    {
            //        Response.Redirect("UserLogin.aspx");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Response.Redirect("UserLogin.aspx");
            //}

            //FileStream infile = File.OpenRead(Server.MapPath(".") + "\\TestData\\ConfiguratorJSON.txt");
           
            //net.overlandagency.nsinkwebservice.Service service = new net.overlandagency.nsinkwebservice.Service();
            //byte[] fileBytes = new byte[infile.Length];
            //infile.Read(fileBytes, 0, (int)infile.Length);
            //service.ReceiveData(fileBytes);
            //ErrorLabel.Text = "";
            ErrorLabelBilling.Text = "";
            ErrorLabelShipping.Text = "";
            ErrorLabelDelivery.Text = "";
            ErrorLabelPayment.Text = "";
            Static6Messages.Text = "";

            //string baseAddress = "http://NSinkWebService.lightspeed-tek.com/Service.svc";
            //HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(baseAddress + "/ReceiveData");
            //req.Method = "POST";
            //req.ContentType = "text/xml; charset=UTF-8";
            //req.Headers.Add("Authorization: Basic TGlnaHRTUEVFRDpSTTZJSnNCYQ==");
            //Stream reqStream = req.GetRequestStream();
            //byte[] bytes = File.ReadAllBytes(Server.MapPath(".") + "\\TestData\\ConfiguratorRadioButtonJSON.txt");

            //reqStream.Write(bytes, 0, bytes.Length);
            //reqStream.Close();
            //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            //StreamReader sr = new StreamReader(resp.GetResponseStream());
            //string content = sr.ReadToEnd();

            RadWindowManager1.VisibleOnPageLoad = false;

            
            this.Parent.Page.Title = "Lightspeed - The Trusted Provider in Classroom Audio Technology";

            if (!IsPostBack)
            {
                //set session that prevents double submit on SubmitOrder
                Session["HasOrderBeenClicked"] = null;
                Session.Remove("HasOrderBeenClicked");

                //set session that prevents double submit on CreateUser
                Session["HasUserBeenClicked"] = null;
                Session.Remove("HasUserBeenClicked");

                Session["AccountBackClicked"] = null;
                Session.Remove("AccountBackClicked");
            }
            
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            SamesAsCheckBox3.Checked = false;
            SamesAsCheckBox32.Checked = false;
            SameAsLabel4CheckBox.Checked = false;
            SameAsBillingLabel4CheckBox.Checked = false;
            SameAsShippingLabel4CheckBox.Checked = false;

           StaticFieldsPanel1.Visible = false;
            StaticFieldsPanel2.Visible = false;
            StaticFieldsPanel3.Visible = false;
            StaticFieldsPanel4.Visible = false;
            StaticFieldsPanel5.Visible = false;
            StaticFieldsPanel6.Visible = false;
            StaticFieldsPanel7.Visible = false;
            StaticFieldsPanel8.Visible = false;
            StaticFieldsPanel9.Visible = false;

            if(Session["AccountCreateType"] == null)
                Session["AccountCreateType"] = "Create";

            bool isCreate = false;

            if (Session["AccountCreateType"] == "Create")
                isCreate = true;

            string message = "";

           
            try
            {
                if (Session["formpage"] == null)
                {
                    Session["formpage"] = "1";
                }

                bool drawForm = false;
                if (Session["RedrawForm"] == null)
                {
                    drawForm = true;
                }
                else
                {
                    if (bool.Parse(Session["RedrawForm"].ToString()))
                        drawForm = true;
                    else
                        drawForm = false;
                }

                if (!IsPostBack)
                {
                    drawForm = true;
                }
                //ErrorLabel.Text = "got here2";
                DataSet dsAll = doQuery("SELECT * FROM FormStaticPageFields");
                if (drawForm)
                {
                    
                    string formID = Session["formpage"].ToString();

                    if (int.Parse(formID) < 7)
                    {

                        if (!isCreate)
                        {
                            Image theImage = (Image)FindControl("Image" + formID);
                            theImage.Visible = true;
                        }

                        DataSet ds = doQuery("SELECT NavTitle FROM FormPages WHERE ID=" + formID);
                        DataSet dsForm = doQuery("SELECT * FROM FormStaticPageFields WHERE FormPageID=" + formID);
                        
                        DataSet dsCount = doQuery("SELECT DISTINCT FormPageID FROM FormStaticPageFields");


                        if (int.Parse(formID) < 6)
                        {
                            //ErrorLabel.Text = "got here";
                            ProductName.Text = ds.Tables[0].Rows[0]["NavTitle"].ToString();

                            
                            if (formID == "2")
                            {
                                AllMoneyPanel.DefaultButton = "Button1";
                                
                            }
                            else if(formID == "5")
                            {
                                AllMoneyPanel.DefaultButton = "Button5";
                                StaticFieldsPanel2.DefaultButton = "Button5";

                                BillingHidePanel.DefaultButton = "Button5";
                            }
                        }
                        else if(formID == "6")
                        {
                            ProductName.Text = "Review Order Information";
                        }
                        else if (formID == "7")
                        {
                            ProductName.Visible = false;

                            //Get the invoice
                            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                            Array theTransactions = (Array)client.GetTransactions(Session["UserID"].ToString(), DateTime.Now.AddMinutes(double.Parse("-10.00")), DateTime.Now);

                            //code recommended by john ogle. refer to bug 1688 in OA bug tracking system.
                            Dictionary<string, IEnumerable<Transaction>> invoiceMap = new Dictionary<string, IEnumerable<Transaction>>();
                            List<Transaction> invoices;

                            foreach (Transaction t in theTransactions)
                            {
                                if (t.Type != "SalesOrd")
                                {
                                    invoices = new List<Transaction>();

                                    if (invoiceMap.ContainsKey(t.CreatedFrom))
                                    {
                                        invoices = (List<Transaction>)invoiceMap[t.CreatedFrom];
                                    }
                                    else
                                    {
                                        invoiceMap.Add(t.CreatedFrom, invoices);
                                    }

                                    invoices.Add(t);
                                }
                            }

                            foreach (Transaction oneTrans in theTransactions)
                            {
                                if (oneTrans.Type == "SalesOrd" && oneTrans.OrderNumber == Session["OrderNumber"].ToString())
                                {
                                    SalesOrderHyperLink.NavigateUrl = oneTrans.PrintoutURL;
                                }
                            }
                        }
                        else if (formID == "8")
                        {
                            ProductName.Visible = false;
                        }

                        if (formID == "6")
                        {
                            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();

                            BuildIt(client, client.GetCustomer(Session["UserID"].ToString()));

                        }

                        Panel staticPanel = (Panel)FindControl("StaticFieldsPanel" + formID);
                        staticPanel.Visible = true;



                        if (dsAll.Tables.Count > 0)
                        {
                            for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                            {
                                Label theLabel = (Label)FindControl(dsAll.Tables[0].Rows[i]["LabelFormName"].ToString());
                                message += dsAll.Tables[0].Rows[i]["LabelFormName"].ToString();
                                theLabel.Text = dsAll.Tables[0].Rows[i]["LabelName"].ToString();
                            }
                        }

                        

                        RadComboBoxItem item;
                        



                        RadComboBox countryDrop = new RadComboBox();
                        RadComboBox stateDrop = new RadComboBox();
                        TextBox stateTextB = new TextBox();

                        for (int i = 0; i < dsCount.Tables[0].Rows.Count; i++)
                        {
                            
                            countryDrop = (RadComboBox)FindControl("CountryLabel" + (i + 1).ToString() + "RadComboBox");
                            stateTextB = (TextBox)FindControl("StateLabel" + (i + 1).ToString() + "TextBox");
                            stateDrop = (RadComboBox)FindControl("StateLabel" + (i + 1).ToString() + "RadComboBox");


                            if (countryDrop != null)
                            {
                                message += "fin got here";
                                DataSet dsCountry = doQuery("SELECT * FROM Countries ORDER BY country_2_code asc");
                                countryDrop.Items.Clear();
                                countryDrop.DataSource = dsCountry;
                                countryDrop.DataTextField = "country_name";
                                countryDrop.DataValueField = "country_id";
                                countryDrop.DataBind();
                                //for (int n = 0; n < dsCountry.Tables["table"].Rows.Count; n++)
                                //{
                                //    item = new RadComboBoxItem(dsCountry.Tables["table"].Rows[n]["country_2_code"].ToString(), 
                                //        dsCountry.Tables["table"].Rows[n]["country_id"].ToString());
                                //    countryDrop.Items.Add(item);
                                //}
                                dsCountry = doQuery("SELECT * FROM countries where country_name='" + ConfigurationManager.AppSettings.Get("defaultcountry") + "'");
                                countryDrop.SelectedValue = dsCountry.Tables["table"].Rows[0]["country_id"].ToString();

                                DataSet dsState = doQuery("SELECT * FROM states WHERE country_id=" + countryDrop.SelectedValue + " ORDER BY state_name ASC");

                                if (dsState.Tables["table"].Rows.Count > 0)
                                {
                                    stateDrop.Items.Clear();
                                    for (int n = 0; n < dsState.Tables["table"].Rows.Count; n++)
                                    {
                                        item = new RadComboBoxItem(dsState.Tables["table"].Rows[n]["state_name"].ToString(), dsState.Tables["table"].Rows[n]["state_code"].ToString());
                                        stateDrop.Items.Add(item);
                                    }
                                    stateTextB.Visible = false;
                                    stateDrop.Visible = true;
                                    stateDrop.Items.Insert(0, new RadComboBoxItem("Please Select...", "-1"));

                                }
                                else
                                {
                                    stateTextB.Visible = true;
                                    stateDrop.Visible = false;
                                }
                            }
                        }


                    }
                    else
                    {
                        GoToForm(int.Parse(formID), 1);
                    }


                }


                DataView dvHelp = new DataView(dsAll.Tables[0], "hasHelp='True'", "", DataViewRowState.CurrentRows);

                for (int j = 0; j < dvHelp.Count; j++)
                {
                    Panel helpPanel = (Panel)FindControl(dvHelp[j]["LabelFormName"].ToString() + "HelpPanel");
                    CreateHelp(dvHelp[j]["HelpText"].ToString(), j.ToString(), ref helpPanel);
                }

                SetBoxes();
                AddFields();



                if (drawForm)
                {
                    switch (Session["formpage"].ToString())
                    {
                        //case "1":
                        //    FillAccount();
                        //    break;
                        case "1":
                            RoleDropDown.Items.Clear();
                            DataSet ds = doQuery("SELECT * FROM ROLES ORDER BY ID ASC");
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = "Please Select...";
                            item.Value = "-1";
                            RoleDropDown.Items.Add(item);
                            for (int i = 0; i < ds.Tables["table"].Rows.Count; i++)
                            {
                                item = new RadComboBoxItem(ds.Tables["table"].Rows[i]["NAME"].ToString(),
                                    ds.Tables["table"].Rows[i]["NetSuiteID"].ToString());
                                RoleDropDown.Items.Add(item);
                            }

                            ds = new DataSet();
                            AccountDropDown.Items.Clear();
                            ds = doQuery("SELECT * FROM AccountType");
                            item = new RadComboBoxItem();
                            item.Text = "Please Select...";
                            item.Value = "-1";
                            AccountDropDown.Items.Add(item);
                            AccountDropDown.Width = 240;
                            for (int i = 0; i < ds.Tables["table"].Rows.Count; i++)
                            {
                                item = new RadComboBoxItem(ds.Tables["table"].Rows[i]["AccountType"].ToString(),
                                    ds.Tables["table"].Rows[i]["NetSuiteID"].ToString());
                                AccountDropDown.Items.Add(item);
                            }
                            break;
                        case "2":
                            StaticFieldsPanel2.DefaultButton = "Button1";
                            BillingHidePanel.DefaultButton = "Button1";
                            FillBilling();
                            break;
                        case "3":
                            FillShipping();
                            break;
                        case "4":
                            FillDelivery();
                            break;
                        case "5":
                            StaticFieldsPanel2.DefaultButton = "Button5";
                            BillingHidePanel.DefaultButton = "Button5";
                            GoToForm(5, 4);
                            //SetBoxes();
                            //BuildIt();
                            FillPayment();
                            break;
                        case "6":
                            GoToForm(6, 5);
                            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();

                            BuildIt(client, client.GetCustomer(Session["UserID"].ToString()));
                            FillPayment();
                            SetBoxes();
                            break;
                        default: break;
                    }
                }

              
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString() + message;
            }

            if (Session["formpage"] == "2")
            {
                if (Session["InShopping"] != null)
                {
                    Button9.Visible = false;
                }
            }

        }

        protected void SetBoxes()
        {
            if (Session["ShippingNameLabel"] != null)
                ShippingNameLabel.Text = Session["ShippingNameLabel"].ToString();

            if (Session["ShippingCompanyLabel"] != null)
                ShippingCompanyLabel.Text = Session["ShippingCompanyLabel"].ToString();

            if (Session["ShippingAddressLabel"] != null)
                ShippingAddressLabel.Text = Session["ShippingAddressLabel"].ToString();

            if (Session["ShippingAddress2Label"] != null)
                ShippingAddress2Label.Text = Session["ShippingAddress2Label"].ToString();

            if (Session["ShippingStateLabel"] != null)
                ShippingStateLabel.Text = Session["ShippingCityLabel"].ToString() + ", " + Session["ShippingStateLabel"].ToString() +
                    " " + Session["ShippingZipLabel"].ToString();

            if (Session["ShippingPhoneLabel"] != null)
                ShippingPhoneLabel.Text = Session["ShippingPhoneLabel"].ToString();

            if (Session["ShippingEx"] != null)
                if (Session["ShippingEx"].ToString().Trim() != "")
                    ShippingPhoneLabel.Text = ShippingPhoneLabel.Text + " x" + Session["ShippingEx"].ToString();



            if (Session["BillingNameLabel"] != null)
                BillingNameLabel.Text = Session["BillingNameLabel"].ToString();

            if (Session["BillingCompanyLabel"] != null)
                BillingCompanyLabel.Text = Session["BillingCompanyLabel"].ToString();

            if (Session["BillingAddressLabel"] != null)
                BillingAddressLabel.Text = Session["BillingAddressLabel"].ToString();

            if (Session["BillingAddress2Label"] != null)
                BillingAddress2Label.Text = Session["BillingAddress2Label"].ToString();

            if (Session["BillingStateLabel"] != null)
                BillingStateLabel.Text = Session["BillingCityLabel"].ToString() + ", " + Session["BillingStateLabel"].ToString()  +
                    " " + Session["BillingZipLabel"].ToString();

            if (Session["BillingPhoneLabel"] != null)
                BillingPhoneLabel.Text = Session["BillingPhoneLabel"].ToString();

            if (Session["BillingEx"] != null)
                if (Session["BillingEx"].ToString().Trim() != "")
                    BillingPhoneLabel.Text = BillingPhoneLabel.Text + " x" + Session["BillingEx"].ToString();



            if (Session["DeliveryNameLabel"] != null)
                DeliveryNameLabel.Text = Session["DeliveryNameLabel"].ToString();

            if (Session["DeliveryCompanyLabel"] != null)
                DeliveryCompanyLabel.Text = Session["DeliveryCompanyLabel"].ToString();

            if (Session["DeliveryAddressLabel"] != null)
                DeliveryAddressLabel.Text = Session["DeliveryAddressLabel"].ToString();

            if (Session["DeliveryAddress2Label"] != null)
                DeliveryAddress2Label.Text = Session["DeliveryAddress2Label"].ToString();

            if (Session["DeliveryStateLabel"] != null)
                DeliveryStateLabel.Text = Session["DeliveryCityLabel"].ToString() + ", " + Session["DeliveryStateLabel"].ToString() +
                    " " + Session["DeliveryZipLabel"].ToString();

            if (Session["DeliveryPhoneLabel"] != null)
                DeliveryPhoneLabel.Text = Session["DeliveryPhoneLabel"].ToString();

            if (Session["DeliveryEx"] != null)
                if (Session["DeliveryEx"].ToString().Trim() != "")
                    DeliveryPhoneLabel.Text = DeliveryPhoneLabel.Text + " x" + Session["DeliveryEx"].ToString();

            if (Session["PaymentCookie"] != null)
            {
                if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
                {
                    if (Session["CreditExpirationLabel"] != null)
                    {
                        CreditCardInfoPanel.Visible = true;
                        POPanel.Visible = false;
                        CreditCardNumberLabel.Text = "";
                        if (CreditCardLabelTextBox.Text.Length > 4)
                        {
                            int count = 0;

                            while (count < CreditCardLabelTextBox.Text.Length - 4)
                            {
                                CreditCardNumberLabel.Text += "x";
                                count++;
                            }
                            CreditCardNumberLabel.Text += CreditCardLabelTextBox.Text.Substring(CreditCardLabelTextBox.Text.Length - 4, 4);
                        }
                        else
                        {
                            CreditCardNumberLabel.Text = CreditCardLabelTextBox.Text;
                        }
                        if (Session["CreditCardLabel"] != null)
                            CreditCardNumberLabel.Text = Session["CreditCardLabel"].ToString();
                    }
                }
                else
                {
                    if (Session["POExpirationLabel"] != null)
                    {
                        CreditCardInfoPanel.Visible = false;
                        POPanel.Visible = true;
                        POTextBox.Text = Session["POExpirationLabel"].ToString();
                    }
                }
            }


        }

        protected void AddFields()
        {
            
            DataSet dsForms = doQuery("SELECT * FROM FormPages");

            for (int n = 0; n < dsForms.Tables[0].Rows.Count; n++)
            {
                string formID = dsForms.Tables[0].Rows[n]["ID"].ToString();
                //DataSet ds = doQuery("SELECT NavTitle FROM FormPages WHERE ID=" + formID);
                //ProductName.Text = ds.Tables[0].Rows[0]["NavTitle"].ToString();

                //get the field
                DataSet dsFields = doQuery("SELECT * FROM FormPageFields FPF, FieldTypes FT WHERE FPF.FieldTypeID=FT.ID AND FPF.FormPageID=" +
                    formID);

                if (dsFields.Tables.Count > 0)
                    if (dsFields.Tables[0].Rows.Count > 0)
                    {

                        for (int i = 0; i < dsFields.Tables[0].Rows.Count; i++)
                        {

                            string fieldType = dsFields.Tables[0].Rows[i]["FieldTypeName"].ToString();
                            string fieldLabel = dsFields.Tables[0].Rows[i]["FieldName"].ToString();
                            bool isRequired = bool.Parse(dsFields.Tables[0].Rows[i]["isRequired"].ToString());
                            int width = int.Parse(dsFields.Tables[0].Rows[i]["Width"].ToString());
                            string NetSuiteField = dsFields.Tables[0].Rows[i]["NetSuiteField"].ToString();
                            string defaultID = dsFields.Tables[0].Rows[i]["SelectedDefault"].ToString();

                            bool isValidation = false;
                            string validationString = "";
                            string validationMessage = "";
                            if (dsFields.Tables[0].Rows[i]["ValidationExpression"] != null)
                            {
                                if (dsFields.Tables[0].Rows[i]["ValidationExpression"].ToString() != "")
                                {
                                    isValidation = true;
                                    validationString = dsFields.Tables[0].Rows[i]["ValidationExpression"].ToString();
                                    validationMessage = dsFields.Tables[0].Rows[i]["ValidationMessage"].ToString();
                                }
                            }

                            bool isResidential = false;
                            bool isHelp = false;
                            string helpText = "";
                            if (bool.Parse(dsFields.Tables[0].Rows[i]["HasResidential"].ToString()))
                                isResidential = true;
                            if (bool.Parse(dsFields.Tables[0].Rows[i]["HasHelpIcon"].ToString()))
                            {
                                isHelp = true;
                                helpText = dsFields.Tables[0].Rows[i]["HelpText"].ToString();
                            }

                            string sortOrder = dsFields.Tables[0].Rows[i]["SortOrder"].ToString();

                            Panel panel = (Panel)FindControl("MorePanel" + formID + sortOrder);
                            string sortTemp = sortOrder;
                            if (panel == null)
                            {
                                while (panel == null && int.Parse(sortTemp) >= 0)
                                {
                                    sortTemp = (int.Parse(sortTemp) - 1).ToString();
                                    panel = (Panel)FindControl("MorePanel" + formID + sortTemp);
                                }
                            }

                            if (panel != null)
                            {
                                panel.Visible = true;
                                switch (fieldType)
                                {
                                    case "DropDown":
                                        CreateDropDown(fieldLabel, dsFields.Tables[0].Rows[i]["CollectionName"].ToString(),
                                            isRequired, width, dsFields.Tables[0].Rows[i]["CollectionValue"].ToString(),
                                            dsFields.Tables[0].Rows[i]["CollectionText"].ToString(), NetSuiteField, defaultID,
                                            dsFields.Tables[0].Rows[i]["DropDownEmptyText"].ToString(), i.ToString(), ref panel);
                                        break;
                                    case "TextBox":
                                        CreateTextBox(fieldLabel, isRequired, width, NetSuiteField, isValidation, validationString,
                                            validationMessage, false, formID + i.ToString(), false, isResidential, isHelp, helpText, ref panel, formID);
                                        break;
                                    case "CheckBox":
                                        CreateCheckBox(fieldLabel, width, NetSuiteField, (i).ToString() + "More", ref panel);
                                        break;
                                    default: break;
                                }
                            }
                        }
                    }

            }
        }

        protected void GoToForm(int formID, int disappearID)
        {
            SamesAsCheckBox32.Checked = false;
            SameAsLabel4CheckBox.Checked = false;
            SameAsBillingLabel4CheckBox.Checked = false;
            SameAsShippingLabel4CheckBox.Checked = false;


            SamesAsCheckBox3.Checked = false;
            SamesAsCheckBox32.Checked = false;
            SameAsLabel4CheckBox.Checked = false;
            SameAsBillingLabel4CheckBox.Checked = false;
            SameAsShippingLabel4CheckBox.Checked = false;

            if (Session["UserID"] != null)
            {
                if (IsUserInternational())
                {
                    formID = 9;
                }
                else
                {
                    Panel staticPanel3 = (Panel)FindControl("StaticFieldsPanel9");
                    staticPanel3.Visible = false;
                }
                
                    Panel staticPanel = (Panel)FindControl("StaticFieldsPanel" + disappearID.ToString());
                    staticPanel.Visible = false;

                    Panel staticPanel2 = (Panel)FindControl("StaticFieldsPanel" + (formID).ToString());
                    staticPanel2.Visible = true;

                    Image1.Visible = false;
                    Image2.Visible = false;
                    Image3.Visible = false;
                    Image4.Visible = false;
                    Image5.Visible = false;
                    Image6.Visible = false;

                    if (formID < 7)
                    {
                        Image theImage = (Image)FindControl("Image" + formID);
                        theImage.Visible = true;
                        
                    }

                    if (formID == 5)
                    {
                        StaticFieldsPanel2.Visible = true;
                        BillingForPaymentPanel.Visible = true;
                        BillingHidePanel.Visible = false;
                        BillingHidePanel2.Visible = false;

                        PaymentButtonsPanel.Visible = true;

                        AllMoneyPanel.DefaultButton = "Button5";


                    }
                    else if (formID == 1)
                    {
                        RoleDropDown.Items.Clear();
                        DataSet ds3 = doQuery("SELECT * FROM ROLES ORDER BY ID ASC");
                        RadComboBoxItem item = new RadComboBoxItem();
                        item.Text = "Please Select...";
                        item.Value = "-1";
                        RoleDropDown.Items.Add(item);
                        for (int i = 0; i < ds3.Tables["table"].Rows.Count; i++)
                        {
                            item = new RadComboBoxItem(ds3.Tables["table"].Rows[i]["NAME"].ToString(),
                                ds3.Tables["table"].Rows[i]["NetSuiteID"].ToString());
                            RoleDropDown.Items.Add(item);
                        }

                        ds3 = new DataSet();
                        AccountDropDown.Items.Clear();
                        ds3 = doQuery("SELECT * FROM AccountType");
                        item = new RadComboBoxItem();
                        item.Text = "Please Select...";
                        item.Value = "-1";
                        AccountDropDown.Items.Add(item);
                        AccountDropDown.Width = 240;
                        for (int i = 0; i < ds3.Tables["table"].Rows.Count; i++)
                        {
                            item = new RadComboBoxItem(ds3.Tables["table"].Rows[i]["AccountType"].ToString(),
                                ds3.Tables["table"].Rows[i]["NetSuiteID"].ToString());
                            AccountDropDown.Items.Add(item);
                        }
                    }
                    else if (formID == 7)
                    {
                        ProductName.Visible = false;

                        HttpContext.Current.Trace.Warn("FormPage_DrawForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review get transactions for invoice Begin------"));
                        //Get the invoice
                        Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                        Array theTransactions = (Array)client.GetTransactions(Session["UserID"].ToString(), DateTime.Now.AddMinutes(double.Parse("-10.00")), DateTime.Now);

                        //code recommended by john ogle. refer to bug 1688 in OA bug tracking system.
                        Dictionary<string, IEnumerable<Transaction>> invoiceMap = new Dictionary<string, IEnumerable<Transaction>>();
                        List<Transaction> invoices;

                        foreach (Transaction t in theTransactions)
                        {
                            if (t.Type != "SalesOrd")
                            {
                                invoices = new List<Transaction>();

                                if (invoiceMap.ContainsKey(t.CreatedFrom))
                                {
                                    invoices = (List<Transaction>)invoiceMap[t.CreatedFrom];
                                }
                                else
                                {
                                    invoiceMap.Add(t.CreatedFrom, invoices);
                                }

                                invoices.Add(t);
                            }
                        }

                        foreach (Transaction oneTrans in theTransactions)
                        {
                            if (oneTrans.Type == "SalesOrd" && oneTrans.OrderNumber == Session["OrderNumber"].ToString())
                            {
                                SalesOrderHyperLink.NavigateUrl = oneTrans.PrintoutURL;
                            }
                        }

                        HttpContext.Current.Trace.Warn("FormPage_DrawForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review get transactions for invoice End------"));
                    }
                    else if (formID == 8)
                    {
                        ProductName.Visible = false;
                    }
                    else if (formID == 2)
                    {
                        StaticFieldsPanel2.Visible = true;
                        BillingForPaymentPanel.Visible = true;
                        BillingHidePanel.Visible = true;
                        BillingHidePanel2.Visible = true;

                        PaymentButtonsPanel.Visible = false;

                        AllMoneyPanel.DefaultButton = "Button1";
                       
                    }
                    else
                    {
                        StaticFieldsPanel2.Visible = false;
                        BillingForPaymentPanel.Visible = false;
                        BillingHidePanel.Visible = true;
                        BillingHidePanel2.Visible = true;

                        PaymentButtonsPanel.Visible = false;
                    }

                    DataSet ds = doQuery("SELECT NavTitle FROM FormPages WHERE ID=" + formID.ToString());

                    Session["formpage"] = formID.ToString();

                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ProductName.Text = ds.Tables[0].Rows[0]["NavTitle"].ToString();

                        }
                        else
                        {
                            if (formID == 6)
                                ProductName.Text = "Review Order Information";
                            else if (formID == 7)
                                ProductName.Visible = false;
                            else if (formID == 8)
                            {
                                ProductName.Visible = false;
                            }
                        }
                    else
                    {
                        if (formID == 6)
                            ProductName.Text = "Review Order Information";
                        else if (formID == 7)
                            ProductName.Visible = false;
                        else if (formID == 8)
                        {
                            ProductName.Visible = false;
                        }
                    }
                
            }
            else
            {
                Response.Redirect("~/UserLogin.aspx");
            }
        }

        protected void CreateDropDown(string FieldLabel, string CollectionName, bool required, int width, 
            string collectionValue, string collectionText, string NetSuiteField, string defaultID, string emptyText, 
            string ID, ref Panel thePanel)
        {
            Literal theLiteral = new Literal();
            theLiteral.Text = "<tr><td>";
            thePanel.Controls.Add(theLiteral);

            Label label = new Label();
            label.Text = FieldLabel;
            if (required)
                label.Text += " *";
            thePanel.Controls.Add(label);

            Literal middleLiteral = new Literal();
            middleLiteral.Text = "</td><td>";

            thePanel.Controls.Add(middleLiteral);

            DropDownList dropdown = new DropDownList();
            dropdown.Width = width;
            dropdown.ID = FieldLabel.Replace(" ", "")+ID;
            dropdown.Attributes.Add("NetSuiteAttribute", NetSuiteField);

            DataSet dsDropDown = doQuery("SELECT * FROM "+CollectionName);

            
            dropdown.DataSource = dsDropDown;
            dropdown.DataTextField = collectionText;
            dropdown.DataValueField = collectionValue;
            dropdown.DataBind();

            dropdown.Items.Insert(0, new ListItem(emptyText, "-1"));

            dropdown.SelectedValue = defaultID;

            thePanel.Controls.Add(dropdown);

            Literal anotherLiteral = new Literal();
            anotherLiteral.Text = "</td></tr>";
            thePanel.Controls.Add(anotherLiteral);

        }

        protected void CreateTextBox(string FieldLabel, bool required, int width,string NetSuiteField, 
            bool isValidation, string validationString, string validationMessage, bool isPassword, 
            string ID, bool isPhone, bool isResidential, bool hasHelp, string helpText, ref Panel thePanel, string formID)
        {
            //thePanel.Controls.Clear();
            Literal theLiteral = new Literal();
            theLiteral.Text = "<tr><td>";
            thePanel.Controls.Add(theLiteral);

            Label label = new Label();
            label.Text = FieldLabel;
            if (required)
                label.Text += " *";
            thePanel.Controls.Add(label);

            Literal middleLiteral = new Literal();

            if (isValidation || required)
            {
                if (!isResidential)
                {
                    middleLiteral.Text = "</td><td width=\"" + (width).ToString() + "\">";
                }
                else
                {
                    if(hasHelp)
                        middleLiteral.Text = "</td><td width=\"" + (width).ToString() + "\"><table><tr><td align=\"left\">";
                    else
                        middleLiteral.Text = "</td><td width=\"" + (width + 20).ToString() + "\"><table><tr><td align=\"left\">";
                }
            }
            else
            {
                if (!isResidential)
                {
                    middleLiteral.Text = "</td><td width=\"" + (width).ToString() + "\">";
                }
                else
                {
                    if (hasHelp)
                        middleLiteral.Text = "</td><td width=\"" + (width).ToString() + "\"><table><tr><td align=\"left\">";
                    else
                        middleLiteral.Text = "</td><td width=\"" + (width + 20).ToString() + "\"><table><tr><td align=\"left\">";
                }
                
            }

            

            thePanel.Controls.Add(middleLiteral);


            TextBox textbox = new TextBox();
            textbox.ID = FieldLabel.Replace(" ", "")+ID;
            if (isPassword)
                textbox.TextMode = TextBoxMode.Password;
            textbox.Width = width;
            textbox.CssClass = "FloatLeft";

            thePanel.Controls.Add(textbox);

            if (hasHelp)
            {
                Literal someLiteral = new Literal();
                someLiteral.Text = "</td><td>";
                thePanel.Controls.Add(someLiteral);
                    CreateHelp(helpText, ID+"More", ref thePanel);
                   

            }

            if(required)
            {
                //<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="email"
                //ErrorMessage="Email required<br />"></asp:RequiredFieldValidator>

                

                RequiredFieldValidator req = new RequiredFieldValidator();
                req.ValidationGroup = "Form"+formID;
                req.Display = ValidatorDisplay.Dynamic;
                req.ControlToValidate = FieldLabel.Replace(" ", "")+ID;
                req.ErrorMessage = "     "+FieldLabel + " is required.";
                thePanel.Controls.Add(req);
            }

            if (isValidation)
            {
               
                RegularExpressionValidator reg = new RegularExpressionValidator();
                reg.ValidationGroup = "Form"+formID;
                reg.Display = ValidatorDisplay.Dynamic;
                reg.ControlToValidate = FieldLabel.Replace(" ", "") + ID;
                reg.ErrorMessage = " " + validationMessage;
                reg.ValidationExpression = validationString;
                thePanel.Controls.Add(reg);
            }

            Literal anotherLiteral3 = new Literal();

            //if (hasHelp)
            //{
            //    anotherLiteral3.Text = "</td></tr></table>";
            //}
            anotherLiteral3.Text += "</td></tr>";

            thePanel.Controls.Add(anotherLiteral3);
        }

        protected void CreateHelp(string helpText, string ID, ref Panel thePanel)
        {
            Literal hoverLiteral = new Literal();
            hoverLiteral.Text = "<div style=\"width: 30px;\" onmouseover=\"showDiv('div" + ID + "');\" onmouseout=\"hideDiv('div" + ID + "');\">";
            hoverLiteral.Text += "<div style=\"max-width: 300px; border: solid 1px black;display:none;"+
            "margin: 5px; padding: 10px; position: absolute; background-color: white;\" id=\"div" + ID + "\">" + helpText + "</div>";

            thePanel.Controls.Add(hoverLiteral);

            Image helpImage = new Image();
            helpImage.ID = "help" + ID;
            helpImage.CssClass = "FloatLeft";
            helpImage.ImageUrl = "~/images/HelpIcon.png";

            thePanel.Controls.Add(helpImage);

            Literal hoverLiteral2 = new Literal();
            hoverLiteral2.Text = "</div>";

            thePanel.Controls.Add(hoverLiteral2);
        }

        protected void CreateLocation(string FieldLabel, bool required, int width, string NetSuiteField, string DefaultID, 
            string ID, ref Panel thePanel)
        {

            Label label = new Label();
            label.Text = "State";
            if (required)
                label.Text += " *";
            thePanel.Controls.Add(label);

            Literal middleLiteral = new Literal();
            middleLiteral.Text = "</td><td>";

            thePanel.Controls.Add(middleLiteral);

            TextBox stateText = new TextBox();
            stateText.ID = "stateText";
            stateText.Width = width;
            stateText.Visible = false;

            thePanel.Controls.Add(stateText);


            DropDownList dropdown = new DropDownList();
            dropdown.Width = width;
            dropdown.ID = "stateDropDown";
            dropdown.Attributes.Add("NetSuiteAttribute", "State");
            dropdown.AutoPostBack = false;

            DataSet dsDropDown = doQuery("SELECT * FROM States");


            dropdown.DataSource = dsDropDown;
            dropdown.DataTextField = "state_name";
            dropdown.DataValueField = "state_id";
            dropdown.DataBind();
            dropdown.Visible = false;

            thePanel.Controls.Add(dropdown);

            Literal middlemiddleLiteral = new Literal();
            middlemiddleLiteral.Text = "</td></tr><tr><td>";

            thePanel.Controls.Add(middlemiddleLiteral);


            //Add the country dropdown
            Label label1 = new Label();
            label1.Text = "Country";
            if (required)
                label1.Text += " *";
            thePanel.Controls.Add(label1);

            Literal middleLiteral2 = new Literal();
            middleLiteral2.Text = "</td><td>";

            thePanel.Controls.Add(middleLiteral2);

            DropDownList dropdown1 = new DropDownList();
            dropdown1.Width = width;
            dropdown1.ID = "countryDropDown";
            dropdown1.Attributes.Add("NetSuiteAttribute", "Country");
            dropdown1.AutoPostBack = true;
            dropdown1.SelectedIndexChanged += new EventHandler(SelectedCountry);

            DataSet dsDropDown1 = doQuery("SELECT * FROM Countries");

            dropdown1.DataSource = dsDropDown1;
            dropdown1.DataTextField = "country_name";
            dropdown1.DataValueField = "country_id";
            dropdown1.DataBind();

            dropdown1.Items.Insert(0, new ListItem("Select Country", "-1"));
            dropdown1.SelectedValue = DefaultID;

            DataSet dsState = doQuery("SELECT * FROM States WHERE country_id="+DefaultID);

            if (dsState.Tables.Count > 0)
                if (dsState.Tables[0].Rows.Count > 0)
                {
                    stateText.Visible = false;
                    dropdown.Visible = true;

                    dropdown.DataSource = dsState;
                    dropdown.DataTextField = "state_name";
                    dropdown.DataValueField = "state_id";
                    dropdown.DataBind();

                    dropdown.Items.Insert(0, new ListItem("Select State", "-1"));
                }
                else
                {
                    stateText.Visible = true;
                }
            else
            {
                stateText.Visible = true;
            }

            thePanel.Controls.Add(dropdown1);

        }

        protected void OpenRadWindowServer(object sender, EventArgs e)
        {
            RadWindow1.Visible = true;
            RadWindowManager1.VisibleOnPageLoad = true;
        }

        protected void SelectedCountry(object sender, EventArgs e)
        {
            string formID = Session["formpage"].ToString();

            RadComboBox countryDrop = (RadComboBox)sender;

            RadComboBox stateDrop = new RadComboBox();
            TextBox stateTextB = new TextBox();

            //if (formID == "1")
            //{
            //    countryDrop = (RadComboBox)FindControl("CountryLabelDropDown");
            //    stateTextB = (TextBox)FindControl("StateLabelTextBox");
            //    stateDrop = (RadComboBox)FindControl("StateLabelDropDown");
            //}
            //else
            //{
                countryDrop = (RadComboBox)FindControl("CountryLabel" + formID + "RadComboBox");
                stateTextB = (TextBox)FindControl("StateLabel" + formID + "TextBox");
                stateDrop = (RadComboBox)FindControl("StateLabel" + formID + "RadComboBox");
            //}

            DataSet dsState = doQuery("SELECT * FROM States WHERE country_id=" + countryDrop.SelectedValue);

            if (dsState.Tables.Count > 0)
                if (dsState.Tables[0].Rows.Count > 0)
                {
                    stateDrop.Items.Clear();
                    stateTextB.Visible = false;
                    stateDrop.Visible = true;

                    stateDrop.DataSource = dsState;
                    stateDrop.DataTextField = "state_name";
                    stateDrop.DataValueField = "state_id";
                    stateDrop.DataBind();

                    stateDrop.Items.Insert(0, new RadComboBoxItem("Please Select", "-1"));
                }
                else
                {
                    stateTextB.Visible = true;
                    stateDrop.Visible = false;
                }
            else
            {
                stateTextB.Visible = true;
                stateDrop.Visible = false;
            }

            if (countryDrop.SelectedValue != "223")
            {
                Panel temp = (Panel)FindControl(countryDrop.ID + "Local");
                temp.Visible = false;
                temp = (Panel)FindControl(countryDrop.ID + "Local2");
                temp.Visible = false;
                temp = (Panel)FindControl(countryDrop.ID + "International");
                temp.Visible = true;
            }
            else
            {
                Panel temp = (Panel)FindControl(countryDrop.ID + "Local");
                temp.Visible = true;
                temp = (Panel)FindControl(countryDrop.ID + "Local2");
                temp.Visible = true;
                temp = (Panel)FindControl(countryDrop.ID + "International");
                temp.Visible = false;
            }

        }
     
        protected void CreateCheckBox(string FieldLabel, int width, string NetSuiteField, string ID, ref Panel thePanel)
        {
            Literal middleLiteral = new Literal();
            middleLiteral.Text = "<tr><td></td><td>";

            thePanel.Controls.Add(middleLiteral);

            CheckBox checkbox = new CheckBox();
            checkbox.ID = FieldLabel.Replace(" ", "")+ID;

            thePanel.Controls.Add(checkbox);

            
            Label label = new Label();
            label.Text = FieldLabel;

            thePanel.Controls.Add(label);

            Literal anotherLiteral = new Literal();
            anotherLiteral.Text = "</td></tr>";
            thePanel.Controls.Add(anotherLiteral);
        }

        protected string IsPasswordValid()
        {
            string message = "";
            if (PasswordTextBox.Text != cPasswordTextBox.Text)
                message = "The Password field does not match the Confirm Password field.";
            else if (PasswordTextBox.Text.Length < 6)
            {
                message = "The Password must be at least 6 characters and contain a digit and a capital letter.";
            }
            else if (!ContainsADigit(PasswordTextBox.Text))
            {
                message = "The Password must be at least 6 characters and contain a digit and a capital letter.";
            }
            else if (!ContainsACapital(PasswordTextBox.Text))
            {
                message = "The Password must be at least 6 characters and contain a digit and a capital letter.";
            }
            else
            {
                message = "success";
            }
            

            return message;
        }

        protected void NextClick(object sender, EventArgs e)
        {
            if (Session["AccountCreateType"] == null)
                Session["AccountCreateType"] = "Create";

            bool isCreate = false;
            bool isShoppingCreate = false;

            if (Session["AccountCreateType"] == "Create")
                isCreate = true;

            if (Session["ShoppingCreate"] != null)
                isShoppingCreate = bool.Parse(Session["ShoppingCreate"].ToString());
            Encryption encrypt = new Encryption();
            bool openMessage = false;
            string boxMessage = "";
            bool success = false;
            if (Session["HasUserBeenClicked"] == null)
            {
                
                try
                {
                    
                    Session["RedrawForm"] = false;
                    string state = "";
                    bool isStateValid = false;

                    if (StateLabel1RadComboBox.Visible)
                    {
                        if (StateLabel1RadComboBox.SelectedValue != "-1")
                            isStateValid = true;
                    }
                    else if (StateLabel1TextBox.Text.Trim() != "")
                        isStateValid = true;

                    if (isStateValid)
                    {
                        if (Page.IsValid && RoleDropDown.SelectedValue != "-1" &&
                            AccountDropDown.SelectedValue != "-1")
                        {

                            Session["FormClicked"] = true;
                            Session["formpage"] = "2";
                            bool hasState = false;

                            if (StateLabel1RadComboBox.Visible)
                            {
                                if (StateLabel1RadComboBox.SelectedValue != "-1")
                                    hasState = true;
                            }
                            else if (StateLabel1TextBox.Text.Trim() != "")
                                hasState = true;


                            string isPassValid = IsPasswordValid();

                            if (hasState)
                            {
                                if (isPassValid == "success")
                                {
                                    Session["AccountTypeLabel"] = AccountDropDown.SelectedItem.Value;

                                    Session["AccountFirstNameLabel"] = FirstNameTextBox.Text;

                                    Session["AccountLastNameLabel"] = LastNameTextBox.Text;

                                    Session["AccountRoleLabel"] = RoleDropDown.SelectedItem.Text;

                                    Session["AccountTitleLabel"] = TitleTextBox.Text;

                                    Session["AccountCompanyLabel"] = CompanyLabelTextBox.Text;

                                    Session["AccountResidentialLabel"] = ResidentialCheckBox.Checked;

                                    if (StateLabel1RadComboBox.Visible)
                                        state = StateLabel1RadComboBox.SelectedItem.Text;
                                    else
                                        state = StateLabel1TextBox.Text;

                                    Session["AccountStateLabel"] = state;

                                    Session["AccountCityLabel"] = CityLabelTextBox.Text;

                                    Session["AccountPhoneLabel"] = PhoneLabelTextBox.Text;

                                    if (exTextBox.Text.Trim() != "")
                                    {
                                        Session["AccountEx"] = exTextBox.Text;
                                    }

                                    Session["AccountAddressLabel"] = Address1LabelTextBox.Text;

                                    Session["AccountAddress2Label"] = Address2LabelTextBox.Text;

                                    Session["AccountZipLabel"] = ZipLabelTextBox.Text;

                                    Session["AccountCountryLabel"] = CountryLabel1RadComboBox.SelectedValue;

                                    Session["AccountEmailLabel"] = EmailTextBox.Text;

                                    Session["AccountPasswordLabel"] = PasswordTextBox.Text;

                                    Session["AccountNewsletterLabel"] = SignUpCheckBox.Checked;

                                    

                                    if (isCreate)
                                    {
                                        bool isError = false;
                                        try
                                        {
                                            success = CreateUser();

                                            if (success)
                                            {
                                                string redir ="/MyAccount.aspx";
                                                if (Request.QueryString["page"] != null && Request.QueryString["page"].Length > 0)
                                                {
                                                    //lblMessage.Text = "Got here 1";
                                                    redir = "../"+Request.QueryString["page"].Replace("%20", "+").Replace(" ", "+");
                                                }

                                                boxMessage = "Your account has been created. " +
                                                    "<br/><br/><button onclick=\"Search('" + redir + "');\" >OK</button><br/>";
                                                openMessage = true;

                                                JobRoles rolesJob = GetJobRole(RoleDropDown.SelectedItem.Text);

                                                if (rolesJob == JobRoles.Reseller_Contractor_Integrator)
                                                {
                                                    boxMessage = "Your account has been created. Since your account type is 'Reseller', please remember that you can contact your representative for better pricing." +
                                                        "<br/><br/><button onclick=\"Search('" + redir + "');\" >OK</button><br/>";
                                                    openMessage = true;
                                                }

                                                Session["HasUserBeenClicked"] = "true";
                                            }
                                        }
                                        catch (CommandErrorException ex)
                                        {
                                            isError = true;
                                            if (ex.ErrorCode.ToString().ToLower() == "invalid_duplicatecustomer")
                                            {
                                                Static1Messages.Text = "A user already exists for this email address. Please choose a different email.";
                                            }
                                            else
                                            {
                                                Static1Messages.Text = ex.ToString();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            isError = true;
                                            Static1Messages.Text = ex.ToString();
                                        }


                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (isShoppingCreate)
                                            {
                                                
                                                success = CreateUser();

                                                //string redir = "/MyAccount.aspx";
                                                //if (Request.QueryString["page"] != null && Request.QueryString["page"].Length > 0)
                                                //{
                                                //    //lblMessage.Text = "Got here 1";
                                                //    redir = Request.QueryString["page"].Replace("%20", "+").Replace(" ", "+");
                                                //}
                                                if (success && Session["AccountBackClicked"] == null)
                                                {
                                                    Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                                                    Customer cust = client.GetCustomer(Session["UserID"].ToString());

                                                    boxMessage = "Your account has been created. " +
                                                            "<br/><br/><button onclick=\"Search('/default.aspx?a=form&ID=2');\" >OK</button><br/>";
                                                    openMessage = true;

                                                    if (IsUserInternational())
                                                    {
                                                        boxMessage = "Your account has been created. At this time we do not provide international sales. To purchase a product, please contact your representative." +
                                                            "<br/><br/><button onclick=\"Search('/MyAccount.aspx');\" >OK</button><br/>";
                                                        openMessage = true;
                                                    }

                                                    JobRoles rolesJob = GetJobRole(RoleDropDown.SelectedItem.Text);

                                                    if (rolesJob == JobRoles.Reseller_Contractor_Integrator)
                                                    {
                                                        boxMessage = "Your account has been created. Since your account type is 'Reseller', please remember that you can contact your representative for better pricing." +
                                                            "<br/><br/><button onclick=\"Search('/default.aspx?a=form&ID=2');\" >OK</button><br/>";
                                                        openMessage = true;
                                                    }

                                                }
                                                else
                                                {
                                                    openMessage = false;
                                                }
                                            }

                                            if (!openMessage && success)
                                            {
                                                if (CountryLabel1RadComboBox.SelectedItem.Value != "223")
                                                {
                                                    Session["RedrawForm"] = true;
                                                    Session["formpage"] = "9";
                                                    GoToForm(9, 1);
                                                }
                                                else
                                                {
                                                    Session["RedrawForm"] = true;
                                                    GoToForm(2, 1);

                                                    Session["formpage"] = "2";
                                                }

                                                

                                            }
                                        }
                                        catch (CommandErrorException ex)
                                        {
                                            Session["formpage"] = "1";
                                            if (ex.ErrorCode.ToString().ToLower() == "invalid_duplicatecustomer")
                                            {
                                                Static1Messages.Text = "A user already exists for this email address. Please choose a different email.";
                                            }
                                            else
                                            {
                                                Static1Messages.Text = ex.ToString();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Session["formpage"] = "1";
                                            Static1Messages.Text = ex.ToString();
                                        }
                                    }

                                    if (openMessage && success)
                                    {
                                        MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(boxMessage);
                                        MessageRadWindow.Visible = true;
                                        MessageRadWindowManager.VisibleOnPageLoad = true;
                                    }

                                }
                                else
                                {
                                    Static1Messages.Text = isPassValid;
                                }
                            }
                            else
                            {
                                Static1Messages.Text = "State is required";
                            }
                        }
                        else
                        {
                            Static1Messages.Text = "Both Role and Account Type are required.";
                        }
                    }
                    else
                    {
                        Static1Messages.Text = "State is required";
                    }
                }
                catch (CommandErrorException ex)
                {
                    if (ex.ToString().Contains("for the following field:"))
                    {
                        ErrorLabel.Text = " The Phone is invalid.";
                    }
                    else
                    {
                        ErrorLabel.Text = ex.Message.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Session["formpage"] = "1";
                    ErrorLabel.Text = ex.ToString();
                }
            }
            else
            {
                if (isCreate)
                {

                    string redir = "/MyAccount.aspx";
                    if (Request.QueryString["page"] != null && Request.QueryString["page"].Length > 0)
                    {
                        //lblMessage.Text = "Got here 1";
                        redir = "../"+Request.QueryString["page"].Replace("%20", "+").Replace(" ", "+");
                    }

                    boxMessage = "Your account has been created. " +
                        "<br/><br/><button onclick=\"Search('" + redir + "');\" >OK</button><br/>";
                    openMessage = true;


                    if (AccountDropDown.SelectedValue == "4")
                    {
                        boxMessage = "Your account has been created. Since your account type is 'Reseller', please remember that you can contact your representative for better pricing." +
                            "<br/><br/><button onclick=\"Search('" + redir + "');\" >OK</button><br/>";
                        openMessage = true;
                    }
                }
                else
                {
                    if (isShoppingCreate)
                    {

                        //string redir = "/MyAccount.aspx";
                        //if (Request.QueryString["page"] != null && Request.QueryString["page"].Length > 0)
                        //{
                        //    //lblMessage.Text = "Got here 1";
                        //    redir = Request.QueryString["page"].Replace("%20", "+").Replace(" ", "+");
                        //}
                        if (Session["AccountBackClicked"] == null)
                        {
                            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                            Customer cust = client.GetCustomer(Session["UserID"].ToString());

                            boxMessage = "Your account has been created. " + cust.SalesRepID +
                                    "<br/><br/><button onclick=\"Search('/default.aspx?a=form&ID=2');\" >OK</button><br/>";
                            openMessage = true;

                            if (IsUserInternational())
                            {
                                boxMessage = "Your account has been created. At this time we do not provide international sales. To purchase a product, please contact your representative." +
                                    "<br/><br/><button onclick=\"Search('/MyAccount.aspx');\" >OK</button><br/>";
                                openMessage = true;
                            }

                            if (AccountDropDown.SelectedValue == "4")
                            {
                                boxMessage = "Your account has been created. Since your account type is 'Reseller', please remember that you can contact your representative for better pricing." +
                                    "<br/><br/><button onclick=\"Search('/default.aspx?a=form&ID=2');\" >OK</button><br/>";
                                openMessage = true;
                            }

                        }
                    }
                    if (!openMessage)
                    {
                        if (CountryLabel1RadComboBox.SelectedItem.Value != "223")
                        {
                            Session["RedrawForm"] = true;
                            Session["formpage"] = "9";
                            GoToForm(9, 1);
                        }
                        else
                        {
                            Session["RedrawForm"] = true;
                            GoToForm(2, 1);

                            Session["formpage"] = "2";
                        }

                        Session["HasUserBeenClicked"] = "true";

                    }
                   
                }

                if (openMessage)
                {
                    MessageRadWindow.NavigateUrl = "Message.aspx?message=" + encrypt.encrypt(boxMessage);
                    MessageRadWindow.Visible = true;
                    MessageRadWindowManager.VisibleOnPageLoad = true;
                }
            }
        }

        protected void SessionDynamicFields(string formID)
        {
            DataSet dsFields = doQuery("SELECT * FROM FormPageFields FPF, FieldTypes FT WHERE FPF.FieldTypeID=FT.ID AND FPF.FormPageID=" +
                    formID);
            DataView dvFields = new DataView(dsFields.Tables[0], "", "", DataViewRowState.CurrentRows);
            if (dvFields.Count > 0)
            {
                for (int i = 0; i < dvFields.Count; i++)
                {
                    TextBox textbox = (TextBox)FindControl(dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString());
                    Session["Account" + dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString()] = textbox.Text;
                }
            }
        }

        protected void SetDynamicFields(string formID)
        {
            DataSet dsFields = doQuery("SELECT * FROM FormPageFields FPF, FieldTypes FT WHERE FPF.FieldTypeID=FT.ID AND FPF.FormPageID=" +
                    formID);
            DataView dvFields = new DataView(dsFields.Tables[0], "", "", DataViewRowState.CurrentRows);
            if (dvFields.Count > 0)
            {
                for (int i = 0; i < dvFields.Count; i++)
                {
                    if (Session["Account" + dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString()] != null)
                    {
                        TextBox textbox = (TextBox)FindControl(dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString());
                        textbox.Text = Session["Account" + dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString()].ToString();
                    }
                }
            }
        }

        protected void ContinueShipping(object sender, EventArgs e)
        {

            try
            {
                if (Page.IsValid)
                {
                    Session["FormClicked"] = true;

                    bool isStateValid = false;
                    if (StateLabel3RadComboBox.Visible)
                    {
                        if (StateLabel3RadComboBox.SelectedValue != "-1")
                            isStateValid = true;
                    }
                    else
                    {
                        if (StateLabel3TextBox.Text.Trim() != "")
                            isStateValid = true;
                    }

                    if (isStateValid)
                    {
                        if (Session["UserID"] != null)
                        {

                            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                            Customer cust = client.GetCustomer(Session["UserID"].ToString());

                            Address adr = new Address();
                            adr.DefaultShipping = true;
                            adr.DefaultBilling = false;
                            adr.DefaultDelivery = false;
                            adr.DefaultAccount = false;

                            Session["formpage"] = "4";

                            Session["ShippingNameLabel"] = CompanyLabel3TextBox.Text;
                            ShippingNameLabel.Text = CompanyLabel3TextBox.Text;


                            Session["ShippingCompanyLabel"] = AttentionLabel2TextBox.Text;
                            ShippingCompanyLabel.Text = AttentionLabel2TextBox.Text;
                            adr.CompanyName = CompanyLabel3TextBox.Text;

                            Session["ShippingResidentialLabel"] = ResidentialCheckBox3.Checked;
                            adr.Residential = ResidentialCheckBox3.Checked;

                            ShippingStateLabel.Text = CityLabel3TextBox.Text;
                            adr.City = CityLabel3TextBox.Text;
                            string state = "";
                            if (StateLabel3RadComboBox.Visible)
                                state = StateLabel3RadComboBox.SelectedItem.Text;
                            else
                                state = StateLabel3TextBox.Text;
                            ShippingStateLabel.Text += ", " + state + " " + ZipLabel3TextBox.Text;
                            Session["ShippingStateLabel"] = state;
                            adr.State = state;

                            Session["ShippingPhoneLabel"] = PhoneLabel3TextBox.Text;
                            Session["ShippingEx"] = ex3TextBox.Text;

                            ShippingPhoneLabel.Text = PhoneLabel3TextBox.Text;
                            adr.Phone = PhoneLabel3TextBox.Text;

                            if (ex3TextBox.Text.Trim() != "")
                            {
                                ShippingPhoneLabel.Text = ShippingPhoneLabel.Text + " x" + ex3TextBox.Text;
                                adr.Phone = adr.Phone + " x" + ex3TextBox.Text;
                            }


                            Session["ShippingCityLabel"] = CityLabel3TextBox.Text;
                            adr.City = CityLabel3TextBox.Text;

                            Session["ShippingAttentionLabel"] = AttentionLabel2TextBox.Text;
                            adr.Attention = AttentionLabel2TextBox.Text;

                            Session["ShippingAddress2Label"] = Address2Label3TextBox.Text;
                            adr.Address2 = Address2Label3TextBox.Text;

                            Session["ShippingAddressLabel"] = Address1Label3TextBox.Text;
                            adr.Address1 = Address1Label3TextBox.Text;

                            Session["ShippingZipLabel"] = ZipLabel3TextBox.Text;
                            adr.Zip = ZipLabel3TextBox.Text;

                            Session["ShippingCountryLabel"] = CountryLabel3RadComboBox.SelectedValue;
                            DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_name = '" +
                            CountryLabel3RadComboBox.SelectedItem.Text + "'");
                            adr.Country = dsCount.Tables[0].Rows[0]["country_2_code"].ToString();


                            object refAdrs = (object)adr;
                            SaveDynamicFields("3", ref refAdrs);
                            adr = (Address)refAdrs;

                            UpdateAddress(adr, ref cust, AddressTypes.Shipping);

                            client.UpdateCustomer(cust);

                            cust = client.GetCustomer(Session["UserID"].ToString());

                            FillDelivery();
                            if (Session["EditShippingClicked"] != null)
                                if (bool.Parse(Session["EditShippingClicked"].ToString()))
                                {
                                    Session["EditShippingClicked"] = "false";
                                    GoToForm(6, 3);
                                    SetBoxes();
                                    BuildIt(client, cust);
                                }
                                else
                                    GoToForm(4, 3);
                            else
                                GoToForm(4, 3);
                        }
                    }
                    else
                    {
                        ErrorLabelShipping.Text = "State is required";
                    }
                }
                else
                {
                    ErrorLabel.Text = "page is not valid";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }

        protected void ShippingBack(object sender, EventArgs e)
        {
            Session["EditShippingClicked"] = "false";
            Session["EditBillingClicked"] = "false";
            Session["EditDeliveryClicked"] = "false";
            Session["EditPaymentClicked"] = "false";

            Session["formpage"] = "2";
            GoToForm(2, 3);

            FillBilling();
        }

        protected void ContinueBilling(object sender, EventArgs e)
        {
            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();

            Customer cust = client.GetCustomer(Session["UserID"].ToString());
            ContBilling(false, true, client, ref cust);
        }

        protected void ContBilling(bool fromPayment, bool validate, Explore.NetSuite.DataAccess.NDAL client, ref Customer cust)
        {
            try
            {
                if (!fromPayment)
                {
                    Session["RedrawForm"] = false;
                }

                bool goOn = true;
                if (validate)
                {
                    if (!Page.IsValid)
                    {
                        goOn = false;
                    }
                }

                if (goOn)
                {
                    if (!fromPayment)
                    {
                        Session["FormClicked"] = true;
                    }
                    bool isStateValid = false;
                    if (StateLabel2RadComboBox.Visible)
                    {
                        if (StateLabel2RadComboBox.SelectedValue != "-1")
                            isStateValid = true;
                    }
                    else
                    {
                        if (StateLabel2TextBox.Text.Trim() != "")
                            isStateValid = true;
                    }
                            
                    if (isStateValid)
                    {
                        if (Session["UserID"] != null)
                        {
                            

                            //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //  "info",
                            //  "---SaveBilling address function GetCustomer() Begin------"));
                            
                            //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //  "info",
                            //  "---SaveBilling address function GetCustomer() End------"));

                            Address adr = new Address();
                            adr.DefaultBilling = true;
                            adr.DefaultShipping = false;
                            adr.DefaultDelivery = false;
                            adr.Residential = false;
                            adr.DefaultAccount = false;

                            if (!fromPayment)
                            {
                                Session["RedrawForm"] = true;
                                Session["formpage"] = "3";
                            }

                            Session["BillingNameLabel"] = AttentionLabel1TextBox.Text;
                            BillingNameLabel.Text = AttentionLabel1TextBox.Text;


                            Session["BillingCompanyLabel"] = CompanyLabel2TextBox.Text;
                            BillingCompanyLabel.Text = CompanyLabel2TextBox.Text;
                            adr.CompanyName = CompanyLabel2TextBox.Text;

                            string state = "";
                            if (StateLabel2RadComboBox.Visible)
                                state = StateLabel2RadComboBox.SelectedItem.Text;
                            else
                                state = StateLabel2TextBox.Text;

                            Session["BillingStateLabel"] = state;
                            BillingStateLabel.Text = CityLabel2TextBox.Text + ", " + state + " " + ZipLabel2TextBox.Text;
                            adr.State = state;

                            Session["BillingCityLabel"] = CityLabel2TextBox.Text;
                            adr.City = CityLabel2TextBox.Text;

                            Session["BillingPhoneLabel"] = PhoneLabel2TextBox.Text;
                            Session["BillingEx"] = ex2TextBox.Text;
                            BillingPhoneLabel.Text = PhoneLabel2TextBox.Text;
                            adr.Phone = PhoneLabel2TextBox.Text;
                            if (ex2TextBox.Text.Trim() != "")
                            {
                                BillingPhoneLabel.Text = PhoneLabel2TextBox.Text + " x" + ex2TextBox.Text;
                                adr.Phone = PhoneLabel2TextBox.Text + " x" + ex2TextBox.Text;
                            }
                            
                            Session["BillingAttentionLabel"] = AttentionLabel1TextBox.Text;
                            adr.Attention = AttentionLabel1TextBox.Text;

                            Session["BillingAddressLabel"] = Address1Label2TextBox.Text;
                            adr.Address1 = Address1Label2TextBox.Text;

                            Session["BillingAddress2Label"] = Address2Label2TextBox.Text;
                            adr.Address2 = Address2Label2TextBox.Text;

                            Session["BillingZipLabel"] = ZipLabel2TextBox.Text;
                            adr.Zip = ZipLabel2TextBox.Text;

                            Session["BillingCountryLabel"] = CountryLabel2RadComboBox.SelectedValue;
                            DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_name = '" +
                            CountryLabel2RadComboBox.SelectedItem.Text + "'");
                            adr.Country = dsCount.Tables[0].Rows[0]["country_2_code"].ToString();

                            object refAdrs = (object)adr;
                            SaveDynamicFields("2", ref refAdrs);
                            adr = (Address)refAdrs;

                            

                            UpdateAddress(adr, ref cust, AddressTypes.Billing);

                            //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //  "info",
                            //  "---SaveBilling address function UpdateCustomer() Begin------"));
                            //
                            //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //  "info",
                            //  "---SaveBilling address function UpdateCustomer() End------"));

                            client.UpdateCustomer(cust);

                            SamesAsCheckBox2.Checked = false;

                            if (!fromPayment)
                            {
                                FillShipping();
                                if (Session["EditBillingClicked"] != null)
                                    if (bool.Parse(Session["EditBillingClicked"].ToString()))
                                    {
                                        
                                        
                                        Session["formpage"] = "6";
                                        GoToForm(6, 2);
                                        SetBoxes();
                                        BuildIt(client, cust);
                                    }
                                    else
                                        GoToForm(3, 2);
                                else
                                    GoToForm(3, 2);
                            }
                        }
                        else
                        {
                            ErrorLabel.Text = "user is null";
                        }
                    }
                    else
                    {
                        ErrorLabelBilling.Text = "State is required.";
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }

        protected void FillCheckOutBilling(Customer cust)
        {
            if (Session["UserID"] != null)
            {
                HttpContext.Current.Trace.Warn("FormPage_FillCheckOutBilling()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Check out Begin------"));
                Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();


                

                List<Address> addresses = cust.Addresses;

                Address address = new Address();

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultBilling.Value)
                    {
                        address = addresses[i];
                    }
                }

                BillingAddressLabel.Text = address.Address1;
                BillingAddress2Label.Text = address.Address2;
                BillingNameLabel.Text = address.Attention;

                BillingCompanyLabel.Text = address.CompanyName;

                BillingStateLabel.Text = address.City + ", " + address.State + " " + address.Zip;

                BillingPhoneLabel.Text = address.Phone;

            }
        }

        protected void FillCheckOutShipping(Customer cust)
        {
            if (Session["UserID"] != null)
            {

                List<Address> addresses = cust.Addresses;

                Address address = new Address();

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultShipping.Value)
                    {
                        address = addresses[i];
                    }
                }

                ShippingAddressLabel.Text = address.Address1;
                ShippingAddress2Label.Text = address.Address2;
                ShippingNameLabel.Text = address.Attention;

                ShippingCompanyLabel.Text = address.CompanyName;

                ShippingStateLabel.Text = address.City + ", " + address.State + " " + address.Zip;

                ShippingPhoneLabel.Text = address.Phone;

            }
        }

        protected void FillCheckOutDelivery(Customer cust)
        {
            if (Session["UserID"] != null)
            {

                List<Address> addresses = cust.Addresses;

                Address address = new Address();

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultDelivery.Value)
                    {
                        address = addresses[i];
                    }
                }

                DeliveryAddressLabel.Text = address.Address1;
                DeliveryAddress2Label.Text = address.Address2;
                DeliveryNameLabel.Text = address.Attention;

                DeliveryCompanyLabel.Text = address.CompanyName;

                DeliveryStateLabel.Text = address.City + ", " + address.State + " " + address.Zip;

                DeliveryPhoneLabel.Text = address.Phone;

            }
        }

        protected void FillCheckOutPayment()
        {
            if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
            {

                CreditCardInfoPanel.Visible = true;
                POPanel.Visible = false;
                CreditCardExpirationLabel.Text = Session["CreditExpirationLabel"].ToString();

                CreditCardNumberLabel.Text = "";
                if (Session["CreditCardLabel"].ToString().Length > 4)
                {
                    int count = 0;

                    while (count < Session["CreditCardLabel"].ToString().Length - 4)
                    {
                        CreditCardNumberLabel.Text += "x";
                        count++;
                    }
                    CreditCardNumberLabel.Text += Session["CreditCardLabel"].ToString().Substring(Session["CreditCardLabel"].ToString().Length - 4, 4);
                }
                else
                {
                    CreditCardNumberLabel.Text = Session["CreditCardLabel"].ToString();
                }

            }
            else
            {
                CreditCardInfoPanel.Visible = false;
                POPanel.Visible = true;
                POTextBox.Text = Session["POExpirationLabel"].ToString();
            }
        }

        protected void BillingBack(object sender, EventArgs e)
        {
            Session["EditShippingClicked"] = "false";
            Session["EditBillingClicked"] = "false";
            Session["EditDeliveryClicked"] = "false";

            Session["HasUserBeenClicked"] = null;
            Session.Remove("HasUserBeenClicked");

            Session["AccountBackClicked"] = true;

            Static1Messages.Text = "";
            Session["formpage"] = "1";
            GoToForm(1, 2);

            FillAccount();
        }

        protected void ContinueDelivery(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Session["FormClicked"] = true;

                bool isStateValid = false;
                if (StateLabel4RadComboBox.Visible)
                {
                    if (StateLabel4RadComboBox.SelectedValue != "-1")
                        isStateValid = true;
                }
                else
                {
                    if (StateLabel4TextBox.Text.Trim() != "")
                        isStateValid = true;
                }

                if (isStateValid)
                {
                    if (Session["UserID"] != null)
                    {
                        Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                        Customer cust = client.GetCustomer(Session["UserID"].ToString());

                        Address adr = new Address();
                        adr.DefaultBilling = false;
                        adr.DefaultShipping = false;
                        adr.DefaultDelivery = true;
                        adr.DefaultAccount = false;

                        Session["formpage"] = "5";

                        Session["DeliveryNameLabel"] = CompanyLabel4TextBox.Text;
                        DeliveryNameLabel.Text = CompanyLabel4TextBox.Text;


                        Session["DeliveryCompanyLabel"] = AttentionLabel3TextBox.Text;
                        DeliveryCompanyLabel.Text = AttentionLabel3TextBox.Text;
                        adr.CompanyName = CompanyLabel4TextBox.Text;

                        DeliveryStateLabel.Text = CityLabel4TextBox.Text;
                        adr.City = CityLabel4TextBox.Text;
                        string state = "";
                        if (StateLabel4RadComboBox.Visible)
                            state = StateLabel4RadComboBox.SelectedItem.Text;
                        else
                            state = StateLabel4TextBox.Text;
                        DeliveryStateLabel.Text += ", " + state + " " + ZipLabel3TextBox.Text;
                        adr.State = state;

                        Session["DeliveryStateLabel"] = state;

                        Session["DeliveryPhoneLabel"] = PhoneLabel4TextBox.Text;
                        Session["DeliveryEx"] = ex4TextBox.Text;

                        DeliveryPhoneLabel.Text = PhoneLabel4TextBox.Text;
                        adr.Phone = PhoneLabel4TextBox.Text;

                        if (ex4TextBox.Text.Trim() != "")
                        {
                            DeliveryPhoneLabel.Text = DeliveryPhoneLabel.Text + " x" + ex4TextBox.Text;
                            adr.Phone = adr.Phone + " x" + ex4TextBox.Text;
                        }

                        Session["DeliveryCityLabel"] = CityLabel4TextBox.Text;

                        Session["DeliveryResidential"] = ResidentialCheckBox3.Checked;
                        adr.Residential = ResidentialCheckBox3.Checked;

                        Session["DeliveryAttentionLabel"] = AttentionLabel3TextBox.Text;
                        adr.Attention = AttentionLabel3TextBox.Text;

                        Session["DeliveryAddress2Label"] = Address2Label4TextBox.Text;
                        adr.Address2 = Address2Label4TextBox.Text;

                        Session["DeliveryAddressLabel"] = Address1Label4TextBox.Text;
                        adr.Address1 = Address1Label4TextBox.Text;

                        Session["DeliveryZipLabel"] = ZipLabel4TextBox.Text;
                        adr.Zip = ZipLabel4TextBox.Text;

                        Session["DeliveryCountryLabel"] = CountryLabel4RadComboBox.SelectedValue;
                        DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_name = '" +
                            CountryLabel4RadComboBox.SelectedItem.Text + "'");
                        adr.Country = dsCount.Tables[0].Rows[0]["country_2_code"].ToString();

                        

                        object refAdrs = (object)adr;
                        SaveDynamicFields("4", ref refAdrs);
                        adr = (Address)refAdrs;

                        UpdateAddress(adr, ref cust, AddressTypes.Delivery);
                        client.UpdateCustomer(cust);
                        FillPayment();
                        if (Session["EditDeliveryClicked"] != null)
                            if (bool.Parse(Session["EditDeliveryClicked"].ToString()))
                            {
                                Session["formpage"] = "5";
                                GoToForm(6, 4);
                                SetBoxes();
                                BuildIt(client, cust);
                            }
                            else
                            {
                                //Session["PaymentCookie"] = "CreditCardPanel";


                                //CreditCardPanel.Visible = true;
                                //PurchaseOrderPanel.Visible = false;
                                
                                GoToForm(5, 4);

                            }
                        else
                        {
                            //Session["PaymentCookie"] = "CreditCardPanel";


                            //CreditCardPanel.Visible = true;
                            //PurchaseOrderPanel.Visible = false;
                            GoToForm(5, 4);
                        }

                        //((Panel)FindControl(Session["PaymentCookie"].Value.ToString())).Visible = true;
                    }
                }
                else
                {
                    ErrorLabelDelivery.Text = "State is required";
                }
            }
            else
            {
                ErrorLabel.Text = "page is not valid";
               // Response.Redirect("UserLogin.aspx");
            }
        }

        protected void DeliveryBack(object sender, EventArgs e)
        {
            Session["EditShippingClicked"] = "false";
            Session["EditBillingClicked"] = "false";
            Session["EditDeliveryClicked"] = "false";
            Session["EditPaymentClicked"] = "false";

            Session["formpage"] = "1";
            GoToForm(3, 4);

            FillShipping();
        }

        protected void ContinuePayment(object sender, EventArgs e)
        {
            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();

            Customer cust = client.GetCustomer(Session["UserID"].ToString());


            try
            {
                if (Page.IsValid)
                {
                    //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                    //          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //          "info",
                    //          "---NDAL create client Begin------"));
                    //Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                    //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                    //         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //         "info",
                    //         "---NDAL create client End------"));

                    //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                    //         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //         "info",
                    //         "---GetCustomer() Begin------"));
                    //Customer cust = client.GetCustomer(Session["UserID"].ToString());
                    //HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                    //         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    //         "info",
                    //         "---GetCustomer() End------"));


                    bool continBill = true;
                    if (Session["UserID"] != null)
                    {
                        if (Session["PaymentCookie"] == null)
                        {

                            if (CreditCardPanel.Visible)
                            {
                                Session["PaymentCookie"] = "CreditCardPanel";
                            }
                            else
                            {
                                Session["PaymentCookie"] = "PurchaseOrderPanel";
                            }

                        }

                        if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
                        {
                            if (CreditCardLabelTextBox.Text.Trim().Length > 4)
                            {

                                string expiration = ExpirationDateLabelMonthDropDown.SelectedItem.Value + "/" +
                                    ExpirationDateLabelYearDropDown.SelectedItem.Text;



                                if (DateTime.Parse(ExpirationDateLabelMonthDropDown.SelectedItem.Value + "/" +
                                    DateTime.DaysInMonth(int.Parse(ExpirationDateLabelYearDropDown.SelectedItem.Text),
                                    int.Parse(ExpirationDateLabelMonthDropDown.SelectedItem.Value)).ToString()
                                    + "/" +
                                    ExpirationDateLabelYearDropDown.SelectedItem.Text) >= DateTime.Now.Date)
                                {
                                    //First check whether a credit card already exists 1/13/2010

                             //       HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                             //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             //"info",
                             //"---Get customer cards Begin------"));
                                    List<CreditCard> cardCheck = cust.CreditCards;
                            //        HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //"info",
                            //"---Get customer cards End------"));

                                    CardTypes thisCardType;

                                    switch (CreditCard2TypeDropDown.SelectedItem.Text)
                                    {
                                        case "Discover":
                                            thisCardType = CardTypes.Discover;
                                            break;
                                        case "Visa":
                                            thisCardType = CardTypes.Visa;
                                            break;
                                        case "MasterCard":
                                            thisCardType = CardTypes.MasterCard;
                                            break;
                                        default:
                                            thisCardType = CardTypes.MasterCard;
                                            break;
                                    }



                                    bool foundCard = false;

                                    string cardInternalID = "";

                                    CreditCard cardToUse = new CreditCard();

                                    foreach (CreditCard cardo in cardCheck)
                                    {

                                        if (cardo.CardType == thisCardType &&
                                            cardo.CardNumber == CreditCardLabelTextBox.Text.Trim().Substring(CreditCardLabelTextBox.Text.Trim().Length - 5, 4)
                                            && cardo.ExpirationDate == expiration &&
                                            cardo.CardholderName == CreditCardNameLabelTextBox.Text.Trim())
                                        {
                                            foundCard = true;
                                            cardInternalID = cardo.InternalID;
                                            cardToUse = cardo;
                                            Session["CardToUser"] = cardToUse.InternalID;
                                        }
                                    }

                                    if (!foundCard)
                                    {

                                        cardToUse = new CreditCard();

                                        cardToUse.CardholderName = CreditCardNameLabelTextBox.Text;
                                        cardToUse.CardNumber = CreditCardLabelTextBox.Text;

                                        switch (CreditCard2TypeDropDown.SelectedItem.Text)
                                        {
                                            case "Discover":
                                                cardToUse.CardType = CardTypes.Discover;
                                                break;
                                            case "Visa":
                                                cardToUse.CardType = CardTypes.Visa;
                                                break;
                                            case "MasterCard":
                                                cardToUse.CardType = CardTypes.MasterCard;
                                                break;
                                            default: break;
                                        }

                                        cardToUse.DefaultCreditCard = true;

                                        cardToUse.ExpirationDate = expiration;
                                    }

                                    object refAdrs = (object)cardToUse;
                                    SaveDynamicFields("5", ref refAdrs);
                                    cardToUse = (CreditCard)refAdrs;

                                    if (!foundCard)
                                    {
                                        if (cust.CreditCards == null)
                                        {
                                            List<CreditCard> cards = new List<CreditCard>();
                                            cards.Add(cardToUse);
                                            cust.CreditCards = cards;
                                        }
                                        else
                                        {
                                            cust.CreditCards.Add(cardToUse);
                                        }

                                        //continBill = false;
                            //            HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //"info",
                            //"---UpdateCustomer Begin------"));
                                        client.UpdateCustomer(cust);
                            //            HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                            //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            //"info",
                            //"---UpdateCustomer End------"));

                                        //continBill = true;
                              //          HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                              //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              //"info",
                              //"---GetCustomer() Begin------"));



                                        //***************GETTING RID OF THIS LINE TO OPTIMIZE CODE **************************
                                        Customer custer = client.GetCustomer(Session["UserID"].ToString());


                              //          HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                              //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              //"info",
                              //"---GetCustomer() End------"));

                              //          HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                              //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              //"info",
                              //"---retrieve customer CreditCards Begin------"));
                                        List<CreditCard> cardsThis = custer.CreditCards;
                              //          HttpContext.Current.Trace.Warn("FormPage_ContinuePayment()", string.Format("{0},{1},{2}",
                              //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              //"info",
                              //"---retrieve customer CreditCards End------"));

                                        foreach (CreditCard c in cardsThis)
                                        {
                                            if (c.DefaultCreditCard.Value)
                                                Session["CardToUser"] = c.InternalID;
                                        }
                                    }



                                    CreditCardInfoPanel.Visible = true;
                                    POPanel.Visible = false;
                                    CreditCardExpirationLabel.Text = expiration;

                                    CreditCardNumberLabel.Text = "";
                                    if (CreditCardLabelTextBox.Text.Length > 4)
                                    {
                                        int count = 0;

                                        while (count < CreditCardLabelTextBox.Text.Length - 4)
                                        {
                                            CreditCardNumberLabel.Text += "x";
                                            count++;
                                        }
                                        CreditCardNumberLabel.Text += CreditCardLabelTextBox.Text.Substring(CreditCardLabelTextBox.Text.Length - 4, 4);
                                    }
                                    else
                                    {
                                        CreditCardNumberLabel.Text = CreditCardLabelTextBox.Text;
                                    }

                                    Session["CreditCardType"] = CreditCard2TypeDropDown.SelectedItem.Text;

                                    Session["CreditExpirationLabel"] = expiration;

                                    Session["CreditCardLabel"] = CreditCardLabelTextBox.Text;

                                    Session["CreditCSVLabel"] = CSVLabelTextBox.Text;

                                    Session["CreditName"] = CreditCardNameLabelTextBox.Text;

                                    Session["formpage"] = "6";
                                }
                                else
                                {
                                    continBill = false;
                                    Session["formpage"] = "5";
                                    Static5Message.Text = "Expiration date must be greater than today.";
                                }
                            }
                            else
                            {
                                Static5Message.Text = "The Credit Card number you have entered is invalid";
                            }
                        }
                        else
                        {
                            
                            Session["formpage"] = "6";
                            CreditCardInfoPanel.Visible = false;
                            POPanel.Visible = true;
                            POTextBox.Text = PurchaseOrderLabelTextBox.Text;

                            Session["POExpirationLabel"] = PurchaseOrderLabelTextBox.Text;

                            //Session["PaymentCompanyLabel"] = CompanyLabel5TextBox.Text;

                            //Session["PaymentAttentionLabel"] = AttentionLabel5TextBox.Text;

                            //string state = "";
                            //if (StateLabel5RadComboBox.Visible)
                            //    state = StateLabel5RadComboBox.SelectedItem.Text;
                            //else
                            //    state = StateLabel5TextBox.Text;

                            //Session["PaymentStateLabel"] = state;

                            //Session["PaymentCityLabel"] = CityLabel5TextBox.Text;

                            //Session["PaymentPhoneLabel"] = PhoneLabel5TextBox.Text;
                            //Session["PaymentEx"] = ex4TextBox.Text;

                            //Session["PaymentAddressLabel"] = Address1Label5TextBox.Text;

                            //Session["PaymentAddress2Label"] = Address2Label5TextBox.Text;

                            //Session["PaymentZipLabel"] = ZipLabel5TextBox.Text;

                            //Session["PaymentCountryLabel"] = CountryLabel5RadComboBox.SelectedValue;

                        }
                        if (continBill)
                        {
                           
                            
                            //Session["FormClicked"] = true;
                            ContBilling(true, false, client, ref cust);
                            client.UpdateCustomer(cust);
                            Session["formpage"] = "6";
                            Session["RedrawForm"] = "True";
                            //Response.Redirect("Account+Creation.aspx");
                            GoToForm(6, 5);
                            BuildIt(client, cust);
                        }
                        else
                        {
                            Session["formpage"] = "5";
                            //Static5Message.Text = "somthing happened";
                            GoToForm(5, 6);
                            BuildIt(client, cust);
                        }
                    }
                    else
                    {
                        ErrorLabel.Text = "user is null";
                        Response.Redirect("UserLogin.aspx");
                    }
                }
                else
                {
                    Session["formpage"] = "5";
                    GoToForm(5, 6);
                    BuildIt(client, cust);
                }
            }
            catch (CommandErrorException ex)
            {
                if (ex.ToString().Contains("for the following field: ccnumber"))
                {
                    Static5Message.Text = "The Credit Card number you have entered is invalid";
                }
                else
                {
                    Static5Message.Text = ex.ToString();
                }

                Session["formpage"] = "5";
                GoToForm(5, 6);
                BuildIt(client, cust);
            }
            catch (Exception ex)
            {
                Static5Message.Text = ex.ToString();

                Session["formpage"] = "5";
                GoToForm(5, 6);
                BuildIt(client, cust);
            }
        }

        protected void ChangePayment(object sender, EventArgs e)
        {
            if (PaymentTypeLabelRadComboBox.SelectedValue == "0")
            {
                CreditCardPanel.Visible = true;
                PurchaseOrderPanel.Visible = false;
                Session["PaymentCookie"] = "CreditCardPanel";
            }
            else
            {
                CreditCardPanel.Visible = false;
                PurchaseOrderPanel.Visible = true;
                Session["PaymentCookie"] = "PurchaseOrderPanel";

            }
        }

        protected void PaymentBack(object sender, EventArgs e)
        {
            Session["EditShippingClicked"] = "false";
            Session["EditBillingClicked"] = "false";
            Session["EditDeliveryClicked"] = "false";
            Session["EditPaymentClicked"] = "false";

            Session["formpage"] = "1";
            GoToForm(4, 5);

            FillDelivery();
        }

        protected void ReviewBack(object sender, EventArgs e)
        {
            Session["formpage"] = "1";
            GoToForm(5, 6);

            FillPayment();
        }

        protected void SameAs(string copyTo, string copyFrom, bool check)
        {
            string message = "";
            try
            {
                Session["RedrawForm"] = false;

                string prefix = "";
                string temp = copyTo;
                if (temp == "1")
                    temp = "";
                TextBox theEx = (TextBox)FindControl("ex" + temp + "TextBox");

                switch (copyFrom)
                {
                    case "1":
                        prefix = "Account";
                        break;
                    case "2":
                        prefix = "Billing";
                        break;
                    case "3":
                        prefix = "Shipping";
                        break;
                    case "4":
                        prefix = "Delivery";
                        break;
                    default: break;
                }



                if (check)
                {
                    Address address = new Address();
                    if (Session["UserID"] != null)
                    {
                        Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                        Customer cust = client.GetCustomer(Session["UserID"].ToString());


                        for (int i = 0; i < cust.Addresses.Count; i++)
                        {
                            if (copyFrom == "1")
                            {
                                if (cust.Addresses[i].DefaultAccount.Value)
                                {
                                    address = cust.Addresses[i];
                                    break;
                                }
                            }
                            else if (copyFrom == "2")
                            {
                                if (cust.Addresses[i].DefaultBilling.Value)
                                {
                                    address = cust.Addresses[i];
                                    break;
                                }
                                prefix = "Billing";
                            }
                            else if (copyFrom == "3")
                            {
                                if (cust.Addresses[i].DefaultShipping.Value)
                                {
                                    address = cust.Addresses[i];
                                    break;
                                }
                                prefix = "Shipping";
                            }
                            else if (copyFrom == "4")
                            {
                                if (cust.Addresses[i].DefaultDelivery.Value)
                                {
                                    address = cust.Addresses[i];
                                    break;
                                }
                                prefix = "Delivery";
                            }
                        }

                    }

                    if (address != null)
                    {

                        message += "address: " + address.Address1;
                        DataSet dsCopyTo = doQuery("SELECT * FROM FormStaticPageFields WHERE FormPageID=" + copyTo);
                        DataView dvCopyTo = new DataView(dsCopyTo.Tables[0], "", "", DataViewRowState.CurrentRows);

                        //Company
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Company%'";

                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.CompanyName;

                        //Attention
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Attention%'";
                        
                            ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = 
                                address.Attention;
                        

                        //Address 1
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Address1%'";
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.Address1;

                        //Address 2
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Address2%'";
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.Address2;

                        //City
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%City%'";
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.City;

                        //Country
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Country%'";
                        DataSet dsCountries = doQuery("SELECT * FROM countries");
                        RadComboBox b = (RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox");
                        b.DataSource = dsCountries;
                        b.DataTextField = "country_name";
                        b.DataValueField = "country_id";
                        b.DataBind();
                        
                        message += " Country: ";
                        string str = address.Country + ", " + address.State;
                        message += "country: " + str;
                        DataSet dsC = doQuery("SELECT * FROM Countries WHERE country_2_code = '" + address.Country.ToUpper().Trim() + "'");
                        if (b != null)
                            b.Items.FindItemByText(dsC.Tables[0].Rows[0]["country_name"].ToString()).Selected = true;

                        //State
                        DataSet dsStates = doQuery("SELECT * FROM States S, Countries C WHERE C.country_2_code='" +
                            address.Country + "' AND C.country_id=S.country_id");

                        if (address.Country != "US")
                        {
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxInternational")).Visible = true;
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxLocal")).Visible = false;
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxLocal2")).Visible = false;
                        }
                        else
                        {
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxInternational")).Visible = false;
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxLocal")).Visible = true;
                            ((Panel)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBoxLocal2")).Visible = true;
                        }

                        DataView dvStates = new DataView(dsStates.Tables[0], "", "", DataViewRowState.CurrentRows);

                        dvCopyTo.RowFilter = "LabelFormName LIKE '%State%'";

                        RadComboBox theBox = ((RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox"));

                        if (dvStates.Count > 0)
                        {
                            theBox.Visible = true;
                            theBox.DataSource = dvStates;
                            theBox.DataTextField = "state_name";
                            theBox.DataValueField = "state_id";
                            theBox.DataBind();

                            theBox.Items.FindItemByText(address.State.Trim()).Selected = true;
                            //((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Visible = false;
                        }
                        else
                        {
                            theBox.Visible = false;
                            ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Visible = true;
                            ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.State;
                        }


                        //Zip
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Zip%'";
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.Zip;

                        //Phone
                        char[] delim = { 'x' };
                        string[] toks = address.Phone.Split(delim);



                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Phone%'";

                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = address.Phone;

                        if (toks.Length > 0)
                        {
                            if (toks.Length >= 2)
                            {
                                theEx.Text = toks[1];
                            }

                            if (toks.Length >= 1)
                                ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = toks[0];
                        }




                        dvCopyTo.RowFilter = "LabelFormName LIKE '%CompanyLabel%'";
                        CheckBox theCheck = ((CheckBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "CheckBox"));
                        if (theCheck != null)
                        {

                            theCheck.Checked = address.Residential.Value;
                        }

                    }
                    else
                    {
                        DataSet dsCopyFrom = doQuery("SELECT * FROM FormStaticPageFields WHERE FormPageID=" + copyFrom);
                        DataView dvCopyFrom = new DataView(dsCopyFrom.Tables[0], "", "", DataViewRowState.CurrentRows);

                        DataSet dsCopyTo = doQuery("SELECT * FROM FormStaticPageFields WHERE FormPageID=" + copyTo);
                        DataView dvCopyTo = new DataView(dsCopyTo.Tables[0], "", "", DataViewRowState.CurrentRows);

                        //Company
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Company%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Company%'";

                        TextBox textAddress2 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress2.Text;

                        //Attention
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Attention%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Attention%'";
                        if (dvCopyFrom.Count > 0 && dvCopyTo.Count > 0)
                        {
                            TextBox textAddress12 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                            if (textAddress12 != null)
                                ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress12.Text;
                        }

                        //Address 1
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Address1%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Address1%'";
                        TextBox textAddress = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress.Text;

                        //Address 2
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Address2%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Address2%'";
                        TextBox textAddress3 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress3.Text;

                        //City
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%City%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%City%'";
                        TextBox textAddress4 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress4.Text;

                        //State
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%State%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%State%'";
                        TextBox textAddress7 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        if (textAddress7.Visible)
                            ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress7.Text;
                        else
                        {
                            RadComboBox textAddress9 = (RadComboBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "RadComboBox");
                            ((RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox")).SelectedValue = textAddress9.SelectedValue;
                        }

                        //Country
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Country%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Country%'";
                        RadComboBox textAddress8 = (RadComboBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "RadComboBox");
                        ((RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox")).SelectedValue = textAddress8.SelectedValue;

                        //Zip
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Zip%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Zip%'";
                        TextBox textAddress5 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress5.Text;

                        //Phone
                        dvCopyFrom.RowFilter = "LabelFormName LIKE '%Phone%'";
                        dvCopyTo.RowFilter = "LabelFormName LIKE '%Phone%'";
                        TextBox textAddress6 = (TextBox)FindControl(dvCopyFrom[0]["LabelFormName"].ToString() + "TextBox");
                        ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = textAddress6.Text;
                    }
                }
                else
                {
                    DataSet dsCopyTo = doQuery("SELECT * FROM FormStaticPageFields WHERE FormPageID=" + copyTo);
                    DataView dvCopyTo = new DataView(dsCopyTo.Tables[0], "", "", DataViewRowState.CurrentRows);

                    //Company
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Company%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";

                    //Address 1
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Address1%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";

                    //Address 2
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Address2%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";

                    //City
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%City%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";

                    //State
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%State%'";
                    ((RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox")).SelectedIndex = -1;

                    //Country
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Country%'";
                    ((RadComboBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "RadComboBox")).SelectedValue = "223";

                    //Zip
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Zip%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";

                    //Phone
                    dvCopyTo.RowFilter = "LabelFormName LIKE '%Phone%'";
                    ((TextBox)FindControl(dvCopyTo[0]["LabelFormName"].ToString() + "TextBox")).Text = "";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = message + ex.ToString();
            }
        }

        protected void SameAsAccount(object sender, EventArgs e)
        {
            Session["RedrawForm"] = true;
            SameAs("2", "1", SamesAsCheckBox2.Checked);
            
        }

        protected void ShippingSameAsAccount(object sender, EventArgs e)
        {
            if (SamesAsCheckBox3.Checked)
                SamesAsCheckBox32.Checked = false;
            SameAs("3", "1", SamesAsCheckBox3.Checked);
        }

        protected void ShippingSameAsBilling(object sender, EventArgs e)
        {
            if (SamesAsCheckBox32.Checked)
                SamesAsCheckBox3.Checked = false;
            SameAs("3", "2", SamesAsCheckBox32.Checked);
        }

        protected void DeliverySameAsAccount(object sender, EventArgs e)
        {
            if (SameAsLabel4CheckBox.Checked)
            {
                SameAsBillingLabel4CheckBox.Checked = false;
                SameAsShippingLabel4CheckBox.Checked = false;
            }
            SameAs("4", "1", SameAsLabel4CheckBox.Checked);
        }

        protected void DeliverySameAsBilling(object sender, EventArgs e)
        {
            if (SameAsBillingLabel4CheckBox.Checked)
            {
                SameAsLabel4CheckBox.Checked = false;
                SameAsShippingLabel4CheckBox.Checked = false;
            }
            SameAs("4", "2", SameAsBillingLabel4CheckBox.Checked);
        }

        protected void DeliverySameAsShipping(object sender, EventArgs e)
        {
            if (SameAsShippingLabel4CheckBox.Checked)
            {
                SameAsLabel4CheckBox.Checked = false;
                SameAsBillingLabel4CheckBox.Checked = false;
            }
            SameAs("4", "3", SameAsShippingLabel4CheckBox.Checked);
        }

        protected void EditBilling(object sender, EventArgs e)
        {
            Session["EditBillingClicked"] = "true";

            GoToForm(2, 6);

            FillBilling();
        }

        protected void EditPayment(object sender, EventArgs e)
        {
            Static5Message.Text = "";
            Session["EditPaymentClicked"] = "true";
            GoToForm(5, 6);

            FillPayment();
        }

        protected void EditShipping(object sender, EventArgs e)
        {
            Session["EditShippingClicked"] = "true";
            GoToForm(3, 6);

            FillShipping();
        }

        protected void EditDelivery(object sender, EventArgs e)
        {
            Session["EditDeliveryClicked"] = "true";
            GoToForm(4, 6);

            FillDelivery();
        }

        protected void EditCart(object sender, EventArgs e)
        {
            Session["EditCartClicked"] = "true";
            Response.Redirect("cart.aspx");
        }

        protected void FillAccount()
        {
            
            try
            {
                DataSet ds;


                if (Session["UserID"] != null)
                {
                    Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                    Customer cust = client.GetCustomer(Session["UserID"].ToString());


                        FirstNameTextBox.Text = cust.FirstName;

                        LastNameTextBox.Text = cust.LastName;

                        AccountDropDown.Items.FindItemByText(GetPriceLevelText(cust.CustomerType)).Selected = true;
                    
                   
       
                        RoleDropDown.Items.FindItemByText(GetJobRoleText(cust.JobRole)).Selected = true;


                        TitleTextBox.Text = cust.Title;


                        CompanyLabelTextBox.Text = cust.CompanyName;

                        Address address = new Address();

                        foreach (Address adr in cust.Addresses)
                        {
                            if (adr.DefaultAccount.Value)
                            {
                                address = adr;
                            }
                        }


                        ResidentialCheckBox.Checked = address.Residential.Value;

                        string country_id = doQuery("SELECT * FROM Countries WHERE country_2_code='" + address.Country + "'").Tables[0].Rows[0]["country_id"].ToString();

                        DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id='" + country_id + "'");

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel1RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel1RadComboBox.DataSource = dsStates;
                            StateLabel1RadComboBox.DataTextField = "state_name";
                            StateLabel1RadComboBox.DataValueField = "state_id";
                            StateLabel1RadComboBox.DataBind();

                            if (Session["AccountStateLabel"] != null)
                                StateLabel1RadComboBox.Items.FindItemByText(address.State).Selected = true;
                        }

                    if (isTextState)
                    {
                            StateLabel1TextBox.Text = address.State;
                    }


                    CityLabelTextBox.Text = address.City;

                    char[] delim = { 'x' };
                    string[] tokens = cust.Phone.Split(delim);
                    PhoneLabelTextBox.Text = tokens[0];

                    if (tokens.Length > 1)
                        exTextBox.Text = tokens[1];

                    Address1LabelTextBox.Text = address.Address1;

                    Address2LabelTextBox.Text = address.Address2;

                    ZipLabelTextBox.Text = address.Zip;

                    EmailTextBox.Text = cust.Email;


                    PasswordTextBox.Text = cust.Password;
                    cPasswordTextBox.Text = cust.Password;
                   

                    SetDynamicFields("1");
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
            }
        }

        protected void FillBilling()
        {
            string message ="";
            try
            {
                if (Session["UserID"] != null)
                {
                    Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                    Customer cust = client.GetCustomer(Session["UserID"].ToString());

                    List<Address> addresses = cust.Addresses;

                    Address billingAddress = new Address();

                    bool billingFound = false;

                    for (int i = 0; i < addresses.Count; i++)
                    {
                        if (addresses[i].DefaultBilling.Value)
                        {
                            billingAddress = addresses[i];
                            billingFound = true;
                            break;
                        }
                    }

                    if (billingFound)
                    {
                        CompanyLabel2TextBox.Text = billingAddress.CompanyName;
                        message += billingAddress.Country;

                        DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_2_code = '" +
                            billingAddress.Country + "'");
                        CountryLabel2RadComboBox.Items.FindItemByText(dsCount.Tables[0].Rows[0]["country_name"].ToString()).Selected = true;

                        DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" +
                            CountryLabel2RadComboBox.SelectedValue);

                        bool isTextState = true;
                        if (dsStates.Tables.Count > 0)
                            if (dsStates.Tables[0].Rows.Count > 0)
                            {
                                StateLabel2RadComboBox.Items.Clear();
                                isTextState = false;
                                StateLabel2RadComboBox.DataSource = dsStates;
                                StateLabel2RadComboBox.DataTextField = "state_name";
                                StateLabel2RadComboBox.DataValueField = "state_id";
                                StateLabel2RadComboBox.DataBind();

                                StateLabel2RadComboBox.Items.FindItemByText(
                                        billingAddress.State).Selected = true;
                            }

                        if (isTextState)
                        {
                            StateLabel2TextBox.Text = billingAddress.State;
                        }

                        CityLabel2TextBox.Text = billingAddress.City;

                        char[] delim = { 'x' };
                        string[] tokensP = billingAddress.Phone.Split(delim);

                        if (tokensP.Length > 0)
                        {

                            if (tokensP.Length >= 1)
                            {
                                PhoneLabel2TextBox.Text = tokensP[0].Trim();
                            }

                            if (tokensP.Length >= 2)
                            {
                                ex2TextBox.Text = tokensP[1];
                            }



                        }

                        AttentionLabel1TextBox.Text = billingAddress.Attention;

                        Address1Label2TextBox.Text = billingAddress.Address1;

                        Address2Label2TextBox.Text = billingAddress.Address2;

                        ZipLabel2TextBox.Text = billingAddress.Zip;

                    }
                }
                else
                {
                    if (Session["BillingNameLabel"] != null)
                        CreditCardNameLabelTextBox.Text = Session["BillingNameLabel"].ToString();

                    if (Session["BillingCompanyLabel"] != null)
                        CompanyLabel2TextBox.Text = Session["BillingCompanyLabel"].ToString();

                    if (Session["BillingCountryLabel"] != null)
                        CountryLabel2RadComboBox.SelectedValue = Session["BillingCountryLabel"].ToString();

                    DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel2RadComboBox.SelectedValue);

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel2RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel2RadComboBox.DataSource = dsStates;
                            StateLabel2RadComboBox.DataTextField = "state_name";
                            StateLabel2RadComboBox.DataValueField = "state_id";
                            StateLabel2RadComboBox.DataBind();

                            if (Session["BillingStateLabel"] != null)
                                StateLabel2RadComboBox.Items.FindItemByText(Session["BillingStateLabel"].ToString()).Selected = true;
                        }

                    if (isTextState)
                    {
                        if (Session["BillingStateLabel"] != null)
                            StateLabel2TextBox.Text = Session["BillingStateLabel"].ToString();
                    }

                    if (Session["BillingCityLabel"] != null)
                        CityLabel2TextBox.Text = Session["BillingCityLabel"].ToString();

                    if (Session["BillingPhoneLabel"] != null)
                        PhoneLabel2TextBox.Text = Session["BillingPhoneLabel"].ToString();

                    if (Session["BillingEx"] != null)
                        ex2TextBox.Text = Session["BillingEx"].ToString();

                    if (Session["BillingAttentionLabel"] != null)
                        AttentionLabel1TextBox.Text = Session["BillingAttentionLabel"].ToString();

                    if (Session["BillingAddressLabel"] != null)
                        Address1Label2TextBox.Text = Session["BillingAddressLabel"].ToString();

                    if (Session["BillingAddress2Label"] != null)
                        Address2Label2TextBox.Text = Session["BillingAddress2Label"].ToString();

                    if (Session["BillingZipLabel"] != null)
                        ZipLabel2TextBox.Text = Session["BillingZipLabel"].ToString();
                }

                SetDynamicFields("2");
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString() + message;
            }
        }

        protected void FillShipping()
        {
            SamesAsCheckBox3.Checked = false;
            SamesAsCheckBox32.Checked = false;

            if (Session["UserID"] != null)
            {
                Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                Customer cust = client.GetCustomer(Session["UserID"].ToString());

                List<Address> addresses = cust.Addresses;

                Address billingAddress = new Address();

                bool addressFound = false;

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultShipping.Value)
                    {
                        billingAddress = addresses[i];
                        addressFound = true;
                        break;
                    }
                }

                if (addressFound)
                {
                    CompanyLabel3TextBox.Text = billingAddress.CompanyName;
                    DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_2_code = '" +
                        billingAddress.Country + "'");
                    CountryLabel3RadComboBox.Items.FindItemByText(dsCount.Tables[0].Rows[0]["country_name"].ToString()).Selected = true;


                    DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel3RadComboBox.SelectedValue);

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel3RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel3RadComboBox.DataSource = dsStates;
                            StateLabel3RadComboBox.DataTextField = "state_name";
                            StateLabel3RadComboBox.DataValueField = "state_id";
                            StateLabel3RadComboBox.DataBind();

                            StateLabel3RadComboBox.Items.FindItemByText(
                                    billingAddress.State).Selected = true;
                        }

                    if (isTextState)
                    {
                        StateLabel3TextBox.Text = billingAddress.State;
                    }

                    CityLabel3TextBox.Text = billingAddress.City;

                    char[] delim = { 'x' };
                    string[] tokensP = billingAddress.Phone.Split(delim);

                    if (tokensP.Length > 0)
                    {

                        if (tokensP.Length >= 1)
                        {
                            PhoneLabel3TextBox.Text = tokensP[0].Trim();
                        }

                        if (tokensP.Length >= 2)
                        {
                            ex3TextBox.Text = tokensP[1];
                        }
                    }

                    AttentionLabel2TextBox.Text = billingAddress.Attention;

                    Address1Label3TextBox.Text = billingAddress.Address1;

                    Address2Label3TextBox.Text = billingAddress.Address2;

                    ZipLabel3TextBox.Text = billingAddress.Zip;

                }
                else
                {
                    //if (Session["ShippingCompanyLabel"] != null)
                    CompanyLabel3TextBox.Text = "";

                    //if (Session["ShippingResidentialLabel"] != null)
                    ResidentialCheckBox3.Checked = false;

                    //if (Session["ShippingCountryLabel"] != null)
                    CountryLabel3RadComboBox.SelectedValue = "";

                    DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel3RadComboBox.SelectedValue);

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel3RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel3RadComboBox.DataSource = dsStates;
                            StateLabel3RadComboBox.DataTextField = "state_name";
                            StateLabel3RadComboBox.DataValueField = "state_id";
                            StateLabel3RadComboBox.DataBind();

                            if (Session["ShippingStateLabel"] != null)
                                StateLabel3RadComboBox.Items.FindItemByText(Session["ShippingStateLabel"].ToString()).Selected = true;
                        }

                    if (isTextState)
                    {
                        //if (Session["ShippingStateLabel"] != null)
                        StateLabel3TextBox.Text = "";
                    }

                    //if (Session["ShippingCityLabel"] != null)
                    CityLabel3TextBox.Text = "";

                    //if (Session["ShippingPhoneLabel"] != null)
                    PhoneLabel3TextBox.Text = "";

                    //if (Session["ShippingEx"] != null)
                    ex3TextBox.Text = "";

                    //if (Session["ShippingAttentionLabel"] != null)
                    AttentionLabel2TextBox.Text = "";

                    //if (Session["ShippingAddress2Label"] != null)
                    Address2Label3TextBox.Text = "";

                    //if (Session["ShippingZipLabel"] != null)
                    ZipLabel3TextBox.Text = "";

                    //if (Session["ShippingAddressLabel"] != null)
                    Address1Label3TextBox.Text = "";
                }
            }
            else
            {
               //if (Session["ShippingCompanyLabel"] != null)
                    CompanyLabel3TextBox.Text = "";

                //if (Session["ShippingResidentialLabel"] != null)
                    ResidentialCheckBox3.Checked = false;

                //if (Session["ShippingCountryLabel"] != null)
                    CountryLabel3RadComboBox.SelectedValue = "";

                DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel3RadComboBox.SelectedValue);

                bool isTextState = true;
                if (dsStates.Tables.Count > 0)
                    if (dsStates.Tables[0].Rows.Count > 0)
                    {
                        StateLabel3RadComboBox.Items.Clear();
                        isTextState = false;
                        StateLabel3RadComboBox.DataSource = dsStates;
                        StateLabel3RadComboBox.DataTextField = "state_name";
                        StateLabel3RadComboBox.DataValueField = "state_id";
                        StateLabel3RadComboBox.DataBind();

                        if (Session["ShippingStateLabel"] != null)
                            StateLabel3RadComboBox.Items.FindItemByText(Session["ShippingStateLabel"].ToString()).Selected = true;
                    }

                if (isTextState)
                {
                    //if (Session["ShippingStateLabel"] != null)
                        StateLabel3TextBox.Text = "";
                }

                //if (Session["ShippingCityLabel"] != null)
                    CityLabel3TextBox.Text = "";

                //if (Session["ShippingPhoneLabel"] != null)
                    PhoneLabel3TextBox.Text = "";

                //if (Session["ShippingEx"] != null)
                    ex3TextBox.Text = "";

                //if (Session["ShippingAttentionLabel"] != null)
                    AttentionLabel2TextBox.Text = "";

                //if (Session["ShippingAddress2Label"] != null)
                    Address2Label3TextBox.Text = "";

                //if (Session["ShippingZipLabel"] != null)
                    ZipLabel3TextBox.Text = "";

                //if (Session["ShippingAddressLabel"] != null)
                    Address1Label3TextBox.Text = "";

                
            }

            SetDynamicFields("3");
        }

        protected void FillDelivery()
        {

            SameAsLabel4CheckBox.Checked = false;
            SameAsBillingLabel4CheckBox.Checked = false;
            SameAsShippingLabel4CheckBox.Checked = false;

            if (Session["UserID"] != null)
            {
                Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                Customer cust = client.GetCustomer(Session["UserID"].ToString());

                List<Address> addresses = cust.Addresses;

                Address billingAddress = new Address();

                bool addressFound = false;

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultDelivery.Value)
                    {
                        billingAddress = addresses[i];
                        addressFound = true;
                        break;
                    }
                }

                if (addressFound)
                {
                    CompanyLabel4TextBox.Text = billingAddress.CompanyName;
                    DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_2_code = '" +
                        billingAddress.Country + "'");
                    CountryLabel4RadComboBox.Items.FindItemByText(dsCount.Tables[0].Rows[0]["country_name"].ToString()).Selected = true;


                    DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel4RadComboBox.SelectedValue);

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel4RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel4RadComboBox.DataSource = dsStates;
                            StateLabel4RadComboBox.DataTextField = "state_name";
                            StateLabel4RadComboBox.DataValueField = "state_id";
                            StateLabel4RadComboBox.DataBind();

                            StateLabel4RadComboBox.Items.FindItemByText(
                                    billingAddress.State).Selected = true;
                        }

                    if (isTextState)
                    {
                        StateLabel4TextBox.Text = billingAddress.State;
                    }

                    CityLabel4TextBox.Text = billingAddress.City;

                    char[] delim = { 'x' };
                    string[] tokensP = billingAddress.Phone.Split(delim);

                    if (tokensP.Length > 0)
                    {

                        if (tokensP.Length >= 1)
                        {
                            PhoneLabel4TextBox.Text = tokensP[0].Trim();
                        }

                        if (tokensP.Length >= 2)
                        {
                            ex4TextBox.Text = tokensP[1];
                        }
                    }

                    AttentionLabel3TextBox.Text = billingAddress.Attention;

                    Address1Label4TextBox.Text = billingAddress.Address1;

                    Address2Label4TextBox.Text = billingAddress.Address2;

                    ZipLabel4TextBox.Text = billingAddress.Zip;

                }
                else
                {
                    //if (Session["DeliveryCompanyLabel"] != null)
                    CompanyLabel4TextBox.Text = "";

                    //if (Session["DeliveryCountryLabel"] != null)
                    CountryLabel4RadComboBox.SelectedValue = "";

                    DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel4RadComboBox.SelectedValue);

                    bool isTextState = true;
                    if (dsStates.Tables.Count > 0)
                        if (dsStates.Tables[0].Rows.Count > 0)
                        {
                            StateLabel4RadComboBox.Items.Clear();
                            isTextState = false;
                            StateLabel4RadComboBox.DataSource = dsStates;
                            StateLabel4RadComboBox.DataTextField = "state_name";
                            StateLabel4RadComboBox.DataValueField = "state_id";
                            StateLabel4RadComboBox.DataBind();

                            if (Session["DeliveryStateLabel"] != null)
                                StateLabel4RadComboBox.Items.FindItemByText(Session["DeliveryStateLabel"].ToString()).Selected = true;
                        }

                    if (isTextState)
                    {
                        //if (Session["DeliveryStateLabel"] != null)
                        StateLabel4TextBox.Text = "";
                    }

                    //if (Session["DeliveryCityLabel"] != null)
                    CityLabel4TextBox.Text = "";

                    //if (Session["DeliveryPhoneLabel"] != null)
                    PhoneLabel4TextBox.Text = "";

                    //if (Session["DeliveryEx"] != null)
                    ex4TextBox.Text = "";

                    //if (Session["DeliveryAttentionLabel"] != null)
                    AttentionLabel3TextBox.Text = "";

                    //if (Session["DeliveryAddress2Label"] != null)
                    Address2Label4TextBox.Text = "";

                    //if (Session["DeliveryZipLabel"] != null)
                    ZipLabel4TextBox.Text = "";

                    //if (Session["DeliveryAddressLabel"] != null)
                    Address1Label4TextBox.Text = "";
                }
            }
            else
            {
                //if (Session["DeliveryCompanyLabel"] != null)
                    CompanyLabel4TextBox.Text = "";

                //if (Session["DeliveryCountryLabel"] != null)
                    CountryLabel4RadComboBox.SelectedValue = "";

                DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel4RadComboBox.SelectedValue);

                bool isTextState = true;
                if (dsStates.Tables.Count > 0)
                    if (dsStates.Tables[0].Rows.Count > 0)
                    {
                        StateLabel4RadComboBox.Items.Clear();
                        isTextState = false;
                        StateLabel4RadComboBox.DataSource = dsStates;
                        StateLabel4RadComboBox.DataTextField = "state_name";
                        StateLabel4RadComboBox.DataValueField = "state_id";
                        StateLabel4RadComboBox.DataBind();

                        if (Session["DeliveryStateLabel"] != null)
                            StateLabel4RadComboBox.Items.FindItemByText(Session["DeliveryStateLabel"].ToString()).Selected = true;
                    }

                if (isTextState)
                {
                    //if (Session["DeliveryStateLabel"] != null)
                        StateLabel4TextBox.Text = "";
                }

                //if (Session["DeliveryCityLabel"] != null)
                    CityLabel4TextBox.Text = "";

                //if (Session["DeliveryPhoneLabel"] != null)
                    PhoneLabel4TextBox.Text = "";

                //if (Session["DeliveryEx"] != null)
                    ex4TextBox.Text = "";

                //if (Session["DeliveryAttentionLabel"] != null)
                    AttentionLabel3TextBox.Text = "";

                //if (Session["DeliveryAddress2Label"] != null)
                    Address2Label4TextBox.Text = "";

                //if (Session["DeliveryZipLabel"] != null)
                    ZipLabel4TextBox.Text = "";

                //if (Session["DeliveryAddressLabel"] != null)
                    Address1Label4TextBox.Text = "";
            }

            SetDynamicFields("4");
        }

        protected void FillPayment()
        {
            //if (Session["UserID"] != null)
            //{
            //    Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
            //    Customer cust = client.GetCustomer(Session["UserID"].ToString());

            //    List<Address> addresses = cust.Addresses;

            //    Address billingAddress = new Address();

            //    for (int i = 0; i < addresses.Count; i++)
            //    {
            //        if (addresses[i].DefaultBilling.Value)
            //        {
            //            billingAddress = addresses[i];
            //            break;
            //        }
            //    }

            //    if (billingAddress != null)
            //    {
            //        CompanyLabel5TextBox.Text = billingAddress.CompanyName;
            //        CountryLabel5RadComboBox.Items.FindItemByText(billingAddress.Country).Selected = true;

            //        DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel5RadComboBox.SelectedValue);

            //        bool isTextState = true;
            //        if (dsStates.Tables.Count > 0)
            //            if (dsStates.Tables[0].Rows.Count > 0)
            //            {
            //                StateLabel5RadComboBox.Items.Clear();
            //                isTextState = false;
            //                StateLabel5RadComboBox.DataSource = dsStates;
            //                StateLabel5RadComboBox.DataTextField = "state_name";
            //                StateLabel5RadComboBox.DataValueField = "state_id";
            //                StateLabel5RadComboBox.DataBind();

            //                StateLabel5RadComboBox.Items.FindItemByText(
            //                        billingAddress.State).Selected = true;
            //            }

            //        if (isTextState)
            //        {
            //            StateLabel5TextBox.Text = billingAddress.State;
            //        }

            //        CityLabel5TextBox.Text = billingAddress.City;

            //        char[] delim = { 'x' };
            //        string[] tokensP = billingAddress.Phone.Split(delim);

            //        if (tokensP.Length > 0)
            //        {

            //            if (tokensP.Length >= 1)
            //            {
            //                PhoneLabel5TextBox.Text = tokensP[0].Trim();
            //            }

            //            if (tokensP.Length >= 2)
            //            {
            //                ex5TextBox.Text = tokensP[1];
            //            }
            //        }

            //        AttentionLabel5TextBox.Text = billingAddress.Attention;

            //        Address1Label5TextBox.Text = billingAddress.Address1;

            //        Address2Label5TextBox.Text = billingAddress.Address2;

            //        ZipLabel5TextBox.Text = billingAddress.Zip;

            //    }
            //}
            FillBilling();
            if (Session["PaymentCookie"] != null)
            {
                if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
                {
                    PaymentTypeLabelRadComboBox.ClearSelection();
                    PaymentTypeLabelRadComboBox.FindItemByValue("0").Selected = true;
                    CreditCardPanel.Visible = true;
                    PurchaseOrderPanel.Visible = false;

                    if (Session["CreditCardLabel"] != null)
                        CreditCardLabelTextBox.Text = Session["CreditCardLabel"].ToString();

                    if (Session["CreditCSVLabel"] != null)
                        CSVLabelTextBox.Text = Session["CreditCSVLabel"].ToString();

                    if (Session["CreditExpirationLabel"] != null)
                    {
                        char[] delim = { '/' };
                        string expiration = Session["CreditExpirationLabel"].ToString();
                        string[] tokens = expiration.Split(delim);

                        ExpirationDateLabelMonthDropDown.ClearSelection();
                        ExpirationDateLabelYearDropDown.ClearSelection();
                        ExpirationDateLabelMonthDropDown.Items.FindByValue(tokens[0]).Selected = true;
                        ExpirationDateLabelYearDropDown.Items.FindByText(tokens[1]).Selected = true;


                    }

                    if (Session["CreditCardType"] != null)
                    {
                        CreditCard2TypeDropDown.Items.FindByText(Session["CreditCardType"].ToString()).Selected = true;
                    }

                    if (Session["CreditName"] != null)
                    {

                        CreditCardNameLabelTextBox.Text = Session["CreditName"].ToString();
                    }
                }
                else
                {
                    PaymentTypeLabelRadComboBox.ClearSelection();
                    PaymentTypeLabelRadComboBox.FindItemByValue("1").Selected = true;
                    CreditCardPanel.Visible = false;
                    PurchaseOrderPanel.Visible = true;

                    if (Session["POExpirationLabel"] != null)
                        PurchaseOrderLabelTextBox.Text = Session["POExpirationLabel"].ToString();

                    //if (Session["PaymentCompanyLabel"] != null)
                    //    CompanyLabel5TextBox.Text = Session["PaymentCompanyLabel"].ToString();

                    //if (Session["PaymentAttentionLabel"] != null)
                    //    AttentionLabel5TextBox.Text = Session["PaymentAttentionLabel"].ToString();

                    //if (Session["PaymentCityLabel"] != null)
                    //    CityLabel5TextBox.Text = Session["PaymentCityLabel"].ToString();

                    //if (Session["PaymentPhoneLabel"] != null)
                    //    PhoneLabel5TextBox.Text = Session["PaymentPhoneLabel"].ToString();

                    //if (Session["PaymentEx"] != null)
                    //    ex5TextBox.Text = Session["PaymentEx"].ToString();

                    //if (Session["PaymentAddressLabel"] != null)
                    //    Address1Label5TextBox.Text = Session["PaymentAddressLabel"].ToString();

                    //if (Session["PaymentAddress2Label"] != null)
                    //    Address2Label5TextBox.Text = Session["PaymentAddress2Label"].ToString();

                    //if (Session["PaymentZipLabel"] != null)
                    //    ZipLabel5TextBox.Text = Session["PaymentZipLabel"].ToString();

                    //if (Session["PaymentCountryLabel"] != null)
                    //    CountryLabel5RadComboBox.Items.FindItemByValue(Session["PaymentCountryLabel"].ToString()).Selected = true;

                    //DataSet dsStates = doQuery("SELECT * FROM States WHERE country_id=" + CountryLabel5RadComboBox.SelectedValue);

                    //bool isTextState = true;
                    //if (dsStates.Tables.Count > 0)
                    //    if (dsStates.Tables[0].Rows.Count > 0)
                    //    {
                    //        StateLabel5RadComboBox.Items.Clear();
                    //        isTextState = false;
                    //        StateLabel5RadComboBox.DataSource = dsStates;
                    //        StateLabel5RadComboBox.DataTextField = "state_name";
                    //        StateLabel5RadComboBox.DataValueField = "state_id";
                    //        StateLabel5RadComboBox.DataBind();

                    //        if (Session["PaymentStateLabel"] != null)
                    //            StateLabel5RadComboBox.Items.FindItemByText(Session["PaymentStateLabel"].ToString()).Selected = true;
                    //    }

                    //if (isTextState)
                    //{
                    //    if (Session["PaymentStateLabel"] != null)
                    //        StateLabel5TextBox.Text = Session["PaymentStateLabel"].ToString();
                    //}

                    //if (Session["PaymentStateLabel"] != null)
                    //    CountryLabel5RadComboBox.Items.FindItemByValue(Session["PaymentCountryLabel"].ToString()).Selected = true;

                }

            }
            else
            {
                PaymentTypeLabelRadComboBox.FindItemByValue("0").Selected = true;
            }
            

            SetDynamicFields("5");
            
        }

        protected void BuildIt(Explore.NetSuite.DataAccess.NDAL client, Customer cust)
        {
            Session["formpage"] = "6";
            Session["Edit"] = "True";
            ThePanel.Controls.Clear();
            try
            {
                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form GetCustomer() Begin------"));

                //Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                //Customer cust = client.GetCustomer(Session["UserID"].ToString());

                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form GetCustomer() End------"));

                FillCheckOutBilling(cust);
                FillCheckOutShipping(cust);
                FillCheckOutDelivery(cust);
                FillCheckOutPayment();

                string thecartID = Session["yourcart"].ToString();

                int multisystemdiscountcount = (int)Session["MultiSystemCount"];

                DataView dvMulti = doQueryDV("SELECT * FROM NetSuitePriceLevel WHERE CustomerType=1 AND MaxQuantity >= " +
                             multisystemdiscountcount.ToString() + " AND MinQuantity <= " + multisystemdiscountcount.ToString());

                decimal multiDiscount = 1.00M;
                if (dvMulti.Count > 0)
                    multiDiscount = (1.00M - decimal.Parse(dvMulti[0]["Discount"].ToString().Trim().Replace("%", "")) / 100.00M);

                DataSet dsCartParent = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
                    "ParentID, SKU, isLoneSKU FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");

                DataSet dsProduct1 = doQuery("SELECT * FROM NetSuiteProducts WHERE NID='" +
                    dsCartParent.Tables[0].Rows[0]["NID"].ToString()+"'");


                DataSet dsCart = doQuery("SELECT * FROM Cart C WHERE C.CartSessionID='" +
                    thecartID + "'");

                DataSet dsCartNotParent = doQuery("SELECT * FROM Cart C, NetSuiteBoxes NSB WHERE " +
                    "NSB.NID=C.SubProductID AND C.isParent='False' AND C.CartSessionID='" +
                    thecartID + "' ORDER BY C.BoxID");

                DataView dvParents = new DataView(doQuery("SELECT * FROM NetSuiteProducts").Tables[0], "", "",
                    DataViewRowState.CurrentRows);

                DataSet dsProdCount = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
                    "ParentID FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");

                DataView dv = new DataView(dsCart.Tables[0], "", "", DataViewRowState.CurrentRows);
                DataView dvSub = new DataView(dsCartNotParent.Tables[0], "", "", DataViewRowState.CurrentRows);
                decimal estimatedTotal = 0.00M;

                string parentID = "";
                Label lab = new Label();
                lab.ID = "TheLabel";
                lab.Visible = false;
                lab.Text = dsCartParent.Tables[0].Rows.Count.ToString();
                ThePanel.Controls.Add(lab);

                #region forloop
                for (int i = 0; i < dsCartParent.Tables[0].Rows.Count; i++)
                {
                    Literal ColumnsLiteral = new Literal();
                    Literal ColumnsLiteralEnd = new Literal();
                    dv.RowFilter = "NID ='" + dsProdCount.Tables[0].Rows[i]["NID"].ToString().Trim().ToUpper() + "' AND isParent = 'True'";
                    parentID = dsProdCount.Tables[0].Rows[i]["ID"].ToString();
                    dvParents.RowFilter = "NID='" + dsProdCount.Tables[0].Rows[i]["NID"].ToString().Trim().ToUpper()+"'";
                    

                    //<a onclick=\"var answer = confirm('Are you sure you want to remove the " +
                    //    dvParents[0]["PriceTitle"].ToString() +
                    //    " from your cart?'); if(answer){ RemoveItem();}\" class=\"LinkUnderline\">Remove</a>



                    string tempSKU = dsCartParent.Tables[0].Rows[i]["SKU"].ToString().Trim();


                    Session["tempSKU"] = "first: " + tempSKU;
                    
                    
                    if (bool.Parse(dsCartParent.Tables[0].Rows[i]["isLoneSKU"].ToString()))
                    {
                        //DataSet dsTemp = doQuery("SELECT * FROM NetSuiteProducts WHERE NID='" + 
                        //    dsCartParent.Tables[0].Rows[i]["NID"].ToString().Trim().ToUpper()+"'");

                        //colLiteral2.Text += "<li>" + dsTemp.Tables[0].Rows[0]["Name"].ToString() + "</li>";
                    }
                    else
                    {
                        dvSub.RowFilter = "ParentID=" + parentID;
                        for (int j = 0; j < dvSub.Count; j++)
                        {
                            
                            tempSKU += dvSub[j]["SKU"].ToString().Trim();
                            Session["tempSKU"] += ", " + j + ":" + tempSKU;
                            //allPrice += decimal.Parse(dvSub[j]["Price"].ToString());
                            //eduPrice += decimal.Parse(dvSub[j]["Price"].ToString());
                            //colLiteral2.Text += "<li>" + dvSub[j]["Name"].ToString() + "</li>";
                        }
                    }

                    Literal colLiteral2 = new Literal();
                    colLiteral2.Text = "<td width=\"200px\" aligh=\"left\">" +
                    tempSKU + "<br/>";
                    dvSub.RowFilter = "ParentID=" + dsProdCount.Tables[0].Rows[i]["ID"].ToString();
                    //decimal allPrice = decimal.Parse(dvParents[0]["Price"].ToString());
                    //decimal eduPrice = decimal.Parse(dvParents[0]["EducationalPrice"].ToString());
                    Session["tempSKU"] += ", whole: "+tempSKU;
                    DataView dvDESC = doQueryDV("SELECT * FROM NetSuiteGroupItem WHERE ItemName='"+tempSKU+"'");

                    colLiteral2.Text += "<ul class=\"CartUL\">";
                    colLiteral2.Text += "<li>" + dvDESC[0]["Description"].ToString() + "</li>";

                    Label label = new Label();
                    label.ID = "labelY" + i.ToString();
                    label.Visible = false;
                    label.Text = tempSKU;

                    ThePanel.Controls.Add(label);

                    dv.RowFilter = "ID ='" + dsProdCount.Tables[0].Rows[i]["ID"].ToString().Trim().ToUpper() +
                        "' AND isParent = 'True'";
                    colLiteral2.Text += "</ul>";
                    colLiteral2.Text += "</td>";
                    colLiteral2.Text += "<td>";

                    ThePanel.Controls.Add(colLiteral2);

                    Label qtyTextBox = new Label();
                    qtyTextBox.ID = "TextBox" + i.ToString();
                    qtyTextBox.Width = 20;
                    qtyTextBox.Height = 20;
                    qtyTextBox.Text = dv[0]["Quantity"].ToString();

                    ThePanel.Controls.Add(qtyTextBox);

                    Literal ColumnsLiteral5 = new Literal();
                    ColumnsLiteral5.Text += "</td>";

                    string msrpCSS = "PriceLabel";

                    if (Session["UserType"] != null)
                        if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
                            NDAL.DataTypes.PriceLevels.Educational)
                            msrpCSS = "PriceLabelCrossed";

                    ColumnsLiteral5.Text += "<td align=\"right\">MSRP: <span class=\"" + msrpCSS + "\">$";

                    ThePanel.Controls.Add(ColumnsLiteral5);

                    Literal ColumnsLiteral2 = new Literal();
                    Label labelAllPrice = new Label();
                    Session["message"] += tempSKU;
                    labelAllPrice.Text = InsertCommaForPrice((multiDiscount*CalculatePrice(tempSKU, 
                        1,
                        NDAL.DataTypes.PriceLevels.MSRP)).ToString(), true); //allPrice.ToString();
                    labelAllPrice.ID = "labelAllPrice" + i.ToString();

                    ThePanel.Controls.Add(labelAllPrice);

                    string priceClass = "PriceLabel";

                    Literal ColumnsLiteral3 = new Literal();
                    Label labelEduPrice = new Label();

                    if (Session["UserType"] != null)
                    {
                        if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
                            NDAL.DataTypes.PriceLevels.Educational)
                        {

                            priceClass = "PriceLabelCrossed";

                            ColumnsLiteral2.Text += "</span><br/><label class=\"CartColumn\">ED Price:</label> <span class=\"PriceLabel\">$";

                            ThePanel.Controls.Add(ColumnsLiteral2);


                            labelEduPrice.Text = InsertCommaForPrice(((CalculatePrice(tempSKU, multisystemdiscountcount,
                                NDAL.DataTypes.PriceLevels.Educational))).ToString(), true); //eduPrice.ToString();
                            labelEduPrice.ID = "labelEduPrice" + i.ToString();

                            ThePanel.Controls.Add(labelEduPrice);

                        }
                    }
                    ColumnsLiteral3.Text += "</span></td>";
                    ColumnsLiteral3.Text += "<td align=\"right\"><span class=\"" + priceClass +
                        "\"><label id=\"allPrice" + i.ToString() + "\">$";

                    ThePanel.Controls.Add(ColumnsLiteral3);

                    Literal ColumnsLiteral4 = new Literal();
                    Label totalAllPrice = new Label();
                    totalAllPrice.ID = "totalAllPrice" + i.ToString();
                    totalAllPrice.Text = InsertCommaForPrice((decimal.Parse(dv[0]["Quantity"].ToString()) * decimal.Parse(labelAllPrice.Text)).ToString(), false);
                    totalAllPrice.ID = "totalAllPrice" + i.ToString();

                    ThePanel.Controls.Add(totalAllPrice);

                    ColumnsLiteral4.Text += "</label></span><br/>";

                    if (Session["UserType"] != null)
                    {
                        if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
                                NDAL.DataTypes.PriceLevels.Educational)
                        {
                            ColumnsLiteral4.Text += "<span class=\"PriceLabel\" id=\"eduPrice" +
                                i.ToString() + "\"><label  id=\"eduPrice" + i.ToString() + "\">$";
                        }
                    }
                    ThePanel.Controls.Add(ColumnsLiteral4);


                    if (Session["UserType"] != null)
                    {
                        if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
                                NDAL.DataTypes.PriceLevels.Educational)
                        {
                            Label totalEduPrice = new Label();
                            totalEduPrice.Text =
                                InsertCommaForPrice((decimal.Parse(dv[0]["Quantity"].ToString()) *
                                decimal.Parse(labelEduPrice.Text)).ToString(), false);
                            totalEduPrice.ID = "totalEduPrice" + i.ToString();
                            estimatedTotal += decimal.Parse(totalEduPrice.Text);
                            ThePanel.Controls.Add(totalEduPrice);

                            ColumnsLiteralEnd.Text += "</label></span></td></tr>";
                            ColumnsLiteralEnd.Text += "<tr><td colspan=\"5\" align=\"right\" class=\"PriceLabel\">You Save: $" +
                                InsertCommaForPrice((decimal.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU, 1,
                                NDAL.DataTypes.PriceLevels.MSRP) - decimal.Parse(totalEduPrice.Text)).ToString(), false) +
                                "</td></tr><tr><td><div style=\"display: none;\">";
                        }
                        else
                        {
                            estimatedTotal += decimal.Parse(totalAllPrice.Text);
                            ColumnsLiteralEnd.Text += "</td></tr><tr><td><div style=\"display: none;\">";
                        }
                    }
                    else
                    {
                        estimatedTotal += decimal.Parse(totalAllPrice.Text);
                        ColumnsLiteralEnd.Text += "</td></tr><tr><td><div style=\"display: none;\">";
                    }

                    Label qtyLabel = new Label();
                    qtyLabel.ID = "qtyLabel" + i.ToString();
                    qtyLabel.Text = dv[0]["Quantity"].ToString();


                    Label idLabel = new Label();
                    idLabel.Visible = false;
                    idLabel.ID = "IDLabel" + i.ToString();
                    idLabel.Text = parentID;

                    Literal ColumnsLiteral6 = new Literal();
                    ColumnsLiteral6.Text = "</div></td></tr><tr><td colspan=\"4\" style=\"padding-top: 10px;\"></td></tr>";



                    ThePanel.Controls.Add(ColumnsLiteralEnd);
                    ThePanel.Controls.Add(qtyLabel);
                    ThePanel.Controls.Add(idLabel);
                    ThePanel.Controls.Add(ColumnsLiteral6);

                    //if ((NDAL.DataTypes.PriceLevels)Session["UserType"] == NDAL.DataTypes.PriceLevels.Educational)
                    //{
                    //    estimatedTotal += decimal.Parse((decimal.Parse(dv[0]["Quantity"].ToString()) * multiDiscount*CalculatePrice(tempSKU, 1,
                    //        NDAL.DataTypes.PriceLevels.Educational)).ToString());
                    //}
                    //else
                    //{
                    //    estimatedTotal += decimal.Parse((decimal.Parse(dv[0]["Quantity"].ToString()) * multiDiscount*CalculatePrice(tempSKU, 1,
                    //        NDAL.DataTypes.PriceLevels.MSRP)).ToString());
                    //}
                }

#endregion

                Session["numProds"] = dsProdCount.Tables[0].Rows.Count;

                //not doing promo for phase I
                //DataSet dsPromo = doQuery("SELECT * FROM CartPromos CP, NetSuitePromos NSP WHERE "+
                //    "CP.CodeID=NSP.NetSuiteID AND CP.CartSession='" + thecartID + "'");

                EstimatedTotalLabel.Text = InsertCommaForPrice(estimatedTotal.ToString(), false);
                TheTotal.Text = InsertCommaForPrice(estimatedTotal.ToString(), false);


                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form GetCustomer() Begin------"));
                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form GetCustomer() End------"));
                Address shipToAddress = new Address();

                for (int i = 0; i < cust.Addresses.Count; i++)
                {
                    if (cust.Addresses[i].DefaultShipping.Value)
                    {
                        shipToAddress = cust.Addresses[i];
                    }
                }

                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form Get Transaction lines Begin------"));
                List<TransactionLineItem> transList = new List<TransactionLineItem>();

                GetTransactionLines(ref transList);

                //HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                //              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //              "info",
                //              "---Draw Review Form Get Transaction lines End------"));

                HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review Form GetUPSRate Begin------"));
                string shippingTotal = client.GetUPSRate(Session["UserID"].ToString(), shipToAddress.InternalID, 
                    ShippingMethods.UPSGround, transList);
                HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review Form GetUPSRate End------"));
                ShippingUPSLabel.Text = "$" + InsertCommaForPrice(shippingTotal, false);

                //Not doing promo for phase I
                //if (dsPromo.Tables.Count > 0)
                //    if (dsPromo.Tables[0].Rows.Count > 0)
                //    {
                //        TheDiscountPercent.Text = dsPromo.Tables[0].Rows[0]["PercentDiscount"].ToString();
                //        MakePromo(dsPromo.Tables[0].Rows[0]["Code"].ToString(),
                //            dsPromo.Tables[0].Rows[0]["PercentDiscount"].ToString());
                //    }

                decimal taxableAmount = 0.00M;
                decimal taxAmount = 0.00M;

                HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review Form GetTax Begin------"));
                decimal taxPercentage = GetTax(shipToAddress, false, ref taxableAmount, ref taxAmount);
                HttpContext.Current.Trace.Warn("FormPage_BuildReviewForm()", string.Format("{0},{1},{2}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "---Draw Review Form GetTax End------"));

                TaxLabel.Text = InsertCommaForPrice(taxAmount.ToString(), false);

                //decimal theTax = (decimal.Parse(TaxLabel.Text) / decimal.Parse("100.00")) * (taxableAmount);

                //TaxLabel.Text = InsertCommaForPrice(theTax.ToString());

                TheTotal.Text = (estimatedTotal + taxAmount + 
                    decimal.Parse(shippingTotal)).ToString();
                EstimatedTotalLabel.Text = InsertCommaForPrice(TheTotal.Text, false);

                //char[] delim = { '.'};
                //string[] tokens = TheTotal.Text.Split(delim);



                //if (tokens[0].Trim().Length > 3)
                //{
                //    //tokens[0] = tokens[0].Insert(tokens[0].Length - 3, ",");
                //    EstimatedTotalLabel.Text = tokens[0];

                //    if (tokens.Length > 1)
                //    {
                //        EstimatedTotalLabel.Text += "." + tokens[1];
                //    }
                //}

                //if (tokens.Length < 2)
                //{
                //    EstimatedTotalLabel.Text += ".00";
                //}
                
            }
            catch (Exception ex)
            {
                Literal ColumnsLiteral = new Literal();
                //ColumnsLiteral.Text = "<tr><td><div>Your cart is empty.</div></td></tr>";
                ColumnsLiteral.Text = ex.ToString();
                //ColumnsLiteral.Text = "<tr><td><div>TempSKU: "+ex.ToString()+"</div></td></tr>";
                ThePanel.Controls.Add(ColumnsLiteral);


            }

        }

        //protected void BuildIt2()
        //{
        //    Session["Edit"] = "True";
        //    string tempSKU = "";
        //    try
        //    {
        //        string thecartID = Session["yourcart"].ToString();


        //        DataSet dsCartParent = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
        //            "ParentID, SKU, isLoneSKU FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");

        //        DataSet dsProduct1 = doQuery("SELECT * FROM NetSuiteProducts WHERE NID=" +
        //            dsCartParent.Tables[0].Rows[0]["NID"].ToString());


        //        DataSet dsCart = doQuery("SELECT * FROM Cart C WHERE C.CartSessionID='" +
        //            thecartID + "'");

        //        DataSet dsCartNotParent = doQuery("SELECT * FROM Cart C, NetSuiteBoxes NSB WHERE " +
        //            "NSB.NID=C.SubProductID AND C.isParent='False' AND C.CartSessionID='" +
        //            thecartID + "' ORDER BY C.BoxID");

        //        DataView dvParents = new DataView(doQuery("SELECT * FROM NetSuiteProducts").Tables[0], "", "",
        //            DataViewRowState.CurrentRows);

        //        DataSet dsProdCount = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
        //            "ParentID FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");



        //        DataView dv = new DataView(dsCart.Tables[0], "", "", DataViewRowState.CurrentRows);
        //        DataView dvSub = new DataView(dsCartNotParent.Tables[0], "", "", DataViewRowState.CurrentRows);
        //        decimal estimatedTotal = 0.00M;

        //        string parentID = "";
        //        Label lab = new Label();
        //        lab.ID = "TheLabel";
        //        lab.Visible = false;
        //        lab.Text = dsCartParent.Tables[0].Rows.Count.ToString();
        //        ThePanel.Controls.Add(lab);
        //        for (int i = 0; i < dsCartParent.Tables[0].Rows.Count; i++)
        //        {
        //            Literal ColumnsLiteral = new Literal();
        //            ColumnsLiteral.ID = "literal1" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            Literal ColumnsLiteralEnd = new Literal();
        //            ColumnsLiteralEnd.ID = "literal7" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            dv.RowFilter = "ID =" + dsCartParent.Tables[0].Rows[i]["ID"].ToString() + " AND isParent = 'True'";
        //            parentID = dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            dvParents.RowFilter = "NID=" + dsCartParent.Tables[0].Rows[i]["NID"].ToString();
        //            ColumnsLiteral.Text += "<tr><td>";

        //            //<a onclick=\"var answer = confirm('Are you sure you want to remove the " +
        //            //    dvParents[0]["PriceTitle"].ToString() +
        //            //    " from your cart?'); if(answer){ RemoveItem();}\" class=\"LinkUnderline\">Remove</a>

        //            ThePanel.Controls.Add(ColumnsLiteral);

        //            LinkButton hyperRemove = new LinkButton();
        //            hyperRemove.ID = "Hyper" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            hyperRemove.Text = "Remove";
        //            hyperRemove.CssClass = "LinkUnderline";
        //            hyperRemove.OnClientClick = "javascript:return confirm('Are you sure you want to remove " +
        //                dvParents[0]["PriceTitle"].ToString() + " from your cart?');";
        //            hyperRemove.Click += new EventHandler(RemoveFromCart);
        //            hyperRemove.CommandArgument = dsCartParent.Tables[0].Rows[i]["ID"].ToString() + ":" + i.ToString();

        //            ThePanel.Controls.Add(hyperRemove);



        //            if (bool.Parse(dsCartParent.Tables[0].Rows[i]["isLoneSKU"].ToString()))
        //            {
        //                Literal colLiteral2 = new Literal();
        //                colLiteral2.ID = "literal2" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //                colLiteral2.Text = "</td><td width=\"200px\" aligh=\"left\">" +
        //                dvParents[0]["PriceTitle"].ToString() + "<br/>";


        //                dvSub.RowFilter = "ParentID=" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //                //decimal allPrice = decimal.Parse(dvParents[0]["Price"].ToString());
        //                //decimal eduPrice = decimal.Parse(dvParents[0]["EducationalPrice"].ToString());
        //                colLiteral2.Text += "<ul class=\"CartUL\">";

        //                DataSet dsTemp = doQuery("SELECT * FROM NetSuiteProducts WHERE NID=" + dsCartParent.Tables[0].Rows[i]["NID"].ToString().Trim());

        //                colLiteral2.Text += "<li>" + dsTemp.Tables[0].Rows[0]["Name"].ToString() + "</li>";


        //                Label label = new Label();
        //                label.ID = "label" + i.ToString();
        //                label.Visible = false;
        //                label.Text = dsCartParent.Tables[0].Rows[i]["SKU"].ToString();

        //                tempSKU = dsCartParent.Tables[0].Rows[i]["SKU"].ToString().Trim();

        //                ThePanel.Controls.Add(label);

        //                //dv.RowFilter = "NID =" + dsProdCount.Tables[0].Rows[i]["NID"].ToString() + 
        //                //    " AND isParent = 'True'";
        //                colLiteral2.Text += "</ul>";
        //                colLiteral2.Text += "</td>";
        //                colLiteral2.Text += "<td>";

        //                ThePanel.Controls.Add(colLiteral2);
        //            }
        //            else
        //            {
        //                Literal colLiteral2 = new Literal();
        //                colLiteral2.ID = "literal2" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //                colLiteral2.Text = "<br/>" +
        //                "<a href=\"default.aspx?a=productconfig&nid=" + dsCartParent.Tables[0].Rows[i]["NID"].ToString() +
        //                "&pid=" + dsCartParent.Tables[0].Rows[i]["ID"].ToString() +
        //                "\" class=\"LinkUnderline\">Edit</a></td><td width=\"200px\" aligh=\"left\">" +
        //                dvParents[0]["PriceTitle"].ToString() + "<br/>";


        //                dvSub.RowFilter = "ParentID=" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //                //decimal allPrice = decimal.Parse(dvParents[0]["Price"].ToString());
        //                //decimal eduPrice = decimal.Parse(dvParents[0]["EducationalPrice"].ToString());
        //                colLiteral2.Text += "<ul class=\"CartUL\">";



        //                tempSKU = dsCartParent.Tables[0].Rows[i]["SKU"].ToString().Trim();

        //                for (int j = 0; j < dvSub.Count; j++)
        //                {
        //                    tempSKU += dvSub[j]["SKU"].ToString().Trim();
        //                    //allPrice += decimal.Parse(dvSub[j]["Price"].ToString());
        //                    //eduPrice += decimal.Parse(dvSub[j]["Price"].ToString());
        //                    colLiteral2.Text += "<li>" + dvSub[j]["Name"].ToString() + "</li>";
        //                }

        //                Label label = new Label();
        //                label.ID = "label" + i.ToString();
        //                label.Visible = false;
        //                label.Text = tempSKU;

        //                ThePanel.Controls.Add(label);

        //                //dv.RowFilter = "NID =" + dsProdCount.Tables[0].Rows[i]["NID"].ToString() + 
        //                //    " AND isParent = 'True'";
        //                colLiteral2.Text += "</ul>";
        //                colLiteral2.Text += "</td>";
        //                colLiteral2.Text += "<td>";

        //                ThePanel.Controls.Add(colLiteral2);
        //            }
        //            TextBox qtyTextBox = new TextBox();
        //            qtyTextBox.ID = "TextBox" + i.ToString();
        //            qtyTextBox.Width = 20;
        //            qtyTextBox.Height = 20;
        //            qtyTextBox.Text = dv[0]["Quantity"].ToString();

        //            ThePanel.Controls.Add(qtyTextBox);

        //            Literal ColumnsLiteral5 = new Literal();
        //            ColumnsLiteral5.ID = "literal3" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            ColumnsLiteral5.Text += "</td>";

        //            string msrpCSS = "PriceLabel";

        //            if (Session["UserType"] != null)
        //                if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
        //                    NDAL.DataTypes.PriceLevels.Educational)
        //                    msrpCSS = "PriceLabelCrossed";

        //            ColumnsLiteral5.Text += "<td width=\"200px\" align=\"right\">MSRP: <span class=\"" + msrpCSS + "\">$";

        //            ThePanel.Controls.Add(ColumnsLiteral5);

        //            Literal ColumnsLiteral2 = new Literal();
        //            ColumnsLiteral2.ID = "literal4" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            Label labelAllPrice = new Label();
        //            Session["message"] += tempSKU;

        //            labelAllPrice.Text = CalculatePrice(tempSKU, 1,
        //                NDAL.DataTypes.PriceLevels.MSRP).ToString(); //allPrice.ToString();

        //            labelAllPrice.ID = "labelAllPrice" + i.ToString();

        //            ThePanel.Controls.Add(labelAllPrice);

        //            string priceClass = "PriceLabel";

        //            Literal ColumnsLiteral3 = new Literal();
        //            ColumnsLiteral3.ID = "literal5" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            Label labelEduPrice = new Label();

        //            if (Session["UserType"] != null)
        //            {
        //                if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
        //                    NDAL.DataTypes.PriceLevels.Educational)
        //                {

        //                    priceClass = "PriceLabelCrossed";

        //                    ColumnsLiteral2.Text += "</span><br/><label class=\"CartColumn\">Educational Price:</label> <span class=\"PriceLabel\">$";

        //                    ThePanel.Controls.Add(ColumnsLiteral2);


        //                    labelEduPrice.Text = CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                        NDAL.DataTypes.PriceLevels.Educational).ToString(); //eduPrice.ToString();
        //                    labelEduPrice.ID = "labelEduPrice" + i.ToString();

        //                    ThePanel.Controls.Add(labelEduPrice);

        //                }
        //            }
        //            ColumnsLiteral3.Text += "</span></td>";
        //            ColumnsLiteral3.Text += "<td align=\"right\"><span class=\"" + priceClass +
        //                "\"><label id=\"allPrice" + i.ToString() + "\">$";

        //            ThePanel.Controls.Add(ColumnsLiteral3);

        //            Literal ColumnsLiteral4 = new Literal();
        //            ColumnsLiteral4.ID = "literal6" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            Label totalAllPrice = new Label();
        //            totalAllPrice.ID = "totalAllPrice" + i.ToString();
        //            totalAllPrice.Text = (double.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                NDAL.DataTypes.PriceLevels.MSRP)).ToString();
        //            totalAllPrice.ID = "totalAllPrice" + i.ToString();

        //            ThePanel.Controls.Add(totalAllPrice);

        //            ColumnsLiteral4.Text += "</label></span><br/>";

        //            if (Session["UserType"] != null)
        //            {
        //                if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
        //                        NDAL.DataTypes.PriceLevels.Educational)
        //                {
        //                    ColumnsLiteral4.Text += "<span class=\"PriceLabel\" id=\"eduPrice" +
        //                        i.ToString() + "\"><label  id=\"eduPrice" + i.ToString() + "\">$";
        //                }
        //            }
        //            ThePanel.Controls.Add(ColumnsLiteral4);


        //            if (Session["UserType"] != null)
        //            {
        //                if ((NDAL.DataTypes.PriceLevels)Session["UserType"] ==
        //                        NDAL.DataTypes.PriceLevels.Educational)
        //                {
        //                    Label totalEduPrice = new Label();
        //                    totalEduPrice.Text = (double.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU,
        //                        int.Parse(dv[0]["Quantity"].ToString()),
        //                        NDAL.DataTypes.PriceLevels.Educational)).ToString();
        //                    totalEduPrice.ID = "totalEduPrice" + i.ToString();

        //                    ThePanel.Controls.Add(totalEduPrice);



        //                    ColumnsLiteralEnd.Text += "</label></span></td></tr>";
        //                    ColumnsLiteralEnd.Text += "<tr><td colspan=\"5\" align=\"right\" class=\"PriceLabel\">You Save: $" +
        //                        (decimal.Parse(dv[0]["Quantity"].ToString()) * decimal.Parse((decimal.Parse(CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                        NDAL.DataTypes.PriceLevels.MSRP).ToString()) -
        //                        decimal.Parse(CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                        NDAL.DataTypes.PriceLevels.Educational).ToString())).ToString())).ToString() +
        //                        "</td></tr><tr><td><div style=\"display: none;\">";
        //                    estimatedTotal += decimal.Parse((double.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                NDAL.DataTypes.PriceLevels.Educational)).ToString());
        //                }
        //                else
        //                {
        //                    ColumnsLiteralEnd.Text += "</td></tr><tr><td><div style=\"display: none;\">";
        //                    estimatedTotal += decimal.Parse((double.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                NDAL.DataTypes.PriceLevels.MSRP)).ToString());
        //                }
        //            }
        //            else
        //            {
        //                ColumnsLiteralEnd.Text += "</td></tr><tr><td><div style=\"display: none;\">";
        //                estimatedTotal += decimal.Parse((double.Parse(dv[0]["Quantity"].ToString()) * CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()),
        //                NDAL.DataTypes.PriceLevels.MSRP)).ToString());
        //            }

        //            Label qtyLabel = new Label();
        //            qtyLabel.ID = "qtyLabel" + i.ToString();
        //            qtyLabel.Text = dv[0]["Quantity"].ToString();


        //            Label idLabel = new Label();
        //            idLabel.Visible = false;
        //            idLabel.ID = "IDLabel" + i.ToString();
        //            idLabel.Text = parentID;

        //            Literal ColumnsLiteral6 = new Literal();
        //            ColumnsLiteral6.ID = "literal8" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
        //            ColumnsLiteral6.Text = "</div></td></tr>";



        //            ThePanel.Controls.Add(ColumnsLiteralEnd);
        //            ThePanel.Controls.Add(qtyLabel);
        //            ThePanel.Controls.Add(idLabel);
        //            ThePanel.Controls.Add(ColumnsLiteral6);


        //        }

        //        Session["numProds"] = dsCartParent.Tables[0].Rows.Count;

        //        DataSet dsPromo = doQuery("SELECT * FROM CartPromos CP, NetSuitePromos NSP WHERE CP.CodeID=NSP.NetSuiteID AND CP.CartSession='" +
        //            thecartID + "'");

        //        EstimatedTotalLabel.Text = estimatedTotal.ToString();
        //        TheTotal.Text = estimatedTotal.ToString();

        //        if (dsPromo.Tables.Count > 0)
        //            if (dsPromo.Tables[0].Rows.Count > 0)
        //            {
        //                TheDiscountPercent.Text = dsPromo.Tables[0].Rows[0]["PercentDiscount"].ToString();
        //                MakePromo(dsPromo.Tables[0].Rows[0]["Code"].ToString(),
        //                    dsPromo.Tables[0].Rows[0]["PercentDiscount"].ToString());
        //            }



        //    }
        //    catch (Exception ex)
        //    {
        //        Literal ColumnsLiteral = new Literal();
        //        ColumnsLiteral.Text = "<tr><td><div>Your cart is empty.</div></td></tr>";
        //        //ColumnsLiteral.Text = "<tr><td><div>TempSKU: "+ex.ToString()+"</div></td></tr>";
        //        ColumnsLiteral.Text = ex.ToString() + " tempSKU: " + tempSKU;
        //        ThePanel.Controls.Add(ColumnsLiteral);
        //        NotEmptyPanel.Visible = false;

        //        //Session["formpage"] = "1";

        //    }

        //}

        protected void MakePromo(string code, string discount)
        {
            if (discount != "")
            {
                PromoEnteredPanel.Visible = true;
                PromoCodeLabel.Text = code + " (" + discount
                     + "% OFF)&nbsp;&nbsp;&nbsp;&nbsp;";
                decimal percentOff = decimal.Parse(discount);
                percentOff = percentOff / 100.00M;
                TheDiscountCode.Text = code;
                TheDiscountPercent.Text = discount;

                decimal tempDec = decimal.Parse(TheTotal.Text) * percentOff;
                char[] delim = { '.' };
                string[] tempStr = tempDec.ToString().Split(delim);

                string finalDisc = "";

                if (tempStr.Length < 2)
                {
                    finalDisc = tempStr[0] + ".00";
                }
                else
                {
                    tempDec = Math.Round(tempDec, 2);

                    string[] temp2 = tempDec.ToString().Split(delim);

                    finalDisc = temp2[0] + "." + temp2[1].Substring(0, 2);
                }

                decimal tempEst = decimal.Parse(TheTotal.Text) - (decimal.Parse(TheTotal.Text) * percentOff);
                string[] tempStr2 = tempEst.ToString().Split(delim);
                string finalEst = "";

                if (tempStr2.Length < 2)
                {
                    finalEst = tempStr2[0] + ".00";
                }
                else
                {
                    tempEst = Math.Round(tempEst, 2);

                    string[] temp2 = tempEst.ToString().Split(delim);

                    finalEst = temp2[0] + "." + temp2[1].Substring(0, 2);
                }

                DiscountPriceLabel.Text = "-" + finalDisc;
                EstimatedTotalLabel.Text = finalEst;
            }
        }

        protected void SubmitOrder(object sender, EventArgs e)
        {
            if (Session["HasOrderBeenClicked"] == null)
            {
                try
                {
                    if (TermsCheckBox.Checked)
                    {

                        HttpContext.Current.Trace.Warn("FormPage_SubmitOrder()", string.Format("{0},{1},{2},{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "SubmitLogin()",
                              "---SubmitOrder Begin------"));
                        Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                        client.MediatorLoggingLevel = Explore.NetSuite.JSONservice.MediatorLogLevels.Debug;
                        HttpContext.Current.Trace.Warn("FormPage_SubmitOrder()", string.Format("{0},{1},{2},{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "SubmitLogin()",
                              "---SubmitOrder End------"));
                        //if (Session["UserID"] == null)
                        //    CreateUser();

                        HttpContext.Current.Trace.Warn("FormPage_SubmitOrder()", string.Format("{0},{1},{2},{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "SubmitLogin()",
                              "---CreateOrder() Begin------"));
                        CreateOrder();
                        HttpContext.Current.Trace.Warn("FormPage_SubmitOrder()", string.Format("{0},{1},{2},{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "SubmitLogin()",
                              "---CreateOrder() End------"));
                        if (Session["OrderStatus"].ToString() == "A")
                        {

                            OrderLabel.Text = "Your order number is #" + Session["OrderNumber"].ToString();
                        }
                        else
                        {
                            OrderLabel.Text = "Your order was not submited!";
                        }
                        string thecartID = Session["yourcart"].ToString();
                        doQuery("DELETE FROM Cart WHERE CartSessionID='" + thecartID+"'");


                        Session["formpage"] = "6";

                        //Clear cart after submitting to NetSuite
                        Session["yourcart"] = null;
                        Session.Remove("yourcart");

                        Session["HasOrderBeenClicked"] = "true";

                        //Update Top Header
                        UpdateTopHeader();

                        if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
                        {
                            GoToForm(8, 6);
                            Session["PaymentCookie"] = null;
                            Session["CreditCardLabel"] = null;
                            Session["CreditCSVLabel"] = null;
                            Session["CreditExpirationLabel"] = null;
                            Session["CreditCardType"] = null;
                            Session["CreditName"] = null;
                            Session["POExpirationLabel"] = null;

                            Session.Remove("PaymentCookie");
                            Session.Remove("CreditCardLabel");
                            Session.Remove("CreditCSVLabel");
                            Session.Remove("CreditExpirationLabel");
                            Session.Remove("CreditCardType");
                            Session.Remove("CreditName");
                            Session.Remove("POExpirationLabel");
                            Session["formpage"] = "8";
                            Response.Redirect("default.aspx?a=form&id=1");
                        }
                        else
                        {
                            GoToForm(7, 6);
                            Session["PaymentCookie"] = null;
                            Session["CreditCardLabel"] = null;
                            Session["CreditCSVLabel"] = null;
                            Session["CreditExpirationLabel"] = null;
                            Session["CreditCardType"] = null;
                            Session["CreditName"] = null;
                            Session["POExpirationLabel"] = null;

                            Session.Remove("PaymentCookie");
                            Session.Remove("CreditCardLabel");
                            Session.Remove("CreditCSVLabel");
                            Session.Remove("CreditExpirationLabel");
                            Session.Remove("CreditCardType");
                            Session.Remove("CreditName");
                            Session.Remove("POExpirationLabel");
                            Session["formpage"] = "7";

                            //Get the invoice
                            Array theTransactions = (Array)client.GetTransactions(Session["UserID"].ToString(), DateTime.Now.AddMinutes(double.Parse("-10.00")), DateTime.Now);

                            //code recommended by john ogle. refer to bug 1688 in OA bug tracking system.
                            Dictionary<string, IEnumerable<Transaction>> invoiceMap = new Dictionary<string, IEnumerable<Transaction>>();
                            List<Transaction> invoices;

                            foreach (Transaction t in theTransactions)
                            {
                                if (t.Type != "SalesOrd")
                                {
                                    invoices = new List<Transaction>();

                                    if (invoiceMap.ContainsKey(t.CreatedFrom))
                                    {
                                        invoices = (List<Transaction>)invoiceMap[t.CreatedFrom];
                                    }
                                    else
                                    {
                                        invoiceMap.Add(t.CreatedFrom, invoices);
                                    }

                                    invoices.Add(t);
                                }
                            }

                            foreach (Transaction oneTrans in theTransactions)
                            {
                                if (oneTrans.Type == "SalesOrd" && oneTrans.OrderNumber == Session["OrderNumber"].ToString())
                                {
                                    SalesOrderHyperLink.NavigateUrl = oneTrans.PrintoutURL;
                                }
                            }

                            Response.Redirect("default.aspx?a=form&id=1");



                        }


                        
                        
                    }
                    else
                    {
                        Static6Messages.Text = "Must agree to terms and conditions.";
                    }
                }
                catch (CommandErrorException ex)
                {
                    if (ex.ToString().Contains("An error occurred while processing your credit card"))
                    {
                        Static6Messages.Text = "There was an error processing your credit card. Please contact the merchant for further assistance.";
                    }
                    else
                    {
                        Static6Messages.Text = ex.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ErrorLabel.Text += ex.ToString();
                }


            }
            else
            {
                //Clear cart after submitting to NetSuite
                
                UpdateTopHeader();

                if (Session["PaymentCookie"].ToString() == "CreditCardPanel")
                {
                    GoToForm(8, 6);
                    Session["formpage"] = "8";
                    Response.Redirect("default.aspx?a=form&id=1");
                }
                else
                {
                    GoToForm(7, 6);
                    Session["formpage"] = "7";
                    Response.Redirect("default.aspx?a=form&id=1");
                }
            }
        }

        protected void CreateOrder()
        {
            string sku = "";
            try
            {

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                              "info",
                              "SubmitLogin()",
                              "---Create NDAL client object Begin------"));
                Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
                client.MediatorLoggingLevel = Explore.NetSuite.JSONservice.MediatorLogLevels.Debug;

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---Create NDAL client object End------"));

                string userID = Session["UserID"].ToString();
                SalesOrder order = new SalesOrder();
                order.Customer = userID;

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetCustomer() Begin------"));
                Customer cust = client.GetCustomer(userID);
                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetCustomer() End------"));

                //What are these? How do we know what to set these to?

                //order.BillTo = "375300";
                //order.CreditCardSelect = "11";
                //order.CSC = "319";

                order.Date = DateTime.Now.ToShortDateString();
                order.DontReceiveBefore = DateTime.Today.AddDays(5).ToShortDateString();
                order.GetAuthorization = true;

                //how would we know this? there's no place to set this in the wireframes.
                order.IncludesInstallation = false;

                order.InstallAddress = null;

                List<Address> addresses = cust.Addresses;

                Address shipToAddress = new Address();


                for (int i = 0; i < addresses.Count; i++)
                {
                    if (addresses[i].DefaultBilling.Value)
                        order.BillTo = addresses[i].InternalID;
                    if (addresses[i].DefaultShipping.Value)
                    {
                        order.ShipTo = addresses[i].InternalID;
                        shipToAddress = addresses[i];
                    }
                    if (addresses[i].DefaultDelivery.Value)
                        order.DeliverTo = addresses[i].InternalID;
                    //if (addresses[i].DefaultAccount.Value)
                    //{

                    //}
                }


                //how do we know this?  ** LST - This should default to the Tualatin Warehouse, denotes where the inventory will be deducted from. 
                order.Location = Locations.TualatinWarehouse;


                //what do we do in case of PO? there's no choice for this.
                if (POPanel.Visible)
                {
                    order.Terms = Terms.Net30;
                    order.PurchaseOrder = PurchaseOrderLabelTextBox.Text.Trim();
                }
                else
                {
                    switch (CreditCard2TypeDropDown.SelectedItem.Value)
                    {
                        case "0":
                            order.PaymentMethod = PaymentMethods.Discover;
                            break;
                        case "1":
                            order.PaymentMethod = PaymentMethods.MasterCard;
                            break;
                        case "2":
                            order.PaymentMethod = PaymentMethods.VISA;
                            break;
                        default:
                            order.PaymentMethod = PaymentMethods.VISA;
                            break;
                    }

                    order.CreditCardSelect = Session["CardToUser"].ToString();

                    //for (int i = 0; i < cust.CreditCards.Count; i++)
                    //{
                    //    if (cust.CreditCards[i].DefaultCreditCard.Value)
                    //    {
                    //        order.CreditCardSelect = cust.CreditCards[0].InternalID;
                    //        break;
                    //    }
                    //}
                }

                string thecartID = Session["yourcart"].ToString();

                //DataSet dsPromo = doQuery("SELECT * FROM CartPromos CP, NetSuitePromos NSP WHERE " +
                //    "CP.CodeID=NSP.NetSuiteID AND CP.CartSession='" + thecartID + "'");

                //if (dsPromo.Tables.Count > 0)
                //    if (dsPromo.Tables[0].Rows.Count > 0)
                //    {
                //        order.PromotionCode = TheDiscountPercent.Text = dsPromo.Tables[0].Rows[0]["NetSuiteID"].ToString();
                //    }

                order.GetAuthorization = false;

                order.Phone = cust.Phone;

                order.SalesRep = cust.SalesRepID;

                order.SchoolDistrict = userID;
                order.ShippingCost = ShippingUPSLabel.Text.Replace("$", "").Replace(",", "");
                order.ShipVia = ShippingMethods.UPSGround;
                order.Type = SalesOrderType.Regular;

                //Don't pass in a taxrate as NetSuite code should be doing this. 
                //Website code just displays the tax information.
                decimal taxableAmount = 0.00M;
                decimal taxAmount = 0.00M;

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTax Begin------"));
                order.TaxRate = GetTax(shipToAddress, false, ref taxableAmount, ref taxAmount).ToString();
                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTax End------"));

                //Take care of the tax following bug 2180 in OA bug tracking system

                string state = doQuery("SELECT * FROM States WHERE state_name='" + shipToAddress.State + "'").Tables[0].Rows[0]["state_code"].ToString();
                DataSet dsTax = doQuery("SELECT * FROM Taxables WHERE State='" + state + "'");

                bool taxeable = false;
                string taxcode = "";
                if (dsTax.Tables[0].Rows.Count > 0)
                {
                    taxeable = bool.Parse(dsTax.Tables[0].Rows[0]["Taxable"].ToString());
                    taxcode = dsTax.Tables[0].Rows[0]["Taxcode"].ToString();

                    order.Taxable = taxeable;
                    order.TaxItem = TaxItems.AVATAX;
                }
                else
                {
                    order.Taxable = false;
                    order.TaxItem = TaxItems.NotTaxable;
                }

                



                //*****************Old transaction code***************************************
                //create transaction items for each item in the cart
                ////test transaction
                //List<TransactionLineItem> transList = new List<TransactionLineItem>();

                //decimal eduPrice = decimal.Parse("1119");
                //TransactionLineItem transLine = new TransactionLineItem();
                //transLine.Amount = (eduPrice * decimal.Parse("2")).ToString(); //or dvParents[0]["Price"].ToString()
                //transLine.Description = "Online order";
                //transLine.DiscountPercent = "0%";//==================================TAKE CARE OF IT FROM DISCOUNT INFO
                //transLine.ItemID = "511";//=================================TAKE CARE OF IT WHEN PUSH SERVICE IS DONE
                //transLine.PriceLevel = PriceLevels.MSRP; //==============TAKE CARE OF IT WHEN KNOW THE ENUMERATION MAPPING
                //transLine.CommissionClass = Classes.Classroom; //need this to make it work: per bug 1504
                ////how do we know the item type?
                //transLine.ItemType = "noninventoryitem"; //=======================CHANGE THIS TO GRAB FROM THE MemberItem table when get these synced up.

                ////will this always be only 'Standard' or 'Educational'?
                //transLine.Quantity = "2";
                //transLine.Rate = "1119";
                //transLine.Tax = "4.44";//=============================================TAKE CARE OF FROM AVATAX
                //transLine.UnitPrice = "1119";

                ////how do we know the shippment location? ** LST - This should default to the Tualatin Warehouse, denotes where the inventory will be deducted from. 
                //transLine.Location = Locations.TualatinWarehouse;

                //transList.Add(transLine);
                //*****************END OLD CODE************************************************



                DataSet dsCartParent = doQuery("SELECT * FROM Cart C, NetSuiteProducts NSP WHERE " +
                    "C.NID=NSP.NID AND isParent = 'True' AND CartSessionID='" + thecartID + "'");

                DataSet dsProduct1 = doQuery("SELECT * FROM NetSuiteProducts WHERE NID='" +
                    dsCartParent.Tables[0].Rows[0]["NID"].ToString().Trim().ToUpper()+"'");


                DataSet dsCart = doQuery("SELECT * FROM Cart C WHERE C.CartSessionID='" +
                    thecartID + "'");

                DataSet dsCartNotParent = doQuery("SELECT * FROM Cart C, NetSuiteBoxes NSB WHERE " +
                    "NSB.NID=C.SubProductID AND C.isParent='False' AND C.CartSessionID='" +
                    thecartID + "' ORDER BY C.BoxID");

                DataView dvParents = new DataView(doQuery("SELECT * FROM NetSuiteProducts").Tables[0], "", "",
                    DataViewRowState.CurrentRows);

                DataSet dsProdCount = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
                    "ParentID FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");


                DataView dv = new DataView(dsCart.Tables[0], "", "", DataViewRowState.CurrentRows);
                DataView dvSub = new DataView(dsCartNotParent.Tables[0], "", "", DataViewRowState.CurrentRows);

                List<TransactionLineItem> transList = new List<TransactionLineItem>();

                sku = "";

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTransactionLines Begin------"));
                GetTransactionLines(ref transList);

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTransactionLines End------"));
                
                order.TransactionLines = transList;

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---CreateOrder() Begin------"));
                CreateOrderResult orderResult = client.CreateOrder(order);
                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---CreateOrder() End------"));


                Session["OrderNumber"] = orderResult.OrderNumber.ToString();
                Session["OrderStatus"] = orderResult.OrderStatus.ToString();

                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTax Begin------"));
                GetTax(shipToAddress, true, ref taxableAmount, ref taxAmount);
                HttpContext.Current.Trace.Warn("FormPage_CreateOrder()", string.Format("{0},{1},{2},{3}",
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             "info",
                             "SubmitLogin()",
                             "---GetTax End------"));

                


                //IEnumerable<Transaction> trans = client.GetTransactions("211829", null, null);
                //}
                //catch (CommandErrorException ex)
                //{
                //    if (ex.ToString().Contains("An error occurred while processing your credit card"))
                //    {
                //        Static6Messages.Text = "There was an error processing your credit card. Please contact the merchant for further assistance.";
                //    }
                //    else
                //    {
                //        Static6Messages.Text = ex.ToString();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    ErrorLabel.Text += ex.ToString() + sku;
                //}
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = "sku: " + sku + ex.ToString();
            }
        }

        protected void GetTransactionLines(ref List<TransactionLineItem> transList)
        {
            string sku = "";
            string thecartID = Session["yourcart"].ToString();
            DataSet dsPromo = doQuery("SELECT * FROM CartPromos CP, NetSuitePromos NSP WHERE " +
                    "CP.CodeID=NSP.NetSuiteID AND CP.CartSession='" + thecartID + "'");

            DataSet dsCartParent = doQuery("SELECT * FROM Cart C, NetSuiteProducts NSP WHERE " +
                    "C.NID=NSP.NID AND isParent = 'True' AND CartSessionID='" + thecartID + "'");

            DataSet dsProduct1 = doQuery("SELECT * FROM NetSuiteProducts WHERE NID='" +
                dsCartParent.Tables[0].Rows[0]["NID"].ToString().Trim().ToUpper() + "'");


            DataSet dsCart = doQuery("SELECT * FROM Cart C WHERE C.CartSessionID='" +
                thecartID + "'");

            DataSet dsCartNotParent = doQuery("SELECT * FROM Cart C, NetSuiteBoxes NSB WHERE " +
                "NSB.NID=C.SubProductID AND C.isParent='False' AND C.CartSessionID='" +
                thecartID + "' ORDER BY C.BoxID");

            DataView dvParents = new DataView(doQuery("SELECT * FROM NetSuiteProducts").Tables[0], "", "",
                DataViewRowState.CurrentRows);

            DataSet dsProdCount = doQuery("SELECT DISTINCT ID, NID, Quantity, isParent, SubProductID, " +
                "ParentID FROM Cart WHERE CartSessionID='" + thecartID + "' AND isParent ='True'");


            DataView dv = new DataView(dsCart.Tables[0], "", "", DataViewRowState.CurrentRows);
            DataView dvSub = new DataView(dsCartNotParent.Tables[0], "", "", DataViewRowState.CurrentRows);

            for (int i = 0; i < dsCartParent.Tables[0].Rows.Count; i++)
            {
                TransactionLineItem transLine = new TransactionLineItem();
                string tempSKU = dsCartParent.Tables[0].Rows[i]["SKU"].ToString().Trim();

                dvSub.RowFilter = "ParentID=" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();

                if (!bool.Parse(dsCartParent.Tables[0].Rows[i]["isLoneSKU"].ToString()))
                {
                    for (int j = 0; j < dvSub.Count; j++)
                    {
                        tempSKU += dvSub[j]["SKU"].ToString().Trim();
                    }
                    sku += " [i: " + i.ToString() + ", tempSKU: " + tempSKU + "]<br/>";
                }

                string itemID = doQuery("SELECT * FROM NetSuiteGroupItem WHERE ItemName='" + tempSKU +
                    "'").Tables[0].Rows[0]["ItemID"].ToString();

                decimal price = decimal.Parse("0.00");
                dv.RowFilter = "ID=" + dsCartParent.Tables[0].Rows[i]["ID"].ToString();
                if ((NDAL.DataTypes.PriceLevels)Session["UserType"] == PriceLevels.Educational)
                {
                    price = CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()), NDAL.DataTypes.PriceLevels.Educational);
                }
                else
                {
                    price = CalculatePrice(tempSKU, int.Parse(dv[0]["Quantity"].ToString()), NDAL.DataTypes.PriceLevels.MSRP);
                }

                transLine.Amount = (price * decimal.Parse(dv[0]["Quantity"].ToString())).ToString();
                transLine.Description = "Online order";

                DataSet dsPromo2 = doQuery("SELECT * FROM CartPromos CP, NetSuitePromos NSP WHERE " +
                "CP.CodeID=NSP.NetSuiteID AND CP.CartSession='" + thecartID + "'");



                if (dsPromo.Tables.Count > 0)
                    if (dsPromo.Tables[0].Rows.Count > 0)
                    {
                        transLine.DiscountPercent = TheDiscountPercent.Text = dsPromo2.Tables[0].Rows[0]["PercentDiscount"].ToString();
                    }
                    else
                    {
                        transLine.DiscountPercent = "0%";
                    }
                else
                    transLine.DiscountPercent = "0%";


                transLine.ItemID = itemID;
                transLine.PriceLevel = (NDAL.DataTypes.PriceLevels)Session["UserType"];
                transLine.CommissionClass = Classes.Classroom; //need this to make it work: per bug 1504
                //how do we know the item type?
                transLine.ItemType = "itemgroup";

                
                //will this always be only 'Standard' or 'Educational'?
                transLine.Quantity = dv[0]["Quantity"].ToString();
                transLine.Rate = price.ToString();
                //transLine.Tax = "0.00";//=============================================TAKE CARE OF FROM AVATAX


                //how do we know the shippment location? ** LST - This should default to the Tualatin Warehouse, denotes where the inventory will be deducted from. 
                transLine.Location = Locations.TualatinWarehouse;

                transList.Add(transLine);
                //}

            }
        }

        protected decimal GetTax(Address shipToAddress, bool isInvoice, ref decimal taxableAmount, ref decimal taxAmount)
        {
            try
            {
                Avalara.AvaTax.Adapter.TaxService.GetTaxRequest getTaxrequest = new Avalara.AvaTax.Adapter.TaxService.GetTaxRequest();

                getTaxrequest.DocDate = DateTime.Now;

                if (isInvoice)
                {
                    getTaxrequest.DocType = Avalara.AvaTax.Adapter.TaxService.DocumentType.SalesInvoice;
                }
                else
                {
                    getTaxrequest.DocType = Avalara.AvaTax.Adapter.TaxService.DocumentType.SalesOrder;
                }
                getTaxrequest.DocCode = "1";

                Avalara.AvaTax.Adapter.AddressService.Address address;
                address = new Avalara.AvaTax.Adapter.AddressService.Address(); //Ship From
                address.Line1 = "11509 SW Herman Rd";
                address.Line2 = null;
                address.Line3 = null;
                address.City = "Tualatin";
                address.Region = "OR";
                address.PostalCode = "97062";
                address.Country = "US";

                getTaxrequest.OriginAddress = address;

                address = new Avalara.AvaTax.Adapter.AddressService.Address(); //Ship To
                address.Line1 = shipToAddress.Address1;
                address.Line2 = shipToAddress.Address2;
                address.City = shipToAddress.City;
                address.Region = shipToAddress.State;
                address.PostalCode = shipToAddress.Zip;
                address.Country = shipToAddress.Country;
                getTaxrequest.DestinationAddress = address;

                getTaxrequest.CustomerCode = shipToAddress.CompanyName;

                //first line is equal to the total and the other equal to the shipping + handling amount
                Avalara.AvaTax.Adapter.TaxService.Line line;
                line = new Avalara.AvaTax.Adapter.TaxService.Line();
                line.No = "01";                                    //What does this number refer to? Do we have to set it to something specific? 
                //line number is a unique identifier for each line of the invoice, usually sequential (1000, 2000, 3000, or something like that) 
                line.ItemCode = "P0000000";                     //I believe Lightspeed already entered the items and their codes into the Avatax    Dashboard. How do we retreive these programatically?                     //if lightspeed entered their items in the dashboard, you will still need to pass the item number to activate the mapping to the tax code.
                line.Qty = 1;
                line.Amount = decimal.Parse(EstimatedTotalLabel.Text);
                line.Discounted = false;

                //Not needed: line.ExemptionNo = "";
                //Not needed: line.Ref1 = "client specific reference string 1";   //optional //What are Ref1 and Ref2 used for? Do we have to set them to anything specific?
                //Not needed: line.Ref2 = "client specific reference #2";
                //Not needed: line.RevAcct = "a revenue account";                 //optional //Does this have to be an account number? If so, where do we get this number from?
                line.TaxCode = "AVATAX";                        //How do we get the Tax code? Is it specific to an item or just one general Tax code? 
                //a tax code will only need to be passed if the item isn't mapped in the dashboard as you have indicated lightspeed has done 
                getTaxrequest.Lines.Add(line);

                //first line is equal to the total and the other equal to the shipping + handling amount
                line = new Avalara.AvaTax.Adapter.TaxService.Line();
                line.No = "02";                                    //What does this number refer to? Do we have to set it to something specific? 
                //line number is a unique identifier for each line of the invoice, usually sequential (1000, 2000, 3000, or something like that) 
                line.ItemCode = "P0000000";                     //I believe Lightspeed already entered the items and their codes into the Avatax    Dashboard. How do we retreive these programatically?                     //if lightspeed entered their items in the dashboard, you will still need to pass the item number to activate the mapping to the tax code.
                line.Qty = 1;
                line.Amount = decimal.Parse(ShippingUPSLabel.Text.Replace("$", ""));
                line.Discounted = false;
                //Not needed: line.ExemptionNo = "";
                //Not needed: line.Ref1 = "client specific reference string 1";   //optional //What are Ref1 and Ref2 used for? Do we have to set them to anything specific?
                //Not needed: line.Ref2 = "client specific reference #2";
                //Not needed: line.RevAcct = "a revenue account";                 //optional //Does this have to be an account number? If so, where do we get this number from?
                line.TaxCode = "FR";                        //How do we get the Tax code? Is it specific to an item or just one general Tax code? 
                //a tax code will only need to be passed if the item isn't mapped in the dashboard as you have indicated lightspeed has done 
                getTaxrequest.Lines.Add(line);



                getTaxrequest.CompanyCode = System.Configuration.ConfigurationManager.AppSettings["AvataxCompanyCode"].ToString();              //How do we get Lightspeed’s company code for Avatax? on the dashboard organization page

                //getTaxrequest.DocCode = "DOC20051234567";          //What is this field and what do we set it to?  An invoice number (unique and probably sequential)...Lightspeed should be able to tell you what to use here 
                //getTaxrequest.DocDate = DateTime.Today;
                //getTaxrequest.DocType = Avalara.AvaTax.Adapter.TaxService.DocumentType.SalesInvoice;
                //getTaxrequest.Discount = 0m;
                //getTaxrequest.ExemptionNo = null;
                //getTaxrequest.DetailLevel = Avalara.AvaTax.Adapter.TaxService.DetailLevel.Tax;
                //getTaxrequest.PurchaserCode = "12345";             //What is this field and what do we set it to?  //optional 
                //getTaxrequest.PurchaserType = null;
                //getTaxrequest.SalespersonCode = null;

                Avalara.AvaTax.Adapter.TaxService.TaxSvc taxSvc = new Avalara.AvaTax.Adapter.TaxService.TaxSvc();
                taxSvc.Configuration.Url = System.Configuration.ConfigurationManager.AppSettings["AvataxUrl"].ToString();                   //What is this url?  //url, account, and license key are in an email that Cathy probably got during the welcome call with Chelssie Brown.  Please check with Cathy for this information. 
                taxSvc.Configuration.Security.Account = System.Configuration.ConfigurationManager.AppSettings["AvataxAccount"].ToString();  //What do we set this to?
                taxSvc.Configuration.Security.License = System.Configuration.ConfigurationManager.AppSettings["AvataxLicense"].ToString();  //What do we set this to?
                //taxSvc.Configuration.Security.Timeout = timeout;  //Is there a specific timeout we should use?  //set this as you wish...300 is often used.
                Avalara.AvaTax.Adapter.TaxService.GetTaxResult getTaxResult = taxSvc.GetTax(getTaxrequest);

                //return the percentage instead of the amount: decimal.Parse(ShippingUPSLabel.Text.Replace("$", "")) + 
                taxableAmount = getTaxResult.TotalTaxable;
                taxAmount = getTaxResult.TotalTax;
                decimal theTax = taxAmount;


                if (taxableAmount != 0.00M)
                    return Math.Round(((theTax / taxableAmount) * decimal.Parse("100.00")), 3);
                else
                    return 0.00M;
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.ToString();
                return 0.00M;
            }
        }

        protected bool CreateUser()
        {
                Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();
           
                //AccountDropDown.SelectedItem.Value;
                string company = CompanyLabelTextBox.Text;
                if (company == "")
                    company = null;

                PriceLevels thelevel = PriceLevels.Educational;
                switch (AccountDropDown.SelectedItem.Value)
                {
                    case "1":
                        thelevel = PriceLevels.MSRP;
                        break;
                    case "2":
                        thelevel = PriceLevels.Educational;
                        break;
                    case "3":
                        thelevel = PriceLevels.State;
                        break;
                    default: break;
                }
                try
                {
                    CreateCustomerResult newCustomer = new CreateCustomerResult();
                    if (Session["UserID"] == null)
                        newCustomer = client.CreateCustomer(company, PhoneLabelTextBox.Text, EmailTextBox.Text
                        , FirstNameTextBox.Text, LastNameTextBox.Text, thelevel, TitleTextBox.Text, null);
                   
                    bool EmailChanged = false;

                    if (newCustomer != null || Session["UserID"] != null)
                    {

                        // retrive the newly created customer so we can update it with an address and password

                        string customerID = "";

                        if (Session["UserID"] != null)
                            customerID = Session["UserID"].ToString();
                        else
                            customerID = newCustomer.CustomerInternalID;

                        Customer customer = client.GetCustomer(customerID);

                        if (Session["UserID"] != null)
                        {
                            if(customer.Email != EmailTextBox.Text)
                                EmailChanged = true;
                            customer.Email = EmailTextBox.Text;
                            customer.CompanyName = company;
                            customer.Phone = PhoneLabelTextBox.Text;
                            customer.FirstName = FirstNameTextBox.Text;
                            customer.LastName = LastNameTextBox.Text;
                            customer.CustomerType = thelevel;
                            customer.Title = TitleTextBox.Text;
                        }

                        customer.JobRole = GetJobRole(RoleDropDown.SelectedItem.Text);


                        // assign at least one address
                        string state = "";
                        if (StateLabel1RadComboBox.Visible)
                            state = StateLabel1RadComboBox.SelectedItem.Text;
                        else
                            state = StateLabel1TextBox.Text;



                        //create primary address: this is no loger being done by Andrew/John request: bug 2387
                        //Address addr = new Address();
                        //addr.Attention = CompanyLabelTextBox.Text;
                        //addr.CompanyName = CompanyLabelTextBox.Text;
                        //addr.Address1 = Address1LabelTextBox.Text;
                        //addr.Address2 = Address2LabelTextBox.Text;
                        //addr.City = CityLabelTextBox.Text;
                        //addr.State = state;
                        //addr.Zip = ZipLabelTextBox.Text;
                        //addr.DefaultBilling = false;
                        //addr.DefaultShipping = true;
                        //addr.DefaultAccount = false;
                        //addr.DefaultDelivery = false;
                        //DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_name = '" +
                        //    CountryLabel1RadComboBox.SelectedItem.Text + "'");
                        //addr.Country = dsCount.Tables[0].Rows[0]["country_2_code"].ToString();
                        //addr.Residential = ResidentialCheckBox.Checked;
                        //addr.Phone = PhoneLabelTextBox.Text;



                        //if (exTextBox.Text.Trim() != "")
                        //{
                        //    addr.Phone = PhoneLabelTextBox.Text + " x" + exTextBox.Text.Trim();
                        //}

                        Address addr2 = new Address();
                        addr2.Attention = FirstNameTextBox.Text + " " + LastNameTextBox.Text;
                        addr2.CompanyName = CompanyLabelTextBox.Text;
                        addr2.Address1 = Address1LabelTextBox.Text;
                        addr2.Address2 = Address2LabelTextBox.Text;
                        addr2.City = CityLabelTextBox.Text;
                        addr2.State = state;
                        addr2.Zip = ZipLabelTextBox.Text;
                        addr2.DefaultBilling = false;
                        addr2.DefaultShipping = false;
                        addr2.DefaultAccount = true;
                        addr2.DefaultDelivery = false;

                        DataSet dsCount = doQuery("SELECT * FROM Countries WHERE country_name = '" +
                            CountryLabel1RadComboBox.SelectedItem.Text + "'");
                        addr2.Country = dsCount.Tables[0].Rows[0]["country_2_code"].ToString();
                        addr2.Residential = ResidentialCheckBox.Checked;
                        addr2.Phone = PhoneLabelTextBox.Text;

                        if (exTextBox.Text.Trim() != "")
                        {
                            addr2.Phone = PhoneLabelTextBox.Text + " x" + exTextBox.Text.Trim();
                        }


                        List<NDAL.DataTypes.Address> addresses = new List<NDAL.DataTypes.Address>();
                        //addresses.Add(addr);
                        addresses.Add(addr2);

                        if (Session["UserID"] == null)
                        {
                            customer.Addresses = addresses;
                        }
                        else
                        {
                            for(int j=0;j<customer.Addresses.Count;j++)
                            {
                                if (customer.Addresses[j].DefaultAccount.Value)
                                {
                                    customer.Addresses[j] = addr2;
                                }
                            }
                        }




                        // set a password for this customer
                        customer.Password = PasswordTextBox.Text;

                        // save dynamic fields
                        object refCust = customer;
                        SaveDynamicFields("1", ref refCust);
                        customer = (Customer)refCust;
                        // perform the update, sending back the complete customer object
                        client.UpdateCustomer(customer);
                        Session["HasUserBeenClicked"] = "true";
                        // get the customer again to verify password was set
                        Customer getCustomer = client.GetCustomer(customerID);

                        string custID = "";

                        //Sign the new user in
                        bool isAuth = false;
                        if (Session["UserID"] == null)
                            isAuth = client.Authenticate(EmailTextBox.Text, PasswordTextBox.Text, out custID);
                        else
                        {
                            if (EmailChanged)
                            {
                                isAuth = client.Authenticate(EmailTextBox.Text, PasswordTextBox.Text, out custID);
                            }
                            else
                            {
                                custID = Session["UserID"].ToString();
                            }
                        }

                        Session["UserID"] = custID;
                        Session["UserType"] = getCustomer.CustomerType;
                        Session["User"] = getCustomer.FirstName;
                        Session["UserSalesRep"] = getCustomer.SalesRepID;

                        string groups = "";
                        groups = "1";
                        UserTracking();
                        FormsAuthenticationTicket authTicket =
                                        new FormsAuthenticationTicket(1, EmailTextBox.Text.Trim(),
                                                      DateTime.Now,
                                                      DateTime.Now.AddMinutes(60),
                                                      false, groups);

                        // Now encrypt the ticket.
                        string encryptedTicket =
                          FormsAuthentication.Encrypt(authTicket);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.
                        HttpCookie authCookie =
                                     new HttpCookie(FormsAuthentication.FormsCookieName,
                                                    encryptedTicket);
                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);
                        //}
                        //catch (CommandErrorException ex)
                        //{
                        //    Console.WriteLine("Oh No! Code: {0} Message: {1}", ex.ErrorCode, ex.Message);

                        //}
                        //catch (TimeoutException ex)
                        //{
                        //    Console.WriteLine("WCF client network request timed out, message: {0}", ex.Message);

                        //}
                        //catch (CommunicationException ex)
                        //{
                        //    Console.WriteLine("WCF communication exception, message: {0}", ex.Message);

                        //}
                        Static1Messages.Text = "";
                        return true;

                    }
                    else
                    {

                        Static1Messages.Text = "A customer with this email address already exists.";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("Existing Customer"))
                    {
                        Static1Messages.Text = "A customer with this email address already exists.";
                    }
                    else
                    {
                        ErrorLabel.Text = ex.ToString();
                    }
                    return false;
                }
            
        }

        protected void SaveDynamicFields(string formID, ref object NSDataType)
        {
            Explore.NetSuite.DataAccess.NDAL client = new Explore.NetSuite.DataAccess.NDAL();

            DataSet dsFields = doQuery("SELECT * FROM FormPageFields FPF, FieldTypes FT WHERE FPF.FieldTypeID=FT.ID AND FPF.FormPageID=" +
                formID);

            DataView dvFields = new DataView(dsFields.Tables[0], "", "", DataViewRowState.CurrentRows);

            if(dvFields.Count > 0)
                for (int i = 0; i < dvFields.Count; i++)
                {
                    TextBox textbox = (TextBox)FindControl(dvFields[i]["FieldName"].ToString().Replace(" ", "") + formID + i.ToString());
                    string dataType = dvFields[i]["NetSuiteDataType"].ToString();

                    switch (dataType)
                    {
                        case "1":
                            Customer cust = (Customer)NSDataType;
                            System.Reflection.MemberInfo[] members = cust.GetType().GetMembers();
                            for (int j = 0; j < members.Length; j++)
                            {
                                if (members[j].MemberType == System.Reflection.MemberTypes.Property)
                                {
                                    if (members[j].Name == dvFields[i]["NetSuiteField"].ToString())
                                    {
                                        cust.GetType().GetProperty(members[j].Name).SetValue(cust, textbox.Text, null);
                                        //cust.GetType().GetField(members[j].Name).SetValue(cust, textbox.Text);
                                    }
                                }
                            }
                            break;
                        case "2":
                            Address adrs = (Address)NSDataType;
                            System.Reflection.MemberInfo[] members2 = adrs.GetType().GetMembers();
                            for (int j = 0; j < members2.Length; j++)
                            {
                                if (members2[j].MemberType == System.Reflection.MemberTypes.Property)
                                {
                                    if (members2[j].Name == dvFields[i]["NetSuiteField"].ToString())
                                    {
                                        adrs.GetType().GetProperty(members2[j].Name).SetValue(adrs, textbox.Text, null);
                                        //adrs.GetType().GetField(members2[j].Name).SetValue(adrs, textbox.Text);
                                    }
                                }
                            }
                            break;
                        case "3":
                            CreditCard card = (CreditCard)NSDataType;
                            System.Reflection.MemberInfo[] members3 = card.GetType().GetMembers();
                            for (int j = 0; j < members3.Length; j++)
                            {
                                if (members3[j].MemberType == System.Reflection.MemberTypes.Property)
                                {
                                    if (members3[j].Name == dvFields[i]["NetSuiteField"].ToString())
                                    {
                                        card.GetType().GetProperty(members3[j].Name).SetValue(card, textbox.Text, null);
                                        //card.GetType().GetField(members3[j].Name).SetValue(card, textbox.Text);
                                    }
                                }
                            }
                            break;
                        default: break;
                    }
                }

        }

        protected void UserTracking()
        {
            
            string s_source = "";
            string s_other = "";
            encryption o_encrypt = new encryption();

            string titleVal = CompanyLabelTextBox.Text;

            string state = "";
            if (StateLabel1RadComboBox.Visible)
                state = StateLabel1RadComboBox.SelectedItem.Text;
            else
                state = StateLabel1TextBox.Text;
           
            if (SignUpCheckBox.Checked)
            {
                newsLetterSignup(FirstNameTextBox.Text, LastNameTextBox.Text, Address1LabelTextBox.Text,
                    Address2LabelTextBox.Text, CityLabelTextBox.Text, state, ZipLabelTextBox.Text, CountryLabel1RadComboBox.SelectedItem.Value,
                    TitleTextBox.Text, CompanyLabelTextBox.Text, EmailTextBox.Text, PhoneLabelTextBox.Text, null);
            }

            string s_emailQuery = "SELECT * FROM ls_user WHERE EMAIL='" + EmailTextBox.Text + "'";

            DataSet ds = doQuery(s_emailQuery);


            if (ds.Tables[0].Rows.Count > 0)
            {
                
            }
            else
            {
                string s_insertQuery = "INSERT INTO ls_user (FIRSTNAME, LASTNAME, TITLE, EMAIL, PASSWORD, PHONE, FAX, "+
                    "SCHOOL, STREET1, STREET2, CITY, STATE, ZIP, COUNTRY, DATE, SOURCE, OTHER)" +
                        "VALUES ('" + fixSQL(FirstNameTextBox.Text) + "' , '" +
                                    fixSQL(LastNameTextBox.Text) + "' , '" +
                                    fixSQL(TitleTextBox.Text) + "' , '" +
                                    fixSQL(EmailTextBox.Text) + "' , '" +
                                    o_encrypt.encrypt(PasswordTextBox.Text.Trim()) + "' , '" +
                                    fixSQL(PhoneLabelTextBox.Text) + "' , '"
                                     + "' , '" +
                                    fixSQL(CompanyLabelTextBox.Text) + "' , '" +
                                    fixSQL(Address1LabelTextBox.Text) + "' , '" +
                                    fixSQL(Address2LabelTextBox.Text) + "' , '" +
                                    fixSQL(CityLabelTextBox.Text) + "' , '" +
                                    fixSQL(state) + "' , '" +
                                    fixSQL(ZipLabelTextBox.Text) + "' , '" +
                                    fixSQL(CountryLabel1RadComboBox.SelectedItem.Value) + "' , '" +
                                    DateTime.Now + "' , '" +
                                    s_source + "' , '" +
                                    s_other + "')";

                execSQL(s_insertQuery);

                string SQL = "INSERT INTO activitylog (SOURCE, FIRSTNAME, LASTNAME, TITLE, SCHOOL, STREET1, STREET2, CITY, STATE, ZIP, COUNTRY, EMAIL, PHONE, FAX, DATE)" +
                    "VALUES ('Registration', '" + fixSQL(FirstNameTextBox.Text) + "' , '" +
                    fixSQL(LastNameTextBox.Text) + "' , '" +
                    fixSQL(TitleTextBox.Text) + "' , '" +
                    fixSQL(CompanyLabelTextBox.Text) + "' , '" +
                    fixSQL(Address1LabelTextBox.Text) + "' , '" +
                    fixSQL(Address2LabelTextBox.Text) + "' , '" +
                    fixSQL(CityLabelTextBox.Text) + "' , '" +
                    fixSQL(state) + "' , '" +
                    fixSQL(ZipLabelTextBox.Text) + "' , '" +
                    fixSQL(CountryLabel1RadComboBox.SelectedItem.Value) + "' , '" +
                    fixSQL(EmailTextBox.Text) + "' , '" +
                    fixSQL(PhoneLabelTextBox.Text) + "' , '" +
                    "' , '" +
                    DateTime.Now + "')";
                execSQL(SQL);

                DataSet subds = doQuery("SELECT * FROM activitylog where EMAIL='" + EmailTextBox.Text + "'  ORDER BY DATE DESC");
                TimeSpan ts = new TimeSpan(10, 0, 0, 0);
                DateTime dt = DateTime.Now;

                if (subds.Tables["table"].Rows.Count > 0)
                {
                    HttpCookie userinfo = new System.Web.HttpCookie("userinfo", subds.Tables["table"].Rows[0]["ID"].ToString());
                    userinfo.Expires = dt.Add(ts);
                    Response.Cookies.Add(userinfo);
                }

                if (ConfigurationManager.AppSettings.Get("netsuitetracking") == "true")
                {
                    LightSPEED.NetSuite.ContactLead contactLead = new LightSPEED.NetSuite.ContactLead();
                    contactLead.Address = Address1LabelTextBox.Text;
                    contactLead.CampaignSource = "10833"; // CAM0001 WEBSITE
                    contactLead.City = CityLabelTextBox.Text;
                    contactLead.Comments = "Registration";
                    contactLead.Company = CompanyLabelTextBox.Text;
                    contactLead.ContactPreference = "";
                    contactLead.ContactReason = ContactReason.Other;
                    DataSet dsCountry = doQuery("SELECT * FROM countries where country_id='" + CountryLabel1RadComboBox.SelectedItem.Value + "'");
                    contactLead.Country = dsCountry.Tables["table"].Rows[0]["netsuite_id"].ToString();
                    contactLead.Creation = DateTime.Now;
                    contactLead.Email = EmailTextBox.Text;
                    contactLead.Fax = null;
                    contactLead.FirstName = FirstNameTextBox.Text;
                    contactLead.LastName = LastNameTextBox.Text;
                    contactLead.Phone = PhoneLabelTextBox.Text;
                    contactLead.Product = "";
                    contactLead.Reference = "";
                    contactLead.Job_Role = RoleDropDown.SelectedItem.Value;
                    String strState = state;
                    DataSet dsState = doQuery("SELECT * FROM STATES WHERE state_name='" + strState + "'");
                    if (dsState.Tables["table"].Rows.Count > 0)
                    {
                        strState = dsState.Tables["table"].Rows[0]["STATE_CODE"].ToString().Trim();
                    }
                    contactLead.StateOrProvince = strState;

                    contactLead.Title = TitleTextBox.Text;
                    contactLead.ZipOrPostalCode = ZipLabelTextBox.Text;
                    contactLead.DownloadName = null;

                    int contactID = ContactLeadManager.Insert(contactLead);


                    HttpCookie contactcookie = new System.Web.HttpCookie("contactID", contactID.ToString());
                    contactcookie.Expires = dt.Add(ts);
                    Response.Cookies.Add(contactcookie);
                }
                String strBody = "";

                strBody += "First name: " + FirstNameTextBox.Text.ToUpperInvariant() + "<br>";
                strBody += "Last name: " + LastNameTextBox.Text.ToUpper() + "<br>";
                strBody += "Job Title: " + titleVal.ToUpper() + "<br>";
                strBody += ConfigurationManager.AppSettings.Get("roletext") + ": " + RoleDropDown.SelectedItem.Value + "<br>";
                strBody += "School District: " + CompanyLabelTextBox.Text.ToUpper() + "<br>";
                strBody += "Email: " + EmailTextBox.Text.ToUpper() + "<br>";
                strBody += "Address: " + Address1LabelTextBox.Text.ToUpper() + "<br>";
                strBody += "Address2: " + Address2LabelTextBox.Text.ToUpper() + "<br>";
                strBody += "City: " + CityLabelTextBox.Text.ToUpper() + "<br>";
                strBody += "State: " + state.ToUpper() + "<br>";
                strBody += "Zip: " + ZipLabelTextBox.Text.ToUpper() + "<br>";
                strBody += "Country: " + CountryLabel1RadComboBox.SelectedItem.Text.ToUpper() + "<br>";
                strBody += "Phone: " + PhoneLabelTextBox.Text.ToUpper() + "<br>";
                strBody += "Fax: " + "<br>";


                ds = doQuery("SELECT * FROM extra WHERE TYPE='REGISTER_EMAIL'");
                String strContact = ds.Tables["table"].Rows[0]["INFO"].ToString();
                sendEmail(strContact, EmailTextBox.Text, "Website: US Registration", strBody);

                execSQL("INSERT INTO comments_users (USRNAME, PASSWD, EMAIL, VALIDATED, DISABLED, BIRTH_YEAR, REG_CODE, RECOMMENDED, ABUSE_FLAGS) values ('" + 
                    fixSQL(FirstNameTextBox.Text.ToUpper()) + " " + fixSQL(LastNameTextBox.Text.ToUpper()) + "', '" + 
                    o_encrypt.encrypt(PasswordTextBox.Text.ToUpper()) + "', '" + fixSQL(EmailTextBox.Text.ToUpper()) + "', 1, 0, 1900, '0', 0, 0)");

                ds = doQuery("SELECT * FROM comments_users");
                System.AppDomain.CurrentDomain.SetData("users", ds);

                HttpCookie c_user = new System.Web.HttpCookie("ls_user", EmailTextBox.Text);
                c_user.Expires = DateTime.Now.AddDays(14);
                Response.Cookies.Add(c_user);

                ds = doQuery("SELECT * FROM comments_users ORDER BY ID DESC");
                HttpCookie cook = new HttpCookie("comments");
                cook.Values.Add("id", ds.Tables["table"].Rows[0]["ID"].ToString());
                cook.Values.Add("name", ds.Tables["table"].Rows[0]["USRNAME"].ToString());
                cook.Expires = DateTime.Now.AddDays(14);
                Response.Cookies.Add(cook);


            }
        }
    }
}