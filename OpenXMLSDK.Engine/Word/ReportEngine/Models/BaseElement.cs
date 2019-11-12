﻿using System.Collections.Generic;

namespace OpenXMLSDK.Engine.Word.ReportEngine.Models
{
    /// <summary>
    /// Base Element for word template
    /// </summary>
    public class BaseElement : BaseModel
    {
        /// <summary>
        /// Is the element visible
        /// </summary>
        public bool Show { get; set; } = true;

        /// <summary>
        /// Key from context used to determine element visibility
        /// </summary>
        public string ShowKey { get; set; }

        /// <summary>
        /// Childrens
        /// </summary>
        public List<BaseElement> ChildElements { get; set; } = new List<BaseElement>();

        /// <summary>
        /// Font
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Font size
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        /// Font color in hex value (RRGGBB format)
        /// </summary>
        public string FontColor { get; set; }

        /// <summary>
        /// Shading color in hex value (RRGGBB format)
        /// </summary>
        public string Shading { get; set; }

        /// <summary>
        /// Is text contained in current element bold
        /// </summary>
        public bool? Bold { get; set; }

        /// <summary>
        /// Key from context used to determine element boldness
        /// </summary>
        public string BoldKey { get; set; }

        /// <summary>
        /// Is text contained in current element Italic
        /// </summary>
        public bool? Italic { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public BaseElement(string type) : base(type)
        {
        }

        /// <summary>
        /// Inherits font properties from parent if local values are null
        /// </summary>
        /// <param name="parent"></param>
        public void InheritFromParent(BaseElement parent)
        {
            if (string.IsNullOrEmpty(FontColor))
                FontColor = parent.FontColor;
            if (string.IsNullOrEmpty(FontName))
                FontName = parent.FontName;
            if (string.IsNullOrEmpty(FontSize))
                FontSize = parent.FontSize;
            if (string.IsNullOrEmpty(Shading))
                Shading = parent.Shading;
            if (!Bold.HasValue)
                Bold = parent.Bold;
            if (!Italic.HasValue)
                Italic = parent.Italic;
        }
    }
}
