using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AppBuku.Models;

namespace AppBuku.TMobileFromWeb.ViewModels
{
    public class UjiWebServiceViewModel : BaseViewModel
    {
        // Deklarasi MyHttpClient Service + Constructor Class
        Services.MyHttpClient myHttpClient;
        Random rnd = new Random();
        string[] NamaReviewers = new string[] { "Adam", "Samuel", "Dave", "Joe" };

        public UjiWebServiceViewModel()
        {
            Title = "UJI WEB SERVICE";
            reviewBukuGet = new AppBuku.Models.ReviewBuku();
            IsBusy = true;

            string baseUri = Application.Current.Properties["BaseWebUri"] as string;
            myHttpClient = new Services.MyHttpClient(baseUri);
            IsBusy = false;
            
        }

        private ICommand cmdGetData;
        public ICommand CmdGetData
        {
            get
            {
                if (cmdGetData == null)
                {
                    cmdGetData = new Command(async() => await PerformCmdGetDataAsync());
                }

                return cmdGetData;
            }
        }

        private async Task PerformCmdGetDataAsync()
        {
            if (!myHttpClient.IsEnable)
            {
                HasilGet = "MyHttpClient disabled!";
                return;
            }

            IsBusy = true;
            try
            {
                string hsl = await myHttpClient.HttpGet("api/XReview", "1");
                HasilGet = hsl;
                ReviewBukuGet = JsonConvert.DeserializeObject<AppBuku.Models.ReviewBuku>(hsl);
            }
            catch (Exception ex)
            {
                HasilGet = "ERROR: " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
            
        }

        private AppBuku.Models.ReviewBuku reviewBukuGet;
        public AppBuku.Models.ReviewBuku ReviewBukuGet 
        { get => reviewBukuGet; set => SetProperty(ref reviewBukuGet, value); }

        private ICommand cmdPutData;

        public ICommand CmdPutData
        {
            get
            {
                if (cmdPutData == null)
                {
                    cmdPutData = new Command(async() => await PerformCmdPutDataAsync());
                }

                return cmdPutData;
            }
        }

        private async Task PerformCmdPutDataAsync()
        {
            string nama = NamaReviewers[rnd.Next(NamaReviewers.Length)];
            int rating = rnd.Next(1, 5);
            ReviewBuku r1 = new ReviewBuku()
            {
                Id = 1,
                BukuId = 1,
                Nama = nama,
                Rating = rating,
                IsiReview = $"Bapak {nama} menyatakan bahwa nilai buku adalah {rating}"
            };
           
            IsBusy = true;
            try
            {
                string hsl = await myHttpClient.HttpPut("api/XReview", "1", r1);
                StatusPut = hsl;
            }
            catch (Exception ex)
            {
                HasilGet = "ERROR: " + ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

       

        private string statusPut;
        public string StatusPut { get => statusPut; set => SetProperty(ref statusPut, value); }

       
        private string hasilGet;
        public string HasilGet { get => hasilGet; set => SetProperty(ref hasilGet, value); }
    }
}
