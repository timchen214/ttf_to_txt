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
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(true);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            List<string> ttfFiles = new List<string>();
            for (int i = 0; i < files.Length; i++)
                if (Path.GetExtension(files[i]) == ".ttf")
                    ttfFiles.Add(files[i]);
            if (ttfFiles.Count == 0)
            {
                Console.WriteLine("No .ttf file found");
                Console.ReadLine();
                return;
            }
            else {
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
                string path = Directory.GetCurrentDirectory()+"\\" +Path.GetFileNameWithoutExtension(ttfFiles[i])+".txt";
                
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(s);
                    }
                }
                Console.WriteLine("Txt file created: " + path);
            }

            Console.ReadLine();
        }
    }
}
