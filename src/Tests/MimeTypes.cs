#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Formplot.Tests
{
    public static class MimeTypes
    {
        /// <summary>
        /// MIME-Type for form plot data (CMM Exchange Format).
        /// </summary>
        public const string Formplot = "application/x-zeiss-piweb-formplot";

        /// <summary>
        /// Extension for form plot data (CMM Exchange Format).
        /// </summary>
        public const string FormplotFileExtension = ".pltx";

        /// <summary>
        /// MIME-Type for name-value-pair lists (CMM Exchange Format).
        /// </summary>
        public const string Properties = "application/x-zeiss-piweb-properties";

        /// <summary>
        /// Extension for name-value-pair lists.
        /// </summary>
        public const string PropertiesFileExtension = ".prpx";
    }
}