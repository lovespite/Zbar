using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZBar
{
    /// <summary>
    /// Mid-level image scanner interface. reads barcodes from 2-D images
    /// </summary>
    public class ImageScanner : IDisposable
    {
        private IntPtr handle = IntPtr.Zero;
        private bool cache = false;

        /// <summary>
        /// Create a new ImageScanner
        /// </summary>
        public ImageScanner()
        {
            this.handle = NativeZBar.zbar_image_scanner_create();
            if (this.handle == IntPtr.Zero)
                throw new Exception("Failed to create an underlying image_scanner!");
        }

        #region Wrapper methods

        /// <summary>
        /// Scan an image for symbols
        /// </summary>
        /// <remarks>
        /// Once an image have been scanned the result will be associated with the image.
        /// Use image.Symbols to access the symbol list.
        /// </remarks>
        /// <param name="image">
        /// A <see cref="ZbImage"/> to be scanned
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/> number of symbols decoded on the image.
        /// </returns>
        public int Scan(ZbImage image)
        {
            int count = NativeZBar.zbar_scan_image(this.handle, image.Handle);
            if (count < 0)
                throw new Exception("Image scanning failed!");
            return count;
        }

        /// <summary>
        /// Scan an image for symbols
        /// </summary>
        /// <param name="image">
        /// A <see cref="System.Drawing.Image"/> to be scanned for symbols
        /// </param>
        /// <remarks>
        /// This method convert the image to the appropriate format,
        /// and release the converted image immidiately. While copying
        /// all the symbols to a list.
        /// </remarks>
        /// <returns>
        /// A list of symbols found in the image
        /// </returns>
        public List<Symbol> Scan(System.Drawing.Image image, bool noConvert = false)
        {
            if (noConvert)
            {
                using (var zimg = ZbImage.CreateNoConvert(image))
                {
                    this.Scan(zimg);
                    return new List<Symbol>(zimg.Symbols);
                }
            }
            else
            { 
                using (ZbImage zimg = new ZbImage(image))
                using (ZbImage grey = zimg.Convert(ZbImage.FourCC('Y', '8', '0', '0')))
                {
                    this.Scan(grey);
                    return new List<Symbol>(grey.Symbols);
                }
            }
        }

        /// <value>
        /// Enable or disable the inter-image result cache (default disabled).
        /// </value>
        /// <remarks>
        /// Mostly useful for scanning video frames, the cache filters duplicate results from consecutive images,
        /// while adding some consistency checking and hysteresis to the results.
        /// this interface also clears the cache.
        /// </remarks>
        public bool Cache
        {
            get
            {
                return this.cache;
            }
            set
            {
                NativeZBar.zbar_image_scanner_enable_cache(this.handle, value ? 1 : 0);
                this.cache = value;
            }
        }

        /// <summary>
        /// Set config for indicated symbology (0 for all) to specified value.
        /// </summary>
        public void SetConfiguration(SymbolType symbology, Config config, int value)
        {
            if (NativeZBar.zbar_image_scanner_set_config(this.handle, (int)symbology, (int)config, value) != 0)
                throw new Exception("Failed to set configuration");
        }

        #endregion Wrapper methods

        #region IDisposable Implementation

        //This pattern for implementing IDisposable is recommended by:
        //Framework Design Guidelines, 2. Edition, Section 9.4

        /// <summary>
        /// Dispose this object
        /// </summary>
        /// <remarks>
        /// This boolean disposing parameter here ensures that objects with a finalizer is not disposed,
        /// this is method is invoked from the finalizer. Do overwrite, and call, this method in base
        /// classes if you use any unmanaged resources.
        /// </remarks>
        /// <param name="disposing">
        /// A <see cref="System.Boolean"/> False if called from the finalizer, True if called from Dispose.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.handle != IntPtr.Zero)
            {
                NativeZBar.zbar_image_scanner_destroy(this.handle);
                this.handle = IntPtr.Zero;
            }
            if (disposing)
            {
                //Release finalizable resources, at the moment none.
            }
        }

        /// <summary>
        /// Release resources held by this object
        /// </summary>
        public void Dispose()
        {
            //We're disposing this object and can release objects that are finalizable
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalize this object
        /// </summary>
        ~ImageScanner()
        {
            //Dispose this object, but do NOT release finalizable objects, we don't know in which order
            //these are release and they may already be finalized.
            this.Dispose(false);
        }

        #endregion IDisposable Implementation

    }

    /// <summary>
    /// Configuration parameters for the image scanner
    /// </summary>
    public enum Config
    {
        /// <summary>
        /// Enable symbology/feature
        /// </summary>
        Enable = 0,

        /// <summary>
        /// Enable check digit when optional
        /// </summary>
        AddCheck,

        /// <summary>
        /// Return check digit when present
        /// </summary>
        EmitCheck,

        /// <summary>
        /// Enable full ASCII character set
        /// </summary>
        ASCII,

        /// <summary>
        /// Number of boolean decoder configs
        /// </summary>
        Num,

        /// <summary>
        /// Minimum data length for valid decode
        /// </summary>
        MinimumLength = 0x20,

        /// <summary>
        /// Maximum data length for valid decode
        /// </summary>
        MaximumLength,

        /// <summary>
        /// Enable scanner to collect position data
        /// </summary>
        Position = 0x80,

        /// <summary>
        /// Image scanner vertical scan density
        /// </summary>
        XDensity = 0x100,

        /// <summary>
        /// Image scanner horizontical scan density
        /// </summary>
        YDensity
    }
}