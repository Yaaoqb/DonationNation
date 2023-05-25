using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Linq;
using DonationNation.Data;
using DonationNation.Data.Models;

namespace DonationNation.Filters
{
    public class UserActionFilter : IActionFilter
    {
        private readonly ApplicationDbContext _context;
        public UserActionFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string activityData = "";

            var route = context.RouteData;

            var area = route.Values["area"];


            if (area != null)
            {
                var url = $"{area}/{route.Values["controller"]}/{route.Values["action"]}";
                var user = context.HttpContext.User.Identity.Name;
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

                if (!string.IsNullOrEmpty(context.HttpContext.Request.QueryString.Value))
                {
                    activityData = context.HttpContext.Request.QueryString.Value;
                }
                else
                {
                    var activity = context.ActionArguments.FirstOrDefault().Value;
                    activityData = JsonConvert.SerializeObject(activity);
                }
                if (!String.IsNullOrEmpty(activityData))
                {
                    SaveActivity(activityData, url, user, ipAddress);
                }
            }

        }

        private void SaveActivity(string activityData, string url, string user, string ipAddress)
        {
            UserActivity ua = new UserActivity
            {
                Data = activityData,
                Url = url,
                UserName = user,
                IPAddress = ipAddress
            };
            _context.UserActivities.Add(ua);
            _context.SaveChanges();
        }
    }
}
