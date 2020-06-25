using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using ZedGraph;
using System.Drawing;

namespace Geophysic_task
{
    public class MagRecord
    {
        public double Data;
        public DateTime DT;
        public int Profile;
        public int Picket;

        public MagRecord (double data, DateTime dt, int pr, int pk)
        {
            Data = data;
            DT = dt;
            Profile = pr;
            Picket = pk;
        }//конструктор
    }//запись файла

    public class MagFile
    {
        public int NumOfStrings = 8;//количество строк в файле
        public string[] Header;//массив строк в заголовке
        public List<MagRecord> magRecordList = new List<MagRecord>();//количество элементов

        public MagFile()
        {
        }
        public MagFile(string fileName, int formatKey)
        {
            FileStream fs;
            StreamReader sr;
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                sr = new StreamReader(fs);
            }
            catch
            {
                throw new Exception("Не удалось открыть файл.");
            }//ловим ошибку

            Header = new string[NumOfStrings];
            for (int i = 0; i < NumOfStrings; i++)
            {
                Header[i] = sr.ReadLine();
            }//считыватель заголовка

            while (!sr.EndOfStream)
            {
                string buf = sr.ReadLine();
                string[] bufarray = buf.Split(' ', '\t');
                try
                {
                    switch (formatKey)
                    {
                        case 1:
                            magRecordList.Add(new MagRecord(double.Parse(bufarray[0], CultureInfo.GetCultureInfo("en-US")), DateTime.Parse(bufarray[4]), 0, 0));
                            break;

                        case 2: magRecordList.Add(new MagRecord(double.Parse(bufarray[0], CultureInfo.GetCultureInfo("en-US")), DateTime.Parse(bufarray[2]), int.Parse(bufarray[3]), int.Parse(bufarray[4]))); break;
                    }
                }

                catch
                {
                    throw new Exception("Ошибка в строке: " + (NumOfStrings) + magRecordList.Count.ToString());
                }//проверка ошибки

                
            }//cчитыватель текста
            sr.Close();
            fs.Close();
        }//конструктор

        public void DrawData(ZedGraphControl ZGC)
        {
            GraphPane pain = ZGC.GraphPane;
            PointPairList list = new PointPairList();
            foreach (MagRecord el in magRecordList)
            {
                XDate XD = new XDate(el.DT);
                list.Add(XD, el.Data);
            }

            Color curveColor = Color.Purple;
            LineItem myCurve = pain.AddCurve("nT(t)", list, curveColor, SymbolType.None);
            pain.XAxis.Title.Text = "Time";
            pain.XAxis.Type = AxisType.Date;
            pain.YAxis.Title.Text = "nT";
            myCurve.Line.IsSmooth = true;
            ZGC.AxisChange();
            ZGC.Invalidate();
        }

        public static MagFile Variations (MagFile Basa, double VarLev)
        {
            MagRecord buf;
            MagFile res = new MagFile();
            if ((this.magRecordList[0].DT > Basa.magRecordList[0].DT) && (this.magRecordList[this.magRecordList.Count - 1].DT < Basa.magRecordList[Basa.magRecordList.Count - 1].DT))
            {
                foreach (MagRecord lr in this.magRecordList)
                {
                    int i = 0;
                    MagRecord lb;
                    while (((lb = Basa.magRecordList[i]).DT < lr.DT) && (i < Basa.magRecordList.Count))
                    {
                        i++;
                    }
                    lr.Data = lr.Data - VarLev + lb.Data - VarLev;
                    res.magRecordList.Add(lr);
                }
            }
            else throw new Exception("Ну пизда короч");
            return res;
        }
    }//запись заголовка


}
