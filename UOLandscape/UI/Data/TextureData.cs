using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace UOLandscape.UI.Data
{
    /// <summary>
    /// Contains the GUIRenderer's texture data element.
    /// </summary>
    public class TextureData
    {
        private int _textureId;

        public IntPtr? FontTextureId;

        public Dictionary<IntPtr, Texture2D> Loaded { get; }

        public int GetTextureId()
        {
            return _textureId++;
        }

        public TextureData()
        {
            Loaded = new Dictionary<IntPtr, Texture2D>();
        }
    }
}
