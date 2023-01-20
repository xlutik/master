using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace F02xmlToText
{
    public static class Transform
    {
        public static StreamWriter NewFileName(string FileName = null)
        {
            if (FileName == null)
            {
                FileName = Path.GetRandomFileName();
                Console.WriteLine(string.Format("New FileName = {0}", FileName));
            }
            return new StreamWriter(FileName, false);
        }
        public static DataSet LoadFileData(string FileName)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(FileName);
            return ds;
        }
        public static void WriteHeader(StreamWriter fstream, DataSet ds)
        {
            fstream.WriteLine("");
            DataRow row = ds.Tables["HEAD"].Rows[0];
            fstream.WriteLine(string.Format("00={0}=________=________={0}=____={1}=__={2}", row["REPORTDATE"].ToString().Replace(".", ""), row["EDRPOU"].ToString(), ds.Tables["DATA"].Rows.Count ));
            fstream.WriteLine("#02=");
        }
        public static void TransformData(StreamWriter fstream, DataSet ds, bool addUnionAll = false)
        {
            Console.WriteLine(string.Format("В файле     : {0} записей с данными.", ds.Tables["DATA"].Rows.Count));
            
            foreach (DataRow row in ds.Tables["DATA"].Rows)
            {
                string AP1 = row["T020"].ToString().Trim();
                string NE = (row["R030"].ToString() == "980") ?"0":"1";
                string BBBB = row["R020"].ToString();
                string VVV = row["R030"].ToString();
                string SumNom = row["T071"].ToString().Replace(".0", "").Trim();
                string SumEkv = row["T070"].ToString().Trim();

                string RetValue = string.Empty;
                if (NE == "1")
                {
                    if (SumNom != "0")
                    {
                        RetValue = string.Format("{0}{1}{2}{3}1={4}", AP1, NE, BBBB, VVV, SumNom);
                        if (addUnionAll)
                            RetValue = string.Format("SELECT '{0}'           UNION ALL", RetValue);
                        fstream.WriteLine(RetValue);
                    }
                    if (SumEkv != "0")
                    {
                        RetValue = string.Format("{0}{1}{2}{3}1={4}", AP1, "0", BBBB, VVV, SumEkv);
                        if (addUnionAll)
                            RetValue = string.Format("SELECT '{0}'           UNION ALL", RetValue);
                        fstream.WriteLine(RetValue);
                    }
                }
                else
                {
                    if (SumEkv != "0")
                    {
                        RetValue = string.Format("{0}{1}{2}{3}1={4}", AP1, "0", BBBB, VVV, SumEkv);
                        if (addUnionAll)
                            RetValue = string.Format("SELECT '{0}'           UNION ALL", RetValue);
                        fstream.WriteLine(RetValue);
                    }
                }
            }

        }
        public static void CloseFile(StreamWriter writer)
        {
            writer.Close();
        }


    }
}
