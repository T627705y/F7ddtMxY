// 代码生成时间: 2025-09-21 07:03:07
using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ExcelGeneratorApp
{
    /// <summary>
    /// Class responsible for generating Excel files.
    /// </summary>
    public class ExcelGenerator
    {
        private const string ExcelExtension = ".xlsx";
        private const string SheetName = "Sheet1";

        /// <summary>
        /// Generates an Excel file with the specified data.
        /// </summary>
        /// <param name="data">List of data to populate the Excel file.</param>
        /// <param name="fileName">Name of the Excel file to generate.</param>
        public void GenerateExcelFile(List<List<string>> data, string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(ExcelExtension))
            {
                throw new ArgumentException("Invalid file name. File must end with .xlsx extension.");
            }

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                SheetData sheetData = new SheetData();
                Sheet sheet = new Sheet()
                {
                    Id = document.WorkbookPart.GetIdOfPart(workbookPart.WorksheetParts.CreateWorksheet(
                        new SheetProperties(), sheetData))
                };
                sheet.Name = SheetName;

                Workbook.Worksheets sheets = workbookPart.Workbook.AppendChild(new Workbook.Worksheets());
                sheets.Append(sheet);

                // Adding data to the sheet
                uint rowNumber = 1;
                foreach (var row in data)
                {
                    Row excelRow = new Row() { RowIndex = rowNumber++ };
                    foreach (var cellData in row)
                    {
                        Cell cell = new Cell() { CellValue = new CellValue(cellData), DataType = new EnumValue<CellValues>(CellValues.String) };
                        excelRow.Append(cell);
                    }
                    sheetData.Append(excelRow);
                }

                workbookPart.Workbook.Save();
            }
        }
    }
}
