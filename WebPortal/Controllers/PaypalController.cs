using PayPal.Api;
using WebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.Data.Entity;

namespace WebPortal.Controllers
{
    public class PaypalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PaymentWithCreditCard([Bind(Include = "ID,ItemName,ItemPrice,ItemQuantity,cvv,month,year,fname,lname,cardnumber,cardtype,Subtotal,Total,Shipping,Tax")] Paymentinfo paymentinfo)
        {
            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "LKR";
            item.price = "100";
            item.quantity = "1";
            item.sku = "sku";


            List<Item> itms = new List<Item>();
            itms.Add(item);
            ItemList itemList = new ItemList();
            itemList.items = itms;


            Address billingAddress = new Address();
            billingAddress.city = "NewYork";
            billingAddress.country_code = "US";
            billingAddress.line1 = "23rd street kew gardens";
            billingAddress.postal_code = "43210";
            billingAddress.state = "NY";



            CreditCard crdtCard = new CreditCard();
            crdtCard.billing_address = billingAddress;
            crdtCard.cvv2 = "673";  // CVV here
            crdtCard.expire_month = 04;
            crdtCard.expire_year = 2018;
            crdtCard.first_name = "Dileepa";
            crdtCard.last_name = "Rajapaksa";
            crdtCard.number = "4403590697872419"; //Card Number Here
            crdtCard.number = paymentinfo.cardnumber.ToString();
            crdtCard.type = "visa";


            Details details = new Details();
            details.shipping = "0";
            details.subtotal = "100";
            details.tax = "0";

            Amount amnt = new Amount();
            amnt.currency = "USD";

            amnt.total = "100";
            amnt.total = paymentinfo.Total.ToString();
            amnt.details = details;


            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = "your new invoice number which you are generating";



            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(tran);



            FundingInstrument fundInstrument = new FundingInstrument();
            fundInstrument.credit_card = crdtCard;



            List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
            fundingInstrumentList.Add(fundInstrument);


            Payer payr = new Payer();
            payr.funding_instruments = fundingInstrumentList;
            payr.payment_method = "credit_card";


            Payment pymnt = new Payment();
            pymnt.intent = "sale";
            pymnt.payer = payr;
            pymnt.transactions = transactions;

            try
            {


                APIContext apiContext = PaypalConfiguration.GetAPIContext();


                Payment createdPayment = pymnt.Create(apiContext);


                if (createdPayment.state.ToLower() != "approved")
                {
                    return View("Failure");
                }
            }
            catch (PayPal.PayPalException ex)
            {
                Logger.Log("Error: " + ex.Message);
                return View("Failure");
            }

            return View("Success");
        }
    }
}