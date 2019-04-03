using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User()
            {
                krestni = "Petr",
                prijmeni = "Pan",
                datumnarozeni = DateTime.Now,
                mesto = "Praha",
                ulice = "Nabrezni",
                pohlavi = 0
            };

            var myContent = JsonConvert.SerializeObject(user);

            var keyValues = new List<KeyValuePair<string, string>>();

            string somedata = user.krestni;
            string somedata2 = user.prijmeni;
            DateTime somedata3 = user.datumnarozeni;
            string somedata4 = user.mesto;
            string somedata5 = user.ulice;
            int somedata6 = user.pohlavi;

            keyValues.Add(new KeyValuePair<string, string>("typ", "user"));
            keyValues.Add(new KeyValuePair<string, string>("krestni", somedata));
            keyValues.Add(new KeyValuePair<string, string>("prijmeni", somedata2));
            keyValues.Add(new KeyValuePair<string, string>("datumnarozeni", somedata3.ToString()));
            keyValues.Add(new KeyValuePair<string, string>("mesto", somedata4));
            keyValues.Add(new KeyValuePair<string, string>("ulice", somedata5));
            keyValues.Add(new KeyValuePair<string, string>("pohlavi", somedata6.ToString()));

            Console.WriteLine(user.datumnarozeni);

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/userapi.php");
            request.Content = new FormUrlEncodedContent(keyValues);
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);
            

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("typ", "ytsubs"));
            keyValues.Add(new KeyValuePair<string, string>("ytnick", txtJmeno.Text));
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/userapi.php");
            request.Content = new FormUrlEncodedContent(keyValues);
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic c = JsonConvert.DeserializeObject<RootObject>(responseContent);

            //Console.WriteLine(c.totalResults);
            int counter = 0;
            foreach(var item in c.items)
            {
                counter++;
            }
            if(counter!=0)
            {
                ctrOdberatele.Text = c.items[0].statistics.subscriberCount;
                ctrZhlednuti.Text = c.items[0].statistics.viewCount;
                ctrVidea.Text = c.items[0].statistics.videoCount;
            }
            else
            {
                ctrOdberatele.Text = "Uživatel nenalezen";
                ctrZhlednuti.Text = "Uživatel nenalezen";
                ctrVidea.Text = "Uživatel nenalezen";
            }
            
            
        }
        
    }
    public class PageInfo
    {
        public int totalResults { get; set; }
        public int resultsPerPage { get; set; }
    }

    public class Default
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Medium
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class High
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Thumbnails
    {
        public Default @default { get; set; }
        public Medium medium { get; set; }
        public High high { get; set; }
    }

    public class Localized
    {
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Snippet
    {
        public string title { get; set; }
        public string description { get; set; }
        public string customUrl { get; set; }
        public DateTime publishedAt { get; set; }
        public Thumbnails thumbnails { get; set; }
        public Localized localized { get; set; }
        public string country { get; set; }
    }

    public class RelatedPlaylists
    {
        public string favorites { get; set; }
        public string uploads { get; set; }
        public string watchHistory { get; set; }
        public string watchLater { get; set; }
    }

    public class ContentDetails
    {
        public RelatedPlaylists relatedPlaylists { get; set; }
    }

    public class Statistics
    {
        public string viewCount { get; set; }
        public string commentCount { get; set; }
        public string subscriberCount { get; set; }
        public bool hiddenSubscriberCount { get; set; }
        public string videoCount { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public Snippet snippet { get; set; }
        public ContentDetails contentDetails { get; set; }
        public Statistics statistics { get; set; }
    }

    public class RootObject
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<Item> items { get; set; }
    }
}
