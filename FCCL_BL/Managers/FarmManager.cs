using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FCCL_DAL.Entities;
using NLog;

namespace FCCL_BL.Managers
{
    public class FarmManager
    {
        private string connectionString;
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public FarmManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<string> GetFarmsForAutocomplete(string farmName)
        {
            var farms = new List<string>();
            var cnn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "select Top 10 Nume from Ferme_CCL where Nume like @Search order by nume";
                cmd.Parameters.AddWithValue("@Search", farmName + "%");
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    farms.Add(reader[0].ToString());
                }
            }     
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFarmsForAutocomplete|farmName:{0} ERROR:{1}", farmName, ex.Message));
            }
            finally
            {
                if(reader != null)
                    reader.Close();
                cnn.Close();
            }
            return farms;
        }

        public List<string> GetCodesForAutocomplete(string code)
        {
            var codes = new List<string>();
            var cnn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "select Top 10 Cod from Ferme_CCL where Cod like @Search order by Cod";
                cmd.Parameters.AddWithValue("@Search", "%" + code + "%");
                cnn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    codes.Add(reader[0].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetCodesForAutocomplete|code:{0} ERROR:{1}", code, ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cnn.Close();
            }
            return codes;
        }

        public List<Ferme_CCL> GetAllFarmsForDropDown()
        {
            var farms = new List<Ferme_CCL>();
            var cn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd = new SqlCommand("SELECT Cod,Nume FROM Ferme_CCL WHERE Cod IS NOT NULL", cn);
                cn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    farms.Add(new Ferme_CCL {Cod = reader[0].ToString(), Nume = reader[1].ToString()});
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAllFarmsForDropDown|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return farms;
        }

        public Ferme_CCL GetFarmById(int farmId)
        {
            Ferme_CCL farm = null;
            var cn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd =
                    new SqlCommand(
                        "SELECT Cod,Ferme_CCL,ID,Nume,FabricaID,Strada,Numar,Oras,Judet,JudetID,Telefon,Email,FermierID,CodPostal,Fax,PersonaDeContact,TelPersoanaContact,SendSms FROM Ferme_CCL WHERE Cod IS NOT NULL AND ID = @id",
                        cn);
                cmd.Parameters.AddWithValue("@id", farmId);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    farm = new Ferme_CCL
                    {
                        Cod = reader[0] as String,
                        Ferme = reader[1] as String,
                        Id = reader[2] as Int32?,
                        Nume = reader[3] as String,
                        FabricaId = reader[4] as Int32?,
                        Strada = reader[5] as String,
                        Numar = reader[6] as String,
                        Oras = reader[7] as String,
                        Judet = reader[8] as String,
                        JudetId = reader[9] as Int32?,
                        Telefon = reader[10] as String,
                        Email = reader[11] as String,
                        FermierId = reader[12] as Int32?,
                        CodPostal = reader[13] as String,
                        Fax = reader[14] as String,
                        PersoanaDeContact = reader[15] as String,
                        TelPersoanaContact = reader[16] as String,
                        SendSms = (bool) reader[17]
                    };
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAllFarmsForDropDown|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return farm;
        }

        public List<Ferme_CCL> GetAllFarms(string farm = null, string cod = null)
        {
            var farms = new List<Ferme_CCL>();
            var cn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd =
                    new SqlCommand(
                        "SELECT Cod,Ferme_CCL,ID,Nume,FabricaID,Strada,Numar,Oras,Judet,JudetID,Telefon,Email,FermierID,CodPostal,Fax,PersonaDeContact,TelPersoanaContact,SendSms FROM Ferme_CCL WHERE Cod IS NOT NULL",
                        cn);
                if (farm != null)
                {
                    cmd.CommandText += " AND Nume Like @FermaNume";
                    cmd.Parameters.AddWithValue("@FermaNume", "%" + farm + "%");
                }
                if (cod != null)
                {
                    cmd.CommandText += " AND Cod Like @Code";
                    cmd.Parameters.AddWithValue("@Code", "%" + cod + "%");
                }
                cn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    farms.Add(new Ferme_CCL
                    {
                        Cod = reader[0] as String,
                        Ferme = reader[1] as String,
                        Id = reader[2] as Int32?,
                        Nume = reader[3] as String,
                        FabricaId = reader[4] as Int32?,
                        Strada = reader[5] as String,
                        Numar = reader[6] as String,
                        Oras = reader[7] as String,
                        Judet = reader[8] as String,
                        JudetId = reader[9] as Int32?,
                        Telefon = reader[10] as String,
                        Email = reader[11] as String,
                        FermierId = reader[12] as Int32?,
                        CodPostal = reader[13] as String,
                        Fax = reader[14] as String,
                        PersoanaDeContact = reader[15] as String,
                        TelPersoanaContact = reader[16] as String,
                        SendSms = (bool)reader[17]
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetAllFarmsForDropDown|error:{0}", ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return farms;
        }

        public Ferme_CCL GetFarmByName(string numeFerma)
        {
            Ferme_CCL farm = null;
            var cn = new SqlConnection(connectionString);
            SqlDataReader reader = null;
            try
            {
                var cmd =
                    new SqlCommand(
                        "SELECT Cod,Ferme_CCL,ID,Nume,FabricaID,Strada,Numar,Oras,Judet,JudetID,Telefon,Email,FermierID,CodPostal,Fax,PersonaDeContact,TelPersoanaContact,SendSms FROM Ferme_CCL WHERE Cod IS NOT NULL AND Nume = @nume",
                        cn);
                cmd.Parameters.AddWithValue("@nume", numeFerma);
                cn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    farm = new Ferme_CCL
                    {
                        Cod = reader[0] as String,
                        Ferme = reader[1] as String,
                        Id = reader[2] as Int32?,
                        Nume = reader[3] as String,
                        FabricaId = reader[4] as Int32?,
                        Strada = reader[5] as String,
                        Numar = reader[6] as String,
                        Oras = reader[7] as String,
                        Judet = reader[8] as String,
                        JudetId = reader[9] as Int32?,
                        Telefon = reader[10] as String,
                        Email = reader[11] as String,
                        FermierId = reader[12] as Int32?,
                        CodPostal = reader[13] as String,
                        Fax = reader[14] as String,
                        PersoanaDeContact = reader[15] as String,
                        TelPersoanaContact = reader[16] as String,
                        SendSms = (bool) reader[17]
                    };
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("GetFarmByName|numeFerma:{0} ERROR:{1}", numeFerma,ex.Message));
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                cn.Close();
            }
            return farm;
        }

        public bool DeleteFarm(string idFarm)
        {
            var cn = new SqlConnection(connectionString);
            int affectedRows = 0;
            try
            {
                var cmd = new SqlCommand("DELETE FROM [Ferme_CCL] WHERE [ID] = @ID");
                cmd.Parameters.AddWithValue("@ID", idFarm);
                cmd.Connection = cn;
                cn.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("DeleteFarm|farmId:{0} ERROR:{1}", idFarm, ex.Message));
            }
            finally
            {
                cn.Close();
            }
            return affectedRows > 0;
        }

        public bool InsertFarm(Ferme_CCL farm)
        {
            var cn = new SqlConnection(connectionString);
            int affectedRows = 0;
            try
            {
                var cmd =
                    new SqlCommand(
                        "INSERT INTO Ferme_CCL(Cod,Nume,FabricaID,Strada,Numar,Oras,Judet,CodPostal,Telefon,Fax,Email,FermierID,PersonaDeContact,TelPersoanaContact,Ferme_CCL,JudetID,SendSms) VALUES (@Cod,@Nume,@FabricaID,@Strada,@Numar,@Oras,@Judet,@CodPostal,@Telefon,@Fax,@Email,@FermierID,@PersonaDeContact,@TelPersoanaContact,@Ferme_CCL,@JudetID,@SendSms)");
                cmd.Parameters.AddWithValue("@Cod", farm.Cod);
                cmd.Parameters.AddWithValue("@Nume", farm.Nume);
                cmd.Parameters.AddWithValue("@FabricaID", farm.FabricaId);
                cmd.Parameters.AddWithValue("@Strada", farm.Strada);
                cmd.Parameters.AddWithValue("@Numar", farm.Numar);
                cmd.Parameters.AddWithValue("@Oras", farm.Oras);
                cmd.Parameters.AddWithValue("@Judet", farm.Judet);
                cmd.Parameters.AddWithValue("@CodPostal", farm.CodPostal);
                cmd.Parameters.AddWithValue("@Telefon", farm.Telefon);
                cmd.Parameters.AddWithValue("@Fax", farm.Fax);
                cmd.Parameters.AddWithValue("@Email", farm.Email);
                cmd.Parameters.AddWithValue("@FermierID", farm.FermierId);
                cmd.Parameters.AddWithValue("@PersonaDeContact", farm.PersoanaDeContact);
                cmd.Parameters.AddWithValue("@TelPersoanaContact", farm.TelPersoanaContact);
                cmd.Parameters.AddWithValue("@Ferme_CCL", farm.Ferme);
                cmd.Parameters.AddWithValue("@JudetID", farm.JudetId);
                cmd.Parameters.AddWithValue("@SendSms", farm.SendSms);
                cmd.Connection = cn;
                cn.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("InsertFarm|farm:{0} ERROR:{1}", farm.Id, ex.Message));
            }
            finally
            {
                cn.Close();
            }
            return affectedRows > 0;
        }

        public bool UpdateFarm(Ferme_CCL farm)
        {
            var cn = new SqlConnection(connectionString);
            int affectedRows = 0;
            try
            {
                var cmd =
                    new SqlCommand(
                        "UPDATE Ferme_CCL SET Cod = @Cod,Nume = @Nume,FabricaID = @FabricaID,Strada = @Strada,Numar = @Numar, Oras = @Oras,Judet = @Judet,CodPostal = @CodPostal,Telefon = @Telefon,Fax = @Fax,Email = @Email,FermierID = @FermierID,PersonaDeContact = @PersonaDeContact,TelPersoanaContact = @TelPersoanaContact,Ferme_CCL = @Ferme_CCL,JudetID =@JudetID,SendSms = @SendSms WHERE ID = @Id");
                cmd.Parameters.AddWithValue("@ID", farm.Id);
                cmd.Parameters.AddWithValue("@Cod", farm.Cod);
                cmd.Parameters.AddWithValue("@Nume", farm.Nume);
                cmd.Parameters.AddWithValue("@FabricaID", farm.FabricaId);
                cmd.Parameters.AddWithValue("@Strada", farm.Strada);
                cmd.Parameters.AddWithValue("@Numar", farm.Numar);
                cmd.Parameters.AddWithValue("@Oras", farm.Oras);
                cmd.Parameters.AddWithValue("@Judet", farm.Judet);
                cmd.Parameters.AddWithValue("@CodPostal", farm.CodPostal);
                cmd.Parameters.AddWithValue("@Telefon", farm.Telefon);
                cmd.Parameters.AddWithValue("@Fax", farm.Fax);
                cmd.Parameters.AddWithValue("@Email", farm.Email);
                cmd.Parameters.AddWithValue("@FermierID", farm.FermierId);
                cmd.Parameters.AddWithValue("@PersonaDeContact", farm.PersoanaDeContact);
                cmd.Parameters.AddWithValue("@TelPersoanaContact", farm.TelPersoanaContact);
                cmd.Parameters.AddWithValue("@Ferme_CCL", farm.Ferme);
                cmd.Parameters.AddWithValue("@JudetID", farm.JudetId);
                cmd.Parameters.AddWithValue("@SendSms", farm.SendSms);
                cmd.Connection = cn;
                cn.Open();
                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("UpdateFarm|farm:{0} ERROR:{1}", farm.Id, ex.Message));
            }
            finally
            {
                cn.Close();
            }
            return affectedRows > 0;
        }
    }
}
