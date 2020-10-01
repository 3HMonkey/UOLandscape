using System;
using System.Runtime.InteropServices;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UOLandscape.UI.Data;
using UOLandscape.UI.Exceptions;

namespace UOLandscape.UI
{
    public sealed class ImGuiRenderer
    {

        private readonly ImGuiIndexData _imGuiIndex;
        private readonly ImGuiVertexData _imGuiVertex;
        private readonly ImguiInputHandler _imguiInputHandler;
        private readonly ImGuiTextureData _imGuiTextureData;
        private readonly BasicEffect _effect;
        private readonly RasterizerState _rasterizerState;

        public Game Owner { get; }
        public GraphicsDevice GraphicsDevice => Owner.GraphicsDevice;

        public void BeginLayout(GameTime gameTime)
        {
            ImGui.GetIO().DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _imguiInputHandler.Update(GraphicsDevice);

            ImGui.NewFrame();
        }

        public void EndLayout()
        {
            ImGui.Render();
            RenderDrawData(ImGui.GetDrawData());
        }

        public IntPtr BindTexture(Texture2D texture)
        {
            var id = new IntPtr(_imGuiTextureData.GetTextureId());
            _imGuiTextureData.Loaded.Add(id, texture);

            return id;
        }

        public void UnbindTexture(IntPtr textureId)
        {
            _imGuiTextureData.Loaded.Remove(textureId);
        }

        public ImGuiRenderer(Game owner)
        {
            Owner = owner;
            _effect = new BasicEffect(owner.GraphicsDevice);
            _imGuiTextureData = new ImGuiTextureData();
            _rasterizerState = RasterizerState.CullNone;
            _imguiInputHandler = new ImguiInputHandler();
            _imGuiVertex = new ImGuiVertexData();
            _imGuiIndex = new ImGuiIndexData();
        }

        public ImGuiRenderer Initialize()
        {
            var context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);

            _imguiInputHandler.Initialize(Owner);
            return this;
        }

        public unsafe ImGuiRenderer RebuildFontAtlas()
        {
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);

