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
    class FEM01 : ComponentClientCode<FEM01.IData>
    {
        private TextMeshPro _textMesh;

        protected static Color24 DefaultColor 
			= new Color24((byte) 38, (byte) 38, (byte) 38);

        private string _id = null;

        public string EmitterId
        {
            get
            {
                if (_id == null || _id == "")
                {
                    if (Address != null)
                    {
                        _id = string.Format("FE{0:X}", Address.ID);
                    }
                    else
                    {
                        Logger.Info("EmitterId [client]: Can't access Address right now...");
                    }
                }
                return _id;
            }
        }

        public interface IData
        {
            // not actually using this currently...
            string IdText { get; set; }
        }

        protected override IList<IDecoration> GenerateDecorations()
        {
            GameObject gameObject = Object
                    .Instantiate<GameObject>(Prefabs
                        .ComponentDecorations
                        .LabelText);
            this._textMesh = gameObject.GetComponent<TextMeshPro>();
            return (IList<IDecoration>) new Decoration[]{
                new Decoration()
                {
                    //                                        THE BOOTY
                    LocalPosition = new Vector3(-0.5f, -0.25f, -0.16f),
                    LocalRotation = Quaternion.Euler(0f, 0f, 0f),
                    DecorationObject = gameObject
                }
            };
        }

        protected override void DataUpdate()
        {
            if (_textMesh == null) return;

            var text = EmitterId ?? "???";
            if (_textMesh.text == text)
            {
                return;
            }
            _textMesh.text = text;

            _textMesh.fontSizeMax = 0.7f;
            _textMesh.font = Fonts.NotoMono;
            _textMesh.color = (Color) FEM01.DefaultColor.WithOpacity();
            _textMesh.fontSharedMaterial = Materials.NotoSansMono_WorldSpace;
            _textMesh.ForceMeshUpdate(false, false);
            _textMesh.GetRectTransform().sizeDelta = new Vector2(1f, 1f);

            _textMesh.horizontalAlignment =
                LabelAlignmentHorizontal.Center.ToTmpEnum();
            _textMesh.verticalAlignment =
                LabelAlignmentVertical.Middle.ToTmpEnum();
            _textMesh.enabled = false;
            _textMesh.enabled = true;
        }

        protected override void SetDataDefaultValues()
        {
            Data.IdText = "FE000";
        }
    }
}
