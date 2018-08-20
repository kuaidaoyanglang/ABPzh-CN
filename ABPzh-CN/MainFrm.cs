using ABPzh_CN.PubClass.Http;
using ABPzh_CN.PubClass.ScanCS;
using ABPzh_CN.PubClass.XML;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABPzh_CN
{
  public class MainFrm : Form
  {
    private string xmlPath = "";
    private string vpDir = "";
    private XMLHelperClass xmlHelper = new XMLHelperClass();
    private ScanCsClass scanCs = new ScanCsClass();
    private IContainer components = (IContainer) null;
    private GroupBox groupBox1;
    private Button button2;
    private TextBox textBox2;
    private Button button1;
    private TextBox textBox1;
    private Label label2;
    private Label label1;
    private Button button3;
    private GroupBox groupBox2;
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private GroupBox groupBox3;
    private Button button6;
    private Button button5;
    private Button button4;
    private TextBox textBox5;
    private Label label5;
    private TextBox textBox4;
    private TextBox textBox3;
    private Label label4;
    private Label label3;
    private GroupBox groupBox4;
    private ListView listView2;
    private ColumnHeader columnHeader4;
    private ColumnHeader columnHeader5;
    private ColumnHeader columnHeader6;
    private FolderBrowserDialog folderBrowserDialog1;
    private OpenFileDialog openFileDialog1;

    public MainFrm()
    {
      this.InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.Multiselect = false;
      this.openFileDialog1.Filter = "XML文件(*.Xml)|*.xml";
      if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox1.Text = this.openFileDialog1.FileName;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.folderBrowserDialog1.SelectedPath = Application.StartupPath;
      if (this.folderBrowserDialog1.ShowDialog() != DialogResult.OK)
        return;
      this.textBox2.Text = this.folderBrowserDialog1.SelectedPath;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      try
      {
        this.xmlHelper.loadXml(this.textBox1.Text);
        if (this.xmlHelper.Texts.Count == 0)
        {
          int num = (int) MessageBox.Show("载入XML失败!");
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
        int num = (int) MessageBox.Show("载入XML失败!");
        return;
      }
      try
      {
        this.scanCs.ScanCs(this.textBox2.Text);
        if (this.scanCs.Tests.Count == 0)
        {
          int num = (int) MessageBox.Show("文件夹中未搜索到需要翻译的字符,或者没有*.cshtml文件!");
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
        int num = (int) MessageBox.Show("文件夹中未搜索到需要翻译的字符,或者没有*.cshtml文件!");
        return;
      }
      this.listView1.Items.Clear();
      this.listView2.Items.Clear();
      this.InitPp(this.listView1, this.listView2);
    }

    private void InitPp(ListView list1, ListView list2)
    {
      this.xmlHelper.TextsOK.Clear();
      this.xmlHelper.TextsNo.Clear();
      int num1 = 0;
      int num2 = 0;
      foreach (TextClass text in this.xmlHelper.Texts)
      {
        text.Index = num1;
        this.xmlHelper.TextsOK.Add(text);
        ++num1;
      }
      foreach (string test in this.scanCs.Tests)
      {
        string scanText = test;
        if (this.xmlHelper.Texts.Where<TextClass>((Func<TextClass, bool>) (x => x.Name == scanText)).FirstOrDefault<TextClass>() == null)
        {
          this.xmlHelper.TextsNo.Add(new TextClass()
          {
            Name = scanText,
            Value = "",
            IsValue = true,
            Index = num2
          });
          ++num2;
        }
      }
      foreach (TextClass textClass in this.xmlHelper.TextsOK)
      {
        ListViewItem listViewItem = list1.Items.Add(textClass.Index.ToString());
        listViewItem.SubItems.Add(textClass.Name);
        listViewItem.SubItems.Add(textClass.Value);
      }
      foreach (TextClass textClass in this.xmlHelper.TextsNo)
      {
        ListViewItem listViewItem = list2.Items.Add(textClass.Index.ToString());
        listViewItem.SubItems.Add(textClass.Name);
        listViewItem.SubItems.Add("");
      }
    }

    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.textBox3.Text = this.listView1.SelectedItems[0].SubItems[1].Text;
        this.textBox4.Text = this.ToSp(this.listView1.SelectedItems[0].SubItems[1].Text);
        this.textBox5.Text = this.listView1.SelectedItems[0].SubItems[2].Text;
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    private void listView2_SelectedIndexChanged(object sender, EventArgs e)
    {
      try
      {
        this.textBox3.Text = this.listView2.SelectedItems[0].SubItems[1].Text;
        this.textBox4.Text = this.ToSp(this.listView2.SelectedItems[0].SubItems[1].Text);
        this.textBox5.Text = "";
        this.button4.PerformClick();
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
    }

    private string ToSp(string t)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int startIndex = 0; startIndex < t.Length; ++startIndex)
      {
        string s = t.Substring(startIndex, 1);
        byte[] bytes = Encoding.ASCII.GetBytes(s);
        if (bytes[0] >= (byte) 97 && bytes[0] <= (byte) 122)
          stringBuilder.Append(s);
        else
          stringBuilder.Append(" ").Append(s);
      }
      return stringBuilder.ToString();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      this.button4.Enabled = false;
      this.textBox5.Text = HttpClass.GetEntoCn(this.textBox4.Text.Trim());
      this.button4.Enabled = true;
    }

    private void button6_Click(object sender, EventArgs e)
    {
      this.button6.Enabled = false;
      string yEn = this.textBox3.Text;
      string text = this.textBox5.Text;
      TextClass textClass1 = this.xmlHelper.TextsOK.Where<TextClass>((Func<TextClass, bool>) (x => x.Name == yEn)).FirstOrDefault<TextClass>();
      if (textClass1 != null)
      {
        textClass1.Value = text;
        this.xmlHelper.TextsOK[textClass1.Index] = textClass1;
        this.listView1.Items[textClass1.Index].SubItems[2].Text = textClass1.Value;
      }
      else
      {
        TextClass textClass2 = this.xmlHelper.TextsNo.Where<TextClass>((Func<TextClass, bool>) (x => x.Name == yEn)).FirstOrDefault<TextClass>();
        if (textClass2 != null)
        {
          this.xmlHelper.TextsNo.Remove(textClass2);
          this.listView2.Items.Remove(this.listView2.SelectedItems[0]);
          textClass2.Value = text;
          textClass2.Index = this.xmlHelper.TextsOK.Count;
          this.xmlHelper.TextsOK.Add(textClass2);
        }
        else
        {
          textClass2 = new TextClass();
          textClass2.Name = yEn;
          textClass2.Value = text;
          textClass2.IsValue = true;
          textClass2.Index = this.xmlHelper.TextsOK.Count;
          this.xmlHelper.TextsOK.Add(textClass2);
        }
        ListViewItem listViewItem = this.listView1.Items.Add(textClass2.Index.ToString());
        listViewItem.SubItems.Add(textClass2.Name);
        listViewItem.SubItems.Add(textClass2.Value);
      }
      try
      {
        if (this.listView2.Items.Count - 1 > this.listView2.SelectedItems[0].Index)
        {
          this.listView2.Items[this.listView2.SelectedItems[0].Index].Selected = true;
          this.listView2.Items[this.listView2.SelectedItems[0].Index].EnsureVisible();
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine((object) ex);
      }
      this.button6.Enabled = true;
    }

    private void ShowList()
    {
      this.listView1.Items.Clear();
      this.listView2.Items.Clear();
      foreach (TextClass textClass in this.xmlHelper.TextsOK)
      {
        ListViewItem listViewItem = this.listView1.Items.Add(textClass.Index.ToString());
        listViewItem.SubItems.Add(textClass.Name);
        listViewItem.SubItems.Add(textClass.Value);
      }
      foreach (TextClass textClass in this.xmlHelper.TextsNo)
      {
        ListViewItem listViewItem = this.listView2.Items.Add(textClass.Index.ToString());
        listViewItem.SubItems.Add(textClass.Name);
        listViewItem.SubItems.Add("");
      }
    }

    private void button5_Click(object sender, EventArgs e)
    {
      this.xmlHelper.writeXml("AbpZeroTemplate-zh-CN.xml");
      int num = (int) MessageBox.Show("OK");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.button3 = new Button();
      this.button2 = new Button();
      this.textBox2 = new TextBox();
      this.button1 = new Button();
      this.textBox1 = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.groupBox2 = new GroupBox();
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.groupBox3 = new GroupBox();
      this.button6 = new Button();
      this.button5 = new Button();
      this.button4 = new Button();
      this.textBox5 = new TextBox();
      this.label5 = new Label();
      this.textBox4 = new TextBox();
      this.textBox3 = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.groupBox4 = new GroupBox();
      this.listView2 = new ListView();
      this.columnHeader4 = new ColumnHeader();
      this.columnHeader5 = new ColumnHeader();
      this.columnHeader6 = new ColumnHeader();
      this.folderBrowserDialog1 = new FolderBrowserDialog();
      this.openFileDialog1 = new OpenFileDialog();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Controls.Add((Control) this.button3);
      this.groupBox1.Controls.Add((Control) this.button2);
      this.groupBox1.Controls.Add((Control) this.textBox2);
      this.groupBox1.Controls.Add((Control) this.button1);
      this.groupBox1.Controls.Add((Control) this.textBox1);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(12, 12);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(646, 79);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.button3.Location = new Point(580, 20);
      this.button3.Name = "button3";
      this.button3.Size = new Size(55, 48);
      this.button3.TabIndex = 6;
      this.button3.Text = "匹配";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.button2.Location = new Point(542, 47);
      this.button2.Name = "button2";
      this.button2.Size = new Size(32, 22);
      this.button2.TabIndex = 5;
      this.button2.Text = "...";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.textBox2.Location = new Point(74, 47);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(462, 21);
      this.textBox2.TabIndex = 4;
      this.textBox2.Text = "F:\\mygit\\my_csharp\\HG-zero-3.4.0\\HG.AbpZero.Web";
      this.button1.Location = new Point(542, 20);
      this.button1.Name = "button1";
      this.button1.Size = new Size(32, 22);
      this.button1.TabIndex = 3;
      this.button1.Text = "...";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.textBox1.Location = new Point(74, 20);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(462, 21);
      this.textBox1.TabIndex = 2;
      this.textBox1.Text = "F:\\mygit\\my_csharp\\HG-zero-3.4.0\\HG.AbpZero.Core\\Localization\\AbpZeroTemplate\\AbpZeroTemplate-zh-CN.xml";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(21, 50);
      this.label2.Name = "label2";
      this.label2.Size = new Size(47, 12);
      this.label2.TabIndex = 1;
      this.label2.Text = "VP-Dir:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 23);
      this.label1.Name = "label1";
      this.label1.Size = new Size(29, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "Xml:";
      this.groupBox2.Controls.Add((Control) this.listView1);
      this.groupBox2.Location = new Point(12, 97);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(320, 221);
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "已经匹配";
      this.listView1.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.listView1.Dock = DockStyle.Fill;
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(3, 17);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(314, 201);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
      this.columnHeader1.Text = "编号";
      this.columnHeader2.Text = "单词";
      this.columnHeader2.Width = 120;
      this.columnHeader3.Text = "中文";
      this.columnHeader3.Width = 120;
      this.groupBox3.Controls.Add((Control) this.button6);
      this.groupBox3.Controls.Add((Control) this.button5);
      this.groupBox3.Controls.Add((Control) this.button4);
      this.groupBox3.Controls.Add((Control) this.textBox5);
      this.groupBox3.Controls.Add((Control) this.label5);
      this.groupBox3.Controls.Add((Control) this.textBox4);
      this.groupBox3.Controls.Add((Control) this.textBox3);
      this.groupBox3.Controls.Add((Control) this.label4);
      this.groupBox3.Controls.Add((Control) this.label3);
      this.groupBox3.Location = new Point(12, 324);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(646, 118);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      this.button6.Location = new Point(520, 87);
      this.button6.Name = "button6";
      this.button6.Size = new Size(57, 21);
      this.button6.TabIndex = 8;
      this.button6.Text = "匹配";
      this.button6.UseVisualStyleBackColor = true;
      this.button6.Click += new EventHandler(this.button6_Click);
      this.button5.Location = new Point(583, 31);
      this.button5.Name = "button5";
      this.button5.Size = new Size(57, 77);
      this.button5.TabIndex = 7;
      this.button5.Text = "写出";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new EventHandler(this.button5_Click);
      this.button4.Location = new Point(520, 31);
      this.button4.Name = "button4";
      this.button4.Size = new Size(57, 21);
      this.button4.TabIndex = 6;
      this.button4.Text = "查询";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new EventHandler(this.button4_Click);
      this.textBox5.Location = new Point(86, 87);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new Size(428, 21);
      this.textBox5.TabIndex = 5;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(21, 90);
      this.label5.Name = "label5";
      this.label5.Size = new Size(59, 12);
      this.label5.TabIndex = 4;
      this.label5.Text = "查询结果:";
      this.textBox4.Location = new Point(86, 60);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(428, 21);
      this.textBox4.TabIndex = 3;
      this.textBox3.Location = new Point(86, 31);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(428, 21);
      this.textBox3.TabIndex = 2;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(21, 63);
      this.label4.Name = "label4";
      this.label4.Size = new Size(59, 12);
      this.label4.TabIndex = 1;
      this.label4.Text = "查询单词:";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(33, 34);
      this.label3.Name = "label3";
      this.label3.Size = new Size(47, 12);
      this.label3.TabIndex = 0;
      this.label3.Text = "原单词:";
      this.groupBox4.Controls.Add((Control) this.listView2);
      this.groupBox4.Location = new Point(338, 97);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(320, 221);
      this.groupBox4.TabIndex = 3;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "未匹配";
      this.listView2.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader4,
        this.columnHeader5,
        this.columnHeader6
      });
      this.listView2.Dock = DockStyle.Fill;
      this.listView2.FullRowSelect = true;
      this.listView2.GridLines = true;
      this.listView2.Location = new Point(3, 17);
      this.listView2.Name = "listView2";
      this.listView2.Size = new Size(314, 201);
      this.listView2.TabIndex = 0;
      this.listView2.UseCompatibleStateImageBehavior = false;
      this.listView2.View = View.Details;
      this.listView2.SelectedIndexChanged += new EventHandler(this.listView2_SelectedIndexChanged);
      this.columnHeader4.Text = "编号";
      this.columnHeader5.Text = "单词";
      this.columnHeader5.Width = 120;
      this.columnHeader6.Text = "中文";
      this.columnHeader6.Width = 120;
      this.openFileDialog1.FileName = "openFileDialog1";
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(668, 450);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Name = nameof (MainFrm);
      this.Text = "ABP翻译机";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
