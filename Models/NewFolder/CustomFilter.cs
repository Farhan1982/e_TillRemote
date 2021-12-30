using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters; 


namespace e_TillRemote.Models.NewFolder
{
    public class CustomFilter : Attribute,IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //var Result=new Dictionary<string, string>() ; 
            //if  (context.HttpContext.Request.Method.ToString()=="POST")
            //{

            //    //var Result = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller).Request.Form;

            //    Result = ((Microsoft.AspNetCore.Mvc.ControllerBase)context.Controller).Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            //    foreach (KeyValuePair<string, string> item in Result)

            //    {
            //        if (item.Key.Contains("DATE"))
            //        {

                        
            //            item.Value.Replace("-", "");
            //            Result[item.Key] = item.Value.ToString();
            //        }
            //    }
            //    //context.Result = (IActionResult)Result;

            //}

            
            
            
        }

        //public void OnResultExecuted(ResultExecutedContext context)
        //{
            
        //}

        //public void OnResultExecuting(ResultExecutingContext context)
        //{
            
        //}
    }


}
