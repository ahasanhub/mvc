using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AccountKit.Models;
using Newtonsoft.Json.Linq;

namespace AccountKit.Controllers
{
    public class ExternalAuthSocialController : Controller
    {
        #region Const

        private const string TokenEndpoint = "https://graph.accountkit.com/{0}/access_token";
        private const string UserInfoEndpoint = "https://graph.accountkit.com/{0}/me";

        #endregion

        #region Fields

        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string _api_Version;
        private readonly string _languageCulture;

        private readonly HttpContextBase _httpContext;

        #endregion

        #region Ctor
        public ExternalAuthSocialController()
        {
            this._httpContext = HttpContext;
            _api_Version = "v1.1";
            _appId = "444985472563667";

        }
        #endregion

        #region Utils

        [NonAction]
        private ExternalAutentificationModel PrepareExternalAutentificationModel()
        {
            var model = new ExternalAutentificationModel()
            {
                AccountKit_ClientKeyIdentifier = _appId,
                AccountKit_API_Version = _api_Version,
                AccountKit_CSRF = Guid.NewGuid().ToString(),
                AccountKit_Locale = _languageCulture
            };

            if (string.IsNullOrWhiteSpace(model.AccountKit_API_Version))
                model.AccountKit_API_Version = "v1.0";

            string[] accountKitLocales = new[]{
                    "af_AF","ar_AR","bn_IN","zh_CN","zh_HK","zh_TW","hr_HR","cs_CZ","da_DK","nl_NL","en_US","fi_FI",
                    "fr_FR","de_DE","el_GR","gu_IN","he_IL","hi_IN","hu_HU","id_ID","it_IT","ja_JP","ko_KR","ms_MY","ml_IN","mr_IN",
                    "nb_NO","pl_PL","pt_BR","pt_PT","pa_IN","ro_RO","ru_RU","sk_SK","es_LA","es_ES","sv_SE","tl_PH","ta_IN","te_IN",
                    "th_TH","tr_TR","vi_VN"
                };
            if (!accountKitLocales.Contains(model.AccountKit_Locale))
                model.AccountKit_Locale = "en_US";

            return model;
        }

        [NonAction]
        private string QueryAccessToken(string code)
        {
            var access_token = string.Join("|", new string[] { "AA", this._appId, this._appSecret });
            var uri = BuildUri(string.Format(TokenEndpoint, this._api_Version), new NameValueCollection {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "access_token", access_token }
            });
            string accessToken = "";
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                using (var webResponse = webRequest.GetResponse())
                using (var stream = webResponse.GetResponseStream())
                {
                    if (stream == null)
                        return null;
                    using (var textReader = new StreamReader(stream))
                    {
                        var response = JObject.Parse(textReader.ReadToEnd());
                        accessToken = response["access_token"].Value<string>();
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return accessToken;
        }

        [NonAction]
        private static Uri BuildUri(string baseUri, NameValueCollection queryParameters)
        {
            var q = HttpUtility.ParseQueryString(string.Empty);
            q.Add(queryParameters);
            var builder = new UriBuilder(baseUri) { Query = q.ToString() };
            return builder.Uri;
        }

        [NonAction]
        private string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2"); /* hex format */
            return sbinary;
        }

        [NonAction]
        private  IDictionary<string, string> GetUserData(string accessToken)
        {
            //$appsecret_proof = hash_hmac('sha256', $access_token, $app_secret); 
            var appsecret_proof = "";
            //hash_hmac("sha256", "message", "key")
            var keyByte = Encoding.UTF8.GetBytes(this._appSecret);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(accessToken));
                appsecret_proof = ByteToString(hmacsha256.Hash).ToLower();
            }

            var uri = BuildUri(string.Format(UserInfoEndpoint, _api_Version), new NameValueCollection {
                {"access_token", accessToken},
                {"appsecret_proof", appsecret_proof}
            });
            var extraData = new Dictionary<string, string>();
            string content = "";
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                using (var webResponse = webRequest.GetResponse())
                {
                    using (var stream = webResponse.GetResponseStream())
                    {
                        if (stream == null)
                            return null;
                        using (var textReader = new StreamReader(stream))
                            content = textReader.ReadToEnd();
                    }
                }
            }
            catch (WebException wex)
            {
                string pageContent = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd().ToString();
                var responseEx = JObject.Parse(pageContent);
                if (!string.IsNullOrWhiteSpace(responseEx["error"]["message"].Value<string>()))
                {
                    var message = responseEx["error"]["message"].Value<string>();
                    extraData.Add("error", message);
                }
                else
                    extraData.Add("error", pageContent);
                return extraData;
            }
            catch (Exception exp)
            {
                extraData.Add("error", exp.Message);
                return extraData;
            }
            var response = JObject.Parse(content);

            /*
{"email":{"address":"youremail\u0040gmail.com"},"id":"1714960621120239"}                      
             */

            extraData.Add("id", response["id"].Value<string>().Trim());
            extraData.Add("accesstoken", accessToken);

            if (response["email"] != null &&
               !string.IsNullOrWhiteSpace(response["email"]["address"].Value<string>()))
                extraData.Add("email", response["email"]["address"].Value<string>().Trim());

            if (response["phone"] != null &&
                !string.IsNullOrWhiteSpace(response["phone"]["number"].Value<string>()))
                extraData.Add("email", response["phone"]["number"].Value<string>().Trim());

            return extraData;
        }

        #endregion

        #region PublicInfo

        //[ChildActionOnly]
        public ActionResult PublicInfo()
        {
            var model = PrepareExternalAutentificationModel();

            //if (_httpContext.Session["externalAutentificationAccountKitCSRF"] == null)
            //    _httpContext.Session["externalAutentificationAccountKitCSRF"] = model.AccountKit_CSRF;
            //else
            //    model.AccountKit_CSRF = _httpContext.Session["externalAutentificationAccountKitCSRF"] as string;

            return View("ExternalAutentification", model);
        }

        #endregion

        #region AccountKit

        [HttpPost]
        public ActionResult AccountKitLoginPost(string code, string csrf_nonce, string returnUrl)
        {
            string csrf = "";
            if (_httpContext.Session["externalAutentificationAccountKitCSRF"] != null)
                csrf = _httpContext.Session["externalAutentificationAccountKitCSRF"] as string;

            // CSRF check
            if (string.IsNullOrWhiteSpace(csrf) || string.IsNullOrWhiteSpace(csrf_nonce) ||
                    !csrf_nonce.Equals(csrf))
            {
                return Json(new
                {
                    success = false,
                    returnUrl = returnUrl,
                    message = "Wrong CSRF check"
                });
            }
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new
                {
                    success = false,
                    returnUrl = returnUrl,
                    message = "Wrong response code"
                });
            }

            var accessToken = this.QueryAccessToken(code);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return Json(new
                {
                    success = false,
                    returnUrl = returnUrl,
                    message = "Empty accessToken"
                });
            }
            var userData = this.GetUserData(accessToken);
            if (userData.ContainsKey("error") && !string.IsNullOrWhiteSpace(userData["error"]))
                return Json(new
                {
                    success = false,
                    returnUrl = returnUrl,
                    message = userData["error"]
                });

            StringBuilder sb = new StringBuilder();
            foreach (var para in userData)
                sb.AppendLine(string.Format("   key={0}, value={1}", para.Key, para.Value));

            return Json(new
            {
                success = false,
                returnUrl = returnUrl,
                message = sb.ToString()
            });
        }

        #endregion
    }
}