// Decompiled with JetBrains decompiler
// Type: ABPzh_CN.PubClass.ScanCS.ScanCsClass
// Assembly: ABPzh-CN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33EE463D-E4AD-4081-9141-5697FEC1B44A
// Assembly location: F:\Source Code\ABP\ABP本地化语言XML快捷制作工具v1.1\ABPzh-CN.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ABPzh_CN.PubClass.ScanCS
{
  public class ScanCsClass
  {
    public List<string> Tests = new List<string>();

    public void ScanCs(string dirPath)
    {
      List<string> stringList1 = new List<string>();
      string[] array = ((IEnumerable<string>) Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories)).Where<string>((Func<string, bool>) (s =>
      {
        if (!s.EndsWith(".cshtml") && !s.EndsWith(".js"))
          return s.EndsWith(".cs");
        return true;
      })).ToArray<string>();
      int length = 100;
      if (array.Length < length)
        length = array.Length;
      Task<List<string>>[] taskArray = new Task<List<string>>[length];
      List<string>[] stringListArray = new List<string>[length];
      int index1 = 0;
      for (int index2 = 0; index2 < length; ++index2)
        stringListArray[index2] = new List<string>();
      foreach (string str in array)
      {
        stringListArray[index1].Add(str);
        ++index1;
        if (index1 >= length)
          index1 = 0;
      }
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (taskArray[index2] == null)
        {
          List<string> t = stringListArray[index2];
          Console.WriteLine((object) t);
          taskArray[index2] = new Task<List<string>>((Func<List<string>>) (() => ScanCsClass.getFile(t)));
          taskArray[index2].Start();
        }
      }
      Task.WaitAll((Task[]) taskArray);
      for (int index2 = 0; index2 < length; ++index2)
        stringList1.AddRange((IEnumerable<string>) taskArray[index2].Result);
      List<string> stringList2 = new List<string>();
      foreach (string str in stringList1)
      {
        if (!stringList2.Contains(str))
          stringList2.Add(str);
      }
      this.Tests = stringList2;
    }

    public static List<string> getFile(List<string> list)
    {
      List<string> stringList = new List<string>();
      try
      {
        foreach (string path in list)
        {
          string input = File.ReadAllText(path, Encoding.UTF8);
          string pattern1 = "app.localize\\([^\\(^\\)]*(\\'|\\\")";
          string pattern2 = "@L\\([^\\(^\\)]*(\\'|\\\")";
          string pattern3 = " L\\(\\\"[^^\\(^\\)]*";
          Regex regex1 = new Regex(pattern1, RegexOptions.IgnoreCase);
          Regex regex2 = new Regex(pattern2, RegexOptions.IgnoreCase);
          Regex regex3 = new Regex(pattern3, RegexOptions.IgnoreCase);
          MatchCollection matchCollection1 = regex1.Matches(input);
          MatchCollection matchCollection2 = regex2.Matches(input);
          MatchCollection matchCollection3 = regex3.Matches(input);
          foreach (Capture capture in matchCollection1)
          {
            string str = capture.Value.Trim().Replace("app.localize(", "").Replace("\"", "").Replace("'", "");
            stringList.Add(str);
          }
          foreach (Capture capture in matchCollection2)
          {
            string str = capture.Value.Trim().Replace("@L(", "").Replace("\"", "").Replace("'", "");
            stringList.Add(str);
          }
          foreach (Capture capture in matchCollection3)
          {
            string str = capture.Value.Replace(" L(", "").Trim().Replace("\"", "").Replace("'", "");
            stringList.Add(str);
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
      }
      return stringList;
    }
  }
}
