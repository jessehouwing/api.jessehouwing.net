using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Flurl.Util;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Classes.ScrumOrg;

namespace Classes
{
    public class Classes
    {
        private int[] allowedTrainers = new[]
        {
            76, // Jesse
            358713, // Manuel
            141, // Evelien,
            217755, // Laurens
            206469, // Chris
            210288, // Willem
            198095, // Just,
            303, // Robbin
            175848, // Wilbert
        };

        private int[] allowedClasses = new[]
        {
            103, //PSF
            104, //PSM
            164, //PSM2
            105, //PSPO
            106, //SPS
            130, //PALE
            107, //PSD-NET
            109, //PSD-JV
            108, //PSD-T
            133, //PSK
            200, //PSU,
            210, //PSPO-A
        };

        [FunctionName("QueryClasses")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            int trainerId = int.Parse(req.Query["trainer"]);
            if (!allowedTrainers.Contains(trainerId))
            {
                throw new ArgumentOutOfRangeException("trainer");
            }

            int[] courseIds = req.Query["course"].Select(s => int.Parse(s)).Intersect(allowedClasses).ToArray();

            var courses = await GetCourses(trainerId, courseIds);

            var formatterSettings = new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            string script = string.Empty;
            if (courses.Any())
            {
                var json = JsonConvert.SerializeObject(courses, Formatting.None, formatterSettings);
                script = $"var data = {json};";
                script += @"
                $(document).ready(function() {
                    var table = '<table>';
                    table += '<thead><tr><th>Start</th><th>End</th><th>Course</th><th>Language</th><th>Location</th><th></th></tr></thead>'
                    table += '<tbody>'
                    for (var key in data)
                    {
                        table += '<tr>';
                        table += '<td>' + new Date(data[key].StartDate).toLocaleDateString() + '</td>';
                        table += '<td>' + new Date(data[key].EndDate).toLocaleDateString() + '</td>';
                        table += '<td>' + data[key].Name + '</td>';
                        table += '<td>' + data[key].Language + '</td>';
                        table += '<td>' + data[key].Location + '</td>';
                        table += '<td><a href=""' + data[key].RegisterUri + '"">Register...</a></td>';
                        table += '</tr>';
                    }
                    table += '</tbody>';
                    table += '</table>';
                    $('#classes').append(table);
                });";
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(script, Encoding.UTF8, "application/javascript")
            };
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = new TimeSpan(1,0,0,0)
            };
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }


        private async Task<IEnumerable<Course>> GetCourses(int trainer, int[] courseIdentifiers)
        {
            var classesUri = "https://www.scrum.org/api/class-search";

            using (HttpClient client = new HttpClient())
            {
                var result = await client.PostAsync(classesUri, JsonContent.Create(new
                {
                    courseTypes = courseIdentifiers,
                    trainerId = trainer,
                    countryCode = string.Empty
                }));

                var courses = JsonConvert.DeserializeObject<Root>(await result.Content.ReadAsStringAsync());

                return courses.Items.Select(course => new Course
                {
                    Name = course.Title,
                    RegisterUri = new Uri(course.RegistrationUrl, UriKind.Absolute),
                    Language = course.Languages.First(),
                    Location = course.Location,
                    Trainers = course.Trainers.Select(t => { return new Trainer() { Name = t.Name, Uri = "https://www.scrum.org/".AppendPathSegment(t.ProfileUrl).ToUri() }; }).ToArray(),
                    StartDate = DateTime.Parse(course.StartDate, CultureInfo.InvariantCulture),
                    EndDate = DateTime.Parse(course.EndDate, CultureInfo.InvariantCulture)
                });
            }
        }

        public struct Course
        {
            public Uri RegisterUri { get; set; }
            public string Name { get; set; }
            public Trainer[] Trainers { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Language { get; set; }
            public string Location { get; set; }
        }

        public struct Trainer
        {
            public string Name { get; set; }
            public Uri Uri { get; set; }
        }
    }
}
