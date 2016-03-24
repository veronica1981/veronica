using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FileInfos
/// </summary>
public class FileInfos
{

    string strName;
    string strDownloadLink;

	public FileInfos()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string Name
    {
        get
        {
            return strName;
        }
        set
        {
            strName = value;
        }
    }

    public string DownloadLink
    {
        get
        {
            return strDownloadLink;
        }
        set
        {
            strDownloadLink = value;
        }
    }
}