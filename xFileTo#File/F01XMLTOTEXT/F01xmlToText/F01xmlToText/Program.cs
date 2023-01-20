using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace F01xmlToText
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Transform File #01 from XML To Text");
            if (args.Count<string>() == 0 || args.Count<string>() == 1)
            {
                Console.WriteLine("F01xmlToText <InputFileName> <OutputFileName> [<ADDUNION>]");
                return;
            }


            string FileInput = null;
            string FileOutput = null;
            bool addUnionAll = false; 
            if (args.Count<string>() > 0 &&  (args[0] != ""))  FileInput = args[0];
            if (args.Count<string>() > 1 && (args[1] != "")) FileOutput = args[1];
            if (args.Count<string>() > 2)
            {
                string S = args[2]; S.ToUpper();
                if (S == "ADDUNION")
                    addUnionAll = true;
            } 

            Console.WriteLine(string.Format(" Input File : {0}", FileInput ));
            Console.WriteLine(string.Format("Output File : {0}", FileOutput ));

            //Console.ReadLine();
            StreamWriter wo = Transform.NewFileName(FileOutput);
            DataSet ds = Transform.LoadFileData(FileInput);
            Transform.WriteHeader(wo, ds);
            Transform.TransformData(wo, ds, addUnionAll);
            Transform.CloseFile(wo);
        }
    }
}
