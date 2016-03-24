using FCCL_BL.Managers;
using FCCL_DAL;
using NLog;
using System;
using System.Data.Entity;
using System.IO;

namespace FCCL_BL.Bus
{
	public class FileImporter
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Logger loggerRezultate = LogManager.GetLogger("Rezultate");
		private MachineManager mManager;
		private string localServer;

		public FileImporter(FCCLDbContext context, string localServer)
		{
			logger.Info(string.Format("FileImporter Ctor|localServer:{0}", localServer));
			this.localServer = localServer;
			mManager = new MachineManager(context);
		}

		public void DoRemoteImport(IMostreDB mostre)
		{
			logger.Info(string.Format("DoRemoteImport started"));

			foreach (var machine in mManager.GetMachines())
			{
				RetrieveAndProcessFiles(machine, machine.RemotePath, mostre);
			}
		}

		public void DoLocalImport(IMostreDB mostre, string localRoot)
		{
			logger.Info(string.Format("DoLocalImport started"));

			foreach (var machine in mManager.GetMachines())
			{
				RetrieveAndProcessFiles(machine, string.Format(machine.LocalDownloadPathFormat, localRoot), mostre);
			}
		}

		private int RetrieveAndProcessFiles(Machine machine, string fromFolder, IMostreDB mostre)
		{
			int fileProcessed = 0;
			try
			{
				foreach (string remoteFilePath in Directory.GetFiles(fromFolder, machine.FileFilter))
				{
					string filename = Path.GetFileName(remoteFilePath);
					string localFilePath = Path.Combine(string.Format(machine.LocalSavePathFormat, localServer), filename);
					string newRemoteFilePath = Path.Combine(fromFolder, Constants.FOLDER_REMOTE_PROCESSED_RELATIVE, filename);
					logger.Info(string.Format("RetrieveAndProcessFiles|filename:{0}|localFilePath:{1}|newRemoteFilePath:{2}", filename, localFilePath, newRemoteFilePath));
					if (!Directory.Exists(Path.GetDirectoryName(localFilePath)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(localFilePath));
					}
					if(File.Exists(localFilePath))
					{
						File.Delete(localFilePath);
						logger.Info(string.Format("RetrieveAndProcessFiles|localFilePath:{0} EXISTED!", localFilePath));
					}
					File.Copy(remoteFilePath, localFilePath, true);
					if (!Directory.Exists(Path.GetDirectoryName(newRemoteFilePath)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(newRemoteFilePath));
					}
					if (File.Exists(newRemoteFilePath))
					{
						File.Delete(newRemoteFilePath);
						logger.Info(string.Format("RetrieveAndProcessFiles|newRemoteFilePath:{0} EXISTED!", newRemoteFilePath));
					}
					File.Move(remoteFilePath, newRemoteFilePath);

					mostre.ImportManual(localFilePath);

					File.Move(newRemoteFilePath, newRemoteFilePath + "_importat" + (DateTime.Now).ToString("yyyyMMddhhmm"));
					fileProcessed++;
				}
			}
			catch (Exception ex)
			{
				logger.DebugException(string.Format("RetrieveAndProcessFiles|machine:{0}|fromFolder:{1}", machine.Name, fromFolder), ex);
				loggerRezultate.Error("exceptie import: " + machine.Name + "  " + ex.Message + ex.StackTrace);
			}
			return fileProcessed;
		}
	}
}