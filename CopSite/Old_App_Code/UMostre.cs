using System;
using System.Globalization;
using FCCL_DAL;

/// <summary>
/// Summary description for UMostre
/// </summary>
public class UMostre
{
    public static int VerificGrasime(string valGrasime)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Grasime, valGrasime) ? 0 : 1;
    }

    public static int VerificProteine(string valProteine)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.ProcentProteine, valProteine) ? 0 : 1;
    }

    public static int VerificLactoza(string valLactoza)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.ProcentLactoza, valLactoza) ? 0 : 1;
    }

    public static int VerificNCS(string valNCS)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.NCS, valNCS) ? 0 : 1;
    }

    public static int VerificNTG(string valNTG)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.NTG, valNTG) ? 0 : 1;
    }

    public static int VerificSolids(string valSolids)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Solids, valSolids) ? 0 : 1;
    }

    public static int VerificPctInghet(string valPctInghet)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.FrzPnt, valPctInghet) ? 0 : 1;
    }

    public static int VerificPh(string valPh)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Ph, valPh) ? 0 : 1;
    }

    public static int VerificUrea(string valUrea)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Urea, valUrea) ? 0 : 1;
    }

    public static int VerificCasein(string valCasein)
    {
        return StaticDataHelper.IntervalManager.IsWithinLimits(FCCLTestType.Casein, valCasein) ? 0 : 1;
    }

    public static string MyRound(string str)
    {
        int pos;
        pos = str.IndexOf(".");
        if (pos > 0)
        {
            int p;
            p = str.Length - (pos + 2);
            if (p > 0)
                return str.Remove(pos + 2, str.Length - (pos + 2));
            return str;
        }
        return str;
    }

    public static string ApaAdaugata(string valPctInghet)
    {
        string sverific = " ";
        double verific;
        if (valPctInghet == null || Convert.ToDouble(valPctInghet, CultureInfo.InvariantCulture) > 5000.00)
            verific = 0;
        else
            verific = -((-0.515 - Convert.ToDouble("-0." + valPctInghet + "00", CultureInfo.InvariantCulture)) / 0.005);
        if (verific <= 0)
            verific = 0;
        sverific = MyRound(Convert.ToString(verific));

        return sverific;
    }
}