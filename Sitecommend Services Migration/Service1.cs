using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Timers;
using MySql.Data.MySqlClient;
namespace EScenic1
{
    public partial class Service1 : ServiceBase
    {
        string strLogPath = System.Configuration.ConfigurationManager.AppSettings["LogPath"];
        Timer _timer = new Timer();
        string strLog = "";
        string strQry = "";
        private string strXmlFolderDate;

        public Service1()
        {
            InitializeComponent();
            _timer.Interval = Convert.ToDouble(ConfigurationSettings.AppSettings["TimerInterval"].ToString());

            //enabling the timer
            _timer.Enabled = true;

            //handle Elapsed event
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
        
        }

        private string sitecommand()
        {
            string strResul = "";
            strXmlFolderDate = "";
            strXmlFolderDate = DateTime.Now.ToString("dd/MM/yyyy");
            strXmlFolderDate = strXmlFolderDate.Replace("/", "").Replace("-", "").Replace(".", "").Replace(":", "");
            string strXmlFolderPath = System.Configuration.ConfigurationManager.AppSettings["XmlFolderPath"].ToString();
            if (Directory.Exists(strXmlFolderPath)) { }
            else
            {
                LogEntries("Directory Path Not Exists : " + strXmlFolderPath);
                return strResul;
            }
            string[] fileNames = Directory.GetFiles(strXmlFolderPath, "*.xml");

            DateTime[] creationTimes = new DateTime[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
                creationTimes[i] = new FileInfo(fileNames[i]).CreationTime;

            // sort it
            Array.Sort(creationTimes, fileNames);
            for (int i = 0; i < fileNames.Length; i++)
            {
                try
                {

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileNames[i].ToString());
                    site(xmlDoc, fileNames[i].ToString());
                }
                catch (Exception ex)
                {

                }
            }
            return strResul;
        }

