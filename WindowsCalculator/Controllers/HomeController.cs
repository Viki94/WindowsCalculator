using WindowsCalculator.Models;
using System;
using System.Web.Mvc;

namespace WindowsCalculator.Controllers
{
    public class HomeController : Controller
    {
        public CalculatorModel Calculator;

        public ActionResult Index()
        {
            Calculator = SessionBagModel.Current.calcInstance;
            if (Calculator == null)
            {
                Calculator = new CalculatorModel();
            }

            string num = Request.QueryString["num"];
            string Operation = Request.QueryString["op"];
            if (num != null)
            {
                int number;
                if (Int32.TryParse(num, out number))
                {
                    Calculator.ProcessNumericButton(number);
                }
            }
            else if (Operation != null)
            {
                Calculator.ProcessOperationButton(Operation);
            }

            return View(Calculator);
        }
    }
}