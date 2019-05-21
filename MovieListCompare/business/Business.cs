//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieListCompare.Models;
using System.Windows;
using System.Collections;
using System.Globalization;

namespace MovieListCompare.business
{
    class MovieCompareClass
    {
        public List<MovieCompare> Compare(string stringAddress1, string stringAddress2, bool? BoolFolderSize)
        {
            List<MovieCompare> ListMovieCompare = new List<MovieCompare>();
            try
            {
                //Generate First Movie List
                List<string> stringArrayMovieListFirst = new List<string>();
                if (System.IO.File.Exists(stringAddress1))
                {
                    foreach (string string1 in System.IO.File.ReadAllLines(stringAddress1, Encoding.UTF8))
                    {
                        stringArrayMovieListFirst.Add(string1);
                    }
                }
                else
                {
                    MessageBox.Show("File not found: " + stringAddress1);
                }
                //Generate Second Movie List

                System.IO.DirectoryInfo DirectoryInfoSecond = new System.IO.DirectoryInfo(stringAddress2);
                List<string> stringArrayMovieListSecond = new List<string>();
                //تعریف کاراکتر جداکننده اعداد
                NumberFormatInfo NumberFormatInfo1 = new NumberFormatInfo();
                NumberFormatInfo1.NumberGroupSeparator = ".";
                foreach (System.IO.DirectoryInfo DirectoryInfoSub in DirectoryInfoSecond.GetDirectories())
                {
                    string stringSecondMovieName = DirectoryInfoSub.Name;
                    long longFolderSize = 0;
                    if (BoolFolderSize == true)
                    {
                        longFolderSize = FolderSize(DirectoryInfoSub) / 1024;
                    }
                    ListMovieCompare.Add(new MovieCompare("", stringSecondMovieName, longFolderSize.ToString("n0",NumberFormatInfo1), true, true));
                }
                foreach (System.IO.FileInfo FileInfo1 in DirectoryInfoSecond.GetFiles())
                {
                    string stringSecondMovieName = FileInfo1.Name;
                    ListMovieCompare.Add(new MovieCompare("", stringSecondMovieName, (FileInfo1.Length / 1024).ToString("n0", NumberFormatInfo1), false, true));
                }
                //Compare Two Lists
                foreach (MovieCompare MovieCompare1 in ListMovieCompare)
                {
                    foreach (string string1 in stringArrayMovieListFirst)
                    {
                        if (CompareMovieNames(string1, MovieCompare1.Movie2))
                        {
                            MovieCompare1.Movie1 = string1;
                            MovieCompare1.Copy = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return ListMovieCompare;
        }
        public bool CompareMovieNames(string stringMovieName1, string stringMovieName2)
        {
            //هر دو نام به حروف کوچک تبدیل میشوند
            stringMovieName1 = stringMovieName1.ToLower();
            stringMovieName2 = stringMovieName2.ToLower();
            // اطلاعات داخل پرانتز از آخر نامها حذف میشوند
            if (stringMovieName1.IndexOf('(') >= 0)
                stringMovieName1 = stringMovieName1.Remove(stringMovieName1.IndexOf('(')).Trim();
            if (stringMovieName2.IndexOf('(') >= 0)
                stringMovieName2 = stringMovieName2.Remove(stringMovieName2.IndexOf('(')).Trim();
            bool boolResult = false;

            if (!string.IsNullOrEmpty(stringMovieName1) && stringMovieName1[0] != '-' && stringMovieName1[0] != '_')
            {
                //مقایسه دو نام پس از تبدیل حروف فاصله به اسپیس
                string stringMovieName2Normalized = stringMovieName2;
                stringMovieName2Normalized = stringMovieName2Normalized.Replace('.', ' ');
                stringMovieName2Normalized = stringMovieName2Normalized.Replace('-', ' ');
                stringMovieName2Normalized = stringMovieName2Normalized.Replace('-', ' ');
                stringMovieName2Normalized = stringMovieName2Normalized.Replace(',', ' ');
                stringMovieName2Normalized = stringMovieName2Normalized.Replace(';', ' ');

                string stringMovieName1Normalized = stringMovieName1.Replace(" ", string.Empty);
                stringMovieName2Normalized = stringMovieName2Normalized.Replace(" ", string.Empty);
                // مقایسه دو نام پس از حذف کاراکترهای فاصله
                if (string.Equals(stringMovieName2Normalized, stringMovieName1Normalized))
                {
                    boolResult = true;
                }
                /// !!!!!!!!!!!!
                //این خط جهت بیشتر کردن دقت مقایسه اضافه شده اند و در صورت عدم لزوم قابل حذف شدن هستند. 
                /// !!!!!!!!!!!!
                //if (stringMovieName2Normalized.Contains(stringMovieName1Normalized))
                //{
                //    boolResult = true;
                //}
            }
            return boolResult;
        }
        public long FolderSize(System.IO.DirectoryInfo DirectoryInfoIn)
        {
            long longFolderSize = 0;
            foreach (System.IO.DirectoryInfo DirectoryInfoSub in DirectoryInfoIn.GetDirectories())
            {
                longFolderSize += FolderSize(DirectoryInfoSub);
            }
            foreach (System.IO.FileInfo FileInfo1 in DirectoryInfoIn.GetFiles())
            {
                longFolderSize += FileInfo1.Length;
            }
            return longFolderSize;
        }
        public static void CopyDirectory(System.IO.DirectoryInfo DirectoryInfoFrom, System.IO.DirectoryInfo DirectoryInfoTo)
        {
            try
            {
                if (DirectoryInfoFrom.Exists)
                {
                    System.IO.FileInfo[] Files = DirectoryInfoFrom.GetFiles();
                    System.IO.DirectoryInfo[] Directories = DirectoryInfoFrom.GetDirectories();

                    //System.IO.Directory.CreateDirectory(DirectoryInfoTo.FullName);
                    foreach (System.IO.FileInfo FileInfo1 in Files)
                    {
                        //System.IO.StreamWriter StreamWriter1 = new System.IO.StreamWriter("CopiedFiles.txt", true);
                        //StreamWriter1.WriteLine(FileInfo1.FullName);
                        //StreamWriter1.Close();

                        System.IO.File.Copy(FileInfo1.FullName, System.IO.Path.Combine(DirectoryInfoTo.FullName, FileInfo1.Name), false);
                    }
                    foreach (System.IO.DirectoryInfo DirectoryInfo1 in Directories)
                    {
                        CopyDirectory(DirectoryInfo1, new System.IO.DirectoryInfo(System.IO.Path.Combine(DirectoryInfoTo.FullName, DirectoryInfo1.Name)));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public bool ExportGridToCSV(IEnumerable IEnumerableGridData, string stringSaveAs)
        {
            try
            {
                StringBuilder StringBuilder1 = new StringBuilder(null);
                foreach (string a in MovieCompare.Properties)
                {
                    if (StringBuilder1.Length == 0)
                        StringBuilder1.Append(a);
                    else
                        StringBuilder1.Append(',' + a);
                }
                StringBuilder1.AppendLine();
                foreach (MovieCompare a in IEnumerableGridData)
                {
                    StringBuilder StringBuilderTemp = new StringBuilder(null);
                    foreach (var b in a.PropertiesValues())
                    {
                        if (StringBuilderTemp.Length == 0)
                            if (b.Length > 0)
                                StringBuilderTemp.Append(b);
                            else
                                StringBuilderTemp.Append(" ");
                        else
                            StringBuilderTemp.Append(',' + b);
                    }
                    StringBuilder1.AppendLine(StringBuilderTemp.ToString());
                }
                System.IO.File.WriteAllText(stringSaveAs, StringBuilder1.ToString(), Encoding.UTF8);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
