using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

/// <summary>
/// Summary description for SampleAnalize
/// </summary>
public class SampleAnalize
{
    public string Nrcontrol { get; set; }

    public int INrcontrol { get; set; }

    public DateTime Datatestare { get; set; }

    public double Cantitate { get; set; }

    public double Grasime { get; set; }

    public double Proteine { get; set; }

    public double Lactoza { get; set; }

    public double Caseina { get; set; }

    public double Ph { get; set; }

    public double Solide { get; set; }

    public double Urea { get; set; }

    public double Ncs { get; set; }

    public double Val { get; set; }

    public double GrasProt { get; set; }

    public static List<SampleAnalize> GetSampleAnalize(int fermaid, string crot)
    {
        int curryear = DateTime.Now.Year;

        List<SampleAnalize> samples = new List<SampleAnalize>();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        string querycrot = "";
        if (crot.Length > 0)
        {
            querycrot = " and CodFerma='" + crot + "' ";
        }
        cmd.CommandText =
            "SELECT NrComanda, DataTestare,CantitateLaPrelevare,Grasime,ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PH,Urea,NumarCeluleSomatice from MostreTancuri Where FermaId =" +
            fermaid + querycrot
            + " and Year(DataTestare) =" + curryear
            + " Order By DataTestare";
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        if (table.Rows != null && table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];

            string refcontrol = row["NrComanda"].ToString();
            string nrcontrol = row["NrComanda"].ToString();
            DateTime datatestare = DateTime.Parse(row["DataTestare"].ToString());


            int nrrows = table.Rows.Count;
            int count = 0;
            double cant = 0;
            double[] summ = new double[9];
            for (int i = 0; i < 9; i++)
                summ[i] = 0.00;
            string nrc = "C1";
            while (count < nrrows)
            {
                SampleAnalize sample = new SampleAnalize();

                if (refcontrol.StartsWith("C"))
                {
                    int cratima = -1;
                    if (refcontrol.IndexOf("-") > 0)
                        cratima = refcontrol.IndexOf("-");
                    if (cratima > 2)
                        nrc = refcontrol.Substring(0, cratima);
                    else
                        nrc = refcontrol.Substring(0, 2);
                }
                sample.Datatestare = datatestare;
                sample.Nrcontrol = nrc;
                sample.INrcontrol = Convert.ToInt32(nrc.Substring(1));
                row = table.Rows[count];
                datatestare = DateTime.Parse(row["DataTestare"].ToString());
                nrcontrol = row["NrComanda"].ToString();

                while (count < nrrows && nrcontrol.Equals(refcontrol))
                {
                    // create avg parameter 
                    double litri = 0.00;
                    try
                    {
                        litri = Convert.ToDouble(row["CantitateLaPrelevare"], CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        litri = 0.00;
                    }
                    cant += litri;
                    for (int i = 0; i < 9; i++)
                    {
                        double sum = 0;
                        try
                        {
                            if (i < 8)
                                sum = Convert.ToDouble(row[3 + i], CultureInfo.InvariantCulture);
                            else
                            {
                                double gras = Convert.ToDouble(row["Grasime"], CultureInfo.InvariantCulture);
                                double prot = Convert.ToDouble(row["ProcentProteine"], CultureInfo.InvariantCulture);
                                if (prot > 0)
                                    sum = gras / prot;
                            }
                        }
                        catch
                        {
                            sum = 0.00;
                        }
                        summ[i] += sum * litri;
                    }

                    count++;
                    if (count < nrrows)
                    {
                        row = table.Rows[count];
                        datatestare = DateTime.Parse(row["DataTestare"].ToString());
                        nrcontrol = row["NrComanda"].ToString();
                    }
                }

                sample.Cantitate = cant;
                if (cant > 0)
                {
                    sample.Grasime = summ[0] / cant;
                    sample.Proteine = summ[1] / cant;
                    sample.Caseina = summ[2] / cant;
                    sample.Lactoza = summ[3] / cant;
                    sample.Solide = summ[4] / cant;
                    sample.Ph = summ[5] / cant;
                    sample.Urea = summ[6] / cant;
                    sample.Ncs = summ[7] / cant;
                    sample.GrasProt = summ[8] / cant;
                }
                samples.Add(sample);
                // add to list
                refcontrol = nrcontrol;
                cant = 0;
                for (int i = 0; i < 9; i++)
                    summ[i] = 0.00;
            } // while rows
        } //if data exists
        return samples;
    }

