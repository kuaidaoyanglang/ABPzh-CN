
using System;
using System.Windows.Forms;

namespace ABPzh_CN
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new MainFrm());
    }
  }
}
