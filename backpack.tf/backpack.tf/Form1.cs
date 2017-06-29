using System;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace backpack.tf
{
    public partial class Form1 : Form
    {
        const string apikeyBPTF = "59517036cf6c757183369af0";
        const string apikeySTEAM = "E075A8709C7B44DA772FD76237021228";

        public Form1()
        {
            InitializeComponent();
        }

        private void tim_vreme_Tick(object sender, EventArgs e)
        {
            status_time.Text = Convert.ToString(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
            status_date.Text = Convert.ToString(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
        }

        private void btn_update_key_steam_Click(object sender, EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString("http://steamcommunity.com/market/priceoverview/?currency=1&appid=440&market_hash_name=Mann%20Co.%20Supply%20Crate%20Key");
                dynamic array = JsonConvert.DeserializeObject(json);

                var low = array.lowest_price.ToString();
                var avg = array.median_price.ToString();
                var cnt = array.volume.ToString();

                txb_jsonprev.Text = formatJson(json);
                txb_key_low.Text = low;
                txb_key_avg.Text = avg;
                txb_key_cnt.Text = cnt;
            }
        }

        private void btn_update_buds_steam_Click(object sender, EventArgs e)
        {
            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString("http://steamcommunity.com/market/priceoverview/?currency=1&appid=440&market_hash_name=Earbuds");
                dynamic array = JsonConvert.DeserializeObject(json);

                var low = array.lowest_price.ToString();
                var avg = array.median_price.ToString();
                var cnt = array.volume.ToString();

                txb_jsonprev.Text = formatJson(json);
                txb_buds_low.Text = low;
                txb_buds_avg.Text = avg;
                txb_buds_cnt.Text = cnt;
            }
        }

        private void btn_getbackpacktf_Click(object sender, EventArgs e)
        {
            updateBackpackTFPrices();
            tim_2ipo.Start();
            btn_getbackpacktf.Enabled = false;
        }

        private string formatJson(string json)
        {
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            var f = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);
            return f;
        }

        private void updateBackpackTFPrices()
        {
            string json = "";
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36");
                json = webClient.DownloadString("https://backpack.tf/api/IGetCurrencies/v1?key=" + apikeyBPTF);

                dynamic array = JsonConvert.DeserializeObject(json);

                string valuer = array.response.currencies.metal.price.value.ToString();
                string currencyr = array.response.currencies.metal.price.currency.ToString();
                txb_ref_price.Text = valuer;
                txb_ref_currency.Text = currencyr;

                string valuek = array.response.currencies.keys.price.value.ToString();
                string currencyk = array.response.currencies.keys.price.currency.ToString();
                txb_key_price.Text = valuek;
                txb_key_currency.Text = currencyk;

                string valueb = array.response.currencies.earbuds.price.value.ToString();
                string currencyb = array.response.currencies.earbuds.price.currency.ToString();
                txb_buds_price.Text = valueb;
                txb_buds_currency.Text = currencyb;
            }
            txb_jsonprev.Text = formatJson(json);
        }

        private void tim_2ipo_Tick(object sender, EventArgs e)
        {
            if (tim_2ipo.Interval >= 150000)
            {
                btn_getbackpacktf.Enabled = true;
                tim_2ipo.Stop();
            }
        }

        private async void btn_getinv_Click(object sender, EventArgs e)
        {
            ulong kresenkoid = 76561198044606823;

            var steamInvData = new SteamInventoryData(apikeySTEAM);
            var items = await steamInvData.GetItems(kresenkoid);

            foreach (var item in items)
            {
                //Do something with the items
                var itemName = await steamInvData.GetItemName(item.DefIndex);
                
                txb_invprev.Text += "You have: " + itemName.ToString() + Environment.NewLine;
                //MessageBox.Show("You have: !"+itemName.ToString());
                MessageBox.Show($"You have a(n) {itemName}!");
            }

            //string id = txb_steamid.Text;
            //string url = "http://steamcommunity.com/id/" + id + "/inventory/json/440/2";
            //string imageUrl = "http://cdn.steamcommunity.com/economy/image/";

            //using (var webClient = new WebClient())
            //{
            //    var json = webClient.DownloadString(url);
            //
            //    dynamic array = JsonConvert.DeserializeObject(json);
            //    var inve = array.rgInventory.ToString();
            //    var desc = array.rgDescriptions.ToString();
            //
            //    //var userList = JsonConvert.DeserializeObject<Dictionary<string, TF2Inv>>(array);
            //
            //    txb_invprev2.Text = formatJson(inve);
            //    txb_invprev.Text = formatJson(desc);
            //
            //    //pcb_rocket.Load(imageUrl + img);
            //
            //}

            ////pcb_rocket.Load(imageUrl + "fWFc82js0fmoRAP-qOIPu5THSWqfSmTELLqcUywGkijVjZULUrsm1j-9xgEMaQkUTxr2vTx8mMnvA-aHAfQ_ktk664MayTl8lBNzO_amNQh1fQfJBLRSXeA09wDpGxgz-dJiWdak_rUDFku65cDEXOZ5c5wVWZXOC6KAYlj97Ew_06BYKceP8im93ynpOzxfXxG6_W4BnLKOu7Q5gj5FQG_w87tcprFVJw");
        }
    }
    /*
    public class TF2Inv
    {
        public bool success { get; set; }
        public TF2ItemInv rgInventory { get; set; }
        public TF2ItemDesc rgDescriptions { get; set; }
    }

    public class TF2ItemInv
    {
        public int id { get; set; }
        public long classid { get; set; }
        public long instanceid { get; set; }
        public int amount { get; set; }
        public int pos { get; set; }
    }

    public class TF2ItemDesc
    {
        public int classid { get; set; }
        public int instanceid { get; set; }
        public string icon_url { get; set; }
        public string market_name { get; set; }
        public string name_color { get; set; }
        public string background_color { get; set; }
    }*/
}