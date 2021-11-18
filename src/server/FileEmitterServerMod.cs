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
			FileEmitterTracker.SetFilePath(id, path);
			LineWriter lineWriter = LConsole.BeginLine();
			lineWriter.WriteLine($"File emitter '{id}' will now read from '{path}'.");
			lineWriter.End();
		}
		
		[Command("fem-setflags", Description="sets file emitter flags (the bit for reset is 1 and big endian is 2)")]
		static void FEMSetFlags(string id, string flagsRaw) {
			if (uint.TryParse(flagsRaw, out uint flags)) {
				FileEmitterTracker.SetFlags(id, flags);
				LineWriter lineWriter = LConsole.BeginLine();
				lineWriter.WriteLine($"File emitter '{id}' now has flags '{flags}'.");
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
