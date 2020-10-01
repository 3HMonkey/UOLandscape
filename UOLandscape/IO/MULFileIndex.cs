using System.Collections.Generic;
using System.IO;

namespace UOLandscape.IO
{
    class MULFileIndex : FileIndexBase
    {
        private readonly string _indexPath;

        public MULFileIndex(string idxFile, string mulFile)
            : base(mulFile)
        {
            _indexPath = idxFile;
        }

        public override bool FilesExist
        {
            get { return File.Exists(_indexPath) && base.FilesExist; }
        }

        protected override FileIndexEntry[] ReadEntries()
        {
            var entries = new List<FileIndexEntry>();

            var length = (int) ((new FileInfo(_indexPath).Length / 3) / 4);

            using( var index = new FileStream(_indexPath, FileMode.Open, FileAccess.Read, FileShare.Read) )
            {
                var bin = new BinaryReader(index);

                var count = (int) (index.Length / 12);

                for( var i = 0; i < count && i < length; ++i )
                {
                    var entry = new FileIndexEntry
                    {
                        Lookup = bin.ReadInt32(),
                        Length = bin.ReadInt32(),
                        Extra = bin.ReadInt32()
                    };

                    entries.Add(entry);
                }

                for( var i = count; i < length; ++i )
                {
                    var entry = new FileIndexEntry
                    {
                        Lookup = -1,
                        Length = -1,
                        Extra = -1
                    };

                    entries.Add(entry);
                }
            }

            return entries.ToArray();
        }
    }
}
