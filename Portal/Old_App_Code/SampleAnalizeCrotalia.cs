using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

/// <summary>
/// Summary description for SampleAnalize
/// </summary>
public class SampleAnalizeCrotalia
{

        string strYear;
        string strCrot;
        double dbC1 = 0;
        double dbC2 = 0;
        double dbC3 =0;
        double dbC4 =0;
        double dbC5 =0;
        double dbC6 =0;
        double dbC7 =0;
        double dbC8 =0;
        double dbC9 =0;
        double dbC10 =0;
        double dbC11 =0;
        double dbC12 =0;
        double dbC13 = 0;
        double dbC14 = 0;
        double dbC15 = 0;
        double dbC16 = 0;
        double dbC17 = 0;
        double dbC18 = 0;
        double dbC19 = 0;
        double dbC20 = 0;
        double dbC21 = 0;
        double dbC22 = 0;

        double dbL1 = 0;
        double dbL2 = 0;
        double dbL3 = 0;
        double dbL4 = 0;
        double dbL5 = 0;
        double dbL6 = 0;
        double dbL7 = 0;
        double dbL8 = 0;
        double dbL9 = 0;
        double dbL10 = 0;
        double dbL11 = 0;
        double dbL12 = 0;
        double dbL13 = 0;
        double dbL14 = 0;
        double dbL15 = 0;
        double dbL16 = 0;
        double dbL17 = 0;
        double dbL18 = 0;
        double dbL19 = 0;
        double dbL20 = 0;
        double dbL21 = 0;
        double dbL22 = 0;

        public SampleAnalizeCrotalia()
	    {
        }
		

        public string Crot
        {
        get
        {
            return strCrot;
        }
        set
        {
            strCrot = value;
        }
        }

        public string Year
        {
            get
            {
                return strYear;
            }
            set
            {
                strYear = value;
            }
        }

        public double C1
        {
            get
            {
                return dbC1;
            }
            set
            {
                dbC1 = value;
            }
        }

    public double C2
        {
            get
            {
                return dbC2;
            }
            set
            {
                dbC2 = value;
            }
        }

        public double C3
        {
            get
            {
                return dbC3;
            }
            set
            {
                dbC3 = value;
            }
        }

        public double C4
        {
            get
            {
                return dbC4;
            }
            set
            {
                dbC4 = value;
            }
        }

        public double C5
        {
            get {return dbC5;}
            set {dbC5 = value;}
        }

        public double C6
        {
            get { return dbC6; }
            set { dbC6 = value; }
        }

        public double C7
        {
            get { return dbC7; }
            set { dbC7 = value; }
        }

        public double C8
        {
            get { return dbC8; }
            set { dbC8 = value; }
        }

        public double C9
        {
            get { return dbC9; }
            set { dbC9 = value; }
        }

        public double C10
        {
            get { return dbC10; }
            set { dbC10 = value; }
        }

        public double C11
        {
            get { return dbC11; }
            set { dbC11 = value; }
        }

        public double C12
        {
            get { return dbC12; }
            set { dbC12 = value; }
        }
//
        public double C13
        {
            get
            {
                return dbC13;
            }
            set
            {
                dbC13 = value;
            }
        }

        public double C14
        {
            get
            {
                return dbC14;
            }
            set
            {
                dbC14 = value;
            }
        }

        public double C15
        {
            get { return dbC15; }
            set { dbC15 = value; }
        }

        public double C16
        {
            get { return dbC16; }
            set { dbC16 = value; }
        }

        public double C17
        {
            get { return dbC17; }
            set { dbC17 = value; }
        }

        public double C18
        {
            get { return dbC18; }
            set { dbC18 = value; }
        }

        public double C19
        {
            get { return dbC19; }
            set { dbC19 = value; }
        }

        public double C20
        {
            get { return dbC20; }
            set { dbC20 = value; }
        }

        public double C21
        {
            get { return dbC21; }
            set { dbC21 = value; }
        }

        public double C22
        {
            get { return dbC22; }
            set { dbC22 = value; }
        }

//

        public double L1
        {
            get { return dbL1; }
            set { dbL1 = value; }
        }

      public double L2
        {
            get { return dbL2; }
            set { dbL2 = value; }
        }

        public double L3
        {
            get { return dbL3; }
            set { dbL3 = value; }
        }

        public double L4
        {
            get { return dbL4; }
            set { dbL4 = value; }
        }

        public double L5
        {
            get { return dbL5; }
            set { dbL5 = value; }
        }

        public double L6
        {
            get { return dbL6; }
            set { dbL6 = value; }
        }

        public double L7
        {
            get { return dbL7; }
            set { dbL7 = value; }
        }

        public double L8
        {
            get { return dbL8; }
            set { dbL8 = value; }
        }

        public double L9
        {
            get { return dbL9; }
            set { dbL9 = value; }
        }

        public double L10
        {
            get { return dbL10; }
            set { dbL10 = value; }
        }

        public double L11
        {
            get { return dbL11; }
            set { dbL11 = value; }
        }

        public double L12
        {
            get { return dbL12; }
            set { dbL12 = value; }
        }
//
        public double L13
        {
            get { return dbL13; }
            set { dbL13 = value; }
        }

        public double L14
        {
            get { return dbL14; }
            set { dbL14 = value; }
        }

        public double L15
        {
            get { return dbL15; }
            set { dbL15 = value; }
        }

        public double L16
        {
            get { return dbL16; }
            set { dbL16 = value; }
        }

        public double L17
        {
            get { return dbL17; }
            set { dbL17 = value; }
        }

        public double L18
        {
            get { return dbL18; }
            set { dbL18 = value; }
        }

        public double L19
        {
            get { return dbL19; }
            set { dbL19 = value; }
        }

        public double L20
        {
            get { return dbL20; }
            set { dbL20 = value; }
        }

        public double L21
        {
            get { return dbL21; }
            set { dbL21 = value; }
        }

        public double L22
        {
            get { return dbL22; }
            set { dbL22 = value; }
        }

//

}