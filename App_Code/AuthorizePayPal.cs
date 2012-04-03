using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.ComponentModel;
using System.Web;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using com.paypal.sdk.core;
using com.paypal.sdk.core.nvp;
using com.paypal.sdk.logging;
using com.paypal.sdk.exceptions;
using com.paypal.sdk.resources;
using com.paypal.sdk.rules;
using com.paypal.sdk.util;
using log4net;

/// <summary>
/// Summary description for AuthorizePayPal
/// </summary>
public class AuthorizePayPal
{
	public AuthorizePayPal()
	{
        apiUserName = System.Configuration.ConfigurationManager.AppSettings["APIUserName"].ToString();
        apiPassword = System.Configuration.ConfigurationManager.AppSettings["APIPassword"].ToString();
        apiSignature = System.Configuration.ConfigurationManager.AppSettings["APISignature"].ToString();
        apiEnvironment = System.Configuration.ConfigurationManager.AppSettings["APIEnvironment"].ToString();
	}

    private string apiUserName = "";
    private string apiPassword = "";
    private string apiSignature = "";
    private string apiEnvironment = "";
    public NVPCodec DoPayment(string paymentAction, string amount, string creditCardType, string creditCardNumber,
        string expdate_month, string expdate_year, string cvv2Number, string firstName, string lastName, string address1, string city, string state,
        string countryCode, string zip, string IPAddress)
    {
        NVPCallerServices caller = new NVPCallerServices();
        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
        /*
         WARNING: Do not embed plaintext credentials in your application code.
         Doing so is insecure and against best practices.
         Your API credentials must be handled securely. Please consider
         encrypting them for use in any production environment, and ensure
         that only authorized individuals may view or modify them.
         */

        // Set up your API credentials, PayPal end point, API operation and version.
        profile.APIUsername = apiUserName;
        profile.APIPassword = apiPassword;
        profile.APISignature = apiSignature;
        profile.Environment = apiEnvironment;
        caller.APIProfile = profile;
      
        NVPCodec encoder = new NVPCodec();

        encoder["VERSION"] = "51.0";
        encoder["METHOD"] = "DoDirectPayment";

        // Add request-specific fields to the request.
        encoder["PAYMENTACTION"] = paymentAction;
        encoder["AMT"] = amount;
        encoder["CREDITCARDTYPE"] = creditCardType;
        encoder["ACCT"] = creditCardNumber;
        //encoder["EXPMONTH"] = "12";
        //encoder["EXPYEAR"] = "2012";
        if (expdate_month.Length == 1)
            expdate_month = "0" + expdate_month;
        encoder["EXPDATE"] = expdate_month + expdate_year;
        //encoder["ExpMonthSpecified"] = "True";
        //encoder["ExpYearSpecified"] = "True";
        encoder["CVV2"] = cvv2Number;
        encoder["FIRSTNAME"] = firstName;
        encoder["LASTNAME"] = lastName;
        encoder["STREET"] = address1;
        encoder["CITY"] = city;
        encoder["STATE"] = state;
        encoder["ZIP"] = zip;
        encoder["COUNTRYCODE"] = countryCode;
        encoder["CURRENCYCODE"] = "USD";
        encoder["IPADDRESS"] = IPAddress;

        // Execute the API operation and obtain the response.
        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = caller.Call(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);

        //string allResults = pStrrequestforNvp + "<br/><br/>" + pStresponsenvp + "<br/><br/>";

        //foreach (string key in decoder.Keys)
        //{
        //    allResults += "key: '" + key + "', value: '" + decoder[key] + "' <br/>";
        //}
        //allResults += "<br/>Encoder:<br/>";
        //foreach (string key in encoder)
        //{
        //    allResults += "key: '" + key + "', value: '" + encoder[key] + "' <br/>";
        //}

        return decoder;
    }

    public NVPCodec CancelAuthorization(string transactionID, string note)
    {
        NVPCallerServices caller = new NVPCallerServices();
        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
        /*
         WARNING: Do not embed plaintext credentials in your application code.
         Doing so is insecure and against best practices.
         Your API credentials must be handled securely. Please consider
         encrypting them for use in any production environment, and ensure
         that only authorized individuals may view or modify them.
         */

        // Set up your API credentials, PayPal end point, API operation and version.
        caller.APIProfile = profile;

        NVPCodec encoder = new NVPCodec();
        encoder["VERSION"] = "51.0";
        encoder["METHOD"] = "DoVoid";

        // Add request-specific fields to the request.
        encoder["AUTHORIZATIONID"] = transactionID;
        encoder["NOTE"] = note;
        encoder["TRXTYPE"] = "V";

        // Execute the API operation and obtain the response.
        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = caller.Call(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        return decoder;
    }

    public NVPCodec DoCaptureCode(string authorization_id, string amount, string invoice_Id, string note)
    {
        NVPCallerServices caller = new NVPCallerServices();
        IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();
        /*
         WARNING: Do not embed plaintext credentials in your application code.
         Doing so is insecure and against best practices.
         Your API credentials must be handled securely. Please consider
         encrypting them for use in any production environment, and ensure
         that only authorized individuals may view or modify them.
         */


        // Set up your API credentials, PayPal end point, API operation and version.
        profile.APIUsername = apiUserName;
        profile.APIPassword = apiPassword;
        profile.APISignature = apiSignature;
        profile.Environment = apiEnvironment;
        caller.APIProfile = profile;

        NVPCodec encoder = new NVPCodec();
        encoder["VERSION"] = "51.0";
        encoder["METHOD"] = "DoCapture";

        // Add request-specific fields to the request.
        encoder["TRXTYPE"] = "D";
        encoder["AUTHORIZATIONID"] = authorization_id;
        encoder["COMPLETETYPE"] = "Complete";
        encoder["AMT"] = amount;
        encoder["INVNUM"] = invoice_Id;
        encoder["NOTE"] = note;

        // Execute the API operation and obtain the response.
        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = caller.Call(pStrrequestforNvp);

        NVPCodec decoder = new NVPCodec();
        decoder.Decode(pStresponsenvp);
        return decoder;
    }
}
