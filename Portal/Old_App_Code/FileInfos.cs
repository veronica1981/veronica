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
    DateTime dtLastWriteTime;
    long lLength;

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

    public long Length
    {
        get
        {
            return lLength;
        }
        set
        {
            lLength = value;
        }
    }

    public DateTime LastWriteTime
    {
        get
        {
            return dtLastWriteTime;
        }
        set
        {
            dtLastWriteTime = value;
        }
    }
}