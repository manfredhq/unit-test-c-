﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using OpenXMLSDK.Engine.interfaces.Word.ReportEngine.Models;
using OpenXMLSDK.Engine.ReportEngine.DataContext;
using OpenXMLSDK.Engine.ReportEngine.DataContext.Charts;
using OpenXMLSDK.Engine.ReportEngine.DataContext.FluentExtensions;
using OpenXMLSDK.Engine.Word;
using OpenXMLSDK.Engine.Word.ReportEngine;
using OpenXMLSDK.Engine.Word.ReportEngine.Models;
using OpenXMLSDK.Engine.Word.ReportEngine.Models.Charts;
using OpenXMLSDK.Engine.Word.ReportEngine.Models.ExtendedModels;
using OpenXMLSDK.Engine.Word.Tables;
using OpenXMLSDK.Engine.Word.Tables.Models;

namespace OpenXMLSDK.UnitTest.ReportEngine
{
    public static class ReportEngineTest
    {
        public static void ReportEngine(string filePath, string documentName)
        {
            // Debut test report engine
            using (var word = new WordManager())
            {
                JsonContextConverter[] converters = { new JsonContextConverter() };

                if (string.IsNullOrWhiteSpace(filePath))
                {
                    if (string.IsNullOrWhiteSpace(documentName))
                        documentName = "ExampleDocument.docx";

                    var template = GetTemplateDocument();
                    var templateJson = JsonConvert.SerializeObject(template);
                    var templateUnserialized = JsonConvert.DeserializeObject<Document>(templateJson, new JsonSerializerSettings() { Converters = converters });

                    var context = GetContext();
                    var contextJson = JsonConvert.SerializeObject(context);
                    var contextUnserialized = JsonConvert.DeserializeObject<ContextModel>(contextJson, new JsonSerializerSettings() { Converters = converters });

                    var res = word.GenerateReport(templateUnserialized, contextUnserialized, new CultureInfo("en-US"));

                    // test ecriture fichier
                    File.WriteAllBytes(documentName, res);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(documentName))
                        documentName = "ExampleDocument.docx";
                    if (!documentName.EndsWith(".docx"))
                        documentName = string.Concat(documentName, ".docx");

                    var stream = File.ReadAllText(filePath);
                    var report = JsonConvert.DeserializeObject<Report>(stream, new JsonSerializerSettings() { Converters = converters });

                    var res = word.GenerateReport(report.Document, report.ContextModel, new CultureInfo("en-US"));

                    // test ecriture fichier
                    File.WriteAllBytes(documentName, res);
                }
            }
        }

        public static void Test()
        {
            Console.WriteLine("Enter the path of your Json file, press enter for an example");
            var filePath = Console.ReadLine();
            var documentName = string.Empty;
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Enter document name");
                documentName = Console.ReadLine();
            }

