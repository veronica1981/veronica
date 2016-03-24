using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using FCCL_BL.Managers;

public partial class EditareFerme : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> AutocompleteFarms(string farmName)
    {
        var farmManager =
                           new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        return farmManager.GetFarmsForAutocomplete(farmName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> AutocompleteFactories(string factoryName)
    {
        var factoryManager =
                   new FactoryManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        return factoryManager.GetFactoriesForAutocomplete(factoryName);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static List<string> AutocompleteCode(string code)
    {
        var farmManager =
           new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        return farmManager.GetCodesForAutocomplete(code);
    }


    public void BindData()
    {
        var values = createTable();
        GridView1.DataSource = values;
        GridView1.DataBind();
    }

    public List<Fabrica> createTable()
    {
        var farmManager =
          new FarmManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);

        var factory = Fabrica.Text.Length > 0 ? Fabrica.Text.Trim() : null;
        var farm = Ferma.Text.Length > 0 ? Ferma.Text.Trim() : null;
        var cod = Code.Text.Length > 0 ? Code.Text.Trim() : null;

        var farms = farmManager.GetAllFarms(farm, cod);

        var factoryManager =
          new FactoryManager(ConfigurationManager.ConnectionStrings["AdditionalInformation"].ConnectionString);
        var factories = factoryManager.GetFactories(factory);

        var countiesManager =
         new CountyManager(ConfigurationManager.ConnectionStrings["fccl2ConnectionString"].ConnectionString);
        var counties = countiesManager.GetAllCounties();

        var values = farms.Select(x => new Fabrica
        {
            id = x.Id.ToString(),
            cod = x.Cod,
            nume = x.Nume,
            numar = x.Numar,
            email = x.Email,
            telefon = x.Telefon,
            oras = x.Oras,
            strada = x.Strada,
            fabricaid = x.FabricaId.ToString(),
            judet = x.Judet
        }).ToList();

        var farctorieslist = values.ToList();

        foreach (var fabrica in farctorieslist)
        {
            var factoryEntity = factories.FirstOrDefault(x => x.Id.ToString(CultureInfo.InvariantCulture) == fabrica.fabricaid);
            if (factoryEntity == null)
                values.Remove(fabrica);
            else
                fabrica.fabricaid = factoryEntity.Nume;

            var countyEntity = counties.FirstOrDefault(x => x.Id.ToString(CultureInfo.InvariantCulture) == fabrica.judet);
            if (countyEntity != null)
                fabrica.judet = countyEntity.DenLoc;
        }

        lcount.Text = values.Count + " ferme";
        return values;
    }
    protected void GridView1_PageIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        BindData();
    }


}
