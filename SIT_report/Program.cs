using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using System.Data;
using System.IO;
//using SIT_report.Helpers;

namespace SIT_report
{
    internal class Program
    {
        public static Dictionary<string, string> terms;
        public static string echo360RootURL = "https://echo360.org.au";
        public static BearerTokenRoot currentToken = null;
        public static bool tokenExpired = false;


        static void Main(string[] args)
        {
            //string apiUrl = "https://api.echo360.org/v1/courses";
            args = System.Environment.GetCommandLineArgs();
            if (args.Length == 2)
            {
                string batch = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
                switch (args[1])
                {
                    case "-poll":
                        Console.Write("poll");
                        break;
                    case "-monitor":
                        Console.Write("monitor");
                        break;
                    case "-log":
                        Console.Write("log");
                        break;
                    case "-report":
                        Console.Write("report");
                        break;
                    case "-testget":
                        BuildReportV2(DateTime.Parse("2024-04-01"), DateTime.Parse("2024-04-02"));
                        break;
                }
            }
        }
        public static void GetNewAccessToken()
        {
            var client = new RestClient("https://echo360.org.au/");

            var requestToken = new RestRequest("https://echo360.org.au/oauth2/access_token", Method.POST);
            requestToken.AddParameter("grant_type", "client_credentials"); // adds to POST or URL querystring based on Method
            requestToken.AddParameter("client_id", "7e92bbbf-5a07-4876-b37f-226e8218e347"); // adds to POST or URL querystring based on Method
            requestToken.AddParameter("client_secret", "f9831066-1008-4147-84e2-6fbdd862d33e"); // adds to POST or URL querystring based on Method

            // execute the request
            IRestResponse response = client.Execute(requestToken);
            var content = response.Content; // raw content as string

            currentToken = Newtonsoft.Json.JsonConvert.DeserializeObject<BearerTokenRoot>(content.ToString());
        }
        public static string GetAccessToken()
        {
            
            if (currentToken != null)
            {
                if (!tokenExpired)
                {
                    return currentToken.access_token;
                }
                else
                {
                    return currentToken.refresh_token;
                }
            }
            throw new Exception("AL: Token not Initialized");
        }
        public static void AccessTokenExpired()
        {
            tokenExpired = true;
        }
        public static Dictionary<string, TermItem> GetTerms()
        {
            
            string sectionEndPoint = "/public/api/v1/terms";

            Dictionary<string, TermItem> allTerms = new Dictionary<string, TermItem>();

            bool end = false;

            while (!end)
            {
                // Form the first URL to call the endpoint
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);

                // TODO: to improve on handing access token. E.g. use refresh token and change the class to a singleton
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());

                // execute the request
                var client = new RestClient(echo360RootURL);

                IRestResponse response = client.Execute(request);
                
                // Parse the raw content using JSON.NET to return C# objects
                TermsRoot termsRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<TermsRoot>(response.Content);

                foreach (TermItem x in termsRoot.data)
                {
                    allTerms.Add(x.id, x);
                }


                // check if there are more data
                if (termsRoot.has_more == false)
                {
                    // no more
                    end = true;
                }
                else
                {
                    sectionEndPoint = termsRoot.next;
                    end = false;
                }
            }
            
