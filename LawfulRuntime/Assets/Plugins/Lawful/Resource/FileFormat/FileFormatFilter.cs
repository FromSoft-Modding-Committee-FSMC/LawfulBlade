using System;

namespace Lawful.Resource.FileFormat
{
    /// <summary>
    /// File Format Filter is used for scanning formats which implement base level capabilities<br/>
    /// </summary>
    [Flags]
    public enum FileFormatFilter : uint
    {
        /// <summary>Include importable formats</summary>
        Importable   = (1 << 0),

        /// <summary>Include exportable formats</summary>
        Exportable   = (1 << 1),

        /// <summary>Include deprecated formats</summary>
        Deprecated   = (1 << 2),

        /// <summary>Include experimental formats</summary>
        Experimental = (1 << 3),

        /// <summary>Include all formats</summary>
        All = Importable | Exportable | Deprecated | Experimental,

        /// <summary>Include all safe formats</summary>
        AllSafe = Importable | Exportable,
    }
}

