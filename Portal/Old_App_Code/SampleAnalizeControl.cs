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
public class SampleAnalizeControl
{
	
    
        int intNrc;
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

        public SampleAnalizeControl()
	    {
        }
		

        public int Nrc
        {
        get
        {
            return intNrc;
        }
        set
        {
            intNrc = value;
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
            get { return dbC13; }
            set { dbC13 = value; }
        }

        public double C14
        {
            get { return dbC14; }
            set { dbC14 = value; }
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
}