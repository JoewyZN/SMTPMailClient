using MailKit.Net.Smtp;
using MimeKit;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMTPMailClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var summary = "";

            var processedSummary = new List<string>();
            processedSummary.Add("<br/>");
            for (int i = 0; i < 1; i++)
            {
                processedSummary.Add("<br/>");
                processedSummary.Add(string.Format("<strong>{0} [13/15]</strong>", "21ABBAK-201806040252-DCL-01ZNCBK"));
                processedSummary.Add("<hr/>");
                processedSummary.Add(string.Format("* Failed - {0} [{1}] {2}", "Intergrity Technologies", "6,849.34", "Invalid Account Number"));
                processedSummary.Add("<br/>");
                processedSummary.Add(string.Format("* Failed - {0} [{1}] {2}", "MJ and Sons Mechanics", "764.03", "Invalid Account Number"));
                processedSummary.Add("<hr/>");
            }

            foreach (var item in processedSummary)
            {
                summary += item;
            }
            processedSummary.Add("<br/>");

            var message = string.Format("CORE BANKING POST SUMMARY {0}.{1}{2}{3}", DateTime.Now, Environment.NewLine, Environment.NewLine, summary);

            var client = new RestClient("http://localhost:61771/API");
            var request = new RestRequest("/Notifications/SendMail", Method.POST);
            request.AddJsonBody(new
            {
                APIKEY = "4EC42E9C-DD26-4E77-8868-9B9073B2E726",
                RecipientName = "Joewy Lombe",
                RecipientEmail = "joewylombe@gmail.com",
                Message = message,
                Subject = string.Format("CBS Post Summary {0}", now)
            });
            var response = client.Execute(request);
            var content = response.Content;
            var obj = JsonConvert.DeserializeObject<SendMailResponseObj>(content);

            MessageBox.Show(obj.Message);
        }
    }

    public class SendMailResponseObj
    {
        public string Source { get; set; }
        public string Context { get; set; }
        public string RequestStamp { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Errors { get; set; }
        public int Count { get; set; }
        public object Object { get; set; }
    }
}
