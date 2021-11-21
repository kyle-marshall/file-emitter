using System;
using System.IO;
using LogicAPI.Server.Components;
using System.Text;
using JimmysUnityUtilities;
using JimmysUnityUtilities.Random;

namespace FileEmitterMod.Server
{
    class FEM01 : LogicComponent
    {
        private const int CUSTOM_DATA_TEXT_OFFSET = 16;
        private int _bitOffset = 0;
        private int _bitStep = 1;
        private byte[] _fileBytes = null;
        private bool _eof = false;
        private bool _reset = false;
        private bool _bigEndian = false;

        // input pins
        private const int EMIT_PIN = 0;
        private const int RESET_PIN = 1;
        private const int BIG_ENDIAN_PIN = 2;

        // output pins
        private const int DATA_PIN = 0;
        private const int EOF_PIN = 1;

        private string GetFilePath() {
            byte[] customDataBytes = ComponentData?.CustomData;

            if (customDataBytes == null || customDataBytes.Length <= CUSTOM_DATA_TEXT_OFFSET) {
                Logger.Info("The text must be updated first via 'x' menu.");
                return null;
            }

            // to find CUSTOM_DATA_TEXT_OFFSET I had to print the bytes coming back and notice where IData.LabelText starts
            // I'm getting the bytes from 16 up to the next byte which is too small to be a valid file path character

            // exclusive end index
            int sliceEndIndex = Array.FindIndex(customDataBytes, CUSTOM_DATA_TEXT_OFFSET, b => b < 32);
            if (sliceEndIndex == -1) sliceEndIndex = customDataBytes.Length;
            int sliceLength = sliceEndIndex - CUSTOM_DATA_TEXT_OFFSET;
            byte[] pathBytes = (new ArraySegment<byte>(customDataBytes, CUSTOM_DATA_TEXT_OFFSET, sliceLength)).ToArray();

            //Logger.Info($"GetFileBytes path bytes: {formatBytes(pathBytes)}");
            return Encoding.UTF8.GetString(pathBytes).Trim();
        }

        private byte[] GetFileBytes()
        {
            if (_fileBytes != null) return _fileBytes;

            string filePath = GetFilePath();
            if (filePath == null || !File.Exists(filePath))
            {
                Logger.Info($"File emitter has invalid path '{filePath}'.");
                return null;
            }
            try
            {
                _fileBytes = File.ReadAllBytes(filePath);
                _bitOffset = 0;
                _eof = false;
            }
            catch (Exception)
            {
                Logger.Info("Could not read file :(");
                return null;
            }
            return _fileBytes;
        }

        private string formatBytes(byte[] bytes)
        {
            if (bytes == null) return "<null>";
            var s = "";
            foreach (var b in bytes)
            {
                s += "|" + b.ToString("X2");
            }
            s += "|";
            return s;
        }

        private bool GetNextBit()
        {
            var bytes = GetFileBytes();
            if (bytes == null)
            {
                return false;
            }
            int byteOffset = _bitOffset / 8;
            if (byteOffset >= bytes.Length)
            {
                // EOF
                _eof = true;
                return false;
            }

			// leaving spam enabled for now for sanity checks
            //Logger.Info($"GetNextBit bytes: {formatBytes(bytes)}");
            //Logger.Info($"GetNextBit offsets (bit, byte): {_bitOffset}, {byteOffset}");
            //Logger.Info($"GetNextBit bigEndian: {bigEndian}");

            byte theByte = bytes[byteOffset];
            int rem = _bitOffset % 8;

            byte mask = (byte)(1 << (_bigEndian ? 7 - rem : rem));
            bool pos = (theByte & mask) != 0;
            _bitOffset += _bitStep;
            return pos;
        }

        private bool IsClockSignalActive()
        {
            return Inputs[0].On;
        }

        private void Reset()
        {
            _bitOffset = 0;
            _eof = false;
            _fileBytes = null;
            Outputs[DATA_PIN].On = false;
            Outputs[EOF_PIN].On = false;
        }

        protected void ReadFlags() {
            _reset = Inputs[RESET_PIN].On;
            _bigEndian = Inputs[BIG_ENDIAN_PIN].On;
        }

        protected override void DoLogicUpdate()
        {
            if (!IsClockSignalActive())
            {
                return;
            }
            ReadFlags();
            if (_reset)
            {
                Reset();
                return;
            }
            Outputs[DATA_PIN].On = GetNextBit();
            Outputs[EOF_PIN].On = _eof;
        }
    }
}
