using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XlsIO;

namespace AUTOHLT.CONSOLE
{
    class Program
    {
        private static List<Lincese> _allUserActive = new List<Lincese>();
        private static List<User> _allUser = new List<User>();
        private static async Task<List<Lincese>> GetUserActive()
        {
            var lince = new List<Lincese>();
            try
            {
                var client = new RestClient("https://api.autohlt.vn/api/version2/LicenseKey/All");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJFeHAiOjE2MTU0NjY2NzUsIklhdCI6MTYxNDg2MTg3NSwiSXNzIjoiYXV0b2hsdC5jb20iLCJJZFVzZXIiOiI1MjQzMmM3MS00MWI5LTRiYjYtOWEzZS1jOTI5NjM3ZGMwZWEiLCJVc2VyTmFtZSI6ImJvbmdsdiIsIlJvbGUiOjB9.qll7_mluziRJC4mUQ-f-NZSIP6SAFAjVQTPb1Nf_H8c");
                var response = await client.ExecuteAsync<AllUserActiveModel>(request);
                if (response.IsSuccessful)
                {
                    lince = response.Data.Data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return lince;
        }
        private static async Task<List<User>> GetAllUser()
        {
            var user = new List<User>();
            try
            {
                var client = new RestClient("https://api.autohlt.vn/api/v1/user/getalluser");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                var response = await client.ExecuteAsync<AllUserModel>(request);
                if (response.IsSuccessful)
                {
                    user = response.Data.Data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return user;
        }

        private static async Task GetData()
        {
            try
            {
                _allUser = await GetAllUser();
                _allUserActive = await GetUserActive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        static async Task Main(string[] args)
        {
            RegisterLicense();
            await GetData().ConfigureAwait(false);
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WindowWidth = 40;
            Console.WindowHeight = 40;
            var key = "";
            while (true)
            {
                Console.WriteLine("1. Lấy toàn bộ danh sách user");
                Console.WriteLine("2. In user đã kich hoạt");
                Console.WriteLine("3. In User chưa kích hoạt");
                Console.WriteLine("4. Tìm user");
                Console.WriteLine("5. Thoát");
                Console.Write("Mời chọn chức năng: ");
                key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        await PrintAllUser("TatCaKhachHang", _allUser);
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    default:
                        break;
                }

                Console.Clear();
            }
        }

        private static void RegisterLicense()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQwODc1QDMxMzkyZTMxMmUzMG4yOGdrMEd0bkZxeEVPUVZHNkJuRnFGY1dGNGpKMzJETnN0U0VsQlFHRFU9");
        }

        private static async Task PrintAllUser<T>(string name, List<T> data)
        {
            try
            {
                ExportExcel(name, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void ExportExcel<T>(string name, List<T> data)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "/" + name + "-" + DateTime.Now + ".xlsx";
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    var application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;
                    var workbook = application.Workbooks.Create(1);
                    var sheet = workbook.Worksheets[0];
                    var nameSheet = workbook.Worksheets[name];
                    //Import data from customerObjects collection
                    sheet.ImportData(data, 2, 1, false);
                    sheet.UsedRange.AutofitColumns();
                    //Save the file in the given path
                    var excelStream =
                        File.Create(path);
                    workbook.SaveAs(excelStream);
                    excelStream.Dispose();
                    File.op
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
