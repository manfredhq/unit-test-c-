﻿using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXMLSDK.Engine.ReportEngine.DataContext;
using OpenXMLSDK.Engine.Word.ReportEngine.Models;

namespace OpenXMLSDK.Engine.Word.ReportEngine.Renders
{
    public static class UniformGridExtensions
    {
        /// <summary>
        /// Render a table element
        /// </summary>
        /// <param name="uniformGrid"></param>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DocumentFormat.OpenXml.Wordprocessing.Table Render(this UniformGrid uniformGrid, Document document, OpenXmlElement parent, ContextModel context, OpenXmlPart documentPart, IFormatProvider formatProvider)
        {
            context.ReplaceItem(uniformGrid, formatProvider);

            if (!string.IsNullOrEmpty(uniformGrid.DataSourceKey) && context.ExistItem<DataSourceModel>(uniformGrid.DataSourceKey))
            {
                var datasource = context.GetItem<DataSourceModel>(uniformGrid.DataSourceKey);

                if (datasource != null && datasource.Items.Count > 0)
                {
                    var createdTable = TableExtensions.CreateTable(document, uniformGrid, context, documentPart, formatProvider);
                    var wordTable = createdTable.Item1;
                    var tableLook = createdTable.Item2;

                    // Before rows :
                    TableExtensions.ManageBeforeAfterRows(document, uniformGrid, uniformGrid.BeforeRows, wordTable, context, documentPart, formatProvider);

                    // Table of cells :
                    List<List<ContextModel>> rowsContentContexts = new List<List<ContextModel>>();

                    int i = 0;
                    var lastCellsRow = new List<ContextModel>();

                    // add content rows
                    foreach (var item in datasource.Items)
                    {
                        if (i > 0 && i % uniformGrid.ColsWidth.Length == 0)
                        {
                            // end of line :
                            rowsContentContexts.Add(lastCellsRow);
                            lastCellsRow = new List<ContextModel>();
                        }
                        lastCellsRow.Add(item);

                        i++;
                    }

                    if (i % uniformGrid.ColsWidth.Length == 0)
                        rowsContentContexts.Add(lastCellsRow);

                    if (datasource.Items.Count % uniformGrid.ColsWidth.Length > 0)
                    {
                        var acountAddNullItems = uniformGrid.ColsWidth.Length - (datasource.Items.Count % uniformGrid.ColsWidth.Length);
                        if (acountAddNullItems > 0)
                        {
                            for (int j = 0; j < acountAddNullItems; j++)
                                lastCellsRow.Add(new ContextModel());
                            rowsContentContexts.Add(lastCellsRow);
                        }
                    }

                    // Now we create all row :
                    i = 0;
                    foreach (var rowContentContext in rowsContentContexts)
                    {
                        var row = new Row
                        {
                            CantSplit = uniformGrid.CantSplitRows,
                        };
                        row.InheritFromParent(uniformGrid);

                        wordTable.AppendChild(row.Render(document, wordTable, context, rowContentContext, uniformGrid.CellModel, documentPart, false, (i % 2 == 1), formatProvider));

                        i++;
                    }

                    // After rows :
                    TableExtensions.ManageBeforeAfterRows(document, uniformGrid, uniformGrid.AfterRows, wordTable, context, documentPart, formatProvider);

                    TableExtensions.ManageFooterRow(document, uniformGrid, wordTable, tableLook, context, documentPart, formatProvider);

                    parent.AppendChild(wordTable);
                    return wordTable;
                }
            }

            return null;
        }
    }
}