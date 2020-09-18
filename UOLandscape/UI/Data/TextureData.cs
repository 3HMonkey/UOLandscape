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
        public int TextureId;
        public IntPtr? FontTextureId;
        public Dictionary<IntPtr, Texture2D> Loaded;

        public int GetTextureId()
        {
            return TextureId++;
        }

        public TextureData()
        {
            Loaded = new Dictionary<IntPtr, Texture2D>();
        }
    }
}
