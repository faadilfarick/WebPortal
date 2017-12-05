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
using WebPortal;

namespace WebPortal.Controllers
{
    public class PaypalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Paypal
        public ActionResult Index([Bind(Include = "ID,Name,City,Address,Category,Description,Latitude,Longitude,Owner")] Business business)
        {
            TempData["Name"] = business.Name;
            TempData["City"] = business.City;
            TempData["Address"] = business.Address;
            TempData["Category"] = business.Category;
            TempData["Description"] = business.Description;
            TempData["Latitude"] = business.Latitude;
            TempData["Longitude"] = business.Longitude;

            return View ("Index");
        }
        public ActionResult PaymentWithCreditCard([Bind(Include = "ID,ItemName,ItemPrice,ItemQuantity,cvv,month,year,fname,lname,cardnumber,cardtype,fee,Subtotal,Total,Shipping,Tax")]
        Paymentinfo paymentinfo)
        {
            Business business = new Business();

            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            if (!(business.Name == null))
            {
                item.price = "20";
            }
            else
            {
                item.price = paymentinfo.fee.ToString();
            }
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
            crdtCard.cvv2 = paymentinfo.cvv.ToString();  // CVV here
            crdtCard.expire_month = paymentinfo.month;
            crdtCard.expire_year = paymentinfo.year;
            crdtCard.first_name = paymentinfo.fname;
            crdtCard.last_name = paymentinfo.lname;
            crdtCard.number = paymentinfo.cardnumber.ToString(); //Card Number Here
            //crdtCard.number = paymentinfo.cardnumber.ToString(); //card number
            crdtCard.type = paymentinfo.cardtype;


            Details details = new Details();
            details.shipping = "0";
            details.subtotal = item.price;
            details.tax = "0";

            Amount amnt = new Amount();
            amnt.currency = "USD";

            amnt.total = details.subtotal;
            //amnt.total = paymentinfo.Total.ToString();
            amnt.details = details;


            Transaction tran = new Transaction();
            tran.amount = amnt;
            tran.description = "Description about the payment amount.";
            tran.item_list = itemList;
            tran.invoice_number = User.Identity.Name + DateTime.Now.Date + DateTime.Now.TimeOfDay;



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

            business.Name = TempData["Name"].ToString();
            business.City = TempData["City"].ToString();
            business.Address = TempData["Address"].ToString();
            business.Category = TempData["Category"].ToString();
            business.Description = TempData["Description"].ToString();
            business.Latitude = Convert.ToDouble(TempData["Latitude"]);
            business.Longitude = Convert.ToDouble(TempData["Longitude"]);
            business.Owner = System.Web.HttpContext.Current.User.Identity.Name;

            if (!(business.City == null))
            {
                if (ModelState.IsValid)
                {
                    db.Businesses.Add(business);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Businesses");
                }

                return View(business);
            }

            return View("Success");
        }
    }
}