    public static List<SampleAnalize> GetSampleAnalize(int fermaid, string crot, int curryear)
    {
        // int curryear = DateTime.Now.Year;

        List<SampleAnalize> samples = new List<SampleAnalize>();
        SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        string querycrot = "";
        if (crot.Length > 0)
            querycrot = " and CodFerma='" + crot + "' ";
        cmd.CommandText =
            "SELECT NrComanda, DataTestare,CantitateLaPrelevare,Grasime,ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PH,Urea,NumarCeluleSomatice from MostreTancuri Where FermaId =" +
            fermaid + querycrot
            + " and Year(DataTestare) =" + curryear
            + " Order By DataTestare";
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        if (table.Rows != null && table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];

            string refcontrol = row["NrComanda"].ToString();
            string nrcontrol = row["NrComanda"].ToString();
            DateTime datatestare = DateTime.Parse(row["DataTestare"].ToString());


            int nrrows = table.Rows.Count;
            int count = 0;
            double cant = 0;
            double[] summ = new double[9];
            for (int i = 0; i < 9; i++)
                summ[i] = 0.00;
            string nrc = "C1";
            if (curryear == 2012)
                nrc = "C3";
            while (count < nrrows)
            {
                SampleAnalize sample = new SampleAnalize();

                if (refcontrol.StartsWith("C"))
                {
                    int cratima = -1;
                    if (refcontrol.IndexOf("-") > 0)
                        cratima = refcontrol.IndexOf("-");
                    if (cratima > 2)
                        nrc = refcontrol.Substring(0, cratima);
                    else
                        nrc = refcontrol.Substring(0, 2);
                }
                sample.Datatestare = datatestare;
                sample.Nrcontrol = nrc;
                sample.INrcontrol = Convert.ToInt32(nrc.Substring(1));
                row = table.Rows[count];
                datatestare = DateTime.Parse(row["DataTestare"].ToString());
                nrcontrol = row["NrComanda"].ToString();

                while (count < nrrows && nrcontrol.Equals(refcontrol))
                {
                    // create avg parameter 
                    double litri = 0.00;
                    try
                    {
                        litri = Convert.ToDouble(row["CantitateLaPrelevare"], CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        litri = 0.00;
                    }
                    cant += litri;
                    for (int i = 0; i < 9; i++)
                    {
                        double sum = 0;
                        try
                        {
                            if (i < 8)
                                sum = Convert.ToDouble(row[3 + i], CultureInfo.InvariantCulture);
                            else
                            {
                                double gras = Convert.ToDouble(row["Grasime"], CultureInfo.InvariantCulture);
                                double prot = Convert.ToDouble(row["ProcentProteine"], CultureInfo.InvariantCulture);
                                if (prot > 0)
                                    sum = gras / prot;
                            }
                        }
                        catch
                        {
                            sum = 0.00;
                        }
                        summ[i] += sum * litri;
                    }

                    count++;
                    if (count < nrrows)
                    {
                        row = table.Rows[count];
                        datatestare = DateTime.Parse(row["DataTestare"].ToString());
                        nrcontrol = row["NrComanda"].ToString();
                    }
                }

                sample.Cantitate = cant;
                if (cant > 0)
                {
                    sample.Grasime = summ[0] / cant;
                    sample.Proteine = summ[1] / cant;
                    sample.Caseina = summ[2] / cant;
                    sample.Lactoza = summ[3] / cant;
                    sample.Solide = summ[4] / cant;
                    sample.Ph = summ[5] / cant;
                    sample.Urea = summ[6] / cant;
                    sample.Ncs = summ[7] / cant;
                    sample.GrasProt = summ[8] / cant;
                }
                samples.Add(sample);
                // add to list
                refcontrol = nrcontrol;
                cant = 0;
                for (int i = 0; i < 9; i++)
                    summ[i] = 0.00;
            } // while rows
        } //if data exists
        return samples;
    }


