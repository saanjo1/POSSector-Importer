﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportApp.Domain.Services
{
    public interface IExcelService
    {
        Task<string> OpenDialog();
        Task<List<string>> ListSheetsFromFile();
        Task<List<string>> ListColumnNames(string excelName);
    }
}
