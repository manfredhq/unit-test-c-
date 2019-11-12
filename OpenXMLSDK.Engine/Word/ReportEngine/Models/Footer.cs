﻿namespace OpenXMLSDK.Engine.Word.ReportEngine.Models
{
    /// <summary>
    /// Model for footer
    /// </summary>
    public class Footer : BaseElement
    {
        /// <summary>
        /// Footer type 
        /// </summary>
        public HeaderFooterValues Type { get; set; } = HeaderFooterValues.Default;

        /// <summary>
        /// Constructor
        /// </summary>
        public Footer()
            : base(typeof(Footer).Name)
        {
        }
    }
}
