using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Put .ttf file in the programe folder");
            while (true)
            {
                Console.WriteLine("Press 1 to create .txt from .ttf");
                Console.WriteLine("Press 2 to check .ttf with .txt,find out what is missing in the .ttf");
                Console.WriteLine("Press 3 to compare .ttf with .ttf,find out what is the differet");
                string line = Console.ReadLine();
                if (line == "1")
                {
                    CreateTxt();
                    break;
                }
                else if (line == "2")
                {
                    CheckTTFwithTxt();
                    break;
                }
                else if (line == "3")
                {
                    compareTTF();
                    break;
                }
                else Console.WriteLine("Invalid input.");
            }
            Console.ReadLine();
        }
        static void CreateTxt() {

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            List<string> ttfFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (Path.GetExtension(files[i]) == ".ttf")
                    ttfFiles.Add(files[i]);
            if (ttfFiles.Count == 0)
            {
                Console.WriteLine("Error: No .ttf file found");
                Console.ReadLine();
                return;
            }
            else
            {
                Console.WriteLine(ttfFiles.Count + " files found");
            }
            for (int i = 0; i < ttfFiles.Count; i++)
            {
                string s = "";
                var families = Fonts.GetFontFamilies(ttfFiles[i]);

                foreach (FontFamily family in families)
                {

                    var typefaces = family.GetTypefaces();
                    foreach (Typeface typeface in typefaces)
                    {
                        GlyphTypeface glyph;

                        typeface.TryGetGlyphTypeface(out glyph);

                        IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;



                        foreach (KeyValuePair<int, ushort> kvp in characterMap)
                        {
                            s += char.ConvertFromUtf32(kvp.Key);
                            //   Console.WriteLine(String.Format("{0}:{1}", (char.ConvertFromUtf32(kvp.Key)), kvp.Value));
                        }


                    }
                }
                string path = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(ttfFiles[i]) ;

                int number = 0;
                while (true)
                {
                    string p = path + ((number == 0) ? "" : number.ToString()) + ".txt";
                    if (!File.Exists(p))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(p))
                        {
                            sw.WriteLine(s);
                            Console.WriteLine("Txt file created: " + p);
                        }
                        break;
                    }
                    else
                    {
                        number++;
                    }
                }
            }
        }
        /// <summary>
        ///check what is missing in current ttf 
        /// </summary>
        static void CheckTTFwithTxt() {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            List<string> ttfFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (Path.GetExtension(files[i]) == ".ttf")
                    ttfFiles.Add(files[i]);
            if (ttfFiles.Count == 0)
            {
                Console.WriteLine("Error: No .ttf file found");
                Console.ReadLine();
                return;
            }
            List<string> txtFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (Path.GetExtension(files[i]) == ".txt")
                    txtFiles.Add(files[i]);
            if (txtFiles.Count == 0)
            {
                Console.WriteLine("Error: No .txt file found");
                Console.ReadLine();
                return;
            }
            for (int x = 0; x < txtFiles.Count; x++)
            {
                string text = System.IO.File.ReadAllText(txtFiles[x]);
                char[] c = text.ToCharArray();
                List<string> allString = new List<string>();
                for (int i = 0; i < c.Length; i++)
                    allString.Add(c[i].ToString());
               
                for (int i = 0; i < ttfFiles.Count; i++)
                {
                    List<string> existString = new List<string>();
                    var families = Fonts.GetFontFamilies(ttfFiles[i]);

                    foreach (FontFamily family in families)
                    {

                        var typefaces = family.GetTypefaces();
                        foreach (Typeface typeface in typefaces)
                        {
                            GlyphTypeface glyph;
                            typeface.TryGetGlyphTypeface(out glyph);
                            IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;
                            foreach (KeyValuePair<int, ushort> kvp in characterMap)
                            {
                                string _s= char.ConvertFromUtf32(kvp.Key);
                                existString.Add(_s);
                            }
                        }
                    }
                    string s = "";
                    for (int y = 0; y < allString.Count; y++)
                    {
                        if (!existString.Contains(allString[y]))
                            s += allString[y];
                    }

                    string path = Directory.GetCurrentDirectory() + "\\" + Path.GetFileNameWithoutExtension(ttfFiles[i]) + " checked by " + Path.GetFileNameWithoutExtension(txtFiles[x]) ;

                    int number = 0;
                    while (true)
                    {
                        string p = path + ((number == 0) ? "" : number.ToString()) + ".txt";
                        if (!File.Exists(p))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(p))
                            {
                                sw.WriteLine(s);
                                Console.WriteLine("Txt file created: " + p);
                            }
                            break;
                        }
                        else
                        {
                            number++;
                        }
                    }
                }
                
            }
        }

        static void compareTTF() {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            List<string> ttfFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (Path.GetExtension(files[i]) == ".ttf")
                    ttfFiles.Add(files[i]);
            if (ttfFiles.Count <=1)
            {
                Console.WriteLine("Error: need at least 2 .ttf files");
                Console.ReadLine();
                return;
            }
            Dictionary<string, List<string>> ttfStrings = new Dictionary<string, List<string>>();
            for (int i = 0; i < ttfFiles.Count; i++)
            {
                ttfStrings.Add(ttfFiles[i], new List<string>());
                var families = Fonts.GetFontFamilies(ttfFiles[i]);

                foreach (FontFamily family in families)
                {

                    var typefaces = family.GetTypefaces();
                    foreach (Typeface typeface in typefaces)
                    {
                        GlyphTypeface glyph;
                        typeface.TryGetGlyphTypeface(out glyph);
                        IDictionary<int, ushort> characterMap = glyph.CharacterToGlyphMap;
                        foreach (KeyValuePair<int, ushort> kvp in characterMap)
                        {
                            string _s = char.ConvertFromUtf32(kvp.Key);
                            ttfStrings[ttfFiles[i]].Add(_s);
                        }
                    }
                }

               
            }
            for (int i = 0; i < ttfFiles.Count; i++)
            {
                for (int n = i+1; n < ttfFiles.Count; n++)
                {
                    string s1 = "";
                    string s2 = "";
                    string path1 = Directory.GetCurrentDirectory() + "\\" + "glyph missing in "+Path.GetFileNameWithoutExtension(ttfFiles[i]) + " compare to " + Path.GetFileNameWithoutExtension(ttfFiles[n]) ;
                    string path2 = Directory.GetCurrentDirectory() + "\\" + "glyph missing in " + Path.GetFileNameWithoutExtension(ttfFiles[n]) + " compare to " + Path.GetFileNameWithoutExtension(ttfFiles[i]) ;
                    for (int x = 0; x < ttfStrings[ttfFiles[n]].Count; x++)
                    {
                        if (!ttfStrings[ttfFiles[i]].Contains(ttfStrings[ttfFiles[n]][x]))
                        {
                            s1 += ttfStrings[ttfFiles[n]][x];
                        }
                    }
                    for (int x = 0; x < ttfStrings[ttfFiles[i]].Count; x++)
                    {
                        if (!ttfStrings[ttfFiles[n]].Contains(ttfStrings[ttfFiles[i]][x]))
                        {
                            s2 += ttfStrings[ttfFiles[i]][x];
                        }
                    }
                    int number = 0;
                    while (true)
                    {
                        string p = path1 + ((number == 0) ? "" : number.ToString())+".txt";
                        if (!File.Exists(p))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(p))
                            {
                                sw.WriteLine(s1);
                                Console.WriteLine("Txt file created: " + p);
                            }
                            break;
                        }
                        else
                        {
                            number++;
                        }
                    }
                    number = 0;
                    while (true)
                    {
                        string p = path2 + ((number == 0) ? "" : number.ToString()) + ".txt";
                        if (!File.Exists(p))
                        {
                            // Create a file to write to.
                            using (StreamWriter sw = File.CreateText(p))
                            {
                                sw.WriteLine(s2);
                                Console.WriteLine("Txt file created: " + p);
                            }
                            break;
                        }
                        else
                        {
                            number++;
                        }
                    }


                }
            }
          
        }
    }
}