            return allTerms;
        }
        public static Dictionary<string, CourseItems> GetCourses()
        {
            string sectionEndPoint = "/public/api/v1/courses";

            Dictionary<string, CourseItems> allCourses = new Dictionary<string, CourseItems>();

            bool end = false;

            while (!end)
            {
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);

                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);
                CourseRoot courseRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<CourseRoot>(response.Content);

                foreach (CourseItems x in courseRoot.data)
                {
                    allCourses.Add(x.id, x);
                }
                if (courseRoot.has_more == false)
                {
                    end = true;
                }
                else
                {
                    sectionEndPoint = courseRoot.next;
                    end = false;
                }
            }

            return allCourses;
        }
        public static Dictionary<string, OrganizationItem> GetOrganizations()
        {
            string sectionEndPoint = "/public/api/v1/organizations";

            Dictionary<string, OrganizationItem> allOrganizations = new Dictionary<string, OrganizationItem>();

            bool end = false;

            while (!end)
            {
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);

                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);
                OrganizationRoot organizationRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<OrganizationRoot>(response.Content);

                foreach (OrganizationItem x in organizationRoot.data)
                {
                    allOrganizations.Add(x.id, x);
                }
                if (organizationRoot.has_more == false)
                {
                    end = true;
                }
                else
                {
                    sectionEndPoint = organizationRoot.next;
                    end = false;
                }
            }

            return allOrganizations;
        }
        public static Dictionary<string, SectionItem> GetSections()
        {
            string sectionEndPoint = "/public/api/v1/sections";

            Dictionary<string, SectionItem> allSections = new Dictionary<string, SectionItem>();

            bool end = false;

            while (!end)
            {
                // Form the first URL to call the endpoint
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);

                // TODO: to improve on handing access token. E.g. use refresh token and change the class to a singleton
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());

                // execute the request
                var client = new RestClient(echo360RootURL);

                IRestResponse response = client.Execute(request);

                // Parse the raw content using JSON.NET to return C# objects
                SectionsRoot sectionsRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionsRoot>(response.Content);

                // Copy the Sections into the List<SectionItem> 
                foreach (SectionItem x in sectionsRoot.data)
                {
                    allSections.Add(x.id, x);
                }
                // check if there are more data
                if (sectionsRoot.has_more == false)
                {
                    // no more sections
                    end = true;
                }
                else
                {
                    sectionEndPoint = sectionsRoot.next;
                    end = false;
                }
            }

            return allSections;
        }
        public static SectionItem GetSections(string sectionId)
        {
            string sectionEndPoint = "GET /public/api/v1/sections/" + sectionId;

            Dictionary<string, SectionItem> allSections = new Dictionary<string, SectionItem>();

            bool end = false;
            SectionItem section = new SectionItem();
            while (!end)
            {
                // Form the first URL to call the endpoint
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);

                // TODO: to improve on handing access token. E.g. use refresh token and change the class to a singleton
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());

                // execute the request
                var client = new RestClient(echo360RootURL);

                IRestResponse response = client.Execute(request);

                // Parse the raw content using JSON.NET to return C# objects
                section = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionItem>(response.Content);

            }

            return section;
        }
        public static Dictionary<string, ScheduleItem> GetSchedules()
        {
            string sectionEndPoint = "/public/api/v1/schedules";

            Dictionary<string, ScheduleItem> allSchedules = new Dictionary<string, ScheduleItem>();

            bool end = false;

            while (!end)
            {
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);
                SchedulesRoot schedulesRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SchedulesRoot>(response.Content);


                foreach (ScheduleItem x in schedulesRoot.data)
                {
                    allSchedules.Add(x.id, x);
                }
                // Copy the Sections into the List<SectionItem>

                // check if there are more data
                if (schedulesRoot.has_more == false)
                {
                    // no more sections
                    end = true;
                }
                else
                {
                    sectionEndPoint = schedulesRoot.next;
                    end = false;
                }
            }

            return allSchedules;
        }

        public static Dictionary<string, ScheduleItem> GetSchedules(DateTime sdate, DateTime edate)
        {
            string sectionEndPoint = "/public/api/v1/schedules";

            Dictionary<string, ScheduleItem> allSchedules = new Dictionary<string, ScheduleItem>();

            bool end = false;

            SchedulesRoot schedulesRoot = null;
            int count = 0;
            while (!end)
            {
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);
                try
                {
                    schedulesRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SchedulesRoot>(response.Content);
                    count += schedulesRoot.data.Count;
                }
                catch (Exception)
                {
                    Console.WriteLine(response.Content);
                }
                foreach (ScheduleItem x in schedulesRoot.data)
                {
                    if (x.inRange(sdate, edate))
                    {
                        //Console.Write("IN ");

                        if (!allSchedules.ContainsKey(x.id))
                        {
                            allSchedules.Add(x.id, x);
                        }
                    }
                    //else
                    //{
                    //    Console.Write("OUT ");
                    //}
                    //Console.Write(x.startDate.ToString() + " ");
                    if (String.IsNullOrEmpty(x.endDate))
                    {
                        x.endDate = "";
                    }
                    //Console.Write(x.endDate.ToString());
                    //Console.WriteLine(x.name);

                }
                if (schedulesRoot.has_more == false)
                {
                    end = true;
                }
                else
                {
                    sectionEndPoint = schedulesRoot.next;
                    end = false;
                }
            }
            //Console.WriteLine("V1 total: " + count);
            return allSchedules;
        }
        public static Dictionary<string, ScheduleV2Item> GetV2Schedules(DateTime sdate, DateTime edate)
        {
            Console.WriteLine(sdate);
            Console.WriteLine(edate);
            string sectionEndPoint = "/public/api/v2/schedules";

            Dictionary<string, ScheduleV2Item> allSchedules = new Dictionary<string, ScheduleV2Item>();

            bool end = false;
            int count = 0;
            ScheduleV2Root schedulesRoot = null;
            while (!end)
            {
                var request = new RestRequest(echo360RootURL + sectionEndPoint, Method.GET);
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);
                try
                {
                    schedulesRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<ScheduleV2Root>(response.Content);
                    count += schedulesRoot.data.Count;
                }
                catch (Exception)
                {
                    Console.WriteLine(response.Content);
                }
                foreach (ScheduleV2Item x in schedulesRoot.data)
                {
                    
                    if (x.inRange(sdate, edate))
                    {

                        if (!allSchedules.ContainsKey(x.id))
                        {
                            //try
                            //{
                            //    Console.WriteLine("IN, add, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd") + ", " + DateTime.Parse(x.endDate).ToString("yyyy-MM-dd"));

                            //}
                            //catch
                            //{
                            //    Console.WriteLine("IN, add, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd"));

                            //}
                            allSchedules.Add(x.id, x);
                        }
                        else
                        {
                            //try
                            //{
                            //    Console.WriteLine("IN, rep, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd") + ", " + DateTime.Parse(x.endDate).ToString("yyyy-MM-dd"));
                            //}
                            //catch
                            //{
                            //    Console.WriteLine("IN, rep, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd") );

                            //}
                        }
                    }
                    else
                    {
                        //try
                        //{
                        //    Console.WriteLine("OUT, exc, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd") + ", " + DateTime.Parse(x.endDate).ToString("yyyy-MM-dd"));
                        //}
                        //catch
                        //{

                        //    Console.WriteLine("OUT, exc, " + x.id + ", " + DateTime.Parse(x.startDate).ToString("yyyy-MM-dd"));
                        //}

                    }

                }
                if (schedulesRoot.has_more == false)
                {
                    end = true;
                }
                else
                {
                    sectionEndPoint = schedulesRoot.next;
                    end = false;
                }
            }
            return allSchedules;
        }

        public static Dictionary<string, MediaAvailability> GetLessonMediaAvailability
            (Dictionary<string, LessonItem> lessons)
        {
            Dictionary<string, MediaAvailability> lessonmedia = new Dictionary<string, MediaAvailability>();

            foreach (LessonItem x in lessons.Values)
            {
                string lessonEndPoint = "/public/api/v1/medias?lessonId=" + x.id;
                var request = new RestRequest(echo360RootURL + lessonEndPoint, Method.GET);
                request.AddHeader("Authorization", "Bearer " + GetAccessToken());
                var client = new RestClient(echo360RootURL);
                IRestResponse response = client.Execute(request);

                if (!response.Content.Contains("Media not found for: Lesson"))
                {
                    MediaRoot mediaRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<MediaRoot>(response.Content);
                    List<MediaItem> tmpmedias = mediaRoot.data;
                    lessonmedia.Add(x.id, GeMediaAvailability(tmpmedias, x.id));
                }

            }

            return lessonmedia;
        }


        public static Dictionary<string, LessonItem> GetLessons
            (Dictionary<string, ScheduleV2SectionItem> sections, DateTime sdate, DateTime edate)
        {
            Dictionary<string, LessonItem> lessons = new Dictionary<string, LessonItem>();

            foreach (ScheduleV2SectionItem y in sections.Values)
            {
                GetNewAccessToken();
                try
                {
                    string lessonEndPoint = "/public/api/v1/sections/" + y.sectionId + "/lessons";
                    var request = new RestRequest(echo360RootURL + lessonEndPoint, Method.GET);
                    request.AddHeader("Authorization", "Bearer " +  GetAccessToken());
                    var client = new RestClient(echo360RootURL);
                    IRestResponse response = client.Execute(request);
                    List<LessonItem> tmplessons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LessonItem>>(response.Content);
                    foreach (LessonItem z in tmplessons)
                    {
                        if (z.timing != null)
                        {
                            if (z.inRange(sdate, edate) && !lessons.ContainsKey(z.id))
                            {
                                //Console.WriteLine("IN," +  z.timing.start.ToString("dd-MM-yyyy") + "," +  z.name +"," + y.sectionId);
                                lessons.Add(z.id, z);
                            }
                            //else
                            //{
                            //   Console.WriteLine("OUT," + z.timing.start.ToString("dd-MM-yyyy") + "," + z.name + ","+ y.sectionId);
                            //}
                        }
                    }

                }
                catch (Exception)
                {

                }
            }


            return lessons;
        }

        public static List<LessonItem> GetLessonBySectionID(string SectionID)
        {
            string lessonEndPoint = "/public/api/v1/sections/" + SectionID + "/lessons";

            //List<LessonRoot> allLessons = new List<LessonRoot>();

            // Form the first URL to call the endpoint
            var request = new RestRequest(echo360RootURL + lessonEndPoint, Method.GET);

            // TODO: to improve on handing access token. E.g. use refresh token and change the class to a singleton
            request.AddHeader("Authorization", "Bearer " + GetAccessToken());

            // execute the request
            var client = new RestClient(echo360RootURL);

            IRestResponse response = client.Execute(request);

            // Parse the raw content using JSON.NET to return C# objects
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<LessonItem>>(response.Content);
        }

        public static MediaAvailability GeMediaAvailability(List<MediaItem> medias, string lessonsid)
        {
            MediaAvailability ma = new MediaAvailability();
            if (medias != null)
            {
                foreach (MediaItem i in medias)
                {
                    if (i.mediaType == "Video")
                    {
                        foreach (MediaLessonItem x in i.lessons)
                        {
                            if (x.lessonId == lessonsid)
                            {
                                switch (x.contentAvailability.type)
                                {
                                    case "Immediate":
                                        ma.videoAvailability = "Y";
                                        break;

                                    case "Unavailable":
                                        ma.videoAvailability = "N";
                                        break;

                                    case "Concrete":
                                        if (DateTime.Today >= DateTime.Parse(x.contentAvailability.body.date))
                                        {
                                            ma.videoAvailability = "Y";
                                            ma.videoAvailabilityDate = x.contentAvailability.body.date;
                                        }
                                        else
                                        {
                                            ma.videoAvailability = "N";
                                            ma.videoAvailabilityDate = x.contentAvailability.body.date;
                                        }
                                        break;

                                }

                            }
                        }
                    }

                    if (i.mediaType == "Presentation")
                    {
                        foreach (MediaLessonItem x in i.lessons)
                        {
                            if (x.lessonId == lessonsid)
                            {
                                switch (x.contentAvailability.type)
                                {
                                    case "Immediate":
                                        ma.presentationAvailability = "Y";
                                        break;

                                    case "Unavailable":
                                        ma.presentationAvailability = "N";
                                        break;

                                    case "Concrete":
                                        if (DateTime.Today >= DateTime.Parse(x.contentAvailability.body.date))
                                        {
                                            // Recording is available
                                            ma.presentationAvailability = "Y";
                                            ma.presentationAvailabilityDate = x.contentAvailability.body.date;
                                        }
                                        else
                                        {
                                            // Recording is not available yet
                                            ma.presentationAvailability = "N";
                                            ma.presentationAvailabilityDate = x.contentAvailability.body.date;
                                        }
                                        break;
                                }

                            }
                        }
                    }
                }
            }

            return ma;
        }
        public static string ReturnEmptyOnNull(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            else
            {
                return s;
            }
        }
        public static string BuildReportV2(DateTime startdate, DateTime enddate)
        {
            GetNewAccessToken();
            string startDateString = startdate.ToString("yyyy-MM");
            string fileName = $"{startDateString}-AvailabilityReport.csv";
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(exeDirectory, fileName);

            DataTable polls = new DataTable();
            //MySqlDataAdapter adp = new MySqlDataAdapter("polls_bydate", ConfigurationManager.AppSettings["connectionstring"]);
            //adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            //adp.SelectCommand.Parameters.AddWithValue("@fromdate", startdate);
            //adp.SelectCommand.Parameters["@fromdate"].Direction = ParameterDirection.Input;
            //adp.SelectCommand.Parameters.AddWithValue("@todate", enddate);
            //adp.SelectCommand.Parameters["@todate"].Direction = ParameterDirection.Input;
            //adp.Fill(polls);
            Dictionary<string, List<PollStatus>> locationpolls = new Dictionary<string, List<PollStatus>>();
            string currlocation = "";
            PollStatus currstatus = new PollStatus();
            List<PollStatus> pollstatus = new List<PollStatus>();
            locationpolls.Add(currlocation, pollstatus);
            Dictionary<string, CourseItems> courselist = GetCourses();
            GetNewAccessToken();
            Dictionary<string, OrganizationItem> organizationlist = GetOrganizations();
            GetNewAccessToken();
            Dictionary<string, TermItem> termlist = GetTerms();
            GetNewAccessToken();
            Dictionary<string, ScheduleV2Item> schedulelist = GetV2Schedules(startdate, enddate);
            Dictionary<string, ScheduleV2SectionItem> sectionlist = new Dictionary<string, ScheduleV2SectionItem>();
            Dictionary<string, ScheduleV2Item> sectionschedulelist = new Dictionary<string, ScheduleV2Item>();
            foreach (ScheduleV2Item x in schedulelist.Values)
            {
                foreach (ScheduleV2SectionItem y in x.sections)
                {
                    if (!sectionlist.ContainsKey(y.sectionId))
                    {
                        sectionlist.Add(y.sectionId, y);
                    }
                    if (!sectionschedulelist.ContainsKey(y.sectionId))
                    {
                        sectionschedulelist.Add(y.sectionId, x);
                    }
                }

            }
            GetNewAccessToken();
            Dictionary<string, LessonItem> lessonlist = GetLessons(sectionlist, startdate, enddate);
            GetNewAccessToken();
            Dictionary<string, MediaAvailability> mediaavailabilitylist = GetLessonMediaAvailability(lessonlist);
            Dictionary<string, List<LessonItem>> sectionlessons = new Dictionary<string, List<LessonItem>>();
            List<string> csv = new List<string>();
            StringBuilder sb = new StringBuilder();
            currstatus = new PollStatus();
            string errortype = "";
            //csv.Add("\"Course name\",\"Term  \",\"Course ID\",\"Course Description\",\"Section ID\",\"Section Description\",\"Lesson ID\",\"Lesson Name\",\"Owner Name\",\"Owner's Email\",\"Room\",\"Schedule Start\",\"Schedule End\",\"Lesson Created On\",\"Lesson Date\",\"Lesson Start\",\"Lesson End\",\"Video Available \",\"Video Availabile On\",\"Presentation Available\",\"Presentation Availabile On \",\"Vision Errors \",\"Error Type\"");
            csv.Add("\"Term  \",\"Course ID\",\"Course Description\",\"Section ID\",\"Section Description\",\"Lesson Name\",\"Lesson Created On\",\"Lesson Start\",\"Lesson End\",\"Video Available \",\"Video Availabile On\",\"Presentation Available\",\"Presentation Availabile On \"");
            //csv.Add(", pollcount,pollrec,offline,nostatus,nosync,syncerror,missedrec,pollpause,pollids");
            csv.Add("\n");
            foreach (LessonItem x in lessonlist.Values)
            {
                Console.WriteLine($"Processing Lesson: {x.name}, ID: {x.id}");
                sb = new StringBuilder("");
                string[] idlist = x.id.Split('_');
                errortype = "";
                currstatus = new PollStatus();
                ScheduleV2SectionItem currsection = sectionlist[x.sectionId];
                ScheduleV2Item currschedule = sectionschedulelist[currsection.sectionId];

                /*
                int pollcount = 0;
                int pollpause = 0;
                int pollrec = 0;
                int offline = 0;
                int nostatus = 0;
                int nosync = 0;
                int missedrec = 0;
                int syncerror = 0;
                string pollids = "";
                foreach (DataRow vp in polls.Rows)
                {
                    DateTime pstart = DateTime.Parse(vp["poll_start"].ToString());
                    string plocation = vp["poll_location"].ToString();
                    if (plocation.IndexOf(currschedule.venue.roomName) > 0 && pstart >= x.timing.start && pstart <= x.timing.end)
                    {
                        pollids += vp["poll_id"].ToString() + " ";
                        pollcount += 1;
                        if (vp["poll_capturestate"].ToString().Equals("active"))
                        {
                            pollrec += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S1-PODOFFLINE") >= 0)
                        {
                            offline += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S1-NOSTATUS") >= 0)
                        {
                            nostatus += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S1-PODNOSYNC") >= 0)
                        {
                            nosync += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S3-PODSYNCERR") >= 0)
                        {
                            syncerror += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S2-MISSEDREC") >= 0)
                        {
                            missedrec += 1;
                        }
                        if (vp["poll_errmsg"].ToString().IndexOf("S3-CAPPAUSED") >= 0)
                        {
                            pollpause += 1;
                        }

                        currstatus.SetStatus(vp["poll_errmsg"].ToString());


                    }
                }
                //errortype = pollcount.ToString() + "|" + pollpause.ToString() + "|" + pollrec + "|";

                if (pollcount == 16)
                {
                    pollcount = 16;
                }
                if (pollcount >= (pollrec * 4))
                {
                    if (pollcount < (offline * 4))
                    {
                        errortype = "System Error";
                    }
                    if (pollcount < (nostatus * 4))
                    {
                        errortype = "System Error";
                    }
                    if (pollcount < (syncerror * 4))
                    {
                        errortype = "System Error";
                    }
                    if (pollcount < (pollpause * 4))
                    {
                        if (String.IsNullOrEmpty(errortype))
                        {
                            errortype = "Human Error";
                        }
                        else
                        {
                            errortype = "System and Human Error";
                        }
                    }
                    if (String.IsNullOrEmpty(errortype))
                    {
                        errortype = "Human Error";
                    }
                    //if (pollcount < (missedrec * 4))
                    //{
                    //    if (errortype.Equals("System Error"))
                    //    {
                    //        errortype = "System Error";
                    //    }
                    //    else
                    //    {
                    //        errortype = "Human Error";
                    //    }
                    //}


                }

                errortype += "\"," + pollcount + "," + pollrec + "," + offline + "," + nostatus + "," + nosync + "," + syncerror + "," + missedrec + "," + pollpause + ",\"" + pollids;
                */
                //Course name
                //sb.Append("\"");
                //try
                //{
                    //sb.Append(organizationlist[courselist[currsection.courseId].organizationId].name.Replace(',', ' '));
                //}
                //catch
                //{
                    //sb.Append("-");
                //}
                //sb.Append("\",\"");
                //Term 
                sb.Append(termlist[currsection.termId].name.Replace(',', ' '));
                sb.Append("\",\"");
                //Course ID 
                sb.Append(currsection.courseId);
                sb.Append("\",\"");
                //Course Description 
                sb.Append(currsection.courseIdentifier.Replace(',', ' '));
                sb.Append("\",\"");
                //Section ID 
                sb.Append(currsection.sectionId);
                sb.Append("\",\"");
                //Section Description 
                sb.Append(currsection.sectionName.Replace(',', ' '));
                sb.Append("\",\"");
                //Lesson ID 
                //sb.Append(x.id);
                //sb.Append("\",\"");
                //Lesson Name 
                sb.Append(x.name.Replace(',', ' '));
                sb.Append("\",\"");
                //Owner Name 
                //try
                //{
                    //sb.Append(sectionschedulelist[currsection.sectionId].presenter.userFullName.Replace(',', ' '));
                //}
                //catch
                //{
                    //sb.Append("-");
                //}
                //sb.Append("\",\"");
                //Owner's Email	
                //try
                //{
                    //sb.Append(sectionschedulelist[currsection.sectionId].presenter.userEmail);
                //}
                //catch
                //{
                    //sb.Append("-");
                //}
                //sb.Append("\",\"");
                //Room	
                //sb.Append(sectionschedulelist[currsection.sectionId].venue.roomName);
                //sb.Append("\",\"");
                //Schedule Start	
                //try
                //{
                    //sb.Append(DateTime.Parse(schedulelist[idlist[1]].startDate).ToString("yyyy-MM-dd"));
                //}
                //catch
                //{
                    //sb.Append("-");
                //}
                //sb.Append("\",\"");
                //Schedule End	
                //try
                //{
                    //sb.Append(DateTime.Parse(schedulelist[idlist[1]].endDate).ToString("yyyy-MM-dd"));
                //}
                //catch
                //{
                    //sb.Append("-");
                //}
                //sb.Append("\",\"");
                //Lesson Created On	
                sb.Append(x.createdAt.ToString("yyyy-MM-dd"));
                sb.Append("\",\"");
                //Lesson Date	
                //sb.Append(x.timing.start.ToString("dd-MM-yyyy"));
                //sb.Append("\",\"");
                //Lesson Start	
                sb.Append(x.timing.start.ToString("hh:mm"));
                sb.Append("\",\"");
                //Lesson End	
                sb.Append(x.timing.end.ToString("hh:mm"));
                sb.Append("\",\"");
                //Video Available	 
                sb.Append(mediaavailabilitylist[x.id].videoAvailability);
                sb.Append("\",\"");
                //Video Availabile On	
                sb.Append(mediaavailabilitylist[x.id].videoAvailabilityDate);
                sb.Append("\",\"");
                //Presentation Available	
                sb.Append(String.IsNullOrEmpty(mediaavailabilitylist[x.id].presentationAvailability) ? mediaavailabilitylist[x.id].presentationAvailability : "-");
                sb.Append("\",\"");
                //Presentation Availabile On
                try
                {
                    sb.Append(DateTime.Parse(mediaavailabilitylist[x.id].presentationAvailabilityDate).ToString("yyyy-MM-dd hh:mm"));
                }
                catch
                {
                    sb.Append("-");
                }
                //sb.Append("\",\"");
                //Vision Errors
                //sb.Append(currstatus.ToCSV());
                //sb.Append("\",\"");
                //Error Type
                //sb.Append(errortype);
                sb.Append("\"\n");

                csv.Add(sb.ToString());

            }
            File.WriteAllText(filePath, String.Join("", csv.ToArray()));
            return String.Join("", csv.ToArray());
        }
    }
}
