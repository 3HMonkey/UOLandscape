using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace UOLandscape.UI.Data
{
    public class ImGuiTextureData
    {
        private int _textureId;

        public IntPtr? FontTextureId;

        public Dictionary<IntPtr, Texture2D> Loaded { get; }

        public int GetTextureId()
        {
            return _textureId++;
        }

        public ImGuiTextureData()
        {
            Loaded = new Dictionary<IntPtr, Texture2D>();
        }
    }
}
