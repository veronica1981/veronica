using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SampleAnalizeExt
/// </summary>
public class SampleAnalizeExt
{
    string strNrmatricol;
    string strCodbare;
    string strNrcontrol;
    string strDatatestare;
    double dbCantitate;
    double dbGrasime;
    double dbProteine;
    double dbCaseina;
    double dbLactoza;
    double dbPh;
    double dbUrea;
    double dbSolide;
    double dbNcs;
    double dbVal;
    double dbGrasProt;

	public SampleAnalizeExt()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string Nrcontrol
    {
        get
        {
            return strNrcontrol;
        }
        set
        {
            strNrcontrol = value;
        }
    }

    public string Nrmatricol
    {
        get
        {
            return strNrmatricol;
        }
        set
        {
            strNrmatricol = value;
        }
    }

    public string Codbare
    {
        get
        {
            return strCodbare;
        }
        set
        {
            strCodbare = value;
        }
    }

    public string Datatestare
    {
        get
        {
            return strDatatestare;
        }
        set
        {
            strDatatestare = value;
        }
    }
    public double Cantitate
    {
        get
        {
            return dbCantitate;
        }
        set
        {
            dbCantitate = value;
        }
    }
    public double Grasime
    {
        get
        {
            return dbGrasime;
        }
        set
        {
            dbGrasime = value;
        }
    }
    public double Proteine
    {
        get
        {
            return dbProteine;
        }
        set
        {
            dbProteine = value;
        }
    }
    public double Lactoza
    {
        get
        {
            return dbLactoza;
        }
        set
        {
            dbLactoza = value;
        }
    }
    public double Caseina
    {
        get
        {
            return dbCaseina;
        }
        set
        {
            dbCaseina = value;
        }
    }
    public double Ph
    {
        get
        {
            return dbPh;
        }
        set
        {
            dbPh = value;
        }
    }
    public double Solide
    {
        get
        {
            return dbSolide;
        }
        set
        {
            dbSolide = value;
        }
    }

    public double Urea
    {
        get
        {
            return dbUrea;
        }
        set
        {
            dbUrea = value;
        }
    }
    public double Ncs
    {
        get
        {
            return dbNcs;
        }
        set
        {
            dbNcs = value;
        }
    }

    public double Val
    {
        get
        {
            return dbVal;
        }
        set
        {
            dbVal = value;
        }
    }

    public double GrasProt
    {
        get
        {
            return dbGrasProt;
        }
        set
        {
            dbGrasProt = value;
        }
    }

}