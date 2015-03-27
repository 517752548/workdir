using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //message class1,class 2
            //args 
            //dir:../Net file:const.proto saveto:../src/NetSources/GameConst.cs
            var root = string.Empty;
            var file = string.Empty;
            var fileSave = string.Empty;
            foreach (var i in args)
            {
                if (i.StartsWith("dir:"))
                {
                    root = i.Replace("dir:", "");
                }

                if (i.StartsWith("file:"))
                {
                    file = i.Replace("file:", "");
                }

                if (i.StartsWith("saveto:"))
                {
                    fileSave = i.Replace("saveto:", "");
                }
            }

            Console.WriteLine(string.Format("dir:{0} file:{1} saveto:{2}", root, file, fileSave));
            var sb = new StringBuilder();
            var path = Path.Combine(root, file);
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    line = line.Trim();
                    //message class1 class2
                    var strs = line.Split(" ".ToArray());
                    if(strs.Length!=3)
                    {
                        Console.WriteLine("Error:" + line);
                    }
                    else
                    {
                        var code = MessageTemplate.Replace("[Name]", strs[0])
                            .Replace("[Request]", strs[1]).Replace("[Response]", strs[2]);
                        sb.AppendLine(code);
                    }
                }
            }
            var result = FileTemplate.Replace("[CLASSES]", sb.ToString());
            File.WriteAllText(Path.Combine(root, fileSave), result);
        }

        public static string FileTemplate = @"using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Proto;

namespace PNet
{
[CLASSES]
}";

        public static string MessageTemplate = @"
    [NetMessage]
    public class [Name] : NetMessage<[Request], [Response]> { }";
    }
}
