using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore_CardTrading.Ultities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore_CardTrading.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IGmailService _gmailService;
        public HomeController(IGmailService gmailService)
        {
            _gmailService = gmailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Map()
        {
            await _gmailService.Send(new Data.GmailData() { 
                from = "k300nonereply@gmail.com",
                to = "nxgthanhcongtln@gmail.com",
                subject = "subject",
                title = "title",
                mainContent = "mainContent"
            });
            return View();
        }
    }
}
