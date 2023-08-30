using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ZBar
{
    /// <summary>
    /// Representation of a decoded symbol
    /// </summary>
    /// <remarks>This symbol does not hold any references to unmanaged resources.</remarks>
    public class Symbol
    {
        /// <summary>
        /// Initialize a symbol from pointer to a symbol
        /// </summary>
        /// <param name="symbol">
        /// Pointer to a symbol
        /// </param>
        internal Symbol(IntPtr symbol)
        {
            if (symbol == IntPtr.Zero)
                throw new Exception("Can't initialize symbol from null pointer.");

            //Get data from the symbol
            IntPtr pData = NativeZBar.zbar_symbol_get_data(symbol);
            int length = (int)NativeZBar.zbar_symbol_get_data_length(symbol);
            this.data = Marshal.PtrToStringAnsi(pData, length);

            //Get the other fields
            this.type = (SymbolType)NativeZBar.zbar_symbol_get_type(symbol);
            this.quality = NativeZBar.zbar_symbol_get_quality(symbol);
            this.count = NativeZBar.zbar_symbol_get_count(symbol);
            this.points = new PointF[4];

            for (uint i = 0; i < 4; i++)
            {
                this.points[i] = new Point
                {
                    X = NativeZBar.zbar_symbol_get_loc_x(symbol, i),
                    Y = NativeZBar.zbar_symbol_get_loc_y(symbol, i),
                };
            }
        }

        private string data;
        private int quality;
        private int count;
        private PointF[] points;
        private SymbolType type;

        public override string ToString()
        {
            return this.data;
        }

        #region Public properties

        public PointF[] Points => this.points;

        /// <value>
        /// Retrieve current cache count.
        /// </value>
        /// <remarks>
        /// When the cache is enabled for the image_scanner this provides inter-frame reliability and redundancy information for video streams.
        /// 	&lt; 0 if symbol is still uncertain.
        /// 	0 if symbol is newly verified.
        /// 	&gt; 0 for duplicate symbols
        /// </remarks>
        public int Count => this.count;

        /// <value>
        /// Data decoded from symbol.
        /// </value>
        public string Data => this.data;

        /// <value>
        /// Get a symbol confidence metric.
        /// </value>
        /// <remarks>
        /// An unscaled, relative quantity: larger values are better than smaller values, where "large" and "small" are application dependent.
        /// </remarks>
        public int Quality => this.quality;

        /// <value>
        /// Type of decoded symbol
        /// </value>
        public SymbolType Type => this.type;

        #endregion Public properties


    }

    /// <summary>
    /// Different symbol types
    /// </summary>
    [Flags]
    public enum SymbolType
    {
        /// <summary>
        /// No symbol decoded
        /// </summary>
        None = 0,

        /// <summary>
        /// Intermediate status
        /// </summary>
        Partial = 1,

        /// <summary>
        /// EAN-8
        /// </summary>
        EAN8 = 8,

        /// <summary>
        /// UPC-E
        /// </summary>
        UPCE = 9,

        /// <summary>
        /// ISBN-10 (from EAN-13)
        /// </summary>
        ISBN10 = 10,

        /// <summary>
        /// UPC-A
        /// </summary>
        UPCA = 12,

        /// <summary>
        /// EAN-13
        /// </summary>
        EAN13 = 13,

        /// <summary>
        /// ISBN-13 (from EAN-13)
        /// </summary>
        ISBN13 = 14,

        /// <summary>
        /// Interleaved 2 of 5.
        /// </summary>
        I25 = 25,

        /// <summary>
        /// Code 39.
        /// </summary>
        CODE39 = 39,

        /// <summary>
        /// PDF417
        /// </summary>
        PDF417 = 57,

        /// <summary>
        /// QR Code
        /// </summary>
        QRCODE = 64,

        /// <summary>
        /// Code 128
        /// </summary>
        CODE128 = 128,

        /// <summary>
        /// mask for base symbol type
        /// </summary>
        Symbole = 0x00ff,

        /// <summary>
        /// 2-digit add-on flag
        /// </summary>
        Addon2 = 0x0200,

        /// <summary>
        /// 5-digit add-on flag
        /// </summary>
        Addon5 = 0x0500,

        /// <summary>
        /// add-on flag mask
        /// </summary>
        Addon = 0x0700
    }
}