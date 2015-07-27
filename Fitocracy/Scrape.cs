﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace Fitocracy
{
    public class Scrape
    {
        const string BaseUrl = "https://www.fitocracy.com";

        public static int GetUserId(string userName)
        {
            var loginResponse = Cacheable.Login();
            var userId = GetUserId(userName, loginResponse);
            var dauberUserId = ConfigurationManager.AppSettings["FitocracyUserId"];
            return userId == dauberUserId ? int.Parse(dauberUserId) : int.Parse(userId); //if you visit a profile that doesn't exist, you just see your own
        }

        internal static CookieCollection Login()
        {
            #region GetLogin
            const string loginUrl = BaseUrl + "/accounts/login/";
            var cookieJar = new CookieContainer();
            var request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.CookieContainer = cookieJar;
            request.UserAgent = "Dauber";
            var getResponse = request.GetResponse();
            var tokenCookie = request.CookieContainer.GetCookies(new Uri(BaseUrl))["csrftoken"];
            #endregion

            #region PostLogin
            request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.CookieContainer = cookieJar;
            request.Method = "POST";
            request.Referer = loginUrl;
            request.UserAgent = "Dauber";

            string username = ConfigurationManager.AppSettings["FitocracyUsername"];
            string password = ConfigurationManager.AppSettings["FitocracyPassword"];
            using (var stream = request.GetRequestStream())
            {
                var bytes = Encoding.UTF8.GetBytes(FormatArgs(new { csrfmiddlewaretoken = tokenCookie.Value, username = username, password = password, json = 1, is_username = 1 }));
                stream.Write(bytes, 0, bytes.Length);
            }

            var getLoginResponse = request.GetResponse() as HttpWebResponse;
            #endregion

            return getLoginResponse.Cookies;
        }

        internal static string GetUserId(string userName, CookieCollection cookies)
        {
            var cookieJar = new CookieContainer();
            cookieJar.Add(cookies);
            #region GetProfile - Gets userId
            var profileUrl = BaseUrl + "/profile/" + userName;
            var request = (HttpWebRequest)WebRequest.Create(profileUrl);
            request.CookieContainer = cookieJar;

            request.UserAgent = "Dauber";

            var profilePage = new HtmlDocument();

            using (var response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    profilePage.Load(stream);
                }
            }

            // <form id="duel_form" name="duel_form" method="POST" action="/set_duel/1867535/">
            //var userId = profilePage.DocumentNode
            //    .SelectSingleNode("//input[@type='hidden' and @name='profile_user']")
            //    .Attributes["value"].Value;

            var userId = profilePage.GetElementbyId("duel_form")
                .GetAttributeValue("action", string.Empty)
                .Replace("/set_duel/", string.Empty).Replace("/", string.Empty);
            #endregion

            return userId;
        }

        private static string FormatArgs(object args)
        {
            return string.Join(
                "&",
                args.GetType()
                    .GetProperties()
                    .Select(prop => string.Format("{0}={1}", HttpUtility.UrlEncode(prop.Name), HttpUtility.UrlEncode(prop.GetValue(args).ToString()))));
        }

    }
}
