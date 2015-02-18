using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using tesseract;

namespace AutoSQ
{
  public class Process
  {
    public readonly Bitmap Bmp;
    public Bitmap NamePrint;
    private TesseractProcessor _processor;
    public Bitmap TestingBmp;
    private readonly bool _testing;

    public Process(bool testing) : this()
    {
      _testing = testing;
    }

    public Process()
    {
      var size = new Size(350, 54 * 8);
      var start = new Point(781, 467);

      Bmp = new Bitmap(size.Width, size.Height);
      var graphic = Graphics.FromImage(Bmp);
      graphic.CopyFromScreen(start, Point.Empty, size);

      if (_testing) TestingBmp = Bmp;
    }

    public void Go(Action<string[]> onCompleted)
    {
      int count = GetNumberOfPlayers(Processor);
      var lines = Enumerable.Repeat("...", count).ToArray();
      
      for (int i = 0; i < count; i++)
      {
        if (_testing) TestingBmp = GetBmp(54 * i);
        lines[i] = (Processor.Apply(GetBmp(54 * i)));
        onCompleted(lines);
      }
    }

    private int GetNumberOfPlayers(TesseractProcessor processer2)
    {
      NamePrint = GetNamePrint();
      var blah = processer2.Apply(NamePrint);
      for (int i = blah.Length - 1; i >= 0; i--)
      {
        if (blah[i] == '\n')
          blah = blah.Remove(i, 1);
        else
          break;
      }

      return blah.ToCharArray().ToList().Count(x => x == '\n') + 1;
    }

    private Bitmap GetBmp(int y)
    {
      var bmp = GetScorePrint(new Point(0, y), new Size(350, 50));
      bmp = MakeGrayscale2(bmp);
      bmp = ApplyInvert(bmp);
      bmp = AdjustContrast(bmp, 50);
      bmp = AdjustImage(bmp, .1f, 1.9f);
      return bmp;
    }

