using System;

namespace UOLandscape.Client
{
    class InvalidClientVersion : Exception
    {
        public InvalidClientVersion(string msg) : base(msg)
        {

        }
    }

    class InvalidClientDirectory : Exception
    {
        public InvalidClientDirectory(string msg) : base(msg)
        {

        }
    }
}
