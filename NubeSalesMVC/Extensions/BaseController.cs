using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Extensions
{
    public enum NotificationType
    {
        success,
        Error,
        Info
    }

    public class BaseController : Controller
    {
        public void BasicNotification(string msg, NotificationType type, string title = "")
        {
            TempData["notification"] = $"Swal.fire('{title}', '{msg}','{type.ToString().ToLower()}')";
        }

    }
}
