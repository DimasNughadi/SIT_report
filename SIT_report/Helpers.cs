using System;
using System.Collections.Generic;
using System.Text;

namespace SIT_report
{
    public class BearerTokenRoot
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }

    public class CourseRoot
    {
        public List<CourseItems> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class CourseItems
    {
        public string id { get; set; }
        public string institutionId { get; set; }
        public string organizationId { get; set; }
        public string departmentId { get; set; }
        public string name { get; set; }
        public string courseIdentifier { get; set; }
        public int sectionCount { get; set; }
        public string externalId { get; set; }

    }

    public class OrganizationRoot
    {
        public List<OrganizationItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }
    public class OrganizationItem
    {
        public string id { get; set; }
        public string institutionId { get; set; }
        public string name { get; set; }
        public int departmentCount { get; set; }
        public int courseCount { get; set; }
        public int sectionCount { get; set; }
        public string externalId { get; set; }

    }

    public class RoomItem
    {
        public string id { get; set; }
        public string buildingId { get; set; }
        public string institutionId { get; set; }
        public string name { get; set; }
        public string roomConfigurationId { get; set; }
        public string deviceSoftwareVersion { get; set; }
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string externalId { get; set; }

    }

    public class RoomsRoot
    {
        public List<RoomItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class SectionItem
    {
        public string id { get; set; }
        public string institutionId { get; set; }
        public string courseId { get; set; }
        public string termId { get; set; }
        public List<object> scheduleIds { get; set; }
        public string sectionNumber { get; set; }
        public string instructorId { get; set; }
        public string description { get; set; }
        public int lessonCount { get; set; }
        public int instructorCount { get; set; }
        public int studentCount { get; set; }
        public List<object> secondaryInstructorIds { get; set; }
        public List<object> lmsCourseIds { get; set; }
        public List<object> lmsCourses { get; set; }
        public string externalId { get; set; }

        public override string ToString()
        {
            return "id: " + id + "\n" +
                "institutionId: " + institutionId + "\n" +
                "courseId: " + courseId + "\n" +
                "termId: " + termId + "\n" +
                "scheduleIds:\n" + string.Join("\n", scheduleIds.ToArray()) + "\n" +
                "sectionNumber: " + sectionNumber + "\n" +
                "instructorId: " + instructorId + "\n" +
                "description: " + description + "\n" +
                "lessonCount: " + lessonCount + "\n" +
                "instructorCount: " + instructorCount + "\n" +
                "studentCount: " + studentCount + "\n" +
                "secondaryInstructorIds:\n" + string.Join("\n", secondaryInstructorIds.ToArray()) + "\n" +
                "lmsCourseIds:\n" + string.Join("\n", lmsCourseIds.ToArray()) + "\n" +
                "lmsCourses:\n" + string.Join("\n", lmsCourses.ToArray()) + "\n" +
                "externalId: " + externalId + "\n";
        }
    }

    public class SectionsRoot
    {
        public List<SectionItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class VenueItem
    {
        public string campusId { get; set; }
        public string campusName { get; set; }
        public string buildingId { get; set; }
        public string buildingName { get; set; }
        public string roomId { get; set; }
        public string roomName { get; set; }
    }

    public class PresenterItem
    {
        public string userId { get; set; }
        public string userEmail { get; set; }
        public string userFullName { get; set; }
    }

    public class ScheduleV2SectionItemAvailability
    {
        public string availability { get; set; }
        public string relativeDelay { get; set; }
        public string concreteTime { get; set; }
        public string unavailabilityDelay { get; set; }

    }

    public class ScheduleV2SectionItem
    {

        public string courseId { get; set; }
        public string courseIdentifier { get; set; }
        public string termId { get; set; }
        public string termName { get; set; }
        public string sectionId { get; set; }
        public string sectionName { get; set; }
        public string ScheduleV2SectionItemAvailability { get; set; }

    }

    public class ScheduleV2Item
    {
        public string institutionId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string endDate { get; set; }
        public List<string> daysOfWeek { get; set; }
        public List<DateTime> exclusionDates { get; set; }
        public VenueItem venue { get; set; }
        public PresenterItem presenter { get; set; }
        public string guestPresenter { get; set; }
        public List<ScheduleV2SectionItem> sections { get; set; }
        public string shouldCaption { get; set; }
        public string shouldStreamLive { get; set; }
        public string shouldAutoPublish { get; set; }
        public string shouldRecurCapture { get; set; }
        public string input1 { get; set; }
        public string input2 { get; set; }
        public string captureQuality { get; set; }
        public string streamQuality { get; set; }
        public string externalId { get; set; }
        public bool inRange(DateTime sdate, DateTime edate)
        {
            bool ret = false;
            if (endDate == null)
            {
                if (startDateTime.CompareTo(edate) <= 0) { ret = true; }
                else { ret = false; }
            }
            else
            {
                if (endtDateTime.CompareTo(sdate) <= 0 || startDateTime.CompareTo(edate) >= 0) { ret = false; }
                else { ret = true; }
            }
            return ret;
        }
        public DateTime startDateTime
        {
            get
            {
                return DateTime.Parse(startDate + " " + startTime);
            }
        }
        public DateTime endtDateTime
        {
            get
            {
                if (endDate == null)
                {
                    return new DateTime();
                }
                else
                {
                    return DateTime.Parse(endDate + " " + endTime);
                }
            }
        }
        public List<DateTime> LessonDates(DateTime start)
        {
            List<DateTime> datelist = new List<DateTime>();
            DateTime curr = start;
            while (curr.CompareTo(endDate) > 0)
            {
                foreach (DayOfWeek x in daysOfWeekList)
                {
                    DateTime nex = GetNextWeekday(curr, x);
                    datelist.Add(nex);
                }
                curr = curr.AddDays(7);
            }

            return datelist;
        }
        public static DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }
        public List<DayOfWeek> daysOfWeekList
        {
            get
            {
                if (daysOfWeek == null)
                {
                    return null;
                }
                else
                {
                    DayOfWeek curr = new DayOfWeek();
                    List<DayOfWeek> list = new List<DayOfWeek>();
                    foreach (string x in daysOfWeek)
                    {
                        switch (x.ToLower())
                        {
                            case "mo":
                                curr = DayOfWeek.Monday;
                                break;
                            case "tu":
                                curr = DayOfWeek.Tuesday;
                                break;
                            case "we":
                                curr = DayOfWeek.Wednesday;
                                break;
                            case "th":
                                curr = DayOfWeek.Thursday;
                                break;
                            case "fr":
                                curr = DayOfWeek.Friday;
                                break;
                            case "sa":
                                curr = DayOfWeek.Saturday;
                                break;
                            case "su":
                                curr = DayOfWeek.Sunday;
                                break;
                        }
                        list.Add(curr);

                    }

                    return list;

                }

            }
        }
    }

    public class ScheduleV2Root
    {
        public List<ScheduleV2Item> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class ScheduleItem
    {
        public string id { get; set; }
        public string startDate { get; set; }
        public string startTime { get; set; }
        public string endDate { get; set; }
        public List<string> daysOfWeek { get; set; }
        public List<string> exclusionDates { get; set; }
        public string durationMinutes { get; set; }
        public string sectionId { get; set; }
        public List<SectionItem> sections { get; set; }
        public string name { get; set; }
        public string roomId { get; set; }
        public string instructorId { get; set; }
        public string guestInstructor { get; set; }
        public string shouldCaption { get; set; }
        public string shouldAutoPublish { get; set; }
        public string shouldStreamLive { get; set; }
        public string input1 { get; set; }
        public string input2 { get; set; }
        public string captureQuality { get; set; }
        public string streamQuality { get; set; }
        public string externalId { get; set; }
        public override string ToString()
        {
            return id;
        }
        public bool inRange(DateTime sdate, DateTime edate)
        {
            bool ret = false;
            DateTime startDateTime = DateTime.Parse(startDate);
            DateTime endtDateTime;




            if (String.IsNullOrEmpty(endDate))
            {

                if (startDateTime.CompareTo(edate) <= 0) { ret = true; }
                else { ret = false; }
            }
            else
            {
                endtDateTime = DateTime.Parse(endDate);
                if (endtDateTime.CompareTo(sdate) <= 0 || startDateTime.CompareTo(edate) >= 0) { ret = false; }
                else { ret = true; }
            }
            return ret;
        }
    }

    public class SchedulesRoot
    {
        public List<ScheduleItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class TermSession
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class TermItem
    {
        public string id { get; set; }
        public string institutionId { get; set; }
        public string name { get; set; }
        public TermSession session { get; set; }
        public List<object> exceptions { get; set; }
        public int sectionCount { get; set; }
        public string externalId { get; set; }
    }


    public class TermsRoot
    {
        public List<TermItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class LessonTiming
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public override string ToString()
        {
            return start.ToString("yyyy-mm-dd hh:mm") + "," + end.ToString("yyyy-mm-dd hh:mm");
        }

    }

    public class LessonItem
    {
        public string id { get; set; }
        public string institutionId { get; set; }
        public string sectionId { get; set; }
        public string captureOccurrenceId { get; set; }
        public string name { get; set; }
        public LessonTiming timing { get; set; }
        public string timeZone { get; set; }
        public bool shouldStreamLive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool inRange(DateTime sdate, DateTime edate)
        {
            bool ret = false;
            if (timing.start >= sdate && timing.start <= edate) { ret = true; }
            return ret;
        }
        public override string ToString()
        {
            return id + "," + timing.ToString();
        }
    }

    public class Body
    {
        public string date { get; set; }
    }

    public class MediaContentAvailability
    {
        public Body body { get; set; }
        public string type { get; set; }
    }

    public class MediaAvailability
    {

        public string videoAvailability { get; set; }
        public string videoAvailabilityDate { get; set; }
        public string presentationAvailability { get; set; }
        public string presentationAvailabilityDate { get; set; }
    }
    public class MediaLessonItem
    {
        public string lessonId { get; set; }
        public string lessonName { get; set; }
        public LessonTiming lessonTiming { get; set; }
        public string sectionId { get; set; }
        public string courseId { get; set; }
        public string termId { get; set; }
        public string isAvailable { get; set; }
        public MediaContentAvailability contentAvailability { get; set; }

    }

    public class MediaRoot
    {
        public List<MediaItem> data { get; set; }
        public bool has_more { get; set; }
        public string next { get; set; }
    }

    public class MediaItem
    {
        public string mediaId { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string userId { get; set; }
        public string mediaType { get; set; }
        public string isPublishable { get; set; }
        public string timeZone { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string captureOccurrenceId { get; set; }

        public List<MediaLessonItem> lessons { get; set; }
    }


    public class PollStatus
    {
        public PollStatus()
        {
            s1_podoffline = false;
            s2_missedrec = false;
            s1_nostatus = false;
            s1_podnosync = false;
            s2_audionosig = false;
            s2_audionovol = false;
            s2_audiolowvol = false;
            s2_hdmi1nosig = false;
            s2_hdmi2nosig = false;
            s3_audionotbal = false;
            s3_podsyncerr = false;
            s3_cappaused = false;
            capturestatus = "";
            start = new DateTime();
        }
        public void SetStatus(string error)
        {
            if (error.ToLower().IndexOf("s1-podoffline") >= 0)
            {
                s1_podoffline = true;
            }
            if (error.ToLower().IndexOf("s2-missedrec") >= 0)
            {
                s2_missedrec = true;
            }
            if (error.ToLower().IndexOf("s1-nostatus") >= 0)
            {
                s1_nostatus = true;
            }
            if (error.ToLower().IndexOf("s1-podnosync") >= 0)
            {
                s1_podnosync = true;
            }
            if (error.ToLower().IndexOf("s2-audionosig") >= 0)
            {
                s2_audionosig = true;
            }
            if (error.ToLower().IndexOf("s2-audionovol") >= 0)
            {
                s2_audionovol = true;
            }
            if (error.ToLower().IndexOf("s2-audiolowvol") >= 0)
            {
                s2_audiolowvol = true;
            }
            if (error.ToLower().IndexOf("s2-hdmi1nosig") >= 0)
            {
                s2_hdmi1nosig = true;
            }
            if (error.ToLower().IndexOf("s2-hdmi2nosig") >= 0)
            {
                s2_hdmi2nosig = true;
            }
            if (error.ToLower().IndexOf("s3-audionotbal") >= 0)
            {
                s3_audionotbal = true;
            }
            if (error.ToLower().IndexOf("s3-podsyncerr") >= 0)
            {
                s3_podsyncerr = true;
            }
            if (error.ToLower().IndexOf("s3-cappaused") >= 0)
            {
                s3_cappaused = true;
            }
        }
        public bool s1_podoffline { get; set; }
        public bool s2_missedrec { get; set; }
        public bool s1_nostatus { get; set; }
        public bool s1_podnosync { get; set; }
        public bool s2_audionosig { get; set; }
        public bool s2_audionovol { get; set; }
        public bool s2_audiolowvol { get; set; }
        public bool s2_hdmi1nosig { get; set; }
        public bool s2_hdmi2nosig { get; set; }
        public bool s3_audionotbal { get; set; }
        public bool s3_podsyncerr { get; set; }
        public bool s3_cappaused { get; set; }
        public string capturestatus { get; set; }
        public DateTime start { get; set; }

        public string ToCSV()
        {
            string tmp = "";
            if (s1_podoffline) { tmp += "S1-PODOFFLINE "; }
            if (s2_missedrec) { tmp += "S2-MISSEDREC "; }
            if (s1_nostatus) { tmp += "S1-NOSTATUS "; }
            if (s1_podnosync) { tmp += "S1-PODNOSYNC "; }
            if (s2_audionosig) { tmp += "S2-AUDIONOSIG "; }
            if (s2_audionovol) { tmp += "S2-AUDIONOVOL "; }
            if (s2_audiolowvol) { tmp += "S2-AUDIOLOWVOL "; }
            if (s2_hdmi1nosig) { tmp += "S2-HDMI1NOSIG "; }
            if (s2_hdmi2nosig) { tmp += "S2-HDMI2NOSIG "; }
            if (s3_audionotbal) { tmp += "S3-AUDIONOTBAL "; }
            if (s3_podsyncerr) { tmp += "S3-PODSYNCERR "; }
            if (s3_cappaused) { tmp += "S3-CAPPAUSED "; }
            return tmp;
        }
    }
    public class CSVString
    {
        // Term
        public string termId { get; set; }
        public string termName { get; set; }

        // Course
        public string courseId { get; set; }
        public string courseDescription { get; set; }

        // Section
        public string sectionId { get; set; }
        public string sectionDescription { get; set; }

        // Lesson
        public string lessonName { get; set; }
        public DateTime lessonStart { get; set; }
        public DateTime lessonEnd { get; set; }
        public DateTime lessonCreatedAt { get; set; }

        // Media
        public string videoAvailability { get; set; }
        public string videoAvailabilityDate { get; set; }
        public string presentationAvailability { get; set; }
        public string presentationAvailabilityDate { get; set; }

        public CSVString(
            string termId, string termName,
            string courseId, string courseDescription,
            string sectionId, string sectionDescription,
            string lessonName, DateTime lessonCreatedAt, DateTime lessonStart, DateTime lessonEnd,
            string videoAvailability, string videoAvailabilityDate, string presentationAvailability, string presentationAvailabilityDate
            )
        {
            this.termId = termId;
            this.termName = termName;

            this.courseId = courseId;
            this.courseDescription = courseDescription;

            this.sectionId = sectionId;
            this.sectionDescription = sectionDescription;

            this.lessonName = lessonName;
            this.lessonStart = lessonStart;
            this.lessonEnd = lessonEnd;
            this.lessonCreatedAt = lessonCreatedAt;

            this.videoAvailability = videoAvailability;
            this.videoAvailabilityDate = videoAvailabilityDate;
            this.presentationAvailability = presentationAvailability;
            this.presentationAvailabilityDate = presentationAvailabilityDate;
        }

        public string GetCSV(char seperator)
        {
            //sw.WriteLine("Term ID|Term|Course ID|Course Description|Section ID|Section Description|Lesson Name|Lesson Created On|Lesson Start|
            //Lesson End|Video Available| Video Availabile On|Presentation Available|Presentation Availabile On");

            StringBuilder sb = new StringBuilder();

            //sb.Append(this.termId + seperator);
            sb.Append(termName + seperator);
            sb.Append(courseId + seperator);
            sb.Append(courseDescription + seperator);
            sb.Append(sectionId + seperator);
            sb.Append(sectionDescription + seperator);
            sb.Append(lessonName + seperator);
            sb.Append(lessonCreatedAt.ToString("dd/MM/yyyy HH:mm") + seperator);

            // Timing not available
            if (lessonStart.Year == 0001)
            {
                sb.Append(seperator);
                sb.Append(seperator);
            }
            else
            {
                sb.Append(lessonStart.ToString("dd/MM/yyyy HH:mm") + seperator);
                sb.Append(lessonEnd.ToString("dd/MM/yyyy HH:mm") + seperator);
            }

            sb.Append(videoAvailability + seperator);
            sb.Append(videoAvailabilityDate.ToString() + seperator);
            sb.Append(presentationAvailability + seperator);
            sb.Append(presentationAvailabilityDate);

            return sb.ToString();
        }

    }
}