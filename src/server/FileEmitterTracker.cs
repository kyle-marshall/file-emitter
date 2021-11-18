using System.Collections.Generic;

namespace FileEmitterMod {
    // tracks some file emitter state during which only persists during runtime
    public static class FileEmitterTracker {
        public static readonly uint FLAG_RESET = 1u;
        public static readonly uint FLAG_BIG_ENDIAN = 2u;
        private static Dictionary<string, (string Path, uint Flags)> _fileEmitterData = new Dictionary<string, (string Path, uint Flags)>();
        
        private static (string Path, uint Flags) GetData(string fileEmitterId) {
            if (_fileEmitterData.TryGetValue(fileEmitterId, out (string Path, uint Flags) data)) {
                return data;
            }
            (string Path, uint Flags) newData = (null, 0u);
            _fileEmitterData[fileEmitterId] = newData;
            return newData;
        }

        public static string GetFilePath(string fileEmitterId) {
            var data = GetData(fileEmitterId);
            return data.Path;
        }

        public static void SetFilePath(string fileEmitterId, string filePath) {
            var data = GetData(fileEmitterId);
            _fileEmitterData[fileEmitterId] = (filePath, data.Flags);
        }

        public static uint GetFlags(string fileEmitterId) {
            var data = GetData(fileEmitterId);
            return data.Flags;
        }

        public static void SetFlags(string fileEmitterId, uint newFlags) {
            var data = GetData(fileEmitterId);
            _fileEmitterData[fileEmitterId] = (data.Path, newFlags);
        }
    }
}