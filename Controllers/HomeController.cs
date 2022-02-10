using e_TillRemote.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace e_TillRemote.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration configuration;
        private IWebHostEnvironment webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, IConfiguration _configuration, IWebHostEnvironment _webHostEnvironment)
        {
            _logger = logger;
            configuration = _configuration;
            webHostEnvironment = _webHostEnvironment;
        }

        //public IActionResult Index()
        //{



        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Index()
        {

            //connection string
            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");

            //SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial catalog=eTill;Integrated Security=true");
            //SqlConnection con = new SqlConnection("Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=True");



            //stored procedures start
            var CustomControl = con.Query<StockScreen>("StockProc", commandType: CommandType.StoredProcedure).ToList();

            var Sec1 = con.Query<Section1>("SectionProc1", commandType: CommandType.StoredProcedure).ToList();

            var Sec2 = con.Query<Section2>("SectionProc2", commandType: CommandType.StoredProcedure).ToList();




            //stored procedures end

            ViewBag.SECTION1 = new SelectList(Sec1, "CODE", "NAME").ToList();
            ViewBag.SECTION2 = new SelectList(Sec2, "CODE", "NAME").ToList();

            return View("Index", CustomControl);
        }

        [HttpPost]
        public ActionResult Index2(Dictionary<string, string> Model)
        {

            //connection string
            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");

            //stored procedures start


            var param = new DynamicParameters();
            foreach (KeyValuePair<string, string> item in Model)

            {
                if (item.Key != "__RequestVerificationToken")
                {



                    param.Add(item.Key, item.Value);
                }
            }


            //param.Add("@ITEMCODE", stock.ITEMCODE);
            //param.Add("@DESCRPTON1", stock.DESCRPTON1);
            //param.Add("@DESCRPTON2", stock.DESCRPTON2);
            //param.Add("@SUPPLIER", stock.SUPPLIER);
            //param.Add("@Q_IN_HAND", stock.Q_IN_HAND.ToString());
            //param.Add("@SECTION1", stock.SECTION1);
            //param.Add("@SECTION2", stock.SECTION2);
            //param.Add("@SDATE_FROM", stock.SDATE_FROM);
            //param.Add("@SDATE_TO", stock.SDATE_TO);

            //foreach (var item in stock)
            //{


            //}



            var Sec1 = con.Query<Section1>("SectionProc1", commandType: CommandType.StoredProcedure).ToList();
            var Sec2 = con.Query<Section2>("SectionProc2", commandType: CommandType.StoredProcedure).ToList();




            //stored procedures end

            ViewBag.SECTION1 = new SelectList(Sec1, "CODE", "NAME").ToList();
            ViewBag.SECTION2 = new SelectList(Sec2, "CODE", "NAME").ToList();

            int result = con.Execute("usp_AddStock", param: param, commandType: CommandType.StoredProcedure);
            //return View("Index", CustomControl);
            return View("Index");
        }


        public ActionResult StockView()
        {


            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");

            var stock = con.Query<AddStock>("usp_GetStock", commandType: CommandType.StoredProcedure).ToList();

            stock = null;
            return View(stock);


        }
        [HttpPost]
        public ActionResult filterStockView(string Stockcol, string colname,string Section)
        {
            if(!string.IsNullOrEmpty(Section))
            {
                colname = Section;
            }

            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");

            var param = new DynamicParameters();
            param.Add("@DDLCOLNAME", Stockcol);

            param.Add("@STRINGVALUE", colname);
            var stock = con.Query<AddStock>("usp_GetStocksDetailsByColumnNames", param: param, commandType: CommandType.StoredProcedure).ToList();

            return View("StockView", stock);

        }
        [HttpPost]
        public JsonResult StockData(string Prefix)
        {


           SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            
            var param = new DynamicParameters();
           
            
            var stock = con.Query<Section2>("usp_GetSectionDetails", commandType: CommandType.StoredProcedure).ToArray();

            var Name = (from N in stock
                        where N.NAME.ToLower().StartsWith(Prefix.ToLower())
                        select new { N.NAME });



            return Json(stock);


        }

        public IActionResult MainMenu()
        {
            return View();
        }

        public ActionResult StockDataTest()
        {
            return View();
        }

       [HttpPost]
        public JsonResult SectionData(string tablename)
        {
           SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            
            var param = new DynamicParameters();
            param.Add("@TABLE", tablename);
            
            var sectionsData = con.Query<Section2>("uspt_getSectionData", param: param, commandType: CommandType.StoredProcedure).ToList();

            ViewBag.SECTION1 = new SelectList(sectionsData, "CODE", "NAME").ToList();
            return Json(sectionsData);


        }

        [HttpPost]
        public IActionResult SendMail(MailModel mailModel, IFormFile[] attachments)
        {
            
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailModel.emailFromAddress);
                    mail.To.Add(mailModel.emailToAddress);
                    mail.Subject = mailModel.subject;
                    mail.Body = mailModel.body;
                    mail.IsBodyHtml = true;
                // mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                List<string> fileNames = null;

                if (attachments != null && attachments.Length > 0)
                {
                    fileNames = new List<string>();
                    foreach (IFormFile attachment in attachments)
                    {
                        var path = Path.Combine(webHostEnvironment.WebRootPath, "uploads", attachment.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            attachment.CopyToAsync(stream);
                        }
                        fileNames.Add(path);
                    }
                }

                if (attachments != null)
                {
                    foreach (var attachment in fileNames)
                    {
                        mail.Attachments.Add(new Attachment(attachment));
                    }
                }



                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("mouzamalimohd749@gmail.com", "Yawer1989");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            
        
            return Redirect("~/Home/StockView");
        }


        public ActionResult getAttachedFileOnSendMailClick()
        {
            var address = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
            var files = Directory.GetFiles(address, "*")
                .OrderByDescending(d => new FileInfo(d).CreationTime).Take(1);
            FileInfo fi = new FileInfo(files.SingleOrDefault());
            var f = fi;
            return Json(f.FullName);
        }

        public ViewResult FirstPage()
        {
            return View();
        }

        public IActionResult StockBreakup()
        {
           string TypeSection= HttpContext.Request.Query["TypeSection"].ToString();
            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            var param = new DynamicParameters();
            param.Add("@TypeSection", TypeSection);
            var sectionsData = con.Query<Section1>("usp_GetSections", param: param, commandType: CommandType.StoredProcedure);
            return View(sectionsData);
        }
        

         [HttpPost]
        public IActionResult SaveStockBreakup(Section1 section1,string TypeSection)
        {

            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            var param = new DynamicParameters();
            param.Add("@TypeSection", TypeSection);
            param.Add("@Code", section1.CODE);
            param.Add("@Name", section1.NAME);
            param.Add("@Name1", section1.NAME1);
            param.Add("@PuNo", section1.PU_NO);
            param.Add("@PuDesc", section1.PU_DESC);
            
            var sectionsData = con.Execute("usp_InsertSectionData", param: param, commandType: CommandType.StoredProcedure);
            return Redirect("~/Home/StockBreakup?TypeSection="+ TypeSection);
        }
        [HttpPost]
        public IActionResult UpdateStockBreakup(Section1 section1, string TypeSection)
        {

            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            var param = new DynamicParameters();
            param.Add("@TypeSection", TypeSection);
            param.Add("@Code", section1.CODE);
            param.Add("@Name", section1.NAME);
            param.Add("@Name1", section1.NAME1);
            param.Add("@PuNo", section1.PU_NO);
            param.Add("@PuDesc", section1.PU_DESC);
            param.Add("@SecId", section1.SecId);
            var sectionsData = con.Execute("usp_UpdateSectionData", param: param, commandType: CommandType.StoredProcedure);
            return Redirect("~/Home/StockBreakup?TypeSection=" + TypeSection);

        }
        [HttpPost]
        public IActionResult DeleteStockBreakup(int SecId, string TypeSection)
        {

            SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
            var param = new DynamicParameters();
            param.Add("@TypeSection", TypeSection);
            param.Add("@SecId", SecId);
            var sectionsData = con.Execute("usp_DeleteSections", param: param, commandType: CommandType.StoredProcedure);
            return Redirect("~/Home/StockBreakup?TypeSection=" + TypeSection);

        }
        //public ViewResult SaveStockBreakup(string TypeSection)
        //{
        //    SqlConnection con = new SqlConnection(@"Data Source=AZAM-PC\SQLEXPRESS;Initial Catalog=eTill;Integrated Security=true");
        //    var param = new DynamicParameters();
        //    param.Add("@TypeSection", TypeSection);
        //    var sectionsData = con.Query<Section1>("usp_GetSections", param: param, commandType: CommandType.StoredProcedure);
        //    return View(sectionsData);
        //}

    }

    public class Auto
    {
        public string name { get; set; }

    }

    

}
