using LICC;
using LogicAPI.Client;

namespace FileEmitterMod
{
    class FileEmitterClientMod : ClientMod
    {
		// This command is useless but I'll leave it for the sake of example
        [Command("tacoclient", Description = "tacos are... (client)")]
        static void TacoClient(string s)
        {
            LineWriter lineWriter = LConsole.BeginLine();
            lineWriter.WriteLine($"client tacos are {s}");
            lineWriter.End();
        }
    }
}
