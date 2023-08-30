using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using Zbar;

/// <summary>
/// ZBar is a library for reading bar codes from video streams
/// </summary>
namespace ZBar
{
    internal static class NativeZBar
    {
        // !important! vc redist 2013 is required to run this library!!
        // https://www.microsoft.com/en-us/download/details.aspx?id=40784

        private const string DllName = "libzbar-0.dll";

        [DllImport(DllName)]
        public static extern IntPtr _zbar_error_string(IntPtr obj, int verbosity);

        [DllImport(DllName)]
        public static extern int _zbar_get_error_code(IntPtr obj);

        [DllImport(DllName)]
        public static extern unsafe int zbar_version(uint* major, uint* minor);

        #region Extern C functions 
        [DllImport(DllName)]
        public static extern void zbar_symbol_ref(IntPtr symbol, int refs);
        [DllImport(DllName)]
        public static extern int zbar_symbol_get_type(IntPtr symbol);
        [DllImport(DllName)]
        public static extern IntPtr zbar_symbol_get_data(IntPtr symbol);
        [DllImport(DllName)]
        public static extern uint zbar_symbol_get_data_length(IntPtr symbol);
        [DllImport(DllName)]
        public static extern int zbar_symbol_get_quality(IntPtr symbol);
        [DllImport(DllName)]
        public static extern int zbar_symbol_get_count(IntPtr symbol);
        [DllImport(DllName)]
        public static extern uint zbar_symbol_get_loc_size(IntPtr symbol);
        [DllImport(DllName)]
        public static extern int zbar_symbol_get_loc_x(IntPtr symbol, uint index);
        [DllImport(DllName)]
        public static extern int zbar_symbol_get_loc_y(IntPtr symbol, uint index);
        [DllImport(DllName)]
        internal static extern IntPtr zbar_symbol_next(IntPtr symbol);
        [DllImport(DllName)]
        public static extern IntPtr zbar_symbol_xml(IntPtr symbol, out IntPtr buffer, out uint buflen);

        #endregion Extern C functions

        #region Extern C functions 
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_scanner_create();
        [DllImport(DllName)]
        public static extern void zbar_image_scanner_destroy(IntPtr scanner);
        public delegate void zbar_image_data_handler(IntPtr image, IntPtr userdata);
        [DllImport(DllName)]
        public static extern zbar_image_data_handler zbar_image_scanner_set_data_handler(IntPtr scanner, zbar_image_data_handler handler, IntPtr userdata);
        [DllImport(DllName)]
        public static extern int zbar_image_scanner_set_config(IntPtr scanner, int symbology, int config, int val);
        [DllImport(DllName)]
        public static extern void zbar_image_scanner_enable_cache(IntPtr scanner, int enable);
        [DllImport(DllName)]
        public static extern int zbar_scan_image(IntPtr scanner, IntPtr image);

        #endregion Extern C functions

        #region Extern C functions 
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_create();
        [DllImport(DllName)]
        public static extern void zbar_image_destroy(IntPtr image);
        [DllImport(DllName)]
        public static extern void zbar_image_ref(IntPtr image, int refs);
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_convert(IntPtr image, uint format);
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_convert_resize(IntPtr image, uint format, uint width, uint height);
        [DllImport(DllName)]
        public static extern uint zbar_image_get_format(IntPtr image);
        [DllImport(DllName)]
        public static extern uint zbar_image_get_sequence(IntPtr image);
        [DllImport(DllName)]
        public static extern uint zbar_image_get_width(IntPtr image);
        [DllImport(DllName)]
        public static extern uint zbar_image_get_height(IntPtr image);
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_get_data(IntPtr image);
        [DllImport(DllName)]
        public static extern uint zbar_image_get_data_length(IntPtr img);
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_first_symbol(IntPtr image);
        [DllImport(DllName)]
        public static extern void zbar_image_set_format(IntPtr image, uint format);
        [DllImport(DllName)]
        public static extern void zbar_image_set_sequence(IntPtr image, uint sequence_num);
        [DllImport(DllName)]
        public static extern void zbar_image_set_size(IntPtr image, uint width, uint height);
        public delegate void zbar_image_cleanup_handler(IntPtr image);
        [DllImport(DllName)]
        public static extern void zbar_image_set_data(IntPtr image, IntPtr data, uint data_byte_length, zbar_image_cleanup_handler cleanup_handler);
        [DllImport(DllName)]
        public static extern void zbar_image_free_data(IntPtr image);
        [DllImport(DllName)]
        public static extern void zbar_image_set_userdata(IntPtr image, IntPtr userdata);
        [DllImport(DllName)]
        public static extern IntPtr zbar_image_get_userdata(IntPtr image);

        #endregion Extern C functions
    }
}