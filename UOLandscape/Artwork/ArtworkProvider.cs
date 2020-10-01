using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using UOLandscape.IO;

namespace UOLandscape.Artwork
{
    //##############################################################
    //##############################################################
    // TESTCLASS - JUST FOR TESTING
    //##############################################################
    //##############################################################
    internal sealed class ArtworkProvider
    {
        private static ILogger _logger;
        private static bool _isInitialized;
        private static FileIndexBase _fileIndex;
        private static bool _isUOPFileIndex;
        private static string _clientPath;

        public ArtworkProvider(ILogger logger, bool isuopfileindex, string clientpath)
        {
            _logger = logger;
            _isUOPFileIndex = isuopfileindex;
            _clientPath = clientpath;
        }

        public static int Length
        {
            get
            {
                if (!_isInitialized)
                {
                    Initialize();
                }

                return _fileIndex.Length;
            }
        }

        public static void Initialize()
        {
            if (_isUOPFileIndex)
            {
                _fileIndex = CreateFileIndex("artLegacyMUL.uop", 0x10000, false, ".tga");
                _logger.Debug(_fileIndex.FilesExist.ToString());
            }
            else
            {
                _fileIndex = CreateFileIndex("artidx.mul", "art.mul");
            }

            _isInitialized = true;
        }

        public static string GetPath(string filename, params object[] args)
        {
            var path = Path.Combine(_clientPath, string.Format(filename, args));

            if (!File.Exists(path))
            {
                _logger.Error($"{path} does not exists.");
            }

            return path;
        }

        private static FileIndexBase CreateFileIndex(string uopFile, int length, bool hasExtra, string extension)
        {
            uopFile = GetPath(uopFile);

            FileIndexBase fileIndex = new UOPFileIndex(uopFile, length, hasExtra, extension);

            if (!fileIndex.FilesExist)
            {
                _logger.Error($"FileIndex was created but {Path.GetFileName(uopFile)} was missing from {_clientPath}");
            }

            fileIndex.Open();

            return fileIndex;
        }

        private static FileIndexBase CreateFileIndex(string idxFile, string mulFile)
        {
            idxFile = GetPath(idxFile);
            mulFile = GetPath(mulFile);

            FileIndexBase fileIndex = new MULFileIndex(idxFile, mulFile);

            if (!fileIndex.FilesExist)
            {
                _logger.Error(
                    $"FileIndex was created but 1 or more files do not exist.  Either {Path.GetFileName(idxFile)} or {Path.GetFileName(mulFile)} were missing from {_clientPath}");
            }

            fileIndex.Open();

            return fileIndex;
        }

        public static unsafe Texture GetLand(GraphicsDevice graphicsDevice, int index)
        {
            index &= 0x3FFF;

            using (var stream = _fileIndex.Seek(index, out _, out _))
            using (var bin = new BinaryReader(stream))
            {
                var texture = new Texture2D(graphicsDevice, 44, 44, false, SurfaceFormat.Bgra5551);
                var buffer = new ushort[44 * 44];

                var xOffset = 21;
                var xRun = 2;

                fixed (ushort* start = buffer)
                {
                    var ptr = start;
                    var delta = texture.Width;

                    for (var y = 0; y < 22; ++y, --xOffset, xRun += 2, ptr += delta)
                    {
                        var cur = ptr + xOffset;
                        var end = cur + xRun;

                        while (cur < end)
                        {
                            *cur++ = (ushort) (bin.ReadUInt16() | 0x8000);
                        }
                    }

                    xOffset = 0;
                    xRun = 44;

                    for (var y = 0; y < 22; ++y, ++xOffset, xRun -= 2, ptr += delta)
                    {
                        var cur = ptr + xOffset;
                        var end = cur + xRun;

                        while (cur < end)
                        {
                            *cur++ = (ushort) (bin.ReadUInt16() | 0x8000);
                        }
                    }
                }

                texture.SetData(buffer);

                return texture;
            }
        }

        public static Task<Texture> GetLandAsync(GraphicsDevice graphicsDevice, int index)
        {
            return Task.FromResult(GetLand(graphicsDevice, index));
        }

        public static unsafe Texture2D GetStatic(GraphicsDevice graphicsDevice, int index)
        {
            index += 0x4000;
            index &= 0xFFFF;

            using (var stream = _fileIndex.Seek(index, out _, out _))
            using (var bin = new BinaryReader(stream))
            {
                bin.ReadInt32(); // Unknown

                int width = bin.ReadInt16();
                int height = bin.ReadInt16();

                if (width <= 0 || height <= 0)
                {
                    return null;
                }

                var lookups = new int[height];
                var lookupStart = (int) bin.BaseStream.Position + (height * 2);

                for (var i = 0; i < height; ++i)
                {
                    lookups[i] = (lookupStart + (bin.ReadUInt16() * 2));
                }

                var texture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Bgra5551);
                var buffer = new ushort[width * height];

                fixed (ushort* start = buffer)
                {
                    var ptr = start;
                    var delta = texture.Width;

                    for (var y = 0; y < height; ++y, ptr += delta)
                    {
                        bin.BaseStream.Seek(lookups[y], SeekOrigin.Begin);

                        var cur = ptr;

                        int xOffset, xRun;

                        while (((xOffset = bin.ReadUInt16()) + (xRun = bin.ReadUInt16())) != 0)
                        {
                            cur += xOffset;
                            var end = cur + xRun;

                            while (cur < end)
                            {
                                *cur++ = (ushort) (bin.ReadUInt16() ^ 0x8000);
                            }
                        }
                    }
                }

                texture.SetData(buffer);

                return texture;
            }
        }

        public static Task<Texture2D> GetStaticAsync(GraphicsDevice graphicsDevice, int index)
        {
            return Task.FromResult(GetStatic(graphicsDevice, index));
        }
    }
}