﻿
namespace OpenXMLSDK.Engine.Word.ReportEngine.Models
{
    public class TemplateModel : BaseElement
    {
        /// <summary>
        /// Id of the template
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateModel()
            : base(typeof(TemplateModel).Name)
        {
        }
    }
}
