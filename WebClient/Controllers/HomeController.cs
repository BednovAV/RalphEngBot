using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace WebClient.Controllers
{
	public class HomeController : Controller
	{
        public string Index()
        {
            return "<h1>Hello world!</h1>";
        }
    }
}
