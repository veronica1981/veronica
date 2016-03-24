using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL
{
	public enum FCCLApp
	{
		Default = 0,
		Cop = 1,
		FCCL = 2
	}

	public enum FCCLReportType
	{
		None = 0,
		Crotalii = 1,
		Registru = 2,
		Fabrica = 3,
		FabricaIntre = 4,
		FCB = 5,
		Ferme = 6
	}

	public enum FCCLTestType
	{
		Grasime = 1,
		ProcentProteine = 2,
		ProcentLactoza = 3,
		NCS = 4,
		NTG = 5,
		FrzPnt = 6,
		Solids = 7,
		Ph = 8,
		Urea = 9,
		Casein = 10
	}

	public static class Constants
	{
		public const string LOG_AUTOMATED_IMPORT_PATH_RELATIVE = @"Downloads\FisiereImportateAutomat\";
		public const string FOLDER_REMOTE_PROCESSED_RELATIVE = @"SalvatePeServer\";
		public const string FOLDER_LOCAL_IMPORTED = "FisiereImportate";
	}
}