            var pixels = new byte[width * height * bytesPerPixel];
            Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length);

            var texture = new Texture2D(GraphicsDevice, width, height, false, SurfaceFormat.Color);
            texture.SetData(pixels);

            if (_imGuiTextureData.FontTextureId.HasValue)
            {
                UnbindTexture(_imGuiTextureData.FontTextureId.Value);
            }

            _imGuiTextureData.FontTextureId = BindTexture(texture);

            io.Fonts.SetTexID(_imGuiTextureData.FontTextureId.Value);
            io.Fonts.ClearTexData();
            return this;
        }

        private void RenderDrawData(ImDrawDataPtr drawData)
        {
            var lastViewport = GraphicsDevice.Viewport;
            var lastScissorRect = GraphicsDevice.ScissorRectangle;

            GraphicsDevice.BlendFactor = Color.White;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.RasterizerState = _rasterizerState;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; //ADD THIS LINE

            drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);

            GraphicsDevice.Viewport = new Viewport(
                0,
                0,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight);

            UpdateBuffers(drawData);
            RenderCommandLists(drawData);

            GraphicsDevice.Viewport = lastViewport;
            GraphicsDevice.ScissorRectangle = lastScissorRect;
        }

        private void RenderCommandLists(ImDrawDataPtr draw_data)
        {
            GraphicsDevice.SetVertexBuffer(_imGuiVertex.Buffer);
            GraphicsDevice.Indices = _imGuiIndex.Buffer;

            var vertexOffset = 0;
            var indexOffset = 0;
            for (var i = 0; i < draw_data.CmdListsCount; ++i)
            {
                var commandList = draw_data.CmdListsRange[i];
                for (var commandIndex = 0; commandIndex < commandList.CmdBuffer.Size; ++commandIndex)
                {
                    var drawCommand = commandList.CmdBuffer[commandIndex];

                    if (!_imGuiTextureData.Loaded.ContainsKey(drawCommand.TextureId))
                    {
                        throw new ImGuiMissingLoadedTextureKeyException(drawCommand.TextureId);
                    }

                    GraphicsDevice.ScissorRectangle = GenerateScissorRect(drawCommand);
                    var effect = UpdateEffect(_imGuiTextureData.Loaded[drawCommand.TextureId]);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        DrawPrimitives(vertexOffset, indexOffset, commandList, drawCommand);
                    }

                    indexOffset += (int)drawCommand.ElemCount;
                }

                vertexOffset += commandList.VtxBuffer.Size;
            }
        }

        private Rectangle GenerateScissorRect(ImDrawCmdPtr draw_command)
        {
            return new Rectangle(
                (int)draw_command.ClipRect.X,
                (int)draw_command.ClipRect.Y,
                (int)(draw_command.ClipRect.Z - draw_command.ClipRect.X),
                (int)(draw_command.ClipRect.W - draw_command.ClipRect.Y));
        }

        private void DrawPrimitives(int vertexOffset, int indexOffset, ImDrawListPtr commandList, ImDrawCmdPtr drawCommand)
        {
            GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                vertexOffset,
                0,
                commandList.VtxBuffer.Size,
                indexOffset,
                (int)(drawCommand.ElemCount / 3));
        }

        private Effect UpdateEffect(Texture2D texture)
        {
            var io = ImGui.GetIO();
            var displaySize = io.DisplaySize;

            var offset = 0f;
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.Identity;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(offset, displaySize.X + offset, displaySize.Y + offset, offset, -1.0f, 1.0f);
            _effect.TextureEnabled = true;
            _effect.Texture = texture;
            _effect.VertexColorEnabled = true;

            return _effect;
        }

        private unsafe void UpdateBuffers(ImDrawDataPtr drawData)
        {
            if (drawData.TotalVtxCount == 0)
            {
                return;
            }

            if (drawData.TotalVtxCount > _imGuiVertex.BufferSize)
            {
                _imGuiVertex.Buffer?.Dispose();
                _imGuiVertex.BufferSize = (int)(drawData.TotalVtxCount * 1.5f);
                _imGuiVertex.Buffer = new VertexBuffer(GraphicsDevice, DrawVertexDeclaration.Declaration, _imGuiVertex.BufferSize, BufferUsage.None);
                _imGuiVertex.Data = new byte[_imGuiVertex.BufferSize * DrawVertexDeclaration.Size];
            }

            if (drawData.TotalIdxCount > _imGuiIndex.BufferSize)
            {
                _imGuiIndex.Buffer?.Dispose();

                _imGuiIndex.BufferSize = (int)(drawData.TotalIdxCount * 1.5f);
                _imGuiIndex.Buffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, _imGuiIndex.BufferSize, BufferUsage.None);
                _imGuiIndex.Data = new byte[_imGuiIndex.BufferSize * sizeof(ushort)];
            }

            var vertexOffset = 0;
            var indexOffset = 0;

            for (var i = 0; i < drawData.CmdListsCount; ++i)
            {
                var commands = drawData.CmdListsRange[i];
                fixed (void* vtxDstPtr = &_imGuiVertex.Data[vertexOffset * DrawVertexDeclaration.Size])
                fixed (void* idxDstPtr = &_imGuiIndex.Data[indexOffset * sizeof(ushort)])
                {
                    Buffer.MemoryCopy((void*)commands.VtxBuffer.Data, vtxDstPtr, _imGuiVertex.Data.Length, commands.VtxBuffer.Size * DrawVertexDeclaration.Size);
                    Buffer.MemoryCopy((void*)commands.IdxBuffer.Data, idxDstPtr, _imGuiIndex.Data.Length, commands.IdxBuffer.Size * sizeof(ushort));
                }

                vertexOffset += commands.VtxBuffer.Size;
                indexOffset += commands.IdxBuffer.Size;
            }

            // Copy the managed byte arrays to the gpu vertex- and index buffers
            _imGuiVertex.Buffer.SetData(_imGuiVertex.Data, 0, drawData.TotalVtxCount * DrawVertexDeclaration.Size);
            _imGuiIndex.Buffer.SetData(_imGuiIndex.Data, 0, drawData.TotalIdxCount * sizeof(ushort));
        }
    }
}