using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for MostreCrio
/// </summary>
public class MostreCrio
{
	public MostreCrio()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string strid;
    public string strcodbare;
    public string strcassette;
    public string strposition;
    public string strerror;
    public string strdatatest;
    public string strtimetest;
    public string strpcti;

    public string id
    {
        get
        {
            return strid;
        }
        set
        {
            strid = value;
        }
    }

    public string codbare
    {
        get
        {
            return strcodbare;
        }
        set
        {
            strcodbare = value;
        }
    }

    public string cassette
    {
        get
        {
            return strcassette;
        }
        set
        {
            strcassette = value;
        }
    }

    public string position
    {
        get
        {
            return strposition;
        }
        set
        {
            strposition = value;
        }
    }

    public string error
    {
        get
        {
            return strerror;
        }
        set
        {
            strerror = value;
        }
    }

    public string datatest
    {
        get
        {
            return strdatatest;
        }
        set
        {
            strdatatest = value;
        }
    }

    public string pcti
    {
        get
        {
            return strpcti;
        }
        set
        {
            strpcti = value;
        }
    }

    public string timetest
    {
        get
        {
            return strtimetest;
        }
        set
        {
            strtimetest = value;
        }
    }
}