using System;
using System.Collections.Generic;
using System.Text;

namespace Classes.ScrumOrg
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Trainer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string profileUrl { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
    }

    public class PtnOrganization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string logoUrl { get; set; }
        public string deliveryMethod { get; set; }
        public string location { get; set; }
        public string countryCode { get; set; }
        public string dateString { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public bool registrationEnabled { get; set; }
        public string registrationUrl { get; set; }
        public List<string> languages { get; set; }
        public List<Trainer> trainers { get; set; }
        public PtnOrganization ptnOrganization { get; set; }
        public string startEndTimes { get; set; }
    }

    public class Root
    {
        public List<Item> items { get; set; }
        public int totalItems { get; set; }
        public int page { get; set; }
    }


}
