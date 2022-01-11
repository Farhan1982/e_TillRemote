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


namespace e_TillRemote.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");

            //SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial catalog=eTill;Integrated Security=true");
            //SqlConnection con = new SqlConnection("Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=True");



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
            SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");

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


            SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");

            var stock = con.Query<AddStock>("usp_GetStock", commandType: CommandType.StoredProcedure).ToList();

            stock = null;
            return View(stock);


        }
        [HttpPost]
        public ActionResult filterStockView(string Stockcol, string colname)
        {

            SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");

            var param = new DynamicParameters();
            param.Add("@DDLCOLNAME", Stockcol);

            param.Add("@STRINGVALUE", colname);
            var stock = con.Query<AddStock>("usp_GetStocksDetailsByColumnNames", param: param, commandType: CommandType.StoredProcedure).ToList();

            return View("StockView", stock);

        }
        [HttpPost]
        public JsonResult StockData(string Prefix)
        {


           SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");
            
            var param = new DynamicParameters();
            //param.Add("@NAME", name);
            
            var stock = con.Query<Section2>("usp_GetSectionDetails", commandType: CommandType.StoredProcedure).ToArray();

            var Name = (from N in stock
                        where N.NAME.ToLower().StartsWith(Prefix.ToLower())
                        select new { N.NAME });



            return Json(stock);


        }



        public ActionResult StockDataTest()
        {

                      

            return View();
        }

       [HttpPost]
        public JsonResult SectionData(string tablename)
        {


           SqlConnection con = new SqlConnection(@"Data Source=MOSAPP;Initial Catalog=eTill;Integrated Security=true");
            
            var param = new DynamicParameters();
            param.Add("@TABLE", tablename);
            
            var sectionsData = con.Query<Section2>("uspt_getSectionData", param: param, commandType: CommandType.StoredProcedure).ToList();

            ViewBag.SECTION1 = new SelectList(sectionsData, "CODE", "NAME").ToList();
            return Json(sectionsData);


        }



    }

    public class Auto
    {
        public string name { get; set; }

    }



}
