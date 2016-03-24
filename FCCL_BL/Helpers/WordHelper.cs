using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FCCL_DAL;
using Novacode;

namespace FCCL_BL.Helpers
{
    public static class WordHelper
    {
        public static void GenerateReceptionRegister(List<sp_Get_RegistruRegeptie_Result> lines, string fileName, DateTime reportDate)
        {
            using (var output = File.Open(fileName, FileMode.Create))
            {
                using (var document = DocX.Create(output))
                {
                    Paragraph title = document.InsertParagraph();
                    title.AppendLine("REGISTRU DE RECEPŢIE A PROBELOR DE LAPTE").FontSize(15).Bold().Alignment = Alignment.center;
					title = document.InsertParagraph();
                    title.AppendLine("Data:" + reportDate.ToString("dd.MM.yyyy") + "          ").FontSize(15).Bold().Alignment = Alignment.right;
                    var t = document.AddTable(lines.Count + 2, 5);
                    //Set header
                    t.Rows[0].Cells[0].Paragraphs.First().Append("Seria PV/Data").Bold().Alignment = Alignment.center;
                    t.Rows[0].Cells[1].Paragraphs.First().Append("Nume/Cod Prelevator").Bold().Alignment = Alignment.center;
                    t.Rows[0].Cells[2].Paragraphs.First().Append("Nume proba").Bold().Alignment = Alignment.center;
                    t.Rows[0].Cells[3].Paragraphs.First().Append("Cod identificare probă").Bold().Alignment = Alignment.center;
                    t.Rows[0].Cells[4].Paragraphs.First().Append("Tipul de analiză").Bold().Alignment = Alignment.center;
                    t.Rows[1].Cells[0].Paragraphs.First().Append("Cerere analiză").Bold().Alignment = Alignment.center;
                    t.Rows[1].Cells[4].Paragraphs.First().Append("a),b),c),d),e)*").Bold().Alignment = Alignment.center;

                    int start = 2;
                    int end = 2;
                    //Add content to this Table.
                    foreach (var commandNrGroup in lines.GroupBy(x => x.NrComanda).ToList())
                    {
                        start = end;
                        end = start + commandNrGroup.Count();
                        t.Rows[start].Cells[0].VerticalAlignment = VerticalAlignment.Center;
                        t.Rows[start].Cells[0].Paragraphs.First().Append(commandNrGroup.Key).Alignment = Alignment.center;
                        if (commandNrGroup.Count() > 1)
                            t.MergeCellsInColumn(0, start, end - 1);
                        foreach (var codGroup in commandNrGroup.GroupBy(x => x.NumeCodPrelevator))
                        {
                            t.Rows[start].Cells[1].VerticalAlignment = VerticalAlignment.Center;
                            t.Rows[start].Cells[1].Paragraphs.First().Append(codGroup.Key).Alignment = Alignment.center;
                            if (codGroup.Count() > 1)
                                t.MergeCellsInColumn(1, start, start + codGroup.Count() - 1);
                            foreach (var probeGroup in codGroup.GroupBy(x => x.NumeProba))
                            {
                                t.Rows[start].Cells[2].VerticalAlignment = VerticalAlignment.Center;
                                t.Rows[start].Cells[2].Paragraphs.First().Append(probeGroup.Key).Alignment = Alignment.center;
                                if (probeGroup.Count() > 1)
                                    t.MergeCellsInColumn(2, start, start + probeGroup.Count() - 1);

                                foreach (var elem in probeGroup)
                                {
                                    t.Rows[start].Cells[3].Paragraphs.First().Append(elem.CodProba).Alignment = Alignment.center;
                                    t.Rows[start].Cells[4].Paragraphs.First().Append(elem.TipAnaliza).Alignment = Alignment.center;

                                    start++;
                                }
                            }
                        }
                    }
                    // Insert the Table into the document.
                    t.Alignment = Alignment.center;
                    t.Design = TableDesign.TableGrid;
                    document.InsertTable(t);
                    

                    document.InsertParagraph().AppendLine(@"
    a) determinarea numărului total de germeni,
    b) determinarea numărului de celule somatice,
    c) determinare proprietăţi fizico-chimice,
    d) determinarea inhibitorilor,
    e) determinarea punctului de congelare.
    În urma verificării calitative şi cantitative efectuate la produsul de încercat/analizat, s-au constatat următoarele:
    Este acceptat un număr de ________ probe.
    Un număr de _______ probe nu corespund din punct de vedere cantitativ / calitativ.
    După încercare produsul este eliminat ca deşeu.").FontSize(12).Alignment = Alignment.left;

                    document.InsertParagraph().Append("Semnătura de primire\n").Alignment = Alignment.right;

					document.InsertParagraph().Append("Cod: FG – L – 12 – 02   Ediţia: 1/11.05.2015    Revizia: 1/11.05.2015").FontSize(8).Alignment = Alignment.left;
                    document.Save();
                }
            }
        }
    }
}