    public static Bitmap AdjustImage(Bitmap original, float brightness, float contrast)
    {
      const float gamma = 1.0f;

      float adjustedBrightness = brightness - 1.0f;
      // create matrix that will brighten and contrast the image
      float[][] ptsArray ={
                    new[] {contrast, 0, 0, 0, 0}, // scale red
                    new[] {0, contrast, 0, 0, 0}, // scale green
                    new[] {0, 0, contrast, 0, 0}, // scale blue
                    new[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                    new[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

      var imageAttributes = new ImageAttributes();
      imageAttributes.ClearColorMatrix();
      imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
      imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
      Graphics g = Graphics.FromImage(original);
      g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height)
          , 0, 0, original.Width, original.Height,
          GraphicsUnit.Pixel, imageAttributes);

      return original;
    }

    public static Bitmap AdjustContrast(Bitmap image, float value)
    {
      value = (100.0f + value) / 100.0f;
      value *= value;
      var newBitmap = (Bitmap)image.Clone();
      BitmapData data = newBitmap.LockBits(
          new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
          ImageLockMode.ReadWrite,
          newBitmap.PixelFormat);

      unsafe
      {
        for (int y = 0; y < newBitmap.Height; ++y)
        {
          byte* row = (byte*)data.Scan0 + (y * data.Stride);
          int columnOffset = 0;
          for (int x = 0; x < newBitmap.Width; ++x)
          {
            byte b = row[columnOffset];
            byte g = row[columnOffset + 1];
            byte r = row[columnOffset + 2];

            float red = r / 255.0f;
            float green = g / 255.0f;
            float blue = b / 255.0f;
            red = (((red - 0.5f) * value) + 0.5f) * 255.0f;
            green = (((green - 0.5f) * value) + 0.5f) * 255.0f;
            blue = (((blue - 0.5f) * value) + 0.5f) * 255.0f;

            var iR = (int)red;
            iR = iR > 255 ? 255 : iR;
            iR = iR < 0 ? 0 : iR;
            var iG = (int)green;
            iG = iG > 255 ? 255 : iG;
            iG = iG < 0 ? 0 : iG;
            var iB = (int)blue;
            iB = iB > 255 ? 255 : iB;
            iB = iB < 0 ? 0 : iB;

            row[columnOffset] = (byte)iB;
            row[columnOffset + 1] = (byte)iG;
            row[columnOffset + 2] = (byte)iR;

            columnOffset += 4;
          }
        }
      }

      newBitmap.UnlockBits(data);

      return newBitmap;
    }

    public static Bitmap MakeGrayscale2(Bitmap original)
    {
      unsafe
      {
        //create an empty bitmap the same size as original
        var newBitmap = new Bitmap(original.Width, original.Height);

        //lock the original bitmap in memory
        BitmapData originalData = original.LockBits(
           new Rectangle(0, 0, original.Width, original.Height),
           ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        //lock the new bitmap in memory
        BitmapData newData = newBitmap.LockBits(
           new Rectangle(0, 0, original.Width, original.Height),
           ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

        //set the number of bytes per pixel
        const int pixelSize = 3;

        for (int y = 0; y < original.Height; y++)
        {
          //get the data from the original image
          byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

          //get the data from the new image
          byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

          for (int x = 0; x < original.Width; x++)
          {
            //create the grayscale version
            var grayScale =
               (byte)((oRow[x * pixelSize] * .11) + //B
               (oRow[x * pixelSize + 1] * .59) +  //G
               (oRow[x * pixelSize + 2] * .3)); //R

            //set the new image's pixel to the grayscale version
            nRow[x * pixelSize] = grayScale; //B
            nRow[x * pixelSize + 1] = grayScale; //G
            nRow[x * pixelSize + 2] = grayScale; //R
          }
        }

        //unlock the bitmaps
        newBitmap.UnlockBits(newData);
        original.UnlockBits(originalData);

        return newBitmap;
      }
    }

    public static Bitmap ApplyInvert(Bitmap original)
    {
      var newBitmap = new Bitmap(original);

      Color pixelColor;

      for (int y = 0; y < newBitmap.Height; y++)
      {
        for (int x = 0; x < newBitmap.Width; x++)
        {
          pixelColor = newBitmap.GetPixel(x, y);
          var a = pixelColor.A;
          var r = (byte)(255 - pixelColor.R);
          var g = (byte)(255 - pixelColor.G);
          var b = (byte)(255 - pixelColor.B);
          newBitmap.SetPixel(x, y, Color.FromArgb(a, r, g, b));
        }
      }

      return newBitmap;
    }

    public static Bitmap GetNamePrint()
    {
      var size = new Size(160, 412);

      var bmp = new Bitmap(size.Width, size.Height);
      var graphic = Graphics.FromImage(bmp);
      graphic.CopyFromScreen(new Point(522, 469), Point.Empty, size);

      return bmp;
    }

    private Bitmap GetScorePrint(Point start, Size size)
    {
      start = new Point(start.X, start.Y);

      var bmp = new Bitmap(size.Width, size.Height);
      var graphic = Graphics.FromImage(bmp);
      graphic.DrawImageUnscaledAndClipped(Bmp, new Rectangle(start.X, -start.Y, Bmp.Width, Bmp.Height));

      var original = bmp;
      var scaled = new Bitmap(original.Width * 4, original.Height * 4);
      using (Graphics graphics = Graphics.FromImage(scaled))
      {
        graphics.DrawImage(original, new Rectangle(0, 0, scaled.Width, scaled.Height));
      }

      return scaled;
    }

    private static int LevenshteinDistance(string one, string two)
    {
      var m = one.Length;
      var n = two.Length;

      var d = new int[m, n];

      for (int i = 0; i < m; i++)
      {
        d[i, 0] = i;
      }

      for (int j = 0; j < n; j++)
      {
        d[0, j] = j;
      }

      for (int j = 1; j < n; j++)
      {
        for (int i = 1; i < m; i++)
        {
          if (one[i] == two[j])
          {
            d[i, j] = d[i - 1, j - 1];
          }
          else
          {
            var deletion = d[i - 1, j] + 1;
            var insertion = d[i, j - 1] + 1;
            var subsitution = d[i - 1, j - 1] + 1;

            d[i, j] = Math.Min(Math.Min(deletion, insertion), subsitution);
          }
        }
      }

      return d[m - 1, n - 1];
    }

    public void Go(int top, int height)
    {
      var bmp = GetBmp(top - 99);
      var line = Processor.Apply(bmp);
      var output = Compute(line);

      var text = File.ReadAllText("Options");
      text = text.Replace("\\t", "\t");
      text = text.Replace("\\n", "\n");
      text = text.Replace("{U}", output[0]);
      text = text.Replace("{I}", output[1]);
      text = text.Replace("{SQ}", output[2]);
      text = text.Replace("{Win}", Win());
      text = text.Replace("{T}", Time());
      text = text.Replace("{W}", Workers(top - 99));
      text = text.Replace("{R}", Race(top - 99));
      text = text.Replace("{ER}", Race(54 - top + 99));
      text = text.Replace("{D}", DateTime.Today.Date.ToShortDateString());

      if (text.Contains("{EL}"))
      {
        var data = new DataEntry(1);
        data.ShowDialog();

        var league = data.Output;
        league = League(league);
        text = text.Replace("{EL}", league);
      }
      if (text.Contains("{EN}"))
      {
        var data = new DataEntry(40);
        data.ShowDialog();

        var enemyName = data.Output;
        text = text.Replace("{EN}", enemyName);
      }

      text += "\n";

      Clipboard.SetText(Clipboard.GetText() + text);
    }

    private static string League(string league)
    {
      league = league.ToUpper();
      if (league == "Q") return "Grandmaster";
      if (league == "M") return "Master";
      if (league == "D") return "Diamond";
      if (league == "P") return "Platinum";
      if (league == "G") return "Gold";
      if (league == "S") return "Silver";
      if (league == "B") return "Bronze";
      return "NA";
    }

    private string Time()
    {
      var size = new Size(1000, 30);
      var bmp = new Bitmap(size.Width, size.Height);
      var g = Graphics.FromImage(bmp);
      g.CopyFromScreen(327, 327, 0, 0, size);
      var line = Processor.Apply(bmp);
      var words = line.Split(' ').ToList();

      for (int i = words.Count - 1; i >= 0; i--)
      {
        var word = words[i];
        word = word.Replace("\n", "");
        if (word.Length < 1) continue;
        var testValue = Convert.ToInt32(word);
        if (testValue < 60) return testValue.ToString();
      }

      return "Err";
    }

    private string Win()
    {
      var size = new Size(224, 31);
      var bmp = new Bitmap(size.Width, size.Height);
      var g = Graphics.FromImage(bmp);
      g.CopyFromScreen(313, 271, 0, 0, size);
      var win = Processor.Apply(bmp);
      win = win == "540100879\n\n" ? "Win" : "Loss";
      return win;
    }

    private string Workers(int y)
    {
      var size = new Size(175, 50);
      var bmp = new Bitmap(size.Width, size.Height);
      var g = Graphics.FromImage(bmp);
      g.CopyFromScreen(791+350, y+ 467, 0, 0, size);
      bmp = MakeGrayscale2(bmp);
      bmp = ApplyInvert(bmp);
      bmp = AdjustContrast(bmp, 50);
      bmp = AdjustImage(bmp, .1f, 1.9f);
      var workers = Processor.Apply(bmp).Replace("\n", "");
      return workers;
    }

    private static string Race(int y)
    {
      var size = new Size(35, 35);
      var bmp = new Bitmap(size.Width, size.Height);
      var g = Graphics.FromImage(bmp);
      g.CopyFromScreen(487, y + 467, 0, 0, size);

      var colors = new List<Color>();
      for (var i = 0; i < 35; i++)
      {
        colors.Add(bmp.GetPixel(i, i));
      }

      var code = string.Empty;
      foreach (var color in colors)
      {
        code += string.Format("({0},{1},{2}),", color.R, color.G, color.B);
      }
      code = code.Remove(code.Length - 1);

      if (code == Properties.Resources.Zerg)
        return "Zerg";
      if (code == Properties.Resources.Protoss)
        return "Protoss";
      if (code == Properties.Resources.Terran)
        return "Terran";
      return FuzzyRaceSearch(code);
    }

    private static string FuzzyRaceSearch(string code)
    {
      var zergDistance = LevenshteinDistance(code, Properties.Resources.Zerg);
      var terranDistance = LevenshteinDistance(code, Properties.Resources.Terran);
      var protossDistance = LevenshteinDistance(code, Properties.Resources.Protoss);

      var dictionary = new Dictionary<string, int>
        {
          { "Zerg", zergDistance },
          { "Terran", terranDistance },
          { "Protoss", protossDistance }
        };

      return dictionary.OrderBy(x => x.Value).First().Key;
    }

    /// <summary>
    /// method for comparing 2 images to see if they are the same. First
    /// we convert both images to a byte array, we then get their hash (their
    /// hash should match if the images are the same), we then loop through
    /// each item in the hash comparing with the 2nd Bitmap
    /// </summary>
    /// <param name="bmp1"></param>
    /// <param name="bmp2"></param>
    /// <returns></returns>
    public bool DoImagesMatch(ref Bitmap bmp1, ref Bitmap bmp2)
    {
      try
      {
        //create instance or System.Drawing.ImageConverter to convert
        //each image to a byte array
        var converter = new ImageConverter();
        //create 2 byte arrays, one for each image
        var imgBytes2 = new byte[1];

        //convert images to byte array
        var imgBytes1 = (byte[])converter.ConvertTo(bmp1, imgBytes2.GetType());
        imgBytes2 = (byte[])converter.ConvertTo(bmp2, imgBytes1.GetType());

        //now compute a hash for each image from the byte arrays
        var sha = new SHA256Managed();
        byte[] imgHash1 = sha.ComputeHash(imgBytes1);
        byte[] imgHash2 = sha.ComputeHash(imgBytes2);

        //now let's compare the hashes
        for (int i = 0; i < imgHash1.Length && i < imgHash2.Length; i++)
        {
          //whoops, found a non-match, exit the loop
          //with a false value
          if (imgHash1[i] != imgHash2[i])
            return false;
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        return false;
      }
      //we made it this far so the images must match
      return true;
    }

    public TesseractProcessor Processor
    {
      get
      {
        if (_processor != null) return _processor;

        var processor = new TesseractProcessor();
        processor.SetVariable("tessedit_char_whitelist", "0123456789,");
        processor.Init(null, "eng", 0);
        _processor = processor;
        return processor;
      }
    }

    private static string[] Compute(string or)
    {
      try
      {
        var words = or.Split(' ');
        var u = Extract(words[0].Replace("\n", ""));
        var i = Extract(words[1].Replace("\n", ""));
        var sq = Compute(u, i);

        return new[] { u.ToString(), i.ToString() , sq.ToString("###")};
      }
      catch
      {
        return new[] { string.Empty, string.Empty, string.Empty };
      }
    }

    private static int Extract(string word)
    {
      int j = -1;
      var word2 = word;
      for (int i = word.Length - 1; i >= 0; i--)
      {
        j++;
        if (j != 3) continue;
        word2 = word2.Remove(i, 1);
      }

      try
      {
        return Convert.ToInt32(word2);
      }
      catch (Exception)
      {
        return 0;
      }
    }

    private static double Compute(double u, double i)
    {
      u = u > 0 ? u : 1;
      return 35 * (0.00137 * i - Math.Log(u)) + 240;
    }
  }
}