        private void site(XmlDocument xdoc, string DocName)
        {
            string email = ""; string name = "";string strVal = ""; string ip = ""; string comment = ""; string upVotes = ""; string downVotes = ""; string ts = ""; string comment_id = ""; string parent_id = ""; string tags = ""; string user_avatar = ""; string article_id = ""; string article_title = ""; string url = ""; string isApproved = "";
            XmlNodeList _ContentNodeList = null;

            XmlNodeList ContentNodeList = xdoc.GetElementsByTagName("site-comments");
            foreach (XmlNode _nodeContent1 in ContentNodeList)
            {
                _ContentNodeList = _nodeContent1.ChildNodes;

                foreach (XmlNode _nodeContent in _ContentNodeList)

                {
                    if (_nodeContent.Name.ToLower() == "title")
                    { }
                    if (_nodeContent.Name.ToLower() == "link")
                    { }
                    if (_nodeContent.Name.ToLower() == "description")
                    { }
                    if (_nodeContent.Name.ToLower() == "copyright")
                    {
                    }
                    bool isArticleToBeInserted = true;
                    if (_nodeContent.Name.ToLower() == "users")
                    {

                        foreach (XmlNode lst in _nodeContent.ChildNodes)
                        {


                            if (lst.Name == "email")
                            {
                                try
                                {

                                    email = lst.FirstChild.Value;
                                    email = email.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "name")
                            {
                                try
                                {
                                    name = lst.FirstChild.Value;
                                    name = name.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "ip")
                            {
                                try
                                {
                                    ip = lst.FirstChild.Value;
                                    ip = ip.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "comment")

                            {

                                try

                                {
                                      //System.Text.Encoding utf_32 = System.Text.Encoding.UTF32;

                                    comment = lst.FirstChild.Value;
                                     
                                    strVal = Convert.ToBase64String(Encoding.UTF8.GetBytes(comment));
 
                                    comment = strVal.Replace("'", "''");

                                }

                                catch { }

                            }
                            if (lst.Name == "upVotes")
                            {
                                try
                                {
                                    upVotes = lst.FirstChild.Value;
                                    upVotes = upVotes.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "downVotes")
                            {
                                try {
                                    downVotes = lst.FirstChild.Value;
                                    downVotes = downVotes.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "parent_id")
                            {
                                try {
                                    parent_id = lst.FirstChild.Value;
                                    parent_id = parent_id.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "tags")
                            {
                                try
                                {
                                    tags = lst.FirstChild.Value;
                                    tags = tags.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "user_avatar")
                            {
                                try {
                                    user_avatar = lst.FirstChild.Value;
                                    user_avatar = user_avatar.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "url")
                            {
                                try {
                                    url = lst.FirstChild.Value;
                                    url = url.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "article_id")
                            {
                                try {
                                    article_id = lst.FirstChild.Value;
                                    article_id = article_id.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "article_title")
                            {
                                try {
                                    article_title = lst.FirstChild.Value;
                                    article_title = article_title.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "ts")
                            {
                                try {
                                    ts = lst.FirstChild.Value;
                                    ts = ts.Replace("'", "''");
                                }
                                catch { }
                            }
                            if (lst.Name == "comment_id")
                            {
                                try {
                                    comment_id = lst.FirstChild.Value;
                                    comment_id = comment_id.Replace("'", "''");//this is the overall xml that may be required for future purposes
                                }
                                catch { }
                            }
                            if (lst.Name == "isApproved")
                            {
                                try {
                                    isApproved = lst.FirstChild.Value;
                                    isApproved = isApproved.Replace("'", "''");//this is the overall xml that may be required for future purposes
                                }
                                catch { }
                            }
                        }

                        try
                        {
                            //check if the article already exists, if true, break


                            strQry = "INSERT INTO sitecomments(`email`,`name`,`ip`,`comment`,`upVotes`,`downVotes`,`ts`,`comment_id`,`parent_id`,`tags`,`user_avatar`,`article_id`,`article_title`,`url`,`isApproved`) VALUES  ";
                            strQry += " ( '" + email + "',";
                            strQry += "'" + name + "',";
                            strQry += "'" + ip + "',";
                            strQry += "'" + comment + "',";
                            strQry += " '" + upVotes + "',";
                            strQry += " '" + downVotes + "',";
                            strQry += " '" + ts + "',";
                            strQry += "'" + comment_id + "',";
                            strQry += " '" + parent_id + "',";
                            strQry += " '" + tags + "',";
                            strQry += " '" + user_avatar + "',";
                            strQry += " '" + article_id + "',";
                            strQry += " '" + article_title + "',";
                            strQry += " '" + url + "',";
                            strQry += " '" + isApproved + "'";

                            strQry += " ) ";


                            int result = ReturnDatatable(strQry);

                        }
                        catch { }
                        }
                }

                
            }
        }

        protected override void OnStart(string[] args)
        {
            _timer.Start();
            strLogPath = ConfigurationSettings.AppSettings["LogPath"].ToString();

            if (!Directory.Exists(strLogPath))
                Directory.CreateDirectory(strLogPath);

            if (!File.Exists(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQL_LOG.txt"))
                using (StreamWriter objWritter = File.CreateText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQL_LOG.txt"))
                    objWritter.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + " _XMLTOMYSQL_LOG  Service Started ....");
            else
                using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQL_LOG.txt"))
                    objWritter.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + " _XMLTOMYSQL_LOG  Service Started ....");
        }

        protected override void OnStop()
        {
            _timer.Stop();

            if (!Directory.Exists(strLogPath))
                Directory.CreateDirectory(strLogPath);

            if (!File.Exists(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQLSTOP_LOG.txt"))
                using (StreamWriter objWritter = File.CreateText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQL_LOG.txt"))
                    objWritter.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + " _XMLTOMYSQL_LOG  Service Stoped ....");
            else
                using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_XMLTOMYSQL_LOG.txt"))
                    objWritter.WriteLine(DateTime.Now.ToString("hh:mm:ss tt") + "_XMLTOMYSQL_LOG  Service Stoped ....");
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                string TimerDuration = ConfigurationSettings.AppSettings["TimerDuration"].ToString();
                string strCurrnetTime = System.DateTime.Now.Hour.ToString() + ":" + System.DateTime.Now.Minute.ToString();


                if (TimerDuration.Split(',').Contains(strCurrnetTime))
                {
                    strLog = DateTime.Now.ToString("hh:mm:ss tt") + "Hittttttted Time taken service " + strCurrnetTime;
                    if (!File.Exists(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                        using (StreamWriter objWritter = File.CreateText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                            objWritter.WriteLine(strLog);
                    else
                        using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                            objWritter.WriteLine(strLog);
                    sitecommand();
                }
                else
                {
                    strLog = DateTime.Now.ToString("hh:mm:ss tt") + " Time not Hitting service " + strCurrnetTime;
                    if (!File.Exists(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                        using (StreamWriter objWritter = File.CreateText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                            objWritter.WriteLine(strLog);
                    else
                        using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + "_DailtPageWise_LOG.txt"))
                            objWritter.WriteLine(strLog);
                }
            }
            catch (Exception ex1)
            {

            }
            finally
            {
                _timer.Start();
            }
        }
        //public string ProcessEscenicXml()
        //{
        //    string strResul = "";
        //    strXmlFolderDate = "";
        //    strXmlFolderDate = DateTime.Now.ToString("dd/MM/yyyy");
        //    strXmlFolderDate = strXmlFolderDate.Replace("/", "").Replace("-", "").Replace(".", "").Replace(":", "");
        //    string strXmlFolderPath = System.Configuration.ConfigurationManager.AppSettings["XmlFolderPath"].ToString();
        //    if (Directory.Exists(strXmlFolderPath)) { }
        //    else
        //    {
        //        LogEntries("Directory Path Not Exists : " + strXmlFolderPath);
        //        return strResul;
        //    }
        //    string[] fileNames = Directory.GetFiles(strXmlFolderPath, "*.xml");

        //    DateTime[] creationTimes = new DateTime[fileNames.Length];
        //    for (int i = 0; i < fileNames.Length; i++)
        //        creationTimes[i] = new FileInfo(fileNames[i]).CreationTime;

        //    // sort it
        //    Array.Sort(creationTimes, fileNames);
        //    for (int i = 0; i < fileNames.Length; i++)
        //    {
        //        try
        //        {

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.Load(fileNames[i].ToString());
        //            FormQuery(xmlDoc, fileNames[i].ToString());
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    return strResul;


        //}

        //private void FormQuery(XmlDocument xdoc, string DocName)
        //{


        //    XmlNodeList _ContentNodeList = null;

        //    XmlNodeList ContentNodeList = xdoc.GetElementsByTagName("escenic");
        //    foreach (XmlNode _nodeContent1 in ContentNodeList)
        //    {
        //        _ContentNodeList = _nodeContent1.ChildNodes;

        //        //ContentNodeList = _ContentNodeList;
        //        foreach (XmlNode _nodeContent in _ContentNodeList)
        //        {
        //            string section = ""; string subsection = ""; string title = ""; string lastmodify = ""; string priority = ""; string author = ""; string relation1 = ""; string ret1 = "";
        //            string leadtext = ""; string body = ""; string storykeyword = ""; string firstname = ""; string Sourceid = ""; string StorySourceid = "";
        //            string creationdate = ""; string publishdate = ""; string state = ""; string type = ""; string exported = ""; string exported1 = ""; string caption = "";
        //            string user = ""; string last = ""; string imgDesc = ""; string lastname = ""; string title1 = ""; string relatedarticles = ""; char leadflag = 'N';
        //            string slideshow = ""; string Sourceid_story = "";

        //            bool isArticleToBeInserted = true;
        //            if (_nodeContent.Name.ToLower() == "content")
        //            {

        //                relatedarticles = ""; // to make sure it does not get added for all the articles in a file
        //                try
        //                {
        //                    lastmodify = _nodeContent.Attributes["last-modified"].Value;
        //                }
        //                catch { }
        //                creationdate = _nodeContent.Attributes["creationdate"].Value;

        //                try
        //                {
        //                    publishdate = _nodeContent.Attributes["publishdate"].Value;

        //                }
        //                catch
        //                {
        //                    isArticleToBeInserted = false;
        //                    // break;
        //                }
        //                state = _nodeContent.Attributes["state"].Value;
        //                type = _nodeContent.Attributes["type"].Value;
        //                exported = _nodeContent.Attributes["exported-dbid"].Value;
        //                //venkat Include the source id also for the story/embed items
        //                StorySourceid = _nodeContent.Attributes["sourceid"].Value;
        //                title = _nodeContent.InnerXml;
        //                title = title.Replace("'", "''");//this is the overall xml that may be required for future purposes
        //                relatedarticles = "";
        //                foreach (XmlNode lst in _nodeContent.ChildNodes)
        //                {



        //                    if (lst.Name.ToLower() == "section-ref")
        //                    {
        //                        if (lst.Attributes.Count > 1)
        //                        {
        //                            subsection = lst.Attributes["unique-name"].Value;
        //                        }
        //                        else
        //                        {
        //                            section = lst.Attributes["unique-name"].Value;
        //                        }
        //                    }


        //                    try
        //                    {

        //                        if (lst.Name.ToLower() == "relation")
        //                        {

        //                            if (lst.Attributes["type"].Value == "lead")
        //                            {
        //                                try
        //                                {
        //                                    imgDesc = lst.ChildNodes[0].InnerText;
        //                                }
        //                                catch { }
        //                                try
        //                                {
        //                                    exported1 = lst.Attributes["exported-dbid"].Value;


        //                                    strQry = "select Exported_Picture_Article_id,creator from  Escenic_picture where Exported_Picture_Article_id='" + exported1 + "'";

        //                                    ret1 = Returnstring(strQry);
        //                                    if (ret1 != null)
        //                                    {

        //                                        leadflag = 'Y';

        //                                        //y is lead image available
        //                                        //A is lead not available


        //                                    }
        //                                    else
        //                                        leadflag = 'N';

        //                                    //lead image not available 
        //                                }
        //                                catch { }


        //                            }
        //                            try
        //                            {

        //                                if (lst.Attributes["type"].Value == "related")
        //                                {
        //                                    if (type == "story" || type == "story")
        //                                    {
        //                                        relatedarticles += lst.Attributes["exported-dbid"].Value + ',';
        //                                        relatedarticles = relatedarticles.Replace("'", "''");
        //                                    }

        //                                    else if (type == "embedYoutube")
        //                                    {

        //                                        relatedarticles += lst.Attributes["exported-dbid"].Value + ',';
        //                                        relatedarticles = relatedarticles.Replace("'", "''");
        //                                        strQry = "select Exported_Picture_Article_id,url from  Escenic_picture where Exported_Picture_Article_id='" + lst.Attributes["exported-dbid"].Value + "'";

        //                                        body = Returnstring(strQry);

        //                                    }
        //                                    else { }
        //                                }
        //                            }
        //                            catch { }

        //                        }
        //                        //gallery images
        //                        try
        //                        {

        //                            if (lst.Attributes["type"].Value == "slideshow")
        //                            {
        //                                slideshow += lst.Attributes["exported-dbid"].Value + ',';
        //                            }
        //                            slideshow = slideshow.Replace("'", "''");

        //                        }
        //                        catch { }
        //                    }
        //                    catch { }
        //                    try
        //                    {

        //                        if (lst.Name.ToLower() == "creator")
        //                        {
        //                            user = lst.Attributes["username"].Value;
        //                            last = lst.Attributes["last-name"].Value;
        //                        }
        //                    }
        //                    catch { }
        //                    try
        //                    {
        //                        if (lst.Name.ToLower() == "author")
        //                        {

        //                            try

        //                            {
        //                                lastname = lst.Attributes["last-name"].Value;
        //                            }
        //                            catch { }
        //                            //try
        //                            //{
        //                            //    username = lst.Attributes["username"].Value;
        //                            //}
        //                            //catch { }
        //                            try
        //                            {
        //                                firstname = lst.Attributes["first-name"].Value;
        //                            }
        //                            catch { }
        //                            author = firstname + ' ' + lastname;
        //                        }
        //                    }
        //                    catch { }
        //                    if (lst.Name.ToLower() == "field")
        //                    {
        //                        storykeyword = "";
        //                        if (lst.Attributes["name"].Value == "title")
        //                        {
        //                            title1 = lst.InnerText;
        //                            title1 = title1.Replace("'", "''");
        //                        }

        //                        if (lst.Attributes["name"].Value == "leadtext")
        //                        {
        //                            leadtext = lst.InnerText;
        //                            leadtext = leadtext.Replace("'", "''");
        //                        }
        //                        try
        //                        {
        //                            if (lst.Attributes["name"].Value == "priority")
        //                                priority = lst.InnerText;
        //                        }
        //                        catch { }
        //                        try
        //                        {
        //                            if (lst.Attributes["name"].Value == "embedurl")
        //                                body = lst.InnerText;
        //                        }
        //                        catch { }

        //                        if (lst.Attributes["name"].Value == "body")
        //                        {


        //                            //if (lst.ChildNodes.Count > 0)
        //                            //{
        //                            foreach (XmlNode p in lst.ChildNodes)
        //                            {
        //                                //body += p.InnerXml;
        //                                if (p.ChildNodes.Count > 0)
        //                                {
        //                                    //      }
        //                                    if (p.FirstChild.Name == "relation")
        //                                    {
        //                                        relation1 = p.InnerXml;
        //                                        Sourceid = p.FirstChild.Attributes["sourceid"].Value;
        //                                        Sourceid_story = p.FirstChild.Attributes["sourceid"].Value;

        //                                        if (p.FirstChild.FirstChild.Attributes["name"].Value == "title")
        //                                        {


        //                                            strQry = "select sourceid, Exported_Picture_Article_id from Escenic_picture where sourceid='" + Sourceid + "' ";


        //                                            var ret = "";
        //                                            ret = Returnstring(strQry);
        //                                            //venkat
        //                                            if (ret == "")
        //                                            {
        //                                                //check if the link exists as a embed item, use the same source id 
        //                                                strQry = "select StorySource_id, Escenic_Article_id from Escenic_Article where StorySource_id='" + Sourceid + "' ";
        //                                                ret = Returnstring(strQry);
        //                                            }


        //                                            p.InnerXml = "<img src ='" + ret + "' />";

        //                                            // img = "<img src ='../picture/_102937567_acdb397d-d7d7-4872-8f13-98c3befd675a_binary_24707391.jpg'/>";
        //                                            // p.InnerXml = img;

        //                                        }

        //                                        try
        //                                        {
        //                                            if (p.FirstChild.FirstChild.Attributes["name"].Value == "caption")
        //                                            {

        //                                                caption = p.FirstChild.FirstChild.Attributes["name"].InnerText;


        //                                            }
        //                                        }
        //                                        catch { }
        //                                        //p.innerxml = imgDesc + caption in xml format
        //                                        //"<img src ='../picture/" + img  + "title=" + caption + "/>"

        //                                    }
        //                                }
        //                                body += p.OuterXml;
        //                            }
        //                            //query the picture table with source id to get file name
        //                            //}
        //                            //body = lst.inner;
        //                            body = body.Replace("'", "''");
        //                        }

        //                        if (lst.Attributes["name"].Value == "storykeywords")

        //                        {
        //                            storykeyword = "";
        //                            if (lst.ChildNodes.Count > 0)
        //                            {
        //                                foreach (XmlNode keyword in lst.ChildNodes)
        //                                {

        //                                    storykeyword += keyword.InnerText + ',';

        //                                }
        //                                storykeyword = storykeyword.Replace("'", "''");
        //                            }
        //                        }

        //                    }

        //                    if (lst.Name.ToLower() == "tag")
        //                    {
        //                    }
        //                }
        //                if (section == "") section = subsection;
        //            }


        //            try
        //            {
        //                //check if the article already exists, if true, break

        //                strQry = "Select * from Escenic_Article where Escenic_Article_id='" + exported + "'";
        //                int result = ReturnDatatable(strQry);
        //                if (result == 1)
        //                {
        //                    using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                    {
        //                        objWritter.WriteLine(exported + "Already exists " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                    }
        //                }
        //                else if (result == -1)
        //                {
        //                    using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                    {
        //                        objWritter.WriteLine(exported + " " + "Database Error " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                    }
        //                }
        //                else
        //                {
        //                    strQry = "INSERT INTO  Escenic_Article (Escenic_Article_id,Type, Cate_Id, Sub_Cate_Id, Title_Ta, Keywords, Description_Ta, Author_Id,content,StorySource_id, Created_Date, Publish_Date, Modified_Date, Leadtext_id, Image_description, Photographer_Id, Article_text,GalleryImages,related_articles,Lead_flag) VALUES ";
        //                    strQry += " ( '" + exported + "',";
        //                    strQry += "'" + type + "',";
        //                    strQry += "'" + section + "',";
        //                    strQry += "'" + subsection + "',";
        //                    strQry += " '" + title1 + "',";
        //                    strQry += " '" + storykeyword + "',";
        //                    strQry += " '" + leadtext + "',";
        //                    strQry += "'" + author + "',";
        //                    strQry += " '" + body + "',";
        //                    strQry += " '" + StorySourceid
        //                        + "',";
        //                    strQry += " '" + creationdate + "',";
        //                    strQry += " '" + publishdate + "',";
        //                    strQry += " '" + lastmodify + "',";
        //                    strQry += " '" + exported1 + "',";
        //                    strQry += " '" + imgDesc + "',";
        //                    strQry += " '" + ret1 + "',";
        //                    strQry += " '" + title + "',";
        //                    strQry += " '" + slideshow + "',";
        //                    strQry += " '" + relatedarticles + "',";
        //                    strQry += " '" + leadflag + "'";
        //                    strQry += " ) ";

        //                    if (isArticleToBeInserted == true)
        //                    {
        //                        int result2;
        //                        result2 = ReturnDatatable(strQry);
        //                        if (result2 == 1)
        //                        {
        //                            using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                            {
        //                                objWritter.WriteLine(exported + "  " + " Inserted " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                            }
        //                        }
        //                        else
        //                        {
        //                            using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                            {
        //                                objWritter.WriteLine(exported + "  " + " Database error " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                        {
        //                            objWritter.WriteLine(exported + "  " + "Unpublished Story " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                        }

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "StoryXml.txt"))
        //                {
        //                    objWritter.WriteLine(ex.ToString());
        //                    objWritter.WriteLine(exported + " " + " Unknown error " + DateTime.Now.ToString("hh:mm:ss tt"));
        //                }

        //            }




        //        }

        //    }

        //}

        //public string ProcesstoPicture()
        //{
        //    string strResul = "";
        //    strXmlFolderDate = "";
        //    strXmlFolderDate = DateTime.Now.ToString("dd/MM/yyyy");
        //    strXmlFolderDate = strXmlFolderDate.Replace("/", "").Replace("-", "").Replace(".", "").Replace(":", "");
        //    string strXmlFolderPath = System.Configuration.ConfigurationManager.AppSettings["PictureFolderPath"].ToString();
        //    if (Directory.Exists(strXmlFolderPath)) { }
        //    else
        //    {
        //        LogEntries("Directory Path Not Exists : " + strXmlFolderPath);
        //        return strResul;
        //    }
        //    string[] fileNames = Directory.GetFiles(strXmlFolderPath, "*.xml");

        //    DateTime[] creationTimes = new DateTime[fileNames.Length];
        //    for (int i = 0; i < fileNames.Length; i++)
        //        creationTimes[i] = new FileInfo(fileNames[i]).CreationTime;

        //    // sort it
        //    Array.Sort(creationTimes, fileNames);
        //    for (int i = 0; i < fileNames.Length; i++)
        //    {
        //        try
        //        {

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.Load(fileNames[i].ToString());
        //            Picture(xmlDoc, fileNames[i].ToString());
        //        }
        //        catch (Exception ex)
        //        {
        //            using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //            {
        //                //objWritter.WriteLine(ex.ToString());
        //                objWritter.WriteLine(strQry);
        //            }
        //        }
        //    }
        //    return strResul;


        //}

        //private void Picture(XmlDocument xdoc, string DocName)
        //{

        //    string title = ""; string lastmodify = ""; string credits = ""; string Sourceid = ""; string title3 = "";
        //    string caption = ""; string firstname = ""; string url = ""; string type = ""; string leadtext = "";
        //    string creationdate = ""; string state = ""; string exported = ""; string title4 = ""; string ret1 = "";
        //    string lastname = ""; string title1 = ""; string creator = "";
        //    byte[] ImageData = null;
        //    XmlNodeList _ContentNodeList = null;

        //    XmlNodeList ContentNodeList = xdoc.GetElementsByTagName("escenic");
        //    foreach (XmlNode _nodeContent1 in ContentNodeList)
        //    {
        //        _ContentNodeList = _nodeContent1.ChildNodes;

        //        //ContentNodeList = _ContentNodeList;
        //        foreach (XmlNode _nodeContent in _ContentNodeList)
        //        {


        //            bool isArticleToBeInserted = true;
        //            if (_nodeContent.Name.ToLower() == "content")
        //            {
        //                url = ""; ImageData = null;
        //                lastmodify = _nodeContent.Attributes["last-modified"].Value;
        //                creationdate = _nodeContent.Attributes["creationdate"].Value;
        //                exported = _nodeContent.Attributes["exported-dbid"].Value;

        //                state = _nodeContent.Attributes["state"].Value;
        //                type = _nodeContent.Attributes["type"].Value;

        //                Sourceid = _nodeContent.Attributes["sourceid"].Value;
        //                title = _nodeContent.InnerXml;
        //                title = title.Replace("'", "''");//this is the overall xml that may be required for future purposes
        //                foreach (XmlNode lst in _nodeContent.ChildNodes)
        //                {

        //                    if (lst.Name.ToLower() == "creator")
        //                    {
        //                        try

        //                        {
        //                            lastname = lst.Attributes["last-name"].Value;
        //                        }
        //                        catch { }

        //                        try
        //                        {
        //                            firstname = lst.Attributes["first-name"].Value;
        //                        }
        //                        catch { }
        //                        creator = firstname + ' ' + lastname;
        //                    }


        //                    if (lst.Name.ToLower() == "field")
        //                    {
        //                        if (lst.Attributes["name"].Value == "title")
        //                        {
        //                            title1 = lst.InnerText;
        //                            title1 = title1.Replace("'", "''");
        //                        }

        //                        if (lst.Attributes["name"].Value == "caption")
        //                        {
        //                            caption = lst.InnerText;
        //                            caption = caption.Replace("'", "''");
        //                        }
        //                        if (lst.Attributes["name"].Value == "credits")
        //                            credits = lst.InnerText;
        //                        try
        //                        {
        //                            FileStream fs;
        //                            BinaryReader br;
        //                            try
        //                            {
        //                                if (lst.Attributes["name"].Value == "binary")
        //                                {
        //                                    title3 = lst.InnerText;

        //                                    string strXmlFolderPath = System.Configuration.ConfigurationManager.AppSettings["PictureFolderPath"].ToString();
        //                                    ret1 = strXmlFolderPath + title3;

        //                                    fs = new FileStream(ret1, FileMode.Open, FileAccess.Read);
        //                                    br = new BinaryReader(fs);

        //                                    ImageData = br.ReadBytes((int)fs.Length);

        //                                    br.Close();
        //                                    fs.Close();


        //                                }
        //                            }
        //                            catch (Exception ex) { }
        //                        }
        //                        catch { }

        //                        if (lst.Attributes["name"].Value == "leadtext")
        //                            leadtext = lst.InnerText;
        //                        leadtext = leadtext.Replace("'", "''");
        //                        if (lst.Attributes["name"].Value == "title")
        //                            title4 = lst.InnerText;
        //                        title4 = title4.Replace("'", "''");
        //                        if (lst.Attributes["name"].Value == "url")
        //                            url = lst.InnerText;

        //                    }


        //                }

        //            }


        //            try
        //            {
        //                //check if the article already exists, if true, break
        //                strQry = "Select Exported_Picture_Article_id from Escenic_picture where Exported_Picture_Article_id='" + exported + "'";
        //                int result = ReturnDatatable(strQry);
        //                if (result == 1)
        //                {
        //                    using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                    {
        //                        objWritter.WriteLine(exported + "  " + "Already exists " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                    }
        //                }
        //                else if (result == -1)
        //                {
        //                    using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                    {
        //                        objWritter.WriteLine(exported + "  " + "Database Error " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                    }
        //                }
        //                else
        //                {
        //                    strQry = "INSERT INTO Escenic_picture (Exported_Picture_Article_id, picture, Picture_name, creator, caption, credit,sourceid, createdate, modifieddate,url,type,title,leadtext) VALUES";
        //                    strQry += " ( '" + exported + "',";
        //                    strQry += " '" + ret1 + "',";
        //                    strQry += " '" + title1 + "',";

        //                    strQry += "'" + creator + "',";
        //                    strQry += " '" + caption + "',";
        //                    strQry += " '" + credits + "',";
        //                    strQry += " '" + Sourceid + "',";
        //                    strQry += " '" + creationdate + "',";
        //                    strQry += " '" + lastmodify + "',";
        //                    strQry += " '" + url + "',";
        //                    strQry += " '" + type + "',";
        //                    strQry += " '" + title4 + "',";
        //                    strQry += " '" + leadtext + "'";
        //                    strQry += " ) ";

        //                    if (isArticleToBeInserted == true)
        //                    {
        //                        int result1;
        //                        result1 = ReturnDatatable(strQry);
        //                        if (result1 == 1)
        //                        {
        //                            using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                            {
        //                                objWritter.WriteLine(exported + "  " + " Inserted " + DateTime.Now.ToString("hh:mm:ss tt"));
        //                                // update if imagedata is not null
        //                                //imagedata.length
        //                            }
        //                            string strObjClob = null;
        //                            strObjClob = System.Configuration.ConfigurationManager.AppSettings["ConStr"].ToString();
        //                            using (MySqlConnection conn = new MySqlConnection(strObjClob))
        //                            {
        //                                conn.Open();
        //                                String comand = "update Escenic_picture set picture=? where Exported_Picture_Article_id=?";

        //                                try
        //                                {
        //                                    MySqlCommand comando = new MySqlCommand(comand, conn);
        //                                    //Use the columnn name in the table
        //                                    comando.Parameters.AddWithValue("@picture", ImageData);
        //                                    comando.Parameters.AddWithValue("@Exported_Picture_Article_id", exported);
        //                                    int result2 = comando.ExecuteNonQuery();
        //                                }
        //                                catch (Exception ex) { }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                            {
        //                                objWritter.WriteLine(exported + "  " + " Database error " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                        {
        //                            objWritter.WriteLine(exported + "  " + "Unpublished Picture " + DateTime.Now.ToString("hh:mm:ss tt"));

        //                        }

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "PictureXml.txt"))
        //                {
        //                    objWritter.WriteLine(ex.ToString());
        //                    objWritter.WriteLine(exported + " Unknown error " + DateTime.Now.ToString("hh:mm:ss tt"));
        //                }
        //            }




        //        }

        //    }

        //}

        public int ReturnDatatable(string strQry)
        {
            string retvalue1 = ""; int result;
            try
            {
                string strObjClob = null;
                strObjClob = System.Configuration.ConfigurationManager.AppSettings["ConStr"].ToString();
                using (MySqlConnection conn = new MySqlConnection())
                {
                    conn.ConnectionString = strObjClob;
                    conn.Open();
                    MySqlCommand cmnd = new MySqlCommand(strQry, conn);
                    if (strQry.Contains("Select"))
                    {
                        MySqlDataReader dr = cmnd.ExecuteReader();
                        while (dr.Read())
                        {
                            retvalue1 = dr.GetValue(0).ToString();
                        }
                        dr.Close();
                        result = (retvalue1 == "") ? 0 : 1;
                    }
                    else
                    {
                        result = cmnd.ExecuteNonQuery();
                    }
                    cmnd.Dispose();
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {

                using (StreamWriter objWritter = File.AppendText(strLogPath + DateTime.Now.ToString("dd-MMM-yyyy") + " " + "Query.txt"))
                {
                    // objWritter.WriteLine(ex.ToString());
                    objWritter.WriteLine(strQry);
                }
                return -1;


            }

        }
        public string Returnstring(string strQry)
        {
            string retvalue = ""; string retvalue1 = ""; string retvalue2 = "";
            string _strConstr = System.Configuration.ConfigurationManager.AppSettings["ConStr"].ToString();
            try
            {
                using (MySqlConnection con = new MySqlConnection(_strConstr))
                {
                    con.ConnectionString = _strConstr;
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(strQry, con);

                    try
                    {
                        MySqlDataReader dr;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            retvalue1 = dr.GetValue(0).ToString();
                            retvalue2 = dr.GetValue(1).ToString();
                        }
                        dr.Close();
                        cmd.Dispose();
                        con.Close();
                    }
                    catch (Exception ex) { }
                    cmd.Dispose();
                }
            }
            catch (Exception ex1) { }
            if (retvalue1 != null)
            {
                retvalue = (retvalue2 != null) ? retvalue2 : "Admin";
            }
            return retvalue;
        }


        private void LogEntries(string v)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["LogFolder"].ToString() != "" && System.Configuration.ConfigurationManager.AppSettings["LogFile"].ToString() != "")
                {
                    //string strFileName = ConfigurationSettings.AppSettings["LogFile"].ToString();
                    string strFileName = System.Configuration.ConfigurationManager.AppSettings["LogFile"].ToString();
                    string strLogPath = System.Configuration.ConfigurationManager.AppSettings["LogFolder"].ToString();
                    string filename = strLogPath + DateTime.Now.ToString("dd-MMM-yyyy").Replace('.', '-').Replace('/', '-') + strFileName;
                    if (!Directory.Exists(strLogPath))
                        Directory.CreateDirectory(strLogPath);
                    if (!File.Exists(filename))
                    {
                        using (StreamWriter objWritter = File.CreateText(filename))
                        {
                            objWritter.WriteLine(strLog + " " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt").Replace('.', '-').Replace('/', '-'));
                        }
                    }
                    else
                    {
                        using (StreamWriter objWritter = File.AppendText(filename))
                        {
                            objWritter.WriteLine(strLog + " " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt").Replace('.', '-').Replace('/', '-'));
                        }
                    }
                }
            }
            catch { }


        }
    }
}
