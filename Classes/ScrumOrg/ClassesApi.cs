using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Classes.ScrumOrg
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Trainer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("profileUrl")]
        public string ProfileUrl { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
    public class PtnOrganization
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }
        [JsonProperty("deliveryMethod")]
        public string DeliveryMethod { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("dateString")]
        public string DateString { get; set; }
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        [JsonProperty("registrationEnabled")]
        public bool RegistrationEnabled { get; set; }
        [JsonProperty("registrationUrl")]
        public string RegistrationUrl { get; set; }
        [JsonProperty("languages")]
        public List<string> Languages { get; set; }
        [JsonProperty("trainers")]
        public List<Trainer> Trainers { get; set; }
        [JsonProperty("ptnOrganization")]
        public PtnOrganization PtnOrganization { get; set; }
        [JsonProperty("startEndTimes")]
        public string StartEndTimes { get; set; }
    }
    public class Root
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
    }
}
