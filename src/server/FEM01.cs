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
		private string _id = "";
		private int _bitOffset = 0;
		private int _bitStep = 1;
		private byte[] _fileBytes = null;
		private bool _overflow = false;

		private const int EMIT_PIN = 0;
		private const int RESET_PIN = 1;

		private const int DATA_PIN = 0;
		private const int EOF_PIN = 1;

		public string EmitterId {
			get {
				if (_id == null || _id == "") {
					if (Address != null) {
						_id = string.Format("FE{0:X}", Address.ID);
					}
					else {
						Logger.Info("EmitterId [server]: Can't access Address right now...");
					}
				}
				return _id;
			}
		}

		private byte[] GetFileBytes() {
			if (_fileBytes == null) {
				var id = EmitterId;
				string filePath = FileEmitterTracker.GetFilePath(id);
				if (filePath == null) {
					Logger.Info($"File emitter '{id}' has no path set.");
					return null;
				}
				try {
					_fileBytes = File.ReadAllBytes(filePath);
					_bitOffset = 0;
					_overflow = false;
				}
				catch(Exception) {
            		Logger.Info("Could not read file :(");
					return null;
				}
			}
			return _fileBytes;
		}

		private string formatBytes(byte[] bytes) {
			var s = "";
			foreach(var b in bytes) {
				s += "|"+b.ToString();
			}
			s += "|";
			return s;
		}

		private bool GetNextBit(uint flags) {
			var bytes = GetFileBytes();
			if (bytes == null) {
				return false;
			}
			int byteOffset = _bitOffset / 8;
			if (byteOffset >= bytes.Length) {
				// EOF
				_overflow = true;
				return false;
			}

			bool bigEndian = (flags & FileEmitterTracker.FLAG_BIG_ENDIAN) > 0;

			Logger.Info($"GetNextBit bytes: {formatBytes(bytes)}");
			Logger.Info($"GetNextBit offsets (bit, byte): {_bitOffset}, {byteOffset}");
			Logger.Info($"GetNextBit bigEndian: {bigEndian}");
			
			byte theByte = bytes[byteOffset];
			int rem = _bitOffset % 8;

			/*
			Logger.Info($"theByte: {theByte}");
			Logger.Info($"bitRemainder: {rem}");
			*/
			byte mask = (byte) (1 << (bigEndian ? 7 - rem : rem));
			bool pos = (theByte & mask) != 0;
			_bitOffset += _bitStep;
			return pos;
		}

		private bool IsClockSignalActive() {
			return Inputs[0].On;
		}

		private void Reset(string id, uint flags) {
			_bitOffset = 0;
			_overflow = false;
			Outputs[0].On = false;
			Outputs[1].On = false;
			FileEmitterTracker.SetFlags(id, flags & (~FileEmitterTracker.FLAG_RESET));
		}

		protected override void DoLogicUpdate()
		{
			if (!IsClockSignalActive()) {
				return;
			}
			var id = EmitterId;
			uint flags = FileEmitterTracker.GetFlags(id);
			if ((flags & FileEmitterTracker.FLAG_RESET) > 0) {
				Reset(id, flags);
				return;
			}
			Outputs[0].On = GetNextBit(flags);
			Outputs[1].On = _overflow;
		}

		/* serialization will be nice someday...
		protected override byte[] SerializeCustomData()
        {
			// serialize the id so that client can render it
			Logger.Info($"SerializeCustomData (server)");
			var id = EmitterId;
			var bytes = System.Text.Encoding.ASCII.GetBytes(id);
			var s = formatBytes(bytes);
			Logger.Info($"{id} --> {s}");
			return bytes;
        }*/
		
		/* deserialization will be nice someday...
		protected override void DeserializeData(byte[] bytes)
		{
			if (bytes != null && bytes.Length > 0) {
				//var id = System.Text.Encoding.ASCII.GetString(bytes);
				var s = "";
				foreach(var b in bytes) {
					s += "|"+b.ToString();
				}
				s += "|";
				var id = System.Text.Encoding.ASCII.GetString(bytes);
				Logger.Info($"server: DeserializeData {s} --> '{id}'");
				_id = id;
				_blessedWithId = true;
			}
		}*/
	}
}
