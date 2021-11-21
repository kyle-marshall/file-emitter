// TODO cleanup unneeded references
using System.Collections.Generic;
using System.Text;
using JimmysUnityUtilities;
using JimmysUnityUtilities.Random;
using LogicLocalization;
using LogicUI;
using LogicWorld.ClientCode;
using LogicWorld.ClientCode.LabelAlignment;
using LogicWorld.ClientCode.Resizing;
using LogicWorld.Interfaces;
using LogicWorld.References;
using LogicWorld.Rendering.Chunks;
using LogicWorld.Rendering.Components;
using LogicWorld.SharedCode.Components;
using TMPro;
using UnityEngine;

namespace FileEmitterMod.Client
{
    class FEM01 : ComponentClientCode<Label.IData>
    {
        private const string PLACEHOLDER_PATH = "/path/to/your/file.yo";

        protected static Color24 DefaultColor 
			= new Color24((byte) 38, (byte) 38, (byte) 38);

        protected override void SetDataDefaultValues()
        {
            Data.LabelText = PLACEHOLDER_PATH;
            // The rest of the IData properties aren't used by the mod
            Data.LabelFontSizeMax = 0f;
            Data.LabelColor = DefaultColor;
            Data.LabelMonospace = false;
            Data.HorizontalAlignment = LabelAlignmentHorizontal.Center;
            Data.VerticalAlignment = LabelAlignmentVertical.Middle;
            Data.SizeX = 1;
            Data.SizeZ = 1;
        }
    }
}
