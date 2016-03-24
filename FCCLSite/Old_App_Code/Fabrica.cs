using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Fabrica
/// </summary>
public class Fabrica
{
    public Fabrica()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string strid;
    public string strnume;
    public string strstrada;
    public string strnumar;
    public string stroras;
    public string strjudet;
    public string strcodpostal;
    public string strtelefon;
    public string strfax;
    public string stremail;
    public string strperscontact;
    public string strtelperscontact;
    public string strcod;
    public string strfabricaid;
    public string strfermier;

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

    public string nume
    {
        get
        {
            return strnume;
        }
        set
        {
            strnume = value;
        }
    }

    public string strada
    {
        get
        {
            return strstrada;
        }
        set
        {
            strstrada = value;
        }
    }

    public string numar
    {
        get
        {
            return strnumar;
        }
        set
        {
            strnumar = value;
        }
    }

    public string oras
    {
        get
        {
            return stroras;
        }
        set
        {
            stroras = value;
        }
    }

    public string judet
    {
        get
        {
            return strjudet;
        }
        set
        {
            strjudet = value;
        }
    }

    public string codpostal
    {
        get
        {
            return strcodpostal;
        }
        set
        {
            strcodpostal = value;
        }
    }

    public string telefon
    {
        get
        {
            return strtelefon;
        }
        set
        {
            strtelefon = value;
        }
    }

    public string fax
    {
        get
        {
            return strfax;
        }
        set
        {
            strfax = value;
        }
    }

    public string email
    {
        get
        {
            return stremail;
        }
        set
        {
            stremail = value;
        }
    }

    public string perscontact
    {
        get
        {
            return strperscontact;
        }
        set
        {
            strperscontact = value;
        }
    }

    public string telperscontact
    {
        get
        {
            return strtelperscontact;
        }
        set
        {
            strtelperscontact = value;
        }
    }


    public string cod
    {
        get
        {
            return strcod;
        }
        set
        {
            strcod = value;
        }
    }

    public string fabricaid
    {
        get
        {
            return strfabricaid;
        }
        set
        {
            strfabricaid = value;
        }
    }

    public string fermier
    {
        get
        {
            return strfermier;
        }
        set
        {
            strfermier = value;
        }
    }
}