            Console.WriteLine("Generation in progress");
            ReportEngine(filePath, documentName);
        }

        /// <summary>
        /// Generate the context for the generated template
        /// </summary>
        /// <returns></returns>
        private static ContextModel GetContext()
        {
            // Classic generation
            ContextModel row2 = new ContextModel();
            row2.AddItem("#Cell1#", new StringModel("Col 2 Row 1"));
            row2.AddItem("#Cell2#", new StringModel("Col 2 Row 2"));
            row2.AddItem("#Label#", new StringModel("Label 2"));
            ContextModel row3 = new ContextModel();
            row3.AddItem("#Cell1#", new StringModel("Col 1 Row 3"));
            row3.AddItem("#Cell2#", new StringModel("Col 2 Row 3"));
            row3.AddItem("#Label#", new StringModel("Label 1"));
            ContextModel row4 = new ContextModel();
            row4.AddItem("#Cell1#", new StringModel("Col 2 Row 4"));
            row4.AddItem("#Cell2#", new StringModel("Col 2 Row 4"));
            row4.AddItem("#Label#", new StringModel("Label 2"));

            // Fluent samples
            ContextModel row1 = new ContextModel()
                        .AddString("#Cell1#", "Col 1 Row 1")
                        .AddString("#Cell2#", "Col 2 Row 1")
                        .AddString("#Label#", "Label 1");

            ContextModel context = new ContextModel()
                        .AddBoolean("#NoRow#", false)
                        .AddString("#ParagraphShading#", "00FF00")
                        .AddString("#ParagraphBorderColor#", "105296")
                        .AddString("#BorderColor#", "00FF00")
                        .AddString("#KeyTest1#", "Key 1")
                        .AddString("#KeyTest2#", "Key 2")
                        .AddBoolean("#BoldKey#", true)
                        .AddString("#FontColorTestRed#", "993333")
                        .AddString("#ParagraphStyleIdTestYellow#", "Yellow")
                        .AddCollection("#Datasource#", row1, row2)
                        .AddCollection("#DatasourcePrefix#", row1, row2, row3, row4);

            ContextModel row11 = new ContextModel();
            row11.AddItem("#IsInGroup#", new BooleanModel(true));
            row11.AddItem("#IsNotFirstLineGroup#", new BooleanModel(false));
            row11.AddItem("#Cell1#", new StringModel("Col 1 Row 1"));
            row11.AddItem("#Cell2#", new StringModel("Col 2 Row 1"));
            row11.AddItem("#Label#", new StringModel("Label 1"));
            ContextModel row12 = new ContextModel();
            row12.AddItem("#IsInGroup#", new BooleanModel(true));
            row12.AddItem("#IsNotFirstLineGroup#", new BooleanModel(true));
            row12.AddItem("#Cell1#", new StringModel("Col 1 Row 1"));
            row12.AddItem("#Cell2#", new StringModel("Col 2 Row 1"));
            row12.AddItem("#Label#", new StringModel("Label 1"));
            ContextModel row13 = new ContextModel();
            row13.AddItem("#IsInGroup#", new BooleanModel(true));
            row13.AddItem("#IsNotFirstLineGroup#", new BooleanModel(true));
            row13.AddItem("#Cell1#", new StringModel("Col 1 Row 1"));
            row13.AddItem("#Cell2#", new StringModel("Col 2 Row 1"));
            row13.AddItem("#Label#", new StringModel("Label 1"));
            ContextModel row22 = new ContextModel();
            row22.AddItem("#IsInGroup#", new BooleanModel(true));
            row22.AddItem("#IsNotFirstLineGroup#", new BooleanModel(false));
            row22.AddItem("#Cell1#", new StringModel("Col 2 Row 1"));
            row22.AddItem("#Cell2#", new StringModel("Col 2 Row 2"));
            row22.AddItem("#Label#", new StringModel("Label 2"));
            ContextModel row23 = new ContextModel();
            row23.AddItem("#IsInGroup#", new BooleanModel(true));
            row23.AddItem("#IsNotFirstLineGroup#", new BooleanModel(true));
            row23.AddItem("#Cell1#", new StringModel("Col 2 Row 1"));
            row23.AddItem("#Cell2#", new StringModel("Col 2 Row 2"));
            row23.AddItem("#Label#", new StringModel("Label 2"));

            context.AddItem("#DatasourceTableFusion#", new DataSourceModel()
            {
                Items = new List<ContextModel>()
                    {
                        row11, row12, row13, row22, row23
                    }
            });

            List<ContextModel> cellsContext = new List<ContextModel>();
            for (int i = 0; i < DateTime.Now.Day; i++)
            {
                ContextModel uniformGridContext = new ContextModel();
                uniformGridContext.AddItem("#CellUniformGridTitle#", new StringModel("Item number " + (i + 1)));
                cellsContext.Add(uniformGridContext);
            }
            context.AddItem("#UniformGridSample#", new DataSourceModel()
            {
                Items = cellsContext
            });

            context.AddItem("#GrahSampleData#", new BarChartModel()
            {
                BarChartContent = new OpenXMLSDK.Engine.ReportEngine.DataContext.Charts.BarModel()
                {
                    Categories = new List<BarCategoryModel>()
                    {
                        new BarCategoryModel()
                        {
                            Name = "Category 1"
                        },
                        new BarCategoryModel()
                        {
                            Name = "Category 2"
                        },
                        new BarCategoryModel()
                        {
                            Name = "Category 3"
                        },
                        new BarCategoryModel()
                        {
                            Name = "Category 4"
                        },
                        new BarCategoryModel()
                        {
                            Name = "Category 5"
                        },
                        new BarCategoryModel()
                        {
                            Name = "Category 6"
                        }
                    },
                    Series = new List<BarSerieModel>()
                    {
                        new BarSerieModel()
                        {
                            Values = new List<double?>()
                            {
                                0, 1, 2, 3, 6, null
                            },
                            Name = "Bar serie 1"
                        },
                        new BarSerieModel()
                        {
                            Values = new List<double?>()
                            {
                                5, null, 7, 8, 0, 10
                            },
                            Name = "Bar serie 2"
                        },
                        new BarSerieModel()
                        {
                            Values = new List<double?>()
                            {
                                9, 10, 11, 12, 13, 14
                            },
                            Name = "Bar serie 3"
                        },
                        new BarSerieModel()
                        {
                            Values = new List<double?>()
                            {
                                9, 10, 11, 12, 15, 25
                            },
                            Name = "Bar serie 4"
                        }
                    }
                }
            });

            return context;
        }

        /// <summary>
        /// Generate the template
        /// </summary>
        /// <returns></returns>
        private static Document GetTemplateDocument()
        {
            var doc = new Document();
            doc.Styles.Add(new Style() { StyleId = "Red", FontColor = "FF0050", FontSize = "42" });
            doc.Styles.Add(new Style() { StyleId = "Yellow", FontColor = "FFFF00", FontSize = "40" });

            var page1 = new Page();
            page1.Margin = new SpacingModel() { Top = 845, Bottom = 1418, Left = 567, Right = 567, Header = 709, Footer = 709 };
            var page2 = new Page();
            page2.Margin = new SpacingModel() { Top = 1418, Left = 845, Header = 709, Footer = 709 };
            doc.Pages.Add(page1);
            doc.Pages.Add(page2);

            // Template 1 :

            var paragraph = new Paragraph();
            paragraph.ChildElements.Add(new Label() { Text = "Label wihtou special character (éèàù).", FontSize = "30", FontName = "Arial" });
            paragraph.ChildElements.Add(new Hyperlink()
            {
                Text = new Label()
                {
                    Text = "Go to github.",
                    FontSize = "20",
                    FontName = "Arial"
                },
                WebSiteUri = "https://www.github.com/"
            });
            paragraph.Indentation = new ParagraphIndentationModel()
            {
                Left = "300",
                Right = "6000"
            };
            paragraph.ChildElements.Add(new Label() { Text = "Ceci est un texte avec accents (éèàù)", FontSize = "30", FontName = "Arial" });
            paragraph.ChildElements.Add(new Label()
            {
                Text = "#KeyTest1#",
                FontSize = "40",
                TransformOperations = new List<LabelTransformOperation>()
                {
                    new LabelTransformOperation()
                    {
                        TransformOperationType = LabelTransformOperationType.ToUpper
                    }
                },
                FontColor = "#FontColorTestRed#",
                Shading = "9999FF",
                BoldKey = "#BoldKey#",
                Bold = false
            });
            paragraph.ChildElements.Add(new Label()
            {
                Text = "#KeyTest2#",
                Show = false
            });
            paragraph.Borders = new BorderModel()
            {
                BorderPositions = BorderPositions.BOTTOM | BorderPositions.TOP | BorderPositions.LEFT,
                BorderWidthBottom = 3,
                BorderWidthLeft = 10,
                BorderWidthTop = 20,
                BorderWidthInsideVertical = 1,
                UseVariableBorders = true,
                BorderColor = "FF0000",
                BorderLeftColor = "CCCCCC",
                BorderTopColor = "123456",
                BorderRightColor = "FFEEDD",
                BorderBottomColor = "FF1234"
            };

            var templateDefinition = new TemplateDefinition()
            {
                TemplateId = "Template 1",
                Note = "Sample paragraph",
                ChildElements = new List<BaseElement>() { paragraph }
            };
            doc.TemplateDefinitions.Add(templateDefinition);

            page1.ChildElements.Add(new TemplateModel() { TemplateId = "Template 1" });
            page1.ChildElements.Add(paragraph);
            page1.ChildElements.Add(new TemplateModel() { TemplateId = "Template 1" });
            page1.ChildElements.Add(new TemplateModel() { TemplateId = "Template 1" });

            var p2 = new Paragraph();
            p2.Shading = "#ParagraphShading#";
            p2.ChildElements.Add(new Label() { Text = "   texte paragraph2 avec espace avant", FontSize = "20", SpaceProcessingModeValue = SpaceProcessingModeValues.Preserve });
            p2.ChildElements.Add(new Label() { Text = "texte2 paragraph2 avec espace après   ", SpaceProcessingModeValue = SpaceProcessingModeValues.Preserve });
            p2.ChildElements.Add(new Label() { Text = "   texte3 paragraph2 avec espace avant et après   ", SpaceProcessingModeValue = SpaceProcessingModeValues.Preserve });
            page1.ChildElements.Add(p2);

            var table = new Table()
            {
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                TableIndentation = new TableIndentation() { Width = 1000 },
                Rows = new List<Row>()
                {
                    new Row()
                    {
                        Cells = new List<Cell>()
                        {
                            new Cell()
                            {
                                VerticalAlignment = TableVerticalAlignmentValues.Center,
                                Justification = JustificationValues.Center,
                                ChildElements = new List<BaseElement>()
                                {
                                    new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "Cell 1 - First paragraph" } }, ParagraphStyleId = "Yellow" },
                                    new Image()
                                    {
                                        Width = 50,
                                        Path = @"Resources\Desert.jpg",
                                        ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                                    },
                                    new Label() { Text = "Cell 1 - Label in a cell" },
                                    new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "Cell 1 - Second paragraph" } } }
                                },
                                Fusion = true
                            },
                            new Cell()
                            {
                                ChildElements = new List<BaseElement>()
                                {
                                    new Label() { Text = "Cell 2 - First label" },
                                    new Image()
                                    {
                                        Height = 10,
                                        Path = @"Resources\Desert.jpg",
                                        ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                                    },
                                    new Label() { Text = "Cell 2 - Second label" }
                                },
                                Borders = new BorderModel()
                                {
                                    BorderColor = "#BorderColor#",
                                    BorderWidth = 20,
                                    BorderPositions = BorderPositions.LEFT | BorderPositions.TOP
                                }
                            }
                        }
                    },
                    new Row()
                    {
                        ShowKey = "#NoRow#",
                        Cells = new List<Cell>()
                        {
                            new Cell()
                            {
                                Fusion = true,
                                FusionChild = true
                            },
                            new Cell()
                            {
                                VerticalAlignment = TableVerticalAlignmentValues.Bottom,
                                Justification = JustificationValues.Right,
                                ChildElements = new List<BaseElement>()
                                {
                                    new Label() { Text = "cellule4" }
                                }
                            }
                        }
                    }
                }
            };

            table.HeaderRow = new Row()
            {
                Cells = new List<Cell>()
                {
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "header1" } } }
                            }
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "header2" }
                            }
                        }
                }
            };

            table.Borders = new BorderModel()
            {
                BorderPositions = BorderPositions.BOTTOM | BorderPositions.INSIDEVERTICAL,
                BorderWidthBottom = 50,
                BorderWidthInsideVertical = 1,
                UseVariableBorders = true,
                BorderColor = "FF0000"
            };

            page1.ChildElements.Add(table);
            page1.ChildElements.Add(new Paragraph());

            if (File.Exists(@"Resources\Desert.jpg"))
                page1.ChildElements.Add(
                    new Paragraph()
                    {
                        ChildElements = new List<BaseElement>()
                        {
                        new Image()
                        {
                            MaxHeight = 100,
                            MaxWidth = 100,
                            Path = @"Resources\Desert.jpg",
                            ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                        }
                        }
                    }
                );

            var tableDataSource = new Table()
            {
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                ColsWidth = new int[2] { 750, 4250 },
                Borders = new BorderModel()
                {
                    BorderPositions = (BorderPositions)63,
                    BorderColor = "328864",
                    BorderWidth = 20,
                },
                RowModel = new Row()
                {
                    Cells = new List<Cell>()
                    {
                        new Cell()
                        {
                            Shading = "FFA0FF",
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "#Cell1#" }
                            }
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "#Cell2#" }
                            }
                        }
                    }
                },
                DataSourceKey = "#Datasource#"
            };

            var tableDataSourceWithPrefix = new Table()
            {
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                ColsWidth = new int[2] { 750, 4250 },
                Borders = new BorderModel()
                {
                    BorderPositions = (BorderPositions)63,
                    BorderColor = "328864",
                    BorderWidth = 20,
                },
                DataSourceKey = "#DatasourcePrefix#",
                AutoContextAddItemsPrefix = "DataSourcePrefix",
                RowModel = new Row()
                {
                    Cells = new List<Cell>()
    {
        new Cell()
        {
            Shading = "FFA0FF",
            ChildElements = new List<BaseElement>()
            {
                new Label() { Text = "Item Datasource (0 index) #DataSourcePrefix_TableRow_IndexBaseZero# - ",
                                ShowKey = "#DataSourcePrefix_TableRow_IsFirstItem#" },
                new Label() { Text = "#Cell1#" }
            }
        },
        new Cell()
        {
            ChildElements = new List<BaseElement>()
            {
                new Label() { Text = "Item Datasource (1 index) #DataSourcePrefix_TableRow_IndexBaseOne# - ",
                                ShowKey = "#DataSourcePrefix_TableRow_IsLastItem#" },
                new Label() { Text = "#Cell2#" }
            }
        }
    }
                }
            };

            page1.ChildElements.Add(tableDataSource);

            page1.ChildElements.Add(tableDataSourceWithPrefix);

            // page 2
            var p21 = new Paragraph();
            p21.Justification = JustificationValues.Center;
            p21.ParagraphStyleId = "Red";
            p21.ChildElements.Add(new Label() { Text = "texte page2", FontName = "Arial" });
            page2.ChildElements.Add(p21);

            var p22 = new Paragraph();
            p22.SpacingBefore = 800;
            p22.SpacingAfter = 800;
            p22.Justification = JustificationValues.Both;
            p22.ParagraphStyleId = "Yellow";
            p22.ChildElements.Add(new Label() { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse urna augue, convallis eu enim vitae, maximus ultrices nulla. Sed egestas volutpat luctus. Maecenas sodales erat eu elit auctor, eu mattis neque maximus. Duis ac risus quis sem bibendum efficitur. Vivamus justo augue, molestie quis orci non, maximus imperdiet justo. Donec condimentum rhoncus est, ut varius lorem efficitur sed. Donec accumsan sit amet nisl vel ornare. Duis aliquet urna eu mauris porttitor facilisis. " });
            page2.ChildElements.Add(p22);

            var p23 = new Paragraph();
            p23.Borders = new BorderModel()
            {
                BorderPositions = (BorderPositions)13,
                BorderWidth = 20,
                BorderColor = "#ParagraphBorderColor#"
            };
            p23.SpacingBetweenLines = 360;
            p23.ChildElements.Add(new Label() { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse urna augue, convallis eu enim vitae, maximus ultrices nulla. Sed egestas volutpat luctus. Maecenas sodales erat eu elit auctor, eu mattis neque maximus. Duis ac risus quis sem bibendum efficitur. Vivamus justo augue, molestie quis orci non, maximus imperdiet justo. Donec condimentum rhoncus est, ut varius lorem efficitur sed. Donec accumsan sit amet nisl vel ornare. Duis aliquet urna eu mauris porttitor facilisis. " });
            page2.ChildElements.Add(p23);

            // Adding a foreach page :
            var foreachPage = new ForEachPage();
            foreachPage.DataSourceKey = "#DatasourceTableFusion#";

            foreachPage.Margin = new SpacingModel() { Top = 1418, Left = 845, Header = 709, Footer = 709 };
            var paragraph21 = new Paragraph();
            paragraph21.ChildElements.Add(new Label() { Text = "Page label : #Label#" });
            foreachPage.ChildElements.Add(paragraph21);
            var p223 = new Paragraph();
            p223.Shading = "#ParagraphShading#";
            p223.ChildElements.Add(new Label() { Text = "Texte paragraph2 avec espace avant", FontSize = "20", SpaceProcessingModeValue = SpaceProcessingModeValues.Preserve });
            foreachPage.ChildElements.Add(p223);
            doc.Pages.Add(foreachPage);

            // page 3
            var page3 = new Page();
            var p31 = new Paragraph() { FontColor = "FF0000", FontSize = "26" };
            p31.ChildElements.Add(new Label() { Text = "Test the HeritFromParent" });
            var p311 = new Paragraph() { FontSize = "16" };
            p311.ChildElements.Add(new Label() { Text = " Success (not the same size)" });
            p31.ChildElements.Add(p311);
            page3.ChildElements.Add(p31);

            TableOfContents tableOfContents = new TableOfContents()
            {
                StylesAndLevels = new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("Red", "1"),
                    new Tuple<string, string>("Yellow", "2"),
                },
                Title = "Tessssssst !",
                TitleStyleId = "Yellow",
                ToCStylesId = new List<string>() { "Red" },
                LeaderCharValue = TabStopLeaderCharValues.underscore
            };
            page3.ChildElements.Add(tableOfContents);

            paragraph = new Paragraph()
            {
                ParagraphStyleId = "#ParagraphStyleIdTestYellow#"
            };
            paragraph.ChildElements.Add(new Label() { Text = "Ceci est un test de paragraph avec Style", FontSize = "30", FontName = "Arial" });
            page3.ChildElements.Add(paragraph);

            doc.Pages.Add(page3);

            var page4 = new Page();
            //New page to manage UniformGrid:
            var uniformGrid = new UniformGrid()
            {
                DataSourceKey = "#UniformGridSample#",
                ColsWidth = new int[2] { 2500, 2500 },
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                CellModel = new Cell()
                {
                    VerticalAlignment = TableVerticalAlignmentValues.Center,
                    Justification = JustificationValues.Center,
                    ChildElements = new List<BaseElement>()
                        {
                            new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "#CellUniformGridTitle#" } } },
                            new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "Cell 1 - Second paragraph" } } }
                        }
                },
                HeaderRow = new Row()
                {
                    Cells = new List<Cell>()
                    {
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "header1" } } }
                            }
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "header2" }
                            }
                        }
                    }
                },
                Borders = new BorderModel()
                {
                    BorderPositions = BorderPositions.BOTTOM | BorderPositions.INSIDEVERTICAL,
                    BorderWidthBottom = 50,
                    BorderWidthInsideVertical = 1,
                    UseVariableBorders = true,
                    BorderColor = "FF0000"
                }
            };

            page4.ChildElements.Add(uniformGrid);

            doc.Pages.Add(page4);

            var page5 = new Page();
            var tableDataSourceWithBeforeAfter = new Table()
            {
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                ColsWidth = new int[2] { 750, 4250 },
                Borders = new BorderModel()
                {
                    BorderPositions = (BorderPositions)63,
                    BorderColor = "328864",
                    BorderWidth = 20,
                },
                BeforeRows = new List<Row>()
                {
                    new Row()
                    {
                        Cells = new List<Cell>()
                        {
                            new Cell()
                            {
                                VerticalAlignment = TableVerticalAlignmentValues.Bottom,
                                Justification = JustificationValues.Left,
                                ChildElements = new List<BaseElement>()
                                {
                                    new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "Cell 1 - A small paragraph" } }, ParagraphStyleId = "Yellow" },
                                    new Image()
                                    {
                                        MaxHeight = 100,
                                        MaxWidth = 100,
                                        Path = @"Resources\Desert.jpg",
                                        ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                                    },
                                    new Label() { Text = "Custom header" },
                                    new Paragraph() { ChildElements = new List<BaseElement>() { new Label() { Text = "Cell 1 - an other paragraph" } } }
                                },
                                Fusion = true
                            },
                            new Cell()
                            {
                                ChildElements = new List<BaseElement>()
                                {
                                    new Label() { Text = "Cell 2 - an other label" },
                                    new Image()
                                    {
                                        MaxHeight = 100,
                                        MaxWidth = 100,
                                        Path = @"Resources\Desert.jpg",
                                        ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                                    },
                                    new Label() { Text = "Cell 2 - an other other label" }
                                },
                                Borders = new BorderModel()
                                {
                                    BorderColor = "00FF22",
                                    BorderWidth = 15,
                                    BorderPositions = BorderPositions.RIGHT | BorderPositions.TOP
                                }
                            }
                        }
                    },
                    new Row()
                    {
                        Cells = new List<Cell>()
                        {
                            new Cell()
                            {
                                Fusion = true,
                                FusionChild = true
                            },
                            new Cell()
                            {
                                VerticalAlignment = TableVerticalAlignmentValues.Bottom,
                                Justification = JustificationValues.Right,
                                ChildElements = new List<BaseElement>()
                                {
                                    new Label() { Text = "celluleX" }
                                }
                            }
                        }
                    }
                },
                RowModel = new Row()
                {
                    Cells = new List<Cell>()
                    {
                        new Cell()
                        {
                            Shading = "FFA2FF",
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "Cell : #Cell1#" }
                            }
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "Cell : #Cell2#" }
                            }
                        }
                    }
                },
                DataSourceKey = "#Datasource#"
            };

            page5.ChildElements.Add(tableDataSourceWithBeforeAfter);

            doc.Pages.Add(page5);

            var page6 = new Page();

            var pr = new Paragraph()
            {
                ChildElements = new List<BaseElement>() {
                    new OpenXMLSDK.Engine.Word.ReportEngine.Models.Charts.BarModel()
                    {
                        Title = "Graph test",
                        ShowTitle = true,
                        FontSize = "23",
                        ShowBarBorder = true,
                        BarChartType = BarChartType.BarChart,
                        BarDirectionValues = BarDirectionValues.Column,
                        BarGroupingValues = BarGroupingValues.PercentStacked,
                        DataSourceKey = "#GrahSampleData#",
                        ShowMajorGridlines = true,
                        HasBorder = true,
                        BorderColor = "#A80890"
                    }
                }
            };

            page6.ChildElements.Add(pr);

            doc.Pages.Add(page6);

            var page7 = new Page();

            var tableDataSourceWithCellFusion = new Table()
            {
                TableWidth = new TableWidthModel() { Width = "5000", Type = TableWidthUnitValues.Pct },
                ColsWidth = new int[3] { 1200, 1200, 1200 },
                Borders = new BorderModel()
                {
                    BorderPositions = (BorderPositions)63,
                    BorderColor = "328864",
                    BorderWidth = 20,
                },
                RowModel = new Row()
                {
                    Cells = new List<Cell>()
                    {
                        new Cell()
                        {
                            Shading = "FFA0FF",
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "#Cell1#" }
                            },
                            FusionKey = "#IsInGroup#",
                            FusionChildKey = "#IsNotFirstLineGroup#"
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "#Cell2#" }
                            }
                        },
                        new Cell()
                        {
                            ChildElements = new List<BaseElement>()
                            {
                                new Label() { Text = "#Cell2#" }
                            }
                        }
                    }
                },
                DataSourceKey = "#DatasourceTableFusion#"
            };

            page7.ChildElements.Add(tableDataSourceWithCellFusion);

            doc.Pages.Add(page7);

            // page 8
            var page8 = new Page();
            var p8 = new Paragraph() { FontColor = "FF0000", FontSize = "26" };
            p8.ChildElements.Add(new Label() { Text = "Label with" + Environment.NewLine + Environment.NewLine + "A new line" });
            page8.ChildElements.Add(p8);

            doc.Pages.Add(page8);

            // Header
            var header = new Header();
            header.Type = HeaderFooterValues.Default;
            var ph = new Paragraph();
            ph.ChildElements.Add(new Label() { Text = "Header Text" });
            if (File.Exists(@"Resources\Desert.jpg"))
                ph.ChildElements.Add(new Image()
                {
                    MaxHeight = 100,
                    MaxWidth = 100,
                    Path = @"Resources\Desert.jpg",
                    ImagePartType = OpenXMLSDK.Engine.Packaging.ImagePartType.Jpeg
                });
            header.ChildElements.Add(ph);
            doc.Headers.Add(header);

            // first header
            var firstHeader = new Header();
            firstHeader.Type = HeaderFooterValues.First;
            var fph = new Paragraph();
            fph.ChildElements.Add(new Label() { Text = "first header Text" });
            firstHeader.ChildElements.Add(fph);
            doc.Headers.Add(firstHeader);

            // Footer
            var footer = new Footer();
            footer.Type = HeaderFooterValues.Default;
            var pf = new Paragraph();
            pf.ChildElements.Add(new Label() { Text = "Footer Text" });
            pf.ChildElements.Add(new Label() { IsPageNumber = true });
            footer.ChildElements.Add(pf);
            doc.Footers.Add(footer);

            return doc;
        }
    }
}