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
using System.IO;

namespace WebPortal.Controllers
{
    public class PaypalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Paypal
        public ActionResult Index([Bind(Include = "ID,Name,City,Address,Category,Description,Image,Latitude,Longitude,Owner")] Business business,
            [Bind(Include = "ID,Title,Venue,Description,Producer,Image,Time,Date,Cinema,Comments")] Movie movie, HttpPostedFileBase file)
        {
            if(business.Name != null)
            {
                string fileName = DateTime.Now.DayOfYear.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString()
                + DateTime.Now.Second.ToString() + System.Web.HttpContext.Current.User.Identity.Name + "business.jpg";
                string fileType = fileName.Substring(fileName.LastIndexOf('.'));
                if ((file != null && file.ContentLength > 0) && ((fileType == ".jpg") || (fileType == ".jpeg") || (fileType == ".png")))
                {
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/businessimages/")) + fileName;
                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                        string filePathString = path;
                        business.Image = fileName;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Error: File is Not Selected or is not an image. Upload only \".jpg\" \".jpeg\" or \".png\" file types" + ex.Message.ToString();
                    }

                }
                else
                {
                    ViewBag.Message = "You have not specified a file. ";
                }

                TempData["Name"] = business.Name;
                TempData["City"] = business.City;
                TempData["Address"] = business.Address;
                TempData["Category"] = business.Category;
                TempData["Description"] = business.Description;
                TempData["Image"] = business.Image;
                TempData["Latitude"] = business.Latitude;
                TempData["Longitude"] = business.Longitude;

                ViewBag.price = 20;

                return View("index");
            }

            if (movie.Cinema != null)
            {
                TempData["Time"] = movie.Time;
                TempData["Date"] = movie.Date;
                TempData["Cinema"] = movie.Cinema;

                ViewBag.price = 5;

                return View("index");
            }

            return View ("Failure");
        }
        public ActionResult PaymentWithCreditCard([Bind(Include = "ID,ItemName,ItemPrice,ItemQuantity,cvv,month,year,fname,lname,cardnumber,cardtype,fee,Subtotal,Total,Shipping,Tax")]
        Paymentinfo paymentinfo)
        {
            Business business = new Business();
            Movie movie = new Movie();

            Item item = new Item();
            item.name = "Demo Item";
            item.currency = "USD";
            if (TempData["Name"] != null)
            {
                item.price = "20";
            }
            else
            {
                item.price = "5";
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
            crdtCard.type = paymentinfo.cardtype;


            Details details = new Details();
            details.shipping = "0";
            details.subtotal = item.price;
            details.tax = "0";

            Amount amnt = new Amount();
            amnt.currency = "USD";

            amnt.total = details.subtotal;
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
            if (TempData["Name"] != null)
            {
                business.Name = TempData["Name"].ToString();
                business.City = TempData["City"].ToString();
                business.Address = TempData["Address"].ToString();
                business.Category = TempData["Category"].ToString();
                business.Description = TempData["Description"].ToString();
                business.Image = TempData["Image"].ToString();
                business.Latitude = Convert.ToDouble(TempData["Latitude"]);
                business.Longitude = Convert.ToDouble(TempData["Longitude"]);
                business.Owner = System.Web.HttpContext.Current.User.Identity.Name;
            }
            


            if (business.Name != null)
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