using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SearchUsing_LLm.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(string Username, string Password, string rememberMe)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ViewBag.LoginError = "Please enter username and password.";
                return View();
            }

            // TODO: Replace this simple check with real authentication logic
            if (Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && Password == "123")
            {
                Session["User"] = Username;
                return RedirectToAction("Index");
            }

            ViewBag.LoginError = "Invalid username or password.";
            return View();
        }

        public ActionResult Searchcontent()
        {
            return View();
        }

        public ActionResult Uploaddoc()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> Uploaddoc(HttpPostedFileBase file, string FirstName)
        {
            charity obj_ = new charity();
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                string name = FirstName + Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/oldtempuser/"), name);

                file.SaveAs(path);
                if (Path.GetExtension(file.FileName).ToUpper() == ".PNG" || Path.GetExtension(file.FileName).ToUpper() == ".JPG")
                {
                    CompressImage(path, Server.MapPath("~/Content/userdoc/"), (int)50);
                }

                path = path.Replace("\\", "\\\\");
                var client = new System.Net.Http.HttpClient();
                //   var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:5000/upload");
                var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.1.92:5000/upload");
                var content = new StringContent("{\"img1\" : \"" + path + "\"}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);

                ViewBag.savemessage = "Saved Successfully";

            }


            return View(obj_);
        }

       
        [HttpPost]
        public async Task<ActionResult> Searchcontent(HttpPostedFileBase file, string searchtext)
        {
            charity obj_ = new charity();
            obj_.imgnew = "";
            obj_.message = "";
            if (file != null && file.ContentLength > 0)
            {

                var fileName_ = Path.GetFileNameWithoutExtension(file.FileName);

                string name_ = fileName_ + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var path_ = Path.Combine(Server.MapPath("~/Content/tempuser/"), name_);
                obj_.imgnew = name_;
                file.SaveAs(path_);
                //if (Path.GetExtension(file.FileName).ToUpper() == ".PNG" || Path.GetExtension(file.FileName).ToUpper() == ".JPG")
                //{
                //    CompressImage(path_, Server.MapPath("~/Content/tempuser/"), (int)50);
                //}
                path_ = path_.Replace("\\", "\\\\");
                var client = new System.Net.Http.HttpClient();
                //var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:5000/ask");
                var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.1.92:5000/ask");
                var content = new StringContent("{\"img1\" : \"" + path_ + "\"}", null, "application/json");
                var content2 = new StringContent("{\"search\" : \"" + searchtext + "\"}", null, "application/json");
                request.Content = content2;
                var response = await client.SendAsync(request);

                if (response.StatusCode.ToString() == "OK")
                {

                    var result = response.Content.ReadAsStringAsync().Result;
                    //List<Root_> myDeserializedClass = JsonConvert.DeserializeObject<List<Root_>>(result);
                    string[] arr = null;
                    if (result.Contains(','))
                    {

                        string[] img = result.TrimStart('[').TrimEnd(']').Split(',');
                        arr = new string[img.Length];
                        for (int i = 0; i < img.Length; i++)
                        {
                            string[] arrd = img[i].Split('\\');
                            arr[i] = arrd[arrd.Length - 1].TrimEnd('"');
                        }

                    }
                    else
                    {

                        string img = result.TrimStart('[').TrimEnd(']');
                        arr = new string[1];
                        string[] arrd = img.Split('\\');

                        arr[0] = arrd[arrd.Length - 1].TrimEnd('"');
                    }
                    obj_.arr = arr;

                    // Console.WriteLine(await response.Content.ReadAsStringAsync());
                }

                else
                {
                    obj_.message = "No result found.";
                }

            }



            return View(obj_);
        }
        public static void CompressImage(string SoucePath, string DestPath, int quality)
        {
            var FileName = Path.GetFileName(SoucePath);
            DestPath = DestPath + "\\" + FileName;
            using (Bitmap bmp1 = new Bitmap(SoucePath))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, quality);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp1.Save(DestPath, jpgEncoder, myEncoderParameters);
            }
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class charity
    {
        public string[] arr { get; set; }
        public string imgnew { get; set; }
        public string message { get; set; }
    }
}