using LICC;
using LogicAPI.Server;

namespace FileEmitterMod
{
	class ServerMain : ServerMod
	{
        protected override void Initialize()
        {
			Logger.Info("FileEmitterMod init");
		}

		[Command("fem-setpath", Description="sets file emitter path")]
		static void FEMSetPath(string id, string path) {
			var idUpper = id.ToUpper();
			FileEmitterTracker.SetFilePath(idUpper, path);
			LineWriter lineWriter = LConsole.BeginLine();
			lineWriter.WriteLine($"File emitter '{idUpper}' will now read from '{path}'.");
			lineWriter.End();
		}
		
		[Command("fem-setflags", Description="sets file emitter flags (the bit for reset is 1 and big endian is 2)")]
		static void FEMSetFlags(string id, string flagsRaw) {
			if (uint.TryParse(flagsRaw, out uint flags)) {
				var idUpper = id.ToUpper();
				FileEmitterTracker.SetFlags(idUpper, flags);
				LineWriter lineWriter = LConsole.BeginLine();
				lineWriter.WriteLine($"File emitter '{idUpper}' now has flags '{flags}'.");
				lineWriter.End();
			}
			else {
				LineWriter lineWriter = LConsole.BeginLine();
				lineWriter.WriteLine($"Could not parse the flags.");
				lineWriter.End();
			}
		}
	}
}
