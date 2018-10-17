using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPCogBingSpellCheck
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public class Spellc
    {
        public string OffsetProperty { get; set; }
        public string tokenProperty { get; set; }
        public string SuggestionProperty { get; set; }

    }

    public class Spellcol
    {
        public Spellc spellc { get; set; }
    }

    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Spellcol> SearchResults { get; set; } = new ObservableCollection<Spellcol>();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnSpellchk_Click(object sender, RoutedEventArgs e)
        {
            var sp = await spellcheck();
            for (int i = 0; i < sp.Count(); i++)
            {
                Spellc sc = sp.ElementAt(i);
                SearchResults.Add(new Spellcol { spellc = sc });
            }
        }
        async Task<IEnumerable<Spellc>> spellcheck()
        {

            List<Spellc> spelc = new List<Spellc>();
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9456ddf5ea3b4752ac8f87b63a7e6013");

            string text = "The Spell Check lets patners chck a text sting for speling and gramar erors. ";
            string mode = "proof";
            string mkt = "en-us";
            var endPoint = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?";
            HttpResponseMessage httpResponseMessage = await client.GetAsync(string.Format("{0}text={1}&mode={2}&mkt={3}", endPoint, text, mode, mkt));

            await WsAsync(spelc, httpResponseMessage);
            return spelc;
        }

        private static async Task WsAsync(List<Spellc> spelc, HttpResponseMessage httpResponseMessage)
        {
            httpResponseMessage.EnsureSuccessStatusCode();
            string readAsString = await httpResponseMessage.Content.ReadAsStringAsync();
            dynamic dataObject = JObject.Parse(readAsString);

            for (int i = 0; i < 6; i++)
            {
                spelc.Add(new Spellc
                {
                    OffsetProperty = "OffsetProperty                    :  " + dataObject.flaggedtokenPropertys[i].OffsetProperty,
                    tokenProperty = "Wrong Word                 :  " + dataObject.flaggedtokenPropertys[i].tokenProperty,
                    SuggestionProperty = "Spelling SuggestionProperty   :  " + dataObject.flaggedtokenPropertys[i].SuggestionPropertys[0].SuggestionProperty
                });

            }
        }
    }
}
