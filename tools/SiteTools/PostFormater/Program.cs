using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostFormater
{
    class Program
    {
        private static string m_SiteDir = "";
        private static string m_PostDir = "";
        private static string m_PostName = "";
        private static string m_PostTitle = "";
        private static string m_PostDate = "";
        private static string m_PostCategory = "";
        private static string m_PostTags = "";
        private static string m_PostShortName = "";

        static void Main(string[] args)
        {
            Console.WriteLine(args.Length);
            if (args.Length < 8)
            {
                Console.WriteLine("usage PostFormater.exe name title date catetory tags");
                return;
            }

            m_SiteDir = args[0];
            m_PostDir = args[1];
            m_PostName = args[2];
            m_PostTitle = args[3];
            m_PostDate = args[4];
            m_PostCategory = args[5];
            m_PostTags = args[6];
            m_PostShortName = args[7];

            Console.WriteLine("m_SiteDir:{0}", m_SiteDir);
            Console.WriteLine("m_PostDir:{0}", m_PostDir);
            Console.WriteLine("m_PostName:{0}", m_PostName);
            Console.WriteLine("m_PostTitle:{0}", m_PostTitle);
            Console.WriteLine("m_PostDate:{0}", m_PostDate);
            Console.WriteLine("m_PostCategory:{0}", m_PostCategory);
            Console.WriteLine("m_PostTags:{0}", m_PostTags);
            Console.WriteLine("m_PostShortName:{0}", m_PostShortName);

            ProcessFiles();
            ProcessHtml();
            Deploy();
        }

        ////////////////////////////////////////////////////////////////////////
        /// 收集资源并重命名
        static List<Tuple<string, string>> m_Files = new List<Tuple<string, string>>();
        static void ProcessFiles()
        {
            //////////////////////////////////////////
            ///收集原资源目录内容
            string dir = Path.Combine(m_PostDir, m_PostName + "_files");
            DirectoryInfo folder = new DirectoryInfo(dir);
            foreach (FileInfo fi in folder.GetFiles())//遍历文件夹下所有文件
            {
                string newName = fi.Name.Replace(" ", "-").Replace("[", "").Replace("]", "");
                Tuple<string, string> pair = Tuple.Create(fi.Name, newName);
                m_Files.Add(pair);
            }

            //////////////////////////////////////////
            ///创建新资源目录
            string newDir = Path.Combine(m_PostDir, m_PostShortName);
            if (Directory.Exists(newDir))
            {
                Directory.Delete(newDir, true);
            }
            Directory.CreateDirectory(newDir);

            foreach (var pair in m_Files)
            {
                File.Copy(dir + "/" + pair.Item1, newDir + "/" + pair.Item2);
            }
        }

        ////////////////////////////////////////////////////////////////////////
        /// 生成新的post并替换资源
        static void ProcessHtml()
        {
            StringBuilder sb = new StringBuilder();

            //header信息
            sb.AppendLine("---");
            sb.AppendLine("title: " + m_PostTitle);
            sb.AppendLine("date: " + m_PostDate);
            sb.AppendLine("categories: ");
            string[] cates = m_PostCategory.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < cates.Length; idx++)
            {
                sb.AppendLine("  - " + cates[idx]);
            }
            sb.AppendLine("tags: ");
            string[] tags = m_PostTags.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int idx = 0; idx < tags.Length; idx++)
            {
                sb.AppendLine("  - " + tags[idx]);
            }
            sb.AppendLine("---");
            sb.AppendLine("");

            //正文html
            string htmlFile = Path.Combine(m_PostDir, m_PostName + ".html");
            string content = File.ReadAllText(htmlFile);
            string newContent = content.Replace("<title>Evernote Export</title>", "<title></title>");
            content = newContent;
            
            foreach (var pair in m_Files)
            {
                string oldPath = m_PostName + "_files" + "/" + pair.Item1;
                string newPath = "/assets/images/" + m_PostShortName + "/" + pair.Item2;
                newContent = content.Replace(oldPath, newPath);
                content = newContent;
            }
            sb.AppendLine(content);

            //写入新文件
            string newHtmlFile = Path.Combine(m_PostDir, m_PostShortName + ".md");
            File.WriteAllText(newHtmlFile, sb.ToString());
        }

        static void Deploy()
        {
            //拷贝post
            string frmHtmlFile = Path.Combine(m_PostDir, m_PostShortName + ".md");
            string toHtmlFile = Path.Combine(m_SiteDir + "/_posts", m_PostDate + "-" + m_PostShortName + ".md");
            File.Copy(frmHtmlFile, toHtmlFile, true);
            File.Delete(frmHtmlFile);

            //拷贝assets
            string frmAssetDir = Path.Combine(m_PostDir, m_PostShortName);
            string toAssetDir = Path.Combine(m_SiteDir + "/assets/images", m_PostShortName);
            if (Directory.Exists(toAssetDir))
            {
                Directory.Delete(toAssetDir, true);
            }
            Directory.CreateDirectory(toAssetDir);

            DirectoryInfo folder = new DirectoryInfo(frmAssetDir);
            foreach (FileInfo fi in folder.GetFiles())//遍历文件夹下所有文件
            {
                File.Copy(frmAssetDir + "/" + fi.Name, toAssetDir + "/" + fi.Name);
            }
            Directory.Delete(frmAssetDir, true);
        }
    }
}
