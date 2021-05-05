using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "/DATA"))
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/DATA");
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
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Title = "Tool lấy dữ liệu khách hàng";
            var key = "";
            RegisterLicense();
            await GetData().ConfigureAwait(false);
            while (true)
            {
                Console.WriteLine("1. Lấy toàn bộ danh sách user");
                Console.WriteLine("2. In user đã kich hoạt");
                Console.WriteLine("0. Thoát");
                Console.Write("Mời chọn chức năng: ");
                key = Console.ReadLine();
                switch (key)
                {
                    case "1":
                        await ExportExcel("Tat-Ca-Khach-Hang", _allUser);
                        break;
                    case "2":
                        var autovip = _allUser.Join(_allUserActive, user => user.ID, lincese => lincese.IdUser,
                            (user, lincese) => user);
                        await ExportExcel("Tat-Ca-Khach-Hang-Da-Nang-Cap-Tai-Khoan", autovip.ToList());
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "0":
                        Console.Write("Nhấn phím bất kỳ để thoát !");
                        Console.ReadKey();
                       return;
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

        private static Task ExportExcel<T>(string name, List<T> data)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "/DATA/" + name + "-" + DateTime.Now.Ticks + ".xlsx";
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    var application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;
                    var workbook = application.Workbooks.Create(1);
                    var sheet = workbook.Worksheets[0];
                    var nameSheet = workbook.Worksheets[name];
                    //Import data from customerObjects collection
                    sheet.ImportData(data, 2, 1, false);
                    sheet["A1"].Text = "ID người dùng";
                    sheet["B1"].Text = "Tài khoản";
                    sheet["C1"].Text = "Tên";
                    sheet["D1"].Text = "Số điện thoại";
                    sheet["E1"].Text = "Email";
                    sheet["F1"].Text = "Ngày đăng ký";
                    sheet["A1:F1"].CellStyle.Font.Bold = true;
                    sheet["A1:F1"].CellStyle.Font.Size = 15;

                    sheet.UsedRange.AutofitColumns();
                    //Save the file in the given path
                    var excelStream =
                        File.Create(path);
                    workbook.SaveAs(excelStream);
                    excelStream.Dispose();
                }

                OpenFile(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Task.FromResult(0);
        }

        private static void OpenFile(string path)
        {
            var startInfo = new ProcessStartInfo(path)
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                Arguments = Path.GetFileName(path)
            };
            Process.Start(startInfo);
        }
    }
}