    public static List<SampleAnalizeExt> GetSampleAnalizeExt(int fermaid)
    {
        int curryear = DateTime.Now.Year;

        List<SampleAnalizeExt> samples = new List<SampleAnalizeExt>();
        SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText =
            "SELECT NrComanda, CodFerma, CodBare, DataTestare,CantitateLaPrelevare,Grasime,ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PH,Urea,NumarCeluleSomatice from MostreTancuri Where FermaId =" +
            fermaid
            + " and Year(DataTestare) =" + curryear
            + " Order By DataTestare ASC, NrComanda ASC";
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        if (table.Rows != null && table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];

            int nrrows = table.Rows.Count;
            int count = 0;
            string nrc = "C1";
            while (count < nrrows)
            {
                row = table.Rows[count];
                SampleAnalizeExt sample = new SampleAnalizeExt();
                string nrcontrol = row["NrComanda"].ToString();
                if (nrcontrol.StartsWith("C"))
                {
                    int cratima = -1;
                    if (nrcontrol.IndexOf("-") > 0)
                        cratima = nrcontrol.IndexOf("-");
                    if (cratima > 2)
                        nrc = nrcontrol.Substring(0, cratima);
                    else
                        nrc = nrcontrol.Substring(0, 2);
                }

                sample.DataTestare = DateTime.Parse(row["DataTestare"].ToString());
                sample.Nrcontrol = nrc;
                sample.INrcontrol = Convert.ToInt32(nrc.Substring(1));
                sample.Cantitate = Convert.ToDouble(row["CantitateLaPrelevare"], CultureInfo.InvariantCulture);
                sample.Nrmatricol = row["CodFerma"].ToString();
                sample.Codbare = row["CodBare"].ToString();
                sample.Grasime = TryCast(row["Grasime"], 0.0);
                sample.Proteine = TryCast(row["ProcentProteine"], 0.0);
                sample.Caseina = TryCast(row["Caseina"], 0.0);
                sample.Lactoza = TryCast(row["ProcentLactoza"], 0.0);
                sample.Solide = TryCast(row["SubstantaUscata"], 0.0);
                sample.Ph = TryCast(row["PH"], 0.0);
                sample.Urea = TryCast(row["Urea"], 0.0);
                sample.Ncs = TryCast(row["NumarCeluleSomatice"], 0.0);
                samples.Add(sample);
                count++;
            }
        } //if data exists
        return samples;
    }


    public static List<SampleAnalizeExt> GetSampleAnalizeExt(int fermaid, int curryear)
    {
        //    int curryear = DateTime.Now.Year;

        List<SampleAnalizeExt> samples = new List<SampleAnalizeExt>();
        SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText =
            "SELECT NrComanda, CodFerma, CodBare, DataTestare,CantitateLaPrelevare,Grasime,ProcentProteine,Caseina,ProcentLactoza,SubstantaUscata,PH,Urea,NumarCeluleSomatice from MostreTancuri Where FermaId =" +
            fermaid
            + " and Year(DataTestare) =" + curryear
            + " Order By DataTestare ASC, NrComanda ASC";
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        if (table.Rows != null && table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];


            int nrrows = table.Rows.Count;
            int count = 0;
            string nrc = "C1";
            if (curryear == 2012)
                nrc = "C3";
            while (count < nrrows)
            {
                row = table.Rows[count];
                SampleAnalizeExt sample = new SampleAnalizeExt();
                string nrcontrol = row["NrComanda"].ToString();
                if (nrcontrol.StartsWith("C"))
                {
                    int cratima = -1;
                    if (nrcontrol.IndexOf("-") > 0)
                        cratima = nrcontrol.IndexOf("-");
                    if (cratima > 2)
                        nrc = nrcontrol.Substring(0, cratima);
                    else
                        nrc = nrcontrol.Substring(0, 2);
                }

                sample.DataTestare = DateTime.Parse(row["DataTestare"].ToString());
                sample.Nrcontrol = nrc;
                sample.INrcontrol = Convert.ToInt32(nrc.Substring(1));
                sample.Cantitate = Convert.ToDouble(row["CantitateLaPrelevare"], CultureInfo.InvariantCulture);
                sample.Nrmatricol = row["CodFerma"].ToString();
                sample.Codbare = row["CodBare"].ToString();
                sample.Grasime = TryCast(row["Grasime"], 0.0);
                sample.Proteine = TryCast(row["ProcentProteine"], 0.0);
                sample.Caseina = TryCast(row["Caseina"], 0.0);
                sample.Lactoza = TryCast(row["ProcentLactoza"], 0.0);
                sample.Solide = TryCast(row["SubstantaUscata"], 0.0);
                sample.Ph = TryCast(row["PH"], 0.0);
                sample.Urea = TryCast(row["Urea"], 0.0);
                sample.Ncs = TryCast(row["NumarCeluleSomatice"], 0.0);
                samples.Add(sample);
                count++;
            }
        } //if data exists
        return samples;
    }

    private static double TryCast(object valueToCast, double defaultValue)
    {
        try
        {
            return Convert.ToDouble(valueToCast, CultureInfo.InvariantCulture);
        }
        catch
        {
            return defaultValue;
        }
    }

    public static List<SampleAnalizeControl> getSampleAnalizeControl(List<SampleAnalize> samples, int curryear)
    {
        List<SampleAnalizeControl> samplescontrol = new List<SampleAnalizeControl>();
        int maxcount = 20;
        SampleAnalizeControl samplecontrol = new SampleAnalizeControl();
        if (samples.Count < maxcount)
            maxcount = samples.Count;
        samplecontrol.Nrc = maxcount;


        for (int i = 0; i < maxcount; i++)
        {
            SampleAnalize sample = samples[i];
            //      sample.Val = Math.Round(sample.Val, 2);
            string nrcontrol = sample.Nrcontrol;
            int nrc = 1;
            if (curryear == 2012)
                nrc = 3;
            if (nrcontrol.StartsWith("C"))
            {
                nrc = Convert.ToInt32(nrcontrol.Substring(1));
            }
            //   if (i <= maxcount - 1) 
            switch (nrc)
            {
                case 1:
                    samplecontrol.C1 = sample.Val;
                    break;
                case 2:
                    samplecontrol.C2 = sample.Val;
                    break;
                case 3:
                    samplecontrol.C3 = sample.Val;
                    break;
                case 4:
                    samplecontrol.C4 = sample.Val;
                    break;
                case 5:
                    samplecontrol.C5 = sample.Val;
                    break;
                case 6:
                    samplecontrol.C6 = sample.Val;
                    break;
                case 7:
                    samplecontrol.C7 = sample.Val;
                    break;
                case 8:
                    samplecontrol.C8 = sample.Val;
                    break;
                case 9:
                    samplecontrol.C9 = sample.Val;
                    break;
                case 10:
                    samplecontrol.C10 = sample.Val;
                    break;
                case 11:
                    samplecontrol.C11 = sample.Val;
                    break;
                case 12:
                    samplecontrol.C12 = sample.Val;
                    break;
                case 13:
                    samplecontrol.C13 = sample.Val;
                    break;
                case 14:
                    samplecontrol.C14 = sample.Val;
                    break;
                case 15:
                    samplecontrol.C15 = sample.Val;
                    break;
                case 16:
                    samplecontrol.C16 = sample.Val;
                    break;
                case 17:
                    samplecontrol.C17 = sample.Val;
                    break;
                case 18:
                    samplecontrol.C18 = sample.Val;
                    break;
                case 19:
                    samplecontrol.C19 = sample.Val;
                    break;
                case 20:
                    samplecontrol.C20 = sample.Val;
                    break;
                case 21:
                    samplecontrol.C21 = sample.Val;
                    break;
                case 22:
                    samplecontrol.C22 = sample.Val;
                    break;
                default:
                    break;
            }
        }
        samplescontrol.Add(samplecontrol);
        return samplescontrol;
    }

    public static List<SampleAnalizeCrotalia> getSampleAnalizeCrotalia(int fermaid, int nrpar, int curryear)
    {
        // int curryear = DateTime.Now.Year;
        List<SampleAnalizeCrotalia> samples = new List<SampleAnalizeCrotalia>();
        SqlConnection conn =
            new SqlConnection(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        //     cmd.CommandText = "SELECT  c.Crotalia,m.NrComanda, m.DataTestare,SUM(m.CantitateLaPrelevare) as Cant, SUM(m.Grasime) as Grasime,"
        //       + "SUM(m.ProcentProteine) as Proteine,SUM(m.Caseina) as Caseina,SUM(m.ProcentLactoza) as Lactoza,SUM(m.SubstantaUscata) as Solide,"
        // +"SUM(m.PH) as Ph,SUM(m.Urea) as Urea,SUM(m.NumarCeluleSomatice) as NCS FROM Crotalii c,  MostreTancuri m "  
        //+"WHERE c.FermaId =m.FermaId AND c.CodBare = m.CodFerma  AND m.Validat = 1  And c.FermaId ="+fermaid + 
        // " GROUP BY c.Crotalia,m.NrComanda,m.DataTestare  ORDER BY c.Crotalia,m.NrComanda";
        cmd.CommandText = "SELECT m.CodFerma as Crotalia,m.NrComanda, m.DataTestare,SUM(m.CantitateLaPrelevare) as Cant, "
                          +
                          " SUM(m.Grasime) as Grasime, SUM(m.ProcentProteine) as Proteine,SUM(m.Caseina) as Caseina,SUM(m.ProcentLactoza) as Lactoza,"
                          +
                          "SUM(m.SubstantaUscata) as Solide,SUM(m.PH) as Ph,SUM(m.Urea) as Urea,SUM(m.NumarCeluleSomatice) as NCS FROM MostreTancuri m "
                          + "WHERE  m.Validat = 1  And m.FermaId =" + fermaid +
                          " and Year(m.DataTestare) =" + curryear +
                          " GROUP BY m.CodFerma,m.NrComanda,m.DataTestare  ORDER BY Crotalia,m.NrComanda";
        conn.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        DataTable table = ds.Tables[0];
        if (table.Rows.Count > 0)
        {
            DataRow row = table.Rows[0];
            string nrcrot = row["Crotalia"].ToString();
            string refcrot = row["Crotalia"].ToString();
            string refcontrol = row["NrComanda"].ToString();
            string year = refcontrol.Substring(refcontrol.Length - 4);

            int nrrows = table.Rows.Count;
            int count = 0;
            int maxcount = 20;
            while (count < nrrows)
            {
                int countcrot = 0;
                SampleAnalizeCrotalia sample = new SampleAnalizeCrotalia();
                sample.Crot = refcrot;
                sample.Year = year;

                row = table.Rows[count];
                nrcrot = row["Crotalia"].ToString();
                while (count < nrrows && nrcrot.Equals(refcrot))
                {
                    //
                    if (countcrot < maxcount)
                    {
                        string nrcom = row["NrComanda"].ToString();
                        string nrcontrol = nrcom; //.Substring(0, 2);
                        int nrc = 1;
                        if (curryear == 2012)
                            nrc = 3;
                        if (nrcontrol.StartsWith("C"))
                        {
                            try
                            {
                                int cratima = -1;
                                if (nrcontrol.IndexOf("-") > 0)
                                    cratima = nrcontrol.IndexOf("-");
                                if (cratima > 2)
                                    nrc = Convert.ToInt32(nrcontrol.Substring(1, cratima - 1));
                                else
                                    nrc = Convert.ToInt32(nrcontrol.Substring(1, 1));
                            }
                            catch
                            {
                            }
                        }
                        // get cant, val par
                        double cant = 0;
                        try
                        {
                            cant = Convert.ToDouble(row["Cant"].ToString(), CultureInfo.InvariantCulture);
                        }
                        catch
                        {
                            cant = 0;
                        }
                        double val = 0;
                        try
                        {
                            if (nrpar != 9)
                                val = Convert.ToDouble(row[4 + nrpar].ToString(), CultureInfo.InvariantCulture);
                            else
                            {
                                double gras = Convert.ToDouble(row["Grasime"], CultureInfo.InvariantCulture);
                                double prot = Convert.ToDouble(row["Proteine"], CultureInfo.InvariantCulture);
                                if (prot > 0)
                                    val = gras / prot;
                            }
                        }
                        catch
                        {
                            val = 0;
                        }

                        //set parameter
                        switch (nrc)
                        {
                            case 1:
                                sample.C1 = val;
                                sample.L1 = cant;
                                break;
                            case 2:
                                sample.C2 = val;
                                sample.L2 = cant;
                                break;
                            case 3:
                                sample.C3 = val;
                                sample.L3 = cant;
                                break;
                            case 4:
                                sample.C4 = val;
                                sample.L4 = cant;
                                break;
                            case 5:
                                sample.C5 = val;
                                sample.L5 = cant;
                                break;
                            case 6:
                                sample.C6 = val;
                                sample.L6 = cant;
                                break;
                            case 7:
                                sample.C7 = val;
                                sample.L7 = cant;
                                break;
                            case 8:
                                sample.C8 = val;
                                sample.L8 = cant;
                                break;
                            case 9:
                                sample.C9 = val;
                                sample.L9 = cant;
                                break;
                            case 10:
                                sample.C10 = val;
                                sample.L10 = cant;
                                break;
                            case 11:
                                sample.C11 = val;
                                sample.L11 = cant;
                                break;
                            case 12:
                                sample.C12 = val;
                                sample.L12 = cant;
                                break;
                            case 13:
                                sample.C13 = val;
                                sample.L13 = cant;
                                break;
                            case 14:
                                sample.C14 = val;
                                sample.L14 = cant;
                                break;
                            case 15:
                                sample.C15 = val;
                                sample.L15 = cant;
                                break;
                            case 16:
                                sample.C16 = val;
                                sample.L16 = cant;
                                break;
                            case 17:
                                sample.C17 = val;
                                sample.L17 = cant;
                                break;
                            case 18:
                                sample.C18 = val;
                                sample.L18 = cant;
                                break;
                            case 19:
                                sample.C19 = val;
                                sample.L19 = cant;
                                break;
                            case 20:
                                sample.C20 = val;
                                sample.L20 = cant;
                                break;
                            case 21:
                                sample.C21 = val;
                                sample.L21 = cant;
                                break;
                            case 22:
                                sample.C22 = val;
                                sample.L22 = cant;
                                break;
                            default:
                                break;
                        }
                    }

                    count++;
                    countcrot++;
                    if (count < nrrows)
                    {
                        row = table.Rows[count];
                        nrcrot = row["Crotalia"].ToString();
                    }
                }
                refcrot = nrcrot;
                refcontrol = row["NrComanda"].ToString();
                year = refcontrol.Substring(refcontrol.Length - 4);
                samples.Add(sample);
                // add crot
            }
        } // if data exists


        return samples;
    }
